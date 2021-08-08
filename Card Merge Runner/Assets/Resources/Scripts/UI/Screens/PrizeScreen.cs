using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hyperlab.Core.UI;
using Hyperlab.Managers;
using UnityEngine.UI;
using TMPro;
using Hyperlab.UI;
using DG.Tweening;
using System;

public class PrizeScreen : CoreScreen<PrizeScreen>
{
    public CoinBar m_CoinBar;
    public KeyBar m_KeyBar;
    public CoreButton m_BtnNextLevel;
    public PrizeBox m_PrizeBox;
    #region MonoBehaviour
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.onAppStart += Hide;
        GameManager.Instance.onNextLevel += Hide;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        //TransactionManager.transactionInitiated += OnTransactionInitiated;
        //TransactionManager.transactionSucceeded += OnTransactionSucceeded;
        //TransactionManager.transactionFailed += OnTransactionFailed;
        //WalletManager.balanceChanged += OnBalanceChange;
    }

    

    protected override void OnDisable()
    {
        base.OnDisable();
        //TransactionManager.transactionInitiated -= OnTransactionInitiated;
        //TransactionManager.transactionSucceeded -= OnTransactionSucceeded;
        //TransactionManager.transactionFailed -= OnTransactionFailed;
    }
    #endregion
    #region Controls
    public override void Show()
    {
        base.Show();
        Setup();
        
    }
    public override void Hide()
    {
        m_PrizeBox.ClearCurrentItems();
        base.Hide();
    }
    #endregion
    #region Setup
    void Setup()
    {
        
        m_BtnNextLevel.gameObject.SetActive(false);
        m_KeyBar.gameObject.SetActive(true);
        //m_CoinBar.UpdateCoin(DataManager.Instance.m_GameData.m_EconomyData.m_Coin);
        //m_KeyBar.UpdateKeys();
        m_PrizeBox.Setup();
    }
    #endregion
    #region Events
    //private void OnBalanceChange(BalanceChangedEventArgs _args)
    //{
    //    if (_args.currency.key == "ckey")
    //    {
    //        int _keyCount = (int)WalletManager.GetBalance(GameManager.Instance.m_KeyCurrency);
    //        m_KeyBar.UpdateKeys(_keyCount);
    //        if (_keyCount == 0)
    //        {
    //            m_BtnNextLevel.gameObject.SetActive(true);
    //            m_KeyBar.gameObject.SetActive(false);
    //        }
    //    }
    //    if (_args.currency.key == "coin")
    //    {
    //        int _coinCount = (int)WalletManager.GetBalance(GameManager.Instance.m_CoinCurrency);
    //        m_CoinBar.UpdateCoin(_coinCount, true);
    //    }
    //}
    #endregion
   
}