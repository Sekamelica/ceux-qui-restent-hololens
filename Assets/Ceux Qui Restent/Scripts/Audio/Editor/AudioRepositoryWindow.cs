using System.Collections.Generic;
using CeuxQuiRestent.Audio;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

namespace CeuxQuiRestent.Tools
{
    public class AudioRepositoryWindow : EditorWindow
    {
        #region Attributes
        private AudioRepository audioRepository;

        private int selectedAudioAssetCategory = 0;

        private Color defaultGUIColor = new Color(0.5f, 0.5f, 0.5f);
        private Color pairColor = new Color(0.7f, 0.65f, 0.8f);
        private Color evenColor = new Color(0.8f, 0.65f, 0.7f);
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
            // Initialization
            defaultGUIColor = GUI.backgroundColor;
            audioRepository = EditorGUILayout.ObjectField("Audio Repository", audioRepository, typeof(AudioRepository), true) as AudioRepository;
            if (audioRepository != null)
            {
                EditorGUILayout.Space();
                List<AudioAssetCategory> audioAssetsCategories = audioRepository.GetAudioAssetsCategories();
                List<string> categoryNames = new List<string>();

                for (int aac = 0; aac < audioAssetsCategories.Count; aac++)
                    categoryNames.Add(audioAssetsCategories[aac].categoryName);
                
                selectedAudioAssetCategory = EditorGUILayout.Popup(selectedAudioAssetCategory, categoryNames.ToArray());

                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = new Color(0.95f, 0.95f, 0.45f);
                if (GUILayout.Button("Add Category", GUILayout.MaxWidth(250)))
                {
                    AudioAssetCategory newAudioAssetCategory = new AudioAssetCategory();
                    for (int aac = 0; aac < audioAssetsCategories.Count; aac++)
                        if (audioAssetsCategories[aac].categoryName == newAudioAssetCategory.categoryName)
                            newAudioAssetCategory.categoryName += "(Bis)";

                    audioAssetsCategories.Add(newAudioAssetCategory);
                    selectedAudioAssetCategory = audioAssetsCategories.Count - 1;
                }
                

                GUI.backgroundColor = new Color(0.95f, 0.45f, 0.45f);
                if (GUILayout.Button("Remove Category", GUILayout.MaxWidth(250)))
                {
                    audioAssetsCategories.RemoveAt(selectedAudioAssetCategory);
                    categoryNames.RemoveAt(selectedAudioAssetCategory);
                    while (selectedAudioAssetCategory < audioAssetsCategories.Count && selectedAudioAssetCategory >= 0)
                        selectedAudioAssetCategory--;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                GUI.backgroundColor = defaultGUIColor;

                for (int aac = 0; aac < audioAssetsCategories.Count; aac++)
                {
                    if (aac == selectedAudioAssetCategory)
                        audioAssetsCategories[aac] = audioAssetCategoryEditor(audioAssetsCategories[aac]);
                }
                

                // End
                audioRepository.SetAudioAssetsCategories(audioAssetsCategories);
            }
            
        }
        #endregion

        #region Methods
        private AudioAssetCategory audioAssetCategoryEditor(AudioAssetCategory _audioAssetCategory)
        {
            // Initialization
            AudioAssetCategory audioAssetCategory = _audioAssetCategory;

            // Process
            audioAssetCategory.categoryName = EditorGUILayout.TextField("Category name", audioAssetCategory.categoryName);
            audioAssetCategory.audioAssets = audioAssetsEditor(audioAssetCategory.audioAssets);

            // End
            return audioAssetCategory;
        }

        private List<AudioAsset> audioAssetsEditor(List<AudioAsset> _audioAssets)
        {
            // Initialization
            List<AudioAsset> audioAssets = _audioAssets;
            int delete = -99;

            // Process
            GUI.backgroundColor = new Color(0.95f, 0.95f, 0.45f);
            if (GUILayout.Button("Add AudioAsset", GUILayout.MaxWidth(250)))
                audioAssets.Add(new AudioAsset());
            EditorGUILayout.Space();
            for (int aa = 0; aa < audioAssets.Count; aa++)
            {
                GUI.backgroundColor = (aa % 2 == 0) ? pairColor : evenColor;

                EditorGUILayout.BeginHorizontal();
                audioAssets[aa].editorDisplay = EditorGUILayout.Foldout(audioAssets[aa].editorDisplay, new GUIContent("Audio Asset n°" + aa.ToString() + ((audioAssets[aa].WwiseEventName != null) ? " - " + audioAssets[aa].WwiseEventName : "")));
                GUI.backgroundColor = new Color(0.95f, 0.45f, 0.45f);
                if (GUILayout.Button("Remove AudioAsset", GUILayout.MaxWidth(250)))
                    delete = aa;
                GUI.backgroundColor = (aa % 2 == 0) ? pairColor : evenColor;
                EditorGUILayout.EndHorizontal();

                if (audioAssets[aa].editorDisplay)
                    audioAssets[aa] = audioAssetEditor(audioAssets[aa]);

                GUI.backgroundColor = defaultGUIColor;

            }
            if (delete != -99)
                audioAssets.RemoveAt(delete);
            
            // End
            return audioAssets;
        }

        private AudioAsset audioAssetEditor(AudioAsset _audioAsset)
        {
            // Initialization
            AudioAsset audioAsset = _audioAsset;

            // Process
            EditorGUI.indentLevel++;
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            audioAsset.WwiseEventName = EditorGUILayout.TextField("Wwise Event Name", audioAsset.WwiseEventName, GUILayout.MaxWidth(Screen.width * 0.35f));
            audioAsset.type = (Audio.AudioType)EditorGUILayout.EnumPopup("Audio Type", audioAsset.type, GUILayout.MaxWidth(Screen.width * 0.35f));
            audioAsset.editorNotes = (EditorGUILayout.TextField("Notes", audioAsset.editorNotes, GUILayout.MaxWidth(Screen.width * 0.3f)));
            EditorGUILayout.EndHorizontal();
            audioAsset.subtitleDuration = EditorGUILayout.FloatField("Subtitle duration", audioAsset.subtitleDuration);
            audioAsset.subtitle = EditorGUILayout.TextField("Subtitle", audioAsset.subtitle, GUILayout.MinHeight(30));
            EditorGUI.indentLevel--;

            // End
            return audioAsset;
        }
        #endregion
    }
}
#endif