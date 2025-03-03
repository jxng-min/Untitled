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

        private DecalProjector m_skill1_projector;
        private DecalProjector m_skill2_projector;
        private DecalProjector m_skill2_inner_projector;

        private int m_skill2_effect_count = 40;
        private float m_skill2_start_radius = 4f;
        private float m_skill2_radius = 15f;
        private float m_expand_speed = 5f;
        private float m_skill2_now_radius = 0;

        private List<GameObject> m_skill2_effects = new List<GameObject>();

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

                DecalProjector[] decals = GetComponentsInChildren<DecalProjector>(true);
                foreach(var decal in decals)
                {
                    if(decal.gameObject.name == "Skill1RedZone")
                    {
                        m_skill1_projector = decal;
                    }
                    else if(decal.gameObject.name == "Skill2RedZone")
                    {
                        m_skill2_projector = decal;
                    }
                    else if(decal.gameObject.name == "Skill2RedZoneInner")
                    {
                        m_skill2_inner_projector = decal;
                    }
                }

                StartCoroutine(SkillCoolDown());
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
            List<int> list = new List<int>() {0, 1};
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
                        StartCoroutine (Skill2RedZone());
                        StartCoroutine(Skill2());
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
                    var effect = (m_enemy_ctrl as EnemyBossCtrl).Effect(0, new Vector3(transform.position.x, 0, transform.position.z));
                    StartCoroutine((m_enemy_ctrl as EnemyBossCtrl).DestroyEffect(effect, 2f));
                    triggeredPoints.Add(0.3f);
                }

                if (n_time >= 0.45f && !triggeredPoints.Contains(0.45f))
                {
                    var effect = (m_enemy_ctrl as EnemyBossCtrl).Effect(0, new Vector3(transform.position.x, 0, transform.position.z));
                    StartCoroutine((m_enemy_ctrl as EnemyBossCtrl).DestroyEffect(effect, 2f));
                    triggeredPoints.Add(0.45f);
                }

                if (n_time >= 0.6f && !triggeredPoints.Contains(0.6f))
                {
                    var effect = (m_enemy_ctrl as EnemyBossCtrl).Effect(0, new Vector3(transform.position.x, 0, transform.position.z));
                    StartCoroutine((m_enemy_ctrl as EnemyBossCtrl).DestroyEffect(effect, 1.5f));
                    triggeredPoints.Add(0.6f);
                }

                yield return null; 
            }
        }

        private IEnumerator Skill1RedZone()
        {
            m_skill1_projector.gameObject.SetActive(true);
            float projector_y = 0f;
            float pivot_y = 0f;
            while (m_skill1_projector.size.y <= 14f)
            {
                projector_y += 10f * Time.deltaTime;
                pivot_y += 6f * Time.deltaTime;
                m_skill1_projector.size = new Vector3(13.5f, projector_y, 0.5f);
                m_skill1_projector.pivot = new Vector3(0, pivot_y,0 );
                yield return null;
            }
            m_skill1_projector.gameObject.SetActive(false);
            m_skill1_projector.size = new Vector3(13.5f, 7, 0.5f);
            m_skill1_projector.pivot = new Vector3(0, 5, 0);
        }

        private IEnumerator Skill2()
        {
            m_enemy_ctrl.Animator.SetTrigger("Skill2");
            StartCoroutine(GetAniLength("Skill2"));
            yield return new WaitForSeconds(0.1f); // 애니메이션 길이 계산시간 동안 대기
            yield return new WaitForSeconds(m_atk_ani_length - 0.3f);
            for (int i=0; i<m_skill2_effect_count; i++)
            {
                float angle = i * Mathf.PI *2 / m_skill2_effect_count;
                Vector3 pos = transform.position + new Vector3 (Mathf.Cos(angle) * m_skill2_start_radius, 0 ,Mathf.Sin(angle) * m_skill2_start_radius);
                var effect = (m_enemy_ctrl as EnemyBossCtrl).Effect(2, pos);
                m_skill2_effects.Add(effect);
            }
            StartCoroutine(ExpandSkill2Effect());
        }

        private IEnumerator ExpandSkill2Effect()
        {
            m_skill2_now_radius = m_skill2_start_radius;
            while (m_skill2_now_radius < m_skill2_radius)
            {
                m_skill2_now_radius += m_expand_speed * Time.deltaTime;

                for (int i = 0; i < m_skill2_effect_count; i++)
                {
                    float angle = i * Mathf.PI * 2 / m_skill2_effect_count;
                    Vector3 new_pos = transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * m_skill2_now_radius;
                    m_skill2_effects[i].transform.position = new_pos;
                }
                yield return null;
            }
            if (m_skill2_now_radius > m_skill2_radius)
            {
                for (int i = 0; i < m_skill2_effects.Count; i++)
                {
                    StartCoroutine((m_enemy_ctrl as EnemyBossCtrl).DestroyEffect(m_skill2_effects[i], 5f));
                }
            }
            m_skill2_effects.Clear();
        }

        private IEnumerator Skill2RedZone()
        {
            m_skill2_projector.gameObject.SetActive(true);
            float projector_x = 0f;
            float projector_y = 0f;
            
            while (m_skill2_inner_projector.size.x < 35f)
            {
                projector_x += 17f * Time.deltaTime;
                projector_y += 17f * Time.deltaTime;
                m_skill2_inner_projector.size = new Vector3(projector_x, projector_y, 0.5f );
                yield return null;
            }
            m_skill2_projector.gameObject.SetActive(false);
            m_skill2_inner_projector.size = new Vector3(10f, 10, 0.5f );
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
            yield return new WaitForSeconds(30f);
            m_can_use_skill = true;
        }

    }
}