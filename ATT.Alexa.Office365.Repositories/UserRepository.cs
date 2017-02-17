using ATT.Alexa.Office365.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Alexa.Office365.Repositories
{
    public class UserRepository : IReadRepositoryAsync<Models.User>, ICreateRepositoryAsync<Models.User>
    {
        private Database<Models.User> database = null;

        public UserRepository(Database<Models.User> database)
        {
            this.database = database;
        }

        public async Task<Models.User> Create(Models.User user)
        {
            return await this.database.CreateDocument(user);
        }

        public async Task<Models.User> Read(Models.User user)
        {
            return await this.database.GetDocument(user);
        }

        public async Task<bool> Update(Models.User user)
        {
            return await this.database.UpdateDocument(user);
        }
    }
}
