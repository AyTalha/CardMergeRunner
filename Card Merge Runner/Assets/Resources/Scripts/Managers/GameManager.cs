using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hyperlab.Core;
using Hyperlab.Controllers;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Cinemachine;
using MoreMountains.Feedbacks;
using HedgehogTeam.EasyTouch;

namespace Hyperlab.Managers
{
    public class GameManager : SingletonComponent<GameManager>
    {
        public enum State
        {
            Awaiting = 0,
            SettingUp = 1,
            Playing = 2,
            Finished = 3,
            Completed = 4,
            Failed = 5,
            Paused = 6
        }

        #region Events
        public UnityAction onAppStart;
        public UnityAction onLevelSetup;
        public UnityAction onUISetup;
        public UnityAction onLevelReady;
        public UnityAction onInitialize;
        public UnityAction<Theme> onThemeSetup;
        public UnityAction onWaitPlayerAct;
        public UnityAction onPlayerFirstAct;
        public UnityAction onStartPlay;
        public UnityAction onLevelFinish;
        public UnityAction onLevelComplete;
        public UnityAction onLevelFail;
        public UnityAction onNextLevel;
        public UnityAction onRestartLevel;
        public UnityAction<int> onCoinChange;
        public UnityAction<int> onInGameCoinChange;
        public UnityAction<int> onInGameKeyChange;
        public UnityAction<int> onKeyChange;
        public UnityAction<int> onScoreChange;
        public bool isFail,isComplete;

        public GameObject cardHouseTransform;
        //public GameObject levelObject;
        #endregion
        #region Variables
        [ReadOnly] public State m_State = State.Awaiting;
        [FoldoutGroup("Currencies", expanded:true), ReadOnly] public int m_InGameCoin;
        [FoldoutGroup("Currencies"), ReadOnly] public int m_InGameKey;
      
        [FoldoutGroup("Level", expanded: true), ReadOnly] public List<Level> m_Levels = new List<Level>();
        [FoldoutGroup("Level"), SerializeField, Range(1, 100)] private int StartLevel;
        public int m_StartLevel
        {
            get
            {
                return StartLevel - 1;
            }
            set
            {
                StartLevel = value;
            }
        }
        [FoldoutGroup("Level"), ReadOnly] public int m_CurrentLevelIndex = 0;
        [FoldoutGroup("Level"), ReadOnly] public Level m_CurrentLevel;
        [FoldoutGroup("Level"), ReadOnly] public bool m_IsPlayerAct = false;
        [FoldoutGroup("Level"), ReadOnly] public bool m_IsPlayerFirstAct = true;
        [FoldoutGroup("Level")] public bool m_FirstTutorialScreenActive = false;
        [FoldoutGroup("Level")] public bool m_IsDebug = true;
        #endregion

        [FoldoutGroup("Components", expanded: true)] public Transform m_PlatformPlaceHolder;
        [FoldoutGroup("Components")] public Camera m_UICamera;
        [FoldoutGroup("Components")] public GameObject m_SplashScreen;

        [FoldoutGroup("Feedbacks", expanded: true)] public MMFeedbacks m_WinFeedback;
        [FoldoutGroup("Feedbacks")] public MMFeedbacks m_FailFeedback;

        [FoldoutGroup("Loop Delay Settings", expanded: true)] public float m_DelayOnStartPlay = 0;
        [FoldoutGroup("Loop Delay Settings")] public float m_DelayOnLevelSetup = 0;
        [FoldoutGroup("Loop Delay Settings")] public float m_DelayAfterLevelSetup = 0;
        [FoldoutGroup("Loop Delay Settings")] public float m_DelayOnFinish = 0;
        [FoldoutGroup("Loop Delay Settings")] public float m_DelayOnComplete = 0;
        [FoldoutGroup("Loop Delay Settings")] public float m_DelayOnFail = 0;

        
        #region MonoBehaviour
        protected override void Awake()
        {
#if UNITY_IOS
            Application.targetFrameRate = 60;
#endif
            m_Levels.Clear();
            base.Awake();
            Init();
        }
        // Start is called before the first frame update
        void Start()
        {
            Core.Logger.Log("Game Manager", "On App Start");
            onAppStart?.Invoke();
        }
        #endregion
        #region Initialization
        void Init()
        {
            StartCoroutine(DOInitialize());
        }
        IEnumerator DOInitialize()
        {
            m_SplashScreen.SetActive(true);
            while (RemoteConfigManager.Instance.m_UseRemote && RemoteConfigManager.Instance.m_LoadState == RemoteConfigManager.LoadState.Unloaded)
            {
                yield return null;
            }
            Core.Logger.Log("Game Manager", "On Remote Config Loaded");
            Core.Logger.Log("Game Manager", JsonUtility.ToJson(RemoteConfigManager.Instance.m_RemoteParameters));
            InitRemoteConfig();
            DataManager.Instance.LoadGameData();
            while (DataManager.Instance.m_LoadState == DataManager.LoadState.Unloaded)
            {
                yield return null;
            }
            Core.Logger.Log("Game Manager", "On Data Loaded");
            Core.Logger.Log("Game Manager", JsonUtility.ToJson(DataManager.Instance.m_GameData));
            InitializeGame();
            onInitialize?.Invoke();
            
        }
        void InitRemoteConfig()
        {
            //Remote
            if (RemoteConfigManager.Instance.m_UseRemote)
            {
                m_IsDebug = RemoteConfigManager.Instance.m_RemoteParameters.m_IsDebug;
                m_StartLevel = RemoteConfigManager.Instance.m_RemoteParameters.m_StartLevel;
            }
        }
        void InitializeGame()
        {
           
            GameData gameData = DataManager.Instance.m_GameData;


            if (!m_IsDebug)
                m_CurrentLevelIndex = gameData.m_PlayerLevel;
            else
                m_CurrentLevelIndex = m_StartLevel;

            //Loop Levels
            m_Levels.AddRange(LevelDB.Instance.m_List);
            List<Level> _levels = new List<Level>();
            for (int i = 0; i < 10; i++)
            {
                _levels.AddRange(m_Levels);
            }
            m_Levels = _levels;
            StartLevelLoop();
        }
        #endregion
        #region EasyTouch
        public void OnTouchStart(Gesture gesture)
        {
            if (m_IsPlayerAct && m_IsPlayerFirstAct) m_IsPlayerFirstAct = false;
        }
        #endregion

        #region Level Loop

        public void StartLevelLoop()
        {
            StartCoroutine(DOLevelLoop(GetLevel(m_CurrentLevelIndex)));
        }
        public void StartPlay()
        {
            m_IsPlayerAct = true;
        }
        IEnumerator DOLevelLoop(Level level)
        {
            Core.Logger.Log("Game Manager", "Level Loop Started");
            yield return StartCoroutine(DOSetup(level));
            
            if (!m_IsPlayerAct)
                yield return StartCoroutine(DOWaitPlayerAct(level));
            else if (m_FirstTutorialScreenActive)
               yield return StartCoroutine(DOWaitFirstPlayerAct(level));
            yield return StartCoroutine(DOStartPlay(level));
            yield return StartCoroutine(DOLevelFinish(level));
            if (m_State == State.Completed)
                yield return StartCoroutine(DOLevelComplete(level));
            else if (m_State == State.Failed)
                yield return StartCoroutine(DOLevelFail(level));
        }
        IEnumerator DOSetup(Level level)
        {
            m_State = State.SettingUp;
            yield return StartCoroutine(DOClearCurrentLevel());
            yield return StartCoroutine(DOSetupLevel(level));
            yield return StartCoroutine(DOSetupUI(level));
            onLevelReady?.Invoke();
            m_SplashScreen.SetActive(false);
        }
        IEnumerator DOClearCurrentLevel()
        {
            if (m_CurrentLevel != null && m_CurrentLevel.m_Platform != null)
            {
                Destroy(m_CurrentLevel.m_Platform.gameObject);
            }
            yield return null;
        }
        IEnumerator DOSetupLevel(Level level)
        {
            isComplete = false;
            GameObject rayfireMan = GameObject.Find("RayFireMan");
            if (rayfireMan!=null)
            {
                Destroy(rayfireMan);
            }
            m_CurrentLevel = ScriptableObject.CreateInstance<Level>();
            m_CurrentLevel.m_Theme = level.m_Theme;
            m_CurrentLevel.m_Platform = Instantiate(level.m_Platform, m_PlatformPlaceHolder);

            //cardhouse değişiklik
            //levelObject = GameObject.FindGameObjectWithTag("Level");

            //bu satır commentlencek
            //CardHouseScript cardHouseScript = cardHouseTransform.GetComponent<CardHouseScript>();
            //cardHouseScript.transform.position = m_CurrentLevel.m_Platform.GetComponent<LevelController>().CardHouseTransformSphere.transform.position;
            //cardHouseTransform.SetActive(true);
            //
         
            CardHouseManager cardHouseManager = cardHouseTransform.GetComponent<CardHouseManager>();
            cardHouseManager.CardHouseObject[cardHouseManager.houseLevelNumber].transform.position= m_CurrentLevel.m_Platform.GetComponent<LevelController>().CardHouseTransformSphere.transform.position;
            cardHouseManager.CardHouseObject[cardHouseManager.houseLevelNumber].gameObject.SetActive(true);

    





            //cardhouse değişiklik

            ThemeManager.Instance.SelectTheme(level.m_Theme);
            m_IsPlayerFirstAct = true;
            //New System
            m_InGameCoin = DataManager.Instance.m_GameData.m_EconomyData.m_Coin;
            m_InGameKey = DataManager.Instance.m_GameData.m_EconomyData.m_Key;
            yield return new WaitForSeconds(m_DelayOnLevelSetup);
            onLevelSetup?.Invoke();
            yield return new WaitForSeconds(m_DelayAfterLevelSetup);
            Core.Logger.Log("Game Manager", "On Level Generator Setup");
        }
        IEnumerator DOSetupUI(Level level)
        {
            onUISetup?.Invoke();
            yield return null;
            Core.Logger.Log("Game Manager", "On UI Setup");
        }
        IEnumerator DOWaitPlayerAct(Level level)
        {
            onWaitPlayerAct?.Invoke();
            Core.Logger.Log("Game Manager", "On Wait Player Act");
            while (!m_IsPlayerAct)
            {
                yield return null;
            }
        }
        IEnumerator DOWaitFirstPlayerAct(Level level)
        {
            Core.Logger.Log("Game Manager", "On Wait Player First Act");
            while (m_IsPlayerFirstAct)
            {
                yield return null;
            }
            onPlayerFirstAct?.Invoke();
        }
        IEnumerator DOStartPlay(Level level)
        {
            m_State = State.Playing;
            yield return new WaitForSeconds(m_DelayOnStartPlay);
            Core.Logger.Log("Game Manager", "On Start Play");
            onStartPlay?.Invoke();
            while (m_State == State.Playing)
            {
                yield return null;
            }
            yield return null;
        }
        IEnumerator DOLevelFinish(Level level)
        {
            Core.Logger.Log("Game Manager", "On Finish Game");
            onLevelFinish?.Invoke();
            yield return new WaitForSeconds(m_DelayOnFinish);
        }
        IEnumerator DOLevelComplete(Level level)
        {
            isComplete = true;
            Core.Logger.Log("Game Manager", "On Complete Level");
            if (m_WinFeedback != null)
                m_WinFeedback.PlayFeedbacks();
            onLevelComplete?.Invoke();
            yield return new WaitForSeconds(m_DelayOnComplete);
            SaveInGameCoin();
            SaveInGameKey();
            DataManager.Instance.m_GameData.m_PlayerLevel++;
        }
        IEnumerator DOLevelFail(Level level)
        {
            yield return new WaitForSeconds(m_DelayOnFail);
            Core.Logger.Log("Game Manager", "On Level Fail");
            if (m_FailFeedback != null)
                m_FailFeedback.PlayFeedbacks();
            onLevelFail?.Invoke();
            yield return null;
        }
        #region Level Loop Controls
        public void FailLevel()
        {
            m_State = State.Failed;
            isFail = true;
        }
        public void FinishLevel()
        {
            m_State = State.Finished;
        }
        public void CompleteLevel()
        {
            m_State = State.Completed;
        }
        public void NextLevel()
        {
            m_CurrentLevelIndex++;
            StartLevelLoop();
            onNextLevel?.Invoke();
        }
        public void RestartLevel()
        {
            StartLevelLoop();
            onRestartLevel?.Invoke();
            isFail = false;
        }
        public Level GetLevel(int index)
        {
            return m_Levels[index];
        }
        #endregion
        #endregion
        #region Economy
       

        public void IncreaseInGameCoin(int coin)
        {
            m_InGameCoin += coin;
            onInGameCoinChange?.Invoke(m_InGameCoin);
        }
        public void DecreaseInGameCoin(int coin)
        {
            if (m_InGameCoin <= 0)
                return;
            m_InGameCoin -= coin;
            onInGameCoinChange?.Invoke(m_InGameCoin);
        }
       
        public void IncreaseInGameKey()
        {
            //if (m_InGameKey >= m_KeyCurrency.maximumBalance)
            //    return;
            m_InGameKey += 1;
            onInGameKeyChange?.Invoke(m_InGameKey);
        }
        public void DecreaseInGameKey()
        {
            if (m_InGameKey <= 0)
                return;
            m_InGameKey -= 1;
            onInGameKeyChange?.Invoke(m_InGameKey);
        }

        public void SaveInGameKey()
        {
            DataManager.Instance.m_GameData.m_EconomyData.m_Key = m_InGameKey;
        }
        public void SaveInGameCoin()
        {
            DataManager.Instance.m_GameData.m_EconomyData.m_Coin = m_InGameCoin;
        }
        #endregion


    }
}