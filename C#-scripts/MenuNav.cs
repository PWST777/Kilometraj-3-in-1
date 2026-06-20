using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuNav : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Navigare Meniu
    // Folosita pentru a controla interfata meniului principal al aplicatiei
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    // Elemente de interfata
    public GameObject AddButton;
    public GameObject PresetButton;
    public Transform orgfel;
    public InputField NameField;
    public GameObject OpenPanel;
    public int PresetSelectr = 0;
    public Sprite cranberry;
    public List<Sprite> templates;
    public GameObject InitialSetup;
    public GameObject TutorialSetup;
    public List<Image> ModesB;
    public List<Sprite> ModesBTex;
    public int OptionSelected;
    public List<Image> LModesB;
    public int LangOptionSelected;
    public GameObject GPSStart;
    public SPEEDMANAGER SM;
    public GameObject LOAD;
    public int Status;
    public int coold;
    public GameObject SelctButton;
    public RectTransform can;
    public float unitlenght;
    public GameObject colorschem;
    public Image clr1; public Image clr2;
    public Image clr3; public Image clr4;
    public List<Sprite> allms;

    public TutorialHints touhou;
    public ColorPicker cp;

    // Functie executata cand elementul este activat pentru prima data 
    void Start() {
        unitlenght = (can.rect.width - 120f) / 3f;
        PresetButton.GetComponent<RectTransform>().sizeDelta = new Vector2(unitlenght, 400f);
        AddButton.GetComponent<RectTransform>().sizeDelta = new Vector2(unitlenght, 400f);
        if (PlayerPrefs.GetInt("INSETUP", 0) == 0) { InitialSetup.SetActive(true);
        if (PlayerPrefs.GetInt("INSETUPLANG", 0) == 1) { InitialSetup.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
        InitialSetup.transform.GetChild(0).GetChild(1).gameObject.SetActive(true); } }
        else if (PlayerPrefs.GetInt("TUTORIALYES", 0) == 0) TutorialSetup.SetActive(true);
        else if (PlayerPrefs.GetInt("SPEEDSOURCE") == 0) { SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>(); GPSStart.SetActive(true); SM.InitGPS(); }
        StartCoroutine(LOADShow(0)); if (PlayerPrefs.GetInt("TUTORIALYES", 0) == 1) Initial();
    }

    // Functie ce arata indiciu daca aplicatia a fost deschisa pentru prima data
    public void Initial() {
        if (PlayerPrefs.GetInt("HASHINT0", 0) == 0) { 
            if(PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(0f, 0f), "Welcome", "To get you started, to use this app you must create presets that you can use later on, to create one, click the green + button", 700f, 0.3f, 0);
            if(PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(0f, 0f), "Bienvenue", "Pour commencer, afin d'utiliser cette application, vous devez créer des préréglages que vous pourrez utiliser ultérieurement. Pour en créer un, cliquez sur le bouton vert +", 700f, 0.3f, 0);
            if(PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(0f, 0f), "Willkommen", "Um mit dieser App zu beginnen, müssen Sie zunächst Voreinstellungen erstellen, die Sie später verwenden können. Klicken Sie dazu auf die grüne Schaltfläche „+“", 700f, 0.3f, 0);
            if(PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(0f, 0f), "Bun venit", "Pentru a începe, pentru a utiliza această aplicație trebuie să creați presetări pe care le puteți folosi ulterior. Pentru a crea una, faceți clic pe butonul verde +", 700f, 0.3f, 0);
        }
    }

    // Functie ce este executata cand presetarea selectata este schimbata
    public void OnChangeOfPresets() {
        for(int v = 0; v < orgfel.childCount; v++) {
            if(orgfel.GetChild(v).gameObject.name != "Add Button") Destroy(orgfel.GetChild(v).gameObject);
        } int Presn = 0; int UsedPresnm = 0;
        while (true) {
            if(PlayerPrefs.GetInt("PRES" + Presn + "EXISTS") == 1) {
                GameObject NewPresn = Instantiate(PresetButton, orgfel); NewPresn.SetActive(true);
                NewPresn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-unitlenght + (unitlenght * (UsedPresnm % 3)), -20f - (400f * (UsedPresnm / 3)));
                NewPresn.transform.GetChild(2).GetComponent<Text>().text = PlayerPrefs.GetString("PRES" + Presn + "NAME");
                StartCoroutine(LDImg(PlayerPrefs.GetString("PRESET" + Presn + "IMAGEPATH", ":/")));
                NewPresn.transform.GetChild(0).GetComponent<Image>().sprite = cranberry;
                if (cranberry == null) NewPresn.transform.GetChild(0).GetComponent<Image>().color = Color.grey;
                else NewPresn.transform.GetChild(0).GetComponent<Image>().color = Color.white;
                int flambda = Presn; NewPresn.GetComponent<Button>().onClick.AddListener(() => OnButtonPresetClick(flambda, NewPresn));
                NewPresn.name = UsedPresnm + ""; UsedPresnm++; }
            else if (PlayerPrefs.GetInt("PRES" + Presn + "EXISTS") == 0) break; Presn++;
        } AddButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-unitlenght + (unitlenght * (UsedPresnm % 3)), -20f - (400f * (UsedPresnm / 3)));
        orgfel.GetComponent<RectTransform>().sizeDelta = new Vector2(orgfel.GetComponent<RectTransform>().sizeDelta.x, 440f + (400f * (UsedPresnm / 3)));
        orgfel.parent.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(unitlenght * 3f, 830f);
        LOAD.SetActive(false);
    }

    // Functie executata cand o presetare noua este creata
    public void OnPresetCreate() {
        StartCoroutine(LOADShow(2));
    }

    // Functie propriu-zisa executata cand o presetare noua este creata
    public void ActualCreation(bool copy) {
        PlayerPrefs.SetInt("PRES" + PlayerPrefs.GetInt("TOTALPRESETS") + "EXISTS", 1);
        PlayerPrefs.SetString("PRES" + PlayerPrefs.GetInt("TOTALPRESETS") + "NAME", NameField.text);
        // Daca un template este selectat, toate datele necesare sunt incarcate aici
        if (PresetSelectr == 1) {
            #region LCD Template 1
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCR", PlayerPrefs.GetFloat("P1CKEY1R", 0.55f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCG", PlayerPrefs.GetFloat("P1CKEY1G", 0.62f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCB", PlayerPrefs.GetFloat("P1CKEY1B", 0.3f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0TYPE", 7);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSX", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSY", -2600f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASESIZE", 8f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEROT", -161.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDCSBREFR", 4);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDCSBBRNT", 0.08f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDCSBCBRN", 0.07f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDCSBTOPS", 40);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDCSBWIDT", 0.105f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDCSBSEGG", 0.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDCSBHEIG", 56f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDCSBDGITCLR", PlayerPrefs.GetFloat("P1CKEY2R", 0.13f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDCSBDGITCLG", PlayerPrefs.GetFloat("P1CKEY2G", 0.13f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDCSBDGITCLB", PlayerPrefs.GetFloat("P1CKEY2B", 0.13f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDCSBBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1TYPE", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSX", 45f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSY", 5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASESIZE", 2.05f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSREFR", 4);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSBRNT", 0.08f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSCBRN", 0.07f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSDGITCLR", PlayerPrefs.GetFloat("P1CKEY2R", 0.13f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSDGITCLG", PlayerPrefs.GetFloat("P1CKEY2G", 0.13f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSDGITCLB", PlayerPrefs.GetFloat("P1CKEY2B", 0.13f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2TYPE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASEPOSX", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASEPOSY", -330f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASESIZE", 11f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASEROT", 90f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LNTHIC", 1.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LNLINECLR", PlayerPrefs.GetFloat("P1CKEY2R", 0.13f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LNLINECLG", PlayerPrefs.GetFloat("P1CKEY2G", 0.13f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LNLINECLB", PlayerPrefs.GetFloat("P1CKEY2B", 0.13f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3TYPE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEPOSX", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEPOSY", -728f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASESIZE", 4f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEROT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LNTHIC", 3.3f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LNLINECLR", PlayerPrefs.GetFloat("P1CKEY2R", 0.13f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LNLINECLG", PlayerPrefs.GetFloat("P1CKEY2G", 0.13f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LNLINECLB", PlayerPrefs.GetFloat("P1CKEY2B", 0.13f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASEPOSX", -486f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASEPOSY", -437f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASESIZE", 0.69f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDMODE", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDNSE", 7);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDCML", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDREFR", 4);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDBRNT", 0.08f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDCBRN", 0.07f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDGITCLR", PlayerPrefs.GetFloat("P1CKEY2R", 0.13f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDGITCLG", PlayerPrefs.GetFloat("P1CKEY2G", 0.13f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDGITCLB", PlayerPrefs.GetFloat("P1CKEY2B", 0.13f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEPOSX", 486f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEPOSY", -437f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASESIZE", 0.69f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDMODE", 5);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDNSE", 6);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDCML", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDREFR", 4);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDBRNT", 0.08f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDCBRN", 0.07f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDGITCLR", PlayerPrefs.GetFloat("P1CKEY2R", 0.13f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDGITCLG", PlayerPrefs.GetFloat("P1CKEY2G", 0.13f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDGITCLB", PlayerPrefs.GetFloat("P1CKEY2B", 0.13f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "TOTALOBJ", 6);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "IMAGEPATH", "TLCD1");
            #endregion
        } if (PresetSelectr == 2) {
            #region LCD Template 2
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCR", PlayerPrefs.GetFloat("P2CKEY1R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCG", PlayerPrefs.GetFloat("P2CKEY1G", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCB", PlayerPrefs.GetFloat("P2CKEY1B", 0));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0TYPE", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSX", 320f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSY", 130f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASESIZE", 2.3f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSREFR", 60);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSDGITCLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSDGITCLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSDGITCLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSX", 320f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSY", -365f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASESIZE", 0.93f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LDMODE", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LDDNSE", 7);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LDDCML", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LDREFR", 60);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LDCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LDDGITCLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LDDGITCLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LDDGITCLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASEPOSX", -650f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASEPOSY", 450f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASESIZE", 0.6f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDMODE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDREFR", 60);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDDGITCLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDDGITCLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDDGITCLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEPOSX", -650f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEPOSY", 170f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASESIZE", 0.75f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDMODE", 7);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDDNSE", 4);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDDCML", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDREFR", 60);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDDGITCLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDDGITCLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDDGITCLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TYPE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASEPOSX", -340f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASEPOSY", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASESIZE", 6f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LNTHIC", 1.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LNLINECLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LNLINECLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LNLINECLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TYPE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEPOSX", -650f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEPOSY", 350f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASESIZE", 3.15f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEROT", 90f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LNTHIC", 3f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LNLINECLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LNLINECLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LNLINECLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BASEPOSX", -650f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BASEPOSY", -25f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BASESIZE", 0.7f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDMODE", 9);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDDNSE", 4);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDDCML", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDREFR", 60);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDDGITCLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDDGITCLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDDGITCLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TYPE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BASEPOSX", -650f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BASEPOSY", -137f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BASESIZE", 3.15f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BASEROT", 90f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LNTHIC", 3f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LNLINECLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LNLINECLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LNLINECLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8BASEPOSX", -650f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8BASEPOSY", -305f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8BASESIZE", 0.6f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDMODE", 5);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDNSE", 4);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDCML", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDREFR", 60);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDGITCLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDGITCLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDGITCLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9BASEPOSX", -480f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9BASEPOSY", -455f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9BASESIZE", 0.4f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDMODE", 12);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDNSE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDCML", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDREFR", 60);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDGITCLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDGITCLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDGITCLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10BASEPOSX", -815f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10BASEPOSY", -455f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10BASESIZE", 0.4f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDMODE", 11);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDDNSE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDDCML", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDREFR", 60);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDDGITCLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDDGITCLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDDGITCLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TYPE", 9);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11BASEPOSX", -510f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11BASEPOSY", -183f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11BASESIZE", 0.43f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXWIDT", 950);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXHEIG", 130);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXTEXTCLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXTEXTCLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXTEXTCLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXKERN", 0f);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXTEXT", "Total Time");
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TYPE", 9);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12BASEPOSX", -413f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12BASEPOSY", 305f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12BASESIZE", 0.43f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXWIDT", 550);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXHEIG", 130);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXTEXTCLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXTEXTCLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXTEXTCLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXKERN", 0f);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXTEXT", "Trip");
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM13TYPE", 9);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM13BASEPOSX", -648f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM13BASEPOSY", -455f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM13BASESIZE", 0.25f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM13TXWIDT", 340);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM13TXHEIG", 130);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM13TXTEXTCLR", PlayerPrefs.GetFloat("P2CKEY2R", 0));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM13TXTEXTCLG", PlayerPrefs.GetFloat("P2CKEY2G", 0.9f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM13TXTEXTCLB", PlayerPrefs.GetFloat("P2CKEY2B", 1));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM13TXKERN", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM13TXLNSP", -20f);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM13TXTEXT", "Max\nAvg");
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "TOTALOBJ", 14);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "IMAGEPATH", "TLCD2");
            #endregion
        } if (PresetSelectr == 3) {
            #region LCD Template 3
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCR", PlayerPrefs.GetFloat("P3CKEY1R", 1));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCG", PlayerPrefs.GetFloat("P3CKEY1G", 1));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCB", PlayerPrefs.GetFloat("P3CKEY1B", 0.75f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0TYPE", 6);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSX", -950f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSY", 460f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASESIZE", 1.36f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDSBHEIG", 100);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDSBWIDT", 1390);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDSBREFR", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDSBBRNT", 0.12f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDSBCBRN", 0.25f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDSBSEGC", 30);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDSBTOPS", 130);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDSBDGITCLR", PlayerPrefs.GetFloat("P3CKEY2R", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDSBDGITCLG", PlayerPrefs.GetFloat("P3CKEY2G", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDSBDGITCLB", PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LDSBBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1TYPE", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSX", -608f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSY", 130f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASESIZE", 1.55f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSREFR", 16);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSDNSE", 3);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSMLD1", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSMTPR", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSSTRX", 0.89f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSBRNT", 0.04f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSCBRN", 0.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSDGITCLR", PlayerPrefs.GetFloat("P3CKEY2R", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSDGITCLG", PlayerPrefs.GetFloat("P3CKEY2G", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSDGITCLB", PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1LSBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASEPOSX", -510f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASEPOSY", -225f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASESIZE", 0.8f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDREFR", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDBRNT", 0.12f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDCBRN", 0.25f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDMODE", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDDNSE", 6);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDDCML", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDSFNT", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDDGITCLR", PlayerPrefs.GetFloat("P3CKEY2R", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDDGITCLG", PlayerPrefs.GetFloat("P3CKEY2G", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDDGITCLB", PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEPOSX", -510f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEPOSY", -412f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASESIZE", 0.76f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDREFR", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDBRNT", 0.12f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCBRN", 0.25f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDMODE", 5);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDDNSE", 5);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDDCML", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDSFNT", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDDGITCLR", PlayerPrefs.GetFloat("P3CKEY2R", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDDGITCLG", PlayerPrefs.GetFloat("P3CKEY2G", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDDGITCLB", PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TYPE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASEPOSX", -50f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASEPOSY", -130f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASESIZE", 5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LNTHIC", 4f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LNLINECLR", PlayerPrefs.GetFloat("P3CKEY2R", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LNLINECLG", PlayerPrefs.GetFloat("P3CKEY2G", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LNLINECLB", PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEPOSX", 460f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEPOSY", 300f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASESIZE", 0.65f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDREFR", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDBRNT", 0.12f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDCBRN", 0.25f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDMODE", 7);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDNSE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDCML", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDSFNT", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDGITCLR", PlayerPrefs.GetFloat("P3CKEY2R", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDGITCLG", PlayerPrefs.GetFloat("P3CKEY2G", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDGITCLB", PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BASEPOSX", 460f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BASEPOSY", 145f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BASESIZE", 0.65f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDREFR", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDBRNT", 0.12f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDCBRN", 0.25f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDMODE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDDNSE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDDCML", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDSFNT", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDDGITCLR", PlayerPrefs.GetFloat("P3CKEY2R", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDDGITCLG", PlayerPrefs.GetFloat("P3CKEY2G", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDDGITCLB", PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BASEPOSX", 460f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BASEPOSY", -60f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BASESIZE", 0.65f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDREFR", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDBRNT", 0.12f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDCBRN", 0.25f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDMODE", 11);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDDNSE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDDCML", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDSFNT", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDDGITCLR", PlayerPrefs.GetFloat("P3CKEY2R", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDDGITCLG", PlayerPrefs.GetFloat("P3CKEY2G", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDDGITCLB", PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8BASEPOSX", 460f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8BASEPOSY", -215f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8BASESIZE", 0.65f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDREFR", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDBRNT", 0.12f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDCBRN", 0.25f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDMODE", 12);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDNSE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDCML", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDSFNT", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDGITCLR", PlayerPrefs.GetFloat("P3CKEY2R", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDGITCLG", PlayerPrefs.GetFloat("P3CKEY2G", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDGITCLB", PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9BASEPOSX", 460f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9BASEPOSY", -430f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9BASESIZE", 0.65f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDREFR", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDBRNT", 0.12f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDCBRN", 0.25f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDMODE", 6);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDNSE", 6);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDCML", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDSFNT", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDGITCLR", PlayerPrefs.GetFloat("P3CKEY2R", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDGITCLG", PlayerPrefs.GetFloat("P3CKEY2G", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDGITCLB", PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDBACK", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10TYPE", 9);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10BASEPOSX", 204f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10BASEPOSY", 280f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10BASESIZE", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10TXWIDT", 580);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10TXHEIG", 280);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10TXTEXTCLR", PlayerPrefs.GetFloat("P3CKEY2R", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10TXTEXTCLG", PlayerPrefs.GetFloat("P3CKEY2G", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10TXTEXTCLB", PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10TXKERN", -1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10TXFTSZ", 200);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10TXFONTF", 2);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10TXFONT", 13);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10TXTEXT", "Trip");
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TYPE", 9);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11BASEPOSX", 145f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11BASEPOSY", -60f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11BASESIZE", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXWIDT", 430);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXHEIG", 280);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXTEXTCLR", PlayerPrefs.GetFloat("P3CKEY2R", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXTEXTCLG", PlayerPrefs.GetFloat("P3CKEY2G", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXTEXTCLB", PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXKERN", -1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXFTSZ", 200);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXFONTF", 2);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXFONT", 13);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TXTEXT", "Max");
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TYPE", 9);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12BASEPOSX", 145f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12BASEPOSY", -235f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12BASESIZE", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXWIDT", 430);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXHEIG", 280);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXTEXTCLR", PlayerPrefs.GetFloat("P3CKEY2R", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXTEXTCLG", PlayerPrefs.GetFloat("P3CKEY2G", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXTEXTCLB", PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXKERN", -1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXFTSZ", 200);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXFONTF", 2);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXFONT", 13);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM12TXTEXT", "Avg");
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "TOTALOBJ", 13);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "IMAGEPATH", "TLCD3");
            #endregion
        } if (PresetSelectr == 4) {
            #region Combined Template
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKMODE", 12);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKMANG", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG1R", 0.8f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG1G", 0.8f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG1B", 0.8f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG2R", 0.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG2G", 0.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG2B", 0.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG3R", 0.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG3G", 0.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG3B", 0.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG4R", 0.8f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG4G", 0.8f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG4B", 0.8f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0TYPE", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSX", -415f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSY", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASESIZE", 1.05f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASSFAM", 0.145f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASEFAM", 0.855f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMSIF", 180f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASC2IF", 150f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMRIF", 10f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASFDAM", 0.737f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR1CLR", 0.75f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR1CLG", 0.75f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR1CLB", 0.75f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR2CLR", 0.75f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR2CLG", 0.35f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR2CLB", 0.35f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1TYPE", 10);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSX", -415f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSY", -350f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASESIZE", 1f);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICIMAGE", "PREMADES0");
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICWIDT", 460);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICHEIG", 225);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICLDMD", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICSPLC", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICIMGCLR", 0.35f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICIMGCLG", 0.35f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICIMGCLB", 0.35f);
            string st = "PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICSPLC";
            PlayerPrefs.SetInt(st + "1", 20); PlayerPrefs.SetInt(st + "2", 20); PlayerPrefs.SetInt(st + "3", 20); PlayerPrefs.SetInt(st + "4", 20);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICIMSZ", 0.6f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2TYPE", 10);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASEPOSX", 535f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASEPOSY", 215f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASESIZE", 1f);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICIMAGE", "PREMADES0");
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICWIDT", 810);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICHEIG", 600);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICLDMD", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICSPLC", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICIMGCLR", 0.6f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICIMGCLG", 0.6f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICIMGCLB", 0.6f);
            string st2 = "PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICSPLC";
            PlayerPrefs.SetInt(st2 + "1", 20); PlayerPrefs.SetInt(st2 + "2", 20); PlayerPrefs.SetInt(st2 + "3", 20); PlayerPrefs.SetInt(st2 + "4", 20);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICIMSZ", 0.5f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3TYPE", 10);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEPOSX", 535f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEPOSY", -305f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASESIZE", 1f);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICIMAGE", "PREMADES1");
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICWIDT", 810);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICHEIG", 400);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICLDMD", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICSPLC", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICIMGCLR", 0.45f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICIMGCLG", 0.45f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICIMGCLB", 0.45f);
            string st3 = "PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICSPLC";
            PlayerPrefs.SetInt(st3 + "1", 16); PlayerPrefs.SetInt(st3 + "2", 16); PlayerPrefs.SetInt(st3 + "3", 16); PlayerPrefs.SetInt(st3 + "4", 16);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICIMSZ", 0.42f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TYPE", 9);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASEPOSX", -400f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASEPOSY", -310f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASESIZE", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXWIDT", 390);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXHEIG", 50);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXTEXTCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXTEXTCLG", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXTEXTCLB", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXKERN", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXFTSZ", 90);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXFONTF", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXFONT", 5);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXALMD", 5);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXDYNA", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXDYNAC", 7);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXMXDE", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXMXDI", 5);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TXRRAT", 5);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TMPMEFFECT", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BTXTCOLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BTXTCOLG", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BTXTCOLB", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TMPMBDILAT", 0.05f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TMPMBSOFTN", 0.32f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TYPE", 9);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEPOSX", -403f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEPOSY", -403f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASESIZE", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXWIDT", 390);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXHEIG", 50);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXTEXTCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXTEXTCLG", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXTEXTCLB", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXKERN", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXFTSZ", 65);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXFONTF", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXFONT", 5);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXALMD", 5);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXDYNA", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXDYNAC", 2);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXMXDE", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXMXDI", 7);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TXRRAT", 5);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TMPMEFFECT", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BTXTCOLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BTXTCOLG", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BTXTCOLB", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TMPMBDILAT", 0.08f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TMPMBSOFTN", 0.5f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TYPE", 9);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BASEPOSX", 460f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BASEPOSY", 185f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BASESIZE", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXWIDT", 930);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXHEIG", 50);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXTEXTCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXTEXTCLG", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXTEXTCLB", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXKERN", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXFTSZ", 320);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXFONTF", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXFONT", 11);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXALMD", 8);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXDYNA", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXDYNAC", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXMXDE", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXMXDI", 3);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TXRRAT", 8);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TMPMEFFECT", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TMPMBCOLMO", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TMPMBGRTYP", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BTXTG41R", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BTXTG41G", 0.15f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BTXTG41B", 0.15f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BTXTG42R", 0.72f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BTXTG42G", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BTXTG42B", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BTXTG43R", 0.72f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BTXTG43G", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BTXTG43B", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BTXTG44R", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BTXTG44G", 0.15f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BTXTG44B", 0.15f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TMPMBDILAT", 0.012f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TMPMBSOFTN", 0.1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TYPE", 9);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BASEPOSX", 450f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BASEPOSY", -25f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BASESIZE", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXWIDT", 930);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXHEIG", 50);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXTEXTCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXTEXTCLG", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXTEXTCLB", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXKERN", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXFTSZ", 180);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXFONTF", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXFONT", 11);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXALMD", 8);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXDYNA", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXDYNAC", 8);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXMXDE", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXMXDI", 5);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TXRRAT", 5);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TMPMEFFECT", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TMPMBCOLMO", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TMPMBGRTYP", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BTXTG41R", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BTXTG41G", 0.15f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BTXTG41B", 0.15f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BTXTG42R", 0.72f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BTXTG42G", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BTXTG42B", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BTXTG43R", 0.72f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BTXTG43G", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BTXTG43B", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BTXTG44R", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BTXTG44G", 0.15f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BTXTG44B", 0.15f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TMPMBDILAT", 0.02f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TMPMBSOFTN", 0.17f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8BASEPOSX", 535f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8BASEPOSY", -390f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8BASESIZE", 0.6f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDREFR", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDBRNT", 0.12f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDCBRN", 0.04f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDMODE", 6);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDNSE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDCML", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDSFNT", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDGITCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDGITCLG", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDGITCLB", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDBACKCLR", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDBACKCLG", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDBACKCLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9BASEPOSX", 535f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9BASEPOSY", -215f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9BASESIZE", 0.6f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDREFR", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDBRNT", 0.12f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDCBRN", 0.04f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDMODE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDNSE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDCML", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDSFNT", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDGITCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDGITCLG", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDGITCLB", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDBACKCLR", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDBACKCLG", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDBACKCLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "TOTALOBJ", 10);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "IMAGEPATH", "TLCOM");
            #endregion
        } if (PresetSelectr == 5) {
            #region Analog Template
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCR", 0.12f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCG", 0.18f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCB", 0.32f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0TYPE", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSX", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSY", -175f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASESIZE", 1.4f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASSFAM", 0.22f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASEFAM", 0.78f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMSIF", 261f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASC2IF", 180f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMRIF", 20f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMRIF", 20f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASILIF", 2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASFDAM", 0.592f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASLABL", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR1CLR", 0.75f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR1CLG", 0.75f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR1CLB", 0.75f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR2CLR", 0.75f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR2CLG", 0.35f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR2CLB", 0.35f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1TYPE", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSX", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSY", -423f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASESIZE", 1.05f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ADDNSE", 6);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ADREFR", 50);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "TOTALOBJ", 2);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "IMAGEPATH", "TLANL");
            #endregion
        } if (PresetSelectr == 6) {
            #region Combined Template 2
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKMODE", 2);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BKIMGICLDMD", 1);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BKIMGICIMAGE", "IMGWOW0");
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0TYPE", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSX", 62f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSY", 285f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASESIZE", 1.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSDNSE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSREFR", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSCBRN", 0.02f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSDGITCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSDGITCLG", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSDGITCLB", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSBACK", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSSTRY", 0.5f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0LSFONT", 20);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1TYPE", 10);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSX", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSY", -180f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASESIZE", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICWIDT", 1510);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICHEIG", 620);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICIMGCLR", 0.6f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICIMGCLG", 0.6f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ICIMGCLB", 0.6f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2TYPE", 10);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASEPOSX", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASEPOSY", -180f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASESIZE", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICWIDT", 1500);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICHEIG", 610);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICIMGCLR", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICIMGCLG", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ICIMGCLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3TYPE", 7);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEPOSX", -440f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEPOSY", -180f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASESIZE", 0.7f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEROT", 32f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCSBHEIG", 60f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCSBWIDT", 0.69f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCSBBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCSBCBRN", 0.025f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCSBISHD", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCSBSEGC", 24);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCSBSEGG", 3f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCSBSPUN", 3);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCSBTOPS", 60);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCSBBACK", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCSBDGITCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCSBDGITCLG", 0.45f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3LDCSBDGITCLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASEPOSX", -425f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASEPOSY", -177f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASESIZE", 1.15f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDMODE", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDNSE", 4);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDCML", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDREFR", 5);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDGITCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDGITCLG", 0.45f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDGITCLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDBACK", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDSTRX", 0.5f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDFONT", 0);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TYPE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEPOSX", -160f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEPOSY", -397f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASESIZE", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEROT", -45f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LNTHIC", 17.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LNLINECLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LNLINECLG", 0.45f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LNLINECLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6TYPE", 8);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BASEPOSX", 327f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BASEPOSY", -329f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BASESIZE", 4.22f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6BASEROT", 90f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LNTHIC", 4.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LNLINECLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LNLINECLG", 0.45f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM6LNLINECLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BASEPOSX", 585f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BASEPOSY", -410f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7BASESIZE", 0.5f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDMODE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDDNSE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDDCML", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDREFR", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDDGITCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDDGITCLG", 0.45f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDDGITCLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDBACK", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM7LDSTRX", 0.75f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8BASEPOSX", 115f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8BASEPOSY", -410f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8BASESIZE", 0.5f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDMODE", 6);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDNSE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDCML", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDREFR", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDGITCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDGITCLG", 0.45f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDDGITCLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDBACK", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM8LDSTRX", 0.75f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9BASEPOSX", 322f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9BASEPOSY", -237f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9BASESIZE", 0.62f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDMODE", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDNSE", 7);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDCML", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDREFR", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDGITCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDGITCLG", 0.45f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDDGITCLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDBACK", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM9LDSTRY", 0.8f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10BASEPOSX", 320f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10BASEPOSY", -98f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10BASESIZE", 0.6f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDMODE", 4);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDDNSE", 7);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDDCML", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDREFR", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDDGITCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDDGITCLG", 0.45f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDDGITCLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDBACK", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM10LDSTRY", 0.835f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11BASEPOSX", 443f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11BASEPOSY", 41f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11BASESIZE", 0.62f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11LDMODE", 7);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11LDDNSE", 5);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11LDDCML", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11LDREFR", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11LDCBRN", 0.03f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11LDDGITCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11LDDGITCLG", 0.45f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11LDDGITCLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11LDBACK", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM11LDSTRY", 0.8f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "TOTALOBJ", 12);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "IMAGEPATH", "TLCM2");
            #endregion
        } if (PresetSelectr == 7) {
            #region OBD-II Template
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKMODE", 12);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKMANG", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG1R", 0.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG1G", 0.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG1B", 0.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG2R", 0.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG2G", 0.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG2B", 0.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG3R", 0.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG3G", 0.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG3B", 0.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG4R", 0.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG4G", 0.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BCG4B", 0.5f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0TYPE", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSX", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASEPOSY", 150f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0BASESIZE", 0.7f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASSPMD", 6);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASSFAM", 0.38f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASEFAM", 0.62f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMSIF", 130f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASC2IF", 140f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMRIF", 20f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASFDAM", 0.58f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASTMAS", 1.15f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASBSAM", 180f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASNWAM", 100f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASNWAM", 100f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASELAM", 115f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASLASI", 75f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR1CLR", 0.05f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR1CLG", 0.05f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR1CLB", 0.05f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR2CLR", 0.75f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR2CLG", 0.35f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASMAR2CLB", 0.35f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASBACKCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASBACKCLG", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASBACKCLB", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASC1CLLIR", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASC1CLLIG", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASC1CLLIB", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASC2CLLIR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASC2CLLIG", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASC2CLLIB", 0.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASNEEDCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASNEEDCLG", 0.25f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASNEEDCLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM0ASFONT", 2);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1TYPE", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSX", -500f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASEPOSY", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1BASESIZE", 0.9f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASSPMD", 4);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASSFAM", 0.1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASEFAM", 0.757f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASMSIF", 7f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASC2IF", 8f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASMRIF", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASFDAM", 0.6f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASMAR1CLR", 0.05f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASMAR1CLG", 0.05f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASMAR1CLB", 0.05f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASMAR2CLR", 0.75f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASMAR2CLG", 0.35f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASMAR2CLB", 0.35f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASBACKCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASBACKCLG", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASBACKCLB", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASC1CLLIR", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASC1CLLIG", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASC1CLLIB", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASC2CLLIR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASC2CLLIG", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASC2CLLIB", 0.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASNEEDCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASNEEDCLG", 0.25f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASNEEDCLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM1ASFONT", 2);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2TYPE", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASEPOSX", 500f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASEPOSY", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2BASESIZE", 0.9f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASSPMD", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASSFAM", 0.243f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASEFAM", 0.9f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASMSIF", 240f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASC2IF", 241f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASMRIF", 20f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASFDAM", 0.6f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASMAR1CLR", 0.05f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASMAR1CLG", 0.05f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASMAR1CLB", 0.05f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASMAR2CLR", 0.75f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASMAR2CLG", 0.35f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASMAR2CLB", 0.35f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASBACKCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASBACKCLG", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASBACKCLB", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASC1CLLIR", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASC1CLLIG", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASC1CLLIB", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASC2CLLIR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASC2CLLIG", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASC2CLLIB", 0.2f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASNEEDCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASNEEDCLG", 0.25f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASNEEDCLB", 0f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM2ASFONT", 2);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3TYPE", 10);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEPOSX", 6f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASEPOSY", -222f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3BASESIZE", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICLDMD", 1);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICIMAGE", "PREMADES2");
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICWIDT", 1060);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICHEIG", 475);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICIMGCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICIMGCLG", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM3ICIMGCLB", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASEPOSX", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASEPOSY", -327f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4BASESIZE", 0.45f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDMODE", 2);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDNSE", 7);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDCML", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDREFR", 5);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDCBRN", 0.03f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDMTPR", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDBACK", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDSTRY", 0.5f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDGITCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDGITCLG", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM4LDDGITCLB", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5TYPE", 3);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEPOSX", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASEPOSY", -247f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5BASESIZE", 0.43f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDMODE", 4);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDNSE", 7);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDCML", 1);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDREFR", 5);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDBRNT", 0f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDCBRN", 0.03f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDMTPR", 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDBACK", 0);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDSTRY", 0.53f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDGITCLR", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDGITCLG", 1f);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM5LDDGITCLB", 1f);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "TOTALOBJ", 6);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "IMAGEPATH", "TLOBD");
            #endregion
        }  if (copy) { // [SISTEM NEFOLOSIT] Sistem de copiere a presetarilor
            #region System
            PlayerPrefs.SetString("PRES" + PlayerPrefs.GetInt("TOTALPRESETS") + "NAME", PlayerPrefs.GetString("PRES" + PlayerPrefs.GetInt("SELECTEDPRESET") + "NAME") + " Copy");
            PlayerPrefs.SetString("PRES" + PlayerPrefs.GetInt("TOTALPRESETS") + "IMAGEPATH", PlayerPrefs.GetString("PRES" + PlayerPrefs.GetInt("SELECTEDPRESET") + "IMAGEPATH"));
            if (false) {
                PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCR", PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BACKSCR"));
                PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCG", PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BACKSCG"));
                PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKSCB", PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BACKSCB"));
                PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BACKMODE", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BACKMODE"));
                PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BKIMGICLDMD", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICLDMD"));
                PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BKIMGICSPLC", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICSPLC"));
                PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BKIMGICFILT", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFILT"));
                PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BKIMGICFMOD", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFMOD"));
                PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BKIMGICFORG", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFORG"));
                PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BKIMGICFAMT", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFAMT"));
                PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BKIMGICIMSZ", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICIMSZ"));
                string st = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICSPLC"; string st2 = "PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BKIMGICSPLC";
                PlayerPrefs.SetInt(st2 + "1", PlayerPrefs.GetInt(st + "1")); PlayerPrefs.SetInt(st2 + "2", PlayerPrefs.GetInt(st + "2"));
                PlayerPrefs.SetInt(st2 + "3", PlayerPrefs.GetInt(st + "3")); PlayerPrefs.SetInt(st2 + "4", PlayerPrefs.GetInt(st + "4"));
                string colorstarter = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMG"; string colorstarter2 = "PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BKIMG";
                PlayerPrefs.SetFloat(colorstarter2 + "ICIMGCLR", PlayerPrefs.GetFloat(colorstarter + "ICIMGCLR"));
                PlayerPrefs.SetFloat(colorstarter2 + "ICIMGCLG", PlayerPrefs.GetFloat(colorstarter + "ICIMGCLG"));
                PlayerPrefs.SetFloat(colorstarter2 + "ICIMGCLB", PlayerPrefs.GetFloat(colorstarter + "ICIMGCLB"));
                PlayerPrefs.SetFloat(colorstarter2 + "BCG1R", PlayerPrefs.GetFloat(colorstarter + "BCG1R"));
                PlayerPrefs.SetFloat(colorstarter2 + "BCG1G", PlayerPrefs.GetFloat(colorstarter + "BCG1G"));
                PlayerPrefs.SetFloat(colorstarter2 + "BCG1B", PlayerPrefs.GetFloat(colorstarter + "BCG1B"));
                PlayerPrefs.SetFloat(colorstarter2 + "BCG2R", PlayerPrefs.GetFloat(colorstarter + "BCG2R"));
                PlayerPrefs.SetFloat(colorstarter2 + "BCG2G", PlayerPrefs.GetFloat(colorstarter + "BCG2G"));
                PlayerPrefs.SetFloat(colorstarter2 + "BCG2B", PlayerPrefs.GetFloat(colorstarter + "BCG2B"));
                PlayerPrefs.SetFloat(colorstarter2 + "BCG3R", PlayerPrefs.GetFloat(colorstarter + "BCG3R"));
                PlayerPrefs.SetFloat(colorstarter2 + "BCG3G", PlayerPrefs.GetFloat(colorstarter + "BCG3G"));
                PlayerPrefs.SetFloat(colorstarter2 + "BCG3R", PlayerPrefs.GetFloat(colorstarter + "BCG3R"));
                PlayerPrefs.SetFloat(colorstarter2 + "BCG4G", PlayerPrefs.GetFloat(colorstarter + "BCG4G"));
                PlayerPrefs.SetFloat(colorstarter2 + "BCG4B", PlayerPrefs.GetFloat(colorstarter + "BCG4B"));
                PlayerPrefs.SetFloat(colorstarter2 + "BCG4B", PlayerPrefs.GetFloat(colorstarter + "BCG4B"));
                PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICPATH", PlayerPrefs.GetString("PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "BKIMGICPATH"));
            }
            int SelPres = PlayerPrefs.GetInt("SELECTEDPRESET");
            int Objects = (PlayerPrefs.GetInt("PRESET" + SelPres + "TOTALOBJ") - PlayerPrefs.GetInt("PRESET" + SelPres + "DELOBJ"));
            for (int o = 0; o < Objects; o++) { string colorstart = "PRESET" + SelPres + "ELEM" + o; string colorstart2 = "PRESET" + PlayerPrefs.GetInt("TOTALPRESETS") + "ELEM" + o;
                if (PlayerPrefs.GetInt(colorstart + "TYPE") == 0) {
                    PlayerPrefs.SetFloat(colorstart2 + "ASSFAM", PlayerPrefs.GetFloat(colorstart + "ASSFAM"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASEFAM", PlayerPrefs.GetFloat(colorstart + "ASEFAM"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASOSAM", PlayerPrefs.GetFloat(colorstart + "ASOSAM"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASLWAM", PlayerPrefs.GetFloat(colorstart + "ASLWAM"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASFDAM", PlayerPrefs.GetFloat(colorstart + "ASFDAM"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBSAM", PlayerPrefs.GetFloat(colorstart + "ASBSAM"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASNWAM", PlayerPrefs.GetFloat(colorstart + "ASNWAM"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASNSAM", PlayerPrefs.GetFloat(colorstart + "ASNSAM"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASELAM", PlayerPrefs.GetFloat(colorstart + "ASELAM"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASDCAM", PlayerPrefs.GetFloat(colorstart + "ASDCAM"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASMSIF", PlayerPrefs.GetFloat(colorstart + "ASMSIF"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASC2IF", PlayerPrefs.GetFloat(colorstart + "ASC2IF"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASMRIF", PlayerPrefs.GetFloat(colorstart + "ASMRIF"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASILIF", PlayerPrefs.GetFloat(colorstart + "ASILIF"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASMAAS", PlayerPrefs.GetFloat(colorstart + "ASMAAS"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASTMAS", PlayerPrefs.GetFloat(colorstart + "ASTMAS"));
                    PlayerPrefs.SetInt(colorstart2 + "ASFONT", PlayerPrefs.GetInt(colorstart + "ASFONT"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASLOAM", PlayerPrefs.GetFloat(colorstart + "ASLOAM"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASLASI", PlayerPrefs.GetFloat(colorstart + "ASLASI"));
                    PlayerPrefs.SetInt(colorstart2 + "ASSPMD", PlayerPrefs.GetInt(colorstart + "ASSPMD"));
                    PlayerPrefs.SetInt(colorstart2 + "ASLABL", PlayerPrefs.GetInt(colorstart + "ASLABL"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBACKCLR", PlayerPrefs.GetFloat(colorstart + "ASBACKCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASBACKCLG", PlayerPrefs.GetFloat(colorstart + "ASBACKCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASBACKCLB", PlayerPrefs.GetFloat(colorstart + "ASBACKCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASOUTLCLR", PlayerPrefs.GetFloat(colorstart + "ASOUTLCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASOUTLCLG", PlayerPrefs.GetFloat(colorstart + "ASOUTLCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASOUTLCLB", PlayerPrefs.GetFloat(colorstart + "ASOUTLCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASC1CLLIR", PlayerPrefs.GetFloat(colorstart + "ASC1CLLIR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASC1CLLIG", PlayerPrefs.GetFloat(colorstart + "ASC1CLLIG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASC1CLLIB", PlayerPrefs.GetFloat(colorstart + "ASC1CLLIB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASC2CLLIR", PlayerPrefs.GetFloat(colorstart + "ASC2CLLIR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASC2CLLIG", PlayerPrefs.GetFloat(colorstart + "ASC2CLLIG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASC2CLLIB", PlayerPrefs.GetFloat(colorstart + "ASC2CLLIB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASNEEDCLR", PlayerPrefs.GetFloat(colorstart + "ASNEEDCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASNEEDCLG", PlayerPrefs.GetFloat(colorstart + "ASNEEDCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASNEEDCLB", PlayerPrefs.GetFloat(colorstart + "ASNEEDCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASMAR1CLR", PlayerPrefs.GetFloat(colorstart + "ASMAR1CLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASMAR1CLG", PlayerPrefs.GetFloat(colorstart + "ASMAR1CLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASMAR1CLB", PlayerPrefs.GetFloat(colorstart + "ASMAR1CLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASMAR2CLR", PlayerPrefs.GetFloat(colorstart + "ASMAR2CLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASMAR2CLG", PlayerPrefs.GetFloat(colorstart + "ASMAR2CLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASMAR2CLB", PlayerPrefs.GetFloat(colorstart + "ASMAR2CLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASUNLACLR", PlayerPrefs.GetFloat(colorstart + "ASUNLACLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASUNLACLG", PlayerPrefs.GetFloat(colorstart + "ASUNLACLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASUNLACLB", PlayerPrefs.GetFloat(colorstart + "ASUNLACLB"));
                } if(PlayerPrefs.GetInt(colorstart + "TYPE") == 1) {
                    PlayerPrefs.SetFloat(colorstart2 + "ADDNSE", PlayerPrefs.GetFloat(colorstart + "ADDNSE"));
                    PlayerPrefs.SetFloat(colorstart2 + "ADREFR", PlayerPrefs.GetFloat(colorstart + "ADREFR"));
                    PlayerPrefs.SetInt(colorstart2 + "ADMSDD", PlayerPrefs.GetInt(colorstart + "ADMSDD"));
                    PlayerPrefs.SetInt(colorstart2 + "ADSDTG", PlayerPrefs.GetInt(colorstart + "ADSDTG"));
                    PlayerPrefs.SetInt(colorstart2 + "ADFONT", PlayerPrefs.GetInt(colorstart + "ADFONT"));
                    PlayerPrefs.SetInt(colorstart2 + "ADANIM", PlayerPrefs.GetInt(colorstart + "ADANIM"));
                    PlayerPrefs.SetFloat(colorstart2 + "ADDGCLR", PlayerPrefs.GetFloat(colorstart + "ADDGCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ADDGCLG", PlayerPrefs.GetFloat(colorstart + "ADDGCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ADDGCLB", PlayerPrefs.GetFloat(colorstart + "ADDGCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ADDG2CLR", PlayerPrefs.GetFloat(colorstart + "ADDG2CLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ADDG2CLG", PlayerPrefs.GetFloat(colorstart + "ADDG2CLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ADDG2CLB", PlayerPrefs.GetFloat(colorstart + "ADDG2CLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ADDGTXCLR", PlayerPrefs.GetFloat(colorstart + "ADDGTXCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ADDGTXCLG", PlayerPrefs.GetFloat(colorstart + "ADDGTXCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ADDGTXCLB", PlayerPrefs.GetFloat(colorstart + "ADDGTXCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ADDG2TXCLR", PlayerPrefs.GetFloat(colorstart + "ADDG2TXCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ADDG2TXCLG", PlayerPrefs.GetFloat(colorstart + "ADDG2TXCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ADDG2TXCLB", PlayerPrefs.GetFloat(colorstart + "ADDG2TXCLB"));
                } if (PlayerPrefs.GetInt(colorstart + "TYPE") == 2) {
                    PlayerPrefs.SetFloat(colorstart2 + "LSDNSE", PlayerPrefs.GetFloat(colorstart + "LSDNSE"));
                    PlayerPrefs.SetFloat(colorstart2 + "LSDMSZ", PlayerPrefs.GetFloat(colorstart + "LSDMSZ"));
                    PlayerPrefs.SetFloat(colorstart2 + "LSREFR", PlayerPrefs.GetFloat(colorstart + "LSREFR"));
                    PlayerPrefs.SetFloat(colorstart2 + "LSBRNT", PlayerPrefs.GetFloat(colorstart + "LSBRNT"));
                    PlayerPrefs.SetFloat(colorstart2 + "LSCBRN", PlayerPrefs.GetFloat(colorstart + "LSCBRN"));
                    PlayerPrefs.SetFloat(colorstart2 + "LSISHD", PlayerPrefs.GetFloat(colorstart + "LSISHD"));
                    PlayerPrefs.SetInt(colorstart2 + "LSSFNT", PlayerPrefs.GetInt(colorstart + "LSSFNT"));
                    PlayerPrefs.SetInt(colorstart2 + "LSDECI", PlayerPrefs.GetInt(colorstart + "LSDECI"));
                    PlayerPrefs.SetInt(colorstart2 + "LSBACK", PlayerPrefs.GetInt(colorstart + "LSBACK"));
                    PlayerPrefs.SetInt(colorstart2 + "LSMLD1", PlayerPrefs.GetInt(colorstart + "LSMLD1"));
                    PlayerPrefs.SetInt(colorstart2 + "LSFONT", PlayerPrefs.GetInt(colorstart + "LSFONT"));
                    PlayerPrefs.SetFloat(colorstart2 + "LSSTRX", PlayerPrefs.GetFloat(colorstart + "LSSTRX"));
                    PlayerPrefs.SetFloat(colorstart2 + "LSSTRY", PlayerPrefs.GetFloat(colorstart + "LSSTRY"));
                    PlayerPrefs.SetInt(colorstart2 + "LSMTPR", PlayerPrefs.GetInt(colorstart + "LSMTPR"));
                    PlayerPrefs.SetInt(colorstart2 + "LSSPUN", PlayerPrefs.GetInt(colorstart + "LSSPUN"));
                    PlayerPrefs.SetFloat(colorstart2 + "LSBACKCLR", PlayerPrefs.GetFloat(colorstart + "LSBACKCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LSBACKCLG", PlayerPrefs.GetFloat(colorstart + "LSBACKCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LSBACKCLB", PlayerPrefs.GetFloat(colorstart + "LSBACKCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "LSDGITCLR", PlayerPrefs.GetFloat(colorstart + "LSDGITCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LSDGITCLG", PlayerPrefs.GetFloat(colorstart + "LSDGITCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LSDGITCLB", PlayerPrefs.GetFloat(colorstart + "LSDGITCLB"));
                } if (PlayerPrefs.GetInt(colorstart + "TYPE") == 3) {
                    PlayerPrefs.SetFloat(colorstart2 + "LDDNSE", PlayerPrefs.GetFloat(colorstart + "LDDNSE"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDDCML", PlayerPrefs.GetFloat(colorstart + "LDDCML"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDREFR", PlayerPrefs.GetFloat(colorstart + "LDREFR"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDBRNT", PlayerPrefs.GetFloat(colorstart + "LDBRNT"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDCBRN", PlayerPrefs.GetFloat(colorstart + "LDCBRN"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDISHD", PlayerPrefs.GetFloat(colorstart + "LDISHD"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDMODE", PlayerPrefs.GetFloat(colorstart + "LDMODE"));
                    PlayerPrefs.SetInt(colorstart2 + "LDSFNT", PlayerPrefs.GetInt(colorstart + "LDSFNT"));
                    PlayerPrefs.SetInt(colorstart2 + "LDBACK", PlayerPrefs.GetInt(colorstart + "LDBACK"));
                    PlayerPrefs.SetInt(colorstart2 + "LDMLD1", PlayerPrefs.GetInt(colorstart + "LDMLD1"));
                    PlayerPrefs.SetInt(colorstart2 + "LDFONT", PlayerPrefs.GetInt(colorstart + "LDFONT"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDSTRX", PlayerPrefs.GetFloat(colorstart + "LDSTRX"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDSTRY", PlayerPrefs.GetFloat(colorstart + "LDSTRY"));
                    PlayerPrefs.SetInt(colorstart2 + "LDMTPR", PlayerPrefs.GetInt(colorstart + "LDMTPR"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDBACKCLR", PlayerPrefs.GetFloat(colorstart + "LDBACKCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LDBACKCLG", PlayerPrefs.GetFloat(colorstart + "LDBACKCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LDBACKCLB", PlayerPrefs.GetFloat(colorstart + "LDBACKCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDDGITCLR", PlayerPrefs.GetFloat(colorstart + "LDDGITCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LDDGITCLG", PlayerPrefs.GetFloat(colorstart + "LDDGITCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LDDGITCLB", PlayerPrefs.GetFloat(colorstart + "LDDGITCLB"));
                } if (PlayerPrefs.GetInt(colorstart + "TYPE") == 4) {
                    PlayerPrefs.SetFloat(colorstart2 + "ASBHEIG", PlayerPrefs.GetFloat(colorstart + "ASBHEIG"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBWIDT", PlayerPrefs.GetFloat(colorstart + "ASBWIDT"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBBOWI", PlayerPrefs.GetFloat(colorstart + "ASBBOWI"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBFILO", PlayerPrefs.GetFloat(colorstart + "ASBFILO"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBNEDW", PlayerPrefs.GetFloat(colorstart + "ASBNEDW"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBNEWT", PlayerPrefs.GetFloat(colorstart + "ASBNEWT"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBLIMT", PlayerPrefs.GetFloat(colorstart + "ASBLIMT"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBC2ST", PlayerPrefs.GetFloat(colorstart + "ASBC2ST"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBMRRT", PlayerPrefs.GetFloat(colorstart + "ASBMRRT"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBINLN", PlayerPrefs.GetFloat(colorstart + "ASBINLN"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBMARW", PlayerPrefs.GetFloat(colorstart + "ASBMARW"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBMTXW", PlayerPrefs.GetFloat(colorstart + "ASBMTXW"));
                    PlayerPrefs.SetInt(colorstart2 + "ASBFONT", PlayerPrefs.GetInt(colorstart + "ASBFONT"));
                    PlayerPrefs.SetInt(colorstart2 + "ASBSPMD", PlayerPrefs.GetInt(colorstart + "ASBSPMD"));
                    PlayerPrefs.SetInt(colorstart2 + "ASBFILL", PlayerPrefs.GetInt(colorstart + "ASBFILL"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASBACKCLR", PlayerPrefs.GetFloat(colorstart + "ASBACKCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASBACKCLG", PlayerPrefs.GetFloat(colorstart + "ASBACKCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASBACKCLB", PlayerPrefs.GetFloat(colorstart + "ASBACKCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASOUTLCLR", PlayerPrefs.GetFloat(colorstart + "ASOUTLCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASOUTLCLG", PlayerPrefs.GetFloat(colorstart + "ASOUTLCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASOUTLCLB", PlayerPrefs.GetFloat(colorstart + "ASOUTLCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASC1CLLIR", PlayerPrefs.GetFloat(colorstart + "ASC1CLLIR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASC1CLLIG", PlayerPrefs.GetFloat(colorstart + "ASC1CLLIG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASC1CLLIB", PlayerPrefs.GetFloat(colorstart + "ASC1CLLIB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASC2CLLIR", PlayerPrefs.GetFloat(colorstart + "ASC2CLLIR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASC2CLLIG", PlayerPrefs.GetFloat(colorstart + "ASC2CLLIG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASC2CLLIB", PlayerPrefs.GetFloat(colorstart + "ASC2CLLIB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASNEEDCLR", PlayerPrefs.GetFloat(colorstart + "ASNEEDCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASNEEDCLG", PlayerPrefs.GetFloat(colorstart + "ASNEEDCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASNEEDCLB", PlayerPrefs.GetFloat(colorstart + "ASNEEDCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASMAR1CLR", PlayerPrefs.GetFloat(colorstart + "ASMAR1CLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASMAR1CLG", PlayerPrefs.GetFloat(colorstart + "ASMAR1CLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASMAR1CLB", PlayerPrefs.GetFloat(colorstart + "ASMAR1CLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ASMAR2CLR", PlayerPrefs.GetFloat(colorstart + "ASMAR2CLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASMAR2CLG", PlayerPrefs.GetFloat(colorstart + "ASMAR2CLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ASMAR2CLB", PlayerPrefs.GetFloat(colorstart + "ASMAR2CLB"));
                } if (PlayerPrefs.GetInt(colorstart + "TYPE") == 5) {
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBVFIL", PlayerPrefs.GetFloat(colorstart + "ACSBVFIL"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBVTIC", PlayerPrefs.GetFloat(colorstart + "ACSBVTIC"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBBOWI", PlayerPrefs.GetFloat(colorstart + "ACSBBOWI"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBFILO", PlayerPrefs.GetFloat(colorstart + "ACSBFILO"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBNEDW", PlayerPrefs.GetFloat(colorstart + "ACSBNEDW"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBNEWT", PlayerPrefs.GetFloat(colorstart + "ACSBNEWT"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBLIMT", PlayerPrefs.GetFloat(colorstart + "ACSBLIMT"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBC2ST", PlayerPrefs.GetFloat(colorstart + "ACSBC2ST"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBMRRT", PlayerPrefs.GetFloat(colorstart + "ACSBMRRT"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBINLN", PlayerPrefs.GetFloat(colorstart + "ACSBINLN"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBMARW", PlayerPrefs.GetFloat(colorstart + "ACSBMARW"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBMTXW", PlayerPrefs.GetFloat(colorstart + "ACSBMTXW"));
                    PlayerPrefs.SetInt(colorstart2 + "ACSBFONT", PlayerPrefs.GetInt(colorstart + "ACSBFONT"));
                    PlayerPrefs.SetInt(colorstart2 + "ACSBCCWI", PlayerPrefs.GetInt(colorstart + "ACSBCCWI"));
                    PlayerPrefs.SetInt(colorstart2 + "ACSBSPMD", PlayerPrefs.GetInt(colorstart + "ACSBSPMD"));
                    PlayerPrefs.SetInt(colorstart2 + "ACSBFILL", PlayerPrefs.GetInt(colorstart + "ACSBFILL"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBBORDCLR", PlayerPrefs.GetFloat(colorstart + "ACSBBORDCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBBORDCLG", PlayerPrefs.GetFloat(colorstart + "ACSBBORDCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBBORDCLB", PlayerPrefs.GetFloat(colorstart + "ACSBBORDCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBBACKCLR", PlayerPrefs.GetFloat(colorstart + "ACSBBACKCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBBACKCLG", PlayerPrefs.GetFloat(colorstart + "ACSBBACKCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBBACKCLB", PlayerPrefs.GetFloat(colorstart + "ACSBBACKCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBNEEDCLR", PlayerPrefs.GetFloat(colorstart + "ACSBNEEDCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBNEEDCLG", PlayerPrefs.GetFloat(colorstart + "ACSBNEEDCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBNEEDCLB", PlayerPrefs.GetFloat(colorstart + "ACSBNEEDCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBFILLCLR", PlayerPrefs.GetFloat(colorstart + "ACSBFILLCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBFILLCLG", PlayerPrefs.GetFloat(colorstart + "ACSBFILLCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBFILLCLB", PlayerPrefs.GetFloat(colorstart + "ACSBFILLCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBMKC1CLR", PlayerPrefs.GetFloat(colorstart + "ACSBMKC1CLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBMKC1CLG", PlayerPrefs.GetFloat(colorstart + "ACSBMKC1CLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBMKC1CLB", PlayerPrefs.GetFloat(colorstart + "ACSBMKC1CLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBMKC2CLR", PlayerPrefs.GetFloat(colorstart + "ACSBMKC2CLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBMKC2CLG", PlayerPrefs.GetFloat(colorstart + "ACSBMKC2CLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ACSBMKC2CLB", PlayerPrefs.GetFloat(colorstart + "ACSBMKC2CLB"));
                } if (PlayerPrefs.GetInt(colorstart + "TYPE") == 6) {
                    PlayerPrefs.SetFloat(colorstart2 + "LDSBHEIG", PlayerPrefs.GetFloat(colorstart + "LDSBHEIG"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDSBWIDT", PlayerPrefs.GetFloat(colorstart + "LDSBWIDT"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDSBREFR", PlayerPrefs.GetFloat(colorstart + "LDSBREFR"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDSBBRNT", PlayerPrefs.GetFloat(colorstart + "LDSBBRNT"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDSBCBRN", PlayerPrefs.GetFloat(colorstart + "LDSBCBRN"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDSBISHD", PlayerPrefs.GetFloat(colorstart + "LDSBISHD"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDSBSEGC", PlayerPrefs.GetFloat(colorstart + "LDSBSEGC"));
                    PlayerPrefs.SetInt(colorstart2 + "LDSBSEGG", PlayerPrefs.GetInt(colorstart + "LDSBSEGG"));
                    PlayerPrefs.SetInt(colorstart2 + "LDSBTOPS", PlayerPrefs.GetInt(colorstart + "LDSBTOPS"));
                    PlayerPrefs.SetInt(colorstart2 + "LDSBSPUN", PlayerPrefs.GetInt(colorstart + "LDSBSPUN"));
                    PlayerPrefs.SetInt(colorstart2 + "LDSBBACK", PlayerPrefs.GetInt(colorstart + "LDSBBACK"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDSBBACKCLR", PlayerPrefs.GetFloat(colorstart + "LDSBBACKCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LDSBBACKCLG", PlayerPrefs.GetFloat(colorstart + "LDSBBACKCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LDSBBACKCLB", PlayerPrefs.GetFloat(colorstart + "LDSBBACKCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDSBDGITCLR", PlayerPrefs.GetFloat(colorstart + "LDSBDGITCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LDSBDGITCLG", PlayerPrefs.GetFloat(colorstart + "LDSBDGITCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LDSBDGITCLB", PlayerPrefs.GetFloat(colorstart + "LDSBDGITCLB"));
                } if (PlayerPrefs.GetInt(colorstart + "TYPE") == 7) {
                    PlayerPrefs.SetFloat(colorstart2 + "LDCSBHEIG", PlayerPrefs.GetFloat(colorstart + "LDCSBHEIG"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDCSBWIDT", PlayerPrefs.GetFloat(colorstart + "LDCSBWIDT"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDCSBREFR", PlayerPrefs.GetFloat(colorstart + "LDCSBREFR"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDCSBBRNT", PlayerPrefs.GetFloat(colorstart + "LDCSBBRNT"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDCSBCBRN", PlayerPrefs.GetFloat(colorstart + "LDCSBCBRN"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDCSBISHD", PlayerPrefs.GetFloat(colorstart + "LDCSBISHD"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDCSBSEGC", PlayerPrefs.GetFloat(colorstart + "LDCSBSEGC"));
                    PlayerPrefs.SetInt(colorstart2 + "LDCSBSEGG", PlayerPrefs.GetInt(colorstart + "LDCSBSEGG"));
                    PlayerPrefs.SetInt(colorstart2 + "LDCSBTOPS", PlayerPrefs.GetInt(colorstart + "LDCSBTOPS"));
                    PlayerPrefs.SetInt(colorstart2 + "LDCSCCWIS", PlayerPrefs.GetInt(colorstart + "LDCSCCWIS"));
                    PlayerPrefs.SetInt(colorstart2 + "LDCSBSPUN", PlayerPrefs.GetInt(colorstart + "LDCSBSPUN"));
                    PlayerPrefs.SetInt(colorstart2 + "LDCSBBACK", PlayerPrefs.GetInt(colorstart + "LDCSBBACK"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDCSBBACKCLR", PlayerPrefs.GetFloat(colorstart + "LDCSBBACKCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LDCSBBACKCLG", PlayerPrefs.GetFloat(colorstart + "LDCSBBACKCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LDCSBBACKCLB", PlayerPrefs.GetFloat(colorstart + "LDCSBBACKCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "LDCSBDGITCLR", PlayerPrefs.GetFloat(colorstart + "LDCSBDGITCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LDCSBDGITCLG", PlayerPrefs.GetFloat(colorstart + "LDCSBDGITCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LDCSBDGITCLB", PlayerPrefs.GetFloat(colorstart + "LDCSBDGITCLB"));
                } if (PlayerPrefs.GetInt(colorstart + "TYPE") == 8) {
                    PlayerPrefs.SetFloat(colorstart2 + "LNTHIC", PlayerPrefs.GetFloat(colorstart + "LNTHIC"));
                    PlayerPrefs.SetFloat(colorstart2 + "LNLENG", PlayerPrefs.GetFloat(colorstart + "LNLENG"));
                    PlayerPrefs.SetFloat(colorstart2 + "LNM1B", PlayerPrefs.GetFloat(colorstart + "LNM1B"));
                    PlayerPrefs.SetInt(colorstart2 + "LNMODE", PlayerPrefs.GetInt(colorstart + "LNMODE"));
                    PlayerPrefs.SetFloat(colorstart2 + "LNLINECLR", PlayerPrefs.GetFloat(colorstart + "LNLINECLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LNLINECLG", PlayerPrefs.GetFloat(colorstart + "LNLINECLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "LNLINECLB", PlayerPrefs.GetFloat(colorstart + "LNLINECLB"));
                } if (PlayerPrefs.GetInt(colorstart + "TYPE") == 9) {
                    PlayerPrefs.SetFloat(colorstart2 + "TXWIDT", PlayerPrefs.GetFloat(colorstart + "TXWIDT"));
                    PlayerPrefs.SetFloat(colorstart2 + "TXHEIG", PlayerPrefs.GetFloat(colorstart + "TXHEIG"));
                    PlayerPrefs.SetString(colorstart2 + "TXTEXT", PlayerPrefs.GetString(colorstart + "TXTEXT"));
                    PlayerPrefs.SetInt(colorstart2 + "TXFONT", PlayerPrefs.GetInt(colorstart + "TXFONT"));
                    PlayerPrefs.SetInt(colorstart2 + "TXFONTF", PlayerPrefs.GetInt(colorstart + "TXFONTF"));
                    PlayerPrefs.SetInt(colorstart2 + "TXBOIT", PlayerPrefs.GetInt(colorstart + "TXBOIT"));
                    PlayerPrefs.SetFloat(colorstart2 + "TXLNSP", PlayerPrefs.GetFloat(colorstart + "TXLNSP"));
                    PlayerPrefs.SetFloat(colorstart2 + "TXKERN", PlayerPrefs.GetFloat(colorstart + "TXKERN"));
                    PlayerPrefs.SetInt(colorstart2 + "TXALMD", PlayerPrefs.GetInt(colorstart + "TXALMD"));
                    PlayerPrefs.SetInt(colorstart2 + "TXWRMD", PlayerPrefs.GetInt(colorstart + "TXWRMD"));
                    PlayerPrefs.SetInt(colorstart2 + "TXFTSZ", PlayerPrefs.GetInt(colorstart + "TXFTSZ"));
                    PlayerPrefs.SetInt(colorstart2 + "TXDYNAC", PlayerPrefs.GetInt(colorstart + "TXDYNAC"));
                    PlayerPrefs.SetInt(colorstart2 + "TXMXDE", PlayerPrefs.GetInt(colorstart + "TXMXDE"));
                    PlayerPrefs.SetInt(colorstart2 + "TXMXDI", PlayerPrefs.GetInt(colorstart + "TXMXDI"));
                    PlayerPrefs.SetInt(colorstart2 + "TXRRAT", PlayerPrefs.GetInt(colorstart + "TXRRAT"));
                    PlayerPrefs.SetInt(colorstart2 + "TXDYNA", PlayerPrefs.GetInt(colorstart + "TXDYNA"));
                    PlayerPrefs.SetFloat(colorstart2 + "TXTEXTCLR", PlayerPrefs.GetFloat(colorstart + "TXTEXTCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "TXTEXTCLG", PlayerPrefs.GetFloat(colorstart + "TXTEXTCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "TXTEXTCLB", PlayerPrefs.GetFloat(colorstart + "TXTEXTCLB"));
                    PlayerPrefs.SetInt(colorstart2 + "TMPMEFFECT", PlayerPrefs.GetInt(colorstart + "TMPMEFFECT"));
                    PlayerPrefs.SetInt(colorstart2 + "TMPMBCOLMO", PlayerPrefs.GetInt(colorstart + "TMPMBCOLMO"));
                    PlayerPrefs.SetInt(colorstart2 + "TMPMSGRTYP", PlayerPrefs.GetInt(colorstart + "TMPMSGRTYP"));
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTCOLR", PlayerPrefs.GetFloat(colorstart + "BTXTCOLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTCOLG", PlayerPrefs.GetFloat(colorstart + "BTXTCOLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTCOLB", PlayerPrefs.GetFloat(colorstart + "BTXTCOLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG21R", PlayerPrefs.GetFloat(colorstart + "BTXTG21R")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG21G", PlayerPrefs.GetFloat(colorstart + "BTXTG21G")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG21B", PlayerPrefs.GetFloat(colorstart + "BTXTG21B"));
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG22R", PlayerPrefs.GetFloat(colorstart + "BTXTG22R")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG22G", PlayerPrefs.GetFloat(colorstart + "BTXTG22G")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG22B", PlayerPrefs.GetFloat(colorstart + "BTXTG22B"));
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG41R", PlayerPrefs.GetFloat(colorstart + "BTXTG41R")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG41G", PlayerPrefs.GetFloat(colorstart + "BTXTG41G")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG41B", PlayerPrefs.GetFloat(colorstart + "BTXTG41B"));
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG42R", PlayerPrefs.GetFloat(colorstart + "BTXTG42R")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG42G", PlayerPrefs.GetFloat(colorstart + "BTXTG42G")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG42B", PlayerPrefs.GetFloat(colorstart + "BTXTG42B"));
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG43R", PlayerPrefs.GetFloat(colorstart + "BTXTG43R")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG43G", PlayerPrefs.GetFloat(colorstart + "BTXTG43G")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG43B", PlayerPrefs.GetFloat(colorstart + "BTXTG43B"));
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG44R", PlayerPrefs.GetFloat(colorstart + "BTXTG44R")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG44G", PlayerPrefs.GetFloat(colorstart + "BTXTG44G")); 
                    PlayerPrefs.SetFloat(colorstart2 + "BTXTG44B", PlayerPrefs.GetFloat(colorstart + "BTXTG44B"));
                    PlayerPrefs.SetFloat(colorstart2 + "TMPMBDILAT", PlayerPrefs.GetFloat(colorstart + "TMPMBDILAT")); 
                    PlayerPrefs.SetFloat(colorstart2 + "TMPMBSOFTN", PlayerPrefs.GetFloat(colorstart + "TMPMBSOFTN"));
                    PlayerPrefs.SetFloat(colorstart2 + "STXTCOLR", PlayerPrefs.GetFloat(colorstart + "STXTCOLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTCOLG", PlayerPrefs.GetFloat(colorstart + "STXTCOLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTCOLB", PlayerPrefs.GetFloat(colorstart + "STXTCOLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG21R", PlayerPrefs.GetFloat(colorstart + "STXTG21R")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG21G", PlayerPrefs.GetFloat(colorstart + "STXTG21G")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG21B", PlayerPrefs.GetFloat(colorstart + "STXTG21B"));
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG22R", PlayerPrefs.GetFloat(colorstart + "STXTG22R")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG22G", PlayerPrefs.GetFloat(colorstart + "STXTG22G")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG22B", PlayerPrefs.GetFloat(colorstart + "STXTG22B"));
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG41R", PlayerPrefs.GetFloat(colorstart + "STXTG41R")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG41G", PlayerPrefs.GetFloat(colorstart + "STXTG41G")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG41B", PlayerPrefs.GetFloat(colorstart + "STXTG41B"));
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG42R", PlayerPrefs.GetFloat(colorstart + "STXTG42R")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG42G", PlayerPrefs.GetFloat(colorstart + "STXTG42G")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG42B", PlayerPrefs.GetFloat(colorstart + "STXTG42B"));
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG43R", PlayerPrefs.GetFloat(colorstart + "STXTG43R")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG43G", PlayerPrefs.GetFloat(colorstart + "STXTG43G")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG43B", PlayerPrefs.GetFloat(colorstart + "STXTG43B"));
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG44R", PlayerPrefs.GetFloat(colorstart + "STXTG44R")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG44G", PlayerPrefs.GetFloat(colorstart + "STXTG44G")); 
                    PlayerPrefs.SetFloat(colorstart2 + "STXTG44B", PlayerPrefs.GetFloat(colorstart + "STXTG44B"));
                    PlayerPrefs.SetFloat(colorstart2 + "TMPMSDILAT", PlayerPrefs.GetFloat(colorstart + "TMPMSDILAT")); 
                    PlayerPrefs.SetFloat(colorstart2 + "TMPMSSOFTN", PlayerPrefs.GetFloat(colorstart + "TMPMSSOFTN"));
                } if (PlayerPrefs.GetInt(colorstart + "TYPE") == 10) {
                    PlayerPrefs.SetInt(colorstart2 + "ICLDMD", PlayerPrefs.GetInt(colorstart + "ICLDMD"));
                    PlayerPrefs.SetInt(colorstart2 + "ICSPLC", PlayerPrefs.GetInt(colorstart + "ICSPLC"));
                    PlayerPrefs.SetInt(colorstart2 + "ICFILT", PlayerPrefs.GetInt(colorstart + "ICFILT"));
                    PlayerPrefs.SetInt(colorstart2 + "ICWIDT", PlayerPrefs.GetInt(colorstart + "ICWIDT"));
                    PlayerPrefs.SetInt(colorstart2 + "ICHEIG", PlayerPrefs.GetInt(colorstart + "ICHEIG"));
                    PlayerPrefs.SetInt(colorstart2 + "ICFMOD", PlayerPrefs.GetInt(colorstart + "ICFMOD"));
                    PlayerPrefs.SetFloat(colorstart2 + "ICFORG", PlayerPrefs.GetFloat(colorstart + "ICFORG"));
                    PlayerPrefs.SetFloat(colorstart2 + "ICFAMT", PlayerPrefs.GetFloat(colorstart + "ICFAMT"));
                    PlayerPrefs.SetFloat(colorstart2 + "ICIMSZ", PlayerPrefs.GetFloat(colorstart + "ICIMSZ")); 
                    string stx = colorstart + "ICSPLC"; string stx2 = colorstart2 + "ICSPLC";
                    PlayerPrefs.SetInt(stx2 + "1", PlayerPrefs.GetInt(stx + "1")); 
                    PlayerPrefs.SetInt(stx2 + "2", PlayerPrefs.GetInt(stx + "2")); 
                    PlayerPrefs.SetInt(stx2 + "3", PlayerPrefs.GetInt(stx + "3")); 
                    PlayerPrefs.SetInt(stx2 + "4", PlayerPrefs.GetInt(stx + "4"));
                    PlayerPrefs.SetFloat(colorstart2 + "ICIMGCLR", PlayerPrefs.GetFloat(colorstart + "ICIMGCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ICIMGCLG", PlayerPrefs.GetFloat(colorstart + "ICIMGCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "ICIMGCLB", PlayerPrefs.GetFloat(colorstart + "ICIMGCLB"));
                    PlayerPrefs.SetString(colorstart2 + "ICPATH", PlayerPrefs.GetString(colorstart + "ICPATH"));
                } if (PlayerPrefs.GetInt(colorstart + "TYPE") == 11) {
                    PlayerPrefs.SetInt(colorstart2 + "COOUTS", PlayerPrefs.GetInt(colorstart + "COOUTS"));
                    PlayerPrefs.SetInt(colorstart2 + "CONEDS", PlayerPrefs.GetInt(colorstart + "CONEDS"));
                    PlayerPrefs.SetInt(colorstart2 + "COMRKS", PlayerPrefs.GetInt(colorstart + "COMRKS"));
                    PlayerPrefs.SetInt(colorstart2 + "CONSEW", PlayerPrefs.GetInt(colorstart + "CONSEW"));
                    PlayerPrefs.SetFloat(colorstart2 + "COPERS", PlayerPrefs.GetFloat(colorstart + "COPERS"));
                    PlayerPrefs.SetInt(colorstart2 + "COFADE", PlayerPrefs.GetInt(colorstart + "COFADE"));
                    PlayerPrefs.SetInt(colorstart2 + "COMO3D", PlayerPrefs.GetInt(colorstart + "COMO3D"));
                    PlayerPrefs.SetInt(colorstart2 + "COLKNE", PlayerPrefs.GetInt(colorstart + "COLKNE"));
                    PlayerPrefs.SetInt(colorstart2 + "CODET1", PlayerPrefs.GetInt(colorstart + "CODET1"));
                    PlayerPrefs.SetInt(colorstart2 + "COSDLN", PlayerPrefs.GetInt(colorstart + "COSDLN"));
                    PlayerPrefs.SetInt(colorstart2 + "COSNLN", PlayerPrefs.GetInt(colorstart + "COSNLN"));
                    PlayerPrefs.SetInt(colorstart2 + "COSALN", PlayerPrefs.GetInt(colorstart + "COSALN"));
                    PlayerPrefs.SetFloat(colorstart2 + "COOUTLCLR", PlayerPrefs.GetFloat(colorstart + "COOUTLCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "COOUTLCLG", PlayerPrefs.GetFloat(colorstart + "COOUTLCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "COOUTLCLB", PlayerPrefs.GetFloat(colorstart + "COOUTLCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "COBACKCLR", PlayerPrefs.GetFloat(colorstart + "COBACKCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "COBACKCLG", PlayerPrefs.GetFloat(colorstart + "COBACKCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "COBACKCLB", PlayerPrefs.GetFloat(colorstart + "COBACKCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "CONEDFCLR", PlayerPrefs.GetFloat(colorstart + "CONEDFCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "CONEDFCLG", PlayerPrefs.GetFloat(colorstart + "CONEDFCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "CONEDFCLB", PlayerPrefs.GetFloat(colorstart + "CONEDFCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "CONEDRCLR", PlayerPrefs.GetFloat(colorstart + "CONEDRCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "CONEDRCLG", PlayerPrefs.GetFloat(colorstart + "CONEDRCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "CONEDRCLB", PlayerPrefs.GetFloat(colorstart + "CONEDRCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "COMARKCLR", PlayerPrefs.GetFloat(colorstart + "COMARKCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "COMARKCLG", PlayerPrefs.GetFloat(colorstart + "COMARKCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "COMARKCLB", PlayerPrefs.GetFloat(colorstart + "COMARKCLB"));
                    PlayerPrefs.SetFloat(colorstart2 + "CODETACLR", PlayerPrefs.GetFloat(colorstart + "CODETACLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "CODETACLG", PlayerPrefs.GetFloat(colorstart + "CODETACLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "CODETACLB", PlayerPrefs.GetFloat(colorstart + "CODETACLB"));
                } if (PlayerPrefs.GetInt(colorstart + "TYPE") == 12) {
                    PlayerPrefs.SetInt(colorstart2 + "MPWIDT", PlayerPrefs.GetInt(colorstart + "MPWIDT"));
                    PlayerPrefs.SetInt(colorstart2 + "MPHEIG", PlayerPrefs.GetInt(colorstart + "MPHEIG"));
                    PlayerPrefs.SetInt(colorstart2 + "MPSELM", PlayerPrefs.GetInt(colorstart + "MPSELM"));
                    PlayerPrefs.SetFloat(colorstart2 + "MPBACKCLR", PlayerPrefs.GetFloat(colorstart + "MPBACKCLR")); 
                    PlayerPrefs.SetFloat(colorstart2 + "MPBACKCLG", PlayerPrefs.GetFloat(colorstart + "MPBACKCLG")); 
                    PlayerPrefs.SetFloat(colorstart2 + "MPBACKCLB", PlayerPrefs.GetFloat(colorstart + "MPBACKCLB"));
                    for (int d = 0; d < 9999; d++) if (PlayerPrefs.GetString(colorstart + "MPLAYOLCOMP" + d, "./") == "./") break;
                    else PlayerPrefs.SetString(colorstart2 + "MPLAYOLCOMP" + d, PlayerPrefs.GetString(colorstart + "MPLAYOLCOMP" + d));
                }
            }
            #endregion
        } // Setarea datelor necesare
        int Presn = PlayerPrefs.GetInt("TOTALPRESETS"); int UsedPresnm = orgfel.childCount - 1;
        GameObject NewPresn = Instantiate(PresetButton, orgfel); NewPresn.SetActive(true);
        NewPresn.GetComponent<RectTransform>().anchoredPosition = new Vector2(-unitlenght + (unitlenght * (UsedPresnm % 3)), -20f - (400f * (UsedPresnm / 3)));
        NewPresn.transform.GetChild(2).GetComponent<Text>().text = PlayerPrefs.GetString("PRES" + Presn + "NAME");
        StartCoroutine(LDImg(PlayerPrefs.GetString("PRESET" + Presn + "IMAGEPATH", ":/")));
        NewPresn.transform.GetChild(0).GetComponent<Image>().sprite = cranberry;
        if (cranberry == null) NewPresn.transform.GetChild(0).GetComponent<Image>().color = Color.grey;
        else NewPresn.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        int flambda = Presn; NewPresn.GetComponent<Button>().onClick.AddListener(() => OnButtonPresetClick(flambda, NewPresn));
        NewPresn.name = UsedPresnm + ""; PlayerPrefs.SetInt("TOTALPRESETS", PlayerPrefs.GetInt("TOTALPRESETS") + 1); UsedPresnm++;
        AddButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-unitlenght + (unitlenght * (UsedPresnm % 3)), -20f - (400f * (UsedPresnm / 3)));
        orgfel.GetComponent<RectTransform>().sizeDelta = new Vector2(orgfel.GetComponent<RectTransform>().sizeDelta.x, 440f + (400f * (UsedPresnm / 3)));
        orgfel.parent.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(unitlenght * 3f, 830f); LOAD.SetActive(false);
        if (PlayerPrefs.GetInt("HASHINT2", 0) == 0) {
            if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(0f, 0f), "Presets", "Once you've created your preset, you can select it by tapping on it", 1000f, 0.3f, 2);
            if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(0f, 0f), "Préréglages", "Une fois votre préréglage créé, vous pouvez le sélectionner en appuyant dessus", 1000f, 0.3f, 2);
            if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(0f, 0f), "Voreinstellungen", "Sobald Sie Ihre Voreinstellung erstellt haben, können Sie sie durch Antippen auswählen", 1000f, 0.3f, 2);
            if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(0f, 0f), "Presetări", "După ce ați creat presetarea, o puteți selecta atingând-o", 1000f, 0.3f, 2);
        }
    }

    // Functie executata cand un preset este selectat prin atingerea acestuia
    public void OnButtonPresetClick(int ID, GameObject btn) { SelctButton = btn;
        OpenPanel.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("PRES" + ID + "NAME");
        OpenPanel.transform.GetChild(1).GetComponent<Text>().text = "Objects: " + (PlayerPrefs.GetInt("PRESET" + ID + "TOTALOBJ") - PlayerPrefs.GetInt("PRESET" + ID + "DELOBJ"));
        PlayerPrefs.SetInt("SELECTEDPRESET", ID);
        if (PlayerPrefs.GetInt("HASHINT3", 0) == 0) {
            if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(0f, 0f), "Using Presets", "You can now use the following buttons:\nUse - use the preset connected to your speedometer\nModdify - moddify the preset in the editor\n... - more options (Delete; Rename; etc.)", 1150f, 0.3f, 3);
            if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(0f, 0f), "Utiliser les préréglages", "Vous pouvez désormais utiliser les boutons suivants :\nUtiliser - utiliser le préréglage associé à votre tachymètre\nModifier - modifier le préréglage dans l’éditeur\n... - plus d’options (Supprimer ; Renommer ; etc.)", 1250f, 0.3f, 3);
            if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(0f, 0f), "Verwenden von Voreinstellungen", "Sie können nun die folgenden Schaltflächen verwenden: \nVerwenden – die mit Ihrem Tachometer verbundene Voreinstellung verwenden\nÄndern – die Voreinstellung im Editor bearbeiten\n... – weitere Optionen (Löschen; Umbenennen; usw.)", 1350f, 0.3f, 3);
            if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(0f, 0f), "Utilizarea presetărilor", "Acum puteți utiliza următoarele butoane:\nUtilizare - utilizați presetarea conectată la vitezometru\nModificare - modificați presetarea în editor\n... - mai multe opțiuni (Ștergere; Redenumire; etc.)", 1200f, 0.3f, 3);
        }
    }

    // Functie executata la stergerea presetarii selectate
    public void DeleteButton() {
        PlayerPrefs.SetInt("PRES" + PlayerPrefs.GetInt("SELECTEDPRESET") + "EXISTS", 2);
        StartCoroutine(LOADShow(1)); OpenPanel.SetActive(false);
    }

    // Functie executata la modificarea presetarii selectate
    public void ModdifyButton() {
        StartCoroutine(LOADShow(3));
    }

    // Functie executata la folosirea presetarii selectate
    public void UseButton(bool flipped) {
        if (flipped) StartCoroutine(LOADShow(5));
        else StartCoroutine(LOADShow(4));
    }

    // [FUNCTIE NEFOLOSITA] Functie folosita pentru a te duce la pagina Google Play a aplicatiei [Aplicatia Google Play nu exista pentru aceasta aplicatie]
    //public void RateApp() {
    //    try { Application.OpenURL("market://details?id=" + Application.identifier); }
    //    catch { Application.OpenURL("https://play.google.com/store/apps/details?id=" + Application.identifier); }
    //}

    // Functie executata cand template-ul selectat este schimbat
    public void SelectTemplatwe(int t) {
        PresetSelectr = t;
        if (PlayerPrefs.GetInt("HASHINT1", 0) == 0) {
            if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(0f, 0f), "Creating Presets", "When creating a preset, you must give it a name, and you can select a pre-made template or an empty one", 1400f, 0.3f, 1);
            if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(0f, 0f), "Création de préréglages", "Lors de la création d'un préréglage, vous devez lui donner un nom et vous pouvez sélectionner un modèle prédéfini ou un modèle vide", 1400f, 0.3f, 1);
            if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(0f, 0f), "Voreinstellungen erstellen", "Beim Erstellen einer Voreinstellung müssen Sie ihr einen Namen geben und können eine vorgefertigte Vorlage oder eine leere Vorlage auswählen", 1400f, 0.3f, 1);
            if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(0f, 0f), "Crearea presetărilor", "Când creați o presetare, trebuie să îi dați un nume și puteți selecta un șablon predefinit sau unul gol", 1400f, 0.3f, 1);
        } if (PresetSelectr == 1 || PresetSelectr == 2 || PresetSelectr == 3) { colorschem.SetActive(true); ColorSchemeSetM(PresetSelectr + 1); } 
        else { colorschem.SetActive(false); }
    }

    // Sistemul propriu-zis executat la stergerea presetarii selectate
    public void DeleteData() {
        #region System
        int SelPres = PlayerPrefs.GetInt("SELECTEDPRESET");
        int Objects = (PlayerPrefs.GetInt("PRESET" + SelPres + "TOTALOBJ") - PlayerPrefs.GetInt("PRESET" + SelPres + "DELOBJ"));
        for (int o = 0; o < Objects; o++) { string colorstart = "PRESET" + SelPres + "ELEM" + o;
            if(PlayerPrefs.GetInt(colorstart + "TYPE") == 0) {
                PlayerPrefs.DeleteKey(colorstart + "ASSFAM");
                PlayerPrefs.DeleteKey(colorstart + "ASEFAM");
                PlayerPrefs.DeleteKey(colorstart + "ASOSAM");
                PlayerPrefs.DeleteKey(colorstart + "ASLWAM");
                PlayerPrefs.DeleteKey(colorstart + "ASFDAM");
                PlayerPrefs.DeleteKey(colorstart + "ASBSAM");
                PlayerPrefs.DeleteKey(colorstart + "ASNWAM");
                PlayerPrefs.DeleteKey(colorstart + "ASNSAM");
                PlayerPrefs.DeleteKey(colorstart + "ASELAM");
                PlayerPrefs.DeleteKey(colorstart + "ASDCAM");
                PlayerPrefs.DeleteKey(colorstart + "ASMSIF");
                PlayerPrefs.DeleteKey(colorstart + "ASC2IF");
                PlayerPrefs.DeleteKey(colorstart + "ASMRIF");
                PlayerPrefs.DeleteKey(colorstart + "ASILIF");
                PlayerPrefs.DeleteKey(colorstart + "ASMAAS");
                PlayerPrefs.DeleteKey(colorstart + "ASTMAS");
                PlayerPrefs.DeleteKey(colorstart + "ASFONT");
                PlayerPrefs.DeleteKey(colorstart + "ASLOAM");
                PlayerPrefs.DeleteKey(colorstart + "ASLASI");
                PlayerPrefs.DeleteKey(colorstart + "ASSPMD");
                PlayerPrefs.DeleteKey(colorstart + "ASLABL");
                PlayerPrefs.DeleteKey(colorstart + "ASBACKCLR"); PlayerPrefs.DeleteKey(colorstart + "ASBACKCLG"); PlayerPrefs.DeleteKey(colorstart + "ASBACKCLB");
                PlayerPrefs.DeleteKey(colorstart + "ASOUTLCLR"); PlayerPrefs.DeleteKey(colorstart + "ASOUTLCLG"); PlayerPrefs.DeleteKey(colorstart + "ASOUTLCLB");
                PlayerPrefs.DeleteKey(colorstart + "ASC1CLLIR"); PlayerPrefs.DeleteKey(colorstart + "ASC1CLLIG"); PlayerPrefs.DeleteKey(colorstart + "ASC1CLLIB");
                PlayerPrefs.DeleteKey(colorstart + "ASC2CLLIR"); PlayerPrefs.DeleteKey(colorstart + "ASC2CLLIG"); PlayerPrefs.DeleteKey(colorstart + "ASC2CLLIB");
                PlayerPrefs.DeleteKey(colorstart + "ASNEEDCLR"); PlayerPrefs.DeleteKey(colorstart + "ASNEEDCLG"); PlayerPrefs.DeleteKey(colorstart + "ASNEEDCLB");
                PlayerPrefs.DeleteKey(colorstart + "ASMAR1CLR"); PlayerPrefs.DeleteKey(colorstart + "ASMAR1CLG"); PlayerPrefs.DeleteKey(colorstart + "ASMAR1CLB");
                PlayerPrefs.DeleteKey(colorstart + "ASMAR2CLR"); PlayerPrefs.DeleteKey(colorstart + "ASMAR2CLG"); PlayerPrefs.DeleteKey(colorstart + "ASMAR2CLB");
                PlayerPrefs.DeleteKey(colorstart + "ASUNLACLR"); PlayerPrefs.DeleteKey(colorstart + "ASUNLACLG"); PlayerPrefs.DeleteKey(colorstart + "ASUNLACLB");
            } if(PlayerPrefs.GetInt(colorstart + "TYPE") == 1) {
                PlayerPrefs.DeleteKey(colorstart + "ADDNSE");
                PlayerPrefs.DeleteKey(colorstart + "ADREFR");
                PlayerPrefs.DeleteKey(colorstart + "ADMSDD");
                PlayerPrefs.DeleteKey(colorstart + "ADSDTG");
                PlayerPrefs.DeleteKey(colorstart + "ADFONT");
                PlayerPrefs.DeleteKey(colorstart + "ADANIM");
                PlayerPrefs.DeleteKey(colorstart + "ADDGCLR"); PlayerPrefs.DeleteKey(colorstart + "ADDGCLG"); PlayerPrefs.DeleteKey(colorstart + "ADDGCLB");
                PlayerPrefs.DeleteKey(colorstart + "ADDG2CLR"); PlayerPrefs.DeleteKey(colorstart + "ADDG2CLG"); PlayerPrefs.DeleteKey(colorstart + "ADDG2CLB");
                PlayerPrefs.DeleteKey(colorstart + "ADDGTXCLR"); PlayerPrefs.DeleteKey(colorstart + "ADDGTXCLG"); PlayerPrefs.DeleteKey(colorstart + "ADDGTXCLB");
                PlayerPrefs.DeleteKey(colorstart + "ADDG2TXCLR"); PlayerPrefs.DeleteKey(colorstart + "ADDG2TXCLG"); PlayerPrefs.DeleteKey(colorstart + "ADDG2TXCLB");
            } if(PlayerPrefs.GetInt(colorstart + "TYPE") == 2) {
                PlayerPrefs.DeleteKey(colorstart + "LSDNSE");
                PlayerPrefs.DeleteKey(colorstart + "LSDMSZ");
                PlayerPrefs.DeleteKey(colorstart + "LSREFR");
                PlayerPrefs.DeleteKey(colorstart + "LSBRNT");
                PlayerPrefs.DeleteKey(colorstart + "LSCBRN");
                PlayerPrefs.DeleteKey(colorstart + "LSISHD");
                PlayerPrefs.DeleteKey(colorstart + "LSSFNT");
                PlayerPrefs.DeleteKey(colorstart + "LSDECI");
                PlayerPrefs.DeleteKey(colorstart + "LSBACK");
                PlayerPrefs.DeleteKey(colorstart + "LSMLD1");
                PlayerPrefs.DeleteKey(colorstart + "LSFONT");
                PlayerPrefs.DeleteKey(colorstart + "LSSTRX");
                PlayerPrefs.DeleteKey(colorstart + "LSSTRY");
                PlayerPrefs.DeleteKey(colorstart + "LSMTPR");
                PlayerPrefs.DeleteKey(colorstart + "LSSPUN");
                PlayerPrefs.DeleteKey(colorstart + "LSBACKCLR"); PlayerPrefs.DeleteKey(colorstart + "LSBACKCLG"); PlayerPrefs.DeleteKey(colorstart + "LSBACKCLB");
                PlayerPrefs.DeleteKey(colorstart + "LSDGITCLR"); PlayerPrefs.DeleteKey(colorstart + "LSDGITCLG"); PlayerPrefs.DeleteKey(colorstart + "LSDGITCLB");
            } if(PlayerPrefs.GetInt(colorstart + "TYPE") == 3) {
                PlayerPrefs.DeleteKey(colorstart + "LDDNSE");
                PlayerPrefs.DeleteKey(colorstart + "LDDCML");
                PlayerPrefs.DeleteKey(colorstart + "LDREFR");
                PlayerPrefs.DeleteKey(colorstart + "LDBRNT");
                PlayerPrefs.DeleteKey(colorstart + "LDCBRN");
                PlayerPrefs.DeleteKey(colorstart + "LDISHD");
                PlayerPrefs.DeleteKey(colorstart + "LDMODE");
                PlayerPrefs.DeleteKey(colorstart + "LDSFNT");
                PlayerPrefs.DeleteKey(colorstart + "LDBACK");
                PlayerPrefs.DeleteKey(colorstart + "LDMLD1");
                PlayerPrefs.DeleteKey(colorstart + "LDFONT");
                PlayerPrefs.DeleteKey(colorstart + "LDSTRX");
                PlayerPrefs.DeleteKey(colorstart + "LDSTRY");
                PlayerPrefs.DeleteKey(colorstart + "LDMTPR");
                PlayerPrefs.DeleteKey(colorstart + "LDBACKCLR"); PlayerPrefs.DeleteKey(colorstart + "LDBACKCLG"); PlayerPrefs.DeleteKey(colorstart + "LDBACKCLB");
                PlayerPrefs.DeleteKey(colorstart + "LDDGITCLR"); PlayerPrefs.DeleteKey(colorstart + "LDDGITCLG"); PlayerPrefs.DeleteKey(colorstart + "LDDGITCLB");
            } if(PlayerPrefs.GetInt(colorstart + "TYPE") == 4) {
                PlayerPrefs.DeleteKey(colorstart + "ASBHEIG");
                PlayerPrefs.DeleteKey(colorstart + "ASBWIDT");
                PlayerPrefs.DeleteKey(colorstart + "ASBBOWI");
                PlayerPrefs.DeleteKey(colorstart + "ASBFILO");
                PlayerPrefs.DeleteKey(colorstart + "ASBNEDW");
                PlayerPrefs.DeleteKey(colorstart + "ASBNEWT");
                PlayerPrefs.DeleteKey(colorstart + "ASBLIMT");
                PlayerPrefs.DeleteKey(colorstart + "ASBC2ST");
                PlayerPrefs.DeleteKey(colorstart + "ASBMRRT");
                PlayerPrefs.DeleteKey(colorstart + "ASBINLN");
                PlayerPrefs.DeleteKey(colorstart + "ASBMARW");
                PlayerPrefs.DeleteKey(colorstart + "ASBMTXW");
                PlayerPrefs.DeleteKey(colorstart + "ASBFONT");
                PlayerPrefs.DeleteKey(colorstart + "ASBSPMD");
                PlayerPrefs.DeleteKey(colorstart + "ASBFILL");
                PlayerPrefs.DeleteKey(colorstart + "ASBACKCLR"); PlayerPrefs.DeleteKey(colorstart + "ASBACKCLG"); PlayerPrefs.DeleteKey(colorstart + "ASBACKCLB");
                PlayerPrefs.DeleteKey(colorstart + "ASOUTLCLR"); PlayerPrefs.DeleteKey(colorstart + "ASOUTLCLG"); PlayerPrefs.DeleteKey(colorstart + "ASOUTLCLB");
                PlayerPrefs.DeleteKey(colorstart + "ASC1CLLIR"); PlayerPrefs.DeleteKey(colorstart + "ASC1CLLIG"); PlayerPrefs.DeleteKey(colorstart + "ASC1CLLIB");
                PlayerPrefs.DeleteKey(colorstart + "ASC2CLLIR"); PlayerPrefs.DeleteKey(colorstart + "ASC2CLLIG"); PlayerPrefs.DeleteKey(colorstart + "ASC2CLLIB");
                PlayerPrefs.DeleteKey(colorstart + "ASNEEDCLR"); PlayerPrefs.DeleteKey(colorstart + "ASNEEDCLG"); PlayerPrefs.DeleteKey(colorstart + "ASNEEDCLB");
                PlayerPrefs.DeleteKey(colorstart + "ASMAR1CLR"); PlayerPrefs.DeleteKey(colorstart + "ASMAR1CLG"); PlayerPrefs.DeleteKey(colorstart + "ASMAR1CLB");
                PlayerPrefs.DeleteKey(colorstart + "ASMAR2CLR"); PlayerPrefs.DeleteKey(colorstart + "ASMAR2CLG"); PlayerPrefs.DeleteKey(colorstart + "ASMAR2CLB");
            } if(PlayerPrefs.GetInt(colorstart + "TYPE") == 5) {
                PlayerPrefs.DeleteKey(colorstart + "ACSBVFIL");
                PlayerPrefs.DeleteKey(colorstart + "ACSBVTIC");
                PlayerPrefs.DeleteKey(colorstart + "ACSBBOWI");
                PlayerPrefs.DeleteKey(colorstart + "ACSBFILO");
                PlayerPrefs.DeleteKey(colorstart + "ACSBNEDW");
                PlayerPrefs.DeleteKey(colorstart + "ACSBNEWT");
                PlayerPrefs.DeleteKey(colorstart + "ACSBLIMT");
                PlayerPrefs.DeleteKey(colorstart + "ACSBC2ST");
                PlayerPrefs.DeleteKey(colorstart + "ACSBMRRT");
                PlayerPrefs.DeleteKey(colorstart + "ACSBINLN");
                PlayerPrefs.DeleteKey(colorstart + "ACSBMARW");
                PlayerPrefs.DeleteKey(colorstart + "ACSBMTXW");
                PlayerPrefs.DeleteKey(colorstart + "ACSBFONT");
                PlayerPrefs.DeleteKey(colorstart + "ACSBCCWI");
                PlayerPrefs.DeleteKey(colorstart + "ACSBSPMD");
                PlayerPrefs.DeleteKey(colorstart + "ACSBFILL");
                PlayerPrefs.DeleteKey(colorstart + "ACSBBORDCLR"); PlayerPrefs.DeleteKey(colorstart + "ACSBBORDCLG"); PlayerPrefs.DeleteKey(colorstart + "ACSBBORDCLB");
                PlayerPrefs.DeleteKey(colorstart + "ACSBBACKCLR"); PlayerPrefs.DeleteKey(colorstart + "ACSBBACKCLG"); PlayerPrefs.DeleteKey(colorstart + "ACSBBACKCLB");
                PlayerPrefs.DeleteKey(colorstart + "ACSBNEEDCLR"); PlayerPrefs.DeleteKey(colorstart + "ACSBNEEDCLG"); PlayerPrefs.DeleteKey(colorstart + "ACSBNEEDCLB");
                PlayerPrefs.DeleteKey(colorstart + "ACSBFILLCLR"); PlayerPrefs.DeleteKey(colorstart + "ACSBFILLCLG"); PlayerPrefs.DeleteKey(colorstart + "ACSBFILLCLB");
                PlayerPrefs.DeleteKey(colorstart + "ACSBMKC1CLR"); PlayerPrefs.DeleteKey(colorstart + "ACSBMKC1CLG"); PlayerPrefs.DeleteKey(colorstart + "ACSBMKC1CLB");
                PlayerPrefs.DeleteKey(colorstart + "ACSBMKC2CLR"); PlayerPrefs.DeleteKey(colorstart + "ACSBMKC2CLG"); PlayerPrefs.DeleteKey(colorstart + "ACSBMKC2CLB");
            } if(PlayerPrefs.GetInt(colorstart + "TYPE") == 6) {
                PlayerPrefs.DeleteKey(colorstart + "LDSBHEIG");
                PlayerPrefs.DeleteKey(colorstart + "LDSBWIDT");
                PlayerPrefs.DeleteKey(colorstart + "LDSBREFR");
                PlayerPrefs.DeleteKey(colorstart + "LDSBBRNT");
                PlayerPrefs.DeleteKey(colorstart + "LDSBCBRN");
                PlayerPrefs.DeleteKey(colorstart + "LDSBISHD");
                PlayerPrefs.DeleteKey(colorstart + "LDSBSEGC");
                PlayerPrefs.DeleteKey(colorstart + "LDSBSEGG");
                PlayerPrefs.DeleteKey(colorstart + "LDSBTOPS");
                PlayerPrefs.DeleteKey(colorstart + "LDSBSPUN");
                PlayerPrefs.DeleteKey(colorstart + "LDSBBACK");
                PlayerPrefs.DeleteKey(colorstart + "LDSBBACKCLR"); PlayerPrefs.DeleteKey(colorstart + "LDSBBACKCLG"); PlayerPrefs.DeleteKey(colorstart + "LDSBBACKCLB");
                PlayerPrefs.DeleteKey(colorstart + "LDSBDGITCLR"); PlayerPrefs.DeleteKey(colorstart + "LDSBDGITCLG"); PlayerPrefs.DeleteKey(colorstart + "LDSBDGITCLB");
            } if(PlayerPrefs.GetInt(colorstart + "TYPE") == 7) {
                PlayerPrefs.DeleteKey(colorstart + "LDCSBHEIG");
                PlayerPrefs.DeleteKey(colorstart + "LDCSBWIDT");
                PlayerPrefs.DeleteKey(colorstart + "LDCSBREFR");
                PlayerPrefs.DeleteKey(colorstart + "LDCSBBRNT");
                PlayerPrefs.DeleteKey(colorstart + "LDCSBCBRN");
                PlayerPrefs.DeleteKey(colorstart + "LDCSBISHD");
                PlayerPrefs.DeleteKey(colorstart + "LDCSBSEGC");
                PlayerPrefs.DeleteKey(colorstart + "LDCSBSEGG");
                PlayerPrefs.DeleteKey(colorstart + "LDCSBTOPS");
                PlayerPrefs.DeleteKey(colorstart + "LDCSCCWIS");
                PlayerPrefs.DeleteKey(colorstart + "LDCSBSPUN");
                PlayerPrefs.DeleteKey(colorstart + "LDCSBBACK");
                PlayerPrefs.DeleteKey(colorstart + "LDCSBBACKCLR"); PlayerPrefs.DeleteKey(colorstart + "LDCSBBACKCLG"); PlayerPrefs.DeleteKey(colorstart + "LDCSBBACKCLB");
                PlayerPrefs.DeleteKey(colorstart + "LDCSBDGITCLR"); PlayerPrefs.DeleteKey(colorstart + "LDCSBDGITCLG"); PlayerPrefs.DeleteKey(colorstart + "LDCSBDGITCLB");
            } if(PlayerPrefs.GetInt(colorstart + "TYPE") == 8) {
                PlayerPrefs.DeleteKey(colorstart + "LNTHIC");
                PlayerPrefs.DeleteKey(colorstart + "LNLENG");
                PlayerPrefs.DeleteKey(colorstart + "LNM1B");
                PlayerPrefs.DeleteKey(colorstart + "LNMODE");
                PlayerPrefs.DeleteKey(colorstart + "LNLINECLR"); PlayerPrefs.DeleteKey(colorstart + "LNLINECLG"); PlayerPrefs.DeleteKey(colorstart + "LNLINECLB");
            } if(PlayerPrefs.GetInt(colorstart + "TYPE") == 9) {
                PlayerPrefs.DeleteKey(colorstart + "TXWIDT");
                PlayerPrefs.DeleteKey(colorstart + "TXHEIG");
                PlayerPrefs.DeleteKey(colorstart + "TXTEXT");
                PlayerPrefs.DeleteKey(colorstart + "TXFONT");
                PlayerPrefs.DeleteKey(colorstart + "TXFONTF");
                PlayerPrefs.DeleteKey(colorstart + "TXBOIT");
                PlayerPrefs.DeleteKey(colorstart + "TXLNSP");
                PlayerPrefs.DeleteKey(colorstart + "TXKERN");
                PlayerPrefs.DeleteKey(colorstart + "TXALMD");
                PlayerPrefs.DeleteKey(colorstart + "TXWRMD");
                PlayerPrefs.DeleteKey(colorstart + "TXFTSZ");
                PlayerPrefs.DeleteKey(colorstart + "TXDYNAC");
                PlayerPrefs.DeleteKey(colorstart + "TXMXDE");
                PlayerPrefs.DeleteKey(colorstart + "TXMXDI");
                PlayerPrefs.DeleteKey(colorstart + "TXRRAT");
                PlayerPrefs.DeleteKey(colorstart + "TXDYNA");
                PlayerPrefs.DeleteKey(colorstart + "TXTEXTCLR"); PlayerPrefs.DeleteKey(colorstart + "TXTEXTCLG"); PlayerPrefs.DeleteKey(colorstart + "TXTEXTCLB");
                PlayerPrefs.DeleteKey(colorstart + "TMPMEFFECT");
                PlayerPrefs.DeleteKey(colorstart + "TMPMBCOLMO");
                PlayerPrefs.DeleteKey(colorstart + "TMPMSGRTYP");
                PlayerPrefs.DeleteKey(colorstart + "BTXTCOLR"); PlayerPrefs.DeleteKey(colorstart + "BTXTCOLG"); PlayerPrefs.DeleteKey(colorstart + "BTXTCOLB");
                PlayerPrefs.DeleteKey(colorstart + "BTXTG21R"); PlayerPrefs.DeleteKey(colorstart + "BTXTG21G"); PlayerPrefs.DeleteKey(colorstart + "BTXTG21B");
                PlayerPrefs.DeleteKey(colorstart + "BTXTG22R"); PlayerPrefs.DeleteKey(colorstart + "BTXTG22G"); PlayerPrefs.DeleteKey(colorstart + "BTXTG22B");
                PlayerPrefs.DeleteKey(colorstart + "BTXTG41R"); PlayerPrefs.DeleteKey(colorstart + "BTXTG41G"); PlayerPrefs.DeleteKey(colorstart + "BTXTG41B");
                PlayerPrefs.DeleteKey(colorstart + "BTXTG42R"); PlayerPrefs.DeleteKey(colorstart + "BTXTG42G"); PlayerPrefs.DeleteKey(colorstart + "BTXTG42B");
                PlayerPrefs.DeleteKey(colorstart + "BTXTG43R"); PlayerPrefs.DeleteKey(colorstart + "BTXTG43G"); PlayerPrefs.DeleteKey(colorstart + "BTXTG43B");
                PlayerPrefs.DeleteKey(colorstart + "BTXTG44R"); PlayerPrefs.DeleteKey(colorstart + "BTXTG44G"); PlayerPrefs.DeleteKey(colorstart + "BTXTG44B");
                PlayerPrefs.DeleteKey(colorstart + "TMPMBDILAT"); PlayerPrefs.DeleteKey(colorstart + "TMPMBSOFTN");
                PlayerPrefs.DeleteKey(colorstart + "STXTCOLR"); PlayerPrefs.DeleteKey(colorstart + "STXTCOLG"); PlayerPrefs.DeleteKey(colorstart + "STXTCOLB");
                PlayerPrefs.DeleteKey(colorstart + "STXTG21R"); PlayerPrefs.DeleteKey(colorstart + "STXTG21G"); PlayerPrefs.DeleteKey(colorstart + "STXTG21B");
                PlayerPrefs.DeleteKey(colorstart + "STXTG22R"); PlayerPrefs.DeleteKey(colorstart + "STXTG22G"); PlayerPrefs.DeleteKey(colorstart + "STXTG22B");
                PlayerPrefs.DeleteKey(colorstart + "STXTG41R"); PlayerPrefs.DeleteKey(colorstart + "STXTG41G"); PlayerPrefs.DeleteKey(colorstart + "STXTG41B");
                PlayerPrefs.DeleteKey(colorstart + "STXTG42R"); PlayerPrefs.DeleteKey(colorstart + "STXTG42G"); PlayerPrefs.DeleteKey(colorstart + "STXTG42B");
                PlayerPrefs.DeleteKey(colorstart + "STXTG43R"); PlayerPrefs.DeleteKey(colorstart + "STXTG43G"); PlayerPrefs.DeleteKey(colorstart + "STXTG43B");
                PlayerPrefs.DeleteKey(colorstart + "STXTG44R"); PlayerPrefs.DeleteKey(colorstart + "STXTG44G"); PlayerPrefs.DeleteKey(colorstart + "STXTG44B");
                PlayerPrefs.DeleteKey(colorstart + "TMPMSDILAT"); PlayerPrefs.DeleteKey(colorstart + "TMPMSSOFTN");
            } if(PlayerPrefs.GetInt(colorstart + "TYPE") == 10) {
                PlayerPrefs.DeleteKey(colorstart + "ICLDMD");
                PlayerPrefs.DeleteKey(colorstart + "ICSPLC");
                PlayerPrefs.DeleteKey(colorstart + "ICFILT");
                PlayerPrefs.DeleteKey(colorstart + "ICWIDT");
                PlayerPrefs.DeleteKey(colorstart + "ICHEIG");
                PlayerPrefs.DeleteKey(colorstart + "ICFMOD");
                PlayerPrefs.DeleteKey(colorstart + "ICFORG");
                PlayerPrefs.DeleteKey(colorstart + "ICFAMT");
                PlayerPrefs.DeleteKey(colorstart + "ICIMSZ"); string stx = colorstart + "ICSPLC";
                PlayerPrefs.DeleteKey(stx + "1"); PlayerPrefs.DeleteKey(stx + "2"); PlayerPrefs.DeleteKey(stx + "3"); PlayerPrefs.DeleteKey(stx + "4");
                PlayerPrefs.DeleteKey(colorstart + "ICIMGCLR"); PlayerPrefs.DeleteKey(colorstart + "ICIMGCLG"); PlayerPrefs.DeleteKey(colorstart + "ICIMGCLB");
                PlayerPrefs.DeleteKey(colorstart + "ICPATH");
            } if(PlayerPrefs.GetInt(colorstart + "TYPE") == 11) {
                PlayerPrefs.DeleteKey(colorstart + "COOUTS");
                PlayerPrefs.DeleteKey(colorstart + "CONEDS");
                PlayerPrefs.DeleteKey(colorstart + "COMRKS");
                PlayerPrefs.DeleteKey(colorstart + "CONSEW");
                PlayerPrefs.DeleteKey(colorstart + "COPERS");
                PlayerPrefs.DeleteKey(colorstart + "COFADE");
                PlayerPrefs.DeleteKey(colorstart + "COMO3D");
                PlayerPrefs.DeleteKey(colorstart + "COLKNE");
                PlayerPrefs.DeleteKey(colorstart + "CODET1");
                PlayerPrefs.DeleteKey(colorstart + "COSDLN");
                PlayerPrefs.DeleteKey(colorstart + "COSNLN");
                PlayerPrefs.DeleteKey(colorstart + "COSALN");
                PlayerPrefs.DeleteKey(colorstart + "COOUTLCLR"); PlayerPrefs.DeleteKey(colorstart + "COOUTLCLG"); PlayerPrefs.DeleteKey(colorstart + "COOUTLCLB");
                PlayerPrefs.DeleteKey(colorstart + "COBACKCLR"); PlayerPrefs.DeleteKey(colorstart + "COBACKCLG"); PlayerPrefs.DeleteKey(colorstart + "COBACKCLB");
                PlayerPrefs.DeleteKey(colorstart + "CONEDFCLR"); PlayerPrefs.DeleteKey(colorstart + "CONEDFCLG"); PlayerPrefs.DeleteKey(colorstart + "CONEDFCLB");
                PlayerPrefs.DeleteKey(colorstart + "CONEDRCLR"); PlayerPrefs.DeleteKey(colorstart + "CONEDRCLG"); PlayerPrefs.DeleteKey(colorstart + "CONEDRCLB");
                PlayerPrefs.DeleteKey(colorstart + "COMARKCLR"); PlayerPrefs.DeleteKey(colorstart + "COMARKCLG"); PlayerPrefs.DeleteKey(colorstart + "COMARKCLB");
                PlayerPrefs.DeleteKey(colorstart + "CODETACLR"); PlayerPrefs.DeleteKey(colorstart + "CODETACLG"); PlayerPrefs.DeleteKey(colorstart + "CODETACLB");
            } if(PlayerPrefs.GetInt(colorstart + "TYPE") == 12) {
                PlayerPrefs.DeleteKey(colorstart + "MPWIDT");
                PlayerPrefs.DeleteKey(colorstart + "MPHEIG");
                PlayerPrefs.DeleteKey(colorstart + "MPSELM");
                PlayerPrefs.DeleteKey(colorstart + "MPBACKCLR"); PlayerPrefs.DeleteKey(colorstart + "MPBACKCLG"); PlayerPrefs.DeleteKey(colorstart + "MPBACKCLB");
                for (int d = 0; d < 9999; d++) if (PlayerPrefs.GetString(colorstart + "MPLAYOLCOMP" + d, "./") == "./") break; else PlayerPrefs.DeleteKey(colorstart + "MPLAYOLCOMP" + d);
            } PlayerPrefs.DeleteKey(colorstart + "TYPE"); PlayerPrefs.DeleteKey(colorstart + "SO");
        } PlayerPrefs.DeleteKey("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BACKMODE");
        PlayerPrefs.DeleteKey("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICLDMD");
        PlayerPrefs.DeleteKey("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICSPLC");
        PlayerPrefs.DeleteKey("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFILT");
        PlayerPrefs.DeleteKey("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFMOD");
        PlayerPrefs.DeleteKey("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFORG");
        PlayerPrefs.DeleteKey("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFAMT");
        PlayerPrefs.DeleteKey("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICIMSZ");
        string st = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICSPLC";
        PlayerPrefs.DeleteKey(st + "1"); PlayerPrefs.DeleteKey(st + "2"); PlayerPrefs.DeleteKey(st + "3"); PlayerPrefs.DeleteKey(st + "4");
        string colorstart2 = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMG";
        PlayerPrefs.DeleteKey(colorstart2 + "ICIMGCLR"); PlayerPrefs.DeleteKey(colorstart2 + "ICIMGCLG"); PlayerPrefs.DeleteKey(colorstart2 + "ICIMGCLB");
        PlayerPrefs.GetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICPATH"); StartCoroutine(Almost());
        #endregion
    }

    // Sistemul executat la redenumirea unei presetari
    public void RenamePreset(InputField input) {
        PlayerPrefs.SetString("PRES" + PlayerPrefs.GetInt("SELECTEDPRESET") + "NAME", input.text);
        SelctButton.transform.GetChild(2).GetComponent<Text>().text = input.text;
        OpenPanel.transform.GetChild(0).GetComponent<Text>().text = input.text;
    }

    // Functie executata la schimbarea paletei de culoare la template-urile LCD
    public void ColorSchemeSetM(int M) { OnColorModdify();
        if(M == 2) { clr4.sprite = allms[0]; clr1.GetComponent<Button>().onClick.RemoveAllListeners(); clr1.GetComponent<Button>().onClick.AddListener(() => { ChangeColors("P1CKEY1"); });
            clr2.GetComponent<Button>().onClick.RemoveAllListeners(); clr2.GetComponent<Button>().onClick.AddListener(() => { ChangeColors("P1CKEY2"); }); }
        if(M == 3) { clr4.sprite = allms[1]; clr1.GetComponent<Button>().onClick.RemoveAllListeners(); clr1.GetComponent<Button>().onClick.AddListener(() => { ChangeColors("P2CKEY1"); });
            clr2.GetComponent<Button>().onClick.RemoveAllListeners(); clr2.GetComponent<Button>().onClick.AddListener(() => { ChangeColors("P2CKEY2"); }); }
        if(M == 4) { clr4.sprite = allms[2]; clr1.GetComponent<Button>().onClick.RemoveAllListeners(); clr1.GetComponent<Button>().onClick.AddListener(() => { ChangeColors("P3CKEY1"); });
            clr2.GetComponent<Button>().onClick.RemoveAllListeners(); clr2.GetComponent<Button>().onClick.AddListener(() => { ChangeColors("P3CKEY2"); }); }
    }

    // Functie executata la cererea de a schimba o culoare
    public void ChangeColors(string KEY) {
        cp.RequestChangeColor(KEY, gameObject);
    }

    // Functie executata dupa schimbarea unei culori
    public void OnColorModdify() {
        if (PresetSelectr == 1) {
            clr1.color = new Color(PlayerPrefs.GetFloat("P1CKEY1R", 0.55f), PlayerPrefs.GetFloat("P1CKEY1G", 0.62f), PlayerPrefs.GetFloat("P1CKEY1B", 0.3f));
            clr3.color = new Color(PlayerPrefs.GetFloat("P1CKEY1R", 0.55f), PlayerPrefs.GetFloat("P1CKEY1G", 0.62f), PlayerPrefs.GetFloat("P1CKEY1B", 0.3f));
            clr2.color = new Color(PlayerPrefs.GetFloat("P1CKEY2R", 0.12f), PlayerPrefs.GetFloat("P1CKEY2G", 0.12f), PlayerPrefs.GetFloat("P1CKEY2B", 0.12f));
            clr4.color = new Color(PlayerPrefs.GetFloat("P1CKEY2R", 0.12f), PlayerPrefs.GetFloat("P1CKEY2G", 0.12f), PlayerPrefs.GetFloat("P1CKEY2B", 0.12f));
        } else if (PresetSelectr == 2) {
            clr1.color = new Color(PlayerPrefs.GetFloat("P2CKEY1R", 0), PlayerPrefs.GetFloat("P2CKEY1G", 0), PlayerPrefs.GetFloat("P2CKEY1B", 0));
            clr3.color = new Color(PlayerPrefs.GetFloat("P2CKEY1R", 0), PlayerPrefs.GetFloat("P2CKEY1G", 0), PlayerPrefs.GetFloat("P2CKEY1B", 0));
            clr2.color = new Color(PlayerPrefs.GetFloat("P2CKEY2R", 0), PlayerPrefs.GetFloat("P2CKEY2G", 0.9f), PlayerPrefs.GetFloat("P2CKEY2B", 1));
            clr4.color = new Color(PlayerPrefs.GetFloat("P2CKEY2R", 0), PlayerPrefs.GetFloat("P2CKEY2G", 0.9f), PlayerPrefs.GetFloat("P2CKEY2B", 1));
        } else if (PresetSelectr == 3) {
            clr1.color = new Color(PlayerPrefs.GetFloat("P3CKEY1R", 1), PlayerPrefs.GetFloat("P3CKEY1G", 1), PlayerPrefs.GetFloat("P3CKEY1B", 0.75f));
            clr3.color = new Color(PlayerPrefs.GetFloat("P3CKEY1R", 1), PlayerPrefs.GetFloat("P3CKEY1G", 1), PlayerPrefs.GetFloat("P3CKEY1B", 0.75f));
            clr2.color = new Color(PlayerPrefs.GetFloat("P3CKEY2R", 0.15f), PlayerPrefs.GetFloat("P3CKEY2G", 0.15f), PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
            clr4.color = new Color(PlayerPrefs.GetFloat("P3CKEY2R", 0.15f), PlayerPrefs.GetFloat("P3CKEY2G", 0.15f), PlayerPrefs.GetFloat("P3CKEY2B", 0.15f));
        }
    }

    // Functie executata pentru a incarca toate presetarile vizual pe interfata ecranului
    public IEnumerator Almost() {
        int ButtonID = int.Parse(SelctButton.name); Destroy(SelctButton);
        yield return null; int Prog = 0;
        for (int v = 0; v < orgfel.childCount; v++) {
            if (orgfel.GetChild(v).gameObject.name != "Add Button") { orgfel.GetChild(v).gameObject.name = Prog + "";
                orgfel.GetChild(v).GetComponent<RectTransform>().anchoredPosition = new Vector2(-unitlenght + (unitlenght * (Prog % 3)), -20f - (400f * (Prog / 3))); Prog++;
            } else AddButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-unitlenght + (unitlenght * ((orgfel.childCount - 1) % 3)), -20f - (400f * ((orgfel.childCount - 1) / 3)));
            orgfel.GetComponent<RectTransform>().sizeDelta = new Vector2(orgfel.GetComponent<RectTransform>().sizeDelta.x , 440f + (400f * (orgfel.childCount / 3)));
            orgfel.parent.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(unitlenght * 3f, 830f);
        } LOAD.SetActive(false);
    }

    // Functie executata cand sursa de viteza este schimbata
    public void SelectOption(int Op) {
        LangOptionSelected = Op; if(Op == 0) { ModesB[1].sprite = ModesBTex[0]; ModesB[2].sprite = ModesBTex[0];
            ModesB[0].sprite = ModesBTex[1]; } else if(Op == 1)
        { ModesB[0].sprite = ModesBTex[0]; ModesB[1].sprite = ModesBTex[1]; ModesB[2].sprite = ModesBTex[0]; }
        else if (Op == 2) { ModesB[0].sprite = ModesBTex[0]; 
            ModesB[1].sprite = ModesBTex[0]; ModesB[2].sprite = ModesBTex[1]; }
    }

    // Functie executata cand limbajul aplicatiei este schimbat
    public void SelectOptionL(int Op) {
        LangOptionSelected = Op; if(Op == 0) { LModesB[1].sprite = ModesBTex[0]; LModesB[2].sprite = ModesBTex[0];
            LModesB[0].sprite = ModesBTex[1]; LModesB[3].sprite = ModesBTex[0]; } else if(Op == 1) { LModesB[0].sprite = ModesBTex[0]; 
            LModesB[1].sprite = ModesBTex[1]; LModesB[2].sprite = ModesBTex[0]; LModesB[3].sprite = ModesBTex[0];  }
        else if (Op == 2) { LModesB[0].sprite = ModesBTex[0]; LModesB[3].sprite = ModesBTex[0];
            LModesB[1].sprite = ModesBTex[0]; LModesB[2].sprite = ModesBTex[1]; }
        else if (Op == 3) { LModesB[0].sprite = ModesBTex[0]; LModesB[2].sprite = ModesBTex[0];
            LModesB[1].sprite = ModesBTex[0]; LModesB[3].sprite = ModesBTex[1]; }
    }

    // Functie executata la prima deschidere a aplicatiei pentru a trece mai departe cu instructiuniile de inceput (Etapa 1)
    public void Continue0() {
        PlayerPrefs.SetInt("MAINLANG", LangOptionSelected);
        PlayerPrefs.SetInt("INSETUPLANG", 1);
        SceneManager.LoadScene(0);
    }

    // Functie executata la prima deschidere a aplicatiei pentru a trece mai departe cu instructiuniile de inceput (Etapa 2)
    public void Continue1() {
        PlayerPrefs.SetInt("SPEEDSOURCE", OptionSelected);
        InitialSetup.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        InitialSetup.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
    }

    // Functie executata pentru a incerca conectarea la GPS inca o data
    public void TryAgain() { SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>(); GPSStart.SetActive(true); SM.InitGPS(); }

    // Functie executata la cererea de permisiune pentru a folosi GPS-ul disozitivului
    public void AskForPerm() { Permission.RequestUserPermission(Permission.FineLocation); TryAgain(); }

    // Functie executata la prima deschidere a aplicatiei pentru a trece mai departe cu instructiuniile de inceput (Etapa 3)
    public void Continue2() {
        InitialSetup.SetActive(false); PlayerPrefs.SetInt("INSETUP", 1); TutorialSetup.SetActive(true);
        if (PlayerPrefs.GetInt("SPEEDSOURCE") == 0) { SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>(); GPSStart.SetActive(true); SM.InitGPS(); }
    }

    // Functie executata la prima deschidere a aplicatiei pentru a trece mai departe cu instructiuniile de inceput (Etapa 4)
    public void Continue3(bool yes) {
        if (yes) { PlayerPrefs.SetInt("TUTORIALYES", 1); Initial(); }
        else PlayerPrefs.SetInt("TUTORIALYES", 2);
        TutorialSetup.SetActive(false);
    }

    // Functie chemata de 60 de ori / secunda | Functie folosita pentru a actualiza interfata meniului
    void FixedUpdate() {
        if (SM != null && GPSStart.activeInHierarchy && coold >= 16) { Status = SM.GPSInit;
        if (Status == 0) { GPSStart.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(true); GPSStart.transform.GetChild(0).GetChild(0).GetChild(4).gameObject.SetActive(true); GPSStart.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(false); GPSStart.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(false); GPSStart.transform.GetChild(0).GetChild(0).GetChild(3).gameObject.SetActive(false);
            GPSStart.transform.GetChild(0).GetChild(0).GetChild(5).gameObject.SetActive(false); GPSStart.transform.GetChild(0).GetChild(0).GetChild(6).gameObject.SetActive(false); } if (Status == 1) GPSStart.SetActive(false); coold = 0;
        if (Status == 2) { GPSStart.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false); GPSStart.transform.GetChild(0).GetChild(0).GetChild(4).gameObject.SetActive(false); GPSStart.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true); GPSStart.transform.GetChild(0).GetChild(0).GetChild(6).gameObject.SetActive(true); }
        if (Status == 4) { GPSStart.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false); GPSStart.transform.GetChild(0).GetChild(0).GetChild(4).gameObject.SetActive(false); GPSStart.transform.GetChild(0).GetChild(0).GetChild(2).gameObject.SetActive(true); GPSStart.transform.GetChild(0).GetChild(0).GetChild(5).gameObject.SetActive(true); }
        if (Status == 5) { GPSStart.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.SetActive(false); GPSStart.transform.GetChild(0).GetChild(0).GetChild(4).gameObject.SetActive(false); GPSStart.transform.GetChild(0).GetChild(0).GetChild(3).gameObject.SetActive(true); GPSStart.transform.GetChild(0).GetChild(0).GetChild(5).gameObject.SetActive(true); } } coold++;
    }

    // Functie folosita pentru a arata iconita de incarcare pentru o functie ce poate dura mai mult timp
    public IEnumerator LOADShow(int Action) {
        LOAD.SetActive(true);
        yield return null;
        if (Action == 0) OnChangeOfPresets();
        else if (Action == 1) DeleteData();
        else if (Action == 2) ActualCreation(false);
        else if (Action == 3) { SceneManager.LoadScene(1); }
        else if (Action == 4) { PlayerPrefs.SetInt("USEFLIP", 0);
            SceneManager.LoadScene(2); }
        else if (Action == 5) { PlayerPrefs.SetInt("USEFLIP", 1);
            SceneManager.LoadScene(2); }
    }

    // Functie propriu-zisa executata la incarcarea unei imagini din dispozitivul 
    public IEnumerator LDImg(string filePath) {
        if (filePath == "TLCD2") { cranberry = templates[0]; }
        else if (filePath == "TLCD1") { cranberry = templates[1]; }
        else if (filePath == "TLCD3") { cranberry = templates[2]; }
        else if (filePath == "TLCOM") { cranberry = templates[3]; }
        else if (filePath == "TLANL") { cranberry = templates[4]; }
        else if (filePath == "TLCM2") { cranberry = templates[5]; }
        else if (filePath == "TLOBD") { cranberry = templates[6]; }
        else if (filePath != ":/") {
        byte[] imageData = File.ReadAllBytes(filePath);
        Texture2D tex = new Texture2D(2, 2);
        if (tex.LoadImage(imageData))
            cranberry = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        yield return null;
        } else cranberry = null;
    }
}
