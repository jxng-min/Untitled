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
            else
            {
                m_player.UpdateHP(-1.5f);
            }

        }
    }

}
