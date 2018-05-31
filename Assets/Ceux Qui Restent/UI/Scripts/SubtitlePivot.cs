using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CeuxQuiRestent.UI
{
    public class SubtitlePivot : MonoBehaviour
    {
        #region Attributes
        public float easePosition;
        public float easeRotation;

        private Transform holoCamera;
        #endregion

        #region MonoBehaviour Methods
        // Use this for initialization
        void Start()
        {
            holoCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        }

        // Update is called once per frame
        void Update()
        {
            //Vector3 desiredPosition = Vector3.Lerp(transform.position, holoCamera.position, easePosition);
            //Quaternion desiredRotation = Quaternion.Lerp(transform.rotation, holoCamera.rotation, easeRotation);
            //transform.SetPositionAndRotation(desiredPosition, desiredRotation);
            transform.position = holoCamera.position;
            transform.rotation = holoCamera.rotation;
        }
        #endregion
    }
}
