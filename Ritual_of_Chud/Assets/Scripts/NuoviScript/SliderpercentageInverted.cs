using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderpercentageInverted : MonoBehaviour {

	Text percentageText;

	// Use this for initialization
	void Start () {
		percentageText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	public void textUpdate (float value) {

		percentageText.text = Mathf.RoundToInt((1-value)*100) + "%";
	
	}
}
