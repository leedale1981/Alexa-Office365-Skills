using ATT.Alexa.Office365.Identity;
using ATT.Alexa.Office365.Models;
using ATT.Alexa.Office365.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace ATT.Alexa.Office365.Service.Controllers
{
    public class CalendarController : ApiController
    {
        private IReadRepositoryAsync<User> readUserRepository = null;
        private IReadRepositoryAsync<Event> readCalendarRepository = null;
        private IReadRepositoryAsync<Company> readCompanyRepository = null;

        public CalendarController(
            IReadRepositoryAsync<User> readUserRepository,
            IReadRepositoryAsync<Event> readCalendarRepository,
            IReadRepositoryAsync<Company> readCompanyRepository)
        {
            this.readUserRepository = readUserRepository;
            this.readCalendarRepository = readCalendarRepository;
            this.readCompanyRepository = readCompanyRepository;
        }

        public async Task<AlexaJson> Post([FromBody] dynamic value)
        {
            string appId = value.session.application.applicationId;
            string accessToken = value.session.user.accessToken;
            List<Event> events = null;

            if (!string.IsNullOrEmpty(accessToken))
            {
                User requestUser = new User() { Id = accessToken };
                User user = await this.readUserRepository.Read(requestUser);
                if (await this.SetCachedCompanyValues(user))
                {
                    var eventRequest = await ((CalendarRepository)readCalendarRepository).ReadEventsForUser(user);
                    events = eventRequest.ToList();
                }
            }

            return this.GetCalendarAlexaResponse(events);
        }

        private async Task<bool> SetCachedCompanyValues(User user)
        {
            try
            {
                Company company = new Company() { Id = user.CompanyId };
                company = await this.readCompanyRepository.Read(company);

                HttpContext.Current.Cache["companyAppId"] = company.AppId;
                HttpContext.Current.Cache["companySecret"] = company.AppSecret;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private AlexaJson GetCalendarAlexaResponse(List<Event> events)
        {
            string cardContent = string.Empty;
            string speechText = "You have the following events in your calendar.";

            foreach (Event calEvent in events)
            {
                cardContent += "Date: " + calEvent.Date + "\n";
                cardContent += "Event: " + calEvent.Subject + "\n";
                cardContent += "Location: " + calEvent.Location + "\r\n";

                speechText += "Date " + calEvent.Date.DayOfWeek + " " + calEvent.Date.Day + " " + calEvent.Date.Month;
                speechText += ". About " + calEvent.Subject;
                speechText += ". At " + calEvent.Location;
            }

            AlexaCard card = new AlexaCard();
            card.Type = "Simple";
            card.Title = "Here are your events for today:";
            card.Content = cardContent;

            AlexaOutputSpeech speech = new AlexaOutputSpeech();
            speech.Type = "PlainText";
            speech.Text = speechText;

            AlexaResponse response = new AlexaResponse();
            response.Card = card;
            response.OutputSpeech = speech;

            AlexaJson json = new AlexaJson();
            json.Response = response;

            return json;
        }
    }
}