using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BPiuCorpo : MonoBehaviour {
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
	step = Holder.Step*100;
	stepback = Holder.StepBack*100;
	minslider = Holder.MaxCorpo*100;
	sliderstart = Holder.StartCorpo*100;



	//var tempStep= GameObject.Find("Save_Button").GetComponent<ProtocolliLoaderClass>();
	//float step1= tempStep.ProtocolliLoaderClass.DataSet;
	//step = step1.Step;
	//Debug.Log("stepimportato"+TempLoader.Step);
	//Debug.Log("nuovostep_Pelos"+stepback);
	
	//yetAnotherScript = otherGameObject.GetComponent<YetAnotherScript>();

	slider.value = sliderstart;


	//button.onClick.AddListener(TaskOnClick);
	 
	
	}
	 public void TaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked
		
        Debug.Log("You have clicked the button!"+slider.value);
		slider.value = slider.value + step;
		if (slider.value >= minslider)
		{
		slider.value = slider.value - stepback;

		Debug.Log("Lo_esegui?");
		}
    }
	// Update is called once per frame
	void Update () {
		
	}

}



