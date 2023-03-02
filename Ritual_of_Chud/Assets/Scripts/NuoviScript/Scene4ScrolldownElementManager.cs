using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Scene4ScrolldownElementManager : MonoBehaviour {

	// Prefab elements
	public Text textID;
	public Text textNome;
	public Text textCognome;
	public Text textData;

	//tentativo di mettere anche il num ripetizioni in cerca
	// public Text textRipetizioni;
	public Button buttonSeleziona;

	// User informations
	SavedDataManager savedDataManager;
	UserInfo userInfo;

	// Screenshot info
	string screenshotPath;

	// Use this for initialization
	void Start () {
	}
	
	public UserInfo getUserInfo(){
		return userInfo;
	}

	public string getScreenshotPath(){
		return screenshotPath;
	}

	public void setUserInfo(UserInfo userInfo){
		this.userInfo = userInfo;

		updateInterface();
		updateScreenshotInfo();
	}

	public void updateInterface(){
		textID.text = userInfo._ID.ToString();
		
		//tentativo mettere numero ripetizioni//textRipetizioni.text = userInfo.numero_ripetizioni.ToString();
		
		textNome.text = userInfo._nome;
		textCognome.text = userInfo._cognome;
		textData.text = userInfo._data_creazione_ragno;
	}

	public void updateScreenshotInfo(){
		savedDataManager = new SavedDataManager();
		string screenshotFileName = userInfo._ID+"_"+userInfo._nome+"_"+userInfo._cognome+".png";
		screenshotPath = savedDataManager.userInfoScreenshotsPathFolder + Path.DirectorySeparatorChar + screenshotFileName;
	}

	public void selezionaProfilo(){
		// Load Spider Image
		Sprite spiderPreviewSprite = LoadNewSprite(screenshotPath);
		GameObject spiderPreviewImage = GameObject.Find("SpiderPreviewImage");
		spiderPreviewImage.GetComponent<Image>().enabled = true;
		spiderPreviewImage.GetComponent<Image>().sprite = spiderPreviewSprite;

		// Store the selected profile info
		savedDataManager.SaveUserInfo(userInfo);

		// Enable "CaricaButton".
		GameObject.Find("CaricaButton").GetComponent<Button>().enabled = true;
	}

	public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f) {
   
     // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
     
     Sprite NewSprite;
     Texture2D SpriteTexture = LoadTexture(FilePath);
     NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height),new Vector2(0,0), PixelsPerUnit);
 
     return NewSprite;
   }
 
   public Texture2D LoadTexture(string FilePath) {
 
     // Load a PNG or JPG file from disk to a Texture2D
     // Returns null if load fails
 
     Texture2D Tex2D;
     byte[] FileData;
 
     if (File.Exists(FilePath)){
       FileData = File.ReadAllBytes(FilePath);
       Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
       if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
         return Tex2D;                 // If data = readable -> return texture
     }  
     return null;                     // Return null if load failed
   }
}
