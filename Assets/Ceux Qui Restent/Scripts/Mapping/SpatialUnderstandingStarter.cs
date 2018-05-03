using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class SpatialUnderstandingStarter : MonoBehaviour {

    public SpatialUnderstanding su;

	// Use this for initialization
	void Start () {
        su.RequestBeginScanning();		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
