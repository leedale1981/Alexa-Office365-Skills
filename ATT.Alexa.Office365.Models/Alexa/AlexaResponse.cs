using Newtonsoft.Json;

namespace ATT.Alexa.Office365.Models
{
    public class AlexaResponse
    {
        [JsonProperty(PropertyName = "outputSpeech")]
        public AlexaOutputSpeech OutputSpeech { get; set; }

        [JsonProperty(PropertyName = "card")]
        public AlexaCard Card { get; set; }
    }
}