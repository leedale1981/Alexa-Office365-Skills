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
    public class MockCalendarRepository : IReadRepositoryAsync<Models.Event>
    {
        private Models.User user = null;
        private IReadRepositoryAsync<Models.User> userRepository;

        public MockCalendarRepository(Models.User user)
        {
            this.user = user;
        }

        public async Task<Models.Event> Read(Models.Event entity)
        {
            return (await this.ReadEventsForUser(null)).FirstOrDefault();   
        }

        public async Task<IEnumerable<Models.Event>> ReadEventsForUser(Models.User user)
        {
            List<Models.Event> myEvents = new List<Models.Event>();

            await Task.Run(() =>
            {
                myEvents.Add(new Models.Event()
                {
                    Date = DateTime.Now,
                    Location = "25 Test Road, London, SE16 4PG",
                    Subject = "Meeting with Mr Robinson about finances"
                });

                myEvents.Add(new Models.Event()
                {
                    Date = DateTime.Now.AddDays(1),
                    Location = "105 Testing Street, Berkshire, B28 4EG",
                    Subject = "Talk about presentation with Abigail"
                });
            });

            return myEvents;
        }
    }
}
