using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using static SkinTextureReplace;
using static UnityEngine.ParticleSystem;

public class BluetoothMenu : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Meniu Bluetooth
    // Folosit pentru actualizarea meniului de setari / bluetooth / etc.
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public CustomTextureUniversal ctu; // Clasa folosita pentru a genera textura folosita pentru meniu

    // Lista de elenete si proprietati pentru clasa
    public Text SpeedTest;
    public Image FlickerTest;
    public Slider Interpol;
    public bool MPHM;
    public List<Image> mymodes;
    public InputField Diameter;
    public Dropdown InMode;
    public Text kmmitx;
    public InputField totalKM;
    public InputField totalTime;
    public Toggle interpSpeed;
    SPEEDMANAGER myspeed; // Clasa universala pentru variabile de viteza, turatie, etc.
    public GameObject TurnOff;
    public List<Image> Buttons2;
    public List<Sprite> ButtonsTex2;
    public List<Sprite> AllBTexc;
    public bool needsTiling;
    public Text Speed2T;
    public Text RPM2T;
    public RectTransform RPMB;
    public Text LoadT;
    public Text CoolantT;
    public Text ThrottleT;
    public Image LoadF;
    public Image CoolantF;
    public Image ThrottleF;
    public RectTransform FuelT;
    public Text RefR;
    public Text realData;
    public List<Image> LangButtons;
    public Toggle interpSpeed2;
    public RectTransform positiveIacc;
    public RectTransform negativeIacc;
    public Text pointBlank;
    public List<UILineRenderer> uilr;
    public RectTransform poinv;
    public Slider InterpSpeed;
    public Slider AssistAmount;
    public Text AssistperS;
    public Dropdown GyroMode;

    public GameObject BLEMODL;
    public GameObject BLUMODL;
    public GameObject BLUMODL2;

    public RectTransform LvContainer;
    public Text lvText;
    public Text expText;
    public RectTransform expBar;
    public Text gainXpText;
    public InputField nameInput;
    public RectTransform trans;
    public GameObject clonerObjBB;
    public GameObject extraMenu;
    public Text ENameText;
    public Text ESDate;
    public Text ESTime;
    public Text EEDate;
    public Text EETime;
    public Text EEXPText;
    public GameObject LOADICO;
    public InputField searchInput;
    public Button tripUploadB;
    public int Level;
    public int Exp;
    public int NeededExp;
    public int PotentialExp;

    public float tripDistance;
    public float tripTime;
    public TextMeshProUGUI tripdist;
    public TextMeshProUGUI triptime;

    float Diamtr = 0;
    bool inital = false;

    int skinSl;
    float rainBoprogress;
    public int selMop;
    public int selLan;

    Color idlec = new Color(1f, 0.9f, 0.48f);
    Color idlecS = new Color(0.6f, 0.4f, 0f);
    Color pozc = new Color(0.36f, 0.57f, 1f);
    Color pozcS = new Color(0.0f, 0.33f, 0.5f);
    Color negc = new Color(1f, 0.3f, 0.3f);
    Color negcS = new Color(0.5f, 0f, 0f);

    // Functie executata cand elementul este activat pentru prima data
    void Start() {
        if (PlayerPrefs.GetInt("USKIN") == 0) { int s = PlayerPrefs.GetInt("SKINMETAL"); skinSl = s;
            ctu = GameObject.Find("UNIVERSALSKIN").GetComponent<CustomTextureUniversal>();
            if(s != 10) { ButtonsTex2.Add(AllBTexc[s]); needsTiling = (s == 3 || s == 5 | s == 7); }
            else { ButtonsTex2.Add(ctu.CustomImgTexture); needsTiling = (PlayerPrefs.GetInt("UTEXTUREGLOSSTEX", 0) == 1); }
        } if (myspeed == null) myspeed = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        if (PlayerPrefs.GetInt("INSETUP", 0) == 1) SelectBLEGPS(PlayerPrefs.GetInt("SPEEDSOURCE"));
        Diamtr = PlayerPrefs.GetFloat("DIAMETER", 56.4f);
        Diameter.text = Diamtr.ToString(CultureInfo.InvariantCulture);
        Interpol.value = PlayerPrefs.GetFloat("SPEEDINTERPOL", 0.7f);
        totalKM.text = myspeed.DistanceMade.ToString("F1");
        totalTime.text = (myspeed.TimeMade / 3600f).ToString("F2");
        InterpSpeed.value = 0.11f - PlayerPrefs.GetFloat("ASSISTINTERPOL", 0.05f);
        AssistAmount.value = PlayerPrefs.GetFloat("ASSISTVALUE", 1f);
        AssistperS.text = (int)(AssistAmount.value * 100f) + "";
        GyroMode.value = PlayerPrefs.GetInt("GYROMODE", 0);
        if (PlayerPrefs.GetInt("MPHMODE", 0) == 0) ToggleModeMPH(false);
        if (PlayerPrefs.GetInt("MPHMODE", 0) == 1) ToggleModeMPH(true);
        int Sel = PlayerPrefs.GetInt("MAINLANG");
        int hasInterp = PlayerPrefs.GetInt("ACCELINTERP", 1);
        Level = PlayerPrefs.GetInt("USERLEVEL", 1);
        Exp = PlayerPrefs.GetInt("USEREXP", 0);
        NeededExp = CalcNeededExp();
        myspeed.AccelerometerAssist = (hasInterp == 1);
        interpSpeed.isOn = (hasInterp == 1);
        interpSpeed2.isOn = (hasInterp == 1);
        for(int b = 0; b < LangButtons.Count; b++) {
            if (b == Sel) { LangButtons[b].sprite = ButtonsTex2[1]; 
            if (needsTiling) LangButtons[b].type = Image.Type.Tiled; else LangButtons[b].type = Image.Type.Sliced; }
            else { LangButtons[b].sprite = ButtonsTex2[0]; LangButtons[b].type = Image.Type.Sliced; }
        } Input.gyro.enabled = true; inital = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadTripData(-1); CalcNeededExp(); UpdateLvParam(); 
        StartCoroutine(InitSearch(""));
        trans.GetChild(0).GetComponent<Button>().onClick.AddListener(() => LoadTripData(-1));
        tripUploadB.interactable = EnabledSend(); myspeed.onStartTrip += setStarter;
    }

    // Functie executata cand data de la senzorul bluetooth este primita (efect vizual)
    public void OnTickSpeed() {
        FlickerTest.color = new Color(1f, 0.5f, 0f, 10f);
    }

    // Functie executata cand modul de viteza este schimbat intre Mph si Km/h
    public void ToggleModeMPH(bool ButOfCourse) {
        MPHM = ButOfCourse; if (MPHM) { mymodes[0].color = new Color(1, 1, 1, 0); mymodes[1].color = new Color(1, 1, 1, 1); PlayerPrefs.SetInt("MPHMODE", 1); kmmitx.text = "mi"; }
        else { mymodes[1].color = new Color(1, 1, 1, 0); mymodes[0].color = new Color(1, 1, 1, 1); PlayerPrefs.SetInt("MPHMODE", 0); kmmitx.text = "km"; } StartCoroutine(askw());
    }

    // Functie executata pentru a modifica valorile de kilometri totali cand modul de viteza este schimbat intre Mph si Km/h
    public void ModValue(bool Time) {
        if(!MPHM) { double MyVal = ((double.Parse(totalKM.text, CultureInfo.InvariantCulture) / myspeed.Diameter / Mathf.PI) * 100000d);
            long MyVal2 = (long)(MyVal); int Int1s = (int)(MyVal2 % 2147483648L); int Int2s = (int)(MyVal2 / 2147483648L);
            if (!Time) { PlayerPrefs.SetInt("TOTALDISTICKSINT1", Int1s); PlayerPrefs.SetInt("TOTALDISTICKSINT2", Int2s); myspeed.DistanceTicks = MyVal2; } 
        else { double Value = (double.Parse(totalTime.text, CultureInfo.InvariantCulture) * 3600f); long ValL = (long)(Value * 10000d);
            int T1 = (int)(ValL % 2147483648L); int T2 = (int)(ValL / 2147483648L);
            PlayerPrefs.SetInt("TOTALDISTIMEINT1",T1); PlayerPrefs.SetInt("TOTALDISTIMEINT2", T2); myspeed.TimeMade = Value; } }
        else { double MyVal = ((double.Parse(totalKM.text, CultureInfo.InvariantCulture) / myspeed.Diameter / Mathf.PI) * 160900d);
            long MyVal2 = (long)(MyVal); int Int1s = (int)(MyVal2 % 2147483648L); int Int2s = (int)(MyVal2 / 2147483648L);
            if (!Time) { PlayerPrefs.SetInt("TOTALDISTICKSINT1", Int1s); PlayerPrefs.SetInt("TOTALDISTICKSINT2", Int2s); myspeed.DistanceTicks = MyVal2; }
        else { double Value = (double.Parse(totalTime.text, CultureInfo.InvariantCulture) * 3600f); long ValL = (long)(Value * 10000d);
        int T1 = (int)(ValL % 2147483648L); int T2 = (int)(ValL / 2147483648L);
        PlayerPrefs.SetInt("TOTALDISTIMEINT1", T1); PlayerPrefs.SetInt("TOTALDISTIMEINT2", T2); myspeed.TimeMade = Value; } }
         StartCoroutine(askw());
    }

    // Functie executata pentru a schimba interfata meniului de setari
    public IEnumerator askw() {
        OnModSetting();
        yield return null;
        OnModSetting();
    }

    // Functie executata cand este schimbata sursa de viteza
    public void SelectBLEGPS(int BLE) { if(BLE != 3 && BLE != 4) PlayerPrefs.SetInt("SPEEDSOURCE", BLE);
        if (BLE == 0) { myspeed = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        GameObject.Find("Nav").GetComponent<MenuNav>().TryAgain(); myspeed.InitGPS();
            Buttons2[0].sprite = ButtonsTex2[1]; if (needsTiling) Buttons2[0].type = Image.Type.Tiled; else Buttons2[0].type = Image.Type.Sliced;
            Buttons2[1].sprite = ButtonsTex2[0]; Buttons2[1].type = Image.Type.Sliced;
            Buttons2[2].sprite = ButtonsTex2[0]; Buttons2[2].type = Image.Type.Sliced; selMop = 0;
        } else if (BLE == 1) { Buttons2[0].sprite = ButtonsTex2[0]; if (needsTiling) Buttons2[1].type = Image.Type.Tiled; else Buttons2[1].type = Image.Type.Sliced;
            Buttons2[1].sprite = ButtonsTex2[1]; Buttons2[0].type = Image.Type.Sliced;
            Buttons2[2].sprite = ButtonsTex2[0]; Buttons2[2].type = Image.Type.Sliced; selMop = 1; }
        else if (BLE == 2) { Buttons2[0].sprite = ButtonsTex2[0]; if (needsTiling) Buttons2[2].type = Image.Type.Tiled; else Buttons2[2].type = Image.Type.Sliced;
            Buttons2[1].sprite = ButtonsTex2[0]; Buttons2[0].type = Image.Type.Sliced;
            Buttons2[2].sprite = ButtonsTex2[1]; Buttons2[1].type = Image.Type.Sliced; selMop = 2; }
        if (BLE != 3 && BLE != 4) myspeed.ss = BLE;
        if (BLE == 3) { if(!interpSpeed.isOn) PlayerPrefs.SetInt("ACCELINTERP", 0);
           else PlayerPrefs.SetInt("ACCELINTERP", 1);
            myspeed.AccelerometerAssist = interpSpeed.isOn;
            interpSpeed2.isOn = interpSpeed.isOn; }
        if (BLE == 4) { if(!interpSpeed2.isOn) PlayerPrefs.SetInt("ACCELINTERP", 0);
           else PlayerPrefs.SetInt("ACCELINTERP", 1);
            myspeed.AccelerometerAssist = interpSpeed2.isOn; 
            interpSpeed.isOn = interpSpeed2.isOn; }
    }

    // Functie executata cand este schimbata sursa de viteza (Folosita pentru interfata)
    public void OnTunB() {
        if (PlayerPrefs.GetInt("SPEEDSOURCE") == 0) TurnOff.SetActive(true); else TurnOff.SetActive(false);
        if (PlayerPrefs.GetInt("SPEEDSOURCE") == 1 || PlayerPrefs.GetInt("SPEEDSOURCE") == 0) BLEMODL.SetActive(true);
        if (PlayerPrefs.GetInt("SPEEDSOURCE") == 2) { BLUMODL.SetActive(true); BLUMODL2.SetActive(true); }
    }

    // Functie propriu-zisa executata cand setarile sunt schimbate
    public void OnModSetting() { if(inital) { 
        PlayerPrefs.SetFloat("SPEEDINTERPOL", Interpol.value);
        if (myspeed == null) myspeed = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        myspeed.MPHMode = MPHM; if (InMode.value == 0) { Diamtr = Mathf.Min(float.Parse(Diameter.text, CultureInfo.InvariantCulture), 500f);
            Diameter.text = Diamtr.ToString(CultureInfo.InvariantCulture); }
        if (InMode.value == 1) { Diamtr = Mathf.Min(float.Parse(Diameter.text, CultureInfo.InvariantCulture) * 2.54f, 500f);
            Diameter.text = (Diamtr / 2.54f).ToString(CultureInfo.InvariantCulture); }
        PlayerPrefs.SetFloat("DIAMETER", Diamtr); totalKM.text = myspeed.DistanceMade.ToString("F1");
        totalTime.text = (myspeed.TimeMade / 3600f).ToString("F2");
        interpSpeed.isOn = myspeed.AccelerometerAssist;
        interpSpeed2.isOn = myspeed.AccelerometerAssist;
        PlayerPrefs.SetFloat("ASSISTINTERPOL", (0.11f - InterpSpeed.value));
        PlayerPrefs.SetFloat("ASSISTVALUE", AssistAmount.value);
        AssistperS.text = (int)(AssistAmount.value * 100f) + "";
        PlayerPrefs.SetInt("GYROMODE", GyroMode.value);
        myspeed.onModSet(true); }
    }

    // Functie executata cand limajul aplicatiei este schimbat
    public void SetLang(int Sel) {
        for(int b = 0; b < LangButtons.Count; b++) {
            if (b == Sel) { LangButtons[b].sprite = ButtonsTex2[1]; 
            if (needsTiling) LangButtons[b].type = Image.Type.Tiled; else LangButtons[b].type = Image.Type.Sliced; }
            else { LangButtons[b].sprite = ButtonsTex2[0]; LangButtons[b].type = Image.Type.Sliced; }
        } PlayerPrefs.SetInt("MAINLANG", Sel);
        SceneManager.LoadScene(0);
    }

    // Functie folosita pentru a calcula experienta necesara pentru a ajunge la urmatorul nivel
    public int CalcNeededExp() {
        return 4775 + ((222 + (Level * 3)) * Level);
    }

    // Functie folosita pentru a calcula experienta primita de la plimbarea ta
    public int CalcPotentialExp() {
        return (int)((tripDistance * 100f) + (tripTime / 3f));
    }

    // Functie folosita pentru a determina daca plimbarea poate fi terminata
    public bool EnabledSend() {
        int cpexp = CalcPotentialExp();
        return (cpexp >= 5);
    }

    // Functie executata cand scena din unity este schimbata
    public void OnSceneLoaded(Scene s, LoadSceneMode lm) {
        if(s.buildIndex == 0) {
            LoadTripData(-1);
            CalcNeededExp();
            UpdateLvParam();
            StartCoroutine(InitSearch(""));
            trans.GetChild(0).GetComponent<Button>().onClick.AddListener(() => LoadTripData(-1));
            tripUploadB.interactable = EnabledSend();
        }
    }

    // Functie executata pentru a incarca data din plimbari vechi sau cea prezenta
    public void LoadTripData(int TripID) {
        if(TripID == -1) { extraMenu.SetActive(false);
            tripDistance = PlayerPrefs.GetFloat("UTRIPDISTANCE");
            if (myspeed.MPHMode) tripDistance *= 0.6213f;
            tripTime = PlayerPrefs.GetFloat("UTRIPTIME");
            int hours = (int)tripTime / 3600;
            int minutes = ((int)tripTime / 60) % 60;
            int seconds = (int)tripTime % 60;
            if(minutes == 0 && hours == 0) {
                triptime.text = "0:" + seconds.ToString("D2");
            } else if(hours == 0) {
                triptime.text = minutes + ":" + seconds.ToString("D2");
            } else {
                triptime.text = hours + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
            } tripdist.text = tripDistance.ToString("F2"); }
        else { extraMenu.SetActive(true);
            float trDistance = PlayerPrefs.GetFloat("TRIP" + TripID + "KMS");
            float trTime = PlayerPrefs.GetFloat("TRIP" + TripID + "TIM");
            int hours = (int)trTime / 3600;
            int minutes = ((int)trTime / 60) % 60;
            int seconds = (int)trTime % 60;
            if(minutes == 0 && hours == 0) {
                triptime.text = "0:" + seconds.ToString("D2");
            } else if(hours == 0) {
                triptime.text = minutes + ":" + seconds.ToString("D2");
            } else {
                triptime.text = hours + ":" + minutes.ToString("D2") + ":" + seconds.ToString("D2");
            } tripdist.text = trDistance.ToString("F2");
            ENameText.text = PlayerPrefs.GetString("TRIP" + TripID + "NAME");
            ESDate.text = PlayerPrefs.GetString("TRIP" + TripID + "STARTDATE");
            ESTime.text = PlayerPrefs.GetString("TRIP" + TripID + "STARTTIME");
            EEDate.text = PlayerPrefs.GetString("TRIP" + TripID + "ENDDATE");
            EETime.text = PlayerPrefs.GetString("TRIP" + TripID + "ENDTIME");
            int gained = PlayerPrefs.GetInt("TRIP" + TripID + "GAINEDXP");
            EEXPText.text = "+" + gained + "xp";
            if (gained <= 99999) { EEXPText.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f); }
            else if (gained <= 999999) { EEXPText.GetComponent<RectTransform>().localScale = new Vector2(0.89f, 1f); }
            else if (gained <= 9999999) { EEXPText.GetComponent<RectTransform>().localScale = new Vector2(0.79f, 1f); }
            else if (gained <= 99999999) { EEXPText.GetComponent<RectTransform>().localScale = new Vector2(0.71f, 1f); }
        }
    }

    // Functie executata pentru a actualiza interfata panoului de nivele
    public void UpdateLvParam() {
        Level = PlayerPrefs.GetInt("USERLEVEL", 1);
        Exp = PlayerPrefs.GetInt("USEREXP", 0);
        NeededExp = CalcNeededExp();
        if (Level < 10) LvContainer.sizeDelta = new Vector2(780f, 140f);
        else if (Level < 100) LvContainer.sizeDelta = new Vector2(838f, 140f);
        else if (Level < 1000) LvContainer.sizeDelta = new Vector2(897f, 140f);
        else if (Level < 10000) LvContainer.sizeDelta = new Vector2(955f, 140f);
        lvText.text = Level + ""; expText.text = Exp + "/" + NeededExp;
        expBar.sizeDelta = new Vector2(((float)Exp / (float)NeededExp) * 540f, expBar.sizeDelta.y);
        PotentialExp = CalcPotentialExp(); gainXpText.text = "+" + PotentialExp + "xp";
        if(PotentialExp <= 99999) { gainXpText.GetComponent<RectTransform>().localScale = new Vector2(1f, 1f); }
        else if(PotentialExp <= 999999) { gainXpText.GetComponent<RectTransform>().localScale = new Vector2(0.89f, 1f); }
        else if(PotentialExp <= 9999999) { gainXpText.GetComponent<RectTransform>().localScale = new Vector2(0.79f, 1f); }
        else if(PotentialExp <= 99999999) { gainXpText.GetComponent<RectTransform>().localScale = new Vector2(0.71f, 1f); }
    }

    // Functie folosita pentru a incarca toate plimbarile vechi cu tot cu datele lor
    public void LoadAllTripData(string searchFilter) {
        string datetime = DateTime.Now.ToString("dd-MM-yyyy"); string timedate = DateTime.Now.ToString("HH:mm:ss");
        if (PlayerPrefs.GetString("LASTTRIPSTART", "yyy") == "yyy") PlayerPrefs.SetString("LASTTRIPSTART", datetime);
        if (PlayerPrefs.GetString("LASTTRIPTIMES", "owo") == "owo") PlayerPrefs.SetString("LASTTRIPTIMES", timedate);
        int AllTrips = PlayerPrefs.GetInt("LASTTRIPID", 0);
        for(int i = trans.childCount - 1; i >= 2; i--) {
            Destroy(trans.GetChild(i).gameObject);
        } if(string.IsNullOrEmpty(searchFilter)) { 
        for(int i = AllTrips - 1; i >= Mathf.Max(0, AllTrips - 50); i--) {
            GameObject clone = Instantiate(clonerObjBB, trans);
            clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -180f + (-85f * ((AllTrips - 1) - i)));
            clone.transform.GetChild(0).GetComponent<TranslateScript>().enabled = false;
            clone.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("TRIP" + i + "NAME");
            clone.GetComponent<Button>().onClick.RemoveAllListeners(); int lbi = i;
            clone.GetComponent<Button>().onClick.AddListener(() => LoadTripData(lbi));
        } trans.sizeDelta = new Vector2(trans.sizeDelta.x, 240f + (85f * AllTrips)); }
        else { int UsedSlots = 0;
            for(int i = AllTrips - 1; i >= 0; i--) {
            string name = PlayerPrefs.GetString("TRIP" + i + "NAME");
            if (!name.ToLower().Contains(searchFilter.ToLower())) continue;
            GameObject clone = Instantiate(clonerObjBB, trans);
            clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -180f + (-85f * UsedSlots));
            clone.transform.GetChild(0).GetComponent<TranslateScript>().enabled = false;
            clone.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("TRIP" + i + "NAME");
            clone.GetComponent<Button>().onClick.RemoveAllListeners(); int lbi = i;
            clone.GetComponent<Button>().onClick.AddListener(() => LoadTripData(lbi)); UsedSlots++;
        } trans.sizeDelta = new Vector2(trans.sizeDelta.x, 240f + (85f * UsedSlots));
        }
    }

    // Functie folosita pentru cautarea unei plimbari vechi bazat be numele lor
    public IEnumerator InitSearch(string sfilter) {
        LOADICO.SetActive(true);
        yield return null;
        LoadAllTripData(sfilter);
        LOADICO.SetActive(false);
    }

    // Functie executata cand valoarea din bara de cautare este schimbata
    public void OnSearchValChanged() {
        int AllTrips = PlayerPrefs.GetInt("LASTTRIPID", 0);
        if (AllTrips > 100 && searchInput.text != "") return;
        StartCoroutine(InitSearch(searchInput.text));
    }

    // Functie executata cand valoarea din bara de cautare este confirmata
    public void OnSearchValEnter() {
        int AllTrips = PlayerPrefs.GetInt("LASTTRIPID", 0);
        if (AllTrips <= 100) return;
        StartCoroutine(InitSearch(searchInput.text));
    }

    // Functie executata pentru a seta data si timpul de inceput al plimbarii
    public void setStarter() {
        string datetime = DateTime.Now.ToString("dd-MM-yyyy"); string timedate = DateTime.Now.ToString("HH:mm:ss");
        PlayerPrefs.SetString("LASTTRIPSTART", datetime);
        PlayerPrefs.SetString("LASTTRIPTIMES", timedate);
    }

    // Functie executata cand o plimbare este incheiata
    public void EndTrip() {
        PotentialExp = CalcPotentialExp(); NeededExp = CalcNeededExp();
        int CurrentTripID = PlayerPrefs.GetInt("LASTTRIPID", 0);
        tripDistance = PlayerPrefs.GetFloat("UTRIPDISTANCE");
        tripTime = PlayerPrefs.GetFloat("UTRIPTIME");
        PlayerPrefs.SetFloat("TRIP" + CurrentTripID + "KMS", tripDistance);
        PlayerPrefs.SetFloat("TRIP" + CurrentTripID + "TIM", tripTime);
        string datetime = DateTime.Now.ToString("dd-MM-yyyy"); string timedate = DateTime.Now.ToString("HH:mm:ss");
        PlayerPrefs.SetString("TRIP" + CurrentTripID + "STARTDATE", PlayerPrefs.GetString("LASTTRIPSTART"));
        PlayerPrefs.SetString("TRIP" + CurrentTripID + "STARTTIME", PlayerPrefs.GetString("LASTTRIPTIMES"));
        PlayerPrefs.SetString("TRIP" + CurrentTripID + "ENDDATE", datetime);
        PlayerPrefs.SetString("TRIP" + CurrentTripID + "ENDTIME", timedate);
        PlayerPrefs.SetInt("TRIP" + CurrentTripID + "GAINEDXP", PotentialExp);
        if (string.IsNullOrEmpty(nameInput.text)) {
            PlayerPrefs.SetString("TRIP" + CurrentTripID + "NAME", "Trip " + datetime);
        } else { PlayerPrefs.SetString("TRIP" + CurrentTripID + "NAME", nameInput.text); }
        PlayerPrefs.SetInt("LASTTRIPID", CurrentTripID + 1);
        PlayerPrefs.SetFloat("UTRIPDISTANCE", 0);
        PlayerPrefs.SetFloat("UTRIPTIME", 0);
        myspeed.TripDistanceTicks = 0;
        myspeed.TripDistanceMade = 0;
        myspeed.TripTimeMade = 0;
        Exp += PotentialExp;
        while(Exp >= NeededExp) {
            Level++; Exp -= NeededExp;
            NeededExp = CalcNeededExp();
        } PlayerPrefs.SetInt("USERLEVEL", Level);
        PlayerPrefs.SetInt("USEREXP", Exp);
        UpdateLvParam(); LoadTripData(-1);
        StartCoroutine(InitSearch(""));
        tripUploadB.interactable = EnabledSend();
        gainXpText.text = "+0xp";
    }

    // Functie chemata de 60 de ori / secunda | Functie folosita pentru a actualiza interfata in timp real
    void FixedUpdate() {
        if(myspeed == null) myspeed = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        SpeedTest.text = myspeed.InterpSpeed.ToString("F1");
        FlickerTest.color = new Color(1f, 0.5f, 0f, FlickerTest.color.a / 1.5f);
        Speed2T.text = myspeed.InterpSpeed.ToString("F1");
        RPM2T.text = ((int)myspeed.RPM).ToString();
        RPMB.sizeDelta = new Vector2(Mathf.Min(myspeed.RPM / 12f, 490f), RPMB.sizeDelta.y);
        LoadT.text = ((int)myspeed.OBDLoad).ToString();
        CoolantT.text = ((int)myspeed.OBDCoolant).ToString();
        ThrottleT.text = ((int)myspeed.OBDThrottle).ToString();
        CoolantF.fillAmount = Mathf.Clamp((myspeed.OBDCoolant - 20f) / 150f, 0f, 0.75f);
        LoadF.fillAmount = Mathf.Clamp(myspeed.OBDLoad / 133f, 0f, 0.75f);
        ThrottleF.fillAmount = Mathf.Clamp(myspeed.OBDThrottle / 133f, 0f, 0.75f);
        FuelT.sizeDelta = new Vector2(Mathf.Min(myspeed.OBDFuel * 6.88f, 688f), FuelT.sizeDelta.y);
        RefR.text = myspeed.OBDRf.ToString("F1"); realData.text = myspeed.obd.lastsend;
        if(Mathf.Abs(myspeed.accelCurrent) < 0.001f) {
            positiveIacc.sizeDelta = new Vector2(0f, positiveIacc.sizeDelta.y);
            negativeIacc.sizeDelta = new Vector2(0f, negativeIacc.sizeDelta.y);
            pointBlank.text = ".000"; pointBlank.color = idlec;
            pointBlank.GetComponent<Shadow>().effectColor = idlecS;
        } else if(myspeed.accelCurrent > 0f) {
            float coef = Mathf.Clamp01(myspeed.accelCurrent * 5f);
            positiveIacc.sizeDelta = new Vector2(coef * 584f, positiveIacc.sizeDelta.y);
            negativeIacc.sizeDelta = new Vector2(0f, negativeIacc.sizeDelta.y);
            pointBlank.text = (myspeed.accelCurrent).ToString("F3").Substring(1); pointBlank.color = pozc;
            pointBlank.GetComponent<Shadow>().effectColor = pozcS;
        } else if(myspeed.accelCurrent < 0f) {
            float coef = Mathf.Clamp01(-myspeed.accelCurrent * 5f);
            negativeIacc.sizeDelta = new Vector2(coef * 584f, negativeIacc.sizeDelta.y);
            positiveIacc.sizeDelta = new Vector2(0f, positiveIacc.sizeDelta.y);
            pointBlank.text = (-myspeed.accelCurrent).ToString("F3").Substring(1); pointBlank.color = negc;
            pointBlank.GetComponent<Shadow>().effectColor = negcS;
        } if(myspeed.positions.Count > 0) { foreach(UILineRenderer ulr in uilr) ulr.Points = myspeed.positions.ToArray();
        int lastPoint = myspeed.positions.Count - 1; poinv.anchoredPosition = myspeed.positions[lastPoint]; }
        if(skinSl == 9) { rainBoprogress += Time.deltaTime * 0.3f; if (rainBoprogress > 1f) rainBoprogress -= 1f;
            Color primary = Color.HSVToRGB(rainBoprogress, 0.7f, 1f);
            for (int i = 0; i < Buttons2.Count; i++) if(selMop == i) Buttons2[i].color = primary; else Buttons2[i].color = Color.white;
            for (int i = 0; i < LangButtons.Count; i++) if (selLan == i) LangButtons[i].color = primary; else LangButtons[i].color = Color.white;
        }
    }
}
