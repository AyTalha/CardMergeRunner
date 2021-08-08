using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hyperlab.Core.UI;
using Hyperlab.Managers;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Hyperlab.Controllers;

namespace Hyperlab.UI
{
    public class FailScreen : CoreScreen<FailScreen>
    {
        public Vector2 m_WorldAvarageRange;
        public TextMeshProUGUI m_WorldAvarageText;
        public TextMeshProUGUI m_MatchSliderRateText;
        public Slider m_MatchSlider;
        public Slider m_WolrdAvarageSlider;
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            GameManager.Instance.onAppStart += Hide;
            GameManager.Instance.onLevelFail += Show;
            GameManager.Instance.onRestartLevel += Hide;
        }
        public override void Show()
        {
            base.Show();
        }
        
    }

}