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

        private EnemySaveLoadManager m_enemy_save_load_manager;

        private DropItemManager m_drop_item_manager;

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
                ItemShopManager.Instance.LoadData();
            }
            else
            {
            }
        }

        public void Setting()
        {
            GameState = GameEventType.SETTING;

            if(m_enemy_save_load_manager == null)
            {
                m_enemy_save_load_manager = GameObject.Find("Enemy Save Load Manager").GetComponent<EnemySaveLoadManager>();
                m_drop_item_manager = GameObject.Find("DropItemManager").GetComponent<DropItemManager>();
            }

            m_enemy_save_load_manager.SaveEnemies();
            m_drop_item_manager.SaveItems();

            Player.ChangeState(PlayerState.IDLE);
        }

        public void Dead()
        {
            GameState = GameEventType.DEAD;

        }
    }
}
