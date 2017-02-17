using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATT.Alexa.Office365.Repositories
{
    public interface ICreateRepositoryAsync<T>
    {
        Task<T> Create(T entity);
    }
}
