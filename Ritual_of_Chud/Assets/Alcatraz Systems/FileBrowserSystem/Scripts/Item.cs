//AdvancedFileBrowser by Alcatraz. 
//Flight Dream Studio (C) 2015. 
//alcatrazalex@gmail.com
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ItemType
{
    File = 0, 
    Folder = 1,
    Disk = 2
}
public class Item : MonoBehaviour {
    public ItemType itemType = ItemType.File;
    public InputField renamer;
    public int id;
    void Start()
    {
        try
        {
            renamer = gameObject.GetComponentInChildren<InputField>();
            renamer.gameObject.SetActive(false);
        }
        catch
        {

        }
    }

    public void EndEditing()
    {
        try
        {
            renamer.text = "";
            renamer.gameObject.SetActive(false);
        }
        catch
        {

        }
    }
}
