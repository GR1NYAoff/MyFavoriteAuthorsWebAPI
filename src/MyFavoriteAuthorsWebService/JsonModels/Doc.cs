using System.Text.Json.Serialization;

namespace MyFavoriteAuthorsWebService.JsonModels
{
    public class Doc
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("alternate_names")]
        public List<string>? AlternateNames { get; set; }
        [JsonPropertyName("birth_date")]
        public string? BirthDate { get; set; }
        [JsonPropertyName("top_work")]
        public string? TopWork { get; set; }
        [JsonPropertyName("work_count")]
        public int WorkCount { get; set; }
        [JsonPropertyName("top_subjects")]
        public List<string>? TopSubjects { get; set; }
        [JsonPropertyName("_version_")]
        public object? Version { get; set; }
    }
}
