using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Collections;

namespace Junyoung
{
    public class EnemyBossAttackState : EnemyAttackState
    {
        private Queue<int> m_combo_queue;
        private Queue<int> m_skill_queue;

        private bool m_can_use_skill = true;

        private GameObject m_skill1_area;

        private bool m_using_skill = false;

        private DecalProjector m_projector;

        public override void OnStateEnter(EnemyCtrl sender)
        {
            if (m_enemy_ctrl == null)
            {
                m_enemy_ctrl = sender;

                m_player = m_enemy_ctrl.Player;
                m_player_ctrl = m_player.GetComponent<PlayerCtrl>();

                m_combo_queue = new Queue<int>();
                m_skill_queue= new Queue<int>();

                EnemyMeleeWeaponCtrl[] weapons = GetComponentsInChildren<EnemyMeleeWeaponCtrl>(true);
                foreach(var weapon in weapons)
                {
                    if(weapon.gameObject.name == "Skill1_Area")
                    {
                        m_skill1_area = weapon.gameObject;
                    }
                }

                m_projector = GetComponentInChildren<DecalProjector>(true);
            }

            m_enemy_ctrl.IsHitting = false;
            m_enemy_ctrl.AttackDelay = 0;

            if(m_can_use_skill ) 
            {
                UseSkill();
            }
            else
            {
                UseCombo();
            }
        }

        public override void OnStateUpdate(EnemyCtrl sender)
        {
            base.OnStateUpdate(sender);
            if (!m_using_skill)
            {
                m_enemy_ctrl.LookPlayer();
            }
        }

        public override void OnStateExit(EnemyCtrl sender)
        {
            base.OnStateExit(sender);
            m_using_skill = false;
            if (m_skill1_area.activeInHierarchy)
            {
                m_skill1_area.SetActive(false);
            }
            m_enemy_ctrl.EnemyStat.AtkDamege = m_enemy_ctrl.OriginEnemyStat.AtkDamege;
        }

        public void InitComboQueue()
        {
            m_combo_queue.Clear();
            List<int> list = new List<int>() { 0, 1, 2 };
            Shuffle(list);
            foreach(int i in list)
            {
                m_combo_queue.Enqueue(i);
            }
        }

        public void InitSkillQueue()
        {
            m_skill_queue.Clear();
            List<int> list = new List<int>() { 0 };
            Shuffle(list);
            foreach (int i in list)
            {
                m_skill_queue.Enqueue(i);
            }
        }

        public void UseCombo()
        {
            if(m_combo_queue.Count == 0)
            {
                InitComboQueue();
            }

            int combo_index = m_combo_queue.Dequeue();

            switch(combo_index)
            {
                case 0:
                    {
                        m_enemy_ctrl.Animator.SetTrigger("Combo1");
                        StartCoroutine(GetAniLength("Combo1"));
                        break;
                    }
                case 1:
                    {
                        m_enemy_ctrl.Animator.SetTrigger("Combo2");
                        StartCoroutine(GetAniLength("Combo2"));
                        break;
                    }
                case 2:
                    {
                        m_enemy_ctrl.Animator.SetTrigger("Combo3");
                        StartCoroutine(GetAniLength("Combo3"));
                        break;
                    }
                default:
                    {
                        Debug.LogError("해당 콤보가 존재하지 않음");
                        break;
                    }
            }
        }

        public void UseSkill()
        {
            m_using_skill = true;
            StartCoroutine(SkillCoolDown());
            if (m_skill_queue.Count == 0)
            {
                InitSkillQueue();
            }

            int skill_index = m_skill_queue.Dequeue();

            switch (skill_index)
            {
                case 0:
                    {
                        StartCoroutine(Skill1RedZone());
                        StartCoroutine(Skill1());
                        break;
                    }
                case 1:
                    {
                        m_enemy_ctrl.Animator.SetTrigger("Skill2");
                        StartCoroutine(GetAniLength("Skill2"));
                        break;
                    }
                default:
                    {
                        Debug.LogError("해당 스킬이 존재하지 않음");
                        break;
                    }
            }
        }

        private IEnumerator Skill1()
        {
            m_enemy_ctrl.Animator.SetTrigger("Skill1");
            StartCoroutine(GetAniLength("Skill1"));
            m_enemy_ctrl.EnemyStat.AtkDamege /= 3f;

            HashSet<float> triggeredPoints = new HashSet<float>(); // 중복 실행 방지

            while (true)
            {
                AnimatorStateInfo info = m_enemy_ctrl.Animator.GetCurrentAnimatorStateInfo(0);
                float n_time = info.normalizedTime;

                if (n_time >= 1f)
                {
                    yield break; 
                }
                if (n_time >= 0.25f && !triggeredPoints.Contains(0.25f))
                {
                    m_skill1_area.SetActive(true);
                    triggeredPoints.Add(0.25f);
                }

                if (n_time >= 0.3f && !triggeredPoints.Contains(0.3f))
                {
                    (m_enemy_ctrl as EnemyBossCtrl).Effect(0);
                    triggeredPoints.Add(0.3f);
                }

                if (n_time >= 0.45f && !triggeredPoints.Contains(0.45f))
                {
                    (m_enemy_ctrl as EnemyBossCtrl).Effect(0);
                    triggeredPoints.Add(0.45f);
                }

                if (n_time >= 0.6f && !triggeredPoints.Contains(0.6f))
                {
                    (m_enemy_ctrl as EnemyBossCtrl).Effect(0);
                    triggeredPoints.Add(0.6f);
                }

                yield return null; // 다음 프레임까지 대기
            }
        }

        private IEnumerator Skill1RedZone()
        {
            m_projector.gameObject.SetActive(true);
            float projector_z = 0f;
            float pivot_z = 0f;
            while (m_projector.size.z <= 14f)
            {
                projector_z += 10f * Time.deltaTime;
                pivot_z += 6f * Time.deltaTime;
                m_projector.size = new Vector3(10, 0.3f, projector_z);
                m_projector.pivot= new Vector3(0, 0, pivot_z);
                yield return null;
            }
            m_projector.gameObject.SetActive(false);
            m_projector.size = new Vector3(13.4f, 0.5f, 7);
            m_projector.pivot = new Vector3(0, 0, 5);
        }

        private void Shuffle(List<int> list) //Fisher Yates Shuffle 알고리즘
        {
            for(int i = list.Count -1; i >0; i-- )
            {
                int rand = Random.Range(0, i + 1);
                (list[i], list[rand]) = (list[rand], list[i]);
            }
        }

        private IEnumerator SkillCoolDown()
        {
            m_can_use_skill = false;
            yield return new WaitForSeconds(1f);
            m_can_use_skill = true;
        }
    }
}