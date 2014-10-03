namespace LanguageCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new ApplicationContainer();
            container.ResolveXmlLanguageParser()
                     .Execute(args);
        }
    }
}
