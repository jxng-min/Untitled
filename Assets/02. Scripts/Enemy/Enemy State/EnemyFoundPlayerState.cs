using Junyoung;
using System.Collections;
using UnityEngine;

namespace Junyoung
{
    public class EnemyFoundPlayerState : MonoBehaviour, IEnemyState<EnemyCtrl>
    {
        private EnemyCtrl m_enemy_ctrl;
        private float m_found_player_ani_length;
        public void OnStateEnter(EnemyCtrl sender)
        {
            if (m_enemy_ctrl == null)
            {
                m_enemy_ctrl = sender;
            }
            m_enemy_ctrl.BackPosition = m_enemy_ctrl.transform.position;
            m_enemy_ctrl.Animator.SetTrigger("FoundPlayer");

            StartCoroutine(GetAniLength());
        }
        public void OnStateUpdate(EnemyCtrl sender)
        {
            if(m_found_player_ani_length>=0)
            {
                m_found_player_ani_length-=Time.deltaTime;
            }
            else
            {
                m_enemy_ctrl.ChangeState(EnemyState.FOLLOW);
            }
        }
        public void OnStateExit(EnemyCtrl sender)
        {

        }
        private IEnumerator GetAniLength() // Attack Ʈ���Ű� ȣ�� ������ �����̰� �־ StateInfo�� �ִϸ��̼� ��ȯ ���� ȣ��Ǵ� ���� ������ ���
        {
            m_found_player_ani_length = 1f;
            yield return new WaitForSeconds(0.1f); // �ణ�� ��� �ð��� �־� �ִϸ��̼� ���� ��ȯ�� ��ٸ�          
           
            m_found_player_ani_length = m_enemy_ctrl.GetAniLength("Found Player") - 0.1f;
        }
    }
}