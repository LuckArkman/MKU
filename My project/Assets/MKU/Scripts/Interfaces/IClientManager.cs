using System.Collections.Generic;
using UnityEngine;

namespace MKU.Scripts.Interfaces {

    public interface IClientManager<T>
    {
        void Connected(string _Guid);
        void Disconnected();
        List<T> GetplayersInGame();
        T Getplayer();
        void SetRoomId(string Id);
        void InstancePlayer();
        void InstanceRoom();
        void InstanceOtherPlayer(string userJoinRoom, string ctr);
    }
}