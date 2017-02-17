using ATT.Alexa.Office365.Identity;
using ATT.Alexa.Office365.Models;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Alexa.Office365.Repositories
{
    public class CompanyRepository : IReadRepositoryAsync<Models.Company>, ICreateRepositoryAsync<Company>
    {
        private Database<Models.Company> database = null;

        public CompanyRepository(Database<Models.Company> database)
        {
            this.database = database;
        }

        public async Task<Models.Company> Create(Models.Company company)
        {
            return await this.database.CreateDocument(company);
        }

        public async Task<Models.Company> Read(Models.Company company)
        {
            return await this.database.GetDocument(company);
        }

        public async Task<IEnumerable<Models.Company>> ReadAll()
        {
            return await this.database.GetAllDocuments();
        }
    }
}
