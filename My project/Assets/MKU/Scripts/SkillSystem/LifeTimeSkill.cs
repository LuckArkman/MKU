using System.Collections;
using UnityEngine;

namespace MKU.Scripts.SkillSystem
{
    public class LifeTimeSkill : MonoBehaviour
    {
        [Range(0f,10.0f)]
        public float timeDestroy;

        private void Start()
        {
            StartCoroutine(OnDestroyVFX());
        }

        public IEnumerator OnDestroyVFX()
        {
            yield return new WaitForSeconds(timeDestroy);
            Destroy(this.gameObject);
        }
    }
}
