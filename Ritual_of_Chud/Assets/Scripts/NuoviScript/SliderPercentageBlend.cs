using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderPercentageBlend : MonoBehaviour {

	Text percentageText;

	// void Awake ()
	// {
	// 	percentageText = GetComponent<Text> ();
	// 	percentageText.text = Mathf.RoundToInt(value) + "%";		
	// }
	// Use this for initialization
	void Start () {
		percentageText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	public void textUpdate (float value) {

		percentageText.text = Mathf.RoundToInt(value) + "%";
	
	}
}
