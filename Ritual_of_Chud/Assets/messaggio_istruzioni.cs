using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class messaggio_istruzioni : MonoBehaviour {
	public Text Istruzioni;
	public string stringa;
	SavedDataManager savedDataManager;
	UserInfo userInfo;
	public GameObject Loader;

	// Use this for initialization
	void Start () {
		var Holder = Loader.GetComponent<ProtocolliLoaderClass>().Dholder;
		stringa = Holder.istruzioni;
		Debug.Log("valore in  stringa"+stringa);
		Istruzioni.text=stringa;

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
