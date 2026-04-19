using UnityEngine;

namespace MKU.Scripts.Strucs
{
    [CreateAssetMenu(fileName = "new MapData", menuName = "Map/MapData")]
    public class MapData : ScriptableObject {

        public string mapName;
        public string unitySceneName;

        public float initXCamera;
        [Range(0f, 1f)]
        public float initYCamera;

        public enum SceneType {
            World = 1,
            Cave = 2,
            Tower = 4,
            ExtraMode = 8
        }
        public SceneType sceneType = SceneType.World;

        public string mapDescription;
        public bool SaveInfo = true;

        public AudioClip defaultAmbientSound;
    }
}