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

        private void Start()
        {
            EventBus.Subscribe(GameEventType.NONE, None);
            EventBus.Subscribe(GameEventType.LOADING, Loading);
            EventBus.Publish(GameEventType.NONE);
        }
        public void None()
        {
            GameState = GameEventType.NONE;

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

            m_player_ctrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
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
