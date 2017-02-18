using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Alexa.Office365.Repositories
{
    public interface IReadRepositoryAsync<T>
    {
        Task<T> Read(T entity);
    }
}
