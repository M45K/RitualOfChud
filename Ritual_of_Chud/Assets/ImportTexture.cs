using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleFileBrowser;

public class ImportTexture : MonoBehaviour {
public string path;
public string userInfoPersistentPathFolder;
	public string userInfoSubjectDataPathFolder;
	public string userInfoScreenshotsPathFolder;
	public string userInfoMainFolder = "Chud";
	public string userInfoSubjectDataFolder = "SubjectData";
	public string userInfoScreenshotsFolder = "Screenshots";
	public GameObject prefab;
	public string nome;
	//public Sprite test;

	// Use this for initialization
	void Start () {
		//setup path
		string userInfoDrivePersistentPath = Environment.GetEnvironmentVariable("HOMEDRIVE");
		string userInfoHomePersistentPath = Environment.GetEnvironmentVariable("HOMEPATH");
		userInfoPersistentPathFolder = userInfoDrivePersistentPath+userInfoHomePersistentPath+Path.DirectorySeparatorChar+userInfoMainFolder;
		userInfoSubjectDataPathFolder = userInfoPersistentPathFolder + Path.DirectorySeparatorChar + userInfoSubjectDataFolder;
		userInfoScreenshotsPathFolder = userInfoPersistentPathFolder + Path.DirectorySeparatorChar + userInfoScreenshotsFolder;
		var nomeparziale = ("*" + nome + "*.*");
		GameObject initialImage;
				//read file	
				var a = 0;
				foreach (string file  in Directory.GetFiles(	userInfoScreenshotsPathFolder,nomeparziale, SearchOption.AllDirectories)) 
					{	
						//load files
						var fileName =file;
						var bytes = File.ReadAllBytes(fileName );
						var texture = new Texture2D( 73, 73 );
						texture.LoadImage( bytes );
					
						Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width / 2, texture.height / 2));
	

						//generate object and assign texture
						
						initialImage=(GameObject)Instantiate(prefab,transform);


       					initialImage.GetComponent<Image>().overrideSprite = sprite; 
						
					
					
						Debug.Log (file);
							
					}
		}
	// Update is called once per frame
	void Update () {
		
	}
}
