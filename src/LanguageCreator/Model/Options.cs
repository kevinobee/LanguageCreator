using System.Globalization;
using System.Xml.Linq;
using CommandLine;
using CommandLine.Text;

namespace LanguageCreator.Model
{
    public class Options
    {
        [Option('f', "file", Required = true, HelpText = "Path to keys.xml file to be processed")]
        public string File { get; set; }

        [Option('l', "language", Required = false, DefaultValue = "mn-MN", HelpText = "Language to add")]
        public string Language { get; set; }

        [Option('m', "marker", Required = false, DefaultValue = "*", HelpText = "Marker character to add around translation text")]
        public string Marker { get; set; }

        [Option('p', "project", Required = true, HelpText = "Project specific source or path to filter by")]
        public string ProjectIdentifier { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }

    public class LanguageCreatorRequest
    {
        public XDocument Document { get; set; }
        public CultureInfo Culture { get; set; }
        public string Marker { get; set; }
        public string SearchTerm { get; set; }
    }
}