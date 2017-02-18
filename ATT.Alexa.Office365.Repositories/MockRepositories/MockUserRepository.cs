using ATT.Alexa.Office365.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Alexa.Office365.Repositories
{
    public class MockUserRepository : IReadRepositoryAsync<Models.User>, ICreateRepositoryAsync<Models.User>
    {
        private Models.User user = null;

        public MockUserRepository()
        {
            this.user = new Models.User();
            user.Id = ConfigurationManager.AppSettings["alexa:MockUserId"];
            user.CompanyId = ConfigurationManager.AppSettings["alexa:MockCompanyId"];
            user.UserName = ConfigurationManager.AppSettings["alexa:MockUserName"];
        }

        public async Task<Models.User> Create(Models.User user)
        {
            return await Task.Run(() => this.user);
        }

        public async Task<Models.User> Read(Models.User user)
        {
            return await Task.Run(() => this.user);
        }

        public async Task<bool> Update(Models.User user)
        {
            return await Task.Run(() => true);
        }
    }
}
