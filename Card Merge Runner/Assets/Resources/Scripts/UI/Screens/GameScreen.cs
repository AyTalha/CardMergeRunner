using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hyperlab.Core.UI;
using Hyperlab.Managers;
using System;
using TMPro;
using DG.Tweening;
using Hyperlab.Controllers;
using Sirenix.OdinInspector;
namespace Hyperlab.UI
{
    public class GameScreen : CoreScreen<GameScreen>
    {
        [FoldoutGroup("Bars", expanded: true)]
        public CoinBar m_CoinBar;
        [FoldoutGroup("Bars")]
        public KeyBar m_KeyBar;
        [FoldoutGroup("Texts", expanded: true)]
        public TextMeshProUGUI m_LevelText;

        [FoldoutGroup("Components")]
        public GameObject m_TouchPanel;
        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            
        }
        protected override void Start()
        {
            GameManager.Instance.onAppStart += Hide;
            GameManager.Instance.onStartPlay += Show;
            GameManager.Instance.onLevelFinish += Hide;
            //Cache Currencies
            GameManager.Instance.onInGameCoinChange += OnCoinChange;
            GameManager.Instance.onInGameKeyChange += OnKeyChange;
        }
        protected override void OnEnable()
        {
            
        }
        protected override void OnDisable()
        {
            
        }
        
        #endregion
        #region Controls
        
        public override void Show()
        {
            base.Show();
            //New System
            m_KeyBar.UpdateKeys(GameManager.Instance.m_InGameKey);
            m_CoinBar.UpdateCoin(GameManager.Instance.m_InGameCoin,false);
            m_LevelText.text = "Level " + (GameManager.Instance.m_CurrentLevelIndex + 1).ToString();
            if (GameManager.Instance.m_FirstTutorialScreenActive)
                m_TouchPanel.gameObject.SetActive(true);
            else
                m_TouchPanel.gameObject.SetActive(false);
        }
        #endregion
        #region Events
        void OnCoinChange(int _coin)
        {
            m_CoinBar.UpdateCoin(_coin, true);
        }
        void OnKeyChange(int _key)
        {
            m_KeyBar.UpdateKeys(_key);
        }
        #endregion
    }
}
