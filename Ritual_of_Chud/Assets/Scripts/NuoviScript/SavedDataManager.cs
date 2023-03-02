using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class SavedDataManager {

	public static string userInfoFileName = "userInfo.dat";
	public static string userInfoFilePath = Application.persistentDataPath+"/"+userInfoFileName;
	public static string currentIdKey = "currentID";

	// Persistent Info.
	public string userInfoPersistentPathFolder;
	public string userInfoSubjectDataPathFolder;
	public string userInfoScreenshotsPathFolder;
	public string userInfoMainFolder = "Chud";
	public string userInfoSubjectDataFolder = "SubjectData";
	public string userInfoScreenshotsFolder = "Screenshots";

	public SavedDataManager(){

		// Init paths
		string userInfoDrivePersistentPath = Environment.GetEnvironmentVariable("HOMEDRIVE");
		string userInfoHomePersistentPath = Environment.GetEnvironmentVariable("HOMEPATH");
		userInfoPersistentPathFolder = userInfoDrivePersistentPath+userInfoHomePersistentPath+Path.DirectorySeparatorChar+userInfoMainFolder;
		userInfoSubjectDataPathFolder = userInfoPersistentPathFolder + Path.DirectorySeparatorChar + userInfoSubjectDataFolder;
		userInfoScreenshotsPathFolder = userInfoPersistentPathFolder + Path.DirectorySeparatorChar + userInfoScreenshotsFolder;
	}

	// UserInfo Manager [BRIDGE FOR USER_INFO: .DAT save]
	public void SaveUserInfo(UserInfo dataToSave){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(userInfoFilePath);

		bf.Serialize(file, dataToSave);
		file.Close();
	}

	public UserInfo LoadUserInfo(){
		if(File.Exists(userInfoFilePath)){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(userInfoFilePath, FileMode.Open);
			UserInfo data = (UserInfo)bf.Deserialize(file);
			file.Close();

			return data;
		}
		return null;
	}

	// UserInfo Manager [SAVE/LOAD .JSON]
	public void SavePersistentUserInfo(UserInfo dataToSave){
		checkIfFolderExists();

		// Assign ID
		dataToSave._ID = generateID();

		// Convert data in json format
		string json_dataToSave = JsonUtility.ToJson(dataToSave);

		// Store info in Json file
		string fileName = dataToSave._ID+"_"+dataToSave._nome+"_"+dataToSave._cognome+"_numero ripetizione_"+dataToSave.numero_ripetizioni+"_nome esperimento_"+dataToSave.nome_esperimento+".json";
		string filePath = userInfoSubjectDataPathFolder + Path.DirectorySeparatorChar + fileName;
		File.WriteAllText(filePath,json_dataToSave);
	}

	public void UpdatePersistentUserInfo(UserInfo dataToSave){
		checkIfFolderExists();

		// Convert data in json format
		string json_dataToSave = JsonUtility.ToJson(dataToSave);

		// Store info in Json file
		string fileName = dataToSave._ID+"_"+dataToSave._nome+"_"+dataToSave._cognome+"_numero ripetizione_"+dataToSave.numero_ripetizioni+"_nome esperimento_"+dataToSave.nome_esperimento+".json";
		string filePath = userInfoSubjectDataPathFolder + Path.DirectorySeparatorChar + fileName;
		File.WriteAllText(filePath,json_dataToSave);
	}


	public UserInfo[] LoadPersistentUserInfo(){
		checkIfFolderExists();

		// Load data from Json
		UserInfo[] savedUserInfo;
		string[] savedUserInfoPaths;
		savedUserInfoPaths = Directory.GetFiles(userInfoSubjectDataPathFolder);
		savedUserInfo = new UserInfo[savedUserInfoPaths.Length];
		for(int i = 0; i<savedUserInfoPaths.Length; i++){
			string info = File.ReadAllText(savedUserInfoPaths[i]);
			savedUserInfo[i] = JsonUtility.FromJson<UserInfo>(info);
		}
		
		return savedUserInfo;
	}

	int generateID(){
		int id = getCurrentAvailableId();
		int nextID = id+1;
		storeCurrentAvailableId(nextID);
		return id;
	}
	
	void storeCurrentAvailableId(int currentID){
		PlayerPrefs.SetInt(currentIdKey,currentID);
	}

	int getCurrentAvailableId(){
		return PlayerPrefs.GetInt(currentIdKey);
	}

	void checkIfFolderExists(){

		Debug.Log(userInfoPersistentPathFolder);

		if(!Directory.Exists(userInfoPersistentPathFolder))
			Directory.CreateDirectory(userInfoPersistentPathFolder);

		if(!Directory.Exists(userInfoSubjectDataPathFolder))
			Directory.CreateDirectory(userInfoSubjectDataPathFolder);
		
		if(!Directory.Exists(userInfoScreenshotsPathFolder))
			Directory.CreateDirectory(userInfoScreenshotsPathFolder);
	}
}