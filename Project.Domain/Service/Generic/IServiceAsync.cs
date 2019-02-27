using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Domain.Service
{
    public interface IServiceAsync<Tv, Te>
    {
        Task<IEnumerable<Tv>> GetAll();
        Task<int> Add(Tv obj);
        Task<bool> Update(Tv obj);
        Task<bool> Remove(int id);
        Task<Tv> GetOne(int id);
        Task<IEnumerable<Tv>> Get(Func<Te, bool> predicate);
    }

}
