using System.Threading.Tasks;
using Mono.Cecil.Cil;
using UnityEngine;

namespace MKU.Scripts.Strucs
{
    public class LoadScreen : MonoBehaviour
    {

        public static LoadScreen Instance;

        [SerializeField]
        private GameObject m_LoadScreen;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public void ActiveLoadScreen()
        {
            m_LoadScreen.SetActive(true);
        }

        public async void DesactiveLoadScreen()
        {
            await Task.Delay(100);
            m_LoadScreen.SetActive(false);
        }

        public bool IsLoading()
        {
            return m_LoadScreen.activeInHierarchy;
        }
    }
}