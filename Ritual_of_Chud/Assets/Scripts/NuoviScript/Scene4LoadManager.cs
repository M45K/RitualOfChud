using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Scene4LoadManager : MonoBehaviour {

	// Private parameters
	SavedDataManager savedDataManager;
	UserInfo[] loadedUserInfo;
	List<UserInfo> queryUserInfo;
	GameObject[] subjectDataListElements;

	// Public objects
	public GameObject scrolldownBodyList;
	public GameObject userInfoListElement;
	public InputField inputFieldName;
	public InputField inputFieldSurname;

	// Search parameters
	string nameSearch;
	string surnameSearch;

	// Use this for initialization
	void Start () {

		savedDataManager = new SavedDataManager();
		loadFilesFromFolder(); 
		updateInterface();

		nameSearch = "";
		surnameSearch = "";

		// Disable "CaricaButton".
		GameObject.Find("CaricaButton").GetComponent<Button>().enabled = false;
	}
	
	public void loadFilesFromFolder(){

		loadedUserInfo = savedDataManager.LoadPersistentUserInfo();
		queryUserInfo = new List<UserInfo>();
		for(int i = 0; i<loadedUserInfo.Length; i++)
			queryUserInfo.Add(loadedUserInfo[i]);
		subjectDataListElements = new GameObject[loadedUserInfo.Length];
	}

	public void updateInterface(){
		// Remove existing elements from interface.
		foreach (Transform child in scrolldownBodyList.transform) {
     		GameObject.Destroy(child.gameObject);
 		}

		// Create a prefab for each element found in the SubjectData folder.
		for(int i = 0; i<queryUserInfo.Count; i++){
			subjectDataListElements[i] = Instantiate(userInfoListElement) as GameObject;

			// Add element to list
			subjectDataListElements[i].transform.parent = scrolldownBodyList.transform;

			// Configure element
			subjectDataListElements[i].name = queryUserInfo[i]._ID+"_"+queryUserInfo[i]._nome+"_"+queryUserInfo[i]._cognome;
			Scene4ScrolldownElementManager scrollScript = subjectDataListElements[i].GetComponent<Scene4ScrolldownElementManager>();
			scrollScript.setUserInfo(queryUserInfo[i]);
		}
	}

	public void cercaButton(){

		// Disable "CaricaButton".
		GameObject.Find("CaricaButton").GetComponent<Button>().enabled = false;

		// Get search type and field
		nameSearch = inputFieldName.text;
		surnameSearch = inputFieldSurname.text;
		// Search with respect to name and surname
		queryUserInfo.Clear();
		queryUserInfo = new List<UserInfo>();
		for(int i = 0; i<loadedUserInfo.Length; i++){
			if(loadedUserInfo[i]._nome.Contains(nameSearch) && loadedUserInfo[i]._cognome.Contains(surnameSearch))
				queryUserInfo.Add(loadedUserInfo[i]);
		}

		updateInterface();
	}
}
