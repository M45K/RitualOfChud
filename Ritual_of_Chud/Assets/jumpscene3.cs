using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class jumpscene3 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void JumpScene3 ()

	{

		SceneManager.LoadScene ( "Scena5" );
	}


    public void JumpSceneunderlay()

    {

        SceneManager.LoadScene("underlayRender");
    }

}
