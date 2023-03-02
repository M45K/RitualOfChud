using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderPercentage : MonoBehaviour {

	Text percentageText;
	public Slider slider;
	private float val;

	//  void OnEnable()
	//  {
	//  	percentageText = GetComponent<Text> ();
	//  	percentageText.text = Mathf.RoundToInt(value * 100) + "%";
	//  	Debug.Log("valore debug value " + value);
	//  }
	// Use this for initialization
	void Awake () {
		percentageText = GetComponent<Text> ();
		slider = GetComponent<Slider>();
		val = slider.value;
		percentageText.text = val + "%";
	}
	
	public void textUpdate (float value) {

		percentageText.text = Mathf.RoundToInt(value * 100) + "%";
	
	}
}
