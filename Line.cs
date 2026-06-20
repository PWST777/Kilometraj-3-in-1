using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Linie
    // Folosita pentru obiectul Linie
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public SelectDrag Sdrg; // Clasa universala pentru aranjarea, crearea si stergerea elementelor
    public ColorPicker CoPi; // Clasa universala pentru selectarea culorii
    public bool SceneMode; // Variabila folosita pentru a determina daca elementul este editad sau utilizat
    public int ElementID; // Vaiabila numerica autonumerotata pentru identificarea elementului
    public bool LoadMode;
    public GameObject EditorModPanel;

    // Lista de elenete si proprietati pentru clasa
    public int Mode; public Dropdown Omdeld;
    public float Tickness; public Slider Tickld;
    public float Lenghtness; public Slider Lend;
    public float Mode1Burn; public Slider m1b;
    public Color NeedC; public Image Ncl;
    public GameObject Mode1Prop;
    public Button DelObj; public InputField soo;

    // Functie executata cand elementul este activat pentru prima data 
    void Start() {
        if (LoadMode) OnSelectOfElement();
        OnComponentLnModdify();
        OnColorModdify();
    }

    // Functie executata cand elementul este creat sau este selectat
    public void OnSelectOfElement() { if (!SceneMode) { LoadMode = true;
        // Aplicarea functiilor pe elementele de interfata (elemente Button, InputField etc.)
        Tickld.onValueChanged.RemoveAllListeners(); Tickld.onValueChanged.AddListener((_) => OnComponentLnModdify());
        Lend.onValueChanged.RemoveAllListeners(); Lend.onValueChanged.AddListener((_) => OnComponentLnModdify());
        Omdeld.onValueChanged.RemoveAllListeners(); Omdeld.onValueChanged.AddListener((_) => OnComponentLnModdify());
        m1b.onValueChanged.RemoveAllListeners(); m1b.onValueChanged.AddListener((_) => OnComponentLnModdify());
        DelObj.onClick.RemoveAllListeners(); DelObj.onClick.AddListener(() => DeleteObj(69));
        soo.onSubmit.RemoveAllListeners(); soo.onSubmit.AddListener((_) => ModSOrtO());
        Ncl.GetComponent<Button>().onClick.RemoveAllListeners(); Ncl.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("LNLINECL")); }
        // Incarcarea variabilelor salvate
        Tickness = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LNTHIC", 15f);
        Lenghtness = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LNLENG", 200f);
        Mode1Burn = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LNM1B", 0.2f);
        Mode = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LNMODE", 0);
        if (!SceneMode) { Tickld.value = Tickness; Omdeld.value = Mode; Lend.value = Lenghtness; m1b.value = Mode1Burn; soo.text = gameObject.transform.GetSiblingIndex() + ""; }
        StartCoroutine(yesofcourse());
    }

    // Functie executata cand o proprietate este schimbata despre element
    public void OnComponentLnModdify() {
        if(!SceneMode && !LoadMode) { Tickness = Tickld.value; Mode = Omdeld.value; Mode1Burn = m1b.value; Lenghtness = Lend.value; }
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Tickness, Lenghtness);
        gameObject.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(Tickness, Lenghtness);
        if (Mode == 1) { gameObject.transform.GetChild(0).gameObject.SetActive(true); if(!SceneMode) Mode1Prop.SetActive(true); }
        else { gameObject.transform.GetChild(0).gameObject.SetActive(false); if (!SceneMode) Mode1Prop.SetActive(false); }
        gameObject.transform.GetChild(0).GetComponent<RawImage>().color = new Color(NeedC.r, NeedC.g, NeedC.b, Mode1Burn);
        if (!SceneMode && !LoadMode) {
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LNTHIC", Tickness);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LNLENG", Lenghtness);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LNM1B", Mode1Burn);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "LNMODE", Mode);
        }
    }

    // Functie executata pentru a astepta un cadru si a actualiza interfata inainte de executarea logicii
    public IEnumerator yesofcourse() {
        yield return null;
        LoadMode = false;
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

    // Functie executata cand o culoare este selectata si schimbata in meniul de culoare
    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        NeedC = new Color(PlayerPrefs.GetFloat(colorstart + "LNLINECLR", 1f), PlayerPrefs.GetFloat(colorstart + "LNLINECLG", 1f), PlayerPrefs.GetFloat(colorstart + "LNLINECLB", 1f));
        gameObject.transform.GetChild(0).GetComponent<RawImage>().color = new Color(NeedC.r, NeedC.g, NeedC.b, Mode1Burn); gameObject.GetComponent<RawImage>().color = NeedC; if (!SceneMode) {Ncl.color = NeedC;}
    }

    // Functie executata cand o culoare trebuie schimbata
    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject);
    }
}
