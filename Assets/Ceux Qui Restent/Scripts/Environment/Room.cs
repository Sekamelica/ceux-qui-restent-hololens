using UnityEngine;
using UnityEngine.PostProcessing;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CeuxQuiRestent
{
    public class Room : MonoBehaviour
    {
        #region Attributes
        [SerializeField]
        private Transform linkablesLayouts;
        [SerializeField]
        private Transform linksRepository;
        [SerializeField]
        private PostProcessingProfile postProcessingProfile;
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            for (int c = linkablesLayouts.childCount - 1; c >= 0; c--)
                linkablesLayouts.GetChild(c).gameObject.SetActive(true);
        }
        #endregion

            #region Getters & Setters
        public Transform GetLinkablesLayouts()
        {
            return linkablesLayouts;
        }

        public Transform GetLinksRepository()
        {
            if (linksRepository != null)
                return linksRepository;
            else
                return transform;
        }

        public void UseRoomPostProcessing()
        {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PostProcessingBehaviour>().profile = postProcessingProfile;
        }
        #endregion

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = 24;
            Handles.Label(transform.position + Vector3.up * 5, gameObject.name, style);
        }
#endif
    }
}
