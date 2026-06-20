using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ImageComponent : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa obiect Image
    // Folosita pe elemente Imagine
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public SelectDrag Sdrg; // Clasa universala pentru aranjarea, crearea si stergerea elementelor
    public ImagePicker imgp; // Clasa universala pentru incarcarea imaginilor din dispozitivul utilizatorului
    public SpriteBordMod imsp; // Clasa universala pentru a felia imaginea
    public ColorPicker CoPi; // Clasa universala pentru selectarea culorii
    public bool SceneMode; // Variabila folosita pentru a determina daca elementul este editad sau utilizat
    public int ElementID; // Vaiabila numerica autonumerotata pentru identificarea elementului
    public bool LoadMode;
    public GameObject EditorModPanel;

    public Image myImg; // Obiectul imagine propriu-zis

    // Lista de elenete si proprietati pentru clasa
    public string activetexpath;
    public Texture2D activetexure;
    public int Width; public InputField WDt;
    public int Height; public InputField HEt;
    public int ImageLoadMode; public Dropdown ImgLM;
    public GameObject ImgLM1; public GameObject ImgLM2;
    public Button ImgLM1b; public Button ImgLM2b;
    public Color Tint; public Image TintSel;
    public int Imagesplice; public Dropdown ImgRendMode;
    public GameObject IMGSP1; public GameObject IMGSP2;
    public GameObject IMGSP3; public RectTransform content;
    public Vector4 ImageSlices; public GameObject SSlices;
    public GameObject TSlices; public float Sizer;
    public InputField SSize; public InputField TSize;
    public Button ImgSP1b; public Button ImgSP2b;
    public Image ImgPrev;
    public int FillModes; public Dropdown FLMD;
    public int FillOrigin; public Dropdown FLORG;
    public float FillAmount; public Slider FLAMT;
    public int Filterm; public Dropdown ImgFiltrM;
    public Button DelObj; public InputField soo;
    public List<RectTransform> MenuPanels;
    public List<Texture2D> premade;

    // Functie executata cand elementul este activat pentru prima data
    void Start() {
        if(LoadMode) OnSelectOfElement();
        OnComponentImgModdify();
        OnColorModdify();
    }

    // Functie executata cand elementul este creat sau este selectat
    public void OnSelectOfElement() { if (!SceneMode) { LoadMode = true;
        // Aplicarea functiilor pe elementele de interfata (elemente Button, InputField etc.)
        ImgLM.onValueChanged.RemoveAllListeners(); ImgLM.onValueChanged.AddListener((_) => OnComponentImgModdify());
        FLMD.onValueChanged.RemoveAllListeners(); FLMD.onValueChanged.AddListener((_) => OnComponentImgModdify());
        FLORG.onValueChanged.RemoveAllListeners(); FLORG.onValueChanged.AddListener((_) => OnComponentImgModdify());
        FLAMT.onValueChanged.RemoveAllListeners(); FLAMT.onValueChanged.AddListener((_) => OnComponentImgModdify());
        ImgFiltrM.onValueChanged.RemoveAllListeners(); ImgFiltrM.onValueChanged.AddListener((_) => ChangeFilter());
        WDt.onSubmit.RemoveAllListeners(); WDt.onSubmit.AddListener((_) => OnComponentImgModdify());
        HEt.onSubmit.RemoveAllListeners(); HEt.onSubmit.AddListener((_) => OnComponentImgModdify());
        ImgLM1b.onClick.RemoveAllListeners(); ImgLM1b.onClick.AddListener(InitSelectImg);
        ImgLM2b.onClick.RemoveAllListeners(); ImgLM2b.onClick.AddListener(InitSelectImgURL);
        ImgSP1b.onClick.RemoveAllListeners(); ImgSP1b.onClick.AddListener(() => imsp.spliceMenu.SetActive(true)); ImgSP1b.onClick.AddListener(OnInitSplice);
        ImgSP2b.onClick.RemoveAllListeners(); ImgSP2b.onClick.AddListener(() => imsp.spliceMenu.SetActive(true)); ImgSP2b.onClick.AddListener(OnInitSplice);
        TintSel.GetComponent<Button>().onClick.RemoveAllListeners(); TintSel.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("ICIMGCL"));
        ImgLM1b.onClick.RemoveAllListeners(); ImgLM1b.onClick.AddListener(InitSelectImg);
        ImgRendMode.onValueChanged.RemoveAllListeners(); ImgRendMode.onValueChanged.AddListener((_) => OnComponentImgModdify());
        SSize.onSubmit.RemoveAllListeners(); SSize.onSubmit.AddListener((_) => OnComponentImgModdify());
        TSize.onSubmit.RemoveAllListeners(); TSize.onSubmit.AddListener((_) => OnComponentImgModdify());
        DelObj.onClick.RemoveAllListeners(); DelObj.onClick.AddListener(() => DeleteObj(69));
        soo.onSubmit.RemoveAllListeners(); soo.onSubmit.AddListener((_) => ModSOrtO()); }
        // Incarcarea variabilelor salvate
        ImageLoadMode = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICLDMD", 0);
        Imagesplice = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICSPLC", 0);
        Filterm = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICFILT", 1);
        Width = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICWIDT", 300);
        Height = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICHEIG", 300);
        FillModes = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICFMOD", 4);
        FillOrigin = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICFORG", 0);
        FillAmount = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICFAMT", 1f);
        Sizer = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICIMSZ", 1f);
        string st = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICSPLC";
        ImageSlices = new Vector4(PlayerPrefs.GetInt(st + "1", 0), PlayerPrefs.GetInt(st + "2", 0), PlayerPrefs.GetInt(st + "3", 0), PlayerPrefs.GetInt(st + "4", 0));
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        Tint = new Color(PlayerPrefs.GetFloat(colorstart + "ICIMGCLR", 1f), PlayerPrefs.GetFloat(colorstart + "ICIMGCLG", 1f), PlayerPrefs.GetFloat(colorstart + "ICIMGCLB", 1f));
        activetexpath = PlayerPrefs.GetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICPATH"); OnImgApply();
        if (!SceneMode) { ImgLM.value = ImageLoadMode; ImgRendMode.value = Imagesplice; WDt.text = Width + ""; HEt.text = Height + ""; ImgFiltrM.value = Filterm; soo.text = transform.GetSiblingIndex() + "";
            if (Imagesplice == 1) SSize.text = Sizer.ToString("F2", CultureInfo.InvariantCulture); FLAMT.value = FillAmount; FLMD.value = FillModes; FLORG.value = FillOrigin;
            if (Imagesplice == 2) TSize.text = Sizer.ToString("F2", CultureInfo.InvariantCulture); if(activetexure != null) {
                SSlices.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(4f + (((float)ImageSlices.x / activetexure.width) * 102f), 0f);
                SSlices.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-4f - (((float)ImageSlices.z / activetexure.width) * 102f), 0f);
                SSlices.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -4f - (((float)ImageSlices.w / activetexure.height) * 102f));
                SSlices.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 4f + (((float)ImageSlices.y / activetexure.height) * 102f));
                TSlices.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(4f + (((float)ImageSlices.x / activetexure.width) * 102f), 0f);
                TSlices.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-4f - (((float)ImageSlices.z / activetexure.width) * 102f), 0f);
                TSlices.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -4f - (((float)ImageSlices.w / activetexure.height) * 102f));
                TSlices.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 4f + (((float)ImageSlices.y / activetexure.height) * 102f));
            } } StartCoroutine(yesofcourse());
    }

    // Functie executata pentru a astepta un cadru si a actualiza interfata inainte de executarea logicii
    public IEnumerator yesofcourse() {
        yield return null;
        LoadMode = false;
    }

    // Functie executata cand filtrul imaginii este schimbat
    public void ChangeFilter() {
        if (!SceneMode && !LoadMode) { ImageLoadMode = ImgLM.value; Filterm = ImgFiltrM.value; }
        StartCoroutine(LDImg("owo", false));
        if(!SceneMode && !LoadMode) {
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICFILT", Filterm);
        }
    }

    // Functie executata cand imagina vrea sa fie feliata
    public void OnInitSplice() { string st = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICSPLC";
        imsp.RequestBChange(st, gameObject, activetexure);
    }

    // Functie executata cand o proprietate este schimbata despre element
    public void OnComponentImgModdify() {
        if(!SceneMode && !LoadMode) { ImageLoadMode = ImgLM.value; Imagesplice = ImgRendMode.value; Width = int.Parse(WDt.text); Height = int.Parse(HEt.text);
            Filterm = ImgFiltrM.value; FillAmount = FLAMT.value; FillModes = FLMD.value; FillOrigin = FLORG.value;
            if (Imagesplice == 1) Sizer = float.Parse(SSize.text, CultureInfo.InvariantCulture); if (Imagesplice == 2) Sizer = float.Parse(TSize.text, CultureInfo.InvariantCulture); }
        if (!SceneMode) { content.sizeDelta = new Vector2(content.sizeDelta.x, 1650f);
            MenuPanels[0].anchoredPosition = new Vector2(0f, -360f); MenuPanels[0].sizeDelta = new Vector2(0f, 510f);
            MenuPanels[1].anchoredPosition = new Vector2(0f, -870f); MenuPanels[1].sizeDelta = new Vector2(0f, 210f);
            MenuPanels[2].anchoredPosition = new Vector2(0f, -1080f); MenuPanels[2].sizeDelta = new Vector2(0f, 510f);
            MenuPanels[3].anchoredPosition = new Vector2(0f, -1080f); MenuPanels[3].sizeDelta = new Vector2(0f, 510f);
            MenuPanels[4].anchoredPosition = new Vector2(0f, -1080f); MenuPanels[4].sizeDelta = new Vector2(0f, 510f);
            MenuPanels[5].anchoredPosition = new Vector2(0f, -1080f); MenuPanels[5].sizeDelta = new Vector2(0f, 210f);
            if (ImageLoadMode == 0) { ImgLM1.SetActive(false); ImgLM2.SetActive(false); myImg.sprite = null; }
            if (ImageLoadMode == 1) { ImgLM1.SetActive(true); ImgLM2.SetActive(false); for (int p = 1; p < MenuPanels.Count; p++) MenuPanels[p].anchoredPosition -= new Vector2(0f, 150f); MenuPanels[0].sizeDelta += new Vector2(0f, 150f); content.sizeDelta += new Vector2(0f, 150f); myImg.sprite = ImgPrev.sprite; }
            if (ImageLoadMode == 2) { ImgLM1.SetActive(false); ImgLM2.SetActive(true); for (int p = 1; p < MenuPanels.Count; p++) MenuPanels[p].anchoredPosition -= new Vector2(0f, 150f); MenuPanels[0].sizeDelta += new Vector2(0f, 150f); content.sizeDelta += new Vector2(0f, 150f); myImg.sprite = ImgPrev.sprite; }
            if (Imagesplice == 0) { IMGSP1.SetActive(false); IMGSP2.SetActive(false); IMGSP3.SetActive(false); myImg.type = Image.Type.Simple; }
            if (Imagesplice == 1) { IMGSP1.SetActive(true); IMGSP2.SetActive(false); IMGSP3.SetActive(false); for (int p = 5; p < MenuPanels.Count; p++) MenuPanels[p].anchoredPosition -= new Vector2(0f, 510f); content.sizeDelta += new Vector2(0f, 510f); myImg.type = Image.Type.Sliced; }
            if (Imagesplice == 2) { IMGSP1.SetActive(false); IMGSP2.SetActive(true); IMGSP3.SetActive(false); for (int p = 5; p < MenuPanels.Count; p++) MenuPanels[p].anchoredPosition -= new Vector2(0f, 510f); content.sizeDelta += new Vector2(0f, 510f); myImg.type = Image.Type.Tiled; }
            if (Imagesplice == 3) { IMGSP1.SetActive(false); IMGSP2.SetActive(false); IMGSP3.SetActive(true); for (int p = 5; p < MenuPanels.Count; p++) MenuPanels[p].anchoredPosition -= new Vector2(0f, 510f); content.sizeDelta += new Vector2(0f, 510f); myImg.type = Image.Type.Filled; }
            content.sizeDelta -= new Vector2(0f, 360f);
        } if (Imagesplice == 0) { myImg.type = Image.Type.Simple; }
            if (Imagesplice == 1) { myImg.type = Image.Type.Sliced; }
            if (Imagesplice == 2) { myImg.type = Image.Type.Tiled; }
            if (Imagesplice == 3) { myImg.type = Image.Type.Filled; }
            if (Imagesplice == 3) { myImg.fillAmount = FillAmount; if (FillModes <= 4) myImg.fillClockwise = true; if (FillModes >= 5) myImg.fillClockwise = false;
                if (FillModes == 0) myImg.fillMethod = Image.FillMethod.Horizontal; if (FillModes == 1) myImg.fillMethod = Image.FillMethod.Vertical;
                if (FillModes == 2 || FillModes == 5) myImg.fillMethod = Image.FillMethod.Radial90; if (FillModes == 3 || FillModes == 6) myImg.fillMethod = Image.FillMethod.Radial180;
                if (FillModes == 4 || FillModes == 7) myImg.fillMethod = Image.FillMethod.Radial360; if (FillModes == 0) myImg.fillOrigin = (FillOrigin / 2);
                if (FillModes == 1) myImg.fillOrigin = (FillOrigin / 2); if (FillModes == 2 || FillModes == 5) myImg.fillOrigin = FillOrigin;
                if (FillModes == 3 || FillModes == 6) myImg.fillOrigin = FillOrigin; if (FillModes == 4 || FillModes == 7) {
                    int fos = FillOrigin; if (fos == 1) fos = 3; else if (fos == 3) fos = 1; myImg.fillOrigin = fos; } }
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Width, Height);
        myImg.pixelsPerUnitMultiplier = Sizer;
        if (!SceneMode && !LoadMode) {
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICLDMD", ImageLoadMode);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICSPLC", Imagesplice);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICWIDT", Width);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICHEIG", Height);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICFMOD", FillModes);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICFORG", FillOrigin);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICFAMT", FillAmount);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICIMSZ", Sizer);
        }
    }

    // Functie executata cand imaginea este feliata
    public void OnSpliceRecieve() { string st = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICSPLC";
        ImageSlices = new Vector4(PlayerPrefs.GetInt(st + "1", 0), PlayerPrefs.GetInt(st + "2", 0), PlayerPrefs.GetInt(st + "3", 0), PlayerPrefs.GetInt(st + "4", 0));
        StartCoroutine(LDImg("owo",false));
    }

    // Functie executata cand o culoare trebuie schimbata
    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject);
    }

    // Functie executata cand o culoare este selectata si schimbata in meniul de culoare 
    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        Tint = new Color(PlayerPrefs.GetFloat(colorstart + "ICIMGCLR", 1f), PlayerPrefs.GetFloat(colorstart + "ICIMGCLG", 1f), PlayerPrefs.GetFloat(colorstart + "ICIMGCLB", 1f));
        myImg.color = Tint; if (!SceneMode) TintSel.color = Tint;
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

    // Functie executata cand o imagine vrea sa fie incarcata din dispozitiv
    public void InitSelectImg() {
        imgp.InitSelection(gameObject, ("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICIMAGE"), 1);
    }

    // [FUNCTIE NEFOLOSITA] Functie executata cand o imagine vrea sa fie incarcata prin URL
    public void InitSelectImgURL() {
        imgp.InitSelection(gameObject, ("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICIMAGE"), 2);
    }

    // Functie ce incarca o imagine bazat pe modul selectat
    public void OnImgApply() { string imgpath = PlayerPrefs.GetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICIMAGE");
        if(ImageLoadMode == 1) StartCoroutine(LDImg(imgpath, true));
        if(ImageLoadMode == 2) StartCoroutine(DownloadImage(imgpath));
    }

    // Functie propriu-zisa executata cand o imagine vrea sa fie incarcata din dispozitiv
    public IEnumerator LDImg(string filePath, bool newload) {
        if (filePath != "") { if (newload) { activetexpath = filePath;
                Texture2D tex = new Texture2D(2, 2);
                if (activetexpath.StartsWith("PREMADES")) { tex = premade[int.Parse(activetexpath[8].ToString())]; }
                else { byte[] imageData = File.ReadAllBytes(filePath); tex.LoadImage(imageData); }
                if (Filterm == 0) tex.filterMode = FilterMode.Point;
                    if (Filterm == 1) tex.filterMode = FilterMode.Bilinear;
                    if (Filterm == 2) tex.filterMode = FilterMode.Trilinear; activetexure = tex;
                    Sprite cranberry = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, ImageSlices);
                    myImg.sprite = cranberry; if (!SceneMode) ImgPrev.sprite = cranberry;
                    PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "ICPATH", activetexpath);  }
            else { if(activetexure != null) { if (Filterm == 0) activetexure.filterMode = FilterMode.Point;
                if (Filterm == 1) activetexure.filterMode = FilterMode.Bilinear;
                if (Filterm == 2) activetexure.filterMode = FilterMode.Trilinear;
                Sprite cranberry = Sprite.Create(activetexure, new Rect(0, 0, activetexure.width, activetexure.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, ImageSlices);
                myImg.sprite = cranberry; if (!SceneMode) ImgPrev.sprite = cranberry;
            } if (!SceneMode && activetexure != null) { SSlices.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(4f + (((float)ImageSlices.x / activetexure.width) * 102f), 0f);
            SSlices.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-4f - (((float)ImageSlices.z / activetexure.width) * 102f), 0f);
            SSlices.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -4f - (((float)ImageSlices.w / activetexure.height) * 102f));
            SSlices.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 4f + (((float)ImageSlices.y / activetexure.height) * 102f));
            TSlices.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(4f + (((float)ImageSlices.x / activetexure.width) * 102f), 0f);
            TSlices.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-4f - (((float)ImageSlices.z / activetexure.width) * 102f), 0f);
            TSlices.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -4f - (((float)ImageSlices.w / activetexure.height) * 102f));
            TSlices.transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 4f + (((float)ImageSlices.y / activetexure.height) * 102f)); } }
        }
        yield return null;
    }

    // [FUNCTIE NEFOLOSITA] Functie propriu-zisa executata cand o imagine vrea sa fie incarcata prin URL
    IEnumerator DownloadImage(string url) {
        yield return null;
    }

    void Update()
    {
        
    }
}
