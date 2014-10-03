using System.Linq;
using System.Xml.Linq;
using LanguageCreator.Model;

namespace LanguageCreator.Infrastructure
{
    public class LanguageCreator : ILanguageCreator
    {
        public LanguageCreatorResponse Execute(LanguageCreatorRequest request)
        {
            const string phraseElement = "phrase";

            var originalPhrases = request.Document.Descendants(phraseElement).Count();

            var phrases = request.Document.Descendants(phraseElement)
                                          .Where(x => !x.Descendants(request.Culture.Name)
                                          .Any())
                                          .ToArray();

            phrases.Where(x => !RelatesToProject(x, request.SearchTerm)).Remove();
            var projectPhrases = phrases.Where(x => RelatesToProject(x, request.SearchTerm)).ToArray();
            
            foreach (var phrase in projectPhrases)
            {
                var newLanguageChild = new XElement(request.Culture.Name)
                {
                    Value = string.Format("{0}{1}{0}", request.Marker, phrase.Descendants("en").First().Value)
                };
            
                phrase.Add(newLanguageChild);
            
                phrase.Descendants("en").Remove();
            }

            return new LanguageCreatorResponse
            {
                Document = request.Document, 
                Statistics = new Statistics
                {
                    SourcePhrasesCount = originalPhrases, 
                    OutputPhrasesCount = projectPhrases.Count()
                }
            };
        }

        private static bool RelatesToProject(XElement phraseElement, string projectIdentifier)
        {
            return IsProjectIdentifierPresentInAttribute(phraseElement, "source", projectIdentifier) ||
                   IsProjectIdentifierPresentInAttribute(phraseElement, "path", projectIdentifier);
        }

        private static bool IsProjectIdentifierPresentInAttribute(XElement phraseElement, string attributeName, string projectIdentifier)
        {
            var sourceAttr = phraseElement.Attributes(attributeName).FirstOrDefault();

            if (sourceAttr != null)
            {
                if (sourceAttr.Value.Contains(projectIdentifier))
                {
                    return true;
                }
            }
            return false;
        }
    }
}