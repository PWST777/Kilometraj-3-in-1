using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class LCDCircSpeedbar : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Bara de viteza LCD Circulara
    // Folosita pentru a modifica aspectul elementului 'Music Player's
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public SelectDrag Sdrg; // Clasa universala pentru aranjarea, crearea si stergerea elementelor
    public ColorPicker CoPi; // Clasa universala pentru selectarea culorii
    public SPEEDMANAGER SM; // Clasa universala pentru variabile de viteza, turatie, etc.
    public bool SceneMode; // Variabila folosita pentru a determina daca elementul este editad sau utilizat
    public int ElementID; // Vaiabila numerica autonumerotata pentru identificarea elementului
    public bool LoadMode;
    public GameObject EditorModPanel;

    // Lista de elenete si proprietati pentru clasa
    public GameObject ClonerObj;
    public Transform orgfel;
    public RawImage VoidF;
    public GameObject PaddingObj;
    public float FillAmount; public Slider FSlide;
    public float Height; public Slider HSlide;
    public int Refrate = 4; public InputField RefRS;
    public float BurnTime = 0.1f; public InputField BTRS;
    public float LCDBurn = 0.13f; public Slider LDBurn;
    public int Segments = 20; public InputField Segs;
    public float SegGap = 3f; public Slider SegGp;
    public bool ShowBackGround; public Toggle BGT;
    public bool CCWise; public Toggle CCW;
    public Color BackColor; public Image BCSelect;
    public Color DigitColor; public Image DGCSelect;
    public int TopSpeed = 50; public InputField STopSpeed;
    public float Padding = 0.5f; public Slider PadSSlide;
    public int SpeedUnit; public Dropdown yesofcourse;
    public Button DelObj; public InputField soo;
    public float LastSpeed; public float CurSpeed; float RTimel = 0f;

    // Functie executata cand elementul este activat pentru prima data
    void Start() {
        SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        if (LoadMode) OnSelectOfElement();
        OnComponentLCDCiSpBrModdify();
        OnColorModdify();
    }

    // Functie executata cand elementul este creat sau este selectat
    public void OnSelectOfElement() { if (!SceneMode) { LoadMode = true;
        // Aplicarea functiilor pe elementele de interfata (elemente Button, InputField etc.)
        FSlide.onValueChanged.RemoveAllListeners(); FSlide.onValueChanged.AddListener((_) => OnComponentLCDCiSpBrModdify()); 
        HSlide.onValueChanged.RemoveAllListeners(); HSlide.onValueChanged.AddListener((_) => OnComponentLCDCiSpBrModdify());
        LDBurn.onValueChanged.RemoveAllListeners(); LDBurn.onValueChanged.AddListener((_) => OnComponentLCDCiSpBrModdify());
        BGT.onValueChanged.RemoveAllListeners(); BGT.onValueChanged.AddListener((_) => OnComponentLCDCiSpBrModdify());
        CCW.onValueChanged.RemoveAllListeners(); CCW.onValueChanged.AddListener((_) => OnComponentLCDCiSpBrModdify());
        PadSSlide.onValueChanged.RemoveAllListeners(); PadSSlide.onValueChanged.AddListener((_) => OnComponentLCDCiSpBrModdify());
        RefRS.onSubmit.RemoveAllListeners(); RefRS.onSubmit.AddListener((_) => OnComponentLCDCiSpBrModdify());
        Segs.onSubmit.RemoveAllListeners(); Segs.onSubmit.AddListener((_) => OnComponentLCDCiSpBrModdify());
        SegGp.onValueChanged.RemoveAllListeners(); SegGp.onValueChanged.AddListener((_) => OnComponentLCDCiSpBrModdify());
        yesofcourse.onValueChanged.RemoveAllListeners(); yesofcourse.onValueChanged.AddListener((_) => OnComponentLCDCiSpBrModdify());
        STopSpeed.onSubmit.RemoveAllListeners(); STopSpeed.onSubmit.AddListener((_) => OnComponentLCDCiSpBrModdify());
        BCSelect.GetComponent<Button>().onClick.RemoveAllListeners(); BCSelect.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("LDCSBBACKCL"));
        DGCSelect.GetComponent<Button>().onClick.RemoveAllListeners(); DGCSelect.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("LDCSBDGITCL"));
        DelObj.onClick.RemoveAllListeners(); DelObj.onClick.AddListener(() => DeleteObj(69));
        soo.onSubmit.RemoveAllListeners(); soo.onSubmit.AddListener((_) => ModSOrtO());
        BTRS.onSubmit.RemoveAllListeners(); BTRS.onSubmit.AddListener((_) => OnComponentLCDCiSpBrModdify()); }
        // Incarcarea variabilelor salvate
        Height = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBHEIG", 300f);
        FillAmount = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBWIDT", 0.875f);
        Refrate = (int)PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBREFR", 5);
        BurnTime = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBBRNT", 0.12f);
        LCDBurn = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBCBRN", 0.13f);
        Padding = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBISHD", 20f);
        Segments = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBSEGC", 20);
        SegGap = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBSEGG", 3f);
        TopSpeed = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBTOPS", 50);
        SpeedUnit = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBSPUN", 0);
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBBACK", 1) == 1) ShowBackGround = true; else ShowBackGround = false;
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSCCWIS", 1) == 1) CCWise = true; else CCWise = false;
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BackColor = new Color(PlayerPrefs.GetFloat(colorstart + "LDCSBBACKCLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "LDCSBBACKCLG", 0.7f), PlayerPrefs.GetFloat(colorstart + "LDCSBBACKCLB", 0.3f));
        int l = PlayerPrefs.GetInt("MAINLANG"); if (!SceneMode) { yesofcourse.options.Clear();
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Speed" : (l == 1) ? "Vitesse" : (l == 2) ? "Geschwindigkeit" : "Viteză"));
            yesofcourse.options.Add(new Dropdown.OptionData("RPM")); yesofcourse.options.Add(new Dropdown.OptionData("RPM x10")); 
            yesofcourse.options.Add(new Dropdown.OptionData("RPM x100")); 
            if ((PlayerPrefs.GetInt("SPEEDSOURCE") == 2 || SpeedUnit >= 4)) { yesofcourse.options.Add(new Dropdown.OptionData("RPM x1000"));
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Fuel %" : (l == 1) ? "Carburant %" : (l == 2) ? "Kraftstoffstand" : "Combustibil %"));
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Coolant Temp." : (l == 1) ? "Température" : (l == 2) ? "Temperatur" : "Temperatură"));
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Engine Load" : (l == 1) ? "Charge du moteur" : (l == 2) ? "Motorlast" : "Sarcina motorului"));
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Throttle" : (l == 1) ? "Étrangler" : (l == 2) ? "Gaspedal" : "Accelerație")); }
            FSlide.value = FillAmount; HSlide.value = Height; STopSpeed.text = TopSpeed + ""; CCW.isOn = CCWise; yesofcourse.value = SpeedUnit;
            LDBurn.value = LCDBurn; BTRS.text = BurnTime.ToString(CultureInfo.InvariantCulture); Segs.text = Segments + ""; SegGp.value = SegGap;
            BGT.isOn = ShowBackGround; BCSelect.color = BackColor; BCSelect.color = BackColor; PadSSlide.value = Padding; RefRS.text = Refrate + ""; soo.text = gameObject.transform.GetSiblingIndex() + "";
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
        BackColor = new Color(PlayerPrefs.GetFloat(colorstart + "LDCSBBACKCLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "LDCSBBACKCLG", 0.7f), PlayerPrefs.GetFloat(colorstart + "LDCSBBACKCLB", 0.3f));
        DigitColor = new Color(PlayerPrefs.GetFloat(colorstart + "LDCSBDGITCLR", 0.16f), PlayerPrefs.GetFloat(colorstart + "LDCSBDGITCLG", 0.16f), PlayerPrefs.GetFloat(colorstart + "LDCSBDGITCLB", 0.16f));
        if (!SceneMode) if (ShowBackGround) { gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(BackColor.r, BackColor.g, BackColor.b, 1f); PaddingObj.GetComponent<Image>().color = new Color(BackColor.r, BackColor.g, BackColor.b, 1f); }
            else { gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f); PaddingObj.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f); } if (!SceneMode) { BCSelect.color = BackColor; DGCSelect.color = DigitColor; }
        ClonerObj.transform.GetChild(0).GetComponent<Image>().color = DigitColor; ClonerObj.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = DigitColor;
        ClonerObj.transform.GetChild(0).GetChild(1).GetComponent<Image>().color = DigitColor; OnComponentLCDCiSpBrModdify();
    }

    // Functie executata cand o proprietate este schimbata despre element
    public void OnComponentLCDCiSpBrModdify() {
        if (!SceneMode && !LoadMode) { Segments = int.Parse(Segs.text); SegGap = SegGp.value; Height = HSlide.value; FillAmount = FSlide.value;
            Refrate = int.Parse(RefRS.text); BurnTime = float.Parse(BTRS.text, CultureInfo.InvariantCulture); LCDBurn = LDBurn.value;
            ShowBackGround = BGT.isOn; Padding = PadSSlide.value; TopSpeed = int.Parse(STopSpeed.text); CCWise = CCW.isOn; SpeedUnit = yesofcourse.value;
            if (SpeedUnit == 5 || SpeedUnit == 7 || SpeedUnit == 8) { TopSpeed = 100; STopSpeed.text = "100"; STopSpeed.enabled = false; } else { STopSpeed.enabled = true; } }
        foreach (Transform br in orgfel) Destroy(br.gameObject); gameObject.transform.GetChild(0).GetComponent<Image>().fillClockwise = CCWise; PaddingObj.GetComponent<Image>().fillClockwise = CCWise;
        gameObject.transform.GetChild(0).GetComponent<Image>().fillAmount = FillAmount; PaddingObj.GetComponent<Image>().fillAmount = FillAmount;
        float scale = 800f / (Mathf.Max(800f - Height, 1f) + 3f);
        VoidF.uvRect = new Rect((-0.5f * (scale - 1f)), (-0.5f * (scale - 1f)), scale, scale);
        PaddingObj.GetComponent<RectTransform>().sizeDelta = new Vector2((800f - Height) + Padding, (800f - Height) + Padding);
        float Fillram = ((FillAmount - (Padding / 1700f)) / Segments) - (SegGap / 1000f);
        float Fillramlossy = ((FillAmount - (Padding / 1700f)) / Segments);
        ClonerObj.transform.GetChild(0).GetComponent<Image>().fillAmount = Fillram;
        ClonerObj.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(800f - Padding, 800f - Padding);
        ClonerObj.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = Fillram;
        ClonerObj.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(800f - Padding, 800f - Padding);
        ClonerObj.transform.GetChild(0).GetChild(1).GetComponent<Image>().fillAmount = Fillram;
        ClonerObj.transform.GetChild(0).GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(800f - Padding, 800f - Padding);
        ClonerObj.transform.GetChild(0).GetComponent<Image>().color = new Color( DigitColor.r, DigitColor.g, DigitColor.b, LCDBurn);
        if(ShowBackGround) { gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(BackColor.r, BackColor.g, BackColor.b, 1f); PaddingObj.GetComponent<Image>().color = new Color(BackColor.r, BackColor.g, BackColor.b, 1f); }
        else { gameObject.transform.GetChild(0).GetComponent<Image>().color = new Color(0f,0f,0f, 0f); PaddingObj.GetComponent<Image>().color = new Color(0f,0f,0f, 0f); }
        float CurrntAngle = (Padding / 8f) + (SegGap / 8f) - 3f; if (!CCWise) { CurrntAngle += 3f; CurrntAngle += Fillramlossy * 360f; }
        for (int s = 0; s < Segments; s++) {
            GameObject mynewr = Instantiate(ClonerObj, orgfel); mynewr.SetActive(true);
            if(CCWise) mynewr.transform.localRotation = Quaternion.Euler(0f, 0f, -CurrntAngle);
            else mynewr.transform.localRotation = Quaternion.Euler(0f, 0f, CurrntAngle);
            CurrntAngle += Fillramlossy * 360f;
        } if (!SceneMode && !LoadMode) {
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBHEIG", Height);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBWIDT", FillAmount);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBREFR", Refrate);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBBRNT", BurnTime);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBCBRN", LCDBurn);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBISHD", Padding);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBSEGC", Segments);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBTOPS", TopSpeed);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBSEGG", SegGap);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBSPUN", SpeedUnit);
            if (ShowBackGround) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBBACK", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSBBACK", 0);
            if (CCWise) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSCCWIS", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LDCSCCWIS", 0);
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

    // Functie chemata in fiecare cadru | Functie folosita pentru a actualiza valoarea de pe kilometraj
    void Update() {
        if (SceneMode) {
            RTimel += Time.deltaTime;
            if (RTimel >= (1f / Refrate)) {
                LastSpeed = CurSpeed; if(SpeedUnit == 0) CurSpeed = SM.InterpSpeed;
                if (SpeedUnit == 1) CurSpeed = SM.RPM;
                if (SpeedUnit == 2) CurSpeed = SM.RPM / 10f;
                if (SpeedUnit == 3) CurSpeed = SM.RPM / 100f;
                if (SpeedUnit == 4) CurSpeed = SM.RPM / 1000f;
                if (SpeedUnit == 5) CurSpeed = SM.OBDFuel;
                if (SpeedUnit == 6) CurSpeed = SM.OBDCoolant;
                if (SpeedUnit == 7) CurSpeed = SM.OBDLoad;
                if (SpeedUnit == 8) CurSpeed = SM.OBDThrottle;
                for (int s = 0; s < Segments; s++) {
                    if (CurSpeed >= (((float)TopSpeed / (float)Segments) * (s + 1))) { orgfel.GetChild(s).GetChild(0).GetChild(0).gameObject.SetActive(true); }
                    else { orgfel.GetChild(s).GetChild(0).GetChild(0).gameObject.SetActive(false); }
                    if (LastSpeed >= (((float)TopSpeed / (float)Segments) * (s + 1))) { orgfel.GetChild(s).GetChild(0).GetChild(1).gameObject.SetActive(true); }
                    else { orgfel.GetChild(s).GetChild(0).GetChild(1).gameObject.SetActive(false); }
                } RTimel -= (1f / Refrate);
            } for (int s = 0; s < Segments; s++) {
                orgfel.GetChild(s).GetChild(0).GetChild(1).GetComponent<Image>().color = new Color(DigitColor.r, DigitColor.g, DigitColor.b, (BurnTime - RTimel) * (1f / BurnTime));
            }
        } string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET");
    }
}
