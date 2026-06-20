using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class LCDSpeedbar : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Bara de viteza LCD
    // Folosita pentru elementul Bara de viteza LCD
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public SelectDrag Sdrg; // Clasa universala pentru aranjarea, crearea si stergerea elementelor
    public ColorPicker CoPi; // Clasa universala pentru selectarea culorii
    public SPEEDMANAGER SM; // Clasa universala pentru variabile de viteza, turatie, etc.
    public bool SceneMode; // Variabila folosita pentru a determina daca elementul este editad sau utilizat
    public int ElementID; // Vaiabila numerica autonumerotata pentru identificarea elementului
    public bool LoadMode;

    // Lista de elenete si proprietati pentru clasa
    public GameObject EditorModPanel;
    public GameObject ClonerObj;
    public Transform orgfel;
    public float Width; public Slider WSlide;
    public float Height; public Slider HSlide;
    public int Refrate = 4; public InputField RefRS;
    public float BurnTime = 0.1f; public InputField BTRS;
    public float LCDBurn = 0.13f; public Slider LDBurn;
    public int Segments = 20; public InputField Segs;
    public float SegGap = 3f; public Slider SegGp;
    public bool ShowBackGround; public Toggle BGT;
    public Color BackColor; public Image BCSelect;
    public Color DigitColor; public Image DGCSelect;
    public int TopSpeed = 50; public InputField STopSpeed;
    public float InnerShadow = 0.5f; public Slider InnerSSlide;
    public int SpeedUnit; public Dropdown yesofcourse;
    public Button DelObj; public InputField soo;
    public float LastSpeed; public float CurSpeed; float RTimel = 0f;

    // Functie executata cand elementul este activat pentru prima data 
    void Start() {
        SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        if (LoadMode) OnSelectOfElement();
        OnComponentLCDSpBrModdify();
        OnColorModdify();
    }

    // Functie executata cand elementul este creat sau este selectat
    public void OnSelectOfElement() { if (!SceneMode) { LoadMode = true;
        // Aplicarea functiilor pe elementele de interfata (elemente Button, InputField etc.)
        WSlide.onValueChanged.RemoveAllListeners(); WSlide.onValueChanged.AddListener((_) => OnComponentLCDSpBrModdify()); 
        HSlide.onValueChanged.RemoveAllListeners(); HSlide.onValueChanged.AddListener((_) => OnComponentLCDSpBrModdify());
        LDBurn.onValueChanged.RemoveAllListeners(); LDBurn.onValueChanged.AddListener((_) => OnComponentLCDSpBrModdify());
        yesofcourse.onValueChanged.RemoveAllListeners(); yesofcourse.onValueChanged.AddListener((_) => OnComponentLCDSpBrModdify());
        BGT.onValueChanged.RemoveAllListeners(); BGT.onValueChanged.AddListener((_) => OnComponentLCDSpBrModdify());
        InnerSSlide.onValueChanged.RemoveAllListeners(); InnerSSlide.onValueChanged.AddListener((_) => OnComponentLCDSpBrModdify());
        RefRS.onSubmit.RemoveAllListeners(); RefRS.onSubmit.AddListener((_) => OnComponentLCDSpBrModdify());
        Segs.onSubmit.RemoveAllListeners(); Segs.onSubmit.AddListener((_) => OnComponentLCDSpBrModdify());
        STopSpeed.onSubmit.RemoveAllListeners(); STopSpeed.onSubmit.AddListener((_) => OnComponentLCDSpBrModdify());
        SegGp.onValueChanged.RemoveAllListeners(); SegGp.onValueChanged.AddListener((_) => OnComponentLCDSpBrModdify());
        BCSelect.GetComponent<Button>().onClick.RemoveAllListeners(); BCSelect.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("LDSBBACKCL"));
        DGCSelect.GetComponent<Button>().onClick.RemoveAllListeners(); DGCSelect.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("LDSBDGITCL"));
        DelObj.onClick.RemoveAllListeners(); DelObj.onClick.AddListener(() => DeleteObj(69));
        soo.onSubmit.RemoveAllListeners(); soo.onSubmit.AddListener((_) => ModSOrtO());
        BTRS.onSubmit.RemoveAllListeners(); BTRS.onSubmit.AddListener((_) => OnComponentLCDSpBrModdify()); }
        // Incarcarea variabilelor salvate
        Height = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBHEIG", 200f);
        Width = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBWIDT", 700f);
        Refrate = (int)PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBREFR", 5);
        BurnTime = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBBRNT", 0.12f);
        LCDBurn = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBCBRN", 0.13f);
        InnerShadow = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBISHD", 0.6f);
        Segments = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBSEGC", 20);
        SegGap = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBSEGG", 3f);
        TopSpeed = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBTOPS", 50);
        SpeedUnit = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBSPUN", 0);
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBBACK", 1) == 1) ShowBackGround = true; else ShowBackGround = false;
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BackColor = new Color(PlayerPrefs.GetFloat(colorstart + "LDSBBACKCLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "LDSBBACKCLG", 0.7f), PlayerPrefs.GetFloat(colorstart + "LDSBBACKCLB", 0.3f));
        int l = PlayerPrefs.GetInt("MAINLANG"); if (!SceneMode) { yesofcourse.options.Clear(); 
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Speed" : (l == 1) ? "Vitesse" : (l == 2) ? "Geschwindigkeit" : "Viteză")); 
            yesofcourse.options.Add(new Dropdown.OptionData("RPM")); yesofcourse.options.Add(new Dropdown.OptionData("RPM x10"));
            yesofcourse.options.Add(new Dropdown.OptionData("RPM x100")); 
            if ((PlayerPrefs.GetInt("SPEEDSOURCE") == 2 || SpeedUnit >= 4)) { yesofcourse.options.Add(new Dropdown.OptionData("RPM x1000"));
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Fuel %" : (l == 1) ? "Carburant %" : (l == 2) ? "Kraftstoffstand" : "Combustibil %"));
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Coolant Temp." : (l == 1) ? "Température" : (l == 2) ? "Temperatur" : "Temperatură"));
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Engine Load" : (l == 1) ? "Charge du moteur" : (l == 2) ? "Motorlast" : "Sarcina motorului"));
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Throttle" : (l == 1) ? "Étrangler" : (l == 2) ? "Gaspedal" : "Accelerație")); }
            WSlide.value = Width; HSlide.value = Height; STopSpeed.text = TopSpeed + ""; yesofcourse.value = SpeedUnit;
            LDBurn.value = LCDBurn; BTRS.text = BurnTime.ToString(CultureInfo.InvariantCulture); Segs.text = Segments + ""; SegGp.value = SegGap;
            BGT.isOn = ShowBackGround; BCSelect.color = BackColor; BCSelect.color = BackColor; InnerSSlide.value = InnerShadow; RefRS.text = Refrate + ""; soo.text = gameObject.transform.GetSiblingIndex() + "";
        } StartCoroutine(yesofcoursea());
    }

    // Functie executata pentru a astepta un cadru si a actualiza interfata inainte de executarea logicii
    public IEnumerator yesofcoursea() {
        yield return null;
        LoadMode = false;
    }

    // Functie executata cand o culoare este selectata si schimbata in meniul de culoare
    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BackColor = new Color(PlayerPrefs.GetFloat(colorstart + "LDSBBACKCLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "LDSBBACKCLG", 0.7f), PlayerPrefs.GetFloat(colorstart + "LDSBBACKCLB", 0.3f));
        DigitColor = new Color(PlayerPrefs.GetFloat(colorstart + "LDSBDGITCLR", 0.16f), PlayerPrefs.GetFloat(colorstart + "LDSBDGITCLG", 0.16f), PlayerPrefs.GetFloat(colorstart + "LDSBDGITCLB", 0.16f));
        if (!SceneMode) if (ShowBackGround) gameObject.GetComponent<Image>().color = new Color(BackColor.r, BackColor.g, BackColor.b, 1f); 
            else gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0); if (!SceneMode) { BCSelect.color = BackColor; DGCSelect.color = DigitColor; }
        ClonerObj.GetComponent<Image>().color = DigitColor; ClonerObj.transform.GetChild(0).GetComponent<Image>().color = DigitColor;
        ClonerObj.transform.GetChild(1).GetComponent<Image>().color = DigitColor; OnComponentLCDSpBrModdify();
    }

    // Functie executata cand o proprietate este schimbata despre element
    public void OnComponentLCDSpBrModdify() {
        if (!SceneMode && !LoadMode) { Segments = int.Parse(Segs.text); SegGap = SegGp.value; Height = HSlide.value; Width = WSlide.value;
            Refrate = int.Parse(RefRS.text); BurnTime = float.Parse(BTRS.text, CultureInfo.InvariantCulture); LCDBurn = LDBurn.value;
            ShowBackGround = BGT.isOn; InnerShadow = InnerSSlide.value; TopSpeed = int.Parse(STopSpeed.text); SpeedUnit = yesofcourse.value;
            if (SpeedUnit == 5 || SpeedUnit == 7 || SpeedUnit == 8) { TopSpeed = 100; STopSpeed.text = "100"; STopSpeed.enabled = false; } else { STopSpeed.enabled = true; } }
        foreach (Transform br in orgfel) Destroy(br.gameObject);
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Width, Height);
        if (ShowBackGround) { gameObject.GetComponent<Image>().color = new Color(BackColor.r, BackColor.g, BackColor.b);
        gameObject.transform.GetChild(0).GetComponent<RawImage>().color = new Color(0f, 0f, 0f, InnerShadow); } else {
            gameObject.GetComponent<Image>().color = new Color(0f,0f,0f,0f);
            gameObject.transform.GetChild(0).GetComponent<RawImage>().color = new Color(0f,0f,0f,0f); }
        float CompLeg = ((Width - 20f) / (float)Segments);
        float GappedCompLeg = ((Width - 20f) / (float)Segments) - SegGap;
        for(int s = 0; s < Segments; s++) { GameObject newSegmm = Instantiate(ClonerObj, orgfel);
            newSegmm.SetActive(true); newSegmm.GetComponent<RectTransform>().sizeDelta = new Vector2(GappedCompLeg, Height - 20f);
            newSegmm.GetComponent<RectTransform>().anchoredPosition = new Vector2((15f + (Width / (Segments * 2.035f)) - (GappedCompLeg / 2) + (s * CompLeg)), -4f);
            newSegmm.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(GappedCompLeg, Height - 20f);
            newSegmm.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(GappedCompLeg, Height - 20f);
            newSegmm.GetComponent<Image>().color = new Color(DigitColor.r, DigitColor.g, DigitColor.b, LCDBurn);
            newSegmm.name = s + ""; } if (!SceneMode && !LoadMode) {
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBHEIG", Height);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBWIDT", Width);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBREFR", Refrate);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBBRNT", BurnTime);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBCBRN", LCDBurn);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBISHD", InnerShadow);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBSEGC", Segments);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBTOPS", TopSpeed);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBSPUN", SpeedUnit);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBSEGG", SegGap);
            if(ShowBackGround) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBBACK", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDSBBACK", 0);
        }
    }

    // Functie executata cand o culoare trebuie schimbata
    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject);
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

    // Functie chemata in fiecare cadru | Functie folosita pentru a actualiza valoarea kilometrajului
    void Update() {
        if (SceneMode) {
            RTimel += Time.deltaTime;
            if (RTimel >= (1f / Refrate)) {
                LastSpeed = CurSpeed; if (SpeedUnit == 0) CurSpeed = SM.InterpSpeed;
                if (SpeedUnit == 1) CurSpeed = SM.RPM;
                if (SpeedUnit == 2) CurSpeed = SM.RPM / 10f;
                if (SpeedUnit == 3) CurSpeed = SM.RPM / 100f;
                if (SpeedUnit == 4) CurSpeed = SM.RPM / 1000f;
                if (SpeedUnit == 5) CurSpeed = SM.OBDFuel;
                if (SpeedUnit == 6) CurSpeed = SM.OBDCoolant;
                if (SpeedUnit == 7) CurSpeed = SM.OBDLoad;
                if (SpeedUnit == 8) CurSpeed = SM.OBDThrottle;
                for (int s = 0; s < Segments; s++) {
                    if (CurSpeed >= (((float)TopSpeed / (float)Segments) * (s + 1))) { orgfel.GetChild(s).GetChild(0).gameObject.SetActive(true); }
                    else { orgfel.GetChild(s).GetChild(0).gameObject.SetActive(false); }
                    if (LastSpeed >= (((float)TopSpeed / (float)Segments) * (s + 1))) { orgfel.GetChild(s).GetChild(1).gameObject.SetActive(true); }
                    else { orgfel.GetChild(s).GetChild(1).gameObject.SetActive(false); }
                } RTimel -= (1f / Refrate);
            } for (int s = 0; s < Segments; s++) {
                orgfel.GetChild(s).GetChild(1).GetComponent<Image>().color = new Color(DigitColor.r, DigitColor.g, DigitColor.b, (BurnTime - RTimel) * (1f / BurnTime));
            }
        }
    }
}
