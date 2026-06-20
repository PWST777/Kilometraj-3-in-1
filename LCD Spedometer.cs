using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LCDSpedometer : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Kilometraj LCD
    // Folosita pentru elementul Kilometraj LCD
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public SelectDrag Sdrg; // Clasa universala pentru aranjarea, crearea si stergerea elementelor
    public ColorPicker CoPi; // Clasa universala pentru selectarea culorii
    public SPEEDMANAGER SM; // Clasa universala pentru variabile de viteza, turatie, etc.
    public bool SceneMode; // Variabila folosita pentru a determina daca elementul este editad sau utilizat
    public int ElementID; // Vaiabila numerica autonumerotata pentru identificarea elementului
    public bool LoadMode;
    public GameObject EditorModPanel;

    // Lista de elenete si proprietati pentru clasa
    public List<GameObject> LDigits;
    public int FontStSet; public Dropdown FStyleSelect;
    public int DigitsNr = 1; public InputField DNrSel;
    public bool Decimal = true; public Toggle ShowDecimal;
    public float DecimalSize; public Slider DecimSize;
    public int Refrate = 4; public InputField RefRS;
    public float BurnTime = 0.1f; public InputField BTRS;
    public float LCDBurn = 0.13f; public Slider LDBurn;
    public float StretchX = 1f; public Slider LDStX;
    public float StretchY = 1f; public Slider LDStY;
    public bool MaintainProportion; public Toggle MTP;
    public bool ShowAllDigits;
    public bool ShowBackGround; public Toggle BGT;
    public bool MakeLastD1; public Toggle MLD1;
    public Color BackColor; public Image BCSelect;
    public Color DigitColor; public Image DGCSelect;
    public int FontSet; public Dropdown StyleSelect;
    public int SpeedUnit; public Dropdown yesofcourse;
    public Slider Weight; public List<Font> LCDS;
    public float InnerShadow = 0.5f; public Slider InnerSSlide;
    float RTimel = 0f; float LastSpd = 0f;
    float SidSpeed = 0f; public Font LCDF2;
    public Button DelObj; public InputField soo;

    // Functie executata cand elementul este activat pentru prima data
    void Start() {
        SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        if (LoadMode) OnSelectOfElement();
        OnComponentLCDSModdify();
        OnChangeOfFont();
        OnColorModdify();
    }

    // Functie executata cand o culoare este selectata si schimbata in meniul de culoare
    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BackColor = new Color(PlayerPrefs.GetFloat(colorstart + "LSBACKCLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "LSBACKCLG", 0.7f), PlayerPrefs.GetFloat(colorstart + "LSBACKCLB", 0.3f));
        DigitColor = new Color(PlayerPrefs.GetFloat(colorstart + "LSDGITCLR", 0.16f), PlayerPrefs.GetFloat(colorstart + "LSDGITCLG", 0.16f), PlayerPrefs.GetFloat(colorstart + "LSDGITCLB", 0.16f));
        if (!SceneMode) if (ShowBackGround) gameObject.GetComponent<Image>().color = new Color(BackColor.r, BackColor.g, BackColor.b, 1f); 
            else gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0); if (!SceneMode) { BCSelect.color = BackColor; DGCSelect.color = DigitColor; }
        for(int o = 0; o < 4; o++) {
            for(int c = 0; c < 3; c++) {
                if (c == 2) LDigits[o].GetComponent<Text>().color = new Color(DigitColor.r, DigitColor.g, DigitColor.b, LCDBurn);
                else LDigits[o].transform.GetChild(c).GetComponent<Text>().color = DigitColor;
                if(MaintainProportion) { if(StretchX <= 0.99f || StretchY <= 0.99f) { 
                if (c == 2) LDigits[o].GetComponent<LCDStretch>().effectColor = new Color(DigitColor.r, DigitColor.g, DigitColor.b, LCDBurn);
                else LDigits[o].transform.GetChild(c).GetComponent<LCDStretch>().effectColor = DigitColor; } }
            } if (BurnTime == 0f) { LDigits[o].transform.GetChild(1).GetComponent<Text>().color = new Color(DigitColor.r, DigitColor.g, DigitColor.b, 0f); }
        } 
    }

    // Functie executata cand o culoare trebuie schimbata
    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject);
    }

    // Functie executata cand elementul este creat sau este selectat
    public void OnSelectOfElement() { if (!SceneMode) { LoadMode = true;
        // Aplicarea functiilor pe elementele de interfata (elemente Button, InputField etc.)
        DNrSel.onSubmit.RemoveAllListeners(); DNrSel.onSubmit.AddListener((_) => OnComponentLCDSModdify());
        ShowDecimal.onValueChanged.RemoveAllListeners(); ShowDecimal.onValueChanged.AddListener((_) => OnComponentLCDSModdify());
        DecimSize.onValueChanged.RemoveAllListeners(); DecimSize.onValueChanged.AddListener((_) => OnComponentLCDSModdify());
        LDBurn.onValueChanged.RemoveAllListeners(); LDBurn.onValueChanged.AddListener((_) => OnComponentLCDSModdify());
        BGT.onValueChanged.RemoveAllListeners(); BGT.onValueChanged.AddListener((_) => OnComponentLCDSModdify());
        MLD1.onValueChanged.RemoveAllListeners(); MLD1.onValueChanged.AddListener((_) => OnComponentLCDSModdify());
        InnerSSlide.onValueChanged.RemoveAllListeners(); InnerSSlide.onValueChanged.AddListener((_) => OnComponentLCDSModdify());
        MTP.onValueChanged.RemoveAllListeners(); MTP.onValueChanged.AddListener((_) => OnComponentLCDSModdify());
        LDStX.onValueChanged.RemoveAllListeners(); LDStX.onValueChanged.AddListener((_) => OnComponentLCDSModdify());
        LDStY.onValueChanged.RemoveAllListeners(); LDStY.onValueChanged.AddListener((_) => OnComponentLCDSModdify());
        yesofcourse.onValueChanged.RemoveAllListeners(); yesofcourse.onValueChanged.AddListener((_) => OnComponentLCDSModdify());
        RefRS.onSubmit.RemoveAllListeners(); RefRS.onSubmit.AddListener((_) => OnComponentLCDSModdify());
        BCSelect.GetComponent<Button>().onClick.RemoveAllListeners(); BCSelect.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("LSBACKCL"));
        DGCSelect.GetComponent<Button>().onClick.RemoveAllListeners(); DGCSelect.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("LSDGITCL"));
        DelObj.onClick.RemoveAllListeners(); DelObj.onClick.AddListener(() => DeleteObj(69));
        soo.onSubmit.RemoveAllListeners(); soo.onSubmit.AddListener((_) => ModSOrtO());
        StyleSelect.onValueChanged.RemoveAllListeners(); StyleSelect.onValueChanged.AddListener((_) => OnChangeOfFont());
        FStyleSelect.onValueChanged.RemoveAllListeners(); FStyleSelect.onValueChanged.AddListener((_) => OnComponentLCDSModdify());
        Weight.onValueChanged.RemoveAllListeners(); Weight.onValueChanged.AddListener((_) => OnChangeOfFont());
        BTRS.onSubmit.RemoveAllListeners(); BTRS.onSubmit.AddListener((_) => OnComponentLCDSModdify()); }
        // Incarcarea variabilelor salvate
        DigitsNr = (int)PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSDNSE", 2);
        DecimalSize = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSDMSZ", 0.55f);
        Refrate = (int)PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSREFR", 5);
        BurnTime = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSBRNT", 0.12f);
        LCDBurn = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSCBRN", 0.13f);
        InnerShadow = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSISHD", 0.6f);
        FontSet = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSFONT", 2);
        FontStSet = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSSFNT", 0);
        SpeedUnit = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSSPUN", 0);
        StretchX = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSSTRX", 1f);
        StretchY = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSSTRY", 1f);
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSDECI", 1) == 1) Decimal = true; else Decimal = false;
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSBACK", 1) == 1) ShowBackGround = true; else ShowBackGround = false;
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSMLD1", 0) == 1) MakeLastD1 = true; else MakeLastD1 = false;
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSMTPR", 1) == 1) MaintainProportion = true; else MaintainProportion = false;
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BackColor = new Color(PlayerPrefs.GetFloat(colorstart + "LSBACKCLR", 0.6f), PlayerPrefs.GetFloat(colorstart + "LSBACKCLG", 0.7f), PlayerPrefs.GetFloat(colorstart + "LSBACKCLB", 0.3f));
        int l = PlayerPrefs.GetInt("MAINLANG"); if (!SceneMode) { yesofcourse.options.Clear(); 
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Speed" : (l == 1) ? "Vitesse" : (l == 2) ? "Geschwindigkeit" : "Viteză")); 
            yesofcourse.options.Add(new Dropdown.OptionData("RPM")); yesofcourse.options.Add(new Dropdown.OptionData("RPM x10")); 
            yesofcourse.options.Add(new Dropdown.OptionData("RPM x100")); 
            if ((PlayerPrefs.GetInt("SPEEDSOURCE") == 2 || SpeedUnit >= 4)) { yesofcourse.options.Add(new Dropdown.OptionData("RPM x1000"));
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Fuel %" : (l == 1) ? "Carburant %" : (l == 2) ? "Kraftstoffstand" : "Combustibil %")); 
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Coolant Temp." : (l == 1) ? "Température" : (l == 2) ? "Temperatur" : "Temperatură"));
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Engine Load" : (l == 1) ? "Charge du moteur" : (l == 2) ? "Motorlast" : "Sarcina motorului"));
            yesofcourse.options.Add(new Dropdown.OptionData((l == 0) ? "Throttle" : (l == 1) ? "Étrangler" : (l == 2) ? "Gaspedal" : "Accelerație")); }
            LDStX.value = StretchX; LDStY.value = StretchY; MTP.isOn = MaintainProportion; yesofcourse.value = SpeedUnit;
            DNrSel.text = DigitsNr + ""; ShowDecimal.isOn = Decimal; DecimSize.value = DecimalSize; LDBurn.value = LCDBurn; BTRS.text = BurnTime.ToString(CultureInfo.InvariantCulture);
            BGT.isOn = ShowBackGround; BCSelect.color = BackColor; InnerSSlide.value = InnerShadow; RefRS.text = Refrate + ""; soo.text = gameObject.transform.GetSiblingIndex() + "";
            MLD1.isOn = MakeLastD1; StyleSelect.value = FontSet / 3; Weight.value = FontSet % 3; FStyleSelect.value = FontStSet;
        } StartCoroutine(yesofcoursea());
    }

    // Functie executata pentru a astepta un cadru si a actualiza interfata inainte de executarea logicii
    public IEnumerator yesofcoursea() {
        yield return null;
        LoadMode = false;
    }

    // Functie executata cand font-ul elementului este schimbat
    public void OnChangeOfFont() { if(FontStSet == 0) { 
        if (!SceneMode && !LoadMode) FontSet = (StyleSelect.value * 3) + (int)Weight.value;
        for(int o = 0; o < 4; o++) {
            for(int c = 0; c < 3; c++) {
                if (c == 2) LDigits[o].GetComponent<Text>().font = LCDS[FontSet];
                else LDigits[o].transform.GetChild(c).GetComponent<Text>().font = LCDS[FontSet];
            } if(FontSet < 12) LDigits[o].GetComponent<Text>().text = "8"; else LDigits[o].GetComponent<Text>().text = "~";
        } PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSFONT", FontSet); }
    }

    // Functie executata cand ordinea obiectului este schimbata in scena
    public void ModSOrtO() {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "SO", int.Parse(soo.text));
        gameObject.GetComponent<SortOrderOrg>().Apply();
    }

    // Functie executata cand o proprietate este schimbata despre element
    public void OnComponentLCDSModdify() {
        if (!SceneMode && !LoadMode) { StretchX = LDStX.value; StretchY = LDStY.value; MaintainProportion = MTP.isOn; SpeedUnit = yesofcourse.value;
            DigitsNr = Mathf.Clamp(int.Parse(DNrSel.text), 1, 3); Decimal = ShowDecimal.isOn; DecimalSize = DecimSize.value;
            Refrate = int.Parse(RefRS.text); BurnTime = float.Parse(BTRS.text, CultureInfo.InvariantCulture); LCDBurn = LDBurn.value;
            MakeLastD1 = MLD1.isOn; ShowBackGround = BGT.isOn; InnerShadow = InnerSSlide.value; FontStSet = FStyleSelect.value;
        } if (DigitsNr == 1) { LDigits[0].SetActive(true); LDigits[1].SetActive(false); LDigits[2].SetActive(false); }
        if (DigitsNr == 2) { LDigits[0].SetActive(true); LDigits[1].SetActive(true); LDigits[2].SetActive(false); }
        if (DigitsNr == 3) { LDigits[0].SetActive(true); LDigits[1].SetActive(true); LDigits[2].SetActive(true); }
        if (Decimal) { LDigits[3].SetActive(true);
            LDigits[3].transform.localScale = new Vector2(DecimalSize, DecimalSize); } else LDigits[3].SetActive(false);
        for (int d = 0; d < 4; d++) LDigits[d].GetComponent<Text>().color = new Color(LDigits[d].GetComponent<Text>().color.r, LDigits[d].GetComponent<Text>().color.g, LDigits[d].GetComponent<Text>().color.b, LCDBurn);
        if (ShowBackGround) { gameObject.GetComponent<Image>().color = new Color(BackColor.r, BackColor.g, BackColor.b, 1f);
            gameObject.transform.GetChild(0).GetComponent<RawImage>().color = new Color(0, 0, 0, InnerShadow); }
        else { gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0); gameObject.transform.GetChild(0).GetComponent<RawImage>().color = new Color(0, 0, 0, 0); }
        if(FontStSet == 1) { for (int d = 0; d < 4; d++) { LDigits[d].GetComponent<Text>().text = "#";
                if(d == 3) LDigits[d].transform.localScale = new Vector2(DecimalSize * 1.38f, DecimalSize * 1.38f);
                else LDigits[d].transform.localScale = new Vector2(1.38f, 1.38f);
                for(int o = 0; o < 4; o++) { for(int c = 0; c < 3; c++) {
                if (c == 2) LDigits[o].GetComponent<Text>().font = LCDF2;
                else LDigits[o].transform.GetChild(c).GetComponent<Text>().font = LCDF2; } }
            } if(!SceneMode) StyleSelect.interactable = false;
            LDigits[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-40, -5);
            LDigits[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(195, -5);
            LDigits[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(430, -5);
            LDigits[3].GetComponent<RectTransform>().pivot = new Vector2(0.78f, 0.21f);
            LDigits[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(-32, 32.44f);
            if (DigitsNr == 1) { gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(255f, 350f); }
            if (DigitsNr == 2) { gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(490f, 350f); }
            if (DigitsNr == 3) { gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(725f, 350f); }
            if (Decimal) gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(35f + (205f * DecimalSize), 0f);
        } else { for (int d = 0; d < 4; d++) { if (FontSet < 12) LDigits[d].GetComponent<Text>().text = "8"; else LDigits[d].GetComponent<Text>().text = "~";
        if (d != 3) LDigits[d].transform.localScale = new Vector2(1f, 1f);
        } if(!SceneMode) StyleSelect.interactable = true;
            LDigits[3].GetComponent<RectTransform>().pivot = new Vector2(1f, 0.08f);
            LDigits[3].GetComponent<RectTransform>().anchoredPosition = new Vector2(-16, 25.92f);
            if (DigitsNr == 1) { gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(255f, 350f); }
            if (DigitsNr == 2) { gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(470f, 350f); }
            if (DigitsNr == 3) { gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(685f, 350f); }
            if (Decimal) gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(15f + (215f * DecimalSize), 0f);
            if(MakeLastD1) { gameObject.GetComponent<RectTransform>().sizeDelta -= new Vector2(140f, 0f); LDigits[0].GetComponent<Text>().text = "1"; if(!SceneMode) LDigits[0].transform.GetChild(0).GetComponent<Text>().text = "1";
            if (!SceneMode) LDigits[0].transform.GetChild(1).GetComponent<Text>().text = "1"; LDigits[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(-135f, -5f);
            LDigits[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(80f, -5f); LDigits[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(295f, -5f); }
            else { LDigits[0].GetComponent<RectTransform>().anchoredPosition = new Vector2(5f, -5f); LDigits[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(220f, -5f);
            LDigits[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(435f, -5f); LDigits[0].GetComponent<Text>().text = "8";
            if (!SceneMode) LDigits[0].transform.GetChild(0).GetComponent<Text>().text = "0"; if (!SceneMode) LDigits[0].transform.GetChild(1).GetComponent<Text>().text = "0"; }
            if (LDigits[0].GetComponent<Text>().font == LCDF2) OnChangeOfFont(); }
        for (int d = 0; d < 4; d++) { float Mlt = 1f; if (FontStSet == 1) Mlt = 1.38f;
            Text Burn = LDigits[d].GetComponent<Text>(); Text ActialText = LDigits[d].transform.GetChild(0).GetComponent<Text>();
            Text ActialTextBurn = LDigits[d].transform.GetChild(1).GetComponent<Text>(); float Bias = 1f; if (d == 3) Bias = DecimalSize;
            if (StretchX <= 0.99f || StretchY <= 0.99f) { Burn.transform.localScale = new Vector2(StretchX * Bias * Mlt, StretchY * Bias * Mlt);
                if (MaintainProportion) { if (StretchX <= 0.99f && StretchY <= 0.99f) Burn.color = new Color(Burn.color.r, Burn.color.g, Burn.color.b, Burn.color.a / 6f);
                    else Burn.color = new Color(Burn.color.r, Burn.color.g, Burn.color.b, Burn.color.a / 3.5f); Burn.GetComponent<LCDStretch>().effectColor = DigitColor; ActialText.GetComponent<LCDStretch>().effectColor = DigitColor;
                    ActialTextBurn.GetComponent<LCDStretch>().effectColor = DigitColor; Burn.GetComponent<LCDStretch>().enabled = true;
                    ActialText.GetComponent<LCDStretch>().enabled = true; ActialTextBurn.GetComponent<LCDStretch>().enabled = true;
                } else { Burn.GetComponent<LCDStretch>().enabled = false; ActialText.GetComponent<LCDStretch>().enabled = false;
                    ActialTextBurn.GetComponent<LCDStretch>().enabled = false; }
                if (d != 3) { Burn.GetComponent<RectTransform>().anchoredPosition *= new Vector2(0.1304f + (StretchX / 1.15f), 1f); Burn.GetComponent<RectTransform>().anchoredPosition += new Vector2((1f - StretchX) * 22f, 0f); }
            } else
            { Burn.GetComponent<LCDStretch>().enabled = false; ActialText.GetComponent<LCDStretch>().enabled = false;
                ActialTextBurn.GetComponent<LCDStretch>().enabled = false; Burn.transform.localScale = new Vector2(Bias * Mlt, Bias * Mlt); }
        } if (StretchX <= 0.99f || StretchY <= 0.99f) {
            gameObject.GetComponent<RectTransform>().sizeDelta -= new Vector2(45f * StretchX, 40f * StretchY);
            gameObject.GetComponent<RectTransform>().sizeDelta *= new Vector2(0.1304f + (StretchX / 1.15f), StretchY);
            gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2(45f * StretchX, 40f * StretchY);
        } if (!SceneMode && !LoadMode) {
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSDNSE", DigitsNr);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSDMSZ", DecimalSize);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSREFR", Refrate);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSBRNT", BurnTime);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSCBRN", LCDBurn);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSISHD", InnerShadow);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSSTRX", StretchX);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSSTRY", StretchY);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSSFNT", FontStSet);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSSPUN", SpeedUnit);
            if (Decimal) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSDECI", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSDECI", 0);
            if (ShowBackGround) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSBACK", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSBACK", 0);
            if (MakeLastD1) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSMLD1", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSMLD1", 0);
            if (MaintainProportion) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSMTPR", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LSMTPR", 0);
        }
    }

    // Functie executata cand obiectul este sters
    public void DeleteObj(int wf) {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TYPE", 69696);
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ") + 1);
        Sdrg.OnElemSelect(72769); Destroy(gameObject);
    }

    // Functie chemata in fiecare cadru | Functie folosita pentru a actualiza valoarea kilometrajului
    void Update() {
        if(FontStSet == 0) if (MakeLastD1) LDigits[0].GetComponent<Text>().text = "1";
        if (SceneMode) {
            RTimel += Time.deltaTime;
            if (RTimel >= (1f / Refrate)) {
                LastSpd = SidSpeed;
                if (SpeedUnit == 0) SidSpeed = SM.InterpSpeed;
                if (SpeedUnit == 1) SidSpeed = SM.RPM;
                if (SpeedUnit == 2) SidSpeed = SM.RPM / 10f;
                if (SpeedUnit == 3) SidSpeed = SM.RPM / 100f;
                if (SpeedUnit == 4) SidSpeed = SM.RPM / 1000f;
                if (SpeedUnit == 5) SidSpeed = SM.OBDFuel;
                if (SpeedUnit == 6) SidSpeed = SM.OBDCoolant;
                if (SpeedUnit == 7) SidSpeed = SM.OBDLoad;
                if (SpeedUnit == 8) SidSpeed = SM.OBDThrottle;
                if(MakeLastD1) if(SidSpeed > 2 * Mathf.Pow(10, DigitsNr - 1)) { SidSpeed = Mathf.Pow(10, DigitsNr - 1) + (SidSpeed % Mathf.Pow(10, DigitsNr - 1)); }
                RTimel -= (1f / Refrate);
            } if(DigitsNr == 1) {
                LDigits[0].transform.GetChild(0).GetComponent<Text>().text = ((int)SidSpeed % 10).ToString();
                LDigits[0].transform.GetChild(1).GetComponent<Text>().text = ((int)LastSpd % 10).ToString();
                if (BurnTime > 0f) LDigits[0].transform.GetChild(1).GetComponent<Text>().color = new Color(LDigits[0].transform.GetChild(1).GetComponent<Text>().color.r, LDigits[0].transform.GetChild(1).GetComponent<Text>().color.g,
                LDigits[0].transform.GetChild(1).GetComponent<Text>().color.b, (BurnTime - RTimel) * (1f / BurnTime));
            } else if (DigitsNr == 2) {
                LDigits[0].transform.GetChild(0).GetComponent<Text>().text = ((int)(SidSpeed / 10) % 10).ToString();
                LDigits[0].transform.GetChild(1).GetComponent<Text>().text = ((int)(LastSpd / 10) % 10).ToString();
                if (BurnTime > 0f) LDigits[0].transform.GetChild(1).GetComponent<Text>().color = new Color(LDigits[0].transform.GetChild(1).GetComponent<Text>().color.r, LDigits[0].transform.GetChild(1).GetComponent<Text>().color.g,
                LDigits[0].transform.GetChild(1).GetComponent<Text>().color.b, (BurnTime - RTimel) * (1f / BurnTime));
                LDigits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)SidSpeed % 10).ToString();
                LDigits[1].transform.GetChild(1).GetComponent<Text>().text = ((int)LastSpd % 10).ToString();
                if (BurnTime > 0f) LDigits[1].transform.GetChild(1).GetComponent<Text>().color = new Color(LDigits[1].transform.GetChild(1).GetComponent<Text>().color.r, LDigits[1].transform.GetChild(1).GetComponent<Text>().color.g,
                LDigits[1].transform.GetChild(1).GetComponent<Text>().color.b, (BurnTime - RTimel) * (1f / BurnTime));
                if (LDigits[0].transform.GetChild(1).GetComponent<Text>().text == "0") LDigits[0].transform.GetChild(1).GetComponent<Text>().text = "";
                if (((int)(SidSpeed / 10) % 10) == 0 && !ShowAllDigits) { LDigits[0].transform.GetChild(0).gameObject.SetActive(false);
                    LDigits[0].transform.GetChild(1).gameObject.SetActive(false); } else { LDigits[0].transform.GetChild(0).gameObject.SetActive(true);
                    LDigits[0].transform.GetChild(1).gameObject.SetActive(true); }
            } else if (DigitsNr == 3) {
                LDigits[0].transform.GetChild(0).GetComponent<Text>().text = ((int)(SidSpeed / 100) % 10).ToString();
                LDigits[0].transform.GetChild(1).GetComponent<Text>().text = ((int)(LastSpd / 100) % 10).ToString();
                if (BurnTime > 0f) LDigits[0].transform.GetChild(1).GetComponent<Text>().color = new Color(LDigits[0].transform.GetChild(1).GetComponent<Text>().color.r, LDigits[0].transform.GetChild(1).GetComponent<Text>().color.g,
                LDigits[0].transform.GetChild(1).GetComponent<Text>().color.b, (BurnTime - RTimel) * (1f / BurnTime));
                LDigits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)(SidSpeed / 10) % 10).ToString();
                LDigits[1].transform.GetChild(1).GetComponent<Text>().text = ((int)(LastSpd / 10) % 10).ToString();
                if (BurnTime > 0f) LDigits[1].transform.GetChild(1).GetComponent<Text>().color = new Color(LDigits[1].transform.GetChild(1).GetComponent<Text>().color.r, LDigits[1].transform.GetChild(1).GetComponent<Text>().color.g,
                LDigits[1].transform.GetChild(1).GetComponent<Text>().color.b, (BurnTime - RTimel) * (1f / BurnTime));
                LDigits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)SidSpeed % 10).ToString();
                LDigits[2].transform.GetChild(1).GetComponent<Text>().text = ((int)LastSpd % 10).ToString();
                if (BurnTime > 0f) LDigits[2].transform.GetChild(1).GetComponent<Text>().color = new Color(LDigits[2].transform.GetChild(1).GetComponent<Text>().color.r, LDigits[2].transform.GetChild(1).GetComponent<Text>().color.g,
                LDigits[2].transform.GetChild(1).GetComponent<Text>().color.b, (BurnTime - RTimel) * (1f / BurnTime));
                if (LDigits[0].transform.GetChild(1).GetComponent<Text>().text == "0") LDigits[0].transform.GetChild(1).GetComponent<Text>().text = "";
                if (LDigits[1].transform.GetChild(1).GetComponent<Text>().text == "0") LDigits[1].transform.GetChild(1).GetComponent<Text>().text = "";
                if (((int)(SidSpeed / 100) % 10) == 0 && !ShowAllDigits) { LDigits[0].transform.GetChild(0).gameObject.SetActive(false);
                    LDigits[0].transform.GetChild(1).gameObject.SetActive(false); } else { LDigits[0].transform.GetChild(0).gameObject.SetActive(true);
                    LDigits[0].transform.GetChild(1).gameObject.SetActive(true); }
                if (((int)(SidSpeed / 100) % 10) == 0 && ((int)(SidSpeed / 10) % 10) == 0 && !ShowAllDigits) { LDigits[1].transform.GetChild(0).gameObject.SetActive(false);
                    LDigits[1].transform.GetChild(1).gameObject.SetActive(false); } else { LDigits[1].transform.GetChild(0).gameObject.SetActive(true);
                    LDigits[1].transform.GetChild(1).gameObject.SetActive(true); }
            } if (Decimal) {
                LDigits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)(SidSpeed * 10) % 10).ToString();
                LDigits[3].transform.GetChild(1).GetComponent<Text>().text = ((int)(LastSpd * 10) % 10).ToString();
                if (BurnTime > 0f) LDigits[3].transform.GetChild(1).GetComponent<Text>().color = new Color(LDigits[0].transform.GetChild(1).GetComponent<Text>().color.r, LDigits[3].transform.GetChild(1).GetComponent<Text>().color.g,
                LDigits[3].transform.GetChild(1).GetComponent<Text>().color.b, (BurnTime - RTimel) * (1f / BurnTime));
            } 
        }
    }
}
