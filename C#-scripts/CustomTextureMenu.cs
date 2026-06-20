using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomTextureMenu : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Meniu textura personalizata
    // Folosit pentru meniul de a schimba texturile de interfata
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public bool Selected; // Vaiabila folosita pentru a arata daca elemntul este activat

    // Elemente grafice folosite pentru a forma textura personalizata
    public Image MainTexture;
    public Image MainBack;
    public Image MainGloss;
    public Image GlossSecondary;
    public Text CustomText;
    public List<Sprite> glosses;
    public bool hasTexture;
    public float refTimer;

    // Functie executata cand elementul este activat pentru prima data
    void Start() {
        MainTexture = gameObject.transform.GetChild(0).GetComponent<Image>();
        MainBack = gameObject.transform.GetChild(1).GetComponent<Image>();
        MainGloss = gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        GlossSecondary = gameObject.transform.GetChild(1).GetChild(1).GetComponent<Image>();
        CustomText = gameObject.transform.GetChild(1).GetChild(2).GetComponent<Text>();
        hasTexture = (PlayerPrefs.GetInt("UCTEXTUREEXIST", 0) == 1); SetTexture(); OnChnageSelected();
    }

    // Functie folosita pentru a actualiza interfata daca valoarea 'Selected' este modificata
    public void OnChnageSelected() {
        MainTexture.gameObject.SetActive(!Selected);
        MainBack.gameObject.SetActive(Selected);
    }

    // Functie folosita pentru a seta textura personalizata pe meniu
    public void SetTexture() {
        if(hasTexture) { // Daca o textura personalizata exista (Se incarca datele texturii: Culoare / Texura)
            Color mainBack = new Color(PlayerPrefs.GetFloat("MYUWUWU0R", 0f), PlayerPrefs.GetFloat("MYUWUWU0G", 0f), PlayerPrefs.GetFloat("MYUWUWU0B", 0f)); ;
            Color mainGloss = new Color(PlayerPrefs.GetFloat("MYUWUWU1R", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1G", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1B", 0.4f));
            int Terra = PlayerPrefs.GetInt("UTEXTUREGLOSSTEX", 0); int Gloss1d = 0;
            int Gloss2d = PlayerPrefs.GetInt("UTEXTUREGLOSSDIR", 2); float alp = MainTexture.color.a;
            MainBack.color = new Color(mainBack.r, mainBack.g, mainBack.b, alp);
            if (Terra == 0) MainGloss.color = new Color(mainGloss.r, mainGloss.g, mainGloss.b, alp); else MainGloss.color = MainBack.color;
            CustomText.color = new Color(0f, 0f, 0f, alp);
            GlossSecondary.sprite = glosses[Terra];
            GlossSecondary.color = new Color(1f, 1f, 1f, alp);
            if (Terra == 0) GlossSecondary.type = Image.Type.Sliced; else GlossSecondary.type = Image.Type.Tiled;
            MainGloss.GetComponent<RectTransform>().localScale = new Vector2((Gloss1d % 2 == 0) ? 1f : -1f, (Gloss1d / 2 == 0) ? 1f : -1f);
            GlossSecondary.GetComponent<RectTransform>().localScale = new Vector2((Gloss2d % 2 == 0) ? 1f : -1f, (Gloss2d / 2 == 0) ? 1f : -1f);
        } else { // Daca o textura personalizata nu exista (Una generata aliatoriu este folosita)
            Color mainBack = randomColor();
            Color mainGloss = Color.Lerp(mainBack, Color.white, 0.5f);
            int Terra = Random.Range(0, 2); int Gloss1d = Random.Range(0, 4);
            int Gloss2d = Random.Range(0, 4); float alp = MainTexture.color.a;
            MainBack.color = new Color(mainBack.r, mainBack.g, mainBack.b, alp);
            MainGloss.color = new Color(mainGloss.r, mainGloss.g, mainGloss.b, alp);
            CustomText.color = new Color(0f, 0f, 0f, alp);
            GlossSecondary.sprite = glosses[Terra];
            GlossSecondary.color = new Color(1f, 1f, 1f, alp);
            if (Terra == 0) GlossSecondary.type = Image.Type.Sliced; else GlossSecondary.type = Image.Type.Tiled;
            MainGloss.GetComponent<RectTransform>().localScale = new Vector2((Gloss1d % 2 == 0) ? 1f : -1f, (Gloss1d / 2 == 0) ? 1f : -1f);
            GlossSecondary.GetComponent<RectTransform>().localScale = new Vector2((Gloss2d % 2 == 0) ? 1f : -1f, (Gloss2d / 2 == 0) ? 1f : -1f);
        }
    }

    // Functie folosita pentru a genera o culoare aleatorie
    public Color randomColor() {
        float randH = Random.Range(0f, 1f);
        float randS = Random.Range(0.5f, 1f);
        float randV = Random.Range(0.5f, 1f);
        return Color.HSVToRGB(randH, randS, randV);
    }

    // Functie chemata in fiecare cadru | Functie folosita pentru a actualiza culoarea texturii curcubeu daca este selectata
    void Update() {
        if(!hasTexture) { refTimer += Time.deltaTime;
            if(refTimer > 0.3f) { refTimer -= 0.3f; SetTexture(); }
        }
    }
}
