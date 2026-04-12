using UnityEngine;

namespace MKU.Scripts.Interfaces
{
    public interface IPickupSpawner
    {
        bool isCollected();
        object getPickupSpawner();
    }
}