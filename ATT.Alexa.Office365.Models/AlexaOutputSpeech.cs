using Newtonsoft.Json;

namespace ATT.Alexa.Office365.Models
{
    public class AlexaOutputSpeech
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}