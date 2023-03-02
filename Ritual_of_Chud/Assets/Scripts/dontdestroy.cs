using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontdestroy : MonoBehaviour {
	
	 void Awake()
	{
	        DontDestroyOnLoad(this.gameObject);
            Debug.Log("Awake: " + this.gameObject);	
	} 
}
