using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MKU.Scripts.Enums;

namespace MKU.Scripts.Interfaces
{
    public interface IAccountRepository
    {
        Task Insert(UserCode _code, object _object, CancellationToken cancellationToken);
        Task<object> Get(UserCode _code, string Id, CancellationToken cancellationToken);
        Task Delete(UserCode _code, object _object, CancellationToken cancellationToken);
        Task Update(UserCode _code, object _object, CancellationToken cancellationToken);
    }
}
