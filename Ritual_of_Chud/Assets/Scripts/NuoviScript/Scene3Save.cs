using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Scene3Save : MonoBehaviour {

	// Scene objects
	public Slider peloSlider;
	public Slider animSlider;
	public Slider blendSlider;
	public GameObject Loader;

	// User Info
	SavedDataManager savedDataManager;
	UserInfo userInfo;

	// Use this for initialization
	void Start () {
		savedDataManager = new SavedDataManager();
		userInfo = savedDataManager.LoadUserInfo();
		peloSlider.value = userInfo.pelo_value;
		animSlider.value = userInfo.anim_value;
		blendSlider.value = userInfo.blend_value*100;
		
	}

	public void SaveAracnoInfo(){
		// Update spider informations
		float Holder = ProtocolliLoaderClass.Nrep;
		string HolderEsperimento =ProtocolliLoaderClass.Expfolder;
		userInfo.pelo_value = peloSlider.value;
		userInfo.anim_value = animSlider.value;
		userInfo.blend_value = blendSlider.value/100f;
		userInfo.numero_ripetizioni = Holder;
		userInfo.nome_esperimento =HolderEsperimento;
		Debug.Log("valore nrep"+Holder);
		//userInfo.numero_ripetizioni= Holder.Nrep;

		// Update date of creation of the spider
		//cambio formato data e ora con orario inizio e fine
		string _data_attuale = DateTime.Now.ToString("HH:mm ddd d MMM  yyyy");
		userInfo._data_creazione_ragno = _data_attuale;

		// Save in .dat file
		savedDataManager.SaveUserInfo(userInfo);

		// Save in .json file
		savedDataManager.UpdatePersistentUserInfo(userInfo);

		Debug.Log("Updated data: "+savedDataManager.LoadUserInfo());
	}
}
