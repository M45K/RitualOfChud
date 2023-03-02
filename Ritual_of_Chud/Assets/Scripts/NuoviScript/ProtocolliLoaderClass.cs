using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;

//using BayatGames.Serialization.Formatters.Json;

public class ProtocolliLoaderClass :MonoBehaviour{

	public static float Nrep = 1;
	float count;
	string path;
	string filename ;
	public static string Expfolder ;
//	static float nextcounter = 1;
	public DataSet Dholder ;


	[Serializable]
 		public class DataSet
 		{
 			public float ProtName;
 			public float Step;
 			public float StepBack;
 			public float CameraPosition;
 			public float MinPelo;
 			public float MaxPelo;
 			public float StartPelo;
 			public float MinAnim;
 			public float MaxAnim;
 			public float StartAnim;
 			public float MinCorpo;
 			public float MaxCorpo;
			public float StartCorpo;
			public string istruzioni;
		 }
	
	
	void Start()

	{
	var contfilenamefolder =  GameObject.Find("FilenameFolder").GetComponent<ContFilenameFolder>();
	filename =contfilenamefolder.filename;
	Expfolder=contfilenamefolder.Expfolder;
	Debug.Log("nome file importato"+filename );
	
 
	//conto i file nella cartella
	path = Application.persistentDataPath +"/"+ Expfolder;
	var info = new DirectoryInfo(path);
	var fileInfo = info.GetFiles();
	
	
	count = fileInfo.Length;
	
		
		if(Nrep<=count)
		{
			
			
			string jsonString = File.ReadAllText(Application.persistentDataPath +"/"+ Expfolder +"/"+filename+Nrep+".json");
			DataSet contenitore = JsonUtility.FromJson<DataSet>(jsonString);
			Dholder = contenitore;
			
			Debug.Log("Valore MaxAnim"+ contenitore.MaxAnim);
			
		}	
			
			
			else {
			//Debug.Log("ciclo concluso");
			//Debug.Log("NRep"+Nrep);
			//Debug.Log("nextcounter"+nextcounter);
			
			
			
			//La scena a cui viene mandato finito esperimento
			SceneManager.LoadScene("Scena1-New-Carica");
			Nrep = 1;
			
			}

			

		}



	public  void TaskOnClick(){
					Nrep= Nrep + 1;
					SceneManager.LoadScene("Scena5");
					Debug.Log("carico nuova scena");
					

				}
	


 	





}

























































































































































// 	public float tupperware;
// public Array objects;

// public static class JsonHelper {

//     public static T[] FromJson<T>(string json) {
//         Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
//         return wrapper.Items;
//     }

//     public static string ToJson<T>(T[] array) {
//         Wrapper<T> wrapper = new Wrapper<T>();
//         wrapper.Items = array;
//         return UnityEngine.JsonUtility.ToJson(wrapper);
//     }

//     [Serializable]
//     private class Wrapper<T> {
//         public T[] Items;
//     }
// }


// // 	[Serializable]
// // 		public class DataProtocol{
// // 	[Serializable]
// // 		public struct DataSet
// // 		{
// // 			public float ProtName;
// // 			public float Step;
// // 			public float StepBack;
// // 			public int NRip;
// // 			public float MinPelo;
// // 			public float MaxPelo;
// // 			public float StartPelo;
// // 			public float MinAnim;
// // 			public float MaxAnim;
// // 			public float StartAnim;
// // 			public float MinCorpo;
// // 			public float MaxCorpo;
// // 			public float StartCorpo;


// // }
	
// // 	public object[] array;
	
	
// // 	}

// [Serializable]
// public class MonsterStats
// {

// public int id;
// public string name;
// public int[] baseStats;
// public int xpyield = 0;//0-4 for each stat
// public int[] evyield = new int[] { 0, 0, 0, 0, 0, 0 };
// public string[] moves;

// public int health;
// public int[] stats;
// public int[] evs;
// public int level;
// public int xp;


// }











// 	string filename = "protocollidata.json";
// //	public static ProtocolliData protocolliData{ get; private set; }

    



// void Start () {
		
// 	string jsonString = File.ReadAllText(Application.dataPath + "/" + filename);
// 	MonsterStats[] monsterData = JsonHelper.FromJson<MonsterStats>(jsonString);
//     Debug.Log(monsterData);
// 		//string jsonString = File.ReadAllText (Application.persistentDataPath + "/" + filename);
		
		
// 		//var pippo = JsonHelper.FromJson <DataProtocol> (jsonString);
		
		
		
// 		//DataProtocol[] Protocollo1=JsonHelper.getJsonArray<DataProtocol>(jsonString);
		
// 		//Debug.Log (pippo);
// 		//Debug.Log("Size of array before: " + mainObject.protocollidata.Length); // 1
// 		//Debug.Log("ProtName" + mainObject);
// 	}










// }
// // public void Deserialize ()
// // {
// // 	    //protocolliData = this;
// // 		//JsonFormatter formatter = new JsonFormatter();
// // }

// // }


