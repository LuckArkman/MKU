using UnityEngine;

namespace MKU.Scripts.DropSystem
{
    [System.Serializable]
    public class Drop
    {
        public int item_id;
        [Range(0f, 100f)] public float percentDrop;
    }
}