using LanguageCreator.Model;

namespace LanguageCreator
{
    public interface ILanguageCreator
    {
        LanguageCreatorResponse Execute(LanguageCreatorRequest request);
    }
}