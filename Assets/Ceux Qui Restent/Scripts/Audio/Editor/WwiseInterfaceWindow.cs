using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace CeuxQuiRestent.Tools
{
    public class WwiseInterfaceWindow : EditorWindow
    {
        #region Attributes

        #endregion

        #region EditorWindow Methods
        [MenuItem("Window/Level Editor")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            WwiseInterfaceWindow window = (WwiseInterfaceWindow)EditorWindow.GetWindow(typeof(WwiseInterfaceWindow), false, "Wwise Interface");
            window.Show();
            window.name = "WWise Interface";
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("Sound Register");
        }
        #endregion

        #region Methods
        #endregion
    }
}
#endif