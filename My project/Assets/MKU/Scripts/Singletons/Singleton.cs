using UnityEngine;
using CharacterController = MKU.Scripts.Character.CharacterController;

namespace MKU.Scripts.Singletons
{
    public class Singleton : MonoBehaviour
    {
        public CharacterController controller = null;

        private static Singleton instance;

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    // Tenta encontrar na cena
                    instance = FindObjectOfType<Singleton>();

                    // Se não existir, cria automaticamente
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("Singleton");
                        instance = obj.AddComponent<Singleton>();

                        Debug.Log("[Singleton] Instância criada automaticamente.");
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }
}