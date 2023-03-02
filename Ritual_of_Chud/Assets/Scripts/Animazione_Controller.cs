using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class Animazione_Controller : MonoBehaviour {

public Slider slider;
public Animator animator;
public static float getlayerweightanim;
public GameObject Loader;
public float SliderAnimStart;


 void Start()
	{
		var Holder = Loader.GetComponent<ProtocolliLoaderClass>().Dholder;
		SliderAnimStart =Holder.StartAnim ;
		slider.value = SliderAnimStart;
    }
void Update () {
	animator = GetComponent<Animator>();
	animator.SetLayerWeight(1, slider.value);
	getlayerweightanim = slider.value;
	//Debug.Log("!animazione-valore-layer1  "+ slider.value);
	//print(slider.value);
	//animator.SetLayerWeight(0,(1f-slider.value));
	}
}

