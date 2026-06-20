using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalogSpeedbar : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Bara de viteza Analogica
    // Folosita pentru elementul Bara de viteza Analogica
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public SelectDrag Sdrg; // Clasa universala pentru aranjarea, crearea si stergerea elementelor
    public ColorPicker CoPi; // Clasa universala pentru selectarea culorii
    public SPEEDMANAGER SM; // Clasa universala pentru variabile de viteza, turatie, etc.
    public bool SceneMode; // Variabila folosita pentru a determina daca elementul este editad sau utilizat
    public int ElementID; // Vaiabila numerica autonumerotata pentru identificarea elementului
    public bool LoadMode;
    public GameObject EditorModPanel;

    public Image Border;
    public Image InBorder;
    public Image Fill;
    public RawImage ImgNeedl;
    public GameObject MarkText;
    public GameObject MarkNoText;
    public Transform orgfel;

    // Lista de elenete si proprietati pentru clasa (Cele marcate cu // vor actualiza marcajele)
    public float Width; public Slider WSlide;
    public float Height; public Slider HSlide;
    public float BorderW; public Slider BWSlide; //
    public Color BorderC; public Image BoCSel; //
    public Color BackC; public Image BaCSel; //
    public Color NeedC; public Image Ncl;
    public bool FillMode; public Toggle FiMode;
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
    public int fonts; public Dropdown Fontttss; //
    public int speedMODE; public Dropdown sMode; //
    public List<Font> actfonts;
    public InputField soo; public Button DelB;

    // Functie executata cand elementul este activat pentru prima data
    void Start() {
        SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        if (LoadMode) OnSelectOfElement();
        OnColorModdify();
        OnComponentAnSpBrModdify();
        OnMarksRegenerate();
    }

    // Functie executata cand elementul este creat sau este selectat
    public void OnSelectOfElement() { if (!SceneMode) { LoadMode = true;
        // Aplicarea functiilor pe elementele de interfata (elemente Button, InputField etc.)
        WSlide.onValueChanged.RemoveAllListeners(); WSlide.onValueChanged.AddListener((_) => OnComponentAnSpBrModdify()); WSlide.onValueChanged.AddListener((_) => OnMarksRegenerate());
        HSlide.onValueChanged.RemoveAllListeners(); HSlide.onValueChanged.AddListener((_) => OnComponentAnSpBrModdify()); HSlide.onValueChanged.AddListener((_) => OnMarksRegenerate());
        BWSlide.onValueChanged.RemoveAllListeners(); BWSlide.onValueChanged.AddListener((_) => OnComponentAnSpBrModdify()); BWSlide.onValueChanged.AddListener((_) => OnMarksRegenerate());
        FiMode.onValueChanged.RemoveAllListeners(); FiMode.onValueChanged.AddListener((_) => OnComponentAnSpBrModdify());
        FOp.onValueChanged.RemoveAllListeners(); FOp.onValueChanged.AddListener((_) => OnComponentAnSpBrModdify());
        NW.onValueChanged.RemoveAllListeners(); NW.onValueChanged.AddListener((_) => OnComponentAnSpBrModdify());
        NWT.onValueChanged.RemoveAllListeners(); NWT.onValueChanged.AddListener((_) => OnComponentAnSpBrModdify());
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
        BoCSel.GetComponent<Button>().onClick.RemoveAllListeners(); BoCSel.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASBBORDCL"));
        Fcl.GetComponent<Button>().onClick.RemoveAllListeners(); Fcl.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASBFILLCL"));
        Ncl.GetComponent<Button>().onClick.RemoveAllListeners(); Ncl.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASBNEEDCL"));
        Mcl1.GetComponent<Button>().onClick.RemoveAllListeners(); Mcl1.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASBMKC1CL"));
        Mcl2.GetComponent<Button>().onClick.RemoveAllListeners(); Mcl2.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASBMKC2CL"));
        BaCSel.GetComponent<Button>().onClick.RemoveAllListeners(); BaCSel.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASBBACKCL")); }
        // Incarcarea variabilelor salvate
        Height = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBHEIG", 200f);
        Width = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBWIDT", 700f);
        BorderW = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBBOWI", 8f);
        FillOpac = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBFILO", 0.5f);
        NeedW = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBNEDW", 100f);
        NeedWTrans = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBNEWT", 0.1f);
        Limit = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBLIMT", 50);
        C2Start = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBC2ST", 32);
        MarkRate = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBMRRT", 10);
        InLine = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBINLN", 1);
        Markwi = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBMARW", 6);
        TextW = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBMTXW", 60);
        fonts = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBFONT", 0);
        speedMODE = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBSPMD", 0);
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBFILL", 1) == 0) FillMode = false; else FillMode = true;
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BorderC = new Color(PlayerPrefs.GetFloat(colorstart + "ASBBORDCLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "ASBBORDCLG", 0f), PlayerPrefs.GetFloat(colorstart + "ASBBORDCLB", 0f));
        BackC = new Color(PlayerPrefs.GetFloat(colorstart + "ASBBACKCLR", 0.92f), PlayerPrefs.GetFloat(colorstart + "ASBBACKCLG", 0.88f), PlayerPrefs.GetFloat(colorstart + "ASBBACKCLB", 0.8f));
        NeedC = new Color(PlayerPrefs.GetFloat(colorstart + "ASBNEEDCLR", 0f), PlayerPrefs.GetFloat(colorstart + "ASBNEEDCLG", 0f), PlayerPrefs.GetFloat(colorstart + "ASBNEEDCLB", 0f));
        FillC = new Color(PlayerPrefs.GetFloat(colorstart + "ASBFILLCLR", 0f), PlayerPrefs.GetFloat(colorstart + "ASBFILLCLG", 0.85f), PlayerPrefs.GetFloat(colorstart + "ASBFILLCLB", 1f));
        MarkCl1 = new Color(PlayerPrefs.GetFloat(colorstart + "ASBMKC1CLR", 0.5f), PlayerPrefs.GetFloat(colorstart + "ASBMKC1CLG", 0.5f), PlayerPrefs.GetFloat(colorstart + "ASBMKC1CLB", 0.5f));
        MarkCl2 = new Color(PlayerPrefs.GetFloat(colorstart + "ASBMKC2CLR", 0.9f), PlayerPrefs.GetFloat(colorstart + "ASBMKC2CLG", 0f), PlayerPrefs.GetFloat(colorstart + "ASBMKC2CLB", 0f));
        int l = PlayerPrefs.GetInt("MAINLANG"); if (!SceneMode) { sMode.options.Clear(); 
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Speed" : (l == 1) ? "Vitesse" : (l == 2) ? "Geschwindigkeit" : "Viteză"));
            sMode.options.Add(new Dropdown.OptionData("RPM")); sMode.options.Add(new Dropdown.OptionData("RPM x10")); 
            sMode.options.Add(new Dropdown.OptionData("RPM x100")); 
            if ((PlayerPrefs.GetInt("SPEEDSOURCE") == 2 || speedMODE >= 4)) { sMode.options.Add(new Dropdown.OptionData("RPM x1000"));
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Fuel %" : (l == 1) ? "Carburant %" : (l == 2) ? "Kraftstoffstand" : "Combustibil %")); 
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Coolant Temp." : (l == 1) ? "Température" : (l == 2) ? "Temperatur" : "Temperatură"));
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Engine Load" : (l == 1) ? "Charge du moteur" : (l == 2) ? "Motorlast" : "Sarcina motorului"));
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Throttle" : (l == 1) ? "Étrangler" : (l == 2) ? "Gaspedal" : "Accelerație")); }
            sMode.value = speedMODE; HSlide.value = Height; BWSlide.value = BorderW; BoCSel.color = BorderC; BaCSel.color = BackC; FiMode.isOn = FillMode;
            Fcl.color = FillC; FOp.value = FillOpac; NW.value = NeedW; NWT.value = NeedWTrans; Mcl1.color = MarkCl1; Mcl2.color = MarkCl2;
            MaxSpeedd.text = Limit + ""; cl2st.text = C2Start + ""; markiply.text = MarkRate + ""; inl.text = InLine + ""; Ncl.color = NeedC;
            MarkW.value = Markwi; TextWid.value = TextW; Fontttss.value = fonts; WSlide.value = Width; soo.text = gameObject.transform.GetSiblingIndex() + ""; }
        StartCoroutine(yesofcourse());
    }

    // Functie executata pentru a astepta un cadru si a actualiza interfata inainte de executarea logicii
    public IEnumerator yesofcourse() {
        yield return null;
        LoadMode = false;
    }

    // Functie executata cand o culoare este selectata si schimbata in meniul de culoare
    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BorderC = new Color(PlayerPrefs.GetFloat(colorstart + "ASBBORDCLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "ASBBORDCLG", 0f), PlayerPrefs.GetFloat(colorstart + "ASBBORDCLB", 0f));
        BackC = new Color(PlayerPrefs.GetFloat(colorstart + "ASBBACKCLR", 0.92f), PlayerPrefs.GetFloat(colorstart + "ASBBACKCLG", 0.88f), PlayerPrefs.GetFloat(colorstart + "ASBBACKCLB", 0.8f));
        NeedC = new Color(PlayerPrefs.GetFloat(colorstart + "ASBNEEDCLR", 0f), PlayerPrefs.GetFloat(colorstart + "ASBNEEDCLG", 0f), PlayerPrefs.GetFloat(colorstart + "ASBNEEDCLB", 0f));
        FillC = new Color(PlayerPrefs.GetFloat(colorstart + "ASBFILLCLR", 0f), PlayerPrefs.GetFloat(colorstart + "ASBFILLCLG", 0.85f), PlayerPrefs.GetFloat(colorstart + "ASBFILLCLB", 1f));
        MarkCl1 = new Color(PlayerPrefs.GetFloat(colorstart + "ASBMKC1CLR", 0.5f), PlayerPrefs.GetFloat(colorstart + "ASBMKC1CLG", 0.5f), PlayerPrefs.GetFloat(colorstart + "ASBMKC1CLB", 0.5f));
        MarkCl2 = new Color(PlayerPrefs.GetFloat(colorstart + "ASBMKC2CLR", 0.9f), PlayerPrefs.GetFloat(colorstart + "ASBMKC2CLG", 0f), PlayerPrefs.GetFloat(colorstart + "ASBMKC2CLB", 0f));
        Border.color = BorderC; InBorder.color = BackC; ImgNeedl.color = NeedC; Fill.color = FillC;
        MarkText.transform.GetChild(0).GetComponent<RawImage>().color = BackC;
        if (!SceneMode) { BoCSel.color = BorderC; BaCSel.color = BackC; Ncl.color = NeedC; Fcl.color = FillC; Mcl1.color = MarkCl1; Mcl2.color = MarkCl2; }
        OnComponentAnSpBrModdify(); OnMarksRegenerate();
    }

    // Functie executata cand o culoare trebuie schimbata
    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject);
    }

    // Functie executata cand o proprietate din afara categoriei de marcaje este schimbata despre element
    public void OnComponentAnSpBrModdify() {
        if (!SceneMode && !LoadMode) {
            Height = HSlide.value; BorderW = BWSlide.value; BorderC = BoCSel.color; BackC = BaCSel.color; FillMode = FiMode.isOn;
            FillC = Fcl.color; FillOpac = FOp.value; NeedW = NW.value; NeedWTrans = NWT.value; Width = WSlide.value; }
        Border.GetComponent<RectTransform>().sizeDelta = new Vector2(Width, Height);
        InBorder.GetComponent<RectTransform>().sizeDelta = new Vector2(Width - (BorderW * 2), Height - (BorderW * 2));
        ImgNeedl.GetComponent<RectTransform>().sizeDelta = new Vector2(NeedW, Height - (BorderW * 2));
        if(!SceneMode) ImgNeedl.GetComponent<RectTransform>().anchoredPosition = new Vector2((NeedW / 10f), 0f);
        if(!SceneMode) Fill.GetComponent<RectTransform>().sizeDelta = new Vector2((NeedW / 10f), Height - (BorderW * 2));
        ImgNeedl.uvRect = new Rect(0f, 0.04f, 1f, NeedWTrans);
        if (FillMode) Fill.gameObject.SetActive(true); else Fill.gameObject.SetActive(false);
        Fill.color = new Color(FillC.r, FillC.g, FillC.b, FillOpac);
        if (!SceneMode && !LoadMode) { // Salvarea variabilelor
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBWIDT", Width);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBHEIG", Height);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBBOWI", BorderW);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBFILO", FillOpac);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBNEDW", NeedW);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBNEWT", NeedWTrans);
            if(FillMode) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBFILL", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBFILL", 0);
        }
    }

    // Functie executata can o proprietate din categoria de marcaje este schimbata despre element | Aici marcajele sunt regenerate
    public void OnMarksRegenerate() {
        if (!SceneMode && !LoadMode) { Width = WSlide.value; Height = HSlide.value; speedMODE = sMode.value;
            BorderW = BWSlide.value;  MarkCl1 = Mcl1.color; MarkCl2 = Mcl2.color; Limit = float.Parse(MaxSpeedd.text); C2Start = float.Parse(cl2st.text);
            MarkRate = float.Parse(markiply.text); InLine = float.Parse(inl.text); Markwi = MarkW.value; TextW = TextWid.value; fonts = Fontttss.value;
            if (speedMODE == 5 || speedMODE == 7 || speedMODE == 8) { Limit = 100f; MaxSpeedd.text = "100"; MaxSpeedd.enabled = false; } else { MaxSpeedd.enabled = true; } }
            foreach (Transform childsss in orgfel) Destroy(childsss.gameObject);
            MarkText.GetComponent<RectTransform>().sizeDelta = new Vector2(Markwi, Height - (BorderW * 2));
            MarkNoText.GetComponent<RectTransform>().sizeDelta = new Vector2((Markwi / 1.6f), (Height - 10f) - (BorderW * 2));
            MarkText.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(Markwi + 2f, TextW + 10f);
            MarkText.transform.GetChild(0).GetChild(0).GetComponent<Text>().fontSize = (int)TextW + 10;
            MarkText.transform.GetChild(0).GetChild(0).GetComponent<Text>().font = actfonts[fonts];
            if (fonts == 0) MarkText.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 3f);
            else if (fonts == 2) MarkText.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -6f);
            else MarkText.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            float CurrntSpeed = 0f; int CurrentL = 0; while (CurrntSpeed < (Limit - 0.1f)) {
            if(CurrntSpeed == 0f || CurrntSpeed == Limit) { CurrntSpeed += (MarkRate / (InLine + 1)); CurrentL++;}
            else { float Pos = ((Width - (BorderW * 2)) * (CurrntSpeed / Limit));
            if (CurrntSpeed < C2Start) { MarkText.GetComponent<RawImage>().color = MarkCl1; MarkNoText.GetComponent<RawImage>().color = MarkCl1;
            MarkText.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = MarkCl1; }
            else { MarkText.GetComponent<RawImage>().color = MarkCl2; MarkNoText.GetComponent<RawImage>().color = MarkCl2;
            MarkText.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = MarkCl2; }
            if (CurrentL % (InLine + 1) == 0) { GameObject mynewTxt = Instantiate(MarkText, orgfel); mynewTxt.SetActive(true);
                mynewTxt.GetComponent<RectTransform>().anchoredPosition = new Vector2(Pos, 0f);
                mynewTxt.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = Mathf.RoundToInt(CurrntSpeed) + "";
            } else { GameObject mynewTxt = Instantiate(MarkNoText, orgfel); mynewTxt.SetActive(true);
                mynewTxt.GetComponent<RectTransform>().anchoredPosition = new Vector2(Pos, 0f); }
            CurrentL++; CurrntSpeed += (MarkRate / (InLine + 1));
            }
        } if (!SceneMode && !LoadMode) { // Salvarea variabilelor
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBLIMT", Limit);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBC2ST", C2Start);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBMRRT", MarkRate);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBINLN", InLine);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBMARW", Markwi);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBMTXW", TextW);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBFONT", fonts);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBSPMD", speedMODE);
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
            ImgNeedl.GetComponent<RectTransform>().anchoredPosition = new Vector2((NeedW / 10f) + (((Width - (BorderW * 2)) - (NeedW / 10f)) * (Vatr / Limit)), 0f);
        Fill.GetComponent<RectTransform>().sizeDelta = new Vector2((NeedW / 10f) + (((Width - (BorderW * 2)) - (NeedW / 10f)) * (Vatr / Limit)), Height - (BorderW * 2)); }
    }
}
