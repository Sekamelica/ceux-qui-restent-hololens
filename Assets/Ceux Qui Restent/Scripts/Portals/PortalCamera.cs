using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent.Portals
{
    public class PortalCamera : MonoBehaviour
    {
        #region Attributes
        public PortalDestination time;
        public Material renderTextureMaterial;
        private Transform mainCamera;
        private Camera cam;
        #endregion

        #region MonoBehaviour Methods
        void Start()
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
            cam = GetComponent<Camera>();
            if (cam.targetTexture != null)
            {
                cam.targetTexture.Release();
            }
            cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
            renderTextureMaterial.mainTexture = cam.targetTexture;
        }
        
        void Update()
        {
            transform.position = new Vector3(mainCamera.position.x, mainCamera.position.y + ((time == PortalDestination.Future) ? 500 : -500), mainCamera.position.z);
            transform.rotation = mainCamera.rotation;
        }
        #endregion
    }

}
