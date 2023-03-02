using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesPassageManager : MonoBehaviour {

	public void caricaScenaHome()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        SceneManager.LoadScene("Scena1-New-Carica", LoadSceneMode.Single);
    }

	public void caricaScenaNuovoProfilo()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        SceneManager.LoadScene("Scena2-Dati-Utente", LoadSceneMode.Single);
    }

	public void caricaScenaConfigurazioneRagno()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        SceneManager.LoadScene("Scena0-Protocolli-Editor-Admin", LoadSceneMode.Single);
    }

	public void caricaScenaCaricamentoProfilo()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        SceneManager.LoadScene("Scena4-Caricamento-Ricerca-DB", LoadSceneMode.Single);
    }
    public void caricaUnderlay()
    {
        // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
        SceneManager.LoadScene("underlaySelector", LoadSceneMode.Single);
    }
}
