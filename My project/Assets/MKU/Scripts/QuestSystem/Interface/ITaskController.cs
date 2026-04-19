using System.Collections.Generic;
using System.Threading.Tasks;
using MKU.Scripts.Enums;

namespace MKU.Scripts.Tasks.Interface
{
    public interface ITaskController
    {
        void VerifyTask<T>(Objective _quest, string data);
        Task CompleteTask<T>(T _quest);
    }
}