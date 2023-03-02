using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blink : MonoBehaviour {
	public float interval;  //intervallo di frame in cui è visualizzata l'immagine safe
	public Image image;

	public float expo;   //quantitativo di esposizione immagine
	float a = 0;
	float b = 0;

	// Use this for initialization
	void Start () {
			
	}
	
	void  FixedUpdate () {
		if (a<= interval){
		a = a+1;
		image.GetComponent<Image>().color = new Color32(0,0,0,0);
		//Debug.Log("plus"+a);
		}
		else
		{
			if (b <= expo){

			image.GetComponent<Image>().color = new Color32(255,255,255,255);
        	//Debug.Log("end"+ b);
			b=b+1;
			}
			else{

				a= 0;
				b= 0;
			}
		}
			
	}
	
 }
