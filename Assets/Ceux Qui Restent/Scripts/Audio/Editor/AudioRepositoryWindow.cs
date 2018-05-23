using System.Collections.Generic;
using CeuxQuiRestent.Audio;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;

namespace CeuxQuiRestent.Tools
{
    public class AudioRepositoryWindow : EditorWindow
    {
        #region Attributes
        private AudioRepository audioRepository;
        
        private int selectedAudioCategory = 0;
        
        private GUIStyle customFoldout;
        private Color defaultGUIColor = new Color(0.5f, 0.5f, 0.5f);
        private Color pairColor = new Color(0.7f, 0.65f, 0.8f);
        private Color evenColor = new Color(0.8f, 0.65f, 0.7f);

        private Vector2 scroll;
        #endregion

        #region EditorWindow Methods
        [MenuItem("Window/Ceux Qui Restent/Audio Repository")]
        static void Init()
        {
            // Get existing open window or if none, make a new one:
            AudioRepositoryWindow window = (AudioRepositoryWindow)EditorWindow.GetWindow(typeof(AudioRepositoryWindow), false, "Audio Repository");
            window.Show();
            window.name = "Audio Repository";
        }

        void OnGUI()
        {
            // Styles
            customFoldout = new GUIStyle(GUI.skin.GetStyle("Foldout"));
            customFoldout.fontStyle = FontStyle.Bold;

            // Initialization
            audioRepository = EditorGUILayout.ObjectField("Audio Repository", audioRepository, typeof(AudioRepository), true) as AudioRepository;
            EditorGUILayout.Space();
            
            if (audioRepository != null)
            {
                scroll = EditorGUILayout.BeginScrollView(scroll);

                EditorGUILayout.Space();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                List<AudioCategory> audioCategories = AudioCategoriesField(audioRepository.GetAudioCategories());
                audioRepository.SetAudioCategories(audioCategories);

                EditorGUILayout.EndScrollView();

                EditorUtility.SetDirty(audioRepository);
            }
        }
        #endregion

        #region Methods
        private bool ButtonAdd(string _text)
        {
            Color contextBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.95f, 0.95f, 0.45f);
            bool buttonClicked = (GUILayout.Button(_text, GUILayout.MaxWidth(250)));
            GUI.backgroundColor = contextBackgroundColor;
            return buttonClicked;
        }

        private bool ButtonRemove(string _text)
        {
            Color contextBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = new Color(0.95f, 0.45f, 0.45f);
            bool buttonClicked = (GUILayout.Button(_text, GUILayout.MaxWidth(250)));
            GUI.backgroundColor = contextBackgroundColor;
            return buttonClicked;
        }

        private void AddCategory(ref List<AudioCategory> _audioCategories)
        {
            AudioCategory newAudioAssetCategory = new AudioCategory();

            for (int aac = 0; aac < _audioCategories.Count; aac++)
                if (_audioCategories[aac].categoryName == newAudioAssetCategory.categoryName)
                    newAudioAssetCategory.categoryName += "(Bis)";

            _audioCategories.Add(newAudioAssetCategory);

            // Editor changes
            selectedAudioCategory = _audioCategories.Count - 1;
        }

        private void RemoveCategory(ref List<AudioCategory> _audioCategories)
        {
            _audioCategories.RemoveAt(selectedAudioCategory);

            // Editor changes
            while (selectedAudioCategory < _audioCategories.Count && selectedAudioCategory >= 0)
                selectedAudioCategory--;
        }

        private List<AudioCategory> AudioCategoriesField(List<AudioCategory> _audioCategories)
        {
            // AudioCategory selector
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Audio Category");
            selectedAudioCategory = EditorGUILayout.Popup(selectedAudioCategory, audioRepository.FindCategoryNames().ToArray());

            if (ButtonAdd("Add Category"))
                AddCategory(ref _audioCategories);
            if (ButtonRemove("Remove Category"))
                RemoveCategory(ref _audioCategories);

            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();

            // Display editing field for the selected AudioCategory
            for (int aac = 0; aac < _audioCategories.Count; aac++)
                if (aac == selectedAudioCategory)
                    _audioCategories[aac] = AudioCategoryField(_audioCategories[aac]);

            // End
            return _audioCategories;
        }

        private AudioCategory AudioCategoryField(AudioCategory _audioCategory)
        {
            EditorGUI.indentLevel++;

            // Process
            EditorGUILayout.LabelField("Category name", EditorStyles.boldLabel);
            _audioCategory.categoryName = EditorGUILayout.TextField(_audioCategory.categoryName);

            EditorGUILayout.BeginHorizontal();
            _audioCategory.editorDisplayAssets = EditorGUILayout.Foldout(_audioCategory.editorDisplayAssets, new GUIContent("Audio Assets"), customFoldout);
            if (ButtonAdd("Add AudioAsset"))
            {
                _audioCategory.audioAssets.Add(new AudioAsset());
                _audioCategory.editorDisplayAssets = true;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel++;

            if (_audioCategory.editorDisplayAssets)
                _audioCategory.audioAssets = AudioAssetsField(_audioCategory.audioAssets);

            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;

            // End
            return _audioCategory;
        }

        private List<AudioAsset> AudioAssetsField(List<AudioAsset> _audioAssets)
        {
            for (int aa = 0; aa < _audioAssets.Count; aa++)
            {
                // Initialization
                AudioAsset audioAsset = _audioAssets[aa];
                // Start editing this audio asset
                bool deleteAudioAsset = false;
                bool positionIncrease = false;
                bool positionDecrease = false;

                // Color switch for higher lisibility
                GUI.backgroundColor = (aa % 2 == 0) ? pairColor : evenColor;

                // Hide / Display audio asset fields
                EditorGUILayout.BeginHorizontal();
                audioAsset.editorDisplay = EditorGUILayout.Foldout(audioAsset.editorDisplay, new GUIContent("Audio Asset n°" + aa.ToString()));

                Color currentGUIBackgroundColor = GUI.backgroundColor;
                
                audioAsset.wwiseEvent = WwiseEventDrawer.WwiseEventField(audioAsset.wwiseEvent, new Rect(position.xMin, position.yMax, position.width, position.height));

                EditorGUI.BeginDisabledGroup(!(aa + 1 < _audioAssets.Count));
                if (GUILayout.Button("▼", GUILayout.MaxWidth(30)))
                    positionIncrease = true;
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(!(aa - 1 >= 0));
                if (GUILayout.Button("▲", GUILayout.MaxWidth(30)))
                    positionDecrease = true;
                EditorGUI.EndDisabledGroup();

                GUI.backgroundColor = new Color(0.95f, 0.45f, 0.45f);
                if (GUILayout.Button("✘", GUILayout.MaxWidth(30)))
                    deleteAudioAsset = true;
                GUI.backgroundColor = currentGUIBackgroundColor;
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel++;

                // Edit an audio asset
                if (audioAsset.editorDisplay)
                {
                    // Process
                    EditorGUI.indentLevel++;

                    audioAsset.type = (Audio.AudioType)EditorGUILayout.EnumPopup("Audio Type", audioAsset.type);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Subtitle");
                    EditorGUILayout.LabelField("Subtitle Duration (s)", GUILayout.MaxWidth(200));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    audioAsset.subtitle = EditorGUILayout.TextArea(audioAsset.subtitle, GUILayout.MinHeight(40));
                    audioAsset.subtitleDuration = EditorGUILayout.Slider(audioAsset.subtitleDuration, 0, 20, GUILayout.MaxWidth(200));
                    EditorGUILayout.EndHorizontal();

                    audioAsset.editorNotes = EditorGUILayout.TextField("Dev Notes", audioAsset.editorNotes, GUILayout.MinHeight(10));
                    EditorGUILayout.Space();
                    EditorGUI.indentLevel--;

                    // End
                    _audioAssets[aa] = audioAsset;
                }

                // Options Button executions
                if (deleteAudioAsset)
                    _audioAssets.RemoveAt(aa);
                if (positionIncrease)
                {
                    AudioAsset tmp = _audioAssets[aa + 1];
                    _audioAssets[aa + 1] = _audioAssets[aa];
                    _audioAssets[aa] = tmp;
                }
                if (positionDecrease)
                {
                    AudioAsset tmp = _audioAssets[aa - 1];
                    _audioAssets[aa - 1] = _audioAssets[aa];
                    _audioAssets[aa] = tmp;
                }

                EditorGUI.indentLevel--;
                
                // End of editing this audio asset
            }
            GUI.backgroundColor = defaultGUIColor;

            // End
            return _audioAssets;
        }
        #endregion
    }
}
#endif