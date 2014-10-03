using LanguageCreator.Infrastructure;

namespace LanguageCreator
{
    internal class ApplicationContainer
    {
        public IXmlLanguageParser ResolveXmlLanguageParser()
        {
            ILanguageCreator languageCreator = new Infrastructure.LanguageCreator();
            IOutputWriter outputWriter = new ConsoleOutputWriter();

            return new XmlLanguageParser(languageCreator, outputWriter);
        }
    }
}