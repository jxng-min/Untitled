using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System.Collections.Generic;

namespace Junyoung
{
    public class EnemySaveLoadManager : MonoBehaviour
    {
        //public List<GameObject> m_playing_enemies;
        private string m_save_path;
        private GameObject m_global_object;

        private void Awake()
        {
            m_save_path = Application.persistentDataPath + "/EnemySaveData";
            m_global_object = GameObject.Find("[Global]");
        }


        public void SaveEnemies()
        {
            List<EnemySaveData> save_data_list = new List<EnemySaveData>();

            NavMeshAgent[] enemies = m_global_object.GetComponentsInChildren<NavMeshAgent>();

            foreach (NavMeshAgent t in enemies)
            {               
                EnemySaveData data = new EnemySaveData(
                    new SVector3(t.gameObject.transform.position),
                    new SQuaternion(t.gameObject.transform.rotation),
                    new SEnemyStat(t.gameObject.GetComponent<EnemyCtrl>().EnemyStat),
                    new SEnemySpawnData(t.gameObject.GetComponent<EnemyCtrl>().EnemySpawnData),
                    t.gameObject.GetComponent<EnemyCtrl>().StateContext.NowStateEnum);
                save_data_list.Add(data);
            }

            string json = JsonUtility.ToJson(new SWrapper<EnemySaveData>(save_data_list), true);
            File.WriteAllText(m_save_path, json);
            Debug.Log(json);
        }

        public List<EnemySaveData> LoadEnemies()
        {
            if(!File.Exists(m_save_path))
            {
                Debug.Log("저장된 적 데이터가 없음");
                return null;
            }

            string json = File.ReadAllText(m_save_path);
            SWrapper<EnemySaveData> swrapper = JsonUtility.FromJson<SWrapper<EnemySaveData>>(json);
            Debug.Log($"적 데이터 로드 완료");
            return swrapper.items;
        }
        


    }
}