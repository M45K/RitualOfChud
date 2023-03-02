//AdvancedFileBrowser by Alcatraz. 
//Flight Dream Studio (C) 2015. 
//alcatrazalex@gmail.com
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DemoManager : MonoBehaviour
{
    public List<string> addedFiles = new List<string>(); //Added files

    public string[] values;
    public char separ  ;
    public RectTransform scrollArea; //ScrollRect. For re-size when new path added
	public float lineWidth = 30f; //Increase scrollArea's height
    public string[] lista;
   void AddPath (string path)  //Receive adress
    {
        if (!addedFiles.Contains(path)) //Is path already added?
        {
            addedFiles.Add(path); //If not yet - path will be added
            scrollArea.sizeDelta = new Vector2(scrollArea.sizeDelta.x, scrollArea.sizeDelta.y + lineWidth); //Change size of the ScrollArea
            scrollArea.GetComponent<Text>().text += (addedFiles.Count > 1) ? "\n" + path : "" + path; //Add new path to Text component
			AdvancedFileBrowser.Done(); //alright.. then cast "Successful !"
        }
        else
        {
            AdvancedFileBrowser.AlreadyExists(); //otherwise cast a error
        }
    
    //INFO: associo le string a una variabile e le mappo su array utilizzando \n (a capo) come separatore

    var strg=  scrollArea.GetComponent<Text>().text;
    lista = strg.Split('\n');
    Debug.Log(lista[1]);
    
   }
}
