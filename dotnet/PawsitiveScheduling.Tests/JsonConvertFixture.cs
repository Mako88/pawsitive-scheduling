using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;

namespace PawsitiveScheduling.Tests
{
    public class JsonConvertFixture
    {
        public JsonConvertFixture()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
        }
    }

    [CollectionDefinition("json")]
    public class JsonConvertCollection : ICollectionFixture<JsonConvertFixture>
    {

    }
}
