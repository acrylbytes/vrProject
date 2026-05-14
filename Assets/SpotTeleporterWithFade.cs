using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Wichtig für das Fade-Bild

public class SpotTeleporterWithFade : MonoBehaviour
{
    public Transform[] spots;
    public GameObject vrPlayerRig;
    public CanvasGroup fadeCanvasGroup; // Das UI-Element für Schwarz
    public float fadeDuration = 0.5f;

    private int currentSpotIndex = 0;
    private bool isTeleporting = false;

    void Update()
    {
        // B-Taste (Button.Two) auf dem rechten Quest-Controller
        if (OVRInput.GetDown(OVRInput.Button.Two) && !isTeleporting)
        {
            StartCoroutine(TeleportSequence());
        }
    }

    IEnumerator TeleportSequence()
    {
        isTeleporting = true;

        // 1. Schwarz werden
        yield return StartCoroutine(Fade(1f));

        // 2. Position springen
        currentSpotIndex = (currentSpotIndex + 1) % spots.Length;
        vrPlayerRig.transform.position = spots[currentSpotIndex].position;

        // Kurze Pause im Dunkeln (optional)
        yield return new WaitForSeconds(0.1f);

        // 3. Wieder hell werden
        yield return StartCoroutine(Fade(0f));

        isTeleporting = false;
    }

    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvasGroup.alpha;
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = targetAlpha;
    }
}