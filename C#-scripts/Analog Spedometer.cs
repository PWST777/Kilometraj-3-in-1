using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalogSpedometer : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Kilometraj Analogic
    // Folosit pentru elementul Kilometraj Analogic
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public SelectDrag Sdrg; // Clasa universala pentru selectarea culorii
    public ColorPicker CoPi; // Clasa universala pentru aranjarea, crearea si stergerea elementelor
    public SPEEDMANAGER SM; // Clasa universala pentru variabile de viteza, turatie, etc.
    public bool SceneMode; // Variabila folosita pentru a determina daca elementul este editad sau utilizat
    public int ElementID; // Vaiabila numerica autonumerotata pentru identificarea elementului
    public bool LoadMode;
    public float StartFill;
    public float EndFill;
    public Image BackOutl;
    public Image Backround1;
    public Image Backround2;
    public Image BackLine2Fill;
    public Image BackLine1Fill;
    public Image StartLFill;
    public RectTransform BaseOfNeedle;
    public RectTransform Needles;
    public GameObject MarkText;
    public GameObject MarkNoText;
    public Transform orgfel;
    public Text UnitLabel;

    public GameObject EditorModPanel;

    // Lista de elenete si proprietati pentru clasa
    public float sfam; public Slider SFillAmS;
    public float efam; public Slider EFillAmS;
    public float osam; public Slider OutSAmS;
    public float lwam; public Slider LineWAmS;
    public float loam; public Slider LineOAmS;
    public float fdam; public Slider FillDifAmS;
    public float bsam; public Slider BaseSizeAmS;
    public float nwam; public Slider NeedWAmS;
    public float nsam; public Slider NeedSLAmS;
    public float elam; public Slider NeedELAmS;
    public float dcam; public Slider NeedDCAmS;
    public float msif; public InputField MaxSpeed;
    public float c2if; public InputField C2Speed;
    public float mrif; public InputField MarkRate;
    public float ilif; public InputField InbLines;
    public float maas; public Slider MarkAmS;
    public float tmas; public Slider TextMarkAmS;
    public int fonts; public Dropdown Fontttss;
    public int speedMODE; public Dropdown sMode;
    public bool LabelOn; public Toggle OnLa;
    public float LabelSize; public Slider LabelLaSi;
    public List<Font> actfonts;
    public Color backcl; public Image BaClImage;
    public Color outlcl; public Image OuClImage;
    public Color c1clli; public Image C1ClImage;
    public Color c2clli; public Image C2ClImage;
    public Color needcl; public Image NeClImage;
    public Color mar1cl; public Image MaC1Image;
    public Color mar2cl; public Image MaC2Image;
    public Color unitcl; public Image UnLaImage;
    public InputField soo; public Button KJURedButton;

    // Functie executata cand elementul este activat pentru prima data
    void Start() {
        SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        if (LoadMode) OnSelectOfElement();
        OnComponentAnSpModdify();
        OnMarksRegenerate();
        OnColorModdify();
    }

    // Functie executata cand elementul este creat sau este selectat
    public void OnSelectOfElement() { if (!SceneMode) { LoadMode = true;
        // Aplicarea functiilor pe elementele de interfata (elemente Button, InputField etc.)
        SFillAmS.onValueChanged.RemoveAllListeners(); SFillAmS.onValueChanged.AddListener((_) => OnComponentAnSpModdify()); SFillAmS.onValueChanged.AddListener((_) => OnMarksRegenerate());
        EFillAmS.onValueChanged.RemoveAllListeners(); EFillAmS.onValueChanged.AddListener((_) => OnComponentAnSpModdify()); EFillAmS.onValueChanged.AddListener((_) => OnMarksRegenerate());
        OutSAmS.onValueChanged.RemoveAllListeners(); OutSAmS.onValueChanged.AddListener((_) => OnComponentAnSpModdify());
        LineWAmS.onValueChanged.RemoveAllListeners(); LineWAmS.onValueChanged.AddListener((_) => OnComponentAnSpModdify());
        LineOAmS.onValueChanged.RemoveAllListeners(); LineOAmS.onValueChanged.AddListener((_) => OnComponentAnSpModdify());
        FillDifAmS.onValueChanged.RemoveAllListeners(); FillDifAmS.onValueChanged.AddListener((_) => OnComponentAnSpModdify());
        BaseSizeAmS.onValueChanged.RemoveAllListeners(); BaseSizeAmS.onValueChanged.AddListener((_) => OnComponentAnSpModdify());
        NeedWAmS.onValueChanged.RemoveAllListeners(); NeedWAmS.onValueChanged.AddListener((_) => OnComponentAnSpModdify());
        NeedSLAmS.onValueChanged.RemoveAllListeners(); NeedSLAmS.onValueChanged.AddListener((_) => OnComponentAnSpModdify());
        NeedELAmS.onValueChanged.RemoveAllListeners(); NeedELAmS.onValueChanged.AddListener((_) => OnComponentAnSpModdify());
        NeedDCAmS.onValueChanged.RemoveAllListeners(); NeedDCAmS.onValueChanged.AddListener((_) => OnComponentAnSpModdify());
        sMode.onValueChanged.RemoveAllListeners(); sMode.onValueChanged.AddListener((_) => OnMarksRegenerate());
        OnLa.onValueChanged.RemoveAllListeners(); OnLa.onValueChanged.AddListener((_) => OnMarksRegenerate());
        LabelLaSi.onValueChanged.RemoveAllListeners(); LabelLaSi.onValueChanged.AddListener((_) => OnMarksRegenerate());
        MaxSpeed.onSubmit.RemoveAllListeners(); MaxSpeed.onSubmit.AddListener((_) => OnMarksRegenerate());
        C2Speed.onSubmit.RemoveAllListeners(); C2Speed.onSubmit.AddListener((_) => OnMarksRegenerate());
        MarkRate.onSubmit.RemoveAllListeners(); MarkRate.onSubmit.AddListener((_) => OnMarksRegenerate());
        InbLines.onSubmit.RemoveAllListeners(); InbLines.onSubmit.AddListener((_) => OnMarksRegenerate());
        MarkAmS.onValueChanged.RemoveAllListeners(); MarkAmS.onValueChanged.AddListener((_) => OnMarksRegenerate());
        TextMarkAmS.onValueChanged.RemoveAllListeners(); TextMarkAmS.onValueChanged.AddListener((_) => OnMarksRegenerate());
        Fontttss.onValueChanged.RemoveAllListeners(); Fontttss.onValueChanged.AddListener((_) => OnMarksRegenerate());
        BaClImage.GetComponent<Button>().onClick.RemoveAllListeners(); BaClImage.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASBACKCL"));
        OuClImage.GetComponent<Button>().onClick.RemoveAllListeners(); OuClImage.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASOUTLCL"));
        C1ClImage.GetComponent<Button>().onClick.RemoveAllListeners(); C1ClImage.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASC1CLLI"));
        C2ClImage.GetComponent<Button>().onClick.RemoveAllListeners(); C2ClImage.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASC2CLLI"));
        NeClImage.GetComponent<Button>().onClick.RemoveAllListeners(); NeClImage.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASNEEDCL"));
        MaC1Image.GetComponent<Button>().onClick.RemoveAllListeners(); MaC1Image.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASMAR1CL"));
        MaC2Image.GetComponent<Button>().onClick.RemoveAllListeners(); MaC2Image.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASMAR2CL"));
        UnLaImage.GetComponent<Button>().onClick.RemoveAllListeners(); UnLaImage.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ASUNLACL"));
        soo.onSubmit.RemoveAllListeners(); soo.onSubmit.AddListener((_) => ModSOrtO());
        KJURedButton.onClick.RemoveAllListeners(); KJURedButton.onClick.AddListener(() => DeleteObj(69)); }
        // Incarcarea variabilelor salvate
        sfam = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASSFAM", 0.125f);
        efam = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASEFAM", 0.875f);
        osam = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASOSAM", 20f);
        lwam = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASLWAM", 20f);
        loam = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASLOAM", 35f);
        fdam = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASFDAM", 0.6f);
        bsam = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBSAM", 150f);
        nwam = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASNWAM", 70f);
        nsam = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASNSAM", 150f);
        elam = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASELAM", 100f);
        dcam = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASDCAM", 0.1f);
        msif = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASMSIF", 50f);
        c2if = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASC2IF", 32f);
        mrif = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASMRIF", 5f);
        ilif = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASILIF", 1f);
        maas = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASMAAS", 1f);
        tmas = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASTMAS", 1f);
        LabelSize = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASLASI", 50f);
        fonts = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASFONT", 0);
        speedMODE = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASSPMD", 0);
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASLABL", 1) == 1) LabelOn = true; else LabelOn = false;
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        backcl = new Color(PlayerPrefs.GetFloat(colorstart + "ASBACKCLR", 0.15f), PlayerPrefs.GetFloat(colorstart + "ASBACKCLG", 0.15f), PlayerPrefs.GetFloat(colorstart + "ASBACKCLB", 0.15f));
        outlcl = new Color(PlayerPrefs.GetFloat(colorstart + "ASOUTLCLR", 0f), PlayerPrefs.GetFloat(colorstart + "ASOUTLCLG", 0f), PlayerPrefs.GetFloat(colorstart + "ASOUTLCLB", 0f));
        c1clli = new Color(PlayerPrefs.GetFloat(colorstart + "ASC1CLLIR", 0f), PlayerPrefs.GetFloat(colorstart + "ASC1CLLIG", 0.75f), PlayerPrefs.GetFloat(colorstart + "ASC1CLLIB", 0f));
        c2clli = new Color(PlayerPrefs.GetFloat(colorstart + "ASC2CLLIR", 0.75f), PlayerPrefs.GetFloat(colorstart + "ASC2CLLIG", 0.28f), PlayerPrefs.GetFloat(colorstart + "ASC2CLLIB", 0f));
        needcl = new Color(PlayerPrefs.GetFloat(colorstart + "ASNEEDCLR", 0.95f), PlayerPrefs.GetFloat(colorstart + "ASNEEDCLG", 0.45f), PlayerPrefs.GetFloat(colorstart + "ASNEEDCLB", 0f));
        mar1cl = new Color(PlayerPrefs.GetFloat(colorstart + "ASMAR1CLR", 0.9f), PlayerPrefs.GetFloat(colorstart + "ASMAR1CLG", 0.9f), PlayerPrefs.GetFloat(colorstart + "ASMAR1CLB", 0.9f));
        mar2cl = new Color(PlayerPrefs.GetFloat(colorstart + "ASMAR2CLR", 0.95f), PlayerPrefs.GetFloat(colorstart + "ASMAR2CLG", 0.45f), PlayerPrefs.GetFloat(colorstart + "ASMAR2CLB", 0.45f));
        unitcl = new Color(PlayerPrefs.GetFloat(colorstart + "ASUNLACLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "ASUNLACLG", 0.6f), PlayerPrefs.GetFloat(colorstart + "ASUNLACLB", 0.6f));
        int l = PlayerPrefs.GetInt("MAINLANG"); if (!SceneMode) { sMode.options.Clear(); 
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Speed" : (l == 1) ? "Vitesse" : (l == 2) ? "Geschwindigkeit" : "Viteză"));
            sMode.options.Add(new Dropdown.OptionData("RPM")); sMode.options.Add(new Dropdown.OptionData("RPM x10")); 
            sMode.options.Add(new Dropdown.OptionData("RPM x100"));
            if ((PlayerPrefs.GetInt("SPEEDSOURCE") == 2 || speedMODE >= 4)) { sMode.options.Add(new Dropdown.OptionData("RPM x1000"));
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Fuel %" : (l == 1) ? "Carburant %" : (l == 2) ? "Kraftstoffstand" : "Combustibil %")); 
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Coolant Temp." : (l == 1) ? "Température" : (l == 2) ? "Temperatur" : "Temperatură"));
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Engine Load" : (l == 1) ? "Charge du moteur" : (l == 2) ? "Motorlast" : "Sarcina motorului"));
            sMode.options.Add(new Dropdown.OptionData((l == 0) ? "Throttle" : (l == 1) ? "Étrangler" : (l == 2) ? "Gaspedal" : "Accelerație")); }
            sMode.value = speedMODE; OnLa.isOn = LabelOn; LabelLaSi.value = LabelSize;
            SFillAmS.value = sfam; EFillAmS.value = efam; OutSAmS.value = osam; LineWAmS.value = lwam; FillDifAmS.value = fdam;
            BaseSizeAmS.value = bsam; NeedWAmS.value = nwam; NeedSLAmS.value = nsam; NeedELAmS.value = elam; NeedDCAmS.value = dcam;
            MaxSpeed.text = msif + ""; C2Speed.text = c2if + ""; MarkRate.text = mrif + ""; InbLines.text = ilif + ""; Fontttss.value = fonts;
            MarkAmS.value = maas; TextMarkAmS.value = tmas; BaClImage.color = backcl; OuClImage.color = outlcl; C1ClImage.color = c1clli; LineOAmS.value = loam;
            C2ClImage.color = c2clli; NeClImage.color = needcl; MaC1Image.color = mar1cl; MaC2Image.color = mar2cl; UnLaImage.color = unitcl;
            soo.text = gameObject.transform.GetSiblingIndex() + ""; } StartCoroutine(yesofcourse());
    }

    // Functie executata pentru a astepta un cadru si a actualiza interfata inainte de executarea logicii
    public IEnumerator yesofcourse() {
        yield return null;
        LoadMode = false;
    }

    // Functie executata cand o culoare este selectata si schimbata in meniul de culoare
    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        backcl = new Color(PlayerPrefs.GetFloat(colorstart + "ASBACKCLR", 0.15f), PlayerPrefs.GetFloat(colorstart + "ASBACKCLG", 0.15f), PlayerPrefs.GetFloat(colorstart + "ASBACKCLB", 0.15f));
        outlcl = new Color(PlayerPrefs.GetFloat(colorstart + "ASOUTLCLR", 0f), PlayerPrefs.GetFloat(colorstart + "ASOUTLCLG", 0f), PlayerPrefs.GetFloat(colorstart + "ASOUTLCLB", 0f));
        c1clli = new Color(PlayerPrefs.GetFloat(colorstart + "ASC1CLLIR", 0f), PlayerPrefs.GetFloat(colorstart + "ASC1CLLIG", 0.75f), PlayerPrefs.GetFloat(colorstart + "ASC1CLLIB", 0f));
        c2clli = new Color(PlayerPrefs.GetFloat(colorstart + "ASC2CLLIR", 0.75f), PlayerPrefs.GetFloat(colorstart + "ASC2CLLIG", 0.28f), PlayerPrefs.GetFloat(colorstart + "ASC2CLLIB", 0f));
        needcl = new Color(PlayerPrefs.GetFloat(colorstart + "ASNEEDCLR", 0.95f), PlayerPrefs.GetFloat(colorstart + "ASNEEDCLG", 0.45f), PlayerPrefs.GetFloat(colorstart + "ASNEEDCLB", 0f));
        mar1cl = new Color(PlayerPrefs.GetFloat(colorstart + "ASMAR1CLR", 0.9f), PlayerPrefs.GetFloat(colorstart + "ASMAR1CLG", 0.9f), PlayerPrefs.GetFloat(colorstart + "ASMAR1CLB", 0.9f));
        mar2cl = new Color(PlayerPrefs.GetFloat(colorstart + "ASMAR2CLR", 0.95f), PlayerPrefs.GetFloat(colorstart + "ASMAR2CLG", 0.45f), PlayerPrefs.GetFloat(colorstart + "ASMAR2CLB", 0.45f));
        unitcl = new Color(PlayerPrefs.GetFloat(colorstart + "ASUNLACLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "ASUNLACLG", 0.6f), PlayerPrefs.GetFloat(colorstart + "ASUNLACLB", 0.6f));
        if (!SceneMode) { BaClImage.color = backcl; OuClImage.color = outlcl; C1ClImage.color = c1clli; C2ClImage.color = c2clli;
            NeClImage.color = needcl; MaC1Image.color = mar1cl; MaC2Image.color = mar2cl; UnLaImage.color = unitcl;
        } Backround1.color = backcl; Backround2.color = backcl; StartLFill.color = backcl; BackOutl.color = outlcl; BackLine1Fill.color = c1clli;
        BackLine2Fill.color = c2clli; Needles.GetComponent<RawImage>().color = needcl; UnitLabel.color = unitcl; OnMarksRegenerate(); OnComponentAnSpModdify();
    }

    // Functie executata cand ordinea obiectului este schimbata in scena
    public void ModSOrtO() {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "SO", int.Parse(soo.text));
        gameObject.GetComponent<SortOrderOrg>().Apply();
    }

    // Functie executata cand o culoare trebuie schimbata
    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject);
    }

    // Functie executata cand o proprietate din afara categoriei de marcaje este schimbata despre element
    public void OnComponentAnSpModdify() {
        if (!SceneMode && !LoadMode) { loam = LineOAmS.value;
            sfam = SFillAmS.value; efam = EFillAmS.value; osam = OutSAmS.value; lwam = LineWAmS.value;
            fdam = FillDifAmS.value; bsam = BaseSizeAmS.value; nwam = NeedWAmS.value; nsam = NeedSLAmS.value;
            elam = NeedELAmS.value; dcam = NeedDCAmS.value; }
        StartFill = sfam; EndFill = efam;
        Backround1.GetComponent<RectTransform>().sizeDelta = new Vector2(1000f - osam, 1000f - osam);
        StartLFill.GetComponent<RectTransform>().sizeDelta = new Vector2(1000f - osam, 1000f - osam);
        Backround2.GetComponent<RectTransform>().sizeDelta = new Vector2(1000f - osam - lwam - loam, 1000f - osam - lwam - loam);
        BackLine1Fill.GetComponent<RectTransform>().sizeDelta = new Vector2(1000f - osam - loam, 1000f - osam - loam);
        BackLine2Fill.GetComponent<RectTransform>().sizeDelta = new Vector2(1000f - osam - loam, 1000f - osam - loam);
        if (!SceneMode) {
            FillDifAmS.minValue = StartFill;
            FillDifAmS.maxValue = EndFill;
        } BackLine2Fill.fillAmount = EndFill;
        BackLine1Fill.fillAmount = fdam;
        StartLFill.fillAmount = StartFill;
        StartLFill.color = Backround1.color;
        Backround2.color = Backround1.color;
        if (!SceneMode) FillDifAmS.transform.GetChild(0).GetComponent<Image>().color = BackLine2Fill.color;
        if (!SceneMode) FillDifAmS.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = BackLine1Fill.color;
        BaseOfNeedle.sizeDelta = new Vector2(bsam, bsam);
        UnitLabel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -(bsam / 2f));
        Needles.sizeDelta = new Vector2(Needles.sizeDelta.x, nwam);
        Needles.localPosition = new Vector2(-160f + ((elam - nsam) / 2f), 0f);
        Needles.sizeDelta = new Vector2(300f + nsam + elam, Needles.sizeDelta.y);
        Needles.GetComponent<RawImage>().uvRect = new Rect(0.04f, 0f, dcam, 1f);
        if (!SceneMode && !LoadMode) { // Salvarea variabilelor
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASSFAM", sfam); 
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASEFAM", efam); 
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASOSAM", osam); 
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASLWAM", lwam); 
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASFDAM", fdam);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASBSAM", bsam);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASNWAM", nwam);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASNSAM", nsam);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASELAM", elam);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASDCAM", dcam);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASLOAM", loam);
        }
    }

    // Functie executata cand o proprietate din categoria de marcaje este schimbata despre element | Aici marcajele sunt regenerate
    public void OnMarksRegenerate() {
        if (!SceneMode && !LoadMode) { speedMODE = sMode.value; LabelOn = OnLa.isOn; LabelSize = LabelLaSi.value;
        msif = float.Parse(MaxSpeed.text); c2if = float.Parse(C2Speed.text); mrif = float.Parse(MarkRate.text);
        ilif = float.Parse(InbLines.text); maas = MarkAmS.value; tmas = TextMarkAmS.value; fonts = Fontttss.value;
        if (speedMODE == 5 || speedMODE == 7 || speedMODE == 8) { msif = 100f; MaxSpeed.text = "100"; MaxSpeed.enabled = false; } else { MaxSpeed.enabled = true; } }
        MarkText.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().font = actfonts[fonts];
        MarkText.transform.GetChild(0).transform.localScale = new Vector2(maas, maas);
        MarkText.transform.GetChild(0).transform.GetChild(0).transform.localScale = new Vector2(tmas / 2.5f, tmas / 2.5f);
        MarkText.transform.GetChild(0).transform.GetChild(0).transform.localPosition = new Vector2(0f, 75f + (tmas * 20f));
        MarkNoText.transform.GetChild(0).transform.localScale = new Vector2(maas, maas);
        for (int c = 0; c < orgfel.childCount; c++)
            Destroy(orgfel.GetChild(c).gameObject);
        float CurrentSpeedOfMark = 0; int MCon = 0;
        while(CurrentSpeedOfMark <= msif) {
            if(MCon % (ilif + 1) == 0) { GameObject Marks1 = Instantiate(MarkText, orgfel); Marks1.SetActive(true);
                Marks1.transform.localRotation = Quaternion.Euler(0f, 0f, -((StartFill * 360f) + ((CurrentSpeedOfMark / msif) * ((EndFill - StartFill) * 360f))));
                Marks1.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = CurrentSpeedOfMark.ToString("0"); Marks1.transform.GetChild(0).transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, 0f);
                if (CurrentSpeedOfMark >= c2if) { Marks1.transform.GetChild(0).GetComponent<RawImage>().color = mar2cl; Marks1.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = mar2cl; }
                else { Marks1.transform.GetChild(0).GetComponent<RawImage>().color = mar1cl; Marks1.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = mar1cl; }
            }else { GameObject Marks2 = Instantiate(MarkNoText, orgfel); Marks2.SetActive(true);
                Marks2.transform.localRotation = Quaternion.Euler(0f, 0f, -((StartFill * 360f) + ((CurrentSpeedOfMark / msif) * ((EndFill - StartFill) * 360f))));
                if (CurrentSpeedOfMark >= c2if) { Marks2.transform.GetChild(0).GetComponent<RawImage>().color = mar2cl; }
                else { Marks2.transform.GetChild(0).GetComponent<RawImage>().color = mar1cl; }
            } CurrentSpeedOfMark += mrif / (ilif + 1f); MCon++;
        } if(LabelOn) { try { if(speedMODE == 0) if(!SM.MPHMode) UnitLabel.text = "km/h"; else UnitLabel.text = "mph";
            if (speedMODE == 1) UnitLabel.text = "RPM";
            if (speedMODE == 2) UnitLabel.text = "RPM x10";
            if (speedMODE == 3) UnitLabel.text = "RPM x100";
            if (speedMODE == 4) UnitLabel.text = "RPM x1000";
            if (speedMODE == 5) UnitLabel.text = "Fuel (%)";
            if (speedMODE == 6) if (!SM.MPHMode) UnitLabel.text = "C°"; else UnitLabel.text = "F°"; 
            if (speedMODE == 7) UnitLabel.text = "Load";
            if (speedMODE == 8) UnitLabel.text = "Throttle"; } // TRANSLATE
            catch { } 
            UnitLabel.fontSize = (int)LabelSize;
            UnitLabel.font = actfonts[fonts];
            UnitLabel.gameObject.SetActive(true);
        } else UnitLabel.gameObject.SetActive(false); if (!SceneMode && !LoadMode) { // Salvarea variabilelor
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASMSIF", msif);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASC2IF", c2if);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASMRIF", mrif);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASILIF", ilif);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASMAAS", maas);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASTMAS", tmas);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASFONT", fonts);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASSPMD", speedMODE);
            if(LabelOn) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASLABL", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASLABL", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ASLASI", LabelSize);
        }
    }

    // Functie executata cand obiectul este sters
    public void DeleteObj(int wf) {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TYPE", 69696);
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ") + 1);
        Sdrg.OnElemSelect(72769); Destroy(gameObject);
    }

    // Functie chemata de 60 de ori / secunda | Functie folosita pentru a actualiza pozitia acului pe kilometraj
    void FixedUpdate() { 
        if (SceneMode) { if (speedMODE == 0) { float limitcap = Mathf.Min(SM.InterpSpeed, msif); 
            BaseOfNeedle.transform.localRotation = Quaternion.Euler(0f, 0f, 90f - ((StartFill * 360f) + ((limitcap / msif) * ((EndFill - StartFill) * 360f)))); }
            if (speedMODE == 1) { float limitcapr = Mathf.Min(SM.RPM, msif); 
                BaseOfNeedle.transform.localRotation = Quaternion.Euler(0f, 0f, 90f - ((StartFill * 360f) + ((limitcapr / msif) * ((EndFill - StartFill) * 360f)))); }
            if (speedMODE == 2) { float limitcapr1 = Mathf.Min(SM.RPM / 10f, msif); 
                BaseOfNeedle.transform.localRotation = Quaternion.Euler(0f, 0f, 90f - ((StartFill * 360f) + ((limitcapr1 / msif) * ((EndFill - StartFill) * 360f)))); }
            if (speedMODE == 3) { float limitcapr10 = Mathf.Min(SM.RPM / 100f, msif); 
                BaseOfNeedle.transform.localRotation = Quaternion.Euler(0f, 0f, 90f - ((StartFill * 360f) + ((limitcapr10 / msif) * ((EndFill - StartFill) * 360f)))); }
            if (speedMODE == 4) { float limitcapr100 = Mathf.Min(SM.RPM / 1000f, msif); 
                BaseOfNeedle.transform.localRotation = Quaternion.Euler(0f, 0f, 90f - ((StartFill * 360f) + ((limitcapr100 / msif) * ((EndFill - StartFill) * 360f)))); }
            if (speedMODE == 5) { float limitcapr = Mathf.Min(SM.OBDFuel, 100f); 
                BaseOfNeedle.transform.localRotation = Quaternion.Euler(0f, 0f, 90f - ((StartFill * 360f) + ((limitcapr / 100f) * ((EndFill - StartFill) * 360f)))); }
            if (speedMODE == 6) { float limitcapr = Mathf.Min(SM.OBDCoolant, msif); 
                BaseOfNeedle.transform.localRotation = Quaternion.Euler(0f, 0f, 90f - ((StartFill * 360f) + ((limitcapr / msif) * ((EndFill - StartFill) * 360f)))); }
            if (speedMODE == 7) { float limitcapr = Mathf.Min(SM.OBDLoad, 100f); 
                BaseOfNeedle.transform.localRotation = Quaternion.Euler(0f, 0f, 90f - ((StartFill * 360f) + ((limitcapr / 100f) * ((EndFill - StartFill) * 360f)))); }
            if (speedMODE == 8) { float limitcapr = Mathf.Min(SM.OBDThrottle, 100f); 
                BaseOfNeedle.transform.localRotation = Quaternion.Euler(0f, 0f, 90f - ((StartFill * 360f) + ((limitcapr / 100f) * ((EndFill - StartFill) * 360f)))); }
        }
    }
}
