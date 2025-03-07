using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace Junyoung
{
    public class GameManager : Singleton<GameManager>
    {
        private PlayerCtrl m_player_ctrl;

        public GameEventType GameStatus { get; private set; }

        private void Start()
        {
            EventBus.Subscribe(GameEventType.NONE, None);
            EventBus.Subscribe(GameEventType.LOADING, Loading);
            EventBus.Publish(GameEventType.NONE);
        }
        public void None()
        {
            GameStatus = GameEventType.NONE;
        }

        public void Loading()
        {
            GameStatus = GameEventType.LOADING;
        }

        public void Playing()
        {
            GameStatus = GameEventType.PLAYING;

            m_player_ctrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
        }

        public void Setting()
        {
            GameStatus = GameEventType.SETTING;

        }

        public void Dead()
        {
            GameStatus = GameEventType.DEAD;

        }

    }
}
