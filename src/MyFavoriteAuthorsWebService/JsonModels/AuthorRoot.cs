using System.Text.Json.Serialization;

namespace MyFavoriteAuthorsWebService.JsonModels
{
    public class RootAuthor
    {
        [JsonPropertyName("numFound")]
        public int NumFound { get; set; }
        [JsonPropertyName("start")]
        public int Start { get; set; }
        [JsonPropertyName("numFoundExact")]
        public bool NumFoundExact { get; set; }
        [JsonPropertyName("docs")]
        public List<Doc>? Docs { get; set; }
    }

}
