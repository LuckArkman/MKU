using System;
using System.Threading.Tasks;
using MKU.Scripts.Strucs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MKU.Scripts.SceneSystem
{
    public class SceneDataManager : MonoBehaviour {

        public static SceneDataManager Instance { get; private set; }

        [SerializeField]
        private MapStorage m_MapStorage;
        public MapStorage MapStorage {
            get {
                return m_MapStorage;
            }
            private set {
                m_MapStorage = value;
            }
        }

        public bool LoadingScene = false;
        public static event Action OnLoadScene;

        private void Awake() {

            if (Instance != null && Instance != this) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }

        }

        public MapData GetCurrentScene() {

            string currentBuildIndex = SceneManager.GetActiveScene().name;
            return null;

        }

        public async Task LoadSceneAsync(string _MapName) {

            await LoadSceneAsynchronously(m_MapStorage.GetMapData(_MapName).unitySceneName);

        }

        public async Task LoadSceneAsync(MapData _MapName) {

            await LoadSceneAsynchronously(_MapName.unitySceneName);

        }

        public async Task LoadSceneAsync(int _MapID) {

            await LoadSceneAsynchronously(m_MapStorage.GetMapData(_MapID).unitySceneName);

        }

        public static bool CompareScenesByType(MapData.SceneType sceneToCompare)
        {
            return Instance.GetCurrentScene().sceneType == sceneToCompare;
        }

        public async Task LoadSceneAsynchronously(string _UnitySceneName) {

            LoadScreen.Instance.ActiveLoadScreen();
            LoadingScene = true;

            /*SceneManager.LoadScene(_UnitySceneName);

            OnLoadScene?.Invoke();

            return;*/

            var sceneLoadOperation = SceneManager.LoadSceneAsync(_UnitySceneName);
            //sceneLoadOperation.allowSceneActivation = false;
            float progress = 0;

            while (!sceneLoadOperation.isDone) {

                progress = Mathf.MoveTowards(progress, sceneLoadOperation.progress, Time.deltaTime);

                //if (sceneLoadOperation.progress >= 0.9f) sceneLoadOperation.allowSceneActivation = true;
                await Task.Delay(200);

            }

            LoadingScene = false;
            OnLoadScene?.Invoke();

        }
        
    }
}