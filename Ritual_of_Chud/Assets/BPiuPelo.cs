﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BPiuPelo : MonoBehaviour {
    public Slider slider;
	public Button button;
	public float step;
	public float stepback;
	public float minslider;
	public float sliderstart;
	public GameObject Loader;
	// Use this for initialization
	
	
		
	void Start () {
	//GameObject Loader = GameObject.Find("Save_Button");
	var Holder = Loader.GetComponent<ProtocolliLoaderClass>().Dholder;
	step = Holder.Step;
	stepback = Holder.StepBack;
	minslider = Holder.MaxPelo;
	sliderstart = Holder.StartPelo;



	//var tempStep= GameObject.Find("Save_Button").GetComponent<ProtocolliLoaderClass>();
	//float step1= tempStep.ProtocolliLoaderClass.DataSet;
	//step = step1.Step;
	//Debug.Log("stepimportato"+TempLoader.Step);
	//Debug.Log("nuovostep_Pelos"+stepback);
	
	//yetAnotherScript = otherGameObject.GetComponent<YetAnotherScript>();

	slider.value = slider.value+sliderstart;
	//Debug.Log("valore iniziale"+sliderstart+ slider.value);


	//button.onClick.AddListener(TaskOnClick);
	 
	
	}
	 public void TaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked
		
        Debug.Log("slidervalue nel ciclo"+slider.value);
		slider.value = slider.value + step;
		if (slider.value >= minslider)
		{
		slider.value = slider.value - stepback;

		//Debug.Log("Lo_esegui?");
		}
    }
	// Update is called once per frame
	void Update () {
		
	}

}


