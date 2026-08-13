#if UNITY_EDITOR
using UnityEngine;

namespace DataBase.InitDataSO
{
    public class LevelDataForConfig : MonoBehaviour
    {
        [SerializeField] private LevelInitData levelInitData;

        [ContextMenu("Save Data")]
        public void SaveDataToConfigLevel()
        {
            GameObject playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawnPoint");
            
            GameObject[] bottleSpawnPoints = GameObject.FindGameObjectsWithTag("BottleSpawnPoints");
            
            GameObject[] moneySpawnPoints = GameObject.FindGameObjectsWithTag("MoneySpawnPoints");

            levelInitData.PlayerSpawnPosition = playerSpawnPoint.transform.position;
            
            levelInitData.BottleSpawnPositions.Clear();
            
            levelInitData.MoneySpawnPositions.Clear();
            
            foreach (var point in bottleSpawnPoints)
            {
                levelInitData.BottleSpawnPositions.Add(point.transform.position);
            }
            
            foreach (var point in moneySpawnPoints)
            {
                levelInitData.MoneySpawnPositions.Add(point.transform.position);
            }

            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(levelInitData);
                UnityEditor.AssetDatabase.SaveAssets();
            }
        }
    }
}
#endif