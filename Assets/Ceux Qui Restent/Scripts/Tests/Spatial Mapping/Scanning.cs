using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent
{
    public class Scanning : MonoBehaviour
    {
        #region Attributes
        // Public
        public Transform scanner;
        public Material scanningMaterial;

        // Private
        private Transform spatialMappingMeshesWrapper;
        private Material spatialMappingDefaultMaterial;

        private bool scan_active = false;
        private bool scan_done = false;
        #endregion

        #region MonoBehaviour Methods
        // Use this for initialization
        void Start()
        {
            spatialMappingMeshesWrapper = GameObject.FindGameObjectWithTag("SpatialMappingManager").transform;
            spatialMappingDefaultMaterial = spatialMappingMeshesWrapper.gameObject.GetComponent<HoloToolkit.Unity.SpatialMapping.SpatialMappingManager>().SurfaceMaterial;
        }

        // Update is called once per frame
        void Update()
        {
            if (scan_active)
            {
                if (!scan_done)
                {

                }
            }
        }
        #endregion

        #region Methods
        public void ActiveScan()
        {
            scan_active = true;
            scanner.gameObject.SetActive(true);
            for (int c = 0; c < spatialMappingMeshesWrapper.childCount; c++)
            {
                GameObject child = spatialMappingMeshesWrapper.GetChild(c).gameObject;
                child.GetComponent<MeshRenderer>().material = scanningMaterial;
            }
        }

        public void FinishScan()
        {
            scan_done = true;
            for (int c = 0; c < spatialMappingMeshesWrapper.childCount; c++)
            {
                GameObject child = spatialMappingMeshesWrapper.GetChild(c).gameObject;
                child.GetComponent<MeshRenderer>().material = spatialMappingDefaultMaterial;
            }
            scanner.gameObject.SetActive(false);
            scan_active = false;
        }
        #endregion
    }

}
