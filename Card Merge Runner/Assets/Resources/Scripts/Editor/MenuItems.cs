using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HedgehogTeam.EasyTouch;
using Hyperlab.Core;
#if UNITY_EDITOR
using UnityEditor;

namespace Hyperlab.Editor
{
    public static class MenuItems
    {
        [MenuItem("GameObject/Hyperlab/GameObjectWithContainer", false, 0)]
        private static void CreateGameObjectWithContainer()
        {
            CreateObjectToScene(ProjectSettings.Instance.m_GameObjectWithContainer, "ObjectWithContainer");
        }
        [MenuItem("GameObject/Hyperlab/UIObjectWithContainer", false, 0)]
        private static void CreateUIObjectWithContainer()
        {
            CreateObjectToScene(ProjectSettings.Instance.m_UIObjectWithContainer, "UIObjectWithContainer");
        }
        [MenuItem("GameObject/Hyperlab/UIObjectWithAnimatedContainer", false, 0)]
        private static void CreateUIObjectWithAnimatedContainer()
        {
            CreateObjectToScene(ProjectSettings.Instance.m_UIObjectWithAnimatedContainer, "UIObjectWithAnimatedContainer");
        }
        [MenuItem("GameObject/Hyperlab/ButtonPub", false, 0)]
        private static void CreateButtonPub()
        {
            CreateObjectToScene(ProjectSettings.Instance.m_ButtonPub, "ButtonPub");
        }
        [MenuItem("GameObject/Hyperlab/ButtonRounded_1", false, 0)]
        private static void CreateButtonRounded_1()
        {
            CreateObjectToScene(ProjectSettings.Instance.m_ButtonRounded_1, "ButtonRounded_1");
        }
        [MenuItem("GameObject/Hyperlab/ButtonRounded_2", false, 0)]
        private static void CreateButtonRounded_2()
        {
            CreateObjectToScene(ProjectSettings.Instance.m_ButtonRounded_2, "ButtonRounded_2");
        }
        [MenuItem("GameObject/Hyperlab/ButtonOutlinedRounded_1", false, 0)]
        private static void CreateButtonOutlinedRounded_1()
        {
            CreateObjectToScene(ProjectSettings.Instance.m_ButtonOutlinedRounded_1, "ButtonOutlinedRounded_1");
        }
        [MenuItem("GameObject/Hyperlab/ButtonOutlinedRounded_2", false, 0)]
        private static void CreateButtonOutlinedRounded_2()
        {
            CreateObjectToScene(ProjectSettings.Instance.m_ButtonOutlinedRounded_2, "ButtonOutlinedRounded_2");
        }
        static void CreateObjectToScene(GameObject _object, string _name)
        {
            GameObject go = PrefabUtility.InstantiatePrefab(_object, Selection.activeTransform) as GameObject;
            PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
            go.name = _name;
            Selection.activeObject = go;
        }
    }

}
#endif