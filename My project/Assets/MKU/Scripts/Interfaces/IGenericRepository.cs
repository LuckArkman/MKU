using MKU.Scripts.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MKU.Scripts.Interface
{
    public interface IGenericRepository<T> : IRepository where T : _Quest
    {
        public Task Insert(T tAggregate, CancellationToken cancellationToken);
        public Task<T> Get(int Id, CancellationToken cancellationToken);

        public Task Delete(T tAggregate, CancellationToken cancellationToken);
        
        public Task Update(T quest, CancellationToken cancellationToken);

    }
}
