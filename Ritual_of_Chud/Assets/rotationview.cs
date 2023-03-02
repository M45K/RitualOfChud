using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationview : MonoBehaviour {
	public  GameObject Loader;
	public float rot=2;



	// Use this for initialization


	//update for debug
	void Start () {
		var Holder = Loader.GetComponent<ProtocolliLoaderClass>().Dholder;
		rot = Holder.CameraPosition;
		if (rot == 1)
		{
			transform.Rotate(0,0,0);
			

		}
		if (rot == 2)
		{
			transform.Rotate(23,180,0);
			//back

		}
		if (rot == 3)
		{
			transform.Rotate(0,90,0);
			

		}
		if (rot == 4)
		{
			transform.Rotate(0,0,-90);
			

		}
		if (rot == 5)
		{
			transform.Rotate(-90,0,0);

		}


		
	}
	public void Ruota(){

	//transform.Rotate(a,b,0);		

	}
	// Update is called once per frame
	


}
