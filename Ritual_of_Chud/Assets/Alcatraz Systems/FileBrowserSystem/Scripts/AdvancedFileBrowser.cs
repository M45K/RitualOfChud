//AdvancedFileBrowser by Alcatraz. 
//Flight Dream Studio (C) 2015. 
//alcatrazalex@gmail.com
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;

public enum StartFolder
{
    FirstDisk = 0,
    Documents = 1,
    Desktop = 2,
    LastOpened = 3
}

[System.Serializable]
public class Disk
{
    public string name, path;
    public Sprite image;
    public Text text;
    public Image icon;
    public Item it;
    public bool isBookMark;
}

[System.Serializable]
public class FileItem
{
    public string fName, fPath, fFormat;
    public Image icon;
    public Text nameObject;
    public bool imageLoaded;
    public FileItem(string name, string format, string path)
    {
        fFormat = format;
        fName = name;
        fPath = path;
    }

}

[System.Serializable]
public class FolderItem
{
    public string fName, fPath;
    public Image icon;
    public Text nameObject;
    public RectTransform obj;
    public FolderItem(string name, string path)
    {
        fName = name;
        fPath = path;
    }

}

[System.Serializable]
public class FileFormat
{
    public string formatName;
    public Sprite icon;
    public bool isImage = false, showInInspector = true;
}


public class AdvancedFileBrowser : MonoBehaviour
{
    public Button pasteButton, pasteButton2;
    public bool workSpaceEntered { get; set; }
    public bool currItemEntered { get; set; }
    public bool darkSkin = true, dragFilesAndFolders = true, settings = true, showFormats = true, resetSelection = true, showFilters = true, hideAtStart = true, useFileFilter = false, renameFiles = true;
    public List<string> formatFilter = new List<string>();
    public bool showHiddenFiles, showHiddenFolders;
    public StartFolder startFolder = StartFolder.Desktop;
    public int filesInLine = 8;
    public GameObject errorMessage, notFound, confirmRemove, dragFailed, fileOrFolderExists, dirMissed;
    public Text deleteFileName;
    public List<Disk> disks = new List<Disk>();
    public List<FileItem> files = new List<FileItem>();
    public List<FileFormat> formats = new List<FileFormat>();
    public List<FolderItem> folders = new List<FolderItem>();
	public GameObject myWindow, createPopupMenu, fileOrFolderPopupMenu, loading;
    public float disksButtonOffset = 20f, fileButtonOffset = 40f;
	public RectTransform diskItem, disksArea, fileItem, fileArea;
    public Sprite desktopIcon, documentsIcon, baseDiskIcon, folderIcon;
    public bool addDesktop = true, addDocumets = true;
    public InputField upperAdressFiled, selectLine;
    public InputField search;
    public FileItem selectedFileInstance;
    public GameObject donePanel, alreadyExists, receiver, deleteBookmark;
    public Image favourite;
    public Text placeholderLine;
    public Sprite addToFavourite, deleteFromFavourite, favouriteIcon;
    public bool loadImagePreview = true, loadPreviewForSelected = true, removePreviousSelectedPreview = true, doubleClickToOpenFiles = true, doubleClickToOpenFolders = true;
    public float doubleClickDelayBetweenClicks = 0.3f, popupMenuOffset = -12f;
    public Color bgColor = Color.blue;
    public float dragHoldTime = 18f;
    public string copyPastPath, copyName;
    public bool closePanelAfterButtonEvent = true, savePathAfterButtonEvent = true, showFormat = true;
    public bool owerWriteFile = true, isFile;

    Image prevBg;
    string currBookmark;
    float currRTimer = 0f;
    bool isEditing, isDragging;
    Item editObj, workObj;
    Vector2 pointerPos;
    Canvas canvas;
    float clickTimer;
    FolderItem currentFolder;
    string appPath;
    bool loaded, cutFILE;
    string savedPath, lastPath;
    List<string> history = new List<string>();
    RectTransform lastDisk, lastFile;
    float t;
    GameObject tempDrag;
    bool canDrag;
    Button cutted;

    public FileItem SelectedFile
    {
        get { 
            return selectedFileInstance; 
        }
        set { 
            this.selectedFileInstance = value;  
        }
    }

    public FolderItem SelectedFolder
    {
        get
        {
            return currentFolder;
        }
        set
        {
            currentFolder = value;
        }
    }


    public void SavePath(bool cut)
    {
        temp = workObj;

        if (cutted != null)
        {
            ColorBlock c = cutted.colors;
            c.normalColor = new Color(c.normalColor.r, c.normalColor.g, c.normalColor.b, 1f);
            c.highlightedColor = new Color(c.highlightedColor.r, c.highlightedColor.g, c.highlightedColor.b, 1f);
            c.pressedColor = new Color(c.pressedColor.r, c.pressedColor.g, c.pressedColor.b, 1f);
            c.disabledColor = new Color(c.disabledColor.r, c.disabledColor.g, c.disabledColor.b, 1f);
            cutted.colors = c;
        }
        cutFILE = cut;
        if (cut)
        {
            cutted = temp.gameObject.GetComponent<Button>();
            ColorBlock c = cutted.colors;
            c.normalColor = new Color(c.normalColor.r, c.normalColor.g, c.normalColor.b, c.normalColor.a/2f);
            c.highlightedColor = new Color(c.highlightedColor.r, c.highlightedColor.g, c.highlightedColor.b, c.highlightedColor.a / 2f);
            c.pressedColor = new Color(c.pressedColor.r, c.pressedColor.g, c.pressedColor.b, c.pressedColor.a/2f);
            c.disabledColor = new Color(c.disabledColor.r, c.disabledColor.g, c.disabledColor.b, c.disabledColor.a/2f);
            cutted.colors = c;
        }
        else
        {
            if (cutted != null)
            {
                ColorBlock c = cutted.colors;
                c.normalColor = new Color(c.normalColor.r, c.normalColor.g, c.normalColor.b, 1f);
                c.highlightedColor = new Color(c.highlightedColor.r, c.highlightedColor.g, c.highlightedColor.b, 1f);
                c.pressedColor = new Color(c.pressedColor.r, c.pressedColor.g, c.pressedColor.b, 1f);
                c.disabledColor = new Color(c.disabledColor.r, c.disabledColor.g, c.disabledColor.b, 15f);
                cutted.colors = c;
            }
        }
    
        if(temp.itemType == ItemType.File) {
            copyPastPath = files[temp.id].fPath;
            copyName = files[temp.id].fName;
            isFile = true;
        }
        else if (temp.itemType == ItemType.Folder)
        {
            isFile = false;
            copyPastPath = folders[temp.id].fPath;
            copyName = folders[temp.id].fName;
        }
            ResetPopupMenu();
            
    }

    public void CopyPasteFile(bool toFolder)
    {
        temp = workObj;
            if (isFile)
            {
                if (File.Exists(copyPastPath))
                {
                    string finalT = "";
                    if (toFolder && temp.itemType == ItemType.Folder)
                    {
                        finalT = folders[temp.id].fPath.Replace("/", @"\") + @"\" + copyName; 
                    }
                    else
                    {
                        finalT = upperAdressFiled.text.Replace("/", @"\") + @"\" + copyName;
                    }
                if (copyPastPath != finalT)
                {
                    File.Copy(copyPastPath, finalT, owerWriteFile);
                }
                if (cutFILE && copyPastPath != finalT)
                {
                    File.Delete(copyPastPath);
                }

                }
            }
            else
            {
                if (Directory.Exists(copyPastPath))
                {
                    string finalPath = "";
                    if (toFolder && temp.itemType == ItemType.Folder)
                    {
                        finalPath = folders[temp.id].fPath.Replace("/", @"\") + @"\" + copyName;
                    }
                    else
                    {
                        finalPath = upperAdressFiled.text.Replace("/", @"\") + @"\" + copyName;
                    }
                    Directory.CreateDirectory(finalPath);
                    string[] fls = Directory.GetFiles(copyPastPath);

                    foreach (string f in fls)
                    {
                        string fName = "";
                        string l = f.Replace(@"\", "/");
                        string[] prts = l.Split('/');
                        fName = prts[prts.Length-1];
                        if (File.Exists(f) && f != finalPath + @"\" + fName)
                        {
                            File.Copy(f, finalPath + @"\" + fName, true);
                        }
                    }
                    if (cutFILE && copyPastPath!=finalPath)
                    {
                        Directory.Delete(copyPastPath,true);
                        cutFILE = false;
                        cutted = null;
                        copyPastPath = "";
                    }
                }
            }
        ResetPopupMenu();
        Refresh();
    }

    public void PasteToFolder()
    {
        CopyPasteFile(true);
    }
    

    public string CheckInBookmarks (string path)
    {
        string res = "false";
        foreach (Disk d in disks)
        {
            if (d.path.Replace("/", @"\") == path.Replace("/", @"\"))
            {
                if (!d.isBookMark)
                {
                    res = "disk";
                }
                else
                {
                    res = "true";
                }
                break;
            }

       
        }
        return res;
    }


    public void RemoveBookmarkWindow()
    {
        deleteBookmark.SetActive(true);
        deleteBookmark.GetComponent<RectTransform>().SetAsLastSibling();
    }

    public void Already() 
    {
        alreadyExists.SetActive(true);
        alreadyExists.GetComponent<RectTransform>().SetAsLastSibling();
    }

	public static void AlreadyExists() {
		AdvancedFileBrowser b = GameObject.FindObjectOfType (typeof(AdvancedFileBrowser)) as AdvancedFileBrowser;
		b.Already ();
	}

	public static void FileNotFound() {
		AdvancedFileBrowser b = GameObject.FindObjectOfType (typeof(AdvancedFileBrowser)) as AdvancedFileBrowser;
		b.FNF ();
	}

	public static void DirectoryNotFound() {
		AdvancedFileBrowser b = GameObject.FindObjectOfType (typeof(AdvancedFileBrowser)) as AdvancedFileBrowser;
		b.DirNotFound ();
	}

    public void FNF()
    {
        notFound.SetActive(true);
        notFound.GetComponent<RectTransform>().SetAsLastSibling();
    }

    public void DirMissed()
    {
        if(!dragFailed.activeSelf)
            dirMissed.SetActive(true);
        if (!IsInvoking("Hide"))
        {
            Invoke("Hide", 4f);
        }
    }

    public void DirNotFound()
    {
        errorMessage.SetActive(true);
        errorMessage.GetComponent<RectTransform>().SetAsLastSibling();
    }

    public void FileOrFolderExists()
    {
        fileOrFolderExists.SetActive(true);
        fileOrFolderExists.GetComponent<RectTransform>().SetAsLastSibling();
    }

    public void DragFailed() {
        if(!dirMissed.activeSelf)
             dragFailed.SetActive(true);
        if (!IsInvoking("Hide"))
        {
            Invoke("Hide", 4f);
        }
    }

    void Hide()
    {
        dragFailed.SetActive(false);
        dirMissed.SetActive(false);
    }

	public static void Done() {
		AdvancedFileBrowser b = GameObject.FindObjectOfType (typeof(AdvancedFileBrowser)) as AdvancedFileBrowser;
		b.DoneMessage ();
	}

    public void DoneMessage()
    {
        if (closePanelAfterButtonEvent)
        {
            myWindow.SetActive(false);
        }
        donePanel.SetActive(true);
        donePanel.GetComponent<RectTransform>().SetAsLastSibling();
    }

    public void OnClick(Image bg)
    {
        if (prevBg != null)
        {
            prevBg.enabled = false;
        }
        bg.enabled = true;
        bg.color = bgColor;
        prevBg = bg;
    }

    public void ResetSelection()
    {
        if (resetSelection)
        {
            if (prevBg != null)
            {
                prevBg.enabled = false;
            }
            selectLine.text = "";
        }
        ResetPopupMenu();
        EndEditing();
    }

    public void AddToBookmarks()
    {
        if (CheckInBookmarks(upperAdressFiled.text) == "false")
        {
            Disk disk = new Disk();
            string line = upperAdressFiled.text.Replace(@"\", "/");
            string[] parts = line.Split('/');
            disk.path = upperAdressFiled.text;
            disk.name = parts[parts.Length - 1];
            disk.image = favouriteIcon;
            RectTransform r = Instantiate(disks[disks.Count - 1].text.transform.parent) as RectTransform;
            disk.it = r.GetComponent<Item>();
            disk.it.itemType = ItemType.Disk;
            
            disk.text = r.GetComponentInChildren<Text>();
            disk.text.text = disk.name;
            disk.icon = r.GetComponent<Image>();
            disk.isBookMark = true;
            disk.icon.sprite = disk.image;
            r.SetParent(disks[disks.Count - 1].text.transform.parent.parent);
            disks.Add(disk);
            disksArea.sizeDelta = new Vector2(disksArea.sizeDelta.x, disksArea.sizeDelta.y + disksButtonOffset);
            disk.it.id = disks.Count - 1;
                StreamWriter writer = new StreamWriter(appPath, true);
                writer.WriteLine(line);
                writer.Close();
                StartCoroutine(LoadDirectory(upperAdressFiled.text, false));

        }
        else if (CheckInBookmarks(upperAdressFiled.text) == "true")
        {
            RemoveBookmark(upperAdressFiled.text);
        }
       

    }

    public void CleanBookmarks()
    {
       File.WriteAllText(appPath, String.Empty);
    }

    public void RemoveBookmark(string b)
    {
        string toDelete = b.Replace("/", @"\");
        string[] s = File.ReadAllLines(appPath);
        List<string> bookmarks = new List<string>(s);
        for (int i = 0; i < s.Length; i++)
        {
            bookmarks[i] = s[i].Replace("/", @"\");
        }
        bookmarks.Remove(toDelete);
        for (int j = 0; j < disks.Count; j++)
        {
            if (disks[j].path.Replace("/", @"\") == b.Replace("/", @"\"))
            {
                Destroy(disks[j].icon.gameObject);
                disks.Remove(disks[j]);
                
            }
        } 

        File.WriteAllLines(appPath, bookmarks.ToArray());
        if (Directory.Exists(b))
        {
            StartCoroutine(LoadDirectory(b, false));
        }
    }

    public void LoadSavedBookmarks()
    {
        if (File.Exists(appPath))
        {
            string[] bookmarks = File.ReadAllLines(appPath);
            for (int i = 0; i < bookmarks.Length; i++)
            {
                string[] parts = bookmarks[i].Split('/');
                AddToFavourite(parts[parts.Length - 1], bookmarks[i], favouriteIcon, true);
            }
        }
            switch (startFolder)
            {
                case StartFolder.LastOpened:
                    if(savePathAfterButtonEvent && PlayerPrefs.GetString("LastPath").Length > 3) {
                        savedPath = PlayerPrefs.GetString("LastPath"); 
                    }
                    else
                    {
                        if (addDesktop)
                        {
                            savedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        }
                        else
                        {
                            savedPath = disks[0].path;
                        }
                    }
                    StartCoroutine(LoadDirectory(savedPath, true));
                    break;
                case StartFolder.FirstDisk: StartCoroutine(LoadDirectory(disks[0].path, true));
                    break;
                case StartFolder.Desktop: StartCoroutine(LoadDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), true));
                    break;
                case StartFolder.Documents: StartCoroutine(LoadDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), true));
                    break;
            }
    }

    public void OpenFileBrowser()
    {
        myWindow.SetActive(true);
        myWindow.GetComponent<RectTransform>().SetAsLastSibling();
    }


    public void Search()
    {
        StartCoroutine(LoadDirectory(lastPath, true));
    }

	public void GetDisks() {
		char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
		foreach (char disk in letters) {
			if(Directory.Exists(disk+":/")) {
                Disk newDisk = new Disk();
				disks.Add(newDisk);
                newDisk.path = disk + @":\";
                newDisk.isBookMark = false;
                newDisk.name = disk + @":\";
				if(disks.Count == 1) {
                    newDisk.icon = diskItem.gameObject.GetComponentInChildren<Image>();
                    newDisk.image = baseDiskIcon;
                    newDisk.it = diskItem.gameObject.GetComponent<Item>();
                    newDisk.icon.sprite = newDisk.image;
                    newDisk.text = diskItem.gameObject.GetComponentInChildren<Text>();
					newDisk.text.text = disk.ToString() + @":\";
					lastDisk = diskItem;
				}else{
					RectTransform rect = Instantiate(diskItem, lastDisk.transform.position, Quaternion.identity) as RectTransform;
					rect.SetParent(diskItem.transform.parent);
                    Item it = rect.gameObject.GetComponent<Item>();
                    it.id = disks.Count-1;
                    newDisk.it = it;
					lastDisk = rect;
                    newDisk.icon = lastDisk.gameObject.GetComponentInChildren<Image>();
                    newDisk.image = baseDiskIcon;
                    newDisk.icon.sprite = newDisk.image;
                    newDisk.text = lastDisk.gameObject.GetComponentInChildren<Text>();
                    newDisk.text.text = disk.ToString() + @":\";
					disksArea.sizeDelta = new Vector2 (disksArea.sizeDelta.x, disksArea.sizeDelta.y + disksButtonOffset);
				}
			}
		}
	}

	public void AddToFavourite(string name, string path, Sprite icon, bool isBookmark) {
        Disk newDisk = new Disk();
        RectTransform rect = Instantiate(lastDisk, lastDisk.transform.position, Quaternion.identity) as RectTransform;
        rect.SetParent(diskItem.transform.parent);
        disksArea.sizeDelta = new Vector2(disksArea.sizeDelta.x, disksArea.sizeDelta.y + disksButtonOffset);
        lastDisk = rect;
        newDisk.name = name;
        newDisk.path = path;
        newDisk.isBookMark = isBookmark;
        newDisk.icon = lastDisk.gameObject.GetComponentInChildren<Image>();
        newDisk.image = icon;
        newDisk.icon.sprite = newDisk.image;
        newDisk.text = lastDisk.gameObject.GetComponentInChildren<Text>();
        newDisk.text.text = name;
        disks.Add(newDisk);
	}

    public void OpenFolderFromFavourite(GameObject go)
    {
        foreach (Disk disk in disks)
        {
            
            if (disk.icon.gameObject == go)
            {
                currBookmark = disk.path;
                if (Directory.Exists(disk.path))
                {
                    StartCoroutine(LoadDirectory(disk.path, true));
                }
                else
                {
                    RemoveBookmarkWindow();
                }
            }
        }
    }

    public void DeleteMissedBookmark()
    {
        RemoveBookmark(currBookmark);
    }

    public void Refresh()
    {
        StartCoroutine(LoadDirectory(lastPath, false));
    }

    public void Back()
    {
        if (history.Count > 1)
        {
            string p = history[history.Count - 2];
            if (Directory.Exists(p))
            {
                StartCoroutine(LoadDirectory(p, true));
                history.Remove(p);
            }
            else
            {
                DirMissed();
            }
        }
    }


    Item temp;
    public void RenameFile()
    {
        EndEditing();
        if (renameFiles)
        {
            string fName = "";
            temp = workObj;
            if (temp.itemType == ItemType.File)
            {
                fName = files[temp.id].fName;
                SelectedFile = files[temp.id];
                string[] n = fName.Split('.');
                string line = string.Join("", n, 0, n.Length - 1);
                temp.renamer.text = line;
            }
            else if (temp.itemType == ItemType.Folder)
            {
                fName = folders[temp.id].fName;
                SelectedFolder = folders[temp.id];
                temp.renamer.text = fName;
            }       
            temp.renamer.gameObject.SetActive(true);
            isEditing = true;
            editObj = temp;
        }
        fileOrFolderPopupMenu.SetActive(false);
    }

    string deleteF, delP;
    public void DeleteFile()
    {
        temp = workObj;
        confirmRemove.SetActive(true);
        confirmRemove.GetComponent<RectTransform>().SetAsLastSibling();
        if (temp.itemType == ItemType.File)
        {
            deleteF = files[temp.id].fName;
            delP = files[temp.id].fPath;
        }
        else if (temp.itemType == ItemType.Folder)
        {
            deleteF = folders[temp.id].fName;
            delP = folders[temp.id].fPath;
        } 
        deleteFileName.text = "Delete '" + deleteF + "'?";
        fileOrFolderPopupMenu.SetActive(false);
    }

    public void ConfirmDelete()
    {
            confirmRemove.SetActive(false);
            if (temp.itemType == ItemType.File)
            {
                if (File.Exists(delP))
                {
                    File.Delete(delP);
                }
            }
            else if (temp.itemType == ItemType.Folder)
            {
                if (Directory.Exists(delP))
                {
                    Directory.Delete(delP, true);
                }
                string res = CheckInBookmarks(delP);
                if (res == "true")
                {
                    RemoveBookmark(delP);
                }
            }
        Refresh();
    }


  

    public void CantDrag()
    {
        canDrag = false;
    }

    public void RMBClick(RectTransform button)
    {
        Item i = button.GetComponent<Item>();
        canDrag = true;
        switch (i.itemType)
        {
            case ItemType.File:
             

                for (int q = 0; q < files.Count; q++)
                {
                    FileItem file = files[q];
                    try
                    {
                        if (file.fPath == files[i.id].fPath)
                        {
                            SelectedFile = file;
                            temp = file.nameObject.transform.parent.GetComponent<Item>(); 
                        }
                    }
                    catch
                    {

                    }
                }

                break;
            case ItemType.Folder:
      

                for (int j = 0; j < folders.Count; j++)
                {
                    FolderItem folder = folders[j];
                    try
                    {
                        if (folder.fPath == folders[i.id].fPath)
                        {
                            SelectedFolder = folder;
                            temp = folder.nameObject.transform.parent.GetComponent<Item>();
                        }
                    }
                    catch
                    {

                    }
                }
                break;

            case ItemType.Disk:
                for (int j = 0; j < disks.Count; j++)
                {
                    Disk d = disks[j];
                    try
                    {
                        if (d.path == disks[i.id].path)
                        {
                            temp = d.it;
                        }
                    }
                    catch
                    {

                    }
                }
                break;
        }

    }

    public void Click(RectTransform button)
    {
        Item i = button.GetComponent<Item>();
        ResetPopupMenu();
        if (renameFiles)
        {
            editObj = i;
        }
        switch (i.itemType)
        {
            case ItemType.File:

                selectLine.text = files[i.id].fPath.Replace("/", @"\");
                if (removePreviousSelectedPreview && SelectedFile.fPath != selectLine.text)
                {
                    foreach (FileFormat format in formats)
                    {
                        try
                        {
                            if (SelectedFile.fFormat.ToLower() == format.formatName.ToLower())
                            {
                                SelectedFile.icon.sprite = format.icon;
                                SelectedFile.imageLoaded = false;
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                for (int q = 0; q < files.Count; q++ )   
                {
                    FileItem file = files[q];
                    try
                    {
                        if (file.fPath == files[i.id].fPath)
                        {
                            SelectedFile = file;
                        }
                    }
                    catch
                    {

                    }
                }

             StartCoroutine(LoadImg());

             if (doubleClickToOpenFiles)
             {
                 if ((Time.time - clickTimer) <= doubleClickDelayBetweenClicks)
                 {
                     Application.OpenURL("file://"+selectLine.text);
                 }
                 
                 clickTimer = Time.time;
             }
                break;
            case ItemType.Folder:

                if (doubleClickToOpenFolders)
                {
                    if ((Time.time - clickTimer) <= doubleClickDelayBetweenClicks)
                    {
                        StartCoroutine(LoadDirectory(folders[i.id].fPath, true));
                    }
                    clickTimer = Time.time;
                }
                else
                {
                    StartCoroutine(LoadDirectory(folders[i.id].fPath, true));
                }

                for (int j = 0; j < folders.Count; j++)
                {
                    FolderItem folder = folders[j];
                    try
                    {
                            if (folder.fPath == folders[i.id].fPath)
                            {
                                SelectedFolder = folder;
                            }
                    }
                    catch
                    {

                    }
                }
                break;
        }

    }


    public IEnumerator LoadImg()
    {
        if (loadImagePreview && loadPreviewForSelected)
        {
            if (!SelectedFile.imageLoaded)
            {

             foreach (FileFormat format in formats)
             {

                if (SelectedFile.fFormat.ToLower() == format.formatName.ToLower())
                {
                    if (format.isImage)
                    {
                        WWW www = new WWW("file://" + SelectedFile.fPath.Replace("/", @"\"));
                        yield return www;
                        if (www.texture)
                            SelectedFile.icon.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
                        SelectedFile.imageLoaded = true;
                    }
                    
                }
            }

                
            }

        }
        else
        {
            foreach (FileFormat format in formats)
            {

                if (SelectedFile.fFormat.ToLower() == format.formatName.ToLower())
                {
                    SelectedFile.icon.sprite = format.icon;
                }
            }
        }
    }

    public IEnumerator LoadDirectory()
    {
        if (loadPreviewForSelected)
        {
            foreach (FileItem file in files)
            {
                if (file.fName == selectLine.text)
                {
                    foreach (FileFormat format in formats)
                    {
                        if (file.fFormat.ToLower() == format.formatName.ToLower() && format.isImage)
                        {
                            WWW www = new WWW("file://" + file.fPath.Replace("/", @"\"));
                            yield return www;
                            if (www.texture)
                                file.icon.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
                            yield return new WaitForSeconds(0.3f);
                        }
                    }
                }
            }
        }
        else
        {
            foreach (FileItem file in files)
            {
                foreach (FileFormat format in formats)
                {
                    if (file.fFormat.ToLower() == format.formatName.ToLower() && format.isImage)
                    {
                        WWW www = new WWW("file://" + file.fPath.Replace("/", @"\"));
                        yield return www;
                        if (www.texture)
                            file.icon.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
                        yield return new WaitForSeconds(0.3f);
                    }
                }
            }
        }
    }

    void CleanAll()
    {
        foreach (FolderItem folder in folders)
        {
            Destroy(folder.icon.gameObject);
        }

        foreach (FileItem file in files)
        {
            Destroy(file.icon.gameObject);
        }

        folders.Clear();
        files.Clear();

    }
    public IEnumerator LoadDirectory(string path, bool saveToHistory) {
        lStart();
        currItemEntered = false;
        workSpaceEntered = true;
        ResetPopupMenu();
        yield return new WaitForSeconds(0.1f);
        clickTimer = currRTimer = 0f;
        if (isEditing)
        {
            EndEditing();
        }
        upperAdressFiled.text = path.Replace("/", @"\");

        string res = CheckInBookmarks(path);

        if (res == "false")
        {
            favourite.sprite = addToFavourite;
            favourite.GetComponent<Button>().interactable = true;
        }
        else if (res == "true")
        {
            favourite.sprite = deleteFromFavourite;
            favourite.GetComponent<Button>().interactable = true;
        }
        else
        {
            favourite.sprite = deleteFromFavourite;
            favourite.GetComponent<Button>().interactable = false;
        }


        if (saveToHistory)
        {
            if (history.Count > 0)
            {
                if (history[history.Count - 1] != upperAdressFiled.text)
                {
                    history.Add(upperAdressFiled.text);
                }
            }
            else
            {
                history.Add(upperAdressFiled.text);
            }
        }

        CleanAll();
        
        string[] _f = Directory.GetDirectories(path);
        List<string> _folders = new List<string>(_f), fs = new List<string>(_f);
        if (search.text != "" && search.text != " ") {
            foreach (string folder in _folders)
            {
                if (!folder.ToLower().Contains(search.text.ToLower()))
                {
                    fs.Remove(folder);
                }
            }
            _folders = fs;
        }

        for (int q = 0; q < _folders.Count; q++)
        {
            try
            {
                string dir = _folders[q];
                string line = dir.Replace(@"\", "/");
                string[] parts = line.Split('/');
                FileAttributes attributes = File.GetAttributes(dir);
                if (!showHiddenFolders)
                {
                    if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden || (attributes & FileAttributes.Hidden) == FileAttributes.System)
                    {
                        continue;
                    }
                }
                FolderItem fI = new FolderItem(parts[parts.Length - 1], dir);
                folders.Add(fI);
                RectTransform rt = Instantiate(fileItem) as RectTransform;
                rt.gameObject.SetActive(true);
                fI.obj = rt;
                Item it = rt.gameObject.AddComponent<Item>();
                it.id = folders.IndexOf(fI);
                it.itemType = ItemType.Folder;
                rt.SetParent(fileItem.parent);
                fI.icon = rt.GetComponent<Image>();
                fI.icon.sprite = folderIcon;
                fI.nameObject = rt.GetComponentInChildren<Text>();
                fI.nameObject.text = fI.fName;
                rt.gameObject.name = fI.fName;
            }

            catch (Exception ex)
            {
                print("Coudn't read folder. "+ ex.Message);
            }

        }

        string[] _fl = Directory.GetFiles(path);
        List<string> _files = new List<string>(_fl), f = new List<string>(_fl);

        if (search.text != "" && search.text != " ")
        {
            foreach (string file in _files)
            {
                if (!file.ToLower().Contains(search.text.ToLower()))
                {
                    f.Remove(file);
                }
            }
            _files = f;
        }

        for (int i = 0; i < _files.Count; i++)
        {
            try
            {
                string file = _files[i];
                string line = file.Replace(@"\", "/");
                string[] parts = line.Split('/');
                string[] points = parts[parts.Length - 1].Split('.');

                if (useFileFilter)
                {
                    if (!formatFilter.Contains(points[points.Length - 1]))
                    {
                        continue;
                    }
                }
                FileAttributes attributes = File.GetAttributes(file);
                if (!showHiddenFiles)
                {
                    if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden || (attributes & FileAttributes.Hidden) == FileAttributes.System)
                    {
                        continue;
                    }
                }
                FileItem fI = new FileItem(parts[parts.Length - 1], "." + points[points.Length - 1], file);
                files.Add(fI);
                RectTransform rt = Instantiate(fileItem) as RectTransform;
                Item it = rt.gameObject.AddComponent<Item>();
                it.id = files.IndexOf(fI);
                it.itemType = ItemType.File;
                rt.gameObject.SetActive(true);
                rt.SetParent(fileItem.parent);
                fI.icon = rt.GetComponent<Image>();
                fI.nameObject = rt.GetComponentInChildren<Text>();
                fI.nameObject.text = (showFormat) ? fI.fName : points[points.Length - 2];
                rt.gameObject.name = fI.fName;
                bool ready = false;
                foreach (FileFormat format in formats)
                {
                    if (fI.fFormat == format.formatName)
                    {
                        fI.icon.sprite = format.icon;
                        ready = true;
                    }
                }
                if (!ready)
                {
                    fI.icon.sprite = formats[0].icon;
                }
            }
            catch (Exception ex)
            {
                print("Coudn't read file. " + ex.Message);
            }
        }
        CalcSize(filesInLine);
        lastPath = path.Replace("/",@"\");
        if (loadImagePreview && !loadPreviewForSelected)
        {
            StartCoroutine(LoadDirectory());
        }
        loading.SetActive(false);
    }

    public void lStart()
    {
        loading.SetActive(true);
    }

    public void CalcSize(int raw)
    {
        filesInLine = raw;
        int lines = (int)Mathf.Ceil((float)(files.Count + folders.Count) / filesInLine);
        fileArea.sizeDelta = new Vector2(fileArea.sizeDelta.x, lines * fileButtonOffset);
    }

    public void ButtonEvent()
    {
     
        if (savePathAfterButtonEvent)
        {
            savedPath  = lastPath.Replace(@"\", "/");
            PlayerPrefs.SetString("LastPath", savedPath);
        }
 
        if (selectLine.text != "")
        {
            if (File.Exists(selectLine.text))
            {
                receiver.SendMessage("AddPath", selectLine.text.Replace("/", @"\"), SendMessageOptions.DontRequireReceiver);
                selectLine.text = "";
            }
            else
            {
                FileNotFound();
            }
        }
    }



    public void Start() {
        confirmRemove.SetActive(false);
        createPopupMenu.SetActive(false);
        fileOrFolderPopupMenu.SetActive(false);
        fileOrFolderExists.SetActive(false);
        dragFailed.SetActive(false);
        dirMissed.SetActive(false);
        canvas = myWindow.transform.root.GetComponent<Canvas>();
        if (startFolder == StartFolder.LastOpened)
        {
            savePathAfterButtonEvent = true;
        }

        if(hideAtStart)
          myWindow.SetActive(false);

        if (useFileFilter)
        {
            string line = string.Join(", ", formatFilter.ToArray());
            placeholderLine.text = line;
        }
        appPath = Application.dataPath + "/Bookmarks.txt";
	}


    void StartSettings()
    {
        if (errorMessage)
            errorMessage.SetActive(false);

        fileItem.gameObject.SetActive(false);

        GetDisks();

        if (addDesktop)
            AddToFavourite("Desktop", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), desktopIcon, false);

        if (addDocumets)
            AddToFavourite("Documents", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), desktopIcon, false);

        LoadSavedBookmarks();

        loaded = true;

    }

    public void Enter()
    {
        if (Directory.Exists(upperAdressFiled.text))
        {
            StartCoroutine(LoadDirectory(upperAdressFiled.text, true));
        }
        else
        {
            DirNotFound();
        }
    }

    public void EndEditing()
    {
        
        if (isEditing)
        {
            if (editObj)
            {
                if (editObj.renamer.text != "")
                {
                    if (editObj.itemType == ItemType.File)
                    {
                        string oldPath = SelectedFile.fPath.Replace(@"\", "/"), newPath;
                        List<string> p = new List<string>(oldPath.Split('/'));
                        p.RemoveAt(p.Count - 1);
                        newPath = string.Join(@"\", p.ToArray()) + @"\" + editObj.renamer.text + SelectedFile.fFormat;
                        oldPath = oldPath.Replace("/", @"\");
                        if (oldPath != newPath)
                        {
                            try 
                            {
                            File.Move(oldPath, newPath);
                            StartCoroutine(LoadDirectory(lastPath, false));
                            }
                            catch
                            {

                            }
                        }
                    }
                    else if (editObj.itemType == ItemType.Folder)
                    {
                        string oldPath = SelectedFolder.fPath.Replace(@"\", "/"), newPath;
                        List<string> p = new List<string>(oldPath.Split('/'));
                        p.RemoveAt(p.Count - 1);
                        newPath = string.Join(@"\", p.ToArray()) + @"\" + editObj.renamer.text;
                        oldPath = oldPath.Replace("/", @"\");
                        if (oldPath != newPath)
                        {
                            try
                            {
                                Directory.Move(oldPath, newPath);
                                StartCoroutine(LoadDirectory(lastPath, false));
                            }
                            catch
                            {

                            }
                            
                        }
                    }


                }
            }
            isEditing = false;
        }
        Item[] items = GameObject.FindObjectsOfType(typeof(Item)) as Item[];
        for (int i = 0; i < items.Length; i++)
        {
            items[i].EndEditing();
        }
    }

    public void ResetPopupMenu()
    {
        createPopupMenu.SetActive(false);
        fileOrFolderPopupMenu.SetActive(false);
    }

    public void CreateEmptyFolder()
    {
        bool r = false;
        int i = 1;
        if (Directory.Exists(lastPath + @"\NewFolder"))
        {
            while (!r)
            {
                if (!Directory.Exists(lastPath + @"\NewFolder_" + i.ToString()))
                {
                    Directory.CreateDirectory(lastPath + @"\NewFolder_"+i.ToString());
                    r = true;
                }
                else
                {
                    i++;
                }
            }
        }
        else
        {
            Directory.CreateDirectory(lastPath + @"\NewFolder");
        }
        ResetPopupMenu();
        Refresh();
    }

    void CheckPaste()
    {
        if (cutFILE)
        {
            if (copyPastPath.Replace("/", @"\") != upperAdressFiled.text.Replace("/", @"\"))
            {
                if (copyPastPath != "")
                {
                    pasteButton.interactable = true;
                    if (temp.itemType == ItemType.Folder)
                         pasteButton2.interactable = true;
                    else
                        pasteButton2.interactable = false;
                }
                else
                {
                    pasteButton.interactable = false;
                    pasteButton2.interactable = false;
                }
            }
            else
            {
                pasteButton.interactable = false;
                pasteButton2.interactable = false;
            }
        }
        else
        {

            if (copyPastPath != "")
            {
                pasteButton.interactable = true;
                if (temp.itemType == ItemType.Folder)
                     pasteButton2.interactable = true;
                else
                    pasteButton2.interactable = false;
            }
            else
            {
                pasteButton.interactable = false;
                pasteButton2.interactable = false;
            }
        }
    }
    void Update() {
        if (myWindow.activeSelf)
        {

            if (dragFilesAndFolders) {
               
            if (Input.GetMouseButton(0))
            {
                if (temp && !tempDrag && canDrag && t>dragHoldTime && !isEditing)
                {
                    if (temp.itemType != ItemType.Disk)
                    {
                        GameObject g = Instantiate(temp.gameObject) as GameObject;
                        tempDrag = g;
                        tempDrag.GetComponent<RectTransform>().SetParent(canvas.transform);
                        tempDrag.GetComponent<RectTransform>().SetAsLastSibling();
                        tempDrag.GetComponent<RectTransform>().sizeDelta = new Vector2(50f, 50f);
                        isDragging = true;
                    }
                }
                if(temp && currItemEntered)
                    t += 1f;
            }

            if (isDragging && tempDrag)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pointerPos);
                tempDrag.transform.position = canvas.transform.TransformPoint(pointerPos-new Vector2(0, 40));
            }

            if(Input.GetMouseButtonUp(0))
            {
                t = 0f;
                if (isDragging)
                {
                    if (temp)
                    {
                            if (temp.itemType != ItemType.File)
                            {
                                string startPath = "", endPath = "";
                                ItemType typeOf = tempDrag.GetComponent<Item>().itemType;
                                if (SelectedFolder != null)
                                {
                                    if (temp.itemType == ItemType.Disk)
                                    {
                                        endPath = disks[temp.id].path;
                                    }
                                    else
                                    {
                                        endPath = SelectedFolder.fPath;
                                    }
                                }

                                if (typeOf == ItemType.File)
                                {
                                    startPath = files[tempDrag.GetComponent<Item>().id].fPath;

                                    if (endPath != startPath && (currItemEntered || temp.itemType == ItemType.Disk))
                                    {
                                        File.Move(startPath, endPath + @"\" + files[tempDrag.GetComponent<Item>().id].fName);
                                        Refresh();
                                    }
                                }
                                else if (typeOf == ItemType.Folder)
                                {
                                    startPath = folders[tempDrag.GetComponent<Item>().id].fPath;
                                    if (endPath != startPath && (currItemEntered || temp.itemType == ItemType.Disk))
                                    {
                                        string end = endPath + @"\" + folders[tempDrag.GetComponent<Item>().id].fName;
                                        string res = CheckInBookmarks(startPath);
                                        try
                                        {
                                            Directory.Move(startPath, end);
                                            Refresh();
                                        }
                                        catch
                                        {
                                            if (temp.itemType == ItemType.Disk)
                                                DragFailed();
                                            else
                                                FileOrFolderExists();
                                        }
                                        
                                        if (res == "true")
                                        {
                                            RemoveBookmark(startPath);
                                        }
                                    }
                                }


                            }
      
                    }
                    canDrag = false;
                    Destroy(tempDrag.gameObject);
                    tempDrag = null;                   
                    isDragging = false;
                }
            }

            }

            if (Input.GetMouseButtonDown(1))
            {
                if (isEditing)
                {
                    EndEditing();
                }
                if (!currItemEntered && workSpaceEntered)
                {
                    createPopupMenu.SetActive(true);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pointerPos);
                    createPopupMenu.transform.position = canvas.transform.TransformPoint(pointerPos + new Vector2(0, popupMenuOffset));
                    CheckPaste();
                    fileOrFolderPopupMenu.SetActive(false);
                }
                else if(currItemEntered)
                {
                    workObj = temp;
                    fileOrFolderPopupMenu.SetActive(true);
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pointerPos);
                    fileOrFolderPopupMenu.transform.position = canvas.transform.TransformPoint(pointerPos + new Vector2(0, popupMenuOffset));
                    createPopupMenu.SetActive(false);
                    CheckPaste();
                }
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Refresh();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!isEditing)
                {
                    Enter();
                }
                else
                {
                    EndEditing();
                }
            }
        }
        if (!loaded && myWindow.activeSelf)
        {
            StartSettings();
        }
    }

}
