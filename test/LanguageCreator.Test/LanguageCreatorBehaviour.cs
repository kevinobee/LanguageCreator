using System.Linq;
using System.Xml.Linq;
using LanguageCreator.Model;
using Should;
using Xunit;

namespace LanguageCreator.Test
{
    public class LanguageCreatorBehaviour
    {
        private readonly LanguageCreatorResponse _results;

        public LanguageCreatorBehaviour()
        {
            ILanguageCreator sut = new Infrastructure.LanguageCreator();

            _results = sut.Execute(new LanguageCreatorRequest
            {
                Document = CreateTestDocument(),
                Culture = new System.Globalization.CultureInfo("mn-MN"),
                Marker = "*",
                SearchTerm = "ExperienceAnalytics"
            });
        }

        [Fact]
        public void should_return_results()
        {
            _results.ShouldNotBeNull();
        }

        [Fact]
        public void results_should_contain_processed_document()
        {
            _results.Document.ShouldNotBeNull();
        }

        [Fact]
        public void results_should_contain_statistics_on_number_of_phrases_in_source_document()
        {
            _results.Statistics.SourcePhrasesCount.ShouldEqual(6);
        }

        [Fact]
        public void results_should_contain_statistics_on_number_of_phrases_in_output_document()
        {
            _results.Statistics.OutputPhrasesCount.ShouldEqual(2);
        }

        [Fact]
        public void results_document_should_contain_2_phrases_in_output_document()
        {
            _results.Document.Descendants("phrase").Count().ShouldEqual(2);
        }

        [Fact]
        public void results_document_should_contain_only_mongolian_phrases_in_output_document()
        {
            _results.Document.Descendants("phrase").Descendants("en").Any().ShouldBeFalse();
        }

        private static XDocument CreateTestDocument()
        {
            const string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<sitecore>
  <phrase key="" (locked by {0})"" source=""Sitecore.Texts"">
    <en> (locked by {0})</en>
  </phrase>
  <phrase key=""#{5A766BA8-F10B-4B8D-929A-334358487802}#{B5E02AD9-D56F-4C41-A065-A133DB87BDEB}"" itemid=""{5A766BA8-F10B-4B8D-929A-334358487802}"" fieldid=""{B5E02AD9-D56F-4C41-A065-A133DB87BDEB}"" path=""/sitecore/client/Applications/ExperienceAnalytics/Common/System/Navigation/Interactions/Interactions"" fieldname=""__Display name"" template=""HyperlinkButton Parameters"">
    <en>Interactions</en>
  </phrase>
  <phrase key=""#{5A789979-4582-4D04-A4C8-2000D084E8B1}#{4DF6DA21-E745-444C-956E-1D4A96AB8821}"" itemid=""{5A789979-4582-4D04-A4C8-2000D084E8B1}"" fieldid=""{4DF6DA21-E745-444C-956E-1D4A96AB8821}"" path=""/sitecore/content/Applications/Content Editor/Ribbons/Chunks/Layout/Details"" fieldname=""Header"" template=""Large Button"">
    <en>Details</en>
  </phrase>
  <phrase key=""#Error: "" source=""Sitecore.Texts"">
    <en>#Error: </en>
  </phrase>
  <phrase key=""#Exception: "" source=""Sitecore.Texts"">
    <en>#Exception: </en>
  </phrase>
  <phrase key=""[unknown country]"" source=""Sitecore.ExperienceAnalytics.Texts+Aggregation+Dimensions+Geo+Country"">
    <en>[unknown country]</en>
  </phrase>
</sitecore>";

            return XDocument.Parse(xml);
        }
    }
}