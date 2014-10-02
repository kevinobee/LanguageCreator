using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CommandLine;

namespace LanguageImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                var keysDoc = XDocument.Load(options.File);

                int originalPhrases = keysDoc.Descendants("phrase").Count();

                var phrases = keysDoc.Descendants("phrase").Where(x => !x.Descendants(options.Language).Any()).ToArray();

                phrases.Where(x => ! RelatesToProject(x, options.ProjectIdentifier)).Remove();
                var projectPhrases = phrases.Where(x => RelatesToProject(x, options.ProjectIdentifier)).ToArray();

                foreach (var phrase in projectPhrases)
                {
                    var newLanguageChild = new XElement(options.Language)
                    {
                        Value = string.Format("{0}{1}{0}", options.Marker, phrase.Descendants("en").First().Value)
                    };

                    phrase.Add(newLanguageChild);

                    phrase.Descendants("en").Remove();
                }

                var outputFile = CreateOutputFileName(options.File, options.Language);
                keysDoc.Save(outputFile);

                Console.WriteLine();
                Console.WriteLine("Processed {0} phrases, {1} translations written to {2}", originalPhrases, projectPhrases.Count(), outputFile);
            }
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

        private static string CreateOutputFileName(string filePath, string language)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            return Path.Combine(Path.GetDirectoryName(filePath), string.Format("{0}-{1}.xml", fileName, language));
        }
    }
}
