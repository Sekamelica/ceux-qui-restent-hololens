using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent
{
    public enum PortalCameraTime
    {
        Future,
        Past
    }

    public class PortalCamera : MonoBehaviour
    {
        #region Attributes
        public PortalCameraTime time;
        public Material renderTextureMaterial;
        private Transform mainCamera;
        private Camera cam;
        #endregion

        // Use this for initialization
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

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(mainCamera.position.x, mainCamera.position.y + ((time == PortalCameraTime.Future) ? 500 : -500), mainCamera.position.z);
            transform.rotation = mainCamera.rotation;
        }
    }

}
