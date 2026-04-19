using System.Collections.Generic;

namespace MKU.Scripts.Interfaces
{
    public interface IQuest
    {
        List<object> _objectives();
        List<object> _rewards();
    }
}