using UnityEngine;

namespace MKU.Scripts.Strucs
{
    public class DestroyForTime : MonoBehaviour {

        public float time;

        void Start () {
            Destroy(this.gameObject,time);
        }

    }
}