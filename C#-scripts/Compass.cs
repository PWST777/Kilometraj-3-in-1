using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Busola
    // Folosit pentru elementul Busola
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public SPEEDMANAGER SM; // Clasa universala pentru variabile de viteza, turatie, etc.
    public SelectDrag Sdrg; // Clasa universala pentru aranjarea, crearea si stergerea elementelor
    public ColorPicker CoPi; // Clasa universala pentru selectarea culorii
    public bool SceneMode; // Variabila folosita pentru a determina daca elementul este editad sau utilizat
    public int ElementID; // Vaiabila numerica autonumerotata pentru identificarea elementului
    public bool LoadMode;
    public GameObject EditorModPanel;

    // Lista de elenete si proprietati pentru clasa
    public GameObject Compass2D;
    public GameObject Compass3Dcont;
    public GameObject Compass3D;

    public RectTransform content;
    public bool Mode3D; public Toggle M3D;
    public int OutLSize; public Slider OuSz;
    public Color OutC; public Image OuC;
    public Color BackC; public Image BaC;
    public bool LockNeed; public Toggle ln;
    public int NeedSize; public Slider NeSz;
    public Color NeFC; public Image NFC;
    public Color NeRC; public Image NRC;
    public int MarkSize; public Slider MaSz;
    public Color MarkC; public Image MineC;
    public int NSEWsys; public List<Toggle> NSEWs;
    public bool Detail1; public Toggle Dt1;
    public bool ShowDigL; public Toggle SDL;
    public bool ShowNorL; public Toggle SNL;
    public Color DetC; public Image DC;
    public float Perspl; public Slider Pers;
    public bool ShowAlML; public Toggle SAM;
    public int FadeRange; public Slider FDR;
    public Button DelObj; public InputField soo;
    public List<RectTransform> MenuPanels;

    // Functie executata cand elementul este activat pentru prima data
    void Start() {
        SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        if (LoadMode) OnSelectOfElement();
        OnComponentCompModdify();
        OnColorModdify();
    }

    // Functie executata cand elementul este creat sau este selectat
    public void OnSelectOfElement() { if (!SceneMode) { LoadMode = true;
        // Aplicarea functiilor pe elementele de interfata (elemente Button, InputField etc.)
        M3D.onValueChanged.RemoveAllListeners(); M3D.onValueChanged.AddListener((_) => OnComponentCompModdify());
        OuSz.onValueChanged.RemoveAllListeners(); OuSz.onValueChanged.AddListener((_) => OnComponentCompModdify());
        ln.onValueChanged.RemoveAllListeners(); ln.onValueChanged.AddListener((_) => OnComponentCompModdify());
        NeSz.onValueChanged.RemoveAllListeners(); NeSz.onValueChanged.AddListener((_) => OnComponentCompModdify());
        MaSz.onValueChanged.RemoveAllListeners(); MaSz.onValueChanged.AddListener((_) => OnComponentCompModdify());
        NSEWs[0].onValueChanged.RemoveAllListeners(); NSEWs[0].onValueChanged.AddListener((_) => OnComponentCompModdify());
        NSEWs[1].onValueChanged.RemoveAllListeners(); NSEWs[1].onValueChanged.AddListener((_) => OnComponentCompModdify());
        NSEWs[2].onValueChanged.RemoveAllListeners(); NSEWs[2].onValueChanged.AddListener((_) => OnComponentCompModdify());
        NSEWs[3].onValueChanged.RemoveAllListeners(); NSEWs[3].onValueChanged.AddListener((_) => OnComponentCompModdify());
        Dt1.onValueChanged.RemoveAllListeners(); Dt1.onValueChanged.AddListener((_) => OnComponentCompModdify());
        SDL.onValueChanged.RemoveAllListeners(); SDL.onValueChanged.AddListener((_) => OnComponentCompModdify());
        SNL.onValueChanged.RemoveAllListeners(); SNL.onValueChanged.AddListener((_) => OnComponentCompModdify());
        Pers.onValueChanged.RemoveAllListeners(); Pers.onValueChanged.AddListener((_) => OnComponentCompModdify());
        SAM.onValueChanged.RemoveAllListeners(); SAM.onValueChanged.AddListener((_) => OnComponentCompModdify());
        FDR.onValueChanged.RemoveAllListeners(); FDR.onValueChanged.AddListener((_) => OnComponentCompModdify());
        OuC.GetComponent<Button>().onClick.RemoveAllListeners(); OuC.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("COOUTLCL"));
        BaC.GetComponent<Button>().onClick.RemoveAllListeners(); BaC.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("COBACKCL"));
        NFC.GetComponent<Button>().onClick.RemoveAllListeners(); NFC.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("CONEDFCL"));
        NRC.GetComponent<Button>().onClick.RemoveAllListeners(); NRC.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("CONEDRCL"));
        MineC.GetComponent<Button>().onClick.RemoveAllListeners(); MineC.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("COMARKCL"));
        DC.GetComponent<Button>().onClick.RemoveAllListeners(); DC.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("CODETACL"));
        DelObj.onClick.RemoveAllListeners(); DelObj.onClick.AddListener(() => DeleteObj(69));
        soo.onSubmit.RemoveAllListeners(); soo.onSubmit.AddListener((_) => ModSOrtO()); }
        // Incarcarea variabilelor salvate
        OutLSize = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COOUTS", 20);
        NeedSize = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "CONEDS", 50);
        MarkSize = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COMRKS", 50);
        NSEWsys = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "CONSEW", 15);
        Perspl = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COPERS", 0.5f);
        FadeRange = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COFADE", 230);
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COMO3D", 0) == 0) Mode3D = false; else Mode3D = true;
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COLKNE", 0) == 0) LockNeed = false; else LockNeed = true;
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "CODET1", 1) == 0) Detail1 = false; else Detail1 = true;
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COSDLN", 1) == 0) ShowDigL = false; else ShowDigL = true;
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COSNLN", 1) == 0) ShowNorL = false; else ShowNorL = true;
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COSALN", 0) == 0) ShowAlML = false; else ShowAlML = true;
        if (!SceneMode) { M3D.isOn = Mode3D; OuSz.value = OutLSize; ln.isOn = LockNeed; NeSz.value = NeedSize; MaSz.value = MarkSize;
        if ((NSEWsys & 1) == 0) NSEWs[0].isOn = false; else NSEWs[0].isOn = true; if ((NSEWsys & 4) == 0) NSEWs[1].isOn = false; else NSEWs[1].isOn = true;
        if ((NSEWsys & 2) == 0) NSEWs[2].isOn = false; else NSEWs[2].isOn = true; if ((NSEWsys & 8) == 0) NSEWs[3].isOn = false; else NSEWs[3].isOn = true;
        Dt1.isOn = Detail1; SDL.isOn = ShowDigL; SNL.isOn = ShowNorL; Pers.value = Perspl; SAM.isOn = ShowAlML; FDR.value = FadeRange; }
        StartCoroutine(yesofcourse());
    }

    // Functie executata pentru a astepta un cadru si a actualiza interfata inainte de executarea logicii
    public IEnumerator yesofcourse() {
        yield return null;
        LoadMode = false;
    }

    // Functie executata cand o proprietate este schimbata despre element
    public void OnComponentCompModdify() {
        if(!SceneMode && !LoadMode) { Mode3D = M3D.isOn; OutLSize = (int)OuSz.value; LockNeed = ln.isOn; NeedSize = (int)NeSz.value; MarkSize = (int)MaSz.value; NSEWsys = 0;
        if (NSEWs[0].isOn) NSEWsys += 1; if (NSEWs[1].isOn) NSEWsys += 2; if (NSEWs[2].isOn) NSEWsys += 4; if (NSEWs[3].isOn) NSEWsys += 8;
            Detail1 = Dt1.isOn; ShowDigL = SDL.isOn; ShowNorL = SNL.isOn; Perspl = Pers.value; ShowAlML = SAM.isOn; FadeRange = (int)FDR.value; }
        if (!Mode3D) { Compass2D.SetActive(true); Compass3D.SetActive(false);
            if (!SceneMode) { content.sizeDelta = new Vector2(content.sizeDelta.x, 2500f); MenuPanels[0].gameObject.SetActive(false); }
            Compass2D.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(500f - OutLSize, 500f - OutLSize);
            Compass2D.transform.GetChild(8).GetComponent<RectTransform>().sizeDelta = new Vector2(NeedSize, 380f);
            Compass2D.transform.GetChild(4).GetComponent<Text>().fontSize = MarkSize; Compass2D.transform.GetChild(5).GetComponent<Text>().fontSize = MarkSize;
            Compass2D.transform.GetChild(6).GetComponent<Text>().fontSize = MarkSize; Compass2D.transform.GetChild(7).GetComponent<Text>().fontSize = MarkSize;
            Compass2D.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(444f - ((float)MarkSize * 1.4444f), 444f - ((float)MarkSize * 1.4444f));
            Compass2D.transform.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(304f - ((float)MarkSize * 1.4444f), 304f - ((float)MarkSize * 1.4444f));
            Compass2D.transform.GetChild(3).GetComponent<RectTransform>().sizeDelta = new Vector2(344f - ((float)MarkSize * 1.4444f), 344f - ((float)MarkSize * 1.4444f));
            Compass2D.transform.GetChild(4).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 248f - ((float)MarkSize * .55f));
            Compass2D.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(248f - ((float)MarkSize * .55f), 0f);
            Compass2D.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -248f + ((float)MarkSize * .55f));
            Compass2D.transform.GetChild(7).GetComponent<RectTransform>().anchoredPosition = new Vector2(-244f + ((float)MarkSize * .55f), 0f);
            if ((NSEWsys & 1) == 0) Compass2D.transform.GetChild(4).gameObject.SetActive(false); else Compass2D.transform.GetChild(4).gameObject.SetActive(true);
            if ((NSEWsys & 4) == 0) Compass2D.transform.GetChild(5).gameObject.SetActive(false); else Compass2D.transform.GetChild(5).gameObject.SetActive(true);
            if ((NSEWsys & 2) == 0) Compass2D.transform.GetChild(6).gameObject.SetActive(false); else Compass2D.transform.GetChild(6).gameObject.SetActive(true);
            if ((NSEWsys & 8) == 0) Compass2D.transform.GetChild(7).gameObject.SetActive(false); else Compass2D.transform.GetChild(7).gameObject.SetActive(true);
            if (Detail1) Compass2D.transform.GetChild(1).gameObject.SetActive(true); else Compass2D.transform.GetChild(1).gameObject.SetActive(false);
            if (ShowDigL) Compass2D.transform.GetChild(2).gameObject.SetActive(true); else Compass2D.transform.GetChild(2).gameObject.SetActive(false);
            if (ShowNorL) Compass2D.transform.GetChild(3).gameObject.SetActive(true); else Compass2D.transform.GetChild(3).gameObject.SetActive(false);
        } else { Compass3D.SetActive(true); Compass2D.SetActive(false); Compass3Dcont.transform.localScale = new Vector2(1f, Perspl);
            if (!SceneMode) { content.sizeDelta = new Vector2(content.sizeDelta.x, 3010f); MenuPanels[0].gameObject.SetActive(true); }
            Compass3D.transform.GetChild(5).localScale = new Vector2(1f, (1f / Perspl)); Compass3D.transform.GetChild(6).localScale = new Vector2(1f, (1f / Perspl));
            Compass3D.transform.GetChild(7).localScale = new Vector2(1f, (1f / Perspl)); Compass3D.transform.GetChild(8).localScale = new Vector2(1f, (1f / Perspl));
            Compass3D.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(500f - OutLSize, 500f - OutLSize);
            Compass3D.transform.GetChild(4).GetComponent<RectTransform>().sizeDelta = new Vector2(NeedSize, 380f);
            Compass3D.transform.GetChild(5).GetComponent<Text>().fontSize = MarkSize; Compass3D.transform.GetChild(6).GetComponent<Text>().fontSize = MarkSize;
            Compass3D.transform.GetChild(7).GetComponent<Text>().fontSize = MarkSize; Compass3D.transform.GetChild(8).GetComponent<Text>().fontSize = MarkSize;
            Compass3D.transform.GetChild(5).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 248f - ((float)MarkSize * .55f));
            Compass3D.transform.GetChild(6).GetComponent<RectTransform>().anchoredPosition = new Vector2(248f - ((float)MarkSize * .55f), 0f);
            Compass3D.transform.GetChild(7).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -248f + ((float)MarkSize * .55f));
            Compass3D.transform.GetChild(8).GetComponent<RectTransform>().anchoredPosition = new Vector2(-244f + ((float)MarkSize * .55f), 0f);
            if ((NSEWsys & 1) == 0) Compass3D.transform.GetChild(5).gameObject.SetActive(false); else Compass3D.transform.GetChild(5).gameObject.SetActive(true);
            if ((NSEWsys & 4) == 0) Compass3D.transform.GetChild(6).gameObject.SetActive(false); else Compass3D.transform.GetChild(6).gameObject.SetActive(true);
            if ((NSEWsys & 2) == 0) Compass3D.transform.GetChild(7).gameObject.SetActive(false); else Compass3D.transform.GetChild(7).gameObject.SetActive(true);
            if ((NSEWsys & 8) == 0) Compass3D.transform.GetChild(8).gameObject.SetActive(false); else Compass3D.transform.GetChild(8).gameObject.SetActive(true);
            if (Detail1) Compass3D.transform.GetChild(1).gameObject.SetActive(true); else Compass3D.transform.GetChild(1).gameObject.SetActive(false);
            if (ShowDigL) Compass3D.transform.GetChild(2).gameObject.SetActive(true); else Compass3D.transform.GetChild(2).gameObject.SetActive(false);
            if (ShowNorL) Compass3D.transform.GetChild(3).gameObject.SetActive(true); else Compass3D.transform.GetChild(3).gameObject.SetActive(false);
            if (ShowAlML) { Compass3D.transform.GetChild(5).GetComponent<Text>().color = MarkC; Compass3D.transform.GetChild(6).GetComponent<Text>().color = MarkC;
            Compass3D.transform.GetChild(7).GetComponent<Text>().color = MarkC; Compass3D.transform.GetChild(8).GetComponent<Text>().color = MarkC; }
        } if(!SceneMode && !LoadMode) { // Salvarea variabilelor
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COOUTS", OutLSize);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "CONEDS", NeedSize);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COMRKS", MarkSize);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "CONSEW", NSEWsys);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COPERS", Perspl);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COFADE", FadeRange);
            if (Mode3D) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COMO3D", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COMO3D", 0);
            if (LockNeed) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COLKNE", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COLKNE", 0);
            if (Detail1) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "CODET1", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "CODET1", 0);
            if (ShowDigL) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COSDLN", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COSDLN", 0);
            if (ShowNorL) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COSNLN", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COSNLN", 0);
            if (ShowAlML) PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COSALN", 1);
            else PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "COSALN", 0);
        }
    }

    // Functie executata cand o culoare trebuie schimbata
    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject);
    }

    // Functie executata cand o culoare este selectata si schimbata in meniul de culoare
    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        OutC = new Color(PlayerPrefs.GetFloat(colorstart + "COOUTLCLR", 0f), PlayerPrefs.GetFloat(colorstart + "COOUTLCLG", 0.025f), PlayerPrefs.GetFloat(colorstart + "COOUTLCLB", 0.25f));
        BackC = new Color(PlayerPrefs.GetFloat(colorstart + "COBACKCLR", 1f), PlayerPrefs.GetFloat(colorstart + "COBACKCLG", 1f), PlayerPrefs.GetFloat(colorstart + "COBACKCLB", 1f));
        NeFC = new Color(PlayerPrefs.GetFloat(colorstart + "CONEDFCLR", 1f), PlayerPrefs.GetFloat(colorstart + "CONEDFCLG", 0f), PlayerPrefs.GetFloat(colorstart + "CONEDFCLB", 0f));
        NeRC = new Color(PlayerPrefs.GetFloat(colorstart + "CONEDRCLR", 0f), PlayerPrefs.GetFloat(colorstart + "CONEDRCLG", 0f), PlayerPrefs.GetFloat(colorstart + "CONEDRCLB", 0f));
        MarkC = new Color(PlayerPrefs.GetFloat(colorstart + "COMARKCLR", 0f), PlayerPrefs.GetFloat(colorstart + "COMARKCLG", 0f), PlayerPrefs.GetFloat(colorstart + "COMARKCLB", 0f));
        DetC = new Color(PlayerPrefs.GetFloat(colorstart + "CODETACLR", 0.82f), PlayerPrefs.GetFloat(colorstart + "CODETACLG", 0.82f), PlayerPrefs.GetFloat(colorstart + "CODETACLB", 0.82f));
        if (Mode3D) { Compass3D.GetComponent<Image>().color = OutC; Compass3D.transform.GetChild(0).GetComponent<Image>().color = BackC;
            Compass3D.transform.GetChild(4).GetComponent<RawImage>().color = BackC; Compass3D.transform.GetChild(4).GetChild(0).GetComponent<RawImage>().color = NeFC;
            Compass3D.transform.GetChild(4).GetChild(1).GetComponent<RawImage>().color = NeRC; Compass3D.transform.GetChild(5).GetComponent<Text>().color = MarkC;
            Compass3D.transform.GetChild(6).GetComponent<Text>().color = MarkC; Compass3D.transform.GetChild(7).GetComponent<Text>().color = MarkC;
            Compass3D.transform.GetChild(8).GetComponent<Text>().color = MarkC; Compass3D.transform.GetChild(1).GetComponent<Image>().color = DetC;
            Compass3D.transform.GetChild(2).GetComponent<Image>().color = DetC; Compass3D.transform.GetChild(3).GetComponent<Image>().color = DetC; }
        else { Compass2D.GetComponent<Image>().color = OutC; Compass2D.transform.GetChild(0).GetComponent<Image>().color = BackC;
            Compass2D.transform.GetChild(8).GetComponent<RawImage>().color = BackC; Compass2D.transform.GetChild(8).GetChild(0).GetComponent<RawImage>().color = NeFC;
            Compass2D.transform.GetChild(8).GetChild(1).GetComponent<RawImage>().color = NeRC; Compass2D.transform.GetChild(4).GetComponent<Text>().color = MarkC;
            Compass2D.transform.GetChild(5).GetComponent<Text>().color = MarkC; Compass2D.transform.GetChild(6).GetComponent<Text>().color = MarkC;
            Compass2D.transform.GetChild(7).GetComponent<Text>().color = MarkC; Compass2D.transform.GetChild(1).GetComponent<Image>().color = DetC;
            Compass2D.transform.GetChild(2).GetComponent<Image>().color = DetC; Compass2D.transform.GetChild(3).GetComponent<Image>().color = DetC; }
        if (!SceneMode) { OuC.color = OutC; BaC.color = BackC; NFC.color = NeFC; NRC.color = NeRC; MineC.color = MarkC; DC.color = DetC; }
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

    // Functie chemata de 60 de ori / secunda | Functie folosita pentru a actualiza elementul cand este folosit
    void FixedUpdate() {
        if(Mode3D) { float angle = 0f; if (SceneMode) angle = SM.CompOrient;
            if (SceneMode) if (LockNeed) { Compass3D.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            Compass3D.transform.GetChild(5).transform.rotation = Quaternion.Euler(0f, 0f, 0f); Compass3D.transform.GetChild(6).transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Compass3D.transform.GetChild(7).transform.rotation = Quaternion.Euler(0f, 0f, 0f); Compass3D.transform.GetChild(8).transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    Compass3D.transform.GetChild(4).transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                } else Compass3D.transform.GetChild(4).transform.rotation = Quaternion.Euler(0f, 0f, -angle);
            if(!ShowAlML) { float FadeR = Mathf.Abs(Mathf.DeltaAngle(angle, 0f)); float FadeR2 = Mathf.Abs(Mathf.DeltaAngle(angle, 90f));
            float FadeR3 = Mathf.Abs(Mathf.DeltaAngle(angle, 180f)); float FadeR4 = Mathf.Abs(Mathf.DeltaAngle(angle, 270f));
            if (FadeR >= ((FadeRange / 2f) - 25f)) Compass3D.transform.GetChild(5).GetComponent<Text>().color = new Color(MarkC.r, MarkC.g, MarkC.b, 1f - ((FadeR - ((FadeRange / 2f) - 25f)) / 25f));
            else Compass3D.transform.GetChild(5).GetComponent<Text>().color = MarkC;
            if (FadeR2 >= ((FadeRange / 2f) - 25f)) Compass3D.transform.GetChild(6).GetComponent<Text>().color = new Color(MarkC.r, MarkC.g, MarkC.b, 1f - ((FadeR2 - ((FadeRange / 2f) - 25f)) / 25f));
            else Compass3D.transform.GetChild(6).GetComponent<Text>().color = MarkC;
            if (FadeR3 >= ((FadeRange / 2f) - 25f)) Compass3D.transform.GetChild(7).GetComponent<Text>().color = new Color(MarkC.r, MarkC.g, MarkC.b, 1f - ((FadeR3 - ((FadeRange / 2f) - 25f)) / 25f));
            else Compass3D.transform.GetChild(7).GetComponent<Text>().color = MarkC;
            if (FadeR4 >= ((FadeRange / 2f) - 25f)) Compass3D.transform.GetChild(8).GetComponent<Text>().color = new Color(MarkC.r, MarkC.g, MarkC.b, 1f - ((FadeR4 - ((FadeRange / 2f) - 25f)) / 25f));
            else Compass3D.transform.GetChild(8).GetComponent<Text>().color = MarkC; }
        } else { float angle = 0f; if (SceneMode) angle = SM.CompOrient;
            if (SceneMode) if (LockNeed) { Compass2D.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                Compass2D.transform.GetChild(4).transform.rotation = Quaternion.Euler(0f, 0f, 0f); Compass2D.transform.GetChild(5).transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                Compass2D.transform.GetChild(6).transform.rotation = Quaternion.Euler(0f, 0f, 0f); Compass2D.transform.GetChild(7).transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                Compass2D.transform.GetChild(8).transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            } else Compass2D.transform.GetChild(8).transform.rotation = Quaternion.Euler(0f, 0f, -angle);
        }
    }
}
