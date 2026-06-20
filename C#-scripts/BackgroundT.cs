using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BackgroundT : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Fundal
    // Folosit pentru fundalul presetarii
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public ImagePicker imgp; // Clasa folosita pentru selectarea unei imagini din dispozitiv
    public SpriteBordMod imsp; // Clasa folosita pentru feliarea unei imagini
    public Camera cam; // Obiect camera din scena unity
    public RectTransform Canv; // Obiect interfata
    public ColorPicker CoPi; // Clasa folosita pentru selectarea unei culori
    public string Mode; // Variabila text folosita pentru a determina modul selectat
    public bool SceneMode; // Variabila folosita pentru a determina daca elementul este editad sau utilizat

    // Lista de elenete si proprietati pentru clasa
    public Transform allTypes;
    public GameObject G2s;
    public GameObject G2m;
    public List<RectTransform> G2o;
    public GameObject G4s;
    public GameObject G4m;
    public List<RectTransform> G4o;
    public List<GameObject> Menuos;
    public InputField anggle;
    public int valB; public Dropdown BMode;
    public int valG; public Dropdown GMode;
    public List<Color> GCols;
    public List<Image> UIGCols;
    public Image myImg;
    public string activetexpath;
    public Texture2D activetexure;
    public int ImageLoadMode; public Dropdown ImgLM;
    public GameObject ImgLM1; public GameObject ImgLM2;
    public Color Tint; public Image TintSel;
    public int Imagesplice; public Dropdown ImgRendMode;
    public GameObject IMGSP1; public GameObject IMGSP2;
    public GameObject IMGSP3; public RectTransform content;
    public Vector4 ImageSlices; public GameObject SSlices;
    public GameObject TSlices; public float Sizer;
    public InputField SSize; public InputField TSize;
    public Image ImgPrev;
    public int FillModes; public Dropdown FLMD;
    public int FillOrigin; public Dropdown FLORG;
    public float FillAmount; public Slider FLAMT;
    public int Filterm; public Dropdown ImgFiltrM;
    public List<RectTransform> MenuPanels;
    public GameObject IMGm;
    public string wowkey;
    public bool allowed;
    public bool alm;
    public List<Texture2D> premade;

    // Functie executata cand elementul este activat pentru prima data
    void Start() { allowed = false; alm = false; // Incarcarea variabilelor salvate
        Mode = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BACKMODE", 0).ToString();
        if (!SceneMode) anggle.text = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BACKMANG", 90) + "";
        OnModeChange(); if (!SceneMode) { BMode.value = int.Parse(Mode[0].ToString()); if(Mode.Length >= 2) GMode.value = (int.Parse(Mode[1].ToString()) - 1); }
        for (int c = 0; c < 4; c++) { wowkey = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BCG" + (c+1); OnColorRecieve(); }
        ImageLoadMode = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICLDMD", 0);
        Imagesplice = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICSPLC", 0);
        Filterm = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFILT", 1);
        FillModes = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFMOD", 4);
        FillOrigin = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFORG", 0);
        FillAmount = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFAMT", 1f);
        Sizer = PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICIMSZ", 1f);
        string st = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICSPLC";
        ImageSlices = new Vector4(PlayerPrefs.GetInt(st + "1", 0), PlayerPrefs.GetInt(st + "2", 0), PlayerPrefs.GetInt(st + "3", 0), PlayerPrefs.GetInt(st + "4", 0));
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMG";
        Tint = new Color(PlayerPrefs.GetFloat(colorstart + "ICIMGCLR", 1f), PlayerPrefs.GetFloat(colorstart + "ICIMGCLG", 1f), PlayerPrefs.GetFloat(colorstart + "ICIMGCLB", 1f));
        activetexpath = PlayerPrefs.GetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICPATH"); OnImgApply();
        if (!SceneMode) { ImgLM.value = ImageLoadMode; ImgRendMode.value = Imagesplice; ImgFiltrM.value = Filterm;
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
            } } alm = true;
        if(SceneMode) { float scale = cam.pixelHeight / 1080f;
        Canv.sizeDelta = new Vector2(cam.pixelWidth / scale, cam.pixelHeight / scale); }
        if (SceneMode) OnComponentImgModdify();
    }

    // Functie folosita pentru setarea valorii 'allowed' = true
    public void ALllwo() {
        allowed = true;
    }

    // Functie executata cand filtrul de imagine este schimbat
    public void ChangeFilter() {
        if (!SceneMode) { ImageLoadMode = ImgLM.value; Filterm = ImgFiltrM.value; }
        StartCoroutine(LDImg("owo", false));
        if(!SceneMode) {
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFILT", Filterm);
        }
    }

    // Functie executata cand feliarea imaginii este initializata
    public void OnInitSplice() { string st = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICSPLC";
        imsp.RequestBChange(st, gameObject, activetexure);
    }

    // Functie executata cand o proprietate este schimbata despre element
    public void OnComponentImgModdify() {
        if(!SceneMode && alm) { ImageLoadMode = ImgLM.value; Imagesplice = ImgRendMode.value;
            Filterm = ImgFiltrM.value; FillAmount = FLAMT.value; FillModes = FLMD.value; FillOrigin = FLORG.value;
            if (Imagesplice == 1) Sizer = float.Parse(SSize.text, CultureInfo.InvariantCulture); if (Imagesplice == 2) Sizer = float.Parse(TSize.text, CultureInfo.InvariantCulture); }
        if (!SceneMode) { content.sizeDelta = new Vector2(content.sizeDelta.x, 1140f);
            MenuPanels[0].anchoredPosition = new Vector2(0f, -410f); MenuPanels[0].sizeDelta = new Vector2(403f, 510f);
            MenuPanels[1].anchoredPosition = new Vector2(0f, -920f); MenuPanels[1].sizeDelta = new Vector2(403f, 210f);
            MenuPanels[2].anchoredPosition = new Vector2(0f, -1130f); MenuPanels[2].sizeDelta = new Vector2(403f, 510f);
            MenuPanels[3].anchoredPosition = new Vector2(0f, -1130f); MenuPanels[3].sizeDelta = new Vector2(403f, 510f);
            MenuPanels[4].anchoredPosition = new Vector2(0f, -1130f); MenuPanels[4].sizeDelta = new Vector2(403f, 510f);
            MenuPanels[5].anchoredPosition = new Vector2(0f, -1130f); MenuPanels[5].sizeDelta = new Vector2(403f, 210f);
            if (ImageLoadMode == 0) { ImgLM1.SetActive(false); ImgLM2.SetActive(false); myImg.sprite = null; }
            if (ImageLoadMode == 1) { ImgLM1.SetActive(true); ImgLM2.SetActive(false); for (int p = 1; p < MenuPanels.Count; p++) MenuPanels[p].anchoredPosition -= new Vector2(0f, 150f); MenuPanels[0].sizeDelta += new Vector2(0f, 150f); content.sizeDelta += new Vector2(0f, 150f); myImg.sprite = ImgPrev.sprite; }
            if (ImageLoadMode == 2) { ImgLM1.SetActive(false); ImgLM2.SetActive(true); for (int p = 1; p < MenuPanels.Count; p++) MenuPanels[p].anchoredPosition -= new Vector2(0f, 150f); MenuPanels[0].sizeDelta += new Vector2(0f, 150f); content.sizeDelta += new Vector2(0f, 150f); myImg.sprite = ImgPrev.sprite; }
            if (Imagesplice == 0) { IMGSP1.SetActive(false); IMGSP2.SetActive(false); IMGSP3.SetActive(false); myImg.type = Image.Type.Simple; }
            if (Imagesplice == 1) { IMGSP1.SetActive(true); IMGSP2.SetActive(false); IMGSP3.SetActive(false); for (int p = 5; p < MenuPanels.Count; p++) MenuPanels[p].anchoredPosition -= new Vector2(0f, 510f); content.sizeDelta += new Vector2(0f, 510f); myImg.type = Image.Type.Sliced; }
            if (Imagesplice == 2) { IMGSP1.SetActive(false); IMGSP2.SetActive(true); IMGSP3.SetActive(false); for (int p = 5; p < MenuPanels.Count; p++) MenuPanels[p].anchoredPosition -= new Vector2(0f, 510f); content.sizeDelta += new Vector2(0f, 510f); myImg.type = Image.Type.Tiled; }
            if (Imagesplice == 3) { IMGSP1.SetActive(false); IMGSP2.SetActive(false); IMGSP3.SetActive(true); for (int p = 5; p < MenuPanels.Count; p++) MenuPanels[p].anchoredPosition -= new Vector2(0f, 510f); content.sizeDelta += new Vector2(0f, 510f); myImg.type = Image.Type.Filled; }
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
        myImg.pixelsPerUnitMultiplier = Sizer;
        if (!SceneMode) {
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICLDMD", ImageLoadMode);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICSPLC", Imagesplice);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFMOD", FillModes);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFORG", FillOrigin);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICFAMT", FillAmount);
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICIMSZ", Sizer);
        }
    }

    // Functie executata cand imaginea este feliata
    public void OnSpliceRecieve() { string st = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICSPLC";
        ImageSlices = new Vector4(PlayerPrefs.GetInt(st + "1", 0), PlayerPrefs.GetInt(st + "2", 0), PlayerPrefs.GetInt(st + "3", 0), PlayerPrefs.GetInt(st + "4", 0));
        StartCoroutine(LDImg("owo",false));
    }

    // Functie executata cand o imagine este incarcata pentru fundal
    public void InitSelectImg() {
        imgp.InitSelection(gameObject, ("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICIMAGE"), 1);
    }

    // [FUNCTIE NEFOLOSITA] Functie executata cand o imagine este incarcata pentru fundal prin URL
    public void InitSelectImgURL() {
        imgp.InitSelection(gameObject, ("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICIMAGE"), 2);
    }

    public void OnImgApply() { string imgpath = PlayerPrefs.GetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICIMAGE");
        if(ImageLoadMode == 1) StartCoroutine(LDImg(imgpath, true));
        if(ImageLoadMode == 2) StartCoroutine(DownloadImage(imgpath));
    }

    // Functie asincrona folosita pentru incarcarea imaginii din dispozitiv
    public IEnumerator LDImg(string filePath, bool newload) {
        if (filePath != "") { if (newload) { Texture2D tex = new Texture2D(2, 2);
            activetexpath = filePath; if (filePath == "IMGWOW0") tex = premade[0];
                else { byte[] imageData = File.ReadAllBytes(filePath);
                tex.LoadImage(imageData); } if (Filterm == 0) tex.filterMode = FilterMode.Point;
                    if (Filterm == 1) tex.filterMode = FilterMode.Bilinear;
                    if (Filterm == 2) tex.filterMode = FilterMode.Trilinear; activetexure = tex;
                    Sprite cranberry = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, ImageSlices);
                    myImg.sprite = cranberry; if (!SceneMode) ImgPrev.sprite = cranberry;
                    PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICPATH", activetexpath); }
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

    // [FUNCTIE NEFOLOSITA] Functie asincrona folosita pentru incarcarea imaginii online prin URL
    IEnumerator DownloadImage(string url) {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url)) {
            yield return uwr.SendWebRequest();
            if (uwr.result == UnityWebRequest.Result.Success){
                Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
                if (Filterm == 0) tex.filterMode = FilterMode.Point;
                if (Filterm == 1) tex.filterMode = FilterMode.Bilinear;
                if (Filterm == 2) tex.filterMode = FilterMode.Trilinear; activetexure = tex;
                Sprite cranberry = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, ImageSlices);
                myImg.sprite = cranberry; if (!SceneMode) ImgPrev.sprite = cranberry;
                PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMGICPATH", activetexpath);
            }
        }
    }

    // Functie executata cand variabila 'valB' este schimbata
    public void CheckBMode() {
        if (!SceneMode) { valB = BMode.value;
            foreach (GameObject g in Menuos) g.SetActive(false);
            Menuos[valB].SetActive(true);
            if (allowed) {
            if (valB == 0) { Mode = "0"; OnModeChange(); }
            if (valB == 1) Mode = "11";
            if (valB == 2) { Mode = "2"; myImg.gameObject.SetActive(true); } }
        } if(Mode == "11" || Mode == "12") CheckGMode();
    }

    // Functie executata cand variabila 'valG' este schimbata
    public void CheckGMode() { if(allowed) { if(!SceneMode) { valG = GMode.value;
        if (valG == 0) Mode = "11"; if (valG == 1) Mode = "12";
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BACKMODE", int.Parse(Mode)); }
        OnModeChange(); }
    }

    // Functie executata cand variabila 'Mode' este schimbata
    public void OnModeChange() {
        foreach (Transform g in allTypes) g.gameObject.SetActive(false);
        if (Mode == "11") { G2s.SetActive(true); if (!SceneMode) { G2m.SetActive(true); G4m.SetActive(false); IMGm.SetActive(false); } }
        if (Mode == "12") { G4s.SetActive(true); if (!SceneMode) { G4m.SetActive(true); G2m.SetActive(false); IMGm.SetActive(false); } }
        if (Mode == "2") { myImg.gameObject.SetActive(true); if (!SceneMode) { G4m.SetActive(false); G2m.SetActive(false); IMGm.SetActive(true); } }
        OnParamsChange();
    }

    // Functie executata cand o proprietate este schimbata despre element 
    public void OnParamsChange() { int angle;
        if (!SceneMode) angle = int.Parse(anggle.text);
        else angle = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BACKMANG", 90);
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BACKMODE", int.Parse(Mode)); 
        if(Mode == "11" || Mode == "12") {
            if (Mode == "11") {
            G2o[1].localEulerAngles = new Vector3(0, 0, (angle - 90));
            G2o[1].localScale = changeScale(angle - 90);
            } if (Mode == "12") {
            G4o[0].localEulerAngles = new Vector3(0, 0, angle);
            G4o[0].localScale = changeScale(angle);
            G4o[1].localEulerAngles = new Vector3(0, 0, (angle - 90));
            G4o[1].localScale = changeScale((angle - 90));
            G4o[2].localEulerAngles = new Vector3(0, 0, (angle + 90));
            G4o[2].localScale = changeScale((angle + 90));
            } if (Mode == "2") OnComponentImgModdify();
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BACKMANG", angle);
        }
    }

    // Functie executata cand o culoare trebuie schimbata
    public void OnCRequestSend(string Keysa) {
        string key = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + Keysa;
        CoPi.RequestChangeColor(key, gameObject); wowkey = key;
    }

    // Functie executata cand o culoare este selectata si schimbata in meniul de culoare
    public void OnColorRecieve() { Color wowc;
        if(wowkey.EndsWith("1")) wowc = new Color(PlayerPrefs.GetFloat(wowkey + "R",1f), PlayerPrefs.GetFloat(wowkey + "G",0f), PlayerPrefs.GetFloat(wowkey + "B",0f));
        else if(wowkey.EndsWith("2")) wowc = new Color(PlayerPrefs.GetFloat(wowkey + "R",1f), PlayerPrefs.GetFloat(wowkey + "G",1f), PlayerPrefs.GetFloat(wowkey + "B",0f));
        else if(wowkey.EndsWith("3")) wowc = new Color(PlayerPrefs.GetFloat(wowkey + "R",0f), PlayerPrefs.GetFloat(wowkey + "G",1f), PlayerPrefs.GetFloat(wowkey + "B",0f));
        else wowc = new Color(PlayerPrefs.GetFloat(wowkey + "R",0f), PlayerPrefs.GetFloat(wowkey + "G",1f), PlayerPrefs.GetFloat(wowkey + "B",1f));
        if(wowkey.EndsWith("BCG1")) { GCols[0] = wowc; G2o[1].GetComponent<Image>().color = wowc; G4o[0].GetComponent<Image>().color = wowc;
            if (!SceneMode) { UIGCols[0].color = wowc; UIGCols[4].color = wowc; UIGCols[6].color = wowc; UIGCols[8].color = wowc; } }
        if(wowkey.EndsWith("BCG2")) { GCols[1] = wowc; G2o[0].GetComponent<Image>().color = wowc; G4o[1].GetComponent<Image>().color = wowc;
            if (!SceneMode) { UIGCols[1].color = wowc; UIGCols[5].color = wowc; UIGCols[7].color = wowc; UIGCols[9].color = wowc; } }
        if(wowkey.EndsWith("BCG3")) { GCols[2] = wowc; G4o[2].GetComponent<Image>().color = wowc;
            if (!SceneMode) { UIGCols[2].color = wowc; UIGCols[10].color = wowc; } }
        if(wowkey.EndsWith("BCG4")) { GCols[3] = wowc; G4o[3].GetComponent<Image>().color = wowc;
            if (!SceneMode) { UIGCols[3].color = wowc; UIGCols[11].color = wowc; } }
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BKIMG";
        Tint = new Color(PlayerPrefs.GetFloat(colorstart + "ICIMGCLR", 1f), PlayerPrefs.GetFloat(colorstart + "ICIMGCLG", 1f), PlayerPrefs.GetFloat(colorstart + "ICIMGCLB", 1f));
        myImg.color = Tint; if (!SceneMode) TintSel.color = Tint;
    }

    // Functie ce genereaza o marime in 2 dimensiuni pentru o tranzitie de culoare
    public Vector2 changeScale(float angle) {
        if(SceneMode) { float scale = cam.pixelHeight / 1080f;
        Canv.sizeDelta = new Vector2(cam.pixelWidth / scale, cam.pixelHeight / scale); }
        float W = Canv.rect.width;
        float H = Canv.rect.height;
        float theta = angle * Mathf.Deg2Rad;
        float rotatedWidth = Mathf.Abs(W * Mathf.Cos(theta)) + Mathf.Abs(H * Mathf.Sin(theta));
        float rotatedHeight = Mathf.Abs(W * Mathf.Sin(theta)) + Mathf.Abs(H * Mathf.Cos(theta));
        float scaleX = rotatedWidth / W;
        float scaleY = rotatedHeight / H;
        return new Vector2(scaleX, scaleY);
    }
}

