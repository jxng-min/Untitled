using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    PlayerCtrl m_player;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!m_player)
            {
                m_player = other.transform.parent.GetComponent<PlayerCtrl>();
            }


            if (!(m_player.GetComponent<PlayerCtrl>().StateContext.Current is PlayerDeadState))
            {
                m_player.UpdateHP(-1.5f);
            }

        }
    }

}
