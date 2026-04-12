using UnityEngine;
using CharacterController = MKU.Scripts.CharacterSystem.CharacterController;

namespace MKU.Scripts.Interfaces
{
    public interface IConversant
    {
        void ShowMessage(CharacterController charController);
        Transform getGameObject();
        object getObject();
    }
}