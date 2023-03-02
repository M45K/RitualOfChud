using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;   
using System;
using UnityEngine.UI;


[System.Serializable]
public class MyClass 
{
    public float expotime;
    public float interval;
    public float repetition;
    public string imagesafe;
}




public class UseData : MonoBehaviour
{

    string root = Environment.GetEnvironmentVariable("HOMEDRIVE");

    string path = Environment.GetEnvironmentVariable("HOMEPATH");

    public float expo = 10;   //quantitativo di esposizione immagine

    float a = 0;
    float r = 0;
    float b = 0;
    public float ripetizioni = 10;
    public float interval = 10;
    public Image immagine;
    public Sprite Sprt;

    public void Debbbb()
    {

        //INFO: Accedo al json del protocollo


        string custompath = root + path + "/Desktop/test.json";
        string protocolli = File.ReadAllText(custompath);
        var myObject = JsonUtility.FromJson<MyClass>(protocolli);

        //STEP: test print elemento json

        Debug.Log(myObject.interval);
        Debug.Log(myObject.imagesafe);
        Debug.Log(myObject.repetition);
        Debug.Log(myObject.expotime);

        //INFO:accedo al componente  che contiene le path delle immagini da esporre sotto soglia

        var UnderImage = GameObject.FindGameObjectWithTag("pippo").GetComponent<DemoManager>().lista;

        var url = UnderImage[1];
        Texture2D tex;
        tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        WWW www = new WWW(url);
        www.LoadImageIntoTexture(tex);


        var oggettodefault = GameObject.Find("overlay");

        immagine = oggettodefault.GetComponent<Image>();

        Sprt = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0, 0), 100.0f);

        ripetizioni = myObject.repetition;
        interval = myObject.interval;
        expo = myObject.expotime;
        //  InvokeRepeating("Ciclo", 1f, 0.1f);


    }
       public void Ciclo()
        {
          //  while (r <= ripetizioni)
          //  {

                if (a <= interval)
                {
                    Debug.Log("sto facendo ciclo a");
                    a = a + 1;
                    immagine.sprite = Sprt;
                }

                else if  (b <= expo)
                {
                    Debug.Log("sto facendo ciclo b");
                    immagine.sprite = null;
                    b = b + 1;
                }
                if (a > interval && b > expo)
                {
                    Debug.Log("reset");
                    a = 0;
                    b = 0;
                r = r + 1;
                }
            //}
       }
}

