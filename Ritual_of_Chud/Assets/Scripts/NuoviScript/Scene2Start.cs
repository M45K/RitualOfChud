using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Scene2Start : MonoBehaviour {

	// Scene objects
	public Text nomeIF;
	public Text cognomeIF;

	// Personal data
	public string  _nome;
	public string  _cognome;
	public float  _ID;
	public string  _data_attuale;

	public float  pelo_value;
	public float  anim_value;
	public float  blend_value;

	// Use this for initialization
	void Start () {

		// Init text scene objects
		nomeIF.text = "";
		cognomeIF.text = "";
	}

	public void onStartClick(){
		if(!informazioniInseriteCorrettamente())
			return;

		aggiornaInformazioniLocali();
		caricaScenaConfigurazione();
	}

	// Controllo preventivo all'esecuzione delle operazioni successive.
	bool informazioniInseriteCorrettamente(){

		if(nomeIF.text == "" || cognomeIF.text == "")
			return false;

		return true;
	}

	// Aggiorna le variabili locali con le informazioni digitate dall'utente, assieme alla data locale
	void aggiornaInformazioniLocali(){

		_nome = nomeIF.text;
		_cognome = cognomeIF.text;
		//cambio formato data e ora con orario inizio e fine
		_data_attuale = DateTime.Now.ToString("HH:mm ddd d MMM  yyyy");

		// Salva nel file locale le informazioni dell'utente
		UserInfo userInfo = new UserInfo();
		userInfo._nome = nomeIF.text;
		userInfo._cognome = cognomeIF.text;
		userInfo._data_creazione_profilo = _data_attuale;
		userInfo._data_creazione_ragno = _data_attuale;
		userInfo._ID = 0;
		userInfo.blend_value = 0f;
		userInfo.anim_value = 0f;
		userInfo.pelo_value = 0f;

		SavedDataManager savedDataManager = new SavedDataManager();

		// Memorizza le informazioni persistentemente
		//savedDataManager.SavePersistentUserInfo(userInfo);

		// Memorizza le informazioni da portare nella scena successiva
		savedDataManager.SaveUserInfo(userInfo);
	}

	
	

	// Caricamento scenario di configurazione del ragno
	void caricaScenaConfigurazione()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        ScenesPassageManager scenesPassageManager = this.gameObject.AddComponent<ScenesPassageManager>();
		scenesPassageManager.caricaScenaConfigurazioneRagno();
	}
}
