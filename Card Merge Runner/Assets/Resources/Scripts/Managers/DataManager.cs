using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using Hyperlab.Core;
using Sirenix.OdinInspector;

namespace Hyperlab.Managers
{
    public class DataManager : SingletonComponent<DataManager>
    {
        public enum LoadState
        {
            Loaded = 1,
            Unloaded = 2
        }
        [ReadOnly]
        public LoadState m_LoadState = LoadState.Unloaded;
        public UnityAction<GameData> onDataLoad;
        public UnityAction<GameData> onDataSave;
        public GameData m_GameData = null;
        private string m_SavedGamesFileName = "data.json";
        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            m_GameData = null;
            m_LoadState = LoadState.Unloaded;
        }

        #endregion
        #region Save / Load / Initialize Games
        public void LoadGameData()
        {
            string filePath = Application.persistentDataPath + "/" + m_SavedGamesFileName;

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                m_GameData = JsonUtility.FromJson<GameData>(dataAsJson);
                if (m_GameData == null)
                {
                    m_GameData = new GameData();
                    SaveGameData();
                }
                Core.Logger.Log("DataManager"," Game Data Loaded " + filePath);
            }
            else
            {
                m_GameData = new GameData();
            }
            m_LoadState = LoadState.Loaded;
            onDataLoad?.Invoke(m_GameData);
        }
        public void SaveGameData()
        {
            string dataAsJson = JsonUtility.ToJson(m_GameData);
            string filePath = Application.persistentDataPath + "/" + m_SavedGamesFileName;
            File.WriteAllText(filePath, dataAsJson);
            Core.Logger.Log("DataManager", " Game Data Saved " + filePath);
            //onDataSave?.Invoke(m_GameData);
        }

        private void OnApplicationQuit()
        {
            SaveGameData();
        }
        #endregion
    }

}