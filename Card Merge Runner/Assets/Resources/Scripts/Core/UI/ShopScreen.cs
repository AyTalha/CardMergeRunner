using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hyperlab.Core.UI;
using Hyperlab.Managers;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Hyperlab.Core;
using Sirenix.OdinInspector;


namespace Hyperlab.UI
{
    public class ShopScreen : CoreScreen<ShopScreen>
    {

#if UNITY_EDITOR
        [ValueDropdown("GetStoreKeys")]
#endif
        public string m_StoreKey;
        [FoldoutGroup("Components")]
        public RectTransform m_TouchBlockPanel;
        [FoldoutGroup("Components")]
        public ShopCarousel m_ItemsCarousel;
        [FoldoutGroup("Components")]
        public GameObject m_ShopButtonContainer;

        //[ReadOnly]
        //public Store m_Store;
        [ReadOnly]
        public CoreShop m_Shop;
        #region MonoBehaviour
        protected override void Awake()
        {
            m_Container.DOLocalMoveY(-1500, 0);
        }
        protected override void Start()
        {
            base.Start();

            GameManager.Instance.onAppStart += Hide;
            GameManager.Instance.onUISetup += OnUISetup;
            GameManager.Instance.onStartPlay += OnStartPlay;
            GameManager.Instance.onInitialize += OnManagerInitialize;

            m_ItemsCarousel.onPageChange += OnCarouselPageChange;

            //InventoryManager.itemAdded += OnInventoryItemChanged;
            //InventoryManager.itemRemoved += OnInventoryItemChanged;

            //TransactionManager.transactionInitiated += OnTransactionInitiated;
            //TransactionManager.transactionProgressed += OnTransactionProgress;
            //TransactionManager.transactionSucceeded += OnTransactionSucceeded;
            //TransactionManager.transactionFailed += OnTransactionFailed;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        public virtual void Initialize()
        {
            //m_Store = GetStore();
            //m_Shop = GetShop();
        }
        public virtual void Setup()
        {
            m_ItemsCarousel.Setup(m_Shop.m_Items);
        }
        #endregion
        #region Screen Controls
        public override void Show()
        {
            Setup();
            base.Show();
            m_Container.DOLocalMoveY(0, 0.7f).SetEase(Ease.OutBounce);
        }
        public override void Hide()
        {
            m_Container.DOLocalMoveY(-1500, 0.7f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                base.Hide();
            });
        }
        #endregion
        #region Store Controls
        //public virtual Store GetStore()
        //{
        //    return GameFoundation.catalogs.storeCatalog.FindItem(m_StoreKey);
        //}
        //public virtual CoreShop GetShop()
        //{
        //    return ShopManager.Instance.GetShop(m_StoreKey);
        //}
        public virtual void UnlockItem()
        {

        }
        public virtual void SetItemLocked()
        {

        }
        public virtual void SetItemUnlocked()
        {

        }
        public virtual void SetItemSelected()
        {

        }
        #endregion
        #region Events
        protected virtual void OnManagerInitialize()
        {
            Initialize();
        }
        protected virtual void OnUISetup()
        {
            if (ProjectSettings.Instance.m_Shop)
                m_ShopButtonContainer.SetActive(true);
            else
                m_ShopButtonContainer.SetActive(false);
        }
        protected virtual void OnStartPlay()
        {
            if (ProjectSettings.Instance.m_Shop)
                m_ShopButtonContainer.SetActive(false);
        }
        protected virtual void OnCarouselPageChange(int _pageIndex)
        {

        }
        #endregion
       

        #region Editor
        IEnumerable GetStoreKeys()
        {
            //StoreAsset[] _stores = GameFoundationDatabaseSettings.database.storeCatalog.GetItems();
            //List<string> _storeKeys = new List<string>();

            //foreach (var _store in _stores)
            //{
            //    _storeKeys.Add(_store.key);
            //}
            //return _storeKeys;
            return null;
        }
        #endregion
    }

}