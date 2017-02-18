using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Alexa.Office365.Models
{
    public class Event
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Location { get; set; }
    }
}
