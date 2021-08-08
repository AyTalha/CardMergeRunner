using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Hyperlab.Controllers;
using Hyperlab.Managers;
namespace Hyperlab.Core
{

    public enum Direction
    {
        Forward,
        Left,
        Right
    }

    [System.Serializable]
    [HideLabel]
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Hyperlab/Create/Level", order = 1)]
   
    public class Level : ScriptableObject
    {
       
  
#if UNITY_EDITOR
        [ValueDropdown("GetThemes")]
#endif
        public string m_Theme;
        public GameObject m_Platform;
#if UNITY_EDITOR
        IEnumerable GetThemes()
        {
            List<string> _themes = new List<string>();
            foreach(var _theme in ThemeDB.Instance.m_List)
            {
                _themes.Add(_theme.m_Id);
            }
            return _themes;
        }
#endif

    }
}