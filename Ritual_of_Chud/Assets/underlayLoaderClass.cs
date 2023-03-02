using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.IO.Ports;
using System;
using System.Threading;

public class underlayLoaderClass : MonoBehaviour
{
    public static SerialPort sp = new SerialPort("COM3", 9600);
    public string message2;
    float timePassed = 0.0f;


    public static float Nrep = 1;
    float count;
    string path;
    string filename;
    public static string Expfolder;

    public GameObject triggerSafe;
    public GameObject triggerInit;
    //	static float nextcounter = 1;
    public DataUnder Dholder;
    public Image image;
    public Image image1;
    public Image crocino;
    public Sprite safer;

    [Serializable]
    public class DataUnder

    {
        public string init;
        public string ImgSafe;
        public float TimeSafe;
        public float IntSafe;
        public string ImgDanger;
        public float TimeDanger;
        public float IntDanger;
    }


    private void Start()
    {
        OpenConnection();

        var contfilenamefolder = GameObject.Find("FilenameFolder").GetComponent<ContFilenameFolder>();

        filename = contfilenamefolder.filename;

        Expfolder = contfilenamefolder.Expfolder;

        path = Application.persistentDataPath + "/" + Expfolder;

        var info = new DirectoryInfo(path);

        var fileInfo = info.GetFiles();


        count = fileInfo.Length;

        StartCoroutine(Expotime());


    }

    /// <summary>
    /// SETUP ARDUINO////////////////
    /// </summary>

    public void OpenConnection()
    {
        if (sp != null)
        {
            if (sp.IsOpen)
            {
           
                print("it was already open!");
            }
            else
            {
                sp.Open();  // opens the connection
                sp.ReadTimeout = 16;  // sets the timeout value before reporting error
                print("Port Opened!");
                //		message = "Port Opened!";
            }
        }
        else
        {
            if (sp.IsOpen)
            {
                print("Port is already open");
            }
            else
            {
                print("Port == null");
            }
        }
    }

    void OnApplicationQuit()
    {
        sp.Close();
    }



    public static void sendGreen()
    {
        sp.Write("g");

        Debug.Log("printo su led");
        //sp.Write("\n");
    }

    public static void sendReset()
    {
        sp.Write("a");

        Debug.Log("printo su led");
        //sp.Write("\n");
    }



    IEnumerator Expotime()
    {
        for (int Nrep = 1; Nrep <= count; Nrep++)
        {

            //GESTIONE JSON
            var contfilenamefolder = GameObject.Find("FilenameFolder").GetComponent<ContFilenameFolder>();

            filename = contfilenamefolder.filename;

            Expfolder = contfilenamefolder.Expfolder;

            path = Application.persistentDataPath + "/" + Expfolder;

            var info = new DirectoryInfo(path);

            var fileInfo = info.GetFiles();

            var jsonObj = File.ReadAllText(Application.persistentDataPath + "/" + Expfolder + "/" + filename + Nrep + ".json");

            DataUnder contenitore = JsonUtility.FromJson<DataUnder>(jsonObj);




            //usare 3 piani uno con init e usare stessa procedura

            crocino.GetComponent<Image>().material.mainTexture = LoadPNG(contenitore.init);

            
            
            //ASSEGNO IMMAGINE SAFE

            image.GetComponent<Image>().material.mainTexture = LoadPNG(contenitore.ImgSafe);

            
            
            //ASSEGNO IMMAGINE DANGER
            image1.GetComponent<Image>().material.mainTexture = LoadPNG(contenitore.ImgDanger);

            yield return new WaitForSeconds(contenitore.IntSafe);

            triggerSafe.transform.gameObject.SetActive(false);
            triggerInit.transform.gameObject.SetActive(false);
            
            
            //TEMPO ESPOSIZIONE SAFE
           // yield return new WaitForSeconds(contenitore.TimeSafe);


            
            //MESSAGGI ARDUINO
            sendGreen();
            sendReset();

            Debug.Log("fine safe " + "\n" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());

            //TEMPO ESPOSIZIONE DANGER
            yield return new WaitForSeconds(contenitore.TimeDanger);

            triggerSafe.transform.gameObject.SetActive(true);

            Debug.Log("fine danger " + "\n" + DateTime.Now.ToString() + " " + DateTime.Now.Millisecond.ToString());

            //TEMPO TRA I CICLI

            yield return new WaitForSeconds(contenitore.IntDanger);

            triggerSafe.transform.gameObject.SetActive(true);

            //LOG  REPETITION
            Debug.Log(Nrep);
        }
        Nrep = 0;
        SceneManager.LoadScene("Scena1-New-Carica", LoadSceneMode.Single);
    }

    //CAraica immagine e converte in texture 2d


    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;

        byte[] fileData;

        fileData = File.ReadAllBytes(filePath);

        tex = new Texture2D(2, 2);

        tex.LoadImage(fileData);

        return tex;
    }
}
