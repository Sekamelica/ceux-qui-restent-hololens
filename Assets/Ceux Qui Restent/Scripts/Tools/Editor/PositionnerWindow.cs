#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using CeuxQuiRestent.Interactables;

namespace CeuxQuiRestent.Tools
{
    public class PositionnerWindow : EditorWindow
    {
        #region Attributes
        private static GameObject goToAssign = null;
        private static GameObject goExample = null;
        #endregion

        #region Window Methods
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            PositionnerWindow window = (PositionnerWindow)EditorWindow.GetWindow(typeof(PositionnerWindow));
            window.name = "Positionner Window";
            window.maxSize = new Vector2(540, 200);
            window.Show();
            
        }

        void OnGUI()
        {
            EditorGUILayout.Space();
            goToAssign = EditorGUILayout.ObjectField("Linkable to reposition", goToAssign, typeof(GameObject), true) as GameObject;
            EditorGUILayout.Space();
            goExample = EditorGUILayout.ObjectField("Linkable example", goExample, typeof(GameObject), true) as GameObject;
            EditorGUILayout.Space();
            if (GUILayout.Button("Reposition the Linkable !"))
            {
                Vector3 modelExamplePos = Vector3.zero;
                Quaternion modelExampleRotation = Quaternion.identity;
                if (goExample.GetComponent<Linkable>() != null)
                {
                    if (goExample.GetComponent<Linkable>().model != null)
                    {
                        Transform modelExampleTransform = goExample.GetComponent<Linkable>().model.gameObject.transform;
                        modelExamplePos = modelExampleTransform.position;
                        modelExampleRotation = modelExampleTransform.rotation;
                    }
                }
                
                if (goToAssign != null)
                {
                    Transform t = goToAssign.transform;
                    t.position = goExample.transform.position;
                    t.rotation = goExample.transform.rotation;
                    if (goToAssign.GetComponent<Linkable>() != null)
                    {
                        MeshRenderer linkableModel = goToAssign.GetComponent<Linkable>().model;
                        if (linkableModel != null)
                        {
                            Transform modelToAssignTransform = linkableModel.gameObject.transform;
                            modelToAssignTransform.position = modelExamplePos;
                            modelToAssignTransform.rotation = modelExampleRotation;
                        }
                    }
                }

                this.Close();

            }
        }
        #endregion

        #region MenuItem Methods
        [MenuItem("CONTEXT/Linkable/Reposition Linkable")]
        private static void LoadWithGoToAssign(MenuCommand menuCommand)
        {
            goToAssign = (menuCommand.context as Linkable).gameObject;
            Init();
        }

        [MenuItem("CONTEXT/Linkable/Linkable Example")]
        private static void LoadWithGoExample(MenuCommand menuCommand)
        {
            goExample = (menuCommand.context as Linkable).gameObject;
            Init();
        }

        /*
        [MenuItem("CONTEXT/Hierarchy/Positionner")]
        private static void LoadPositionnerWindow()
        {
            gosToAssign = Selection.gameObjects;
            Init();
        }*/
        #endregion
    }
}
#endif