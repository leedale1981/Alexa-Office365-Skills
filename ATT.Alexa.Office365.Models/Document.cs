using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Alexa.Office365.Models
{
    public class Document
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
