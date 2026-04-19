using System.Threading.Tasks;
using UnityEngine;

namespace MKU.Scripts.Interfaces
{
    public interface IPlayerBase
    {
        Task<object?> OnQuestReceive(string Id);
        void OnCompleteQuest(object _quest);
        void SetClick(bool value);
        GameObject getPlayerBase();
    }
}