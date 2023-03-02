using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContFilenameFolder : MonoBehaviour {
public string filename = "testfilename";
public string Expfolder = "testfolder" ;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void ConvalidaNome(string usrinputfilename)
    {
        filename = usrinputfilename;
		Debug.Log(usrinputfilename);
    }

	public void ConvalidaFolder(string usrinputfolder)
    {
        Expfolder = usrinputfolder;
		Debug.Log(usrinputfolder);
    }
}
