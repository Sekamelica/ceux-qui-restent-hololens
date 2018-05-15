using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent
{
    public class Room : MonoBehaviour
    {
        #region Attributes
        [SerializeField]
        private Transform linksRepository;
        [SerializeField]
        private Transform linkablesLayouts;
        public GameObject roomMesh;
        #endregion

        #region MonoBehaviour Methods
        #endregion

        #region Getters & Setters
        public Transform GetLinksRepository()
        {
            if (linksRepository != null)
                return linksRepository;
            else
                return transform;
        }

        public Transform GetLinkablesLayouts()
        {
            return linkablesLayouts;
        }
        #endregion
    }
}
