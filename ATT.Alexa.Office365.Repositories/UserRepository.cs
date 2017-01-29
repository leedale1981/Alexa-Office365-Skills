using ATT.Alexa.Office365.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Alexa.Office365.Repositories
{
    public class UserRepository : IReadRepository<User>, ICreateRepository<User>
    {
        public User Create(User user)
        {
            throw new NotImplementedException();
        }

        public User Read(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> ReadAll()
        {
            throw new NotImplementedException();
        }
    }
}
