using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Selector de Culoare
    // Folosit pentru elementul de selectare a culorii
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    // Lista de elenete si proprietati pentru clasa
    public RectTransform cav;
    public bool Touchy;
    public GameObject CoSlPanel;
    public RectTransform Slider2Handle;
    public Slider HueHSVSlide;
    public List<Image> SliderRGB;
    public List<Slider> RGBSlides;
    public Image ClrPrev;
    public Image ClrPrev2;
    public Color ActualColor;
    public bool ModeHSV;
    public string Key;
    public GameObject EditorOfColor;
    public bool HDRMode;
    public Slider HDRMult;
    public Image HDRD;
    public Image HDRD2;
    public bool ALPMode;
    public Slider ALPMult;
    public Image ALPD;

    // Functie executata cand elementul este activat pentru prima data
    public void Start() {
        for(int i = 0; i < RGBSlides.Count; i++) {
            RGBSlides[i].transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(130f, (cav.rect.width - 1040f) / 2f);
            RGBSlides[i].GetComponent<RectTransform>().sizeDelta = new Vector2((cav.rect.width - 1060f) / 2f, 120f);
        } ClrPrev.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2((cav.rect.width - 1040f) / 2f, 375f);
        HueHSVSlide.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2((cav.rect.width - 1640f) / 2f, 800f);
        HueHSVSlide.GetComponent<RectTransform>().sizeDelta = new Vector2(790f, (cav.rect.width - 1660f) / 2f);
        HDRMult.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, (cav.rect.width - 1320f));
        HDRMult.GetComponent<RectTransform>().sizeDelta = new Vector2((cav.rect.width - 1340f), 90f);
        HDRMult.transform.parent.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(50f, (1016f - cav.rect.width) / 2f);
        ALPMult.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(100f, (cav.rect.width - 1320f));
        ALPMult.GetComponent<RectTransform>().sizeDelta = new Vector2((cav.rect.width - 1340f), 90f);
        ALPMult.transform.parent.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(50f, (1016f - cav.rect.width) / 2f);
    }

    // Functie executata cand selectorul de culoare este mutat prin miscare
    public void OnDrg(bool draggg) {
        Touchy = draggg;
    }

    // Functie folosita pentru a seta variabila 'ModeHSV'
    public void HSVTog(bool HSV) {
        ModeHSV = HSV;
    }

    // Functie folosita pentru a aplica culoarea generata in meniul de culoare
    public void ApplyColor() {
        if(ALPMode) {
            PlayerPrefs.SetFloat(Key + "R", ActualColor.r * 255f);
            PlayerPrefs.SetFloat(Key + "G", ActualColor.g * 255f);
            PlayerPrefs.SetFloat(Key + "B", ActualColor.b * 255f);
            PlayerPrefs.SetFloat(Key + "A", ALPMult.value * 255f);
        } else {
            PlayerPrefs.SetFloat(Key + "R", ActualColor.r);
            PlayerPrefs.SetFloat(Key + "G", ActualColor.g);
            PlayerPrefs.SetFloat(Key + "B", ActualColor.b);
        } if (EditorOfColor.GetComponent<AnalogSpedometer>()) EditorOfColor.GetComponent<AnalogSpedometer>().OnColorModdify();
        else if (EditorOfColor.GetComponent<AnalogDisplay>()) EditorOfColor.GetComponent<AnalogDisplay>().OnColorModdify();
        else if (EditorOfColor.GetComponent<LCDSpedometer>()) EditorOfColor.GetComponent<LCDSpedometer>().OnColorModdify();
        else if (EditorOfColor.GetComponent<LCDDisplay>()) EditorOfColor.GetComponent<LCDDisplay>().OnColorModdify();
        else if (EditorOfColor.GetComponent<SelectDrag>()) EditorOfColor.GetComponent<SelectDrag>().OnColorModdify();
        else if (EditorOfColor.GetComponent<AnalogSpeedbar>()) EditorOfColor.GetComponent<AnalogSpeedbar>().OnColorModdify();
        else if (EditorOfColor.GetComponent<CircSpeedBar>()) EditorOfColor.GetComponent<CircSpeedBar>().OnColorModdify();
        else if (EditorOfColor.GetComponent<LCDSpeedbar>()) EditorOfColor.GetComponent<LCDSpeedbar>().OnColorModdify();
        else if (EditorOfColor.GetComponent<LCDCircSpeedbar>()) EditorOfColor.GetComponent<LCDCircSpeedbar>().OnColorModdify();
        else if (EditorOfColor.GetComponent<Line>()) EditorOfColor.GetComponent<Line>().OnColorModdify();
        else if (EditorOfColor.GetComponent<TextComp>()) EditorOfColor.GetComponent<TextComp>().OnColorModdify();
        else if (EditorOfColor.GetComponent<ImageComponent>()) EditorOfColor.GetComponent<ImageComponent>().OnColorModdify();
        else if (EditorOfColor.GetComponent<Compass>()) EditorOfColor.GetComponent<Compass>().OnColorModdify();
        else if (EditorOfColor.GetComponent<TextEffect>()) EditorOfColor.GetComponent<TextEffect>().OnColorModdify();
        else if (EditorOfColor.GetComponent<MusicPlayer>()) EditorOfColor.GetComponent<MusicPlayer>().OnColorModdify();
        else if (EditorOfColor.GetComponent<LayoutModd>()) EditorOfColor.GetComponent<LayoutModd>().LoadColor();
        else if (EditorOfColor.GetComponent<BackgroundT>()) EditorOfColor.GetComponent<BackgroundT>().OnColorRecieve();
        else if (EditorOfColor.GetComponent<MapSystem>()) EditorOfColor.GetComponent<MapSystem>().OnColorModdify();
        else if (EditorOfColor.GetComponent<MenuNav>()) EditorOfColor.GetComponent<MenuNav>().OnColorModdify();
        else if (EditorOfColor.GetComponent<SkinSelect>()) EditorOfColor.GetComponent<SkinSelect>().OnColorModdify();
    }

    // Functie executata cand un element cere o schimbare de culoare
    public void RequestChangeColor(string FKey, GameObject EOC) { Key = FKey; CoSlPanel.SetActive(true);
        ActualColor = new Color(PlayerPrefs.GetFloat(Key + "R"), PlayerPrefs.GetFloat(Key + "G"), PlayerPrefs.GetFloat(Key + "B"));
        EditorOfColor = EOC; if (EditorOfColor.GetComponent<TextEffect>()) HDRMode = true; else HDRMode = false;
        if (EditorOfColor.GetComponent<LayoutModd>()) ALPMode = true; else ALPMode = false;
    }

    // Functie chemata in fiecare cadru | Functie folosita pentru a actualiza cifrele elementului cand este folosit
    void Update() {
        float H; float S; float V; Color.RGBToHSV(ActualColor, out H, out S, out V);
        if (ModeHSV) {
            ActualColor = Color.HSVToRGB(HueHSVSlide.value, ((Slider2Handle.anchoredPosition.x + 378f) / 756f), ((Slider2Handle.anchoredPosition.y + 378f) / 756f));
            RGBSlides[0].value = ActualColor.r; RGBSlides[1].value = ActualColor.g; RGBSlides[2].value = ActualColor.b;
        } else {
            ActualColor = new Color(RGBSlides[0].value, RGBSlides[1].value, RGBSlides[2].value);
            Slider2Handle.anchoredPosition = new Vector2(Mathf.Clamp((S - 0.5f) * 756f, - 378f, 378f), Mathf.Clamp((V - 0.5f) * 756f, -378f, 378f)); HueHSVSlide.value = H;
        } if (HDRMode) { HDRD2.color = ActualColor; ActualColor *= (HDRMult.value * HDRMult.value * HDRMult.value);
            HDRD.color = ActualColor * 10f; HDRMult.transform.parent.gameObject.SetActive(true); }
        else HDRMult.transform.parent.gameObject.SetActive(false); ClrPrev.color = ActualColor; ClrPrev2.color = Color.HSVToRGB(H, 1f, 1f);
        if (ALPMode) { ALPD.color = ActualColor; ALPMult.transform.parent.gameObject.SetActive(true); } else ALPMult.transform.parent.gameObject.SetActive(false);
        SliderRGB[0].color = new Color(0f, ActualColor.g, ActualColor.b);
        SliderRGB[1].color = new Color(ActualColor.r, 0f, ActualColor.b);
        SliderRGB[2].color = new Color(ActualColor.r, ActualColor.g, 0f);
        SliderRGB[3].color = new Color(1f, ActualColor.g, ActualColor.b);
        SliderRGB[4].color = new Color(ActualColor.r, 1f, ActualColor.b);
        SliderRGB[5].color = new Color(ActualColor.r, ActualColor.g, 1f);

        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved && Touchy) { 
                Slider2Handle.anchoredPosition += touch.deltaPosition;
                Slider2Handle.anchoredPosition = new Vector2(Mathf.Clamp(Slider2Handle.anchoredPosition.x, -378f, 378f), Mathf.Clamp(Slider2Handle.anchoredPosition.y, -378f, 378f)); }
        }
    }
}
