using UnityEngine;

namespace Junyoung
{
    public class GameManager : Singleton<GameManager>
    {
        private PlayerCtrl m_player_ctrl;
        public PlayerCtrl Player
        {
            get { return m_player_ctrl; }
        }

        public GameEventType GameState { get; private set; }

        private bool m_can_init = false;

        private new void Awake()
        {
            base.Awake();

            EventBus.Subscribe(GameEventType.NONE, None);
            EventBus.Subscribe(GameEventType.LOADING, Loading);
            EventBus.Publish(GameEventType.NONE);
        }
        public void None()
        {
            GameState = GameEventType.NONE;

            m_can_init = true;

            SettingManager.Instance.Initialize();
            SoundManager.Instance.UpdateBackgroundVolume();
            ObjectManager.Instance.Initialize();
            NPCDataManager.Instance.Initialize();
        }

        public void Loading()
        {
            GameState = GameEventType.LOADING;
        }

        public void Playing()
        {
            GameState = GameEventType.PLAYING;

            if(m_can_init)
            {
                m_can_init = false;

                m_player_ctrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();

                DataManager.Instance.Initialize();
            }
            else
            {

            }
        }

        public void Setting()
        {
            GameState = GameEventType.SETTING;
        }

        public void Dead()
        {
            GameState = GameEventType.DEAD;

        }
    }
}
