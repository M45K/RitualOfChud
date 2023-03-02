using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpscontroller : MonoBehaviour {

	// Use this for initialization
	void Start () {
	 Application.targetFrameRate = 300;
	 QualitySettings.vSyncCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
