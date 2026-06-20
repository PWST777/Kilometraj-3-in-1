using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LCDDisplay : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Display LCD
    // Folosita pentru elementul Display LCD
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public SelectDrag Sdrg; // Clasa universala pentru aranjarea, crearea si stergerea elementelor
    public ColorPicker CoPi; // Clasa universala pentru selectarea culorii
    public SPEEDMANAGER SM; // Clasa universala pentru variabile de viteza, turatie, etc.
    public bool SceneMode; // Variabila folosita pentru a determina daca elementul este editad sau utilizat
    public int ElementID; // Vaiabila numerica autonumerotata pentru identificarea elementului
    public bool LoadMode;
    public GameObject EditorModPanel;

    // Lista de elenete si proprietati pentru clasa
    public Text Burn;
    public Text ActialText;
    public Text ActialTextBurn;
    public int FontStSet; public Dropdown FStyleSelect;
    public int Mode; public Dropdown SHowOnDis;
    public int MaxDi; public InputField MxDIspl;
    public int MaxDe; public InputField MxDEcim;
    public int FontSet; public Dropdown StyleSelect;
    public Slider Weight; public List<Font> LCDS;
    public int Refrate = 4; public InputField RefRS;
    public float BurnTime = 0.1f; public InputField BTRS;
    public float LCDBurn = 0.13f; public Slider LDBurn;
    public float StretchX = 1f; public Slider LDStX;
    public float StretchY = 1f; public Slider LDStY;
    public bool MaintainProportion; public Toggle MTP;
    public bool ShowAllDigits;
    public bool ShowBackGround; public Toggle BGT;
    public bool MakeLastDigit1; public Toggle MLD1;
    public Color BackColor; public Image BCSelect;
    public Color DigitColor; public Image DGCSelect;
    public float InnerShadow = 0.5f; public Slider InnerSSlide;
    public Button DelObj; public InputField soo;
    public string LasMessage; float RTimel = 0f;
    public Font LCDF2;

    // Functie executata cand elementul este activat pentru prima data
    void Start() {
        SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        if (LoadMode) OnSelectOfElement();
        OnComponentLCDDModdify();
        OnChangeOfFont();
        OnColorModdify();
        if (StretchX <= 0.99f || StretchY <= 0.99f) if (MaintainProportion) {
        if (StretchX <= 0.99f && StretchY <= 0.99f) Burn.color = new Color(Burn.color.r, Burn.color.g, Burn.color.b, Burn.color.a / 6f);
            else Burn.color = new Color(Burn.color.r, Burn.color.g, Burn.color.b, Burn.color.a / 3.5f); }
    }

    // Functie executata cand o culoare este selectata si schimbata in meniul de culoare
    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BackColor = new Color(PlayerPrefs.GetFloat(colorstart + "LDBACKCLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "LDBACKCLG", 0.7f), PlayerPrefs.GetFloat(colorstart + "LDBACKCLB", 0.3f));
        DigitColor = new Color(PlayerPrefs.GetFloat(colorstart + "LDDGITCLR", 0.16f), PlayerPrefs.GetFloat(colorstart + "LDDGITCLG", 0.16f), PlayerPrefs.GetFloat(colorstart + "LDDGITCLB", 0.16f));
        if (!SceneMode) if (ShowBackGround) gameObject.GetComponent<Image>().color = new Color(BackColor.r, BackColor.g, BackColor.b, 1f); 
            else gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0); if (!SceneMode) { BCSelect.color = BackColor; DGCSelect.color = DigitColor; }
        Burn.color = new Color(DigitColor.r, DigitColor.g, DigitColor.b, LCDBurn);
        ActialText.color = new Color(DigitColor.r, DigitColor.g, DigitColor.b, 1f);
        ActialTextBurn.color = new Color(DigitColor.r, DigitColor.g, DigitColor.b, 1f);
        if(MaintainProportion) { if(StretchX <= 0.99f || StretchY <= 0.99f) { 
            Burn.GetComponent<LCDStretch>().effectColor = DigitColor; ActialText.GetComponent<LCDStretch>().effectColor = DigitColor;
            ActialTextBurn.GetComponent<LCDStretch>().effectColor = DigitColor;; } }
        if (BurnTime == 0f) ActialTextBurn.color = new Color(ActialText.color.r, ActialText.color.g, ActialText.color.b, 0f);
    }

    // Functie executata cand o culoare trebuie schimbata
    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject);
    }

    // Functie executata cand elementul este creat sau este selectat
    public void OnSelectOfElement() { if (!SceneMode) { LoadMode = true;
        // Aplicarea functiilor pe elementele de interfata (elemente Button, InputField etc.)
        MxDIspl.onSubmit.RemoveAllListeners(); MxDIspl.onSubmit.AddListener((_) => OnComponentLCDDModdify());
        MxDEcim.onSubmit.RemoveAllListeners(); MxDEcim.onSubmit.AddListener((_) => OnComponentLCDDModdify());
        LDBurn.onValueChanged.RemoveAllListeners(); LDBurn.onValueChanged.AddListener((_) => OnComponentLCDDModdify());
        FStyleSelect.onValueChanged.RemoveAllListeners(); FStyleSelect.onValueChanged.AddListener((_) => OnComponentLCDDModdify());
        SHowOnDis.onValueChanged.RemoveAllListeners(); SHowOnDis.onValueChanged.AddListener((_) => OnComponentLCDDModdify());
        BGT.onValueChanged.RemoveAllListeners(); BGT.onValueChanged.AddListener((_) => OnComponentLCDDModdify());
        MLD1.onValueChanged.RemoveAllListeners(); MLD1.onValueChanged.AddListener((_) => OnComponentLCDDModdify());
        MTP.onValueChanged.RemoveAllListeners(); MTP.onValueChanged.AddListener((_) => OnComponentLCDDModdify());
        LDStX.onValueChanged.RemoveAllListeners(); LDStX.onValueChanged.AddListener((_) => OnComponentLCDDModdify());
        LDStY.onValueChanged.RemoveAllListeners(); LDStY.onValueChanged.AddListener((_) => OnComponentLCDDModdify());
        InnerSSlide.onValueChanged.RemoveAllListeners(); InnerSSlide.onValueChanged.AddListener((_) => OnComponentLCDDModdify());
        RefRS.onSubmit.RemoveAllListeners(); RefRS.onSubmit.AddListener((_) => OnComponentLCDDModdify());
        BCSelect.GetComponent<Button>().onClick.RemoveAllListeners(); BCSelect.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("LDBACKCL"));
        DGCSelect.GetComponent<Button>().onClick.RemoveAllListeners(); DGCSelect.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("LDDGITCL"));
        DelObj.onClick.RemoveAllListeners(); DelObj.onClick.AddListener(() => DeleteObj(69));
        soo.onSubmit.RemoveAllListeners(); soo.onSubmit.AddListener((_) => ModSOrtO());
        StyleSelect.onValueChanged.RemoveAllListeners(); StyleSelect.onValueChanged.AddListener((_) => OnChangeOfFont());
        Weight.onValueChanged.RemoveAllListeners(); Weight.onValueChanged.AddListener((_) => OnChangeOfFont());
        BTRS.onSubmit.RemoveAllListeners(); BTRS.onSubmit.AddListener((_) => OnComponentLCDDModdify()); }
        // Incarcarea variabilelor salvate
        Mode = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDMODE", 2);
        MaxDi = (int)PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDDNSE", 3);
        MaxDe = (int)PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDDCML", 1);
        Refrate = (int)PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDREFR", 5);
        BurnTime = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDBRNT", 0.12f);
        LCDBurn = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCBRN", 0.13f);
        InnerShadow = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDISHD", 0.6f);
        StretchX = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSTRX", 1f);
        StretchY = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSTRY", 1f);
        FontSet = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDFONT", 2);
        FontStSet = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSFNT", 0);
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDBACK", 1) == 1) ShowBackGround = true; else ShowBackGround = false;
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDMLD1", 0) == 1) MakeLastDigit1 = true; else MakeLastDigit1 = false;
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDMTPR", 1) == 1) MaintainProportion = true; else MaintainProportion = false;
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BackColor = new Color(PlayerPrefs.GetFloat(colorstart + "LDBACKCLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "LDBACKCLG", 0.7f), PlayerPrefs.GetFloat(colorstart + "LDBACKCLB", 0.3f));
        int l = PlayerPrefs.GetInt("MAINLANG"); if (!SceneMode) { SHowOnDis.options.Clear();
            SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Speed" : (l == 1) ? "Vitesse" : (l == 2) ? "Geschwindigkeit" : "Viteză"));
            SHowOnDis.options.Add(new Dropdown.OptionData("RPM"));
            SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Odometer" : (l == 1) ? "Odomètre" : (l == 2) ? "Kilometerzähler" : "Odometru"));
            SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Clock" : (l == 1) ? "Horloge" : (l == 2) ? "Uhr" : "Ceas"));
            SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Total Time (h:mm)" : (l == 1) ? "Durée totale (h:mm)" : (l == 2) ? "Gesamtzeit (h:mm)" : "Timp total (h:mm)"));
            SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Total Time (h)" : (l == 1) ? "Durée totale (h)" : (l == 2) ? "Gesamtzeit (h)" : "Timp Total (h)"));
            SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Date" : (l == 1) ? "Date" : (l == 2) ? "Datum" : "Dată"));
            SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Trip Dist." : (l == 1) ? "Distance du trajet" : (l == 2) ? "Reiseentfernung" : "Distanță plimbare"));
            SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Trip Time (m:ss)" : (l == 1) ? "Temps de trajet (m:ss)" : (l == 2) ? "Reisezeit (m:ss)" : "Timp plimbare (m:ss)"));
            SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Trip Time (h:mm)" : (l == 1) ? "Temps de trajet (h:mm)" : (l == 2) ? "Reisezeit (h:mm)" : "Timp plimbare (h:mm)"));
            SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Trip Time (h)" : (l == 1) ? "Temps de trajet (h)" : (l == 2) ? "Reisezeit (h)" : "Timp plimbare (h)"));
            SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Max Speed" : (l == 1) ? "Vitesse maximale" : (l == 2) ? "Max. Geschwindigkeit" : "Viteza maxima"));
            SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Avg. Speed" : (l == 1) ? "Vitesse moyenne" : (l == 2) ? "Durchschnittsgeschwindigkeit" : "Viteza medie"));
            if ((PlayerPrefs.GetInt("SPEEDSOURCE") == 2 || Mode >= 12)) {
                SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Engine Load" : (l == 1) ? "Charge du moteur" : (l == 2) ? "Motorlast" : "Sarcina motorului"));
                SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Coolant Temp." : (l == 1) ? "Température du moteur" : (l == 2) ? "Motortemperatur" : "Temperatura motorului"));
                SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Throttle" : (l == 1) ? "Étrangler" : (l == 2) ? "Gaspedal" : "Accelerație"));
                SHowOnDis.options.Add(new Dropdown.OptionData((l == 0) ? "Fuel (%)" : (l == 1) ? "Carburant (%)" : (l == 2) ? "Kraftstoff (%)" : "Combustibil (%)"));
            } SHowOnDis.GetComponent<DropdownControl>().scaleCoef = (l == 0) ? 1 : (l == 1) ? 0.9f : (l == 2) ? 0.7f : 0.9f;
            SHowOnDis.GetComponent<DropdownControl>().UpdateScale(); LDStX.value = StretchX; LDStY.value = StretchY; MTP.isOn = MaintainProportion;
            MxDIspl.text = MaxDi + ""; MxDEcim.text = MaxDe + ""; LDBurn.value = LCDBurn; BTRS.text = BurnTime.ToString(CultureInfo.InvariantCulture);
            BGT.isOn = ShowBackGround; BCSelect.color = BackColor; InnerSSlide.value = InnerShadow; RefRS.text = Refrate + ""; soo.text = gameObject.transform.GetSiblingIndex() + "";
            MLD1.isOn = MakeLastDigit1; StyleSelect.value = FontSet / 3; Weight.value = FontSet % 3; SHowOnDis.value = Mode; FStyleSelect.value = FontStSet;
        } StartCoroutine(yesofcourse());
    }

    // Functie executata pentru a astepta un cadru si a actualiza interfata inainte de executarea logicii
    public IEnumerator yesofcourse() {
        yield return null;
        LoadMode = false;
    }

    // Functie executata cand o proprietate este schimbata despre element
    public void OnComponentLCDDModdify() {
        if (!SceneMode && !LoadMode) { StretchX = LDStX.value; StretchY = LDStY.value; MaintainProportion = MTP.isOn;
            MaxDi = int.Parse(MxDIspl.text); MaxDe = Mathf.Min(int.Parse(MxDEcim.text), (MaxDi - 1)); FontStSet = FStyleSelect.value;
            Refrate = int.Parse(RefRS.text); BurnTime = float.Parse(BTRS.text, CultureInfo.InvariantCulture); LCDBurn = LDBurn.value;
            MakeLastDigit1 = MLD1.isOn; ShowBackGround = BGT.isOn; InnerShadow = InnerSSlide.value; Mode = SHowOnDis.value;
        } if (Mode == 3 || Mode == 4 || Mode == 8 || Mode == 9) MaxDe = 0;
        if (Mode == 3) MaxDi = 4; if (Mode == 6) MaxDi = 6;
        if (Mode == 4 || Mode == 8 || Mode == 9) MaxDi = Mathf.Max(MaxDi, 3);
        if (!MakeLastDigit1) {
            if (Mode == 3 || Mode == 4 || Mode == 8 || Mode == 9) Burn.text = new string('8', MaxDi - 2) + ":" + new string('8', 2);
            else if (Mode == 5 || Mode == 10) {
                if (MaxDe > 0) Burn.text = new string('8', MaxDi - MaxDe) + "." + new string('8', MaxDe) + "h";
                else if (Mode == 6) {
                    ActialText.text = DateTime.Now.Day.ToString("D2") + "." + DateTime.Now.Month.ToString("D2") + "." + DateTime.Now.Year.ToString().Substring(2, 2);
                    Burn.text = "88.88.88"; }
                else Burn.text = new string('8', MaxDi) + "h";
            } else if (MaxDe > 0) Burn.text = new string('8', MaxDi - MaxDe) + "." + new string('8', MaxDe);
            else Burn.text = new string('8', MaxDi);
            if (!SceneMode) {
                if (Mode == 3 || Mode == 4 || Mode == 8 || Mode == 9) ActialText.text = "0:" + new string('0', 2);
                else if (Mode == 5 || Mode == 10) {
                    if (MaxDe > 0) ActialText.text = "0." + new string('0', MaxDe) + "h";
                    else ActialText.text = "0h";
                } else if (Mode == 6) {
                    ActialText.text = DateTime.Now.Day.ToString("D2") + "." + DateTime.Now.Month.ToString("D2") + "." + DateTime.Now.Year.ToString().Substring(2, 2);
                    Burn.text = "88.88.88"; }
                else if (MaxDe > 0) ActialText.text = "0." + new string('0', MaxDe);
                else ActialText.text = "0";
            }
        } else {
            if (Mode == 3 || Mode == 4 || Mode == 8 || Mode == 9)
                if (Mode != 3) Burn.text = "1" + new string('8', MaxDi - 3) + ":" + new string('8', 2);
                else Burn.text = new string('8', MaxDi - 2) + ":" + new string('8', 2);
            else if (Mode == 5 || Mode == 10) {
                if (MaxDe > 0) Burn.text = "1" + new string('8', MaxDi - MaxDe - 1) + "." + new string('8', MaxDe) + "h";
                else Burn.text = "1" + new string('8', MaxDi - 1) + "h";
            } else if (Mode == 6) {
                ActialText.text = DateTime.Now.Day.ToString("D2") + "." + DateTime.Now.Month.ToString("D2") + "." + DateTime.Now.Year.ToString().Substring(2, 2);
                Burn.text = "88.88.88"; }
            else if (MaxDe > 0) Burn.text = "1" + new string('8', MaxDi - MaxDe - 1) + "." + new string('8', MaxDe);
            else Burn.text = "1" + new string('8', MaxDi - 1);
            if (!SceneMode) {
                if (Mode == 3 || Mode == 4 || Mode == 8 || Mode == 9)
                    ActialText.text = "1:" + new string('0', 2);
                else if (Mode == 5 || Mode == 10) {
                    if (MaxDe > 0) ActialText.text = "1." + new string('0', MaxDe) + "h";
                    else ActialText.text = "0h";
                } else if (Mode == 6) {
                    ActialText.text = DateTime.Now.Day.ToString("D2") + "." + DateTime.Now.Month.ToString("D2") + "." + DateTime.Now.Year.ToString().Substring(2, 2);
                    Burn.text = "88.88.88"; }
                else if (MaxDe > 0) ActialText.text = "1." + new string('0', MaxDe);
                else ActialText.text = "1";
            }
        } if (ShowBackGround) { gameObject.GetComponent<Image>().color = new Color(BackColor.r, BackColor.g, BackColor.b, 1f);
            gameObject.transform.GetChild(0).GetComponent<RawImage>().color = new Color(0, 0, 0, InnerShadow); }
        else { gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0); gameObject.transform.GetChild(0).GetComponent<RawImage>().color = new Color(0, 0, 0, 0); }
        Burn.color = new Color(ActialText.color.r, ActialText.color.g, ActialText.color.b, LCDBurn);
        if(FontStSet == 1) {
            ActialTextBurn.font = LCDF2; ActialTextBurn.fontSize = 300;
            ActialText.font = LCDF2; ActialText.fontSize = 300;
            Burn.font = LCDF2; Burn.fontSize = 300;
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(30f + (180f * MaxDi), 265f);
            if (Mode == 5 || Mode == 10) gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(180f, 0f);
            if (Mode == 6) Burn.text = "88.88.88"; 
            foreach (char c in Burn.text) if (c == '.' || c == ':') gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(55f, 0f);
            Burn.text = Burn.text.Replace('8', '#'); Burn.text = Burn.text.Replace('h', '#');
            if (!SceneMode) StyleSelect.interactable = false;
        } else { if (ActialText.font = LCDF2) OnChangeOfFont();
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(12.5f + (197.5f * MaxDi), 300f);
            if (MakeLastDigit1 && Mode != 3 && Mode != 6 && FontStSet == 0) gameObject.GetComponent<RectTransform>().sizeDelta -= new Vector2(115f, 0f);
            if (Mode == 3 || Mode == 4 || Mode == 8 || Mode == 9) gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(48f, 0f);
            if (Mode == 5 || Mode == 10) gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(197.5f, 0f);
            if (Mode == 6) gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(96f, 0f);
            if (FontSet >= 12) Burn.text = Burn.text.Replace('8', '~');
            else Burn.text = Burn.text.Replace('~', '8');
            ActialTextBurn.fontSize = 240;
            ActialText.fontSize = 240;
            Burn.fontSize = 240;
            if (!SceneMode) StyleSelect.interactable = true;
        } if(StretchX <= 0.99f || StretchY <= 0.99f) { Burn.transform.localScale = new Vector2(StretchX, StretchY);
                ActialText.transform.localScale = new Vector2(StretchX, StretchY); ActialTextBurn.transform.localScale = new Vector2(StretchX, StretchY); 
                if(MaintainProportion) { if (StretchX <= 0.99f && StretchY <= 0.99f) Burn.color = new Color(Burn.color.r, Burn.color.g, Burn.color.b, Burn.color.a / 6f);
                else Burn.color = new Color(Burn.color.r, Burn.color.g, Burn.color.b, Burn.color.a / 3.5f); Burn.GetComponent<LCDStretch>().effectColor = DigitColor; ActialText.GetComponent<LCDStretch>().effectColor = DigitColor;
                ActialTextBurn.GetComponent<LCDStretch>().effectColor = DigitColor; Burn.GetComponent<LCDStretch>().enabled = true;
                ActialText.GetComponent<LCDStretch>().enabled = true; ActialTextBurn.GetComponent<LCDStretch>().enabled = true;
                } else { Burn.GetComponent<LCDStretch>().enabled = false; ActialText.GetComponent<LCDStretch>().enabled = false;
                ActialTextBurn.GetComponent<LCDStretch>().enabled = false; }
                gameObject.GetComponent<RectTransform>().sizeDelta -= new Vector2(55f * StretchX, 75f * StretchY);
                gameObject.GetComponent<RectTransform>().sizeDelta *= new Vector2(StretchX, StretchY);
                gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(55f * StretchX, 75f * StretchY); } else
            { Burn.GetComponent<LCDStretch>().enabled = false; ActialText.GetComponent<LCDStretch>().enabled = false;
                ActialTextBurn.GetComponent<LCDStretch>().enabled = false; Burn.transform.localScale = new Vector2(1f, 1f);
            ActialText.transform.localScale = new Vector2(1f, 1f); ActialTextBurn.transform.localScale = new Vector2(1f, 1f); }
        if (!SceneMode && !LoadMode) {
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDDNSE", MaxDi);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDDCML", MaxDe);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDREFR", Refrate);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDBRNT", BurnTime);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCBRN", LCDBurn);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDISHD", InnerShadow);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSTRX", StretchX);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSTRY", StretchY);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDMODE", Mode);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSFNT", FontStSet);
            if (ShowBackGround) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDBACK", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDBACK", 0);
            if (MakeLastDigit1) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDMLD1", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDMLD1", 0);
            if (MaintainProportion) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDMTPR", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDMTPR", 0);
        }
    }

    // Functie executata cand font-ul elementului este schimbat
    public void OnChangeOfFont() { if(FontStSet == 0) {
        if (!SceneMode && !LoadMode) FontSet = (StyleSelect.value * 3) + (int)Weight.value;
        Burn.font = LCDS[FontSet]; ActialText.font = LCDS[FontSet]; ActialTextBurn.font = LCDS[FontSet];
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDFONT", FontSet);
            if (FontSet >= 12) Burn.text = Burn.text.Replace('8', '~');
            else Burn.text = Burn.text.Replace('~', '8');
        }
    }

    // Functie executata cand ordinea obiectului este schimbata in scena
    public void ModSOrtO() {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "SO", int.Parse(soo.text));
        gameObject.GetComponent<SortOrderOrg>().Apply();
    }

    // Functie executata cand obiectul este sters
    public void DeleteObj(int wf) {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TYPE", 69696);
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ") + 1);
        Sdrg.OnElemSelect(72769); Destroy(gameObject);
    }

    // Functie chemata in fiecare cadru | Functie folosita pentru a actualiza valoarea display-ului
    void Update() {
        if (SceneMode) {
            RTimel += Time.deltaTime;
            if (RTimel >= (1f / Refrate)) {
                if (Mode != 3 && Mode != 6) if (Mode == 4 || Mode == 8 || Mode == 9) ActialText.text = ActialText.text.Substring(Mathf.Max(0, ActialText.text.Length - (MaxDi + 1)), Mathf.Min(MaxDi + 1, ActialText.text.Length));
                else if (MaxDe != 0) ActialText.text = ActialText.text.Substring(Mathf.Max(0, ActialText.text.Length - (MaxDi + 1)), Mathf.Min(MaxDi + 1, ActialText.text.Length));
                else ActialText.text = ActialText.text.Substring(Mathf.Max(0, ActialText.text.Length - MaxDi), Mathf.Min(MaxDi, ActialText.text.Length));
                if (MakeLastDigit1) if (Mode != 3 && Mode != 6) if (Mode == 4 || Mode == 8 || Mode == 9) { if (ActialText.text.Length >= MaxDi + 1) { ActialText.text = ActialText.text.Substring(1); ActialText.text = "1" + ActialText.text; } }
                        else if (MaxDe != 0) { if (ActialText.text.Length >= MaxDi + 1) { ActialText.text = ActialText.text.Substring(1); ActialText.text = "1" + ActialText.text; } }
                        else { if (ActialText.text.Length >= MaxDi) { ActialText.text = ActialText.text.Substring(1); ActialText.text = "1" + ActialText.text; } }
                LasMessage = ActialText.text;
                if (Mode == 0) { // Speed
                    ActialText.text = SM.InterpSpeed.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 1) { // RPM
                    ActialText.text = SM.RPM.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 2) { // Odometer / Km Counter
                    ActialText.text = SM.DistanceMade.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 3) { // Clock
                    ActialText.text = DateTime.Now.Hour.ToString("D2") + ":" + DateTime.Now.Minute.ToString("D2");
                } else if (Mode == 4) { // Total Time (h:mm)
                    ActialText.text = (int)(SM.TimeMade / 3600f) + ":" + ((int)(SM.TimeMade / 60f) % 60).ToString("D2");
                } else if (Mode == 5) { // Total Time (h)
                    ActialText.text = ((SM.TimeMade / 3600f)).ToString("F" + MaxDe).Replace(",", ".") + "h";
                } else if (Mode == 6) { // Date (dd.mm.yy)
                    ActialText.text = DateTime.Now.Day.ToString("D2") + "." + DateTime.Now.Month.ToString("D2") + "." + DateTime.Now.Year.ToString().Substring(2,2);
                } else if (Mode == 7) { // Trip Distance
                    ActialText.text = SM.TripDistanceMade.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 8) { // Trip Time (m:ss)
                    ActialText.text = (int)(SM.TripTimeMade / 60f) + ":" + ((int)(SM.TripTimeMade) % 60).ToString("D2");
                } else if (Mode == 9) { // Trip Time (h:mm)
                    ActialText.text = (int)(SM.TripTimeMade / 3600f) + ":" + ((int)(SM.TripTimeMade / 60f) % 60).ToString("D2");
                } else if (Mode == 10) { // Trip Time (h)
                    ActialText.text = ((SM.TripTimeMade / 3600f)).ToString("F" + MaxDe).Replace(",", ".") + "h";
                } else if (Mode == 11) { // Max Speed
                    ActialText.text = SM.MaxSpeed.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 12) { // Avg Speed
                    ActialText.text = SM.AvgSpeed.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 13) { // Engine Load
                    ActialText.text = SM.OBDLoad.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 14) { // Coolant Temp.
                    ActialText.text = SM.OBDCoolant.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 15) { // Throttle
                    ActialText.text = SM.OBDThrottle.ToString("F" + MaxDe).Replace(",", ".");
                } else if (Mode == 16) { // Fuel (%)
                    ActialText.text = SM.OBDFuel.ToString("F" + MaxDe).Replace(",", ".");
                } RTimel -= (1f / Refrate); int DeffinedMax;
                if (Mode != 3 && Mode != 6) if (Mode == 4 || Mode == 8 || Mode == 9) { ActialText.text = ActialText.text.Substring(Mathf.Max(0, ActialText.text.Length - (MaxDi + 1)), Mathf.Min(MaxDi + 1, ActialText.text.Length)); }
                    else if (MaxDe != 0) ActialText.text = ActialText.text.Substring(Mathf.Max(0, ActialText.text.Length - (MaxDi + 1)), Mathf.Min(MaxDi + 1, ActialText.text.Length));
                    else ActialText.text = ActialText.text.Substring(Mathf.Max(0, ActialText.text.Length - MaxDi), Mathf.Min(MaxDi, ActialText.text.Length));
                if(MakeLastDigit1) if (Mode != 3 && Mode != 6) if (Mode == 4 || Mode == 8 || Mode == 9) { if (ActialText.text.Length >= MaxDi + 1) { ActialText.text = ActialText.text.Substring(1); ActialText.text = "1" + ActialText.text; } }
                    else if (MaxDe != 0) { if (ActialText.text.Length >= MaxDi + 1) { ActialText.text = ActialText.text.Substring(1); ActialText.text = "1" + ActialText.text; } }
                        else { if (ActialText.text.Length >= MaxDi) { ActialText.text = ActialText.text.Substring(1); ActialText.text = "1" + ActialText.text; } }
            }
        } ActialTextBurn.text = LasMessage;
        if (BurnTime > 0f) ActialTextBurn.color = new Color(ActialText.color.r, ActialText.color.g, ActialText.color.b, (BurnTime - RTimel) * (1f / BurnTime));
    }
}
