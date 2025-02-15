using UnityEngine;

namespace Junyoung
{
    public class EnemyBowReadyState : EnemyReadyState
    {
        GameObject m_arrow;
        public override void OnStateEnter(EnemyCtrl sender)
        {
            base.OnStateEnter(sender);
            if (!m_arrow)
            {
                Transform[] all_childs = GetComponentsInChildren<Transform>(true);
                foreach (Transform child in all_childs)
                {
                    if (child.name == "arrow")
                    {
                        m_arrow = child.gameObject;
                    }
                }
            }
            m_arrow.SetActive(true);
        }

        public override void OnStateExit(EnemyCtrl sender)
        {
            base.OnStateExit(sender);
            m_arrow.SetActive(false);
        }
    }
}