using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Wichtig für Coroutines!

public class SceneSwitcher : MonoBehaviour
{
    public void GoToScene(string sceneName)
    {
        // Wir starten eine "Coroutine", die im Hintergrund läuft
        StartCoroutine(LoadSceneInBackground(sceneName));
    }

    IEnumerator LoadSceneInBackground(string sceneName)
    {
        Debug.Log("Starte Hintergrund-Ladevorgang für: " + sceneName);

        // Das hier sagt Unity: "Lade die Szene, aber lass das Spiel weiterlaufen"
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Solange die Szene noch nicht fertig geladen ist...
        while (!operation.isDone)
        {
            // ...warten wir einen Frame und lassen das VR-Bild flüssig weiterlaufen
            yield return null;
        }
    }
}