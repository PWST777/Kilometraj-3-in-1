using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextComp : MonoBehaviour
{

    public SelectDrag Sdrg;
    public SPEEDMANAGER SM;
    public ColorPicker CoPi;
    public TextEffect TeEf;
    public bool SceneMode;
    public int ElementID;
    public bool LoadMode;
    public GameObject EditorModPanel;

    public TextMeshProUGUI MyText;
    public TextMeshProUGUI MyText2;

    public List<Sprite> Textsur;
    public RectTransform TCont; public InputField lilb;
    public int Width; public InputField WSlide;
    public int Height; public InputField HSlide;
    public string Txt; public InputField TxtInput;
    public int fontss; public Dropdown fonts;
    public int fontssF; public Dropdown fontsF;
    public List<GameObject> UIFontSel; public Dropdown fonts2;
    public TMP_FontAsset actfont;
    public TMP_FontAsset effectfont;
    public int BoldItal; public Toggle Bold; public Toggle Italic;
    public int Dynamic; public Dropdown Dyn;
    public int Mode; public Dropdown DF;
    public int FontSize; public InputField FontSizeInput;
    public int MaxDi; public InputField MaximDi;
    public int MaxDe; public InputField MaximDe;
    public int RefRate; public InputField RRT;
    public float LineSpace; public InputField LnSpcInput;
    public float Kerning; public InputField KernInput;
    public Color TextC; public Image Tcl;
    public int AllignMode; public List<Image> Buttns;
    public int WrapM; public Toggle HorWr;
    public Button SpecialEffect;
    public Button DelObj; public InputField soo;
    public GameObject dynMenu;
    public bool HasStroke; public float RTimel;

    void Start() {
        SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        if (LoadMode) OnSelectOfElement();
        OnComponentTextModdify();
        OnColorModdify();
        LoadEffect();
    }

    public void OnSelectOfElement() { if (!SceneMode) { LoadMode = true;
        WSlide.onSubmit.RemoveAllListeners(); WSlide.onSubmit.AddListener((_) => OnComponentTextModdify());
        HSlide.onSubmit.RemoveAllListeners(); HSlide.onSubmit.AddListener((_) => OnComponentTextModdify());
        MaximDe.onSubmit.RemoveAllListeners(); MaximDe.onSubmit.AddListener((_) => OnComponentTextModdify());
        MaximDi.onSubmit.RemoveAllListeners(); MaximDi.onSubmit.AddListener((_) => OnComponentTextModdify());
        RRT.onSubmit.RemoveAllListeners(); RRT.onSubmit.AddListener((_) => OnComponentTextModdify());
        TxtInput.onValueChanged.RemoveAllListeners(); TxtInput.onValueChanged.AddListener((_) => OnComponentTextModdify());
        fonts.onValueChanged.RemoveAllListeners(); fonts.onValueChanged.AddListener((_) => OnFontChange());
        fonts2.onValueChanged.RemoveAllListeners(); fonts2.onValueChanged.AddListener((_) => OnFontChange());
        fontsF.onValueChanged.RemoveAllListeners(); fontsF.onValueChanged.AddListener((_) => OnFontChange());
        Bold.onValueChanged.RemoveAllListeners(); Bold.onValueChanged.AddListener((_) => OnComponentTextModdify());
        Dyn.onValueChanged.RemoveAllListeners(); Dyn.onValueChanged.AddListener((_) => OnComponentTextModdify());
        DF.onValueChanged.RemoveAllListeners(); DF.onValueChanged.AddListener((_) => OnComponentTextModdify());
        Italic.onValueChanged.RemoveAllListeners(); Italic.onValueChanged.AddListener((_) => OnComponentTextModdify());
        LnSpcInput.onSubmit.RemoveAllListeners(); LnSpcInput.onSubmit.AddListener((_) => OnComponentTextModdify());
        FontSizeInput.onSubmit.RemoveAllListeners(); FontSizeInput.onSubmit.AddListener((_) => OnComponentTextModdify());
        KernInput.onSubmit.RemoveAllListeners(); KernInput.onSubmit.AddListener((_) => OnComponentTextModdify());
        HorWr.onValueChanged.RemoveAllListeners(); HorWr.onValueChanged.AddListener((_) => OnComponentTextModdify());
        SpecialEffect.onClick.RemoveAllListeners(); SpecialEffect.onClick.AddListener(OnSpecialEffect);
        for(int b = 0; b < 9; b++) { int lb = b;
            Buttns[lb].GetComponent<Button>().onClick.RemoveAllListeners(); Buttns[lb].GetComponent<Button>().onClick.AddListener(() => ChangeAllignMode(lb)); }
        DelObj.onClick.RemoveAllListeners(); DelObj.onClick.AddListener(() => DeleteObj(69));
        soo.onSubmit.RemoveAllListeners(); soo.onSubmit.AddListener((_) => ModSOrtO());
        Tcl.GetComponent<Button>().onClick.RemoveAllListeners(); Tcl.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("TXTEXTCL")); }
        Width = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXWIDT", 420);
        Height = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXHEIG", 260);
        Txt = PlayerPrefs.GetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXTEXT", "Text");
        fontss = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXFONT", 0);
        fontssF = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXFONTF", 0);
        BoldItal = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXBOIT", 0);
        LineSpace = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXLNSP", 0f);
        Kerning = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXKERN", 0f);
        AllignMode = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXALMD", 4);
        FontSize = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXFTSZ", 150);
        WrapM = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXWRMD", 0);
        Mode = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXDYNAC", 2);
        MaxDe = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXMXDE", 1);
        MaxDi = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXMXDI", 3);
        RefRate = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXRRAT", 5);
        Dynamic = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXDYNA", 0);
        ChangeAllignMode(AllignMode); string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        TextC = new Color(PlayerPrefs.GetFloat(colorstart + "TXTEXTCLR", 1f), PlayerPrefs.GetFloat(colorstart + "TXTEXTCLG", 1f), PlayerPrefs.GetFloat(colorstart + "TXTEXTCLB", 1f));
        int l = PlayerPrefs.GetInt("MAINLANG"); if (!SceneMode) { DF.options.Clear();
            DF.options.Add(new Dropdown.OptionData((l == 0) ? "Speed" : (l == 1) ? "Vitesse" : (l == 2) ? "Geschwindigkeit" : "Viteză"));
            DF.options.Add(new Dropdown.OptionData("RPM"));
            DF.options.Add(new Dropdown.OptionData((l == 0) ? "Odometer" : (l == 1) ? "Odomètre" : (l == 2) ? "Kilometerzähler" : "Odometru"));
            DF.options.Add(new Dropdown.OptionData((l == 0) ? "Clock" : (l == 1) ? "Horloge" : (l == 2) ? "Uhr" : "Ceas"));
            DF.options.Add(new Dropdown.OptionData((l == 0) ? "Total Time (h:mm)" : (l == 1) ? "Durée totale (h:mm)" : (l == 2) ? "Gesamtzeit (h:mm)" : "Timp total (h:mm)"));
            DF.options.Add(new Dropdown.OptionData((l == 0) ? "Total Time (h)" : (l == 1) ? "Durée totale (h)" : (l == 2) ? "Gesamtzeit (h)" : "Timp Total (h)"));
            DF.options.Add(new Dropdown.OptionData((l == 0) ? "Date" : (l == 1) ? "Date" : (l == 2) ? "Datum" : "Dată"));
            DF.options.Add(new Dropdown.OptionData((l == 0) ? "Trip Dist." : (l == 1) ? "Distance du trajet" : (l == 2) ? "Reiseentfernung" : "Distanță plimbare"));
            DF.options.Add(new Dropdown.OptionData((l == 0) ? "Trip Time (m:ss)" : (l == 1) ? "Temps de trajet (m:ss)" : (l == 2) ? "Reisezeit (m:ss)" : "Timp plimbare (m:ss)"));
            DF.options.Add(new Dropdown.OptionData((l == 0) ? "Trip Time (h:mm)" : (l == 1) ? "Temps de trajet (h:mm)" : (l == 2) ? "Reisezeit (h:mm)" : "Timp plimbare (h:mm)"));
            DF.options.Add(new Dropdown.OptionData((l == 0) ? "Trip Time (h)" : (l == 1) ? "Temps de trajet (h)" : (l == 2) ? "Reisezeit (h)" : "Timp plimbare (h)"));
            DF.options.Add(new Dropdown.OptionData((l == 0) ? "Max Speed" : (l == 1) ? "Vitesse maximale" : (l == 2) ? "Max. Geschwindigkeit" : "Viteza maxima"));
            DF.options.Add(new Dropdown.OptionData((l == 0) ? "Avg. Speed" : (l == 1) ? "Vitesse moyenne" : (l == 2) ? "Durchschnittsgeschwindigkeit" : "Viteza medie"));
            if ((PlayerPrefs.GetInt("SPEEDSOURCE") == 2 || Mode >= 12)) {
                DF.options.Add(new Dropdown.OptionData((l == 0) ? "Engine Load" : (l == 1) ? "Charge du moteur" : (l == 2) ? "Motorlast" : "Sarcina motorului"));
                DF.options.Add(new Dropdown.OptionData((l == 0) ? "Coolant Temp." : (l == 1) ? "Température du moteur" : (l == 2) ? "Motortemperatur" : "Temperatura motorului"));
                DF.options.Add(new Dropdown.OptionData((l == 0) ? "Throttle" : (l == 1) ? "Étrangler" : (l == 2) ? "Gaspedal" : "Accelerație"));
                DF.options.Add(new Dropdown.OptionData((l == 0) ? "Fuel (%)" : (l == 1) ? "Carburant (%)" : (l == 2) ? "Kraftstoff (%)" : "Combustibil (%)"));
            } DF.GetComponent<DropdownControl>().scaleCoef = (l == 0) ? 1 : (l == 1) ? 0.9f : (l == 2) ? 0.7f : 0.9f;
            DF.GetComponent<DropdownControl>().UpdateScale();
            Dyn.value = Dynamic; DF.value = Mode; MaximDe.text = MaxDe + ""; MaximDi.text = MaxDi + "";
            WSlide.text = Width + ""; HSlide.text = Height + ""; TxtInput.text = Txt; fontsF.value = fontssF; if(fontssF == 0) fonts.value = fontss;
            if (fontssF == 1) fonts2.value = (fontss - 5); FontSizeInput.text = FontSize + ""; RRT.text = RefRate + "";
            if (BoldItal % 2 == 0) Bold.isOn = false; else Bold.isOn = true; if (BoldItal >= 2) Italic.isOn = true; else Italic.isOn = false;
            if (WrapM == 0) HorWr.isOn = false; else HorWr.isOn = true; KernInput.text = Kerning.ToString(CultureInfo.InvariantCulture);
            LnSpcInput.text = LineSpace.ToString(CultureInfo.InvariantCulture); soo.text = gameObject.transform.GetSiblingIndex() + ""; }
        StartCoroutine(yesofcourse());
    }

    public IEnumerator yesofcourse() {
        yield return null;
        LoadMode = false;
    }

    public void OnSpecialEffect() {
        if (!SceneMode && !LoadMode) { fontssF = fontsF.value; if(fontssF == 0) fontss = fonts.value;
            if (fontssF == 1) fontss = fonts2.value + 5; if (fontssF == 2) fontss = 13; }
        if (effectfont == null) { effectfont = TeEf.RequestRegular(fontss, true); MyText2.font = effectfont; }
        TeEf.OnTextEffectModTrigger(gameObject, fontss, HasStroke, ("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID));
    }

    public void OnFontChange() {
        if (!SceneMode && !LoadMode) { fontssF = fontsF.value; fontss = fonts.value; } LoadEffect();
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXFONT", fontss);
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXFONTF", fontssF);
        if (!SceneMode) for (int g = 0; g < 3; g++) if (fontssF == g) UIFontSel[g].SetActive(true); else UIFontSel[g].SetActive(false);
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TMPMBODBEV", 0) == 1) MyText.fontMaterial.EnableKeyword("BEVEL_ON");
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TMPMSODBEV", 0) == 1) MyText2.fontMaterial.EnableKeyword("BEVEL_ON");
    }

    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject);
    }

    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        TextC = new Color(PlayerPrefs.GetFloat(colorstart + "TXTEXTCLR", 1f), PlayerPrefs.GetFloat(colorstart + "TXTEXTCLG", 1f), PlayerPrefs.GetFloat(colorstart + "TXTEXTCLB", 1f));
        MyText.color = TextC; if (!SceneMode) {Tcl.color = TextC;}
    }

    public void ChangeAllignMode(int AM) {
        if (!SceneMode) { foreach (Image b in Buttns) b.sprite = Textsur[0];
        Buttns[AM].sprite = Textsur[1]; }
        if (AM == 0) { MyText.alignment = TextAlignmentOptions.TopLeft; MyText2.alignment = TextAlignmentOptions.TopLeft; }
        if (AM == 1) { MyText.alignment = TextAlignmentOptions.Top; MyText2.alignment = TextAlignmentOptions.Top; }
        if (AM == 2) { MyText.alignment = TextAlignmentOptions.TopRight; MyText2.alignment = TextAlignmentOptions.TopRight; }
        if (AM == 3) { MyText.alignment = TextAlignmentOptions.MidlineLeft; MyText2.alignment = TextAlignmentOptions.MidlineLeft; }
        if (AM == 4) { MyText.alignment = TextAlignmentOptions.Midline; MyText2.alignment = TextAlignmentOptions.Midline; }
        if (AM == 5) { MyText.alignment = TextAlignmentOptions.MidlineRight; MyText2.alignment = TextAlignmentOptions.MidlineRight; }
        if (AM == 6) { MyText.alignment = TextAlignmentOptions.BottomLeft; MyText2.alignment = TextAlignmentOptions.BottomLeft; }
        if (AM == 7) { MyText.alignment = TextAlignmentOptions.Bottom; MyText2.alignment = TextAlignmentOptions.Bottom; }
        if (AM == 8) { MyText.alignment = TextAlignmentOptions.BottomRight; MyText2.alignment = TextAlignmentOptions.BottomRight; }
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXALMD", AM);
    }

    public void OnComponentTextModdify() {
        if(!SceneMode && !LoadMode) { Width = int.Parse(WSlide.text); Height = int.Parse(HSlide.text); Txt = TxtInput.text;
            fontssF = fontsF.value; if (fontssF == 0) fontss = fonts.value; Dynamic = Dyn.value; MaxDi = int.Parse(MaximDi.text);
            if (fontssF == 1) fontss = fonts2.value + 5; if (fontssF == 2) fontss = 13; Mode = DF.value; RefRate = int.Parse(RRT.text);
            BoldItal = 0; if (Bold.isOn) BoldItal += 1; if (Italic.isOn) BoldItal += 2; MaxDe = int.Parse(MaximDe.text);
            WrapM = 0; if (HorWr.isOn) WrapM = 1; LineSpace = float.Parse(LnSpcInput.text, CultureInfo.InvariantCulture);
            Kerning = float.Parse(KernInput.text, CultureInfo.InvariantCulture); FontSize = int.Parse(FontSizeInput.text);
        } if (Dynamic == 0) { MyText.text = Txt; if (!SceneMode) dynMenu.SetActive(false); } else if (Dynamic == 1) {
            if (!SceneMode) dynMenu.SetActive(true); if (Mode == 3 || Mode == 4 || Mode == 8 || Mode == 9) MaxDe = 0;
            if (Mode == 3) MaxDi = 4; if (Mode == 6) MaxDi = 6;
            if (Mode == 4 || Mode == 8 || Mode == 9) MaxDi = Mathf.Max(MaxDi, 3);
            if (!SceneMode) {
                if (Mode == 3 || Mode == 4 || Mode == 8 || Mode == 9) MyText.text = "0:" + new string('0', 2);
                else if (Mode == 5 || Mode == 10) {
                    if (MaxDe > 0) MyText.text = "0." + new string('0', MaxDe) + "h"; else MyText.text = "0h";
                } else if (Mode == 6) MyText.text = DateTime.Now.Day.ToString("D2") + "." + DateTime.Now.Month.ToString("D2") + "." + DateTime.Now.Year.ToString().Substring(2, 2);
                else if (MaxDe > 0) MyText.text = "0." + new string('0', MaxDe);
                else MyText.text = "0";
            }
        } gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Width, Height);
        if(!SceneMode) { TCont.sizeDelta = new Vector2(TCont.sizeDelta.x, 50f + lilb.preferredHeight); }
        if (BoldItal == 0) MyText.fontStyle = FontStyles.Normal; if (BoldItal == 1) MyText.fontStyle = FontStyles.Bold;
        if (BoldItal == 2) MyText.fontStyle = FontStyles.Italic; if (BoldItal == 3) MyText.fontStyle = FontStyles.Bold | FontStyles.Italic;
        MyText.enableWordWrapping = true; MyText.lineSpacing = LineSpace; MyText.characterSpacing = Kerning;
        MyText.fontSize = FontSize;
        if (WrapM == 1) MyText.overflowMode = TextOverflowModes.Truncate; else MyText.overflowMode = TextOverflowModes.Overflow;
        if(HasStroke) { MyText2.gameObject.SetActive(true); MyText2.text = Txt; MyText2.alignment = MyText.alignment;
            if (BoldItal == 0) MyText2.fontStyle = FontStyles.Normal; if (BoldItal == 1) MyText2.fontStyle = FontStyles.Bold;
            if (BoldItal == 2) MyText2.fontStyle = FontStyles.Italic; if (BoldItal == 3) MyText2.fontStyle = FontStyles.Bold | FontStyles.Italic;
            MyText2.enableWordWrapping = true; MyText2.lineSpacing = LineSpace; MyText2.characterSpacing = Kerning;
            if (WrapM == 1) MyText2.overflowMode = TextOverflowModes.Truncate; else MyText2.overflowMode = TextOverflowModes.Overflow;
            MyText2.fontSize = FontSize; MyText2.text = MyText.text;
        } else MyText2.gameObject.SetActive(false);
        if (!SceneMode && !LoadMode) {
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXWIDT", Width);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXHEIG", Height);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXTEXT", Txt);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXBOIT", BoldItal);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXWRMD", WrapM);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXLNSP", LineSpace);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXKERN", Kerning);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXFTSZ", FontSize);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXDYNAC", Mode);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXMXDE", MaxDe);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXMXDI", MaxDi);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXRRAT", RefRate);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TXDYNA", Dynamic);
        }
    }

    public void ModSOrtO() {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "SO", int.Parse(soo.text));
        gameObject.GetComponent<SortOrderOrg>().Apply();
    }

    public void LoadEffect() { string key = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        if (!SceneMode && !LoadMode) { fontssF = fontsF.value; if (fontssF == 0) fontss = fonts.value;
            if (fontssF == 1) fontss = fonts2.value + 5; if (fontssF == 2) fontss = 13; }
        TMP_FontAsset bodyFont = TeEf.RequestRegular(fontss, false);
        MyText.font = bodyFont; MyText.fontMaterial = new Material(bodyFont.material);
        TMP_FontAsset strokeFont = TeEf.RequestRegular(fontss, true);
        MyText2.font = strokeFont; MyText2.fontMaterial = new Material(strokeFont.material);
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TMPMEFFECT", 0) == 1) { 
            if (PlayerPrefs.GetInt(key + "TMPMBCOLMO", 0) == 0) MyText.color = new Color(PlayerPrefs.GetFloat(key + "BTXTCOLR", 1f), PlayerPrefs.GetFloat(key + "BTXTCOLG", 1f), PlayerPrefs.GetFloat(key + "BTXTCOLB", 1f)); else MyText.color = Color.white;
        if (PlayerPrefs.GetInt(key + "TMPMBCOLMO", 0) == 1) { MyText.enableVertexGradient = true;
            if (PlayerPrefs.GetInt(key + "TMPMBGRTYP", 0) == 0) { MyText.colorGradient = new VertexGradient(new Color (PlayerPrefs.GetFloat(key + "BTXTG21R", 1f), PlayerPrefs.GetFloat(key + "BTXTG21G", 0f), PlayerPrefs.GetFloat(key + "BTXTG21B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "BTXTG22R", 1f), PlayerPrefs.GetFloat(key + "BTXTG22G", 1f), PlayerPrefs.GetFloat(key + "BTXTG22B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "BTXTG21R", 1f), PlayerPrefs.GetFloat(key + "BTXTG21G", 0f), PlayerPrefs.GetFloat(key + "BTXTG21B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "BTXTG22R", 1f), PlayerPrefs.GetFloat(key + "BTXTG22G", 1f), PlayerPrefs.GetFloat(key + "BTXTG22B", 0f))); }
            if (PlayerPrefs.GetInt(key + "TMPMBGRTYP", 0) == 1) { MyText.colorGradient = new VertexGradient(new Color (PlayerPrefs.GetFloat(key + "BTXTG21R", 1f), PlayerPrefs.GetFloat(key + "BTXTG21G", 0f), PlayerPrefs.GetFloat(key + "BTXTG21B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "BTXTG21R", 1f), PlayerPrefs.GetFloat(key + "BTXTG21G", 0f), PlayerPrefs.GetFloat(key + "BTXTG21B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "BTXTG22R", 1f), PlayerPrefs.GetFloat(key + "BTXTG22G", 1f), PlayerPrefs.GetFloat(key + "BTXTG22B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "BTXTG22R", 1f), PlayerPrefs.GetFloat(key + "BTXTG22G", 1f), PlayerPrefs.GetFloat(key + "BTXTG22B", 0f))); }
            if (PlayerPrefs.GetInt(key + "TMPMBGRTYP", 0) == 2) { MyText.colorGradient = new VertexGradient(new Color (PlayerPrefs.GetFloat(key + "BTXTG41R", 1f), PlayerPrefs.GetFloat(key + "BTXTG41G", 0f), PlayerPrefs.GetFloat(key + "BTXTG41B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "BTXTG42R", 1f), PlayerPrefs.GetFloat(key + "BTXTG42G", 1f), PlayerPrefs.GetFloat(key + "BTXTG42B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "BTXTG43R", 0f), PlayerPrefs.GetFloat(key + "BTXTG43G", 1f), PlayerPrefs.GetFloat(key + "BTXTG43B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "BTXTG44R", 0f), PlayerPrefs.GetFloat(key + "BTXTG44G", 1f), PlayerPrefs.GetFloat(key + "BTXTG44B", 1f))); }
        } else MyText.enableVertexGradient = false;

        if (PlayerPrefs.GetInt(key + "TMPMSCOLMO", 0) == 0) MyText2.color = new Color(PlayerPrefs.GetFloat(key + "STXTCOLR", 1f), PlayerPrefs.GetFloat(key + "STXTCOLG", 1f), PlayerPrefs.GetFloat(key + "STXTCOLB", 1f)); else MyText2.color = Color.white;
        if (PlayerPrefs.GetInt(key + "TMPMSCOLMO", 0) == 1) { MyText2.enableVertexGradient = true;
            if (PlayerPrefs.GetInt(key + "TMPMSGRTYP", 0) == 0) { MyText2.colorGradient = new VertexGradient(new Color (PlayerPrefs.GetFloat(key + "STXTG21R", 1f), PlayerPrefs.GetFloat(key + "STXTG21G", 0f), PlayerPrefs.GetFloat(key + "STXTG21B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "STXTG22R", 1f), PlayerPrefs.GetFloat(key + "STXTG22G", 1f), PlayerPrefs.GetFloat(key + "STXTG22B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "STXTG21R", 1f), PlayerPrefs.GetFloat(key + "STXTG21G", 0f), PlayerPrefs.GetFloat(key + "STXTG21B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "STXTG22R", 1f), PlayerPrefs.GetFloat(key + "STXTG22G", 1f), PlayerPrefs.GetFloat(key + "STXTG22B", 0f))); }
            if (PlayerPrefs.GetInt(key + "TMPMSGRTYP", 0) == 1) { MyText2.colorGradient = new VertexGradient(new Color (PlayerPrefs.GetFloat(key + "STXTG21R", 1f), PlayerPrefs.GetFloat(key + "STXTG21G", 0f), PlayerPrefs.GetFloat(key + "STXTG21B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "STXTG21R", 1f), PlayerPrefs.GetFloat(key + "STXTG21G", 0f), PlayerPrefs.GetFloat(key + "STXTG21B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "STXTG22R", 1f), PlayerPrefs.GetFloat(key + "STXTG22G", 1f), PlayerPrefs.GetFloat(key + "STXTG22B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "STXTG22R", 1f), PlayerPrefs.GetFloat(key + "STXTG22G", 1f), PlayerPrefs.GetFloat(key + "STXTG22B", 0f))); }
            if (PlayerPrefs.GetInt(key + "TMPMSGRTYP", 0) == 2) { MyText2.colorGradient = new VertexGradient(new Color (PlayerPrefs.GetFloat(key + "STXTG41R", 1f), PlayerPrefs.GetFloat(key + "STXTG41G", 0f), PlayerPrefs.GetFloat(key + "STXTG41B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "STXTG42R", 1f), PlayerPrefs.GetFloat(key + "STXTG42G", 1f), PlayerPrefs.GetFloat(key + "STXTG42B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "STXTG43R", 0f), PlayerPrefs.GetFloat(key + "STXTG43G", 1f), PlayerPrefs.GetFloat(key + "STXTG43B", 0f)),
                new Color(PlayerPrefs.GetFloat(key + "STXTG44R", 0f), PlayerPrefs.GetFloat(key + "STXTG44G", 1f), PlayerPrefs.GetFloat(key + "STXTG44B", 1f))); }
        } else MyText2.enableVertexGradient = false;

        MyText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, PlayerPrefs.GetFloat(key + "TMPMBDILAT", 0f));
        MyText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, PlayerPrefs.GetFloat(key + "TMPMBSOFTN", 0f));
        MyText2.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, PlayerPrefs.GetFloat(key + "TMPMSDILAT", 0f));
        MyText2.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, PlayerPrefs.GetFloat(key + "TMPMSSOFTN", 0f));

        if (PlayerPrefs.GetInt(key + "TMPMSTROKE", 0) == 1) HasStroke = true; else HasStroke = false; }
        OnComponentTextModdify();
        MyText.UpdateMeshPadding(); MyText.havePropertiesChanged = true; MyText.SetMaterialDirty();
        MyText2.UpdateMeshPadding(); MyText2.havePropertiesChanged = true; MyText2.SetMaterialDirty();
    }

    public void DeleteObj(int wf) {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TYPE", 69696);
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ") + 1);
        Sdrg.OnElemSelect(72769); Destroy(gameObject);
    }

    void Update() {
        if (SceneMode && Dynamic == 1) {
            RTimel += Time.deltaTime;
            if (RTimel >= (1f / RefRate)) {
                if (Mode == 0) { // Speed
                    MyText.text = SM.InterpSpeed.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 1) { // RPM
                    MyText.text = SM.RPM.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 2) { // Odometer / Km Counter
                    MyText.text = SM.DistanceMade.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 3) { // Clock
                    MyText.text = DateTime.Now.Hour.ToString("D2") + ":" + DateTime.Now.Minute.ToString("D2");
                } else if (Mode == 4) { // Total Time (h:mm)
                    MyText.text = (int)(SM.TimeMade / 3600f) + ":" + ((int)(SM.TimeMade / 60f) % 60).ToString("D2");
                } else if (Mode == 5) { // Total Time (h)
                    MyText.text = ((SM.TimeMade / 3600f)).ToString("F" + MaxDe).Replace(",", ".") + "h";
                } else if (Mode == 6) { // Date (dd.mm.yy)
                    MyText.text = DateTime.Now.Day.ToString("D2") + "." + DateTime.Now.Month.ToString("D2") + "." + DateTime.Now.Year.ToString().Substring(2,2);
                } else if (Mode == 7) { // Trip Distance
                    MyText.text = SM.TripDistanceMade.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 8) { // Trip Time (m:ss)
                    MyText.text = (int)(SM.TripTimeMade / 60f) + ":" + ((int)(SM.TripTimeMade) % 60).ToString("D2");
                } else if (Mode == 9) { // Trip Time (h:mm)
                    MyText.text = (int)(SM.TripTimeMade / 3600f) + ":" + ((int)(SM.TripTimeMade / 60f) % 60).ToString("D2");
                } else if (Mode == 10) { // Trip Time (h)
                    MyText.text = ((SM.TripTimeMade / 3600f)).ToString("F" + MaxDe).Replace(",", ".") + "h";
                } else if (Mode == 11) { // Max Speed
                    MyText.text = SM.MaxSpeed.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 12) { // Avg Speed
                    MyText.text = SM.AvgSpeed.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 13) { // Engine Load
                    MyText.text = SM.OBDLoad.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 14) { // Coolant Temp.
                    MyText.text = SM.OBDCoolant.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 15) { // Throttle
                    MyText.text = SM.OBDThrottle.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 16) { // Fuel (%)
                    MyText.text = SM.OBDFuel.ToString("F" + MaxDe).Replace(",", ".");
                } if (Mode != 3 && Mode != 6) if(Mode == 4 || Mode == 8 || Mode == 9) MyText.text = MyText.text.Substring(Mathf.Max(0, MyText.text.Length - (MaxDi + 1)), Mathf.Min(MaxDi + 1, MyText.text.Length));
                else if (MaxDe != 0) MyText.text = MyText.text.Substring(Mathf.Max(0, MyText.text.Length - (MaxDi + 1)), Mathf.Min(MaxDi + 1, MyText.text.Length));
                else MyText.text = MyText.text.Substring(Mathf.Max(0, MyText.text.Length - MaxDi), Mathf.Min(MaxDi, MyText.text.Length));
                RTimel -= (1f / RefRate); if (HasStroke) MyText2.text = MyText.text;
            }
        }
    }
}
