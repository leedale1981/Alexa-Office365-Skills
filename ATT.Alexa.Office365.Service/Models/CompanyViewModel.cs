using ATT.Alexa.Office365.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATT.Alexa.Office365.Service.Models
{
    public class CompanyViewModel
    {
        [Display(Name = "Company Name")]
        public string SelectedCompanyId { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }
    }
}