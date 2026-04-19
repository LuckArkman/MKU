using MKU.Scripts.CharacterSystem;
using UnityEngine;

namespace MKU.Scripts.Interfaces
{
    public interface IConversant
    {
        void ShowMessage(CharController charController);
        Transform getGameObject();
        object getObject();
    }
}