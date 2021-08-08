using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hyperlab.Managers;
using System;
using ElephantSDK;

namespace Hyperlab.Core
{
    public class ElephantLogger : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
       
        private void OnEnable()
        {
            GameManager.Instance.onStartPlay += OnStartPlay;
            GameManager.Instance.onLevelComplete += OnLevelComplete;
            GameManager.Instance.onLevelFail += OnLevelFail;
        }
        private void OnDisable()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.onStartPlay -= OnStartPlay;
            GameManager.Instance.onLevelComplete -= OnLevelComplete;
            GameManager.Instance.onLevelFail -= OnLevelFail;
        }
        private void OnStartPlay()
        {
            Elephant.LevelStarted(GameManager.Instance.m_CurrentLevelIndex+1);
        }
        private void OnLevelComplete()
        {
            Elephant.LevelCompleted(GameManager.Instance.m_CurrentLevelIndex + 1);
        }
        private void OnLevelFail()
        {
            Elephant.LevelFailed(GameManager.Instance.m_CurrentLevelIndex + 1);
        }
       
    } 
}
