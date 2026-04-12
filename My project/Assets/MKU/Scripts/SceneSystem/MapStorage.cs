using System.Collections.Generic;
using MKU.Scripts.Strucs;
using UnityEngine;

namespace MKU.Scripts.SceneSystem
{
    [CreateAssetMenu(fileName ="new MapStorage", menuName ="Map/MapStorage")]
    public class MapStorage : ScriptableObject {

        [SerializeField]
        private List<MapData> mapDataList = new List<MapData>();

        public MapData GetMapData(int index) {

            if (index < 0) return null;
            return mapDataList[index];

        }

        public MapData GetMapData(string name) {

            for (int i = 0; i < mapDataList.Count; i++) {

                if (mapDataList[i].mapName == name) {
                    return mapDataList[i];
                }

            }

            return null;

        }

        public MapData GetUnityMapData(string unitySceneName) {

            for (int i = 0; i < mapDataList.Count; i++) {

                if (mapDataList[i].unitySceneName == unitySceneName) {
                    return mapDataList[i];
                }

            }

            return mapDataList[0];

        }

        public int GetMapIndex(MapData mapData) {

            for (int i = 0; i < mapDataList.Count; i++) {

                if (mapDataList[i] == mapData) {
                    return i;
                }

            }

            return 0;

        }

    }
}