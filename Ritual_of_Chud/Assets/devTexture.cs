using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class devTexture : MonoBehaviour {
    public Sprite Sprt;
    public Image immagine;
    public string  url = "file://C:/Users/Osvaldo/Desktop/Katsuskull.png";

   IEnumerator Start() { 

        Texture2D tex;
        tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        using (WWW www = new WWW(url))
        {
            yield return www;
            www.LoadImageIntoTexture(tex);
        Sprt = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0, 0), 100.0f);

        immagine = this.GetComponent<Image>();

        immagine.sprite = Sprt;

        }
    }

}
