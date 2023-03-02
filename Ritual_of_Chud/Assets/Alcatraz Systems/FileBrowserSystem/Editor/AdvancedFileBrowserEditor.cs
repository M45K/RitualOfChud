//AdvancedFileBrowser by Alcatraz. 
//Flight Dream Studio (C) 2015. 
//alcatrazalex@gmail.com
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AdvancedFileBrowser))]
public class AdvancedFileBrowserEditor : Editor
{
	Texture2D image, add, remove;
	GUISkin skin, skin2;
	
	public override void OnInspectorGUI() {
        AdvancedFileBrowser script = (AdvancedFileBrowser)target;
        skin = (GUISkin)(AssetDatabase.LoadAssetAtPath("Assets/Alcatraz Systems/FileBrowserSystem/Editor/GUISkin.guiskin", typeof(GUISkin)));
        skin2 = (GUISkin)(AssetDatabase.LoadAssetAtPath("Assets/Alcatraz Systems/FileBrowserSystem/Editor/GUISkin2.guiskin", typeof(GUISkin)));
		if(!image) {
            image = AssetDatabase.LoadAssetAtPath("Assets/Alcatraz Systems/FileBrowserSystem/Editor/fdsLogo.png", typeof(Texture2D)) as Texture2D;
            add = AssetDatabase.LoadAssetAtPath("Assets/Alcatraz Systems/FileBrowserSystem/Editor/add.png", typeof(Texture2D)) as Texture2D;
            remove = AssetDatabase.LoadAssetAtPath("Assets/Alcatraz Systems/FileBrowserSystem/Editor/delete.png", typeof(Texture2D)) as Texture2D;
		}	
		if(script.darkSkin)
		GUI.skin = skin;

		EditorGUILayout.BeginVertical("Box");
		GUI.DrawTexture ( GUILayoutUtility.GetRect(64f, 64f),image); 
		EditorGUILayout.EndVertical();
		
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.LabelField ("Advanced File Browser v 1.1");

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);

        script.settings = EditorGUILayout.Foldout(script.settings, "Settings");
        EditorGUILayout.EndHorizontal();

        if (script.settings)
        {
            EditorGUILayout.HelpBox("Base Settings", MessageType.None);
            script.startFolder = (StartFolder)EditorGUILayout.EnumPopup("Start folder:", script.startFolder);
            script.showHiddenFolders = EditorGUILayout.Toggle("Show hidden folders:", script.showHiddenFolders);
            script.showHiddenFiles = EditorGUILayout.Toggle("Show hidden files:", script.showHiddenFiles);
            script.showFormat = EditorGUILayout.Toggle("Show formats:", script.showFormat);
            script.hideAtStart = EditorGUILayout.Toggle("Hide Browser at start:", script.hideAtStart);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Filter files", MessageType.None);
            script.useFileFilter = EditorGUILayout.Toggle("Use filter", script.useFileFilter);
            if (script.useFileFilter)
            {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            script.showFilters = EditorGUILayout.Foldout(script.showFilters, "Edit Filters");
            EditorGUILayout.EndHorizontal();

            if (script.showFilters)
            {
                GUI.skin = skin2;
                if (GUILayout.Button(add, GUILayout.Width(35), GUILayout.Height(35)))
                {
                    string s = "";
                    script.formatFilter.Add(s);
                }
                for (int i = 0; i < script.formatFilter.Count; i++)
                {
                    try
                    {
                        EditorGUILayout.BeginVertical("Box");

                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button(remove, GUILayout.Width(30), GUILayout.Height(30)))
                        {
                            script.formatFilter.Remove(script.formatFilter[i]);
                        }
                        script.formatFilter[i] = EditorGUILayout.TextField("", script.formatFilter[i]);
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.EndVertical();
                        GUILayout.Space(5f);
                    }
                    catch
                    {

                    }
                }
                GUI.skin = skin;
            }

            }
            EditorGUILayout.HelpBox("Supported Formats", MessageType.None);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            script.showFormats = EditorGUILayout.Foldout(script.showFormats, "Edit Formats");
            EditorGUILayout.EndHorizontal();

            if (script.showFormats)
            {
                EditorGUILayout.BeginVertical("Box");
                GUI.skin = skin2;
                if (GUILayout.Button(add, GUILayout.Width(35), GUILayout.Height(35)))
                {
                    FileFormat f = new FileFormat();
                    f.formatName = ".YourFormat";
                    script.formats.Add(f);
                }

                for (int i = 0; i < script.formats.Count; i++)
                {
                    FileFormat format = script.formats[i];
                    EditorGUILayout.BeginVertical("Box");
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Space(10);
                    format.showInInspector = EditorGUILayout.Foldout(format.showInInspector, format.formatName);
                    GUILayout.Space(10);
                    if (GUILayout.Button(remove, GUILayout.Width(30), GUILayout.Height(30)))
                    {
                        script.formats.Remove(format);
                    }
                    EditorGUILayout.EndHorizontal();

                    if (format.showInInspector)
                    {
                        format.formatName = EditorGUILayout.TextField("Format Name", format.formatName);
                        format.icon = EditorGUILayout.ObjectField("Format Icon", format.icon, typeof(Sprite), true) as Sprite;
                        format.isImage = EditorGUILayout.Toggle("Is image format?", format.isImage);
                    }
                    GUILayout.Space(10);
                    EditorGUILayout.EndVertical();
                }
                GUI.skin = skin;
                EditorGUILayout.EndVertical();
            }
	
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("On File Click", MessageType.None);
            script.savePathAfterButtonEvent = EditorGUILayout.Toggle("Save path after:", script.savePathAfterButtonEvent);
            script.closePanelAfterButtonEvent = EditorGUILayout.Toggle("Close FileBrowser after:", script.closePanelAfterButtonEvent);
            GUILayout.Space(10);
            script.bgColor = EditorGUILayout.ColorField("Background color", script.bgColor);
            script.resetSelection = EditorGUILayout.Toggle("Reset selection:", script.resetSelection);
            EditorGUILayout.HelpBox("Reset selection when user click to background", MessageType.Info);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Open Files&Folders", MessageType.None);
              script.doubleClickToOpenFiles = EditorGUILayout.Toggle("Double-click [Files]:", script.doubleClickToOpenFiles);
            script.doubleClickToOpenFolders = EditorGUILayout.Toggle("Double-click [Folders]:", script.doubleClickToOpenFolders);
            if (script.doubleClickToOpenFiles || script.doubleClickToOpenFolders)
            { 
                script.doubleClickDelayBetweenClicks = EditorGUILayout.FloatField(" Clicks delay:", script.doubleClickDelayBetweenClicks);
                EditorGUILayout.HelpBox("Delay between clicks to detect double click event", MessageType.Info);
            }
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Rename Files", MessageType.None);
            script.renameFiles = EditorGUILayout.Toggle("Rename files", script.renameFiles);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Drag Files&Folders", MessageType.None);
            script.dragFilesAndFolders = EditorGUILayout.Toggle("Drag Files&Folders", script.dragFilesAndFolders);
            script.dragHoldTime = EditorGUILayout.FloatField("Hold time to drag:", script.dragHoldTime);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Copy&Paste&Cut [Files&Folders]", MessageType.None);
            script.owerWriteFile = EditorGUILayout.Toggle("Owerwrite", script.owerWriteFile);
            script.pasteButton = EditorGUILayout.ObjectField("Paste button [Menu 1]", script.pasteButton, typeof(Button), true) as Button;
            script.pasteButton2 = EditorGUILayout.ObjectField("Paste button [Menu 2]", script.pasteButton2, typeof(Button), true) as Button;
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Images Preview", MessageType.None);
            script.loadImagePreview = EditorGUILayout.Toggle("Load Image preview:", script.loadImagePreview);
            if (script.loadImagePreview)
            {
                script.loadPreviewForSelected = EditorGUILayout.Toggle("Load for selected only:", script.loadPreviewForSelected);
                if (script.loadPreviewForSelected)
                script.removePreviousSelectedPreview = EditorGUILayout.Toggle("Delete previous preview:", script.removePreviousSelectedPreview);
            }
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Base bookmarks", MessageType.None);
            script.addDesktop = EditorGUILayout.Toggle("Add Desktop:", script.addDesktop);
            script.addDocumets = EditorGUILayout.Toggle("Add Documents:", script.addDocumets);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("UI Objects", MessageType.None);
            script.myWindow = EditorGUILayout.ObjectField("Main UI Panel", script.myWindow, typeof(GameObject), true) as GameObject;
            script.loading = EditorGUILayout.ObjectField("Loading screen", script.loading, typeof(GameObject), true) as GameObject;
            EditorGUILayout.HelpBox("Loading text: when FileBroser loads folder", MessageType.Info);
            GUILayout.Space(10);
            script.confirmRemove = EditorGUILayout.ObjectField("Confirm action[Delete]", script.confirmRemove, typeof(GameObject), true) as GameObject;
            script.deleteFileName = EditorGUILayout.ObjectField("Delete text[Text]", script.deleteFileName, typeof(Text), true) as Text;
            GUILayout.Space(10);
            script.createPopupMenu = EditorGUILayout.ObjectField("Popup menu #1", script.createPopupMenu, typeof(GameObject), true) as GameObject;
            EditorGUILayout.HelpBox("Popup menu for RMB click to create a folder", MessageType.Info);
            GUILayout.Space(10);
            script.fileOrFolderPopupMenu = EditorGUILayout.ObjectField("Popup menu #2", script.fileOrFolderPopupMenu, typeof(GameObject), true) as GameObject;
            EditorGUILayout.HelpBox("Popup menu for RMB click to rename or delete a file or a folder", MessageType.Info);
            GUILayout.Space(10);
            script.receiver = EditorGUILayout.ObjectField("Recipient", script.receiver, typeof(GameObject), true) as GameObject;
            EditorGUILayout.HelpBox("Object which will receive paths to selected files", MessageType.Info);
            GUILayout.Space(10);
            script.errorMessage = EditorGUILayout.ObjectField("Wrong Directory", script.errorMessage, typeof(GameObject), true) as GameObject;
            script.deleteBookmark = EditorGUILayout.ObjectField("Bookmark Not Found", script.deleteBookmark, typeof(GameObject), true) as GameObject;
            script.notFound = EditorGUILayout.ObjectField("File not found", script.notFound, typeof(GameObject), true) as GameObject;
            script.alreadyExists = EditorGUILayout.ObjectField("Already added", script.alreadyExists, typeof(GameObject), true) as GameObject;
            script.donePanel = EditorGUILayout.ObjectField("File added", script.donePanel, typeof(GameObject), true) as GameObject;
            script.fileOrFolderExists = EditorGUILayout.ObjectField("File|Folder Already Exists", script.fileOrFolderExists, typeof(GameObject), true) as GameObject;
            script.dragFailed = EditorGUILayout.ObjectField("Drag Failed", script.dragFailed, typeof(GameObject), true) as GameObject;
            script.dirMissed = EditorGUILayout.ObjectField("Directory Missed", script.dirMissed, typeof(GameObject), true) as GameObject;
            GUILayout.Space(5);
            script.favourite = EditorGUILayout.ObjectField("Favourite", script.favourite, typeof(Image), true) as Image;
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Sprites", MessageType.None);
            script.addToFavourite = EditorGUILayout.ObjectField("Add To Favourite", script.addToFavourite, typeof(Sprite), true) as Sprite;
            script.deleteFromFavourite = EditorGUILayout.ObjectField("Delete From Favourite", script.deleteFromFavourite, typeof(Sprite), true) as Sprite;
            script.favouriteIcon = EditorGUILayout.ObjectField("Favourite Icon", script.favouriteIcon, typeof(Sprite), true) as Sprite;
            script.baseDiskIcon = EditorGUILayout.ObjectField("Base Disk Icon", script.baseDiskIcon, typeof(Sprite), true) as Sprite;
            script.folderIcon = EditorGUILayout.ObjectField("Folder Icon", script.folderIcon, typeof(Sprite), true) as Sprite;
            script.desktopIcon = EditorGUILayout.ObjectField("Desktop Icon", script.desktopIcon, typeof(Sprite), true) as Sprite;
            script.documentsIcon = EditorGUILayout.ObjectField("Documents Icon", script.documentsIcon, typeof(Sprite), true) as Sprite;
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Offset Of Buttons", MessageType.None);
            script.disksButtonOffset = EditorGUILayout.FloatField("Disk offset", script.disksButtonOffset);
            script.fileButtonOffset = EditorGUILayout.FloatField("File offset:", script.fileButtonOffset);
            script.popupMenuOffset = EditorGUILayout.FloatField("Popups offset:", script.popupMenuOffset);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Input Fields", MessageType.None);
            script.search = EditorGUILayout.ObjectField("Search", script.search, typeof(InputField), true) as InputField;
            script.upperAdressFiled = EditorGUILayout.ObjectField("Adress", script.upperAdressFiled, typeof(InputField), true) as InputField;
            script.selectLine = EditorGUILayout.ObjectField("Selected file", script.selectLine, typeof(InputField), true) as InputField;
            script.placeholderLine = EditorGUILayout.ObjectField("Placeholder", script.placeholderLine, typeof(Text), true) as Text;
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Column With Disks", MessageType.None);
            script.diskItem = EditorGUILayout.ObjectField("Prototype", script.diskItem, typeof(RectTransform), true) as RectTransform;
            script.disksArea = EditorGUILayout.ObjectField("Scroll Area", script.disksArea, typeof(RectTransform), true) as RectTransform;
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Field With Files/Folders", MessageType.None);
            script.fileItem = EditorGUILayout.ObjectField("Prototype", script.fileItem, typeof(RectTransform), true) as RectTransform;
            script.fileArea = EditorGUILayout.ObjectField("Scroll Area", script.fileArea, typeof(RectTransform), true) as RectTransform;
            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Custom Editor", MessageType.None);
            script.darkSkin = EditorGUILayout.Toggle("Inspector Dark Style:", script.darkSkin);
        }
        EditorGUILayout.EndVertical();
		
		
		EditorGUILayout.EndVertical();
		
		if (GUI.changed){
			EditorUtility.SetDirty (script);
		}
	}

	
}
