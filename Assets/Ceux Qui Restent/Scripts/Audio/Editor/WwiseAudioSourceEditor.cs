using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace CeuxQuiRestent.Audio
{
    [CustomEditor(typeof(WwiseAudioSource))]
    public class WwiseAudioSourceEditor : Editor
    {
        WwiseAudioSource audioSource;

        void OnEnable()
        {
            audioSource = target as WwiseAudioSource;
        }

        public override void OnInspectorGUI()
        {
            Color defaultGUIBackgroundColor = GUI.backgroundColor;

            audioSource.autoRename = EditorGUILayout.Toggle(new GUIContent("Auto Rename"), audioSource.autoRename);
            audioSource.playAtStart = EditorGUILayout.Toggle(new GUIContent("Play at start"), audioSource.playAtStart);

            EditorGUILayout.Space();

            if (audioSource.playAudioAsset)
            {
                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = new Color(0.95f, 0.85f, 0.75f);
                EditorGUILayout.LabelField("Audio Asset:");
                if (GUILayout.Button("Switch to Wwise Event"))
                    audioSource.playAudioAsset = !audioSource.playAudioAsset;
                GUI.backgroundColor = new Color(0.75f, 0.85f, 0.95f);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                AudioManager audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
                
                if (audioManager.audioRepository.GetAudioCategories().Count > 0)
                {
                    audioSource.categoryID = EditorGUILayout.Popup(audioSource.categoryID, audioManager.audioRepository.FindCategoryNames().ToArray());

                    List<string> audioAssetIDs = audioManager.audioRepository.FindAudioAssetIDs(audioSource.categoryID);
                    if (audioAssetIDs.Count > 0)
                    {
                        int audioAssetIndex = audioManager.audioRepository.FindAudioAssetIndex(audioSource.audioAssetID, audioAssetIDs);
                        List<string> audioAssetNames = FindAudioAssetNames(audioManager.audioRepository, audioSource.categoryID);
                        int newAudioAssetIndex = EditorGUILayout.Popup(audioAssetIndex, audioAssetNames.ToArray());
                        audioSource.audioAssetID = audioAssetIDs[newAudioAssetIndex];
                        AudioAsset audioAsset = audioManager.audioRepository.FindAudioAsset(audioSource.categoryID, audioSource.audioAssetID);
                        EditorGUILayout.HelpBox("Type: " + audioAsset.type.ToString() + "\nSubtitle: (" + audioAsset.subtitleDuration + "s)\n" + audioAsset.subtitle + "\nDev notes: " + audioAsset.editorNotes, MessageType.Info, true);
                        if (audioSource.autoRename)
                            audioSource.gameObject.name = "WAudioSource - (" + audioAsset.subtitleDuration + "s) - " + audioAssetNames[newAudioAssetIndex];
                    }
                    else
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.LabelField("No audio assets found in this category.");
                        if (audioSource.autoRename)
                            audioSource.gameObject.name = "WAudioSource - ";
                        EditorGUI.EndDisabledGroup();
                    }
                }
                else
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.LabelField("No audio categories in this repository.");
                    if (audioSource.autoRename)
                        audioSource.gameObject.name = "WAudioSource - ";
                    EditorGUI.EndDisabledGroup();
                }

            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = new Color(0.75f, 0.85f, 0.95f);
                EditorGUILayout.LabelField("Wwise Event:");
                if (GUILayout.Button("Switch to Audio Asset"))
                    audioSource.playAudioAsset = !audioSource.playAudioAsset;
                GUI.backgroundColor = new Color(0.95f, 0.85f, 0.75f);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Select Wwise Event");
                string eventName = "";
                audioSource.wwiseEvent = WwiseEventDrawer.WwiseEventField(ref eventName, audioSource.wwiseEvent, GUILayoutUtility.GetLastRect());
                EditorGUILayout.EndHorizontal();
                if (audioSource.autoRename)
                    audioSource.gameObject.name = "WAudioSource - " + eventName;
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Play Sound"))
            {
                audioSource.Play();
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }

        public List<string> FindAudioAssetNames(AudioRepository audioRepo, int _categoryID)
        {
            List<string> audioAssetNames = new List<string>();

            if (_categoryID >= 0 && _categoryID < audioRepo.GetAudioCategories().Count)
                for (int aa = 0; aa < audioRepo.GetAudioCategories()[_categoryID].audioAssets.Count; aa++)
                    audioAssetNames.Add(WwiseEventDrawer.GetEventName(audioRepo.GetAudioCategories()[_categoryID].audioAssets[aa].wwiseEvent));

            return audioAssetNames;
        }
    }
}
#endif

