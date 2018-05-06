using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Utility
{
    [CustomEditor(typeof(ActionExecuter))]
    [System.Serializable]
    public class ActionExecuterEditor : Editor {

        // Attributes
        private ActionExecuter actionsExecuter;

        void OnEnable()
        {
            actionsExecuter = target as ActionExecuter;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            #region Attributes
            EditorGUILayout.LabelField("Parameters", EditorStyles.boldLabel);
            //actionsExecuter.SetActionsName(EditorGUILayout.TextField(new GUIContent("Actions Name"), actionsExecuter.GetActionsName()));
            actionsExecuter.SetStarted(EditorGUILayout.Toggle(new GUIContent("Started"), actionsExecuter.GetStarted()));
            actionsExecuter.SetSecondsToWait(EditorGUILayout.FloatField(new GUIContent("Seconds to wait"), actionsExecuter.GetSecondsToWait()));
            actionsExecuter.SetLoop(EditorGUILayout.Toggle(new GUIContent("Loop"), actionsExecuter.GetLoop()));
            #endregion

            EditorGUILayout.Space();

            #region Actions
            List<GenericAction> actions = new List<GenericAction>(actionsExecuter.GetActions());

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Actions to Do", EditorStyles.boldLabel);
            if (GUILayout.Button("+"))
                actions.Add(new GenericAction(GenericActionKind.Instantiate));
            if (GUILayout.Button("-"))
                actions.RemoveAt(actions.Count - 1);
            EditorGUILayout.EndHorizontal();

            float normalLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = Screen.width / 4;
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Seconds to wait");
            EditorGUILayout.LabelField("Action Kind");
            EditorGUILayout.LabelField("Action Target");
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            EditorGUIUtility.labelWidth = normalLabelWidth;

            Color baseGuiColor = GUI.backgroundColor;
            for (int i = 0; i < actions.Count; i++)
            {
                int newActionID = i;
                GenericAction action = GenericActionField(i, actions[i], out newActionID);
                if (newActionID == -99)
                    actions.RemoveAt(i);
                else
                {
                    if (newActionID != i)
                    {
                        if (newActionID >= 0 && newActionID < actions.Count)
                        {
                            GenericAction actionSwitched = actions[newActionID];
                            actions[newActionID] = action;
                            actions[i] = actionSwitched;
                        }
                        else
                            actions[i] = action;
                    }
                    else
                        actions[i] = action;
                }
            }
            GUI.backgroundColor = baseGuiColor;

            actionsExecuter.SetActions(actions);
            #endregion

            EditorUtility.SetDirty(actionsExecuter);
            serializedObject.ApplyModifiedProperties();
        }

        public GenericAction GenericActionField(int _actionID, GenericAction _genericAction, out int _newActionID)
        {
            
            if (_actionID % 2 == 0)
                GUI.backgroundColor = new Color(0.85f, 0.75f, 0.95f);
            else
                GUI.backgroundColor = new Color(0.75f, 0.85f, 0.95f);
            _newActionID = _actionID;
            GenericAction genericAction = new GenericAction();
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            genericAction.SetSecondsToWait((float)EditorGUILayout.FloatField(_genericAction.GetSecondsToWait()));
            genericAction.SetActionKind((GenericActionKind)EditorGUILayout.EnumPopup(_genericAction.GetActionKind()));
            genericAction.SetTarget((GameObject)EditorGUILayout.ObjectField(_genericAction.GetTarget(), typeof(GameObject), true));
            if (GUILayout.Button("^"))
                _newActionID--;
            if (GUILayout.Button("v"))
                _newActionID++;
            if (GUILayout.Button("X"))
                _newActionID = -99;
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            return genericAction;
        }
    }
}
#endif