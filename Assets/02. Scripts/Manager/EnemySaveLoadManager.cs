using UnityEngine;
using System.Collections.Generic;

namespace Junyoung
{
    public class EnemySaveLoadManager : MonoBehaviour
    {
        public List<GameObject> m_playing_enemies;
        private string m_save_path;
        private GameObject m_global_object;

        private void Awake()
        {
            m_save_path = Application.persistentDataPath + "/EnemySaveData";
            m_global_object = GameObject.Find("[Global]");
        }


        private void SaveEnemies()
        {
            List<EnemySaveData> save_data_list = new List<EnemySaveData>();

            Transform[] enemies = GetComponentsInChildren<Transform>();

            foreach (Transform t in enemies)
            {
                /*
                EnemySaveData data = new EnemySaveData(
                    new SVector3(t.gameObject.transform.position),
                    new SQuaternion(t.gameObject.transform.rotation),
                    t.gameObject.GetComponent<EnemyCtrl>().EnemyStat,
                    t.gameObject.GetComponent<EnemyCtrl>().EnemySpawnData,
                    t.gameObject.GetComponent<EnemyCtrl>().StateContext.NowState.ToString());
                */
            }

        }


    }
}