﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATT.Alexa.Office365.Models
{
    public class User : Document
    {
        public string AccessToken { get; set; }
        public string UserName { get; set; }
        public string CompanyId { get; set; }
    }
}