using UnityEngine;
using TMPro; // Wichtig für TextMeshPro

public class TextSequenz : MonoBehaviour
{
    public TextMeshProUGUI textFeld; // Hier dein "Einleitung" Objekt reinziehen
    [TextArea(3, 10)]
    public string[] texte; // Hier im Inspector die Texte schreiben
    public GameObject canvasZumAusschalten; // Das ganze UI-Objekt

    private int aktuellerIndex = 0;

    void Start()
    {
        ZeigeAktuellenText();
    }

    // Diese Funktion rufen wir auf, wenn gedrückt wird
    public void NaechsterText()
    {
        aktuellerIndex++;

        if (aktuellerIndex < texte.Length)
        {
            ZeigeAktuellenText();
        }
        else
        {
            // Ende erreicht -> UI ausblenden
            canvasZumAusschalten.SetActive(false);
        }
    }

    void ZeigeAktuellenText()
    {
        if (texte.Length > 0)
        {
            textFeld.text = texte[aktuellerIndex];
        }
    }
}