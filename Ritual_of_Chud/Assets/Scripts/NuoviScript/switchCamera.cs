using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchCamera : MonoBehaviour {

public Camera[] cameras;
private int currentCameraIndex;

	void Start () {
		currentCameraIndex = 0;
	
         
         //Turn all cameras off, except the first default one
         for (int i=1; i<cameras.Length; i++) 
         {
             cameras[i].gameObject.SetActive(false);
         }
         
         //If any cameras were added to the controller, enable the first one
         if (cameras.Length>0)
         {
             cameras [0].gameObject.SetActive (true);

			}
	
	}
	
	// Update is called once per frame
	public void switchCameraView () {
		     //if (Input.GetKeyDown(KeyCode.C))
        // {
             currentCameraIndex ++;
             Debug.Log ("C button has been pressed. Switching to the next camera");
             if (currentCameraIndex < cameras.Length)
             {
                 cameras[currentCameraIndex-1].gameObject.SetActive(false);
                 cameras[currentCameraIndex].gameObject.SetActive(true);
                 Debug.Log ("Camera with name: " + cameras [currentCameraIndex].GetComponent<Camera>().name + ", is now enabled");
             }
             else
             {
                 cameras[currentCameraIndex-1].gameObject.SetActive(false);
                 currentCameraIndex = 0;
                 cameras[currentCameraIndex].gameObject.SetActive(true);
                 Debug.Log ("Camera with name: " + cameras [currentCameraIndex].GetComponent<Camera>().name + ", is now enabled");
             }
		
	}
}
