using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Alexa.Office365.Models
{
    public class AlexaJson
    {
        [JsonProperty(PropertyName ="version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "response")]
        public AlexaResponse Response { get; set; }

        public AlexaJson()
        {
            this.Version = "1.0";
        }
    }
}
