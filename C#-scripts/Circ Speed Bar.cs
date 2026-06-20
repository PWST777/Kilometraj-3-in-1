using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircSpeedBar : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Bara Analogica Circulara
    // Folosit pentru elementul Bara Analogica Circulara
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public SelectDrag Sdrg; // Clasa universala pentru aranjarea, crearea si stergerea elementelor
    public ColorPicker CoPi; // Clasa universala pentru selectarea culorii 
    public SPEEDMANAGER SM; // Clasa universala pentru variabile de viteza, turatie, etc.
    public bool SceneMode; // Variabila folosita pentru a determina daca elementul este editad sau utilizat
    public int ElementID; // Vaiabila numerica autonumerotata pentru identificarea elementului
    public bool LoadMode;
    public GameObject EditorModPanel;

    // Lista de elenete folosite pentru a genera elementual vizual
    public Image OutF1;
    public Image InnerF1;
    public Image OutF2;
    public RawImage VoidF;
    public Image VoidF2;
    public GameObject Needll;
    public GameObject MarkText;
    public GameObject MarkNoText;
    public Transform orgfel;
    public Image Fillr;

    // Lista de elenete si proprietati pentru clasa (Cele marcate cu // vor actualiza marcajele)
    public float FillAmount; public Slider Famt;
    public float CirclThicc; public Slider Cith;
    public float BorderW; public Slider BWSlide; //
    public Color BorderC; public Image BoCSel; //
    public Color BackC; public Image BaCSel; //
    public Color NeedC; public Image Ncl;
    public bool FillMode; public Toggle FiMode;
    public bool CCWise; public Toggle CCW;
    public Color FillC; public Image Fcl;
    public float FillOpac; public Slider FOp;
    public float NeedW; public Slider NW;
    public float NeedWTrans; public Slider NWT;
    public Color MarkCl1; public Image Mcl1; //
    public Color MarkCl2; public Image Mcl2; //
    public float Limit; public InputField MaxSpeedd; //
    public float C2Start; public InputField cl2st; //
    public float MarkRate; public InputField markiply; //
    public float InLine; public InputField inl; //
    public float Markwi; public Slider MarkW; //
    public float TextW; public Slider TextWid; //
    public int speedMODE; public Dropdown sMode; //
    public int fonts; public Dropdown Fontttss; //
    public List<Font> actfonts;
    public InputField soo; public Button DelB;

    // Functie executata cand elementul este activat pentru prima data
    void Start() {
        SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        if (LoadMode) OnSelectOfElement();
        OnColorModdify();
        OnComponentAnCiSpBrModdify();
        OnMarksRegenerate();
    }

    // Functie executata cand elementul este creat sau este selectat
    public void OnSelectOfElement() { if (!SceneMode) { LoadMode = true;
        // Aplicarea functiilor pe elementele de interfata (elemente Button, InputField etc.)
        Famt.onValueChanged.RemoveAllListeners(); Famt.onValueChanged.AddListener((_) => OnComponentAnCiSpBrModdify()); Famt.onValueChanged.AddListener((_) => OnMarksRegenerate());
        Cith.onValueChanged.RemoveAllListeners(); Cith.onValueChanged.AddListener((_) => OnComponentAnCiSpBrModdify()); Cith.onValueChanged.AddListener((_) => OnMarksRegenerate());
        BWSlide.onValueChanged.RemoveAllListeners(); BWSlide.onValueChanged.AddListener((_) => OnComponentAnCiSpBrModdify()); BWSlide.onValueChanged.AddListener((_) => OnMarksRegenerate());
        CCW.onValueChanged.RemoveAllListeners(); CCW.onValueChanged.AddListener((_) => OnComponentAnCiSpBrModdify()); CCW.onValueChanged.AddListener((_) => OnMarksRegenerate());
        FiMode.onValueChanged.RemoveAllListeners(); FiMode.onValueChanged.AddListener((_) => OnComponentAnCiSpBrModdify());
        FOp.onValueChanged.RemoveAllListeners(); FOp.onValueChanged.AddListener((_) => OnComponentAnCiSpBrModdify());
        NW.onValueChanged.RemoveAllListeners(); NW.onValueChanged.AddListener((_) => OnComponentAnCiSpBrModdify());
        NWT.onValueChanged.RemoveAllListeners(); NWT.onValueChanged.AddListener((_) => OnComponentAnCiSpBrModdify());
        MaxSpeedd.onSubmit.RemoveAllListeners(); MaxSpeedd.onSubmit.AddListener((_) => OnMarksRegenerate());
        cl2st.onSubmit.RemoveAllListeners(); cl2st.onSubmit.AddListener((_) => OnMarksRegenerate());
        markiply.onSubmit.RemoveAllListeners(); markiply.onSubmit.AddListener((_) => OnMarksRegenerate());
        inl.onSubmit.RemoveAllListeners(); inl.onSubmit.AddListener((_) => OnMarksRegenerate());
        MarkW.onValueChanged.RemoveAllListeners(); MarkW.onValueChanged.AddListener((_) => OnMarksRegenerate());
        TextWid.onValueChanged.RemoveAllListeners(); TextWid.onValueChanged.AddListener((_) => OnMarksRegenerate());
        Fontttss.onValueChanged.RemoveAllListeners(); Fontttss.onValueChanged.AddListener((_) => OnMarksRegenerate());
        sMode.onValueChanged.RemoveAllListeners(); sMode.onValueChanged.AddListener((_) => OnMarksRegenerate());
        DelB.onClick.RemoveAllListeners(); DelB.onClick.AddListener(() => DeleteObj(69));
        soo.onSubmit.RemoveAllListeners(); soo.onSubmit.AddListener((_) => ModSOrtO());
        BoCSel.GetComponent<Button>().onClick.RemoveAllListeners(); BoCSel.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ACSBBORDCL"));
        Fcl.GetComponent<Button>().onClick.RemoveAllListeners(); Fcl.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ACSBFILLCL"));
        Ncl.GetComponent<Button>().onClick.RemoveAllListeners(); Ncl.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ACSBNEEDCL"));
        Mcl1.GetComponent<Button>().onClick.RemoveAllListeners(); Mcl1.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ACSBMKC1CL"));
        Mcl2.GetComponent<Button>().onClick.RemoveAllListeners(); Mcl2.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ACSBMKC2CL"));
        BaCSel.GetComponent<Button>().onClick.RemoveAllListeners(); BaCSel.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ACSBBACKCL")); }
        // Incarcarea variabilelor salvate
        FillAmount = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBVFIL", 0.875f);
        CirclThicc = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBVTIC", 160f);
        BorderW = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBBOWI", 8f);
        FillOpac = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBFILO", 0.5f);
        NeedW = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBNEDW", 100f);
        NeedWTrans = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBNEWT", 0.1f);
        Limit = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBLIMT", 50);
        C2Start = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBC2ST", 32);
        MarkRate = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBMRRT", 10);
        InLine = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBINLN", 1);
        Markwi = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBMARW", 6);
        TextW = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBMTXW", 60);
        fonts = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBFONT", 0);
        speedMODE = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBSPMD", 0);
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBFILL", 1) == 0) FillMode = false; else FillMode = true;
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBCCWI", 1) == 0) CCWise = false; else CCWise = true;
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BorderC = new Color(PlayerPrefs.GetFloat(colorstart + "ACSBBORDCLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "ACSBBORDCLG", 0f), PlayerPrefs.GetFloat(colorstart + "ACSBBORDCLB", 0f));
        BackC = new Color(PlayerPrefs.GetFloat(colorstart + "ACSBBACKCLR", 0.92f), PlayerPrefs.GetFloat(colorstart + "ACSBBACKCLG", 0.88f), PlayerPrefs.GetFloat(colorstart + "ACSBBACKCLB", 0.8f));
        NeedC = new Color(PlayerPrefs.GetFloat(colorstart + "ACSBNEEDCLR", 0f), PlayerPrefs.GetFloat(colorstart + "ACSBNEEDCLG", 0f), PlayerPrefs.GetFloat(colorstart + "ACSBNEEDCLB", 0f));
        FillC = new Color(PlayerPrefs.GetFloat(colorstart + "ACSBFILLCLR", 0f), PlayerPrefs.GetFloat(colorstart + "ACSBFILLCLG", 0.85f), PlayerPrefs.GetFloat(colorstart + "ACSBFILLCLB", 1f));
        MarkCl1 = new Color(PlayerPrefs.GetFloat(colorstart + "ACSBMKC1CLR", 0.5f), PlayerPrefs.GetFloat(colorstart + "ACSBMKC1CLG", 0.5f), PlayerPrefs.GetFloat(colorstart + "ACSBMKC1CLB", 0.5f));
        MarkCl2 = new Color(PlayerPrefs.GetFloat(colorstart + "ACSBMKC2CLR", 0.9f), PlayerPrefs.GetFloat(colorstart + "ACSBMKC2CLG", 0f), PlayerPrefs.GetFloat(colorstart + "ACSBMKC2CLB", 0f));
        int l = PlayerPrefs.GetInt("MAINLANG"); if (!SceneMode) { sMode.options.Clear(); 
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Speed" : (l == 1) ? "Vitesse" : (l == 2) ? "Geschwindigkeit" : "Viteză")); 
            sMode.options.Add(new Dropdown.OptionData("RPM")); sMode.options.Add(new Dropdown.OptionData("RPM x10")); 
            sMode.options.Add(new Dropdown.OptionData("RPM x100")); 
            if ((PlayerPrefs.GetInt("SPEEDSOURCE") == 2 || speedMODE >= 4)) { sMode.options.Add(new Dropdown.OptionData("RPM x1000"));
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Fuel %" : (l == 1) ? "Carburant %" : (l == 2) ? "Kraftstoffstand" : "Combustibil %")); 
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Coolant Temp." : (l == 1) ? "Température" : (l == 2) ? "Temperatur" : "Temperatură"));
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Engine Load" : (l == 1) ? "Charge du moteur" : (l == 2) ? "Motorlast" : "Sarcina motorului"));
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Throttle" : (l == 1) ? "Étrangler" : (l == 2) ? "Gaspedal" : "Accelerație")); }
            CCW.isOn = CCWise; sMode.value = speedMODE; Famt.value = FillAmount; BWSlide.value = BorderW; BoCSel.color = BorderC; BaCSel.color = BackC; 
            FiMode.isOn = FillMode; Fcl.color = FillC; FOp.value = FillOpac; NW.value = NeedW; NWT.value = NeedWTrans; Mcl1.color = MarkCl1; Mcl2.color = MarkCl2;
            MaxSpeedd.text = Limit + ""; cl2st.text = C2Start + ""; markiply.text = MarkRate + ""; inl.text = InLine + ""; Ncl.color = NeedC;
            MarkW.value = Markwi; TextWid.value = TextW; Fontttss.value = fonts; Cith.value = CirclThicc; soo.text = gameObject.transform.GetSiblingIndex() + ""; }
        StartCoroutine(yesofcourse());
    }

    // Functie executata pentru a astepta un cadru si a actualiza interfata inainte de executarea logicii
    public IEnumerator yesofcourse() {
        yield return null;
        LoadMode = false;
    }

    // Functie executata cand o culoare trebuie schimbata
    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject);
    }

    // Functie executata cand o proprietate din afara categoriei de marcaje este schimbata despre element
    public void OnComponentAnCiSpBrModdify() {
        if (!SceneMode && !LoadMode) { FillAmount = Famt.value; CirclThicc = Cith.value; NeedW = NW.value; NeedWTrans = NWT.value; FillMode = FiMode.isOn; FillOpac = FOp.value;
            BorderW = BWSlide.value;  MarkCl1 = Mcl1.color; MarkCl2 = Mcl2.color; Limit = float.Parse(MaxSpeedd.text); C2Start = float.Parse(cl2st.text);
            MarkRate = float.Parse(markiply.text); InLine = float.Parse(inl.text); Markwi = MarkW.value; TextW = TextWid.value; CCWise = CCW.isOn;
            if (speedMODE == 5) { Limit = 100f; MaxSpeedd.text = "100"; MaxSpeedd.enabled = false; } else { MaxSpeedd.enabled = true; } }
        OutF1.fillAmount = Mathf.Min(FillAmount + (BorderW / 1000f),1f);
        OutF2.fillAmount = Mathf.Min(FillAmount + 0.002f,1f);
        OutF1.fillClockwise = CCWise; OutF2.fillClockwise = CCWise; InnerF1.fillClockwise = CCWise; Fillr.fillClockwise = CCWise;
        InnerF1.fillAmount = FillAmount; if(CCWise) InnerF1.transform.localRotation = Quaternion.Euler(0f, 0f, -(BorderW / 5.555f));
        else InnerF1.transform.localRotation = Quaternion.Euler(0f, 0f, BorderW / 5.555f);
        InnerF1.GetComponent<RectTransform>().sizeDelta = new Vector2(800f - (BorderW * 2), 800f - (BorderW * 2));
        OutF2.GetComponent<RectTransform>().sizeDelta = new Vector2(800f - (CirclThicc * 2), 800f - (CirclThicc * 2));
        VoidF2.GetComponent<RectTransform>().sizeDelta = new Vector2(800f - ((BorderW + CirclThicc) * 2), 800f - ((BorderW + CirclThicc) * 2));
        float scale = 800f / (Mathf.Max(800f - ((BorderW + CirclThicc) * 2), 1f) + 3f);
        VoidF.uvRect = new Rect((-0.5f * (scale-1f)), (-0.5f * (scale - 1f)), scale, scale);
        Needll.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -399f + BorderW);
        Needll.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(NeedW, (CirclThicc - BorderW) + 2f);
        if(CCWise) Needll.transform.localRotation = Quaternion.Euler(0f, 0f, -(NeedWTrans * (NeedW / 4f)));
        else Needll.transform.localRotation = Quaternion.Euler(0f, 0f, -(-(NeedWTrans * (NeedW / 4f))));
        Needll.transform.GetChild(0).GetComponent<RawImage>().uvRect = new Rect(0f, 0.04f, 1f, NeedWTrans);
        if (FillMode) Fillr.gameObject.SetActive(true); else Fillr.gameObject.SetActive(false); Fillr.fillAmount = (NeedWTrans * (NeedW / 4f)) / 360f;
        Fillr.color = new Color(FillC.r, FillC.g, FillC.b, FillOpac);
        if (!SceneMode && !LoadMode) {
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBVFIL", FillAmount);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBVTIC", CirclThicc);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBBOWI", BorderW);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBFILO", FillOpac);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBNEDW", NeedW);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBNEWT", NeedWTrans);
            if (FillMode) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBFILL", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBFILL", 0);
        }
    }

    // Functie executata cand o culoare este selectata si schimbata in meniul de culoare
    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BorderC = new Color(PlayerPrefs.GetFloat(colorstart + "ACSBBORDCLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "ACSBBORDCLG", 0f), PlayerPrefs.GetFloat(colorstart + "ACSBBORDCLB", 0f));
        BackC = new Color(PlayerPrefs.GetFloat(colorstart + "ACSBBACKCLR", 0.92f), PlayerPrefs.GetFloat(colorstart + "ACSBBACKCLG", 0.88f), PlayerPrefs.GetFloat(colorstart + "ACSBBACKCLB", 0.8f));
        NeedC = new Color(PlayerPrefs.GetFloat(colorstart + "ACSBNEEDCLR", 0f), PlayerPrefs.GetFloat(colorstart + "ACSBNEEDCLG", 0f), PlayerPrefs.GetFloat(colorstart + "ACSBNEEDCLB", 0f));
        FillC = new Color(PlayerPrefs.GetFloat(colorstart + "ACSBFILLCLR", 0f), PlayerPrefs.GetFloat(colorstart + "ACSBFILLCLG", 0.85f), PlayerPrefs.GetFloat(colorstart + "ACSBFILLCLB", 1f));
        MarkCl1 = new Color(PlayerPrefs.GetFloat(colorstart + "ACSBMKC1CLR", 0.5f), PlayerPrefs.GetFloat(colorstart + "ACSBMKC1CLG", 0.5f), PlayerPrefs.GetFloat(colorstart + "ACSBMKC1CLB", 0.5f));
        MarkCl2 = new Color(PlayerPrefs.GetFloat(colorstart + "ACSBMKC2CLR", 0.9f), PlayerPrefs.GetFloat(colorstart + "ACSBMKC2CLG", 0f), PlayerPrefs.GetFloat(colorstart + "ACSBMKC2CLB", 0f));
        OutF1.color = BorderC; OutF2.color = BorderC; InnerF1.color = BackC; Needll.transform.GetChild(0).GetComponent<RawImage>().color = NeedC; Fillr.color = FillC;
        MarkText.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = BackC;
        if (!SceneMode) { BoCSel.color = BorderC; BaCSel.color = BackC; Ncl.color = NeedC; Fcl.color = FillC; Mcl1.color = MarkCl1; Mcl2.color = MarkCl2; }
        OnComponentAnCiSpBrModdify(); OnMarksRegenerate();
    }

    // Functie executata can o proprietate din categoria de marcaje este schimbata despre element | Aici marcajele sunt regenerate
    public void OnMarksRegenerate() {
        if (!SceneMode && !LoadMode) { FillAmount = Famt.value; CirclThicc = Cith.value; FillOpac = FOp.value; CCWise = CCW.isOn; speedMODE = sMode.value;
            BorderW = BWSlide.value; MarkCl1 = Mcl1.color; MarkCl2 = Mcl2.color; Limit = float.Parse(MaxSpeedd.text); C2Start = float.Parse(cl2st.text);
            MarkRate = float.Parse(markiply.text); InLine = float.Parse(inl.text); Markwi = MarkW.value; TextW = TextWid.value; fonts = Fontttss.value;
            if (speedMODE == 5 || speedMODE == 7 || speedMODE == 8) { Limit = 100f; MaxSpeedd.text = "100"; MaxSpeedd.enabled = false; } else { MaxSpeedd.enabled = true; } }
            foreach (Transform childsss in orgfel) Destroy(childsss.gameObject);
            MarkText.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -399f + BorderW);
            MarkNoText.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -399f + BorderW);
            MarkText.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(Markwi, CirclThicc - BorderW);
            MarkNoText.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2((Markwi / 1.6f), (CirclThicc - 10f) - BorderW);
            MarkText.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(Markwi + 2f, TextW + 10f);
            MarkText.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().fontSize = (int)TextW + 10;
            MarkText.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().font = actfonts[fonts];
            if (fonts == 0) MarkText.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -3f);
            else if (fonts == 2) MarkText.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 6f);
            else MarkText.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            float CurrntSpeed = 0f; int CurrentL = 0; while (CurrntSpeed < (Limit - 0.1f)) {
            if(CurrntSpeed == 0f || CurrntSpeed == Limit) { CurrntSpeed += (MarkRate / (InLine + 1)); CurrentL++;}
            else { float Pos = -((360f * FillAmount) * (CurrntSpeed / Limit)); if (!CCWise) Pos *= -1;
            if (CurrntSpeed < C2Start) { MarkText.transform.GetChild(0).GetComponent<RawImage>().color = MarkCl1; MarkNoText.transform.GetChild(0).GetComponent<RawImage>().color = MarkCl1;
            MarkText.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().color = MarkCl1; }
            else { MarkText.transform.GetChild(0).GetComponent<RawImage>().color = MarkCl2; MarkNoText.transform.GetChild(0).GetComponent<RawImage>().color = MarkCl2;
            MarkText.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().color = MarkCl2; }
            if (CurrentL % (InLine + 1) == 0) { GameObject mynewTxt = Instantiate(MarkText, orgfel); mynewTxt.SetActive(true);
                mynewTxt.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, Pos);
                mynewTxt.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = Mathf.RoundToInt(CurrntSpeed) + "";
            } else { GameObject mynewTxt = Instantiate(MarkNoText, orgfel); mynewTxt.SetActive(true);
                mynewTxt.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, Pos); }
            CurrentL++; CurrntSpeed += (MarkRate / (InLine + 1));
            }
        } if (!SceneMode && !LoadMode) {
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBLIMT", Limit);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBC2ST", C2Start);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBMRRT", MarkRate);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBINLN", InLine);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBMARW", Markwi);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBMTXW", TextW);
            if(CCWise) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBCCWI", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBCCWI", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBFONT", fonts);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ACSBSPMD", speedMODE);
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

    // Functie chemata de 60 de ori / secunda | Functie folosita pentru a actualiza pozitia acului pe kilometraj
    void FixedUpdate() {
        if (SceneMode) { float Vatr = 0f; if (speedMODE == 0) Vatr = Mathf.Min(SM.InterpSpeed, Limit); if (speedMODE == 1) Vatr = Mathf.Min(SM.RPM, Limit);
            if (speedMODE == 2) Vatr = Mathf.Min(SM.RPM / 10f, Limit); if (speedMODE == 3) Vatr = Mathf.Min(SM.RPM / 100f, Limit);
            if (speedMODE == 4) Vatr = Mathf.Min(SM.RPM / 1000f, Limit); if (speedMODE == 5) Vatr = Mathf.Min(SM.OBDFuel, 100f);
            if (speedMODE == 6) Vatr = Mathf.Min(SM.OBDCoolant, Limit); if (speedMODE == 7) Vatr = Mathf.Min(SM.OBDLoad, Limit);
            if (speedMODE == 8) Vatr = Mathf.Min(SM.OBDThrottle, Limit);
            if (CCWise) Needll.transform.localRotation = Quaternion.Euler(0f, 0f, -(NeedWTrans * (NeedW / 4f)) - (((360f - (NeedWTrans * (NeedW / 2f))) * FillAmount) * (Vatr / Limit)));
        else Needll.transform.localRotation = Quaternion.Euler(0f, 0f, -(-(NeedWTrans * (NeedW / 4f)) - (((360f - (NeedWTrans * (NeedW / 2f))) * FillAmount) * (Vatr / Limit))));
            Fillr.fillAmount = ((NeedWTrans * (NeedW /4f)) / 360f) + ((FillAmount - ((NeedWTrans * (NeedW /4f)) / 180f)) * (Vatr / Limit)); } string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET");
    }
}
