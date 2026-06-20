using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalogDisplay : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Display Analogic
    // Folosit pentru elementul Display Analogic
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public ColorPicker CoPi; // Clasa universala pentru selectarea culorii
    public SelectDrag Sdrg; // Clasa universala pentru aranjarea, crearea si stergerea elementelor
    public SPEEDMANAGER SM; // Clasa universala pentru variabile de viteza, turatie, etc.
    public bool SceneMode; // Variabila folosita pentru a determina daca elementul este editad sau utilizat
    public int ElementID; // Vaiabila numerica autonumerotata pentru identificarea elementului
    public int Mode;
    public bool LoadMode;
    public GameObject EditorModPanel;
    public List<GameObject> Digits;

    // Lista de elenete si proprietati pentru clasa
    public int DigitsNr = 1; public InputField DNrSel;
    public bool Decimal = true; public Toggle ShowDecimal;
    public bool Animated; public Toggle Animted;
    public int Refrate = 4; public InputField RefRS;
    public Color DgtCl; public Image DgtImg;
    public Color Dgt2Cl; public Image Dgt2Img;
    public Color DgtTxCl; public Image DgtTxImg;
    public Color Dgt2TxCl; public Image Dgt2TxImg;
    float RTimel = 0f; public Dropdown ModeSel;
    public int fonts; public Dropdown Fontttss;
    public List<Font> actfonts; public List<Sprite> comsp;
    public InputField soo; public Button KJURedButton;

    // Functie executata cand elementul este activat pentru prima data
    void Start() {
        SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        if (LoadMode) OnSelectOfElement();
        OnComponentAnDiModdify();
        OnColorModdify();
    }

    // Functie executata cand elementul este creat sau este selectat
    public void OnSelectOfElement() { if (!SceneMode) { LoadMode = true;
        // Aplicarea functiilor pe elementele de interfata (elemente Button, InputField etc.)
        DNrSel.onSubmit.RemoveAllListeners(); DNrSel.onSubmit.AddListener((_) => OnComponentAnDiModdify());
        ShowDecimal.onValueChanged.RemoveAllListeners(); ShowDecimal.onValueChanged.AddListener((_) => OnComponentAnDiModdify());
        Animted.onValueChanged.RemoveAllListeners(); Animted.onValueChanged.AddListener((_) => OnComponentAnDiModdify());
        RefRS.onSubmit.RemoveAllListeners(); RefRS.onSubmit.AddListener((_) => OnComponentAnDiModdify());
        ModeSel.onValueChanged.RemoveAllListeners(); ModeSel.onValueChanged.AddListener((_) => OnComponentAnDiModdify());
        soo.onSubmit.RemoveAllListeners(); soo.onSubmit.AddListener((_) => ModSOrtO());
        Fontttss.onValueChanged.RemoveAllListeners(); Fontttss.onValueChanged.AddListener((_) => OnComponentAnDiModdify());
        DgtImg.GetComponent<Button>().onClick.RemoveAllListeners(); DgtImg.GetComponent<Button>().onClick.AddListener(() => { TriggerColorChange("ADDGCL"); });
        Dgt2Img.GetComponent<Button>().onClick.RemoveAllListeners(); Dgt2Img.GetComponent<Button>().onClick.AddListener(() => { TriggerColorChange("ADDG2CL"); });
        DgtTxImg.GetComponent<Button>().onClick.RemoveAllListeners(); DgtTxImg.GetComponent<Button>().onClick.AddListener(() => { TriggerColorChange("ADDGTXCL"); });
        Dgt2TxImg.GetComponent<Button>().onClick.RemoveAllListeners(); Dgt2TxImg.GetComponent<Button>().onClick.AddListener(() => { TriggerColorChange("ADDG2TXCL"); });
        KJURedButton.onClick.RemoveAllListeners(); KJURedButton.onClick.AddListener(() => DeleteObj(69)); }
        // Incarcarea variabilelor salvate
        DigitsNr = (int)PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADDNSE", 2);
        Refrate = (int)PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADREFR", 30);
        Mode = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADMSDD", 2);
        fonts = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADFONT", 1);
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADSDTG", 1) == 1) Decimal = true; else Decimal = false;
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADANIM", 1) == 1) Animated = true; else Animated = false;
        int l = PlayerPrefs.GetInt("MAINLANG"); if (!SceneMode) { ModeSel.options.Clear();
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Speed" : (l == 1) ? "Vitesse" : (l == 2) ? "Geschwindigkeit" : "Viteză")); 
            ModeSel.options.Add(new Dropdown.OptionData("RPM"));
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Odometer" : (l == 1) ? "Odomètre" : (l == 2) ? "Kilometerzähler" : "Odometru"));
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Clock" : (l == 1) ? "Horloge" : (l == 2) ? "Uhr" : "Ceas"));
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Total Time (h:mm)" : (l == 1) ? "Durée totale (h:mm)" : (l == 2) ? "Gesamtzeit (h:mm)" : "Timp total (h:mm)"));
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Total Time (h)" : (l == 1) ? "Durée totale (h)" : (l == 2) ? "Gesamtzeit (h)" : "Timp Total (h)"));
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Trip Dist." : (l == 1) ? "Distance du trajet" : (l == 2) ? "Reiseentfernung" : "Distanță plimbare"));
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Trip Time (m:ss)" : (l == 1) ? "Temps de trajet (m:ss)" : (l == 2) ? "Reisezeit (m:ss)" : "Timp plimbare (m:ss)"));
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Trip Time (h:mm)" : (l == 1) ? "Temps de trajet (h:mm)" : (l == 2) ? "Reisezeit (h:mm)" : "Timp plimbare (h:mm)"));
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Trip Time (h)" : (l == 1) ? "Temps de trajet (h)" : (l == 2) ? "Reisezeit (h)" : "Timp plimbare (h)"));
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Max Speed" : (l == 1) ? "Vitesse maximale" : (l == 2) ? "Max. Geschwindigkeit" : "Viteza maxima"));
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Avg. Speed" : (l == 1) ? "Vitesse moyenne" : (l == 2) ? "Durchschnittsgeschwindigkeit" : "Viteza medie"));
            if ((PlayerPrefs.GetInt("SPEEDSOURCE") == 2 || Mode >= 12)) { 
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Engine Load" : (l == 1) ? "Charge du moteur" : (l == 2) ? "Motorlast" : "Sarcina motorului"));
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Coolant Temp." : (l == 1) ? "Température du moteur" : (l == 2) ? "Motortemperatur" : "Temperatura motorului"));
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Throttle" : (l == 1) ? "Étrangler" : (l == 2) ? "Gaspedal" : "Accelerație"));
            ModeSel.options.Add(new Dropdown.OptionData((l == 0) ? "Fuel (%)" : (l == 1) ? "Carburant (%)" : (l == 2) ? "Kraftstoff (%)" : "Combustibil (%)")); }
            ModeSel.GetComponent<DropdownControl>().scaleCoef = (l == 0) ? 1 : (l == 1) ? 0.9f : (l == 2) ? 0.7f : 0.9f;
            ModeSel.GetComponent<DropdownControl>().UpdateScale();
            DNrSel.text = DigitsNr + ""; RefRS.text = Refrate + ""; soo.text = gameObject.transform.GetSiblingIndex() + "";
            ModeSel.value = Mode; ShowDecimal.isOn = Decimal; Fontttss.value = fonts; Animted.isOn = Animated; } StartCoroutine(yesofcourse());
    }

    // Functie executata pentru a astepta un cadru si a actualiza interfata inainte de executarea logicii
    public IEnumerator yesofcourse() {
        yield return null;
        LoadMode = false;
    }

    // Functie executata cand o culoare trebuie schimbata
    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID; // Seteaza valoarea culorii ce va fi schimbata
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject); // Schimba culoarea selectata
    }

    // Functie executata cand o culoare este selectata si schimbata in meniul de culoare
    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        DgtCl = new Color(PlayerPrefs.GetFloat(colorstart + "ADDGCLR", 0f), PlayerPrefs.GetFloat(colorstart + "ADDGCLG", 0f), PlayerPrefs.GetFloat(colorstart + "ADDGCLB", 0f));
        Dgt2Cl = new Color(PlayerPrefs.GetFloat(colorstart + "ADDG2CLR", 1f), PlayerPrefs.GetFloat(colorstart + "ADDG2CLG", 1f), PlayerPrefs.GetFloat(colorstart + "ADDG2CLB", 1f));
        DgtTxCl = new Color(PlayerPrefs.GetFloat(colorstart + "ADDGTXCLR", 1f), PlayerPrefs.GetFloat(colorstart + "ADDGTXCLG", 1f), PlayerPrefs.GetFloat(colorstart + "ADDGTXCLB", 1f));
        Dgt2TxCl = new Color(PlayerPrefs.GetFloat(colorstart + "ADDG2TXCLR", 0f), PlayerPrefs.GetFloat(colorstart + "ADDG2TXCLG", 0f), PlayerPrefs.GetFloat(colorstart + "ADDG2TXCLB", 0f));
        if (DgtCl == Color.black) for (int f = 1; f < 10; f++) { Digits[f].GetComponent<Image>().sprite = comsp[0]; Digits[f].GetComponent<Image>().color = Color.white; }
        else for (int f = 1; f < 10; f++) { Digits[f].GetComponent<Image>().sprite = comsp[1]; Digits[f].GetComponent<Image>().color = DgtCl; }
        if (Dgt2Cl == Color.black) { Digits[0].GetComponent<Image>().sprite = comsp[0]; Digits[0].GetComponent<Image>().color = Color.white; }
        else { Digits[0].GetComponent<Image>().sprite = comsp[1]; Digits[0].GetComponent<Image>().color = Dgt2Cl; }
        for (int s = 1; s < 10; s++) { Digits[s].transform.GetChild(0).GetComponent<Text>().color = DgtTxCl; Digits[s].transform.GetChild(1).GetComponent<Text>().color = DgtTxCl; }
        Digits[0].transform.GetChild(0).GetComponent<Text>().color = Dgt2TxCl; Digits[0].transform.GetChild(1).GetComponent<Text>().color = Dgt2TxCl;
        if (!SceneMode) { DgtImg.color = DgtCl; Dgt2Img.color = Dgt2Cl; DgtTxImg.color = DgtTxCl; Dgt2TxImg.color = Dgt2TxCl; }
    }

    // Functie executata cand ordinea obiectului este schimbata in scena
    public void ModSOrtO() {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "SO", int.Parse(soo.text));
        gameObject.GetComponent<SortOrderOrg>().Apply();
    }

    // Functie executata cand o proprietate este schimbata despre element
    public void OnComponentAnDiModdify() {
        if (!SceneMode && !LoadMode) {
            DigitsNr = int.Parse(DNrSel.text); Decimal = ShowDecimal.isOn; Animated = Animted.isOn;
            Refrate = int.Parse(RefRS.text); Mode = ModeSel.value; fonts = Fontttss.value; } 
        if (Mode == 3 || Mode == 4 || Mode == 7 || Mode == 8) Decimal = false;
        if (Mode == 3) DigitsNr = 4;
        if (Mode == 4 || Mode == 7 || Mode == 8) DigitsNr = Mathf.Max(DigitsNr, 3);
        if (!SceneMode) if (Mode == 5 || Mode == 9) { Decimal = true; Digits[0].transform.GetChild(0).GetComponent<Text>().text = "h"; }
            else Digits[0].transform.GetChild(0).GetComponent<Text>().text = "0";
        for (int g = 0; g < 9; g++) { 
            if (DigitsNr > g) Digits[g + 1].SetActive(true); else Digits[g + 1].SetActive(false);
            if (Decimal) { Digits[g + 1].GetComponent<RectTransform>().anchoredPosition = new Vector2(-225f - (150f * g), 0f);
                Digits[0].GetComponent<RectTransform>().sizeDelta = new Vector2(150f * (DigitsNr + 1), 200f); }
            else { Digits[g + 1].GetComponent<RectTransform>().anchoredPosition = new Vector2(-75f - (150f * g), 0f);
                Digits[0].GetComponent<RectTransform>().sizeDelta = new Vector2(150f * DigitsNr, 200f); }
        } if (Mode == 3 || Mode == 4 || Mode == 7 || Mode == 8) { Digits[0].transform.GetChild(11).gameObject.SetActive(true);
            Digits[0].GetComponent<RectTransform>().sizeDelta = new Vector2(50f + (150f * DigitsNr), 200f);
            Digits[1].GetComponent<RectTransform>().anchoredPosition = new Vector2(-75f, 0f); Digits[1].SetActive(true);
            Digits[2].GetComponent<RectTransform>().anchoredPosition = new Vector2(-225f, 0f); Digits[2].SetActive(true);
            for (int d = 3; d < 10; d++) {
                if (DigitsNr >= d) { Digits[d].GetComponent<RectTransform>().anchoredPosition = new Vector2(-425f - ((d - 3) * 150f), 0f); Digits[d].SetActive(true); }
                else { Digits[d].SetActive(false); } }
        } else Digits[0].transform.GetChild(11).gameObject.SetActive(false);
        for(int d = 0; d < 10; d++) {
            Digits[d].transform.GetChild(0).GetComponent<Text>().font = actfonts[fonts];
            Digits[d].transform.GetChild(1).GetComponent<Text>().font = actfonts[fonts];
        } if (!SceneMode && !LoadMode) { // Salvarea variabilelor
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADDNSE", DigitsNr);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADREFR", Refrate);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADMSDD", Mode);
            if (Decimal) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADSDTG", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADSDTG", 0);
            if (Animated) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADANIM", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADANIM", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ADFONT", fonts);
        }
    }

    // Functie executata cand obiectul este sters
    public void DeleteObj(int wf) {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TYPE", 69696);
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ") + 1);
        Sdrg.OnElemSelect(72769); Destroy(gameObject);
    }

    // Functie chemata in fiecare cadru | Functie folosita pentru a actualiza cifrele elementului cand este folosit
    void Update() {
        if (SceneMode) {
            RTimel += Time.deltaTime;
            if (RTimel >= (1f / Refrate)) {
                if (Mode == 0) { // Speed
                    Digits[0].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.InterpSpeed * 10f) % 10).ToString();
                    Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)SM.InterpSpeed % 10).ToString();
                    Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.InterpSpeed % 100) / 10).ToString();
                    Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.InterpSpeed % 1000) / 100).ToString();
                    Digits[4].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.InterpSpeed % 10000) / 1000).ToString();
                    if (Animated) { float DigProgression = ((float)(SM.InterpSpeed * 100f) % 10f) / 10f;
                        Digits[0].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Max(1f - (DigProgression / 0.8f),0f));
                        Digits[0].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression - 0.2f) / 0.8f,0f));
                        float DigProgression2 = -9f + ((float)(SM.InterpSpeed * 10f) % 10f);
                        Digits[1].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression2 / 0.8f), 0f, 1f));
                        Digits[1].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                        float DigProgression3 = -99f + (((float)SM.InterpSpeed * 10f) % 100f);
                        Digits[2].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression3 / 0.8f), 0f, 1f));
                        Digits[2].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                        float DigProgression4 = -999f + (((float)SM.InterpSpeed * 10f) % 1000f);
                        Digits[3].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression4 / 0.8f), 0f, 1f));
                        Digits[3].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                        float DigProgression5 = -9999f + (((float)SM.InterpSpeed * 10f) % 10000f);
                        Digits[4].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression5 / 0.8f), 0f, 1f));
                        Digits[4].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression5 - 0.2f) / 0.8f, 0f));
                        for (int s = 0; s < 5; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10) + "";
                    }
                } else if (Mode == 1) { // RPM
                    Digits[0].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.RPM * 10f) % 10).ToString();
                    Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)SM.RPM % 10).ToString();
                    Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.RPM % 100) / 10).ToString();
                    Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.RPM % 1000) / 100).ToString();
                    Digits[4].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.RPM % 10000) / 1000).ToString();
                    if (Animated) {
                        float DigProgression = ((SM.RPM * 100f) % 10f) / 10f;
                        Digits[0].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Max(1f - (DigProgression / 0.8f), 0f));
                        Digits[0].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression - 0.2f) / 0.8f, 0f));
                        float DigProgression2 = -9f + ((SM.RPM * 10f) % 10f);
                        Digits[1].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression2 / 0.8f), 0f, 1f));
                        Digits[1].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                        float DigProgression3 = -99f + ((SM.RPM * 10f) % 100f);
                        Digits[2].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression3 / 0.8f), 0f, 1f));
                        Digits[2].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                        float DigProgression4 = -999f + ((SM.RPM * 10f) % 1000f);
                        Digits[3].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression4 / 0.8f), 0f, 1f));
                        Digits[3].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                        float DigProgression5 = -9999f + ((SM.RPM * 10f) % 10000f);
                        Digits[3].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression5 / 0.8f), 0f, 1f));
                        Digits[3].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression5 - 0.2f) / 0.8f, 0f));
                        for (int s = 0; s < 5; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10) + "";
                    }
                } else if (Mode == 2) { // Distance / Odometer
                    Digits[0].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.DistanceMade * 10f) % 10).ToString();
                    Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)SM.DistanceMade % 10).ToString();
                    Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.DistanceMade % 100) / 10).ToString();
                    Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.DistanceMade % 1000) / 100).ToString();
                    Digits[4].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.DistanceMade % 10000) / 1000).ToString();
                    Digits[5].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.DistanceMade % 100000) / 10000).ToString();
                    Digits[6].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.DistanceMade % 1000000) / 100000).ToString();
                    Digits[7].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.DistanceMade % 10000000) / 1000000).ToString();
                    Digits[8].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.DistanceMade % 100000000) / 10000000).ToString();
                    Digits[9].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.DistanceMade % 1000000000) / 100000000).ToString();
                    if (Animated) { float DigProgression = (float)(((SM.DistanceMade * 100) % 10) / 10);
                        Digits[0].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Max(1f - (DigProgression / 0.8f),0f));
                        Digits[0].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression - 0.2f) / 0.8f,0f));
                        float DigProgression2 = (float)(-9 + ((SM.DistanceMade * 10) % 10));
                        Digits[1].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression2 / 0.8f), 0f, 1f));
                        Digits[1].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                        float DigProgression3 = (float)(-99 + ((SM.DistanceMade * 10) % 100));
                        Digits[2].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression3 / 0.8f), 0f, 1f));
                        Digits[2].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                        float DigProgression4 = (float)(-999 + ((SM.DistanceMade * 10) % 1000));
                        Digits[3].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression4 / 0.8f), 0f, 1f));
                        Digits[3].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                        float DigProgression5 = (float)(-9999 + ((SM.DistanceMade * 10) % 10000));
                        Digits[4].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression5 / 0.8f), 0f, 1f));
                        Digits[4].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression5 - 0.2f) / 0.8f, 0f));
                        float DigProgression6 = (float)(-99999 + ((SM.DistanceMade * 10) % 100000));
                        Digits[5].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression6 / 0.8f), 0f, 1f));
                        Digits[5].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression6 - 0.2f) / 0.8f, 0f));
                        float DigProgression7 = (float)(-999999 + ((SM.DistanceMade * 10) % 1000000));
                        Digits[6].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression7 / 0.8f), 0f, 1f));
                        Digits[6].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression7 - 0.2f) / 0.8f, 0f));
                        float DigProgression8 = (float)(-9999999 + ((SM.DistanceMade * 10) % 10000000));
                        Digits[7].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression8 / 0.8f), 0f, 1f));
                        Digits[7].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression8 - 0.2f) / 0.8f, 0f));
                        float DigProgression9 = (float)(-99999999 + ((SM.DistanceMade * 10) % 100000000));
                        Digits[8].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression9 / 0.8f), 0f, 1f));
                        Digits[8].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression9 - 0.2f) / 0.8f, 0f));
                        float DigProgression10 = (float)(-999999999 + ((SM.DistanceMade * 10) % 1000000000));
                        Digits[9].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression10 / 0.8f), 0f, 1f));
                        Digits[9].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression10 - 0.2f) / 0.8f, 0f));
                        for(int s = 0; s < 10; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10) + "";
                    }
                } else if (Mode == 3) { // Clock
                    Digits[1].transform.GetChild(0).GetComponent<Text>().text = DateTime.Now.Minute.ToString("D2").Substring(1, 1);
                    Digits[2].transform.GetChild(0).GetComponent<Text>().text = DateTime.Now.Minute.ToString("D2").Substring(0, 1);
                    Digits[3].transform.GetChild(0).GetComponent<Text>().text = DateTime.Now.Hour.ToString("D2").Substring(1, 1);
                    Digits[4].transform.GetChild(0).GetComponent<Text>().text = DateTime.Now.Hour.ToString("D2").Substring(0, 1);
                } else if (Mode == 4) { // Total Time (h:mm)
                    Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 60) % 10).ToString();
                    Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 600) % 6).ToString();
                    Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 3600) % 10).ToString();
                    Digits[4].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 36000) % 10).ToString();
                    Digits[5].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 360000) % 10).ToString();
                    Digits[6].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 3600000) % 10).ToString();
                    Digits[7].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 36000000) % 10).ToString();
                    Digits[8].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 360000000) % 10).ToString();
                    Digits[9].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 3600000000) % 10).ToString();
                    if (Animated) { float DigProgression2 = (float)(-9 + ((SM.TimeMade / 6) % 10));
                        Digits[1].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression2 / 0.8f)));
                        Digits[1].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                        float DigProgression3 = (float)(-99 + ((SM.TimeMade / 6) % 100));
                        Digits[2].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression3 / 0.8f)));
                        Digits[2].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                        float DigProgression4 = (float)(-599 + ((SM.TimeMade / 6) % 600));
                        Digits[3].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression4 / 0.8f)));
                        Digits[3].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                        float DigProgression5 = (float)(-5999 + ((SM.TimeMade / 6) % 6000));
                        Digits[4].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression5 / 0.8f)));
                        Digits[4].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression5 - 0.2f) / 0.8f, 0f));
                        float DigProgression6 = (float)(-59999 + ((SM.TimeMade / 6) % 60000));
                        Digits[5].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression6 / 0.8f)));
                        Digits[5].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression6 - 0.2f) / 0.8f, 0f));
                        float DigProgression7 = (float)(-599999 + ((SM.TimeMade / 6) % 600000));
                        Digits[6].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression7 / 0.8f)));
                        Digits[6].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression7 - 0.2f) / 0.8f, 0f));
                        float DigProgression8 = (float)(-5999999 + ((SM.TimeMade / 6) % 6000000));
                        Digits[7].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression8 / 0.8f)));
                        Digits[7].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression8 - 0.2f) / 0.8f, 0f));
                        float DigProgression9 = (float)(-59999999 + ((SM.TimeMade / 6) % 60000000));
                        Digits[8].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression9 / 0.8f)));
                        Digits[8].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression9 - 0.2f) / 0.8f, 0f));
                        float DigProgression10 = (float)(-599999999 + ((SM.TimeMade / 6) % 600000000));
                        Digits[9].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression10 / 0.8f)));
                        Digits[9].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression10 - 0.2f) / 0.8f, 0f));
                        for (int s = 1; s < 10; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10).ToString();
                    }
                } else if (Mode == 5) { // Total Time (h)
                    Digits[0].transform.GetChild(0).GetComponent<Text>().text = "h";
                    Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 3600) % 10).ToString();
                    Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 36000) % 10).ToString();
                    Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 360000) % 10).ToString();
                    Digits[4].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 3600000) % 10).ToString();
                    Digits[5].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 36000000) % 10).ToString();
                    Digits[6].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 360000000) % 10).ToString();
                    Digits[7].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 3600000000) % 10).ToString();
                    Digits[8].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 36000000000) % 10).ToString();
                    Digits[9].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TimeMade / 360000000000) % 10).ToString();
                    if (Animated) { float DigProgression2 = (float)(-99 + ((SM.TimeMade / 36) % 100));
                        Digits[1].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression2 / 0.8f)));
                        Digits[1].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                        float DigProgression3 = (float)(-999 + ((SM.TimeMade / 36) % 1000));
                        Digits[2].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression3 / 0.8f)));
                        Digits[2].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                        float DigProgression4 = (float)(-9999 + ((SM.TimeMade / 36) % 10000));
                        Digits[3].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression4 / 0.8f)));
                        Digits[3].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                        float DigProgression5 = (float)(-99999 + ((SM.TimeMade / 36) % 100000));
                        Digits[4].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression5 / 0.8f)));
                        Digits[4].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression5 - 0.2f) / 0.8f, 0f));
                        float DigProgression6 = (float)(-999999 + ((SM.TimeMade / 36) % 1000000));
                        Digits[5].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression6 / 0.8f)));
                        Digits[5].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression6 - 0.2f) / 0.8f, 0f));
                        float DigProgression7 = (float)(-9999999 + ((SM.TimeMade / 36) % 10000000));
                        Digits[6].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression7 / 0.8f)));
                        Digits[6].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression7 - 0.2f) / 0.8f, 0f));
                        float DigProgression8 = (float)(-99999999 + ((SM.TimeMade / 36) % 100000000));
                        Digits[7].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression8 / 0.8f)));
                        Digits[7].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression8 - 0.2f) / 0.8f, 0f));
                        float DigProgression9 = (float)(-999999999 + ((SM.TimeMade / 36) % 1000000000));
                        Digits[8].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression9 / 0.8f)));
                        Digits[8].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression9 - 0.2f) / 0.8f, 0f));
                        float DigProgression10 = (float)(-9999999999 + ((SM.TimeMade / 36) % 10000000000));
                        Digits[9].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression10 / 0.8f)));
                        Digits[9].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression10 - 0.2f) / 0.8f, 0f));
                        for (int s = 1; s < 10; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10).ToString();
                    }
                } else if (Mode == 6) { // Trip Distance
                    Digits[0].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripDistanceMade * 10f) % 10).ToString();
                    Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)SM.TripDistanceMade % 10).ToString();
                    Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.TripDistanceMade % 100) / 10).ToString();
                    Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.TripDistanceMade % 1000) / 100).ToString();
                    Digits[4].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.TripDistanceMade % 10000) / 1000).ToString();
                    Digits[5].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.TripDistanceMade % 100000) / 10000).ToString();
                    Digits[6].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.TripDistanceMade % 1000000) / 100000).ToString();
                    Digits[7].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.TripDistanceMade % 10000000) / 1000000).ToString();
                    Digits[8].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.TripDistanceMade % 100000000) / 10000000).ToString();
                    Digits[9].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.TripDistanceMade % 1000000000) / 100000000).ToString();
                    if (Animated) { float DigProgression = (float)(((SM.TripDistanceMade * 100) % 10) / 10);
                        Digits[0].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Max(1f - (DigProgression / 0.8f),0f));
                        Digits[0].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression - 0.2f) / 0.8f,0f));
                        float DigProgression2 = (float)(-9 + ((SM.TripDistanceMade * 10) % 10));
                        Digits[1].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression2 / 0.8f), 0f, 1f));
                        Digits[1].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                        float DigProgression3 = (float)(-99 + ((SM.TripDistanceMade * 10) % 100));
                        Digits[2].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression3 / 0.8f), 0f, 1f));
                        Digits[2].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                        float DigProgression4 = (float)(-999 + ((SM.TripDistanceMade * 10) % 1000));
                        Digits[3].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression4 / 0.8f), 0f, 1f));
                        Digits[3].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                        float DigProgression5 = (float)(-9999 + ((SM.TripDistanceMade * 10) % 10000));
                        Digits[4].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression5 / 0.8f), 0f, 1f));
                        Digits[4].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression5 - 0.2f) / 0.8f, 0f));
                        float DigProgression6 = (float)(-99999 + ((SM.TripDistanceMade * 10) % 100000));
                        Digits[5].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression6 / 0.8f), 0f, 1f));
                        Digits[5].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression6 - 0.2f) / 0.8f, 0f));
                        float DigProgression7 = (float)(-999999 + ((SM.TripDistanceMade * 10) % 1000000));
                        Digits[6].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression7 / 0.8f), 0f, 1f));
                        Digits[6].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression7 - 0.2f) / 0.8f, 0f));
                        float DigProgression8 = (float)(-9999999 + ((SM.TripDistanceMade * 10) % 10000000));
                        Digits[7].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression8 / 0.8f), 0f, 1f));
                        Digits[7].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression8 - 0.2f) / 0.8f, 0f));
                        float DigProgression9 = (float)(-99999999 + ((SM.TripDistanceMade * 10) % 100000000));
                        Digits[8].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression9 / 0.8f), 0f, 1f));
                        Digits[8].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression9 - 0.2f) / 0.8f, 0f));
                        float DigProgression10 = (float)(-999999999 + ((SM.TripDistanceMade * 10) % 1000000000));
                        Digits[9].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression10 / 0.8f), 0f, 1f));
                        Digits[9].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression10 - 0.2f) / 0.8f, 0f));
                        for(int s = 0; s < 10; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10) + "";
                    }
                } else if (Mode == 7) { // Trip Time (m:ss)
                    Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade) % 10).ToString();
                    Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 10) % 6).ToString();
                    Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 60) % 10).ToString();
                    Digits[4].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 600) % 10).ToString();
                    Digits[5].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 6000) % 10).ToString();
                    Digits[6].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 60000) % 10).ToString();
                    Digits[7].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 600000) % 10).ToString();
                    Digits[8].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 6000000) % 10).ToString();
                    Digits[9].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 60000000) % 10).ToString();
                    if (Animated) { float DigProgression2 = (float)(SM.TripTimeMade % 1);
                        Digits[1].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression2 / 0.8f)));
                        Digits[1].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                        float DigProgression3 = (float)(-9 + (SM.TripTimeMade % 10));
                        Digits[2].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression3 / 0.8f)));
                        Digits[2].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                        float DigProgression4 = (float)(-59 + (SM.TripTimeMade % 60));
                        Digits[3].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression4 / 0.8f)));
                        Digits[3].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                        float DigProgression5 = (float)(-599 + (SM.TripTimeMade % 600));
                        Digits[4].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression5 / 0.8f)));
                        Digits[4].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression5 - 0.2f) / 0.8f, 0f));
                        float DigProgression6 = (float)(-5999 + (SM.TripTimeMade % 6000));
                        Digits[5].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression6 / 0.8f)));
                        Digits[5].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression6 - 0.2f) / 0.8f, 0f));
                        float DigProgression7 = (float)(-59999 + (SM.TripTimeMade % 60000));
                        Digits[6].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression7 / 0.8f)));
                        Digits[6].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression7 - 0.2f) / 0.8f, 0f));
                        float DigProgression8 = (float)(-599999 + (SM.TripTimeMade % 600000));
                        Digits[7].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression8 / 0.8f)));
                        Digits[7].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression8 - 0.2f) / 0.8f, 0f));
                        float DigProgression9 = (float)(-5999999 + (SM.TripTimeMade % 6000000));
                        Digits[8].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression9 / 0.8f)));
                        Digits[8].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression9 - 0.2f) / 0.8f, 0f));
                        float DigProgression10 = (float)(-59999999 + (SM.TripTimeMade % 60000000));
                        Digits[9].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression10 / 0.8f)));
                        Digits[9].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression10 - 0.2f) / 0.8f, 0f));
                        for (int s = 1; s < 10; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10).ToString();
                    }
                } else if (Mode == 8) { // Trip Time (h:mm)
                    Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 60) % 10).ToString();
                    Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 600) % 6).ToString();
                    Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 3600) % 10).ToString();
                    Digits[4].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 36000) % 10).ToString();
                    Digits[5].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 360000) % 10).ToString();
                    Digits[6].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 3600000) % 10).ToString();
                    Digits[7].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 36000000) % 10).ToString();
                    Digits[8].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 360000000) % 10).ToString();
                    Digits[9].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 3600000000) % 10).ToString();
                    if (Animated) { float DigProgression2 = (float)(-9 + ((SM.TripTimeMade / 6) % 10));
                        Digits[1].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression2 / 0.8f)));
                        Digits[1].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                        float DigProgression3 = (float)(-99 + ((SM.TripTimeMade / 6) % 100));
                        Digits[2].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression3 / 0.8f)));
                        Digits[2].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                        float DigProgression4 = (float)(-599 + ((SM.TripTimeMade / 6) % 600));
                        Digits[3].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression4 / 0.8f)));
                        Digits[3].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                        float DigProgression5 = (float)(-5999 + ((SM.TripTimeMade / 6) % 6000));
                        Digits[4].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression5 / 0.8f)));
                        Digits[4].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression5 - 0.2f) / 0.8f, 0f));
                        float DigProgression6 = (float)(-59999 + ((SM.TripTimeMade / 6) % 60000));
                        Digits[5].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression6 / 0.8f)));
                        Digits[5].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression6 - 0.2f) / 0.8f, 0f));
                        float DigProgression7 = (float)(-599999 + ((SM.TripTimeMade / 6) % 600000));
                        Digits[6].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression7 / 0.8f)));
                        Digits[6].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression7 - 0.2f) / 0.8f, 0f));
                        float DigProgression8 = (float)(-5999999 + ((SM.TripTimeMade / 6) % 6000000));
                        Digits[7].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression8 / 0.8f)));
                        Digits[7].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression8 - 0.2f) / 0.8f, 0f));
                        float DigProgression9 = (float)(-59999999 + ((SM.TripTimeMade / 6) % 60000000));
                        Digits[8].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression9 / 0.8f)));
                        Digits[8].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression9 - 0.2f) / 0.8f, 0f));
                        float DigProgression10 = (float)(-599999999 + ((SM.TripTimeMade / 6) % 600000000));
                        Digits[9].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression10 / 0.8f)));
                        Digits[9].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression10 - 0.2f) / 0.8f, 0f));
                        for (int s = 1; s < 10; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10).ToString();
                    }
                } else if (Mode == 9) { // Trip Time (h)
                    Digits[0].transform.GetChild(0).GetComponent<Text>().text = "h";
                    Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 3600) % 10).ToString();
                    Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 36000) % 10).ToString();
                    Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 360000) % 10).ToString();
                    Digits[4].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 3600000) % 10).ToString();
                    Digits[5].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 36000000) % 10).ToString();
                    Digits[6].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 360000000) % 10).ToString();
                    Digits[7].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 3600000000) % 10).ToString();
                    Digits[8].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 36000000000) % 10).ToString();
                    Digits[9].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.TripTimeMade / 360000000000) % 10).ToString();
                    if (Animated) { float DigProgression2 = (float)(-99 + ((SM.TripTimeMade / 36) % 100));
                        Digits[1].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression2 / 0.8f)));
                        Digits[1].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                        float DigProgression3 = (float)(-999 + ((SM.TripTimeMade / 36) % 1000));
                        Digits[2].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression3 / 0.8f)));
                        Digits[2].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                        float DigProgression4 = (float)(-9999 + ((SM.TripTimeMade / 36) % 10000));
                        Digits[3].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression4 / 0.8f)));
                        Digits[3].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                        float DigProgression5 = (float)(-99999 + ((SM.TripTimeMade / 36) % 100000));
                        Digits[4].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression5 / 0.8f)));
                        Digits[4].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression5 - 0.2f) / 0.8f, 0f));
                        float DigProgression6 = (float)(-999999 + ((SM.TripTimeMade / 36) % 1000000));
                        Digits[5].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression6 / 0.8f)));
                        Digits[5].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression6 - 0.2f) / 0.8f, 0f));
                        float DigProgression7 = (float)(-9999999 + ((SM.TripTimeMade / 36) % 10000000));
                        Digits[6].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression7 / 0.8f)));
                        Digits[6].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression7 - 0.2f) / 0.8f, 0f));
                        float DigProgression8 = (float)(-99999999 + ((SM.TripTimeMade / 36) % 100000000));
                        Digits[7].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression8 / 0.8f)));
                        Digits[7].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression8 - 0.2f) / 0.8f, 0f));
                        float DigProgression9 = (float)(-999999999 + ((SM.TripTimeMade / 36) % 1000000000));
                        Digits[8].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression9 / 0.8f)));
                        Digits[8].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression9 - 0.2f) / 0.8f, 0f));
                        float DigProgression10 = (float)(-9999999999 + ((SM.TripTimeMade / 36) % 10000000000));
                        Digits[9].transform.GetChild(0).localScale = new Vector2(1f, Mathf.Clamp01(1f - (DigProgression10 / 0.8f)));
                        Digits[9].transform.GetChild(1).localScale = new Vector2(1f, Mathf.Max((DigProgression10 - 0.2f) / 0.8f, 0f));
                        for (int s = 1; s < 10; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10).ToString();
                    }
                } else if (Mode == 10) { // Max Speed
                    Digits[0].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.MaxSpeed * 10f) % 10).ToString();
                    Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)SM.MaxSpeed % 10).ToString();
                    Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.MaxSpeed % 100) / 10).ToString();
                    Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.MaxSpeed % 1000) / 100).ToString();
                    if (Animated) { float DigProgression = ((float)(SM.MaxSpeed * 100f) % 10f) / 10f;
                        Digits[0].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Max(1f - (DigProgression / 0.8f),0f));
                        Digits[0].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression - 0.2f) / 0.8f,0f));
                        float DigProgression2 = -9f + ((float)(SM.MaxSpeed * 10f) % 10f);
                        Digits[1].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression2 / 0.8f), 0f, 1f));
                        Digits[1].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                        float DigProgression3 = -99f + (((float)SM.MaxSpeed * 10f) % 100f);
                        Digits[2].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression3 / 0.8f), 0f, 1f));
                        Digits[2].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                        float DigProgression4 = -999f + (((float)SM.MaxSpeed * 10f) % 1000f);
                        Digits[3].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression4 / 0.8f), 0f, 1f));
                        Digits[3].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                        for (int s = 0; s < 4; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10) + "";
                    }
                } else if (Mode == 11) { // Avg Speed
                    Digits[0].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.AvgSpeed * 10f) % 10).ToString();
                    Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)SM.AvgSpeed % 10).ToString();
                    Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.AvgSpeed % 100) / 10).ToString();
                    Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.AvgSpeed % 1000) / 100).ToString();
                    if (Animated) { float DigProgression = ((float)(SM.AvgSpeed * 100f) % 10f) / 10f;
                        Digits[0].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Max(1f - (DigProgression / 0.8f),0f));
                        Digits[0].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression - 0.2f) / 0.8f,0f));
                        float DigProgression2 = -9f + ((float)(SM.AvgSpeed * 10f) % 10f);
                        Digits[1].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression2 / 0.8f), 0f, 1f));
                        Digits[1].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                        float DigProgression3 = -99f + (((float)SM.AvgSpeed * 10f) % 100f);
                        Digits[2].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression3 / 0.8f), 0f, 1f));
                        Digits[2].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                        float DigProgression4 = -999f + (((float)SM.AvgSpeed * 10f) % 1000f);
                        Digits[3].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression4 / 0.8f), 0f, 1f));
                        Digits[3].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                        for (int s = 0; s < 4; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10) + "";
                    }
                } else if (Mode == 12) { // Engine Load
                        Digits[0].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.OBDLoad * 10f) % 10).ToString();
                        Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)SM.OBDLoad % 10).ToString();
                        Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.OBDLoad % 100) / 10).ToString();
                        Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.OBDLoad % 1000) / 100).ToString();
                        Digits[4].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.OBDLoad % 10000) / 1000).ToString();
                        if (Animated) {
                            float DigProgression = ((float)(SM.OBDLoad * 100f) % 10f) / 10f;
                            Digits[0].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Max(1f - (DigProgression / 0.8f), 0f));
                            Digits[0].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression - 0.2f) / 0.8f, 0f));
                            float DigProgression2 = -9f + ((float)(SM.OBDLoad * 10f) % 10f);
                            Digits[1].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression2 / 0.8f), 0f, 1f));
                            Digits[1].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                            float DigProgression3 = -99f + (((float)SM.OBDLoad * 10f) % 100f);
                            Digits[2].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression3 / 0.8f), 0f, 1f));
                            Digits[2].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                            float DigProgression4 = -999f + (((float)SM.OBDLoad * 10f) % 1000f);
                            Digits[3].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression4 / 0.8f), 0f, 1f));
                            Digits[3].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                            float DigProgression5 = -9999f + (((float)SM.OBDLoad * 10f) % 10000f);
                            Digits[4].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression5 / 0.8f), 0f, 1f));
                            Digits[4].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression5 - 0.2f) / 0.8f, 0f));
                            for (int s = 0; s < 5; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10) + "";
                        }
                    } else if (Mode == 13) { // Coolant Temp.
                        Digits[0].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.OBDCoolant * 10f) % 10).ToString();
                        Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)SM.OBDCoolant % 10).ToString();
                        Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.OBDCoolant % 100) / 10).ToString();
                        Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.OBDCoolant % 1000) / 100).ToString();
                        Digits[4].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.OBDCoolant % 10000) / 1000).ToString();
                        if (Animated) {
                            float DigProgression = ((float)(SM.OBDCoolant * 100f) % 10f) / 10f;
                            Digits[0].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Max(1f - (DigProgression / 0.8f), 0f));
                            Digits[0].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression - 0.2f) / 0.8f, 0f));
                            float DigProgression2 = -9f + ((float)(SM.OBDCoolant * 10f) % 10f);
                            Digits[1].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression2 / 0.8f), 0f, 1f));
                            Digits[1].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                            float DigProgression3 = -99f + (((float)SM.OBDCoolant * 10f) % 100f);
                            Digits[2].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression3 / 0.8f), 0f, 1f));
                            Digits[2].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                            float DigProgression4 = -999f + (((float)SM.OBDCoolant * 10f) % 1000f);
                            Digits[3].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression4 / 0.8f), 0f, 1f));
                            Digits[3].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                            float DigProgression5 = -9999f + (((float)SM.OBDCoolant * 10f) % 10000f);
                            Digits[4].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression5 / 0.8f), 0f, 1f));
                            Digits[4].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression5 - 0.2f) / 0.8f, 0f));
                            for (int s = 0; s < 5; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10) + "";
                        }
                    } else if (Mode == 14) { // Throttle
                        Digits[0].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.OBDThrottle * 10f) % 10).ToString();
                        Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)SM.OBDThrottle % 10).ToString();
                        Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.OBDThrottle % 100) / 10).ToString();
                        Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.OBDThrottle % 1000) / 100).ToString();
                        Digits[4].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.OBDThrottle % 10000) / 1000).ToString();
                        if (Animated) {
                            float DigProgression = ((float)(SM.OBDThrottle * 100f) % 10f) / 10f;
                            Digits[0].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Max(1f - (DigProgression / 0.8f), 0f));
                            Digits[0].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression - 0.2f) / 0.8f, 0f));
                            float DigProgression2 = -9f + ((float)(SM.OBDThrottle * 10f) % 10f);
                            Digits[1].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression2 / 0.8f), 0f, 1f));
                            Digits[1].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                            float DigProgression3 = -99f + (((float)SM.OBDThrottle * 10f) % 100f);
                            Digits[2].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression3 / 0.8f), 0f, 1f));
                            Digits[2].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                            float DigProgression4 = -999f + (((float)SM.OBDThrottle * 10f) % 1000f);
                            Digits[3].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression4 / 0.8f), 0f, 1f));
                            Digits[3].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                            float DigProgression5 = -9999f + (((float)SM.OBDThrottle * 10f) % 10000f);
                            Digits[4].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression5 / 0.8f), 0f, 1f));
                            Digits[4].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression5 - 0.2f) / 0.8f, 0f));
                            for (int s = 0; s < 5; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10) + "";
                        }
                    } else if (Mode == 15) { // Fuel Level
                        Digits[0].transform.GetChild(0).GetComponent<Text>().text = ((int)(SM.OBDFuel * 10f) % 10).ToString();
                        Digits[1].transform.GetChild(0).GetComponent<Text>().text = ((int)SM.OBDFuel % 10).ToString();
                        Digits[2].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.OBDFuel % 100) / 10).ToString();
                        Digits[3].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.OBDFuel % 1000) / 100).ToString();
                        Digits[4].transform.GetChild(0).GetComponent<Text>().text = ((int)((int)SM.OBDFuel % 10000) / 1000).ToString();
                        if (Animated) {
                            float DigProgression = ((float)(SM.OBDFuel * 100f) % 10f) / 10f;
                            Digits[0].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Max(1f - (DigProgression / 0.8f), 0f));
                            Digits[0].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression - 0.2f) / 0.8f, 0f));
                            float DigProgression2 = -9f + ((float)(SM.OBDFuel * 10f) % 10f);
                            Digits[1].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression2 / 0.8f), 0f, 1f));
                            Digits[1].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression2 - 0.2f) / 0.8f, 0f));
                            float DigProgression3 = -99f + (((float)SM.OBDFuel * 10f) % 100f);
                            Digits[2].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression3 / 0.8f), 0f, 1f));
                            Digits[2].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression3 - 0.2f) / 0.8f, 0f));
                            float DigProgression4 = -999f + (((float)SM.OBDFuel * 10f) % 1000f);
                            Digits[3].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression4 / 0.8f), 0f, 1f));
                            Digits[3].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression4 - 0.2f) / 0.8f, 0f));
                            float DigProgression5 = -9999f + (((float)SM.OBDFuel * 10f) % 10000f);
                            Digits[4].transform.GetChild(0).transform.localScale = new Vector2(1f, Mathf.Clamp(1f - (DigProgression5 / 0.8f), 0f, 1f));
                            Digits[4].transform.GetChild(1).transform.localScale = new Vector2(1f, Mathf.Max((DigProgression5 - 0.2f) / 0.8f, 0f));
                            for (int s = 0; s < 5; s++) Digits[s].transform.GetChild(1).GetComponent<Text>().text = ((int.Parse(Digits[s].transform.GetChild(0).GetComponent<Text>().text) + 1) % 10) + "";
                        }
                    }
                RTimel -= (1f / Refrate);
            }
        }
    }
}
