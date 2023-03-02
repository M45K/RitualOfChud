using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Sliderstarthere : MonoBehaviour {

public Slider mSlider;
// Update is called once per frame
	void Awake()
	{
	mSlider.value = 1f;
	mSlider.maxValue = 1.0f;
	//
	}

}
