using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Alexa.Office365.Models
{
    public class Company : Document
    {
        public string CompanyName { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}
