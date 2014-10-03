using System.Xml.Linq;

namespace LanguageCreator.Model
{
    public class LanguageCreatorResponse
    {
        public XDocument Document { get; set; }
        public Statistics Statistics { get; set; }
    }
}