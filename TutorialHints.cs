using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHints : MonoBehaviour
{

    public GameObject hinter;
    public List<Vector2> v2quewe;
    public List<string> tiquewe;
    public List<string> coquewe;
    public List<float> wiquewe;
    public List<float> hequewe;
    public List<float> boquewe;
    public List<int> idquewe;
    public float timeProg;
    public Text testText;

    public void AddTutorialHint(Vector2 position, string title, string content, float width, float backOpacity, int playerPrefID) {
        if (PlayerPrefs.GetInt("TUTORIALYES", 0) == 1) {
            if (hequewe.Count == 0) timeProg = 0f;
            v2quewe.Add(position); tiquewe.Add(title); coquewe.Add(content); wiquewe.Add(width); boquewe.Add(backOpacity);
            idquewe.Add(playerPrefID); StartCoroutine(waitFr(width, content));
        }
    }

    public void AddSpecial(int g) {
        if (g == 0) if (PlayerPrefs.GetInt("HASHINT18", 0) == 0) {
            if (PlayerPrefs.GetInt("MAINLANG") == 0) AddTutorialHint(new Vector2(0f, 0f), "Layout Modifier", "In this panel, you can moddify the layout of the Music PLayer element, this functions similar to the editor, you can add and delete objects, moddify their properties, etc. and you can select each one from the left inspector", 1200f, 0.3f, 18); 
            if (PlayerPrefs.GetInt("MAINLANG") == 1) AddTutorialHint(new Vector2(0f, 0f), "Modificateur de mise en page", "Dans ce panneau, vous pouvez modifier la disposition de l'élément Lecteur de musique. Son fonctionnement est similaire à celui de l'éditeur : vous pouvez ajouter et supprimer des objets, modifier leurs propriétés, etc. Vous pouvez sélectionner chaque élément dans l'inspecteur situé à gauche", 1400f, 0.3f, 18); 
            if (PlayerPrefs.GetInt("MAINLANG") == 2) AddTutorialHint(new Vector2(0f, 0f), "Layout-Modifikator", "In diesem Bedienfeld können Sie das Layout des Musikplayer-Elements ändern. Es funktioniert ähnlich wie der Editor; Sie können Objekte hinzufügen und löschen, deren Eigenschaften ändern usw. und jedes einzelne über den linken Inspektor auswählen", 1300f, 0.3f, 18); 
            if (PlayerPrefs.GetInt("MAINLANG") == 3) AddTutorialHint(new Vector2(0f, 0f), "Modificator de aspect", "În acest panou, puteți modifica aspectul elementului Music Player; acesta funcționează similar cu editorul, puteți adăuga și șterge obiecte, le puteți modifica proprietățile etc. și puteți selecta fiecare dintre ele din inspectorul din stânga", 1300f, 0.3f, 18); }
        if (g == 1) if (PlayerPrefs.GetInt("HASHINT19", 0) == 0) {
            if (PlayerPrefs.GetInt("MAINLANG") == 0) AddTutorialHint(new Vector2(0f, 0f), "Image Slicer", "In this panel, you can slice an image in up to 9 pieces, based on what mode you selected:\nSliced: Corners stay the same, edges stretch on 1 axis, and the center stretches freely.\nTiled: Corners stay the same, edges tile on 1 direction and the center tiles in any direction", 1200f, 0.3f, 19); 
            if (PlayerPrefs.GetInt("MAINLANG") == 1) AddTutorialHint(new Vector2(0f, 0f), "Séparateur d'images", "Dans ce panneau, vous pouvez découper une image en 9 morceaux maximum, selon le mode sélectionné:\nDécoupé : les coins restent fixes, les bords s’étirent sur un axe et le centre s’étire librement.\nMosaïque : les coins restent fixes, les bords se répètent dans une direction et le centre se répète dans n’importe quelle direction", 1300f, 0.3f, 19); 
            if (PlayerPrefs.GetInt("MAINLANG") == 2) AddTutorialHint(new Vector2(0f, 0f), "Bildsplitter", "In diesem Bedienfeld können Sie ein Bild in bis zu 9 Teile zerlegen, je nach gewähltem Modus.\nGeschnitten: Die Ecken bleiben unverändert, die Kanten werden entlang einer Achse gestreckt und der Mittelpunkt ist frei dehnbar.\nGekachelt: Die Ecken bleiben unverändert, die Kanten werden in einer Richtung gekachelt und der Mittelpunkt in beliebiger Richtung gekachelt", 1400f, 0.3f, 19); 
            if (PlayerPrefs.GetInt("MAINLANG") == 3) AddTutorialHint(new Vector2(0f, 0f), "Divizor de imagine", "În acest panou, puteți tăia o imagine în până la 9 bucăți, în funcție de modul selectat:\nFeliat: Colțurile rămân la fel, marginile se întind pe o axă, iar centrul se întinde liber.\nMozaic: Colțurile rămân la fel, marginile se întind pe o direcție, iar centrul se întind în orice direcție", 1200f, 0.3f, 19); }
        if (g == 2) if (PlayerPrefs.GetInt("HASHINT20", 0) == 0) {
            if (PlayerPrefs.GetInt("MAINLANG") == 0) AddTutorialHint(new Vector2(0f, 0f), "URL Songs", "In this panel, you can load songs from Google Drive URLs so you can play using the Music Player, here you can add, remove and modify your list of loaded links", 1200f, 0.3f, 20); 
            if (PlayerPrefs.GetInt("MAINLANG") == 1) AddTutorialHint(new Vector2(0f, 0f), "Chansons URL", "Dans ce panneau, vous pouvez charger des morceaux depuis des URL Google Drive pour les écouter avec le lecteur de musique. Vous pouvez également ajouter, supprimer et modifier la liste des liens chargés", 1300f, 0.3f, 20); 
            if (PlayerPrefs.GetInt("MAINLANG") == 2) AddTutorialHint(new Vector2(0f, 0f), "URL-Songs", "In diesem Bereich können Sie Songs von Google Drive-URLs laden und mit dem Musikplayer abspielen. Hier können Sie außerdem die Liste der geladenen Links hinzufügen, entfernen und bearbeiten", 1200f, 0.3f, 20); 
            if (PlayerPrefs.GetInt("MAINLANG") == 3) AddTutorialHint(new Vector2(0f, 0f), "Cântece URL", "În acest panou, puteți încărca melodii din adresele URL ale Google Drive, astfel încât să le puteți reda folosind Playerul muzical. Aici puteți adăuga, elimina și modifica lista de linkuri încărcate", 1300f, 0.3f, 20); }
    }

    public void HintTap() { timeProg = 0f;
        v2quewe.RemoveAt(0); tiquewe.RemoveAt(0); coquewe.RemoveAt(0); wiquewe.RemoveAt(0); hequewe.RemoveAt(0); boquewe.RemoveAt(0);
        PlayerPrefs.SetInt("HASHINT" + idquewe[0], 1); idquewe.RemoveAt(0);
    }

    public IEnumerator waitFr(float width, string content) {
        testText.text = content; testText.GetComponent<RectTransform>().sizeDelta = new Vector2(width - 70f, 1000f);
        RectTransform rt = testText.GetComponent<RectTransform>(); LayoutRebuilder.ForceRebuildLayoutImmediate(rt);
        hequewe.Add(testText.preferredHeight + 30f); yield break;
    }

    void FixedUpdate() { timeProg += Time.fixedDeltaTime;
        if(hequewe.Count > 0) { hinter.SetActive(true);
            for(int g = 1; g < 3; g++) hinter.transform.GetChild(g).GetComponent<RectTransform>().anchoredPosition = v2quewe[0];
            for(int g = 3; g < 6; g++) hinter.transform.GetChild(g).GetComponent<RectTransform>().anchoredPosition = new Vector2(v2quewe[0].x, v2quewe[0].y - 35f);
            hinter.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector2(v2quewe[0].x, v2quewe[0].y + (hequewe[0] / 2f));
            hinter.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(wiquewe[0] * Mathf.Min(timeProg * 4f, 1f), (hequewe[0] + 100f) * Mathf.Min(timeProg * 4f, 1f));
            hinter.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(wiquewe[0] * Mathf.Clamp01((timeProg - 0.1f) * 4f), (hequewe[0] + 100f) * Mathf.Clamp01((timeProg - 0.1f) * 4f));
            hinter.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2((wiquewe[0] - 30f) * Mathf.Clamp01((timeProg - 0.2f) * 4f), hequewe[0] * Mathf.Clamp01((timeProg - 0.2f) * 4f));
            hinter.transform.GetChild(4).GetComponent<RectTransform>().sizeDelta = new Vector2((wiquewe[0] - 30f) * Mathf.Clamp01((timeProg - 0.3f) * 4f), hequewe[0] * Mathf.Clamp01((timeProg - 0.3f) * 4f));
            hinter.transform.GetChild(5).GetComponent<RectTransform>().sizeDelta = new Vector2((wiquewe[0] - 70f), hequewe[0] * Mathf.Clamp01((timeProg - 0.3f) * 4f));
            hinter.transform.GetChild(6).GetComponent<RectTransform>().sizeDelta = new Vector2((wiquewe[0] - 30f) * Mathf.Clamp01((timeProg - 0.3f) * 4f), 50f);
            hinter.transform.GetChild(5).GetComponent<Text>().text = coquewe[0]; hinter.transform.GetChild(6).GetComponent<Text>().text = tiquewe[0];
            hinter.transform.GetChild(5).GetComponent<Text>().color = new Color(0, 0, 0, Mathf.Clamp01((timeProg - 0.3f) * 4f));
            hinter.transform.GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0, boquewe[0]);
        } else hinter.SetActive(false);
    }
}
