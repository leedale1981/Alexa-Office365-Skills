using ATT.Alexa.Office365.Identity;
using ATT.Alexa.Office365.Models;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Alexa.Office365.Repositories
{
    public class CalendarRepository : IReadRepositoryAsync<Models.Event>
    {
        private Models.User user = null;
        private IReadRepositoryAsync<Models.User> userRepository;

        public CalendarRepository(Models.User user)
        {
            this.user = user;
        }

        public async Task<Models.Event> Read(Models.Event entity)
        {
            return this.ReadEventsForUser(this.user).Result
                .Where(e => e.Id == entity.Id).FirstOrDefault();    
        }

        public async Task<IEnumerable<Models.Event>> ReadEventsForUser(Models.User user)
        {
            UserAuthentication userAuth = new UserAuthentication(user);
            GraphServiceClient client = userAuth.GetAuthenticatedClient();
            ICalendarEventsCollectionPage userEvents = null;

            try
            {
                userEvents = await client.Me.Calendar.Events.Request().GetAsync();
            }
            catch (Exception exception)
            {
                try
                {
                    string newAccessToken = await userAuth.GetUserAccessTokenAsync();

                    userEvents = await client.Me.Calendar.Events.Request().GetAsync();
                    user.AccessToken = newAccessToken;
                    await ((UserRepository)this.userRepository).Update(user);
                }
                catch (Exception newException)
                {
                    throw;
                }
            }

            List<Models.Event> myEvents = new List<Models.Event>();

            foreach (Microsoft.Graph.Event graphEvent in userEvents)
            {
                myEvents.Add(new Models.Event()
                {
                    Date = DateTime.ParseExact(graphEvent.Start.DateTime, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                    Location = graphEvent.Location.DisplayName,
                    Subject = graphEvent.Subject
                });
            }

            return myEvents;
        }
    }
}
