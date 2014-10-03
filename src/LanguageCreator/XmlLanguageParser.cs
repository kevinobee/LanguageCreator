using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using CommandLine;
using LanguageCreator.Model;

namespace LanguageCreator
{
    public class XmlLanguageParser : IXmlLanguageParser
    {
        private readonly ILanguageCreator _languageCreator;
        private readonly IOutputWriter _outputWriter;

        public XmlLanguageParser(ILanguageCreator languageCreator, IOutputWriter outputWriter)
        {
            _languageCreator = languageCreator;
            _outputWriter = outputWriter;
        }

        public void Execute(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                var request = new LanguageCreatorRequest
                {
                    Document = XDocument.Load(options.File),
                    Culture =
                        CultureInfo.GetCultures(CultureTypes.AllCultures).First(c => c.Name.EndsWith(options.Language)),
                    Marker = options.Marker,
                    SearchTerm = options.ProjectIdentifier
                };
                
                var results = _languageCreator.Execute(request);

                var outputFile = CreateOutputFileName(options.File, options.Language);
                results.Document.Save(outputFile);

                _outputWriter.WriteLine();
                _outputWriter.WriteFormatLine(
                    MessageType.Success, 
                    "Processed {0} phrases, {1} translations written to {2}", 
                    results.Statistics.SourcePhrasesCount, results.Statistics.OutputPhrasesCount, outputFile);
            }
        }

        private static string CreateOutputFileName(string filePath, string language)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            return Path.Combine(Path.GetDirectoryName(filePath), string.Format("{0}-{1}.xml", fileName, language));
        }
    }
}