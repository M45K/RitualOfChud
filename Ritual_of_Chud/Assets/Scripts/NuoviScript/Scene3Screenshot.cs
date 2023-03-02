using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Scene3Screenshot : MonoBehaviour {

	Texture2D screenCap;
	bool takeScreenshotFromNextFrame = false;
	UserInfo userInfo;

	// Parameters
	public int screenshotImageWidth;
	public int screenshotImageHeight;

	// Cameras
	public GameObject uiCamera;
	Camera myCamera;

	// Use this for initialization
	void Start () {

		// Get user info to save properly the screenshot file.
		SavedDataManager savedDataManager = new SavedDataManager();
		userInfo = savedDataManager.LoadUserInfo();

		// Get camera from which obtain the screenshot
		myCamera = GetComponent<Camera>();

		// Initialize variables
		takeScreenshotFromNextFrame = false;
	}

	public void takeScreenshot(){
		
		myCamera.targetTexture = RenderTexture.GetTemporary(screenshotImageWidth, screenshotImageHeight);
		takeScreenshotFromNextFrame = true;
	}

	private void OnPostRender(){
		if(takeScreenshotFromNextFrame){
			takeScreenshotFromNextFrame = false;
			
			RenderTexture renderTexture = myCamera.targetTexture;
			screenCap = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
			Rect rect = new Rect(0,0,renderTexture.width, renderTexture.height);
			screenCap.ReadPixels(rect, 0, 0);
			
			byte[] byteArray = screenCap.EncodeToPNG();

			SavedDataManager savedDataManager = new SavedDataManager();
			string screenshotsFullPath = savedDataManager.userInfoPersistentPathFolder + Path.DirectorySeparatorChar + savedDataManager.userInfoScreenshotsFolder;
			string fileName = userInfo._ID+"_"+userInfo._nome+"_"+userInfo._cognome+"_numero ripetizione_"+userInfo.numero_ripetizioni+"_nome esperimento_"+userInfo.nome_esperimento+".png";
			File.WriteAllBytes(screenshotsFullPath+ Path.DirectorySeparatorChar +fileName, byteArray);

			RenderTexture.ReleaseTemporary(renderTexture);
			myCamera.targetTexture = null;
			
		}
	}

}
