using System.Collections;
using System;
using System.Linq;
using System.IO;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class MusicPlayer : MonoBehaviour
{

    public AudioSource asasd;
    public MainAudioBuffer mab;
    public MediaBridge bridge;
    public SelectDrag Sdrg;
    public ColorPicker CoPi;
    public LayoutModd lm;
    public ImagePicker imPk;
    public bool SceneMode;
    public int ElementID;
    public bool LoadMode;
    public GameObject EditorModPanel;

    public Image backwow;

    public int Width; public InputField WInp;
    public int Height; public InputField HInp;
    public int SelM; public Dropdown wowmyu;
    public List<GameObject> UICompsM; public Text exp; 
    public Color BackCol; public Image BCLs;
    public Button BrowseFolder; public Text BFold;
    public Button Layout;
    public Button DelObj; public InputField soo;
    public List<string> Comps;
    public List<GameObject> ClonerObjs;
    public GameObject myobj;
    public GameObject Menu2;
    public List<GameObject> Menu2Playback;
    public RectTransform content2;
    public GameObject cln1; public GameObject cln2;
    public GameObject LoaderIco;
    string[] audioExtensions = { ".mp3", ".wav", ".ogg" };
    public string[] songs;
    public string currentsong;
    public int IndexinFOld;
    public bool Looping;
    public bool MOD;
    public string activetexpath;
    public float[] Datas = new float[4];
    public long savedDuration;
    public float currentValue;
    public float refcool;
    public bool savedplay;

    public int SamplesMax;
    public int FFtMax;

    // Start is called before the first frame update
    void Start() {
        if (LoadMode) OnSelectOfElement();
        OnComponentEmPeTModdify();
        OnColorModdify(); LoadComps();
        if (SceneMode) {
            if (SelM != 0) {
                Menu2.SetActive(true);
                LoadDIrecyt(PlayerPrefs.GetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPSTARTSTRING"));
            } else { Menu2.SetActive(false); bridge.OnPlaybackStateChanged(""); bridge.OnSongChanged(""); }
        }
    }

    public void OnSelectOfElement() { if (!SceneMode) { LoadMode = true;
        WInp.onSubmit.RemoveAllListeners(); WInp.onSubmit.AddListener((_) => OnComponentEmPeTModdify());
        HInp.onSubmit.RemoveAllListeners(); HInp.onSubmit.AddListener((_) => OnComponentEmPeTModdify());
        wowmyu.onValueChanged.RemoveAllListeners(); wowmyu.onValueChanged.AddListener((_) => OnComponentEmPeTModdify());
        DelObj.onClick.RemoveAllListeners(); DelObj.onClick.AddListener(() => DeleteObj(69));
        soo.onSubmit.RemoveAllListeners(); soo.onSubmit.AddListener((_) => ModSOrtO());
        Layout.onClick.RemoveAllListeners(); Layout.onClick.AddListener(RequestLayout);
        BrowseFolder.onClick.RemoveAllListeners(); BrowseFolder.onClick.AddListener(OnBrowse);
        BCLs.GetComponent<Button>().onClick.RemoveAllListeners(); BCLs.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("MPBACKCL")); }
        Width = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPWIDT", 900);
        Height = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPHEIG", 550);
        SelM = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPSELM", 0);
        if (PlayerPrefs.GetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP0", ":(") == ":(") {
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP0", "0/" + Width + "/" + Height + "/5/0,0,0/255,255,255/255,255,255/255,255,255/p");
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP1", "1/0/-60/1.55/1.55/255,255,255,255/0,0,0,255/1/0/0");
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP2", "1/185/-69/1.25/1.25/255,255,255,255/0,0,0,255/1/1/0");
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP3", "1/350/-69/1.25/1.25/255,255,255,255/0,0,0,255/1/2/0");
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP4", "1/-185/-69/1.25/1.25/255,255,255,255/0,0,0,255/1/1/1");
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP5", "1/-350/-69/1.25/1.25/255,255,255,255/0,0,0,255/1/2/1");
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP6", "2/0/-217/840/100/0,0,0,255/255,255,255,255/55/255,255,255,255");
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP7", "3/0/138/800/160/255,255,255,255/32/50/0.5/1/20/1/1");
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP8", "1/-395/220/0.8/0.8/0,0,0,255/255,255,255,255/1.25/3/0");
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP9", "1/395/220/0.8/0.8/0,0,0,255/255,255,255,255/1/4/0");
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP10", "5/0/235/680/50/255,255,255,255/0,0,0,255/0/0/1/1/3.0/33");
        } LoadSaved();
        if (!SceneMode) { WInp.text = Width + ""; HInp.text = Height + ""; wowmyu.value = SelM; soo.text = gameObject.transform.GetSiblingIndex() + "";
        if (SelM == 0) BFold.text = "Unavaliable";
        else if(SelM == 1) BFold.text = Path.GetFileName(PlayerPrefs.GetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPSTARTSTRING"));
        else if(SelM == 2) BFold.text = "Unavaliable"; }
        StartCoroutine(yesofcourse());
    }

    public IEnumerator yesofcourse() {
        yield return null;
        LoadMode = false;
    }

    public void OnComponentEmPeTModdify() {
        if(!SceneMode && !LoadMode) { Width = int.Parse(WInp.text); Height = int.Parse(HInp.text); SelM = wowmyu.value; }
        backwow.GetComponent<RectTransform>().sizeDelta = new Vector2(Width, Height);
        if (!SceneMode) {
            for (int y = 0; y < UICompsM.Count; y++) if (y == (SelM)) UICompsM[y].SetActive(true); else UICompsM[y].SetActive(false);
            if (SelM == 0) { exp.text = "Controls App Currently Playing Music (e.g. Youtube, Spotify)";
                BFold.text = "Unavaliable"; }
            if (SelM == 1) { exp.text = "Gets Songs From A Selected Directory And It's Subfolders";
                BFold.text = Path.GetFileName(PlayerPrefs.GetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPSTARTSTRING")); }
            if (SelM == 2) { exp.text = "Gets Songs From Loaded Google Drive Links In A List";
                BFold.text = "Unavaliable"; } }
        if (!SceneMode && !LoadMode) { string ok = PlayerPrefs.GetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP0");
            string[] attr = ok.Split('/'); ok = attr[3] + '/' + attr[4] + '/' + attr[5] + '/' + attr[6] + '/' + attr[7] + '/' + attr[8];
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPWIDT", Width);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPHEIG", Height);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPSELM", SelM);
            PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYOLCOMP0", "0/" + Width + "/" + Height + '/' + ok);
        }
    }

    public void OnBrowse() {
        imPk.InitSelection(gameObject, "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPSTARTSTRING", 1);
    }

    public void OnBrowseFinish(string j) {
        BFold.text = Path.GetFileName(j);
    }

    public void MODChT() {
        MOD = true;
    }

    public void MODChF(GameObject sender) {
        if (SelM == 0) {
            try { long durt = bridge.GetDuration();
                bridge.Seek((long)(durt * sender.GetComponent<Slider>().value));
                refcool = 0.1f; } catch { }
        } else { asasd.time = sender.GetComponent<Slider>().value * asasd.clip.length; }
        MOD = false;
    }

    public void ModSOrtO() {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "SO", int.Parse(soo.text));
        gameObject.GetComponent<SortOrderOrg>().Apply();
    }

    public void DeleteObj(int wf) {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TYPE", 69696);
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ") + 1);
        Sdrg.OnElemSelect(72769); Destroy(gameObject);
    }

    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BackCol = new Color(PlayerPrefs.GetFloat(colorstart + "MPBACKCLR", 0f), PlayerPrefs.GetFloat(colorstart + "MPBACKCLG", 0f), PlayerPrefs.GetFloat(colorstart + "MPBACKCLB", 0f));
        backwow.color = BackCol; if (!SceneMode) { BCLs.color = BackCol; }
    }

    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject);
    }

    public void RequestLayout() {
        string keys = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYO";
        lm.TriggerChange(gameObject, keys);
    }

    public void LoadSaved() { string kety = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MPLAYO";
        Comps.Clear(); for (int c = 0; c < 999; c++) {
            if(PlayerPrefs.GetString(kety + "LCOMP" + c, ":(") != ":(") {
                Comps.Add(PlayerPrefs.GetString(kety + "LCOMP" + c));
            } else break;
        } LoadComps();
    }

    // Voids exclusive to Use not Moddify
    public void PlayBd() {
        if (SelM == 0) { refcool = 1f;
            bool wasPlaying = bridge.IsPlaying(); bridge.TogglePlayPause();
            if (SceneMode) if (wasPlaying) { Menu2Playback[0].transform.GetChild(0).gameObject.SetActive(true); Menu2Playback[0].transform.GetChild(1).gameObject.SetActive(false); }
            else { Menu2Playback[0].transform.GetChild(1).gameObject.SetActive(true); Menu2Playback[0].transform.GetChild(0).gameObject.SetActive(false); }
            if (SceneMode) foreach (string s in Comps) if(s.StartsWith("1/") && (s.EndsWith("/0/0") || s.EndsWith("/0/1"))) { 
               myobj.transform.GetChild(5).GetChild(Comps.IndexOf(s)-1).GetChild(0).GetChild(0).gameObject.SetActive(wasPlaying);
               myobj.transform.GetChild(5).GetChild(Comps.IndexOf(s)-1).GetChild(0).GetChild(1).gameObject.SetActive(!wasPlaying); }
            currentValue = (bridge.GetPosition() / 1000f);
        } else { 
        if (!asasd.isPlaying) asasd.Play(); else asasd.Pause();
        if(SceneMode) if (!asasd.isPlaying) { Menu2Playback[0].transform.GetChild(0).gameObject.SetActive(true); Menu2Playback[0].transform.GetChild(1).gameObject.SetActive(false); }
        else { Menu2Playback[0].transform.GetChild(1).gameObject.SetActive(true); Menu2Playback[0].transform.GetChild(0).gameObject.SetActive(false); }
        if (SceneMode) foreach (string s in Comps) if(s.StartsWith("1/") && (s.EndsWith("/0/0") || s.EndsWith("/0/1"))) { 
               myobj.transform.GetChild(5).GetChild(Comps.IndexOf(s)-1).GetChild(0).GetChild(0).gameObject.SetActive(!asasd.isPlaying);
               myobj.transform.GetChild(5).GetChild(Comps.IndexOf(s)-1).GetChild(0).GetChild(1).gameObject.SetActive(asasd.isPlaying); } }
    }
    //
    public void SkipB(int M) {
        if (SelM == 0) { refcool = 0.1f;
            try { bridge.Seek(bridge.GetPosition() + ((M == 1) ? -5000 : 5000)); } catch { }
        } else {
            if (M == 1) asasd.time = Mathf.Max(0f, asasd.time - 5f); else asasd.time = asasd.time = Mathf.Min(asasd.time + 5f, asasd.clip.length);
        }
    }
    //
    public void TrackB(int M) {
        if(SelM == 0) { refcool = 0.1f;
            if (M == 0) bridge.Next();
            else bridge.Previous();
        } else if(SelM == 1) { 
        if (M == 1) { if (asasd.time >= 2.25f) asasd.time = 0f;
            else { IndexinFOld = Mathf.Max(0, IndexinFOld - 1);
            currentsong = songs[IndexinFOld]; asasd.Stop();
            StartCoroutine(LoadAudio(currentsong)); }
        } else { IndexinFOld = Mathf.Min(songs.Length - 1, IndexinFOld + 1);
            currentsong = songs[IndexinFOld]; asasd.Stop();
            StartCoroutine(LoadAudio(currentsong));
        } } else if(SelM == 2) { 
        if (M == 1) { if (asasd.time >= 2.25f) asasd.time = 0f;
            else { IndexinFOld = Mathf.Max(0, IndexinFOld - 1);
            currentsong = songs[IndexinFOld]; asasd.Stop();
            StartCoroutine(LoadAudio2(PlayerPrefs.GetString("URLSONG" + currentsong, "Nothing!!!!"))); }
        } else { IndexinFOld = Mathf.Min(songs.Length - 1, IndexinFOld + 1);
            currentsong = songs[IndexinFOld]; asasd.Stop();
            StartCoroutine(LoadAudio2(PlayerPrefs.GetString("URLSONG" + currentsong, "Nothing!!!!")));
        } }
    }
    //
    public void ChangeSong(string title, string artist, string album, long duration) {
        savedDuration = duration; currentValue = 0f; refcool = 20f;
        for(int c = 0; c < Comps.Count; c++) {
            if(Comps[c].StartsWith("5/")) {
                if (string.IsNullOrEmpty(album)) myobj.transform.GetChild(5).GetChild(c - 1).GetComponent<PlaybackText>().actualtext.text = artist + " - " + title;
                else myobj.transform.GetChild(5).GetChild(c - 1).GetComponent<PlaybackText>().actualtext.text = artist + " - " + album + " - " + title;
                myobj.transform.GetChild(5).GetChild(c - 1).GetComponent<PlaybackText>().LayChange();
            }
        }
    }
    //
    public void BackB() {
        if (SelM == 0) return; Menu2.SetActive(true);
        if (!asasd.isPlaying) { Menu2Playback[0].transform.GetChild(0).gameObject.SetActive(true); Menu2Playback[0].transform.GetChild(1).gameObject.SetActive(false); }
        else { Menu2Playback[0].transform.GetChild(1).gameObject.SetActive(true); Menu2Playback[0].transform.GetChild(0).gameObject.SetActive(false); }
        if(SelM == 1) Menu2Playback[1].GetComponent<Text>().text = Path.GetFileNameWithoutExtension(currentsong);
        if (SelM == 2) Menu2Playback[1].GetComponent<Text>().text = PlayerPrefs.GetString("URLSONGNAME" + currentsong, "Nothing!!!!");
    }
    // 
    public void LoopValueChanged() { if (SelM == 0) return;
        Looping = !Looping; if (SceneMode) foreach (string s in Comps) if(s.StartsWith("1/") && (s.EndsWith("/4/0") || s.EndsWith("/4/1"))) {
        Color32 ad = myobj.transform.GetChild(5).GetChild(Comps.IndexOf(s) - 1).GetChild(4).GetChild(0).GetComponent<Image>().color;
        if (Looping) myobj.transform.GetChild(5).GetChild(Comps.IndexOf(s) - 1).GetChild(4).GetChild(0).GetComponent<Image>().color = new Color32(ad.r, ad.g, ad.b, (byte)(ad.a * 2));
        else myobj.transform.GetChild(5).GetChild(Comps.IndexOf(s) - 1).GetChild(4).GetChild(0).GetComponent<Image>().color = new Color32(ad.r, ad.g, ad.b, (byte)(ad.a / 2)); }
    }
    //
    public void LoadSwetup(string startfile, string director) {
        string[] songs2 = GetFoldersAndAudioFiles(director, true);
        IndexinFOld = Array.IndexOf(songs2, startfile);
        songs = songs2; currentsong = startfile;
        StartCoroutine(LoadAudio(startfile));
    } 
    // 
    public void LoadSwetup2(string startfile, string director) { string[] songs2;
        List<string> Indx = new List<string>();
            for (int s = 0; s < PlayerPrefs.GetInt("GETURLSONGS", 0); s++) {
            if (PlayerPrefs.GetString("URLSONG" + s, "nn") != "nn") {
                Indx.Add(s + "");
            }
        } songs2 = Indx.ToArray();
        IndexinFOld = Array.IndexOf(songs2, startfile);
        songs = songs2; currentsong = startfile;
        StartCoroutine(LoadAudio2(PlayerPrefs.GetString("URLSONG" + startfile, "Nothing!!!!")));
    } 
    // 
    public static string[] GetFoldersAndAudioFiles(string path, bool songonly) {
        if (!Directory.Exists(path)) return Array.Empty<string>();
        string[] audioExtensions = { ".mp3", ".wav", ".ogg" };
        var folders = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path).Where(file => audioExtensions.Contains(Path.GetExtension(file).ToLower())).ToArray();
        if(!songonly) return folders.Concat(files).ToArray();
        else return files.ToArray();
    }
    //
    public void LoadDIrecyt(string p) { string[] files;
        if (SelM == 1) files = GetFoldersAndAudioFiles(p, false);
        else { List<string> Indx = new List<string>();
            for (int s = 0; s < PlayerPrefs.GetInt("GETURLSONGS", 0); s++) {
            if (PlayerPrefs.GetString("URLSONG" + s, "nn") != "nn") {
                Indx.Add(s + "");
            }
        } files = Indx.ToArray(); }
        foreach (Transform k in content2) Destroy(k.gameObject); int realj = 0;
        int FilesPrRow = (int)((gameObject.GetComponent<RectTransform>().sizeDelta.x - 88) / 300);
        float FileSize = ((gameObject.GetComponent<RectTransform>().sizeDelta.x - 88) / (float)FilesPrRow) - 5f;
        for(int j = -1; j < files.Length; j++) { if(j == -1) { if(SelM == 2) { string aa = Path.GetDirectoryName(p);
                GameObject backfold = Instantiate(cln2,content2); realj++;
                backfold.GetComponent<RectTransform>().anchoredPosition = new Vector2(14f, -14f);
                backfold.GetComponent<RectTransform>().sizeDelta = new Vector2(FileSize, 100f);
                backfold.GetComponent<Button>().onClick.AddListener(() => { LoadDIrecyt(aa); });
                backfold.SetActive(true); }
            } else { if (audioExtensions.Contains(Path.GetExtension(files[j])) || SelM == 2) {
                    GameObject file = Instantiate(cln1, content2); file.SetActive(true);
                    file.GetComponent<RectTransform>().anchoredPosition = new Vector2(14f + ((realj % FilesPrRow) * (FileSize + 5f)), -14f - ((realj / FilesPrRow) * 105f));
                    file.GetComponent<RectTransform>().sizeDelta = new Vector2(FileSize, 100f);
                    if (SelM == 1) file.transform.GetChild(0).GetComponent<Text>().text = Path.GetFileNameWithoutExtension(files[j]);
                    if (SelM == 2) file.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("URLSONGNAME" + files[j], "Nothing!!!!");
                    file.SetActive(true); realj++;
                    int lf = j; if(SelM == 1) file.GetComponent<Button>().onClick.AddListener(() => { LoadSwetup(files[lf],Path.GetDirectoryName(files[lf])); });
                    if(SelM == 2) file.GetComponent<Button>().onClick.AddListener(() => { LoadSwetup2(files[lf],Path.GetDirectoryName(files[lf])); });
                } else { GameObject folder = Instantiate(cln2, content2); folder.SetActive(true);
                    folder.GetComponent<RectTransform>().anchoredPosition = new Vector2(14f + ((realj % FilesPrRow) * (FileSize + 5f)), -14f - ((realj / FilesPrRow) * 105f));
                    folder.GetComponent<RectTransform>().sizeDelta = new Vector2(FileSize, 100f);
                    folder.transform.GetChild(0).GetComponent<Text>().text = Path.GetFileName(files[j]); folder.SetActive(true);
                    int lf = j; folder.GetComponent<Button>().onClick.AddListener(() => { LoadDIrecyt(files[lf]); }); realj++;
                } 
            }
        } content2.sizeDelta = new Vector2(content2.sizeDelta.x, 125f + ((realj / FilesPrRow) * 105f));
    }
    // End

    private IEnumerator LoadAudio(string filePath) {
        string url = "file://" + filePath; LoaderIco.SetActive(true); LoaderIco.GetComponent<LoadIcon>().enabled = false;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, GetAudioType(filePath))) {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success) {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                asasd.clip = clip; PlayBd();
                LoaderIco.SetActive(false);
                Menu2.SetActive(false);
                for(int c = 0; c < Comps.Count; c++) {
                    if(Comps[c].StartsWith("5/")) {
                        myobj.transform.GetChild(5).GetChild(c - 1).GetComponent<PlaybackText>().actualtext.text = Path.GetFileNameWithoutExtension(currentsong);
                        myobj.transform.GetChild(5).GetChild(c - 1).GetComponent<PlaybackText>().LayChange();
                    }
                }
            }
        }
    }

    private IEnumerator LoadAudio2(string googleExportUrl) {
        LoaderIco.SetActive(true); LoaderIco.GetComponent<LoadIcon>().enabled = true;
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(googleExportUrl, AudioType.MPEG)) {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success) {
                LoaderIco.GetComponent<LoadIcon>().enabled = false;
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                asasd.clip = clip; PlayBd();
                LoaderIco.SetActive(false);
                Menu2.SetActive(false);
                for (int c = 0; c < Comps.Count; c++) {
                    if (Comps[c].StartsWith("5/")) {
                        myobj.transform.GetChild(5).GetChild(c - 1).GetComponent<PlaybackText>().actualtext.text = PlayerPrefs.GetString("URLSONGNAME" + currentsong, "Nothing!!!!");
                        myobj.transform.GetChild(5).GetChild(c - 1).GetComponent<PlaybackText>().LayChange();
                    }
                }
            }
        }
    }

    private AudioType GetAudioType(string filePath)
    {
        string ext = System.IO.Path.GetExtension(filePath).ToLower();
        if (ext == ".ogg") return AudioType.OGGVORBIS;
        else if (ext == ".mp3") return AudioType.MPEG;
        else if (ext == ".wav") return AudioType.WAV;
        else return AudioType.UNKNOWN;
    }

    public void LoadComps() {
        for (int c = 0; c < myobj.transform.GetChild(5).childCount; c++) Destroy(myobj.transform.GetChild(5).GetChild(c).gameObject);
        for(int lc = 0; lc < Comps.Count; lc++) { string[] attr = Comps[lc].Split('/');
             if (attr[0] == "0") { // Background - SIZE X - SIZE Y - MODE - COLOR 1 - COLOR 2 - COLOR 3 - COLOR 4 - IMGPATH
                string[] attr2 = attr[4].Split(','); string[] attr3 = attr[5].Split(','); string[] attr4 = attr[6].Split(','); string[] attr5 = attr[7].Split(',');
                if (attr[3] == "0") { myobj.GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), 255);
                    myobj.transform.GetChild(0).gameObject.SetActive(false); myobj.transform.GetChild(1).gameObject.SetActive(false);
                    myobj.transform.GetChild(2).gameObject.SetActive(false); myobj.transform.GetChild(3).gameObject.SetActive(false);
                    myobj.transform.GetChild(4).gameObject.SetActive(false); }
                else if (attr[3] == "1") { myobj.GetComponent<Image>().color = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), 255);
                    myobj.transform.GetChild(0).GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), 255);
                    myobj.transform.GetChild(0).gameObject.SetActive(true); myobj.transform.GetChild(1).gameObject.SetActive(false);
                    myobj.transform.GetChild(2).gameObject.SetActive(false); myobj.transform.GetChild(3).gameObject.SetActive(false);
                    myobj.transform.GetChild(4).gameObject.SetActive(false);
                } else if (attr[3] == "2") { myobj.GetComponent<Image>().color = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), 255);
                    myobj.transform.GetChild(1).GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), 255);
                    myobj.transform.GetChild(0).gameObject.SetActive(false); myobj.transform.GetChild(1).gameObject.SetActive(true);
                    myobj.transform.GetChild(2).gameObject.SetActive(false); myobj.transform.GetChild(3).gameObject.SetActive(false);
                    myobj.transform.GetChild(4).gameObject.SetActive(false);
                } else if (attr[3] == "3") { myobj.GetComponent<Image>().color = new Color32(byte.Parse(attr5[0]), byte.Parse(attr5[1]), byte.Parse(attr5[2]), 255);
                    myobj.transform.GetChild(2).GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), 255);
                    myobj.transform.GetChild(3).GetComponent<Image>().color = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), 255);
                    myobj.transform.GetChild(4).GetComponent<Image>().color = new Color32(byte.Parse(attr4[0]), byte.Parse(attr4[1]), byte.Parse(attr4[2]), 255);
                    myobj.transform.GetChild(0).gameObject.SetActive(false); myobj.transform.GetChild(1).gameObject.SetActive(false);
                    myobj.transform.GetChild(2).gameObject.SetActive(true); myobj.transform.GetChild(3).gameObject.SetActive(true);
                    myobj.transform.GetChild(4).gameObject.SetActive(true);
                } else if (attr[3] == "4") { myobj.GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), 255);
                    myobj.transform.GetChild(0).gameObject.SetActive(false); myobj.transform.GetChild(1).gameObject.SetActive(false);
                    myobj.transform.GetChild(2).gameObject.SetActive(false); myobj.transform.GetChild(3).gameObject.SetActive(false);
                    myobj.transform.GetChild(4).gameObject.SetActive(false); activetexpath = attr[8];
                    if(activetexpath != "p" || activetexpath != "") OnImgLoad(); }
                else if (attr[3] == "5") { myobj.GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), 255); 
                    myobj.transform.GetChild(2).GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), 255); 
                    myobj.transform.GetChild(3).GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), 255); 
                    myobj.transform.GetChild(4).GetComponent<Image>().color = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), 255); 
                    myobj.transform.GetChild(0).gameObject.SetActive(false); myobj.transform.GetChild(1).gameObject.SetActive(false);
                    myobj.transform.GetChild(2).gameObject.SetActive(true); myobj.transform.GetChild(3).gameObject.SetActive(true);
                    myobj.transform.GetChild(4).gameObject.SetActive(true); }
            } else if (attr[0] == "1") { // Button - POS X - POS Y - SIZE X - SIZE Y - BACK COLOR - ICON COLOR - ICON SIZE - TYPE (0-Play;1-Skip;2-Song;3-Back;4-Loop) - REVERSE TYPE
                GameObject PlayB = Instantiate(ClonerObjs[0], myobj.transform.GetChild(5)); PlayB.SetActive(true);
                PlayB.GetComponent<RectTransform>().anchoredPosition = new Vector2(int.Parse(attr[1]), int.Parse(attr[2]));
                PlayB.transform.localScale = new Vector2(float.Parse(attr[3], CultureInfo.InvariantCulture), float.Parse(attr[4], CultureInfo.InvariantCulture));
                string[] attr2 = attr[5].Split(',');
                PlayB.GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                string[] attr3 = attr[6].Split(','); int attrtyp = int.Parse(attr[8]); PlayB.transform.GetChild(attrtyp).gameObject.SetActive(true);
                Color32 mycol = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), byte.Parse(attr3[3]));
                if (attrtyp == 0) { PlayB.transform.GetChild(0).GetChild(1).GetComponent<RawImage>().color = mycol;
                PlayB.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<RawImage>().color = mycol;
                if(SceneMode) PlayB.GetComponent<Button>().onClick.AddListener(() => { PlayBd(); }); }
                if (attrtyp == 1) { PlayB.transform.GetChild(1).GetChild(0).GetComponent<RawImage>().color = mycol;
                PlayB.transform.GetChild(1).GetChild(1).GetComponent<RawImage>().color = mycol;
                if(SceneMode) PlayB.GetComponent<Button>().onClick.AddListener(() => { SkipB(int.Parse(attr[9])); }); }
                if (attrtyp == 2) { PlayB.transform.GetChild(2).GetChild(0).GetComponent<RawImage>().color = mycol;
                PlayB.transform.GetChild(2).GetChild(1).GetComponent<RawImage>().color = mycol;
                if(SceneMode) PlayB.GetComponent<Button>().onClick.AddListener(() => { TrackB(int.Parse(attr[9])); }); }
                if (attrtyp == 3) {  PlayB.transform.GetChild(3).GetChild(0).GetComponent<Image>().color = mycol;
                PlayB.transform.GetChild(3).GetChild(1).GetComponent<Image>().color = mycol;
                PlayB.transform.GetChild(3).GetChild(2).GetComponent<Image>().color = mycol;
                if(SceneMode) PlayB.GetComponent<Button>().onClick.AddListener(() => { BackB(); }); }
                if (attrtyp == 4) { PlayB.transform.GetChild(4).GetChild(0).GetComponent<Image>().color = new Color32(mycol.r,mycol.g,mycol.b,(byte)(mycol.a / 2));
                if(SceneMode) PlayB.GetComponent<Button>().onClick.AddListener(() => { LoopValueChanged(); }); }
                PlayB.transform.GetChild(attrtyp).localScale = new Vector2(float.Parse(attr[7], CultureInfo.InvariantCulture), float.Parse(attr[7], CultureInfo.InvariantCulture));
                if (attr[9] == "1") PlayB.transform.GetChild(attrtyp).rotation = Quaternion.Euler(0f, 0f, 180f); else PlayB.transform.GetChild(attrtyp).rotation = Quaternion.Euler(0f, 0f, 0f);
                int llc = lc;  PlayB.name = lc + "";
            } else if(attr[0] == "2") { // Playback Slider - POS X - POS Y - RSIZE X - RSIZE Y - BACK COLOR - HANDLE COLOR - HANDLE SIZE X - FILL COLOR
                GameObject PlayB = Instantiate(ClonerObjs[1], myobj.transform.GetChild(5)); PlayB.SetActive(true);
                PlayB.GetComponent<RectTransform>().anchoredPosition = new Vector2(int.Parse(attr[1]), int.Parse(attr[2]));
                PlayB.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(attr[3]), int.Parse(attr[4]));
                string[] attr2 = attr[5].Split(',');
                PlayB.transform.GetChild(0).GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                PlayB.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                string[] attr3 = attr[6].Split(',');
                PlayB.transform.GetChild(0).GetComponent<Outline>().effectColor = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), byte.Parse(attr3[3]));
                PlayB.transform.GetChild(2).GetChild(0).GetComponent<Outline>().effectColor = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), byte.Parse(attr3[3]));
                PlayB.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(attr[7]), PlayB.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
                string[] attr4 = attr[8].Split(',');
                PlayB.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(byte.Parse(attr4[0]), byte.Parse(attr4[1]), byte.Parse(attr4[2]), byte.Parse(attr4[3]));
                int llc = lc; PlayB.name = lc + "";
            } else if(attr[0] == "3") { // Spectrum - POS X - POS Y - RSIZE X - RSIZE Y - COLOR - SEGMENTS - REFRESH RATE - AUDIO MULT - GAPS - GAP SIZE - BOTTOMED - SMOOTH
                GameObject PlayB = Instantiate(ClonerObjs[2], myobj.transform.GetChild(5));
                PlayB.GetComponent<RectTransform>().anchoredPosition = new Vector2(int.Parse(attr[1]), int.Parse(attr[2]));
                PlayB.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(attr[3]), int.Parse(attr[4]));
                string[] attr2 = attr[5].Split(',');
                PlayB.transform.GetChild(1).GetComponent<RawImage>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                if(attr[9] == "1") PlayB.GetComponent<Spectrum>().Gapped = true; else PlayB.GetComponent<Spectrum>().Gapped = false;
                if(attr[11] == "1") PlayB.GetComponent<Spectrum>().Bottomed = true; else PlayB.GetComponent<Spectrum>().Bottomed = false;
                if(attr[12] == "1") PlayB.GetComponent<Spectrum>().Smoothing = true; else PlayB.GetComponent<Spectrum>().Smoothing = false;
                PlayB.GetComponent<Spectrum>().GapSize = int.Parse(attr[10]); PlayB.GetComponent<Spectrum>().Multiplier = float.Parse(attr[8], CultureInfo.InvariantCulture);
                PlayB.GetComponent<Spectrum>().Segments = int.Parse(attr[6]); PlayB.GetComponent<Spectrum>().RefreshRate = int.Parse(attr[7]);
                FFtMax = Mathf.Max(Mathf.Max(Mathf.ClosestPowerOfTwo(int.Parse(attr[6])), 64), FFtMax);
                PlayB.SetActive(true); PlayB.GetComponent<Spectrum>().SelectedMod = SelM; PlayB.GetComponent<Spectrum>().Start();
                int llc = lc; PlayB.name = lc + "";
            } else if(attr[0] == "4") { // Scope - POS X - POS Y - RSIZE X - RSIZE Y - COLOR - SAMPLES - REFRESH RATE - LINE SIZE - VECTOR
                GameObject PlayB = Instantiate(ClonerObjs[3], myobj.transform.GetChild(5));
                PlayB.GetComponent<RectTransform>().anchoredPosition = new Vector2(int.Parse(attr[1]), int.Parse(attr[2]));
                PlayB.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(attr[3]), int.Parse(attr[4]));
                string[] attr2 = attr[5].Split(',');
                PlayB.GetComponent<UILineRenderer>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                if(attr[9] == "1") PlayB.GetComponent<Osciloscope>().Vectorscope = true; else PlayB.GetComponent<Osciloscope>().Vectorscope = false;
                PlayB.GetComponent<Osciloscope>().Samples = int.Parse(attr[6]); PlayB.GetComponent<Osciloscope>().RefRate = int.Parse(attr[7]);
                PlayB.GetComponent<Osciloscope>().LNTHICK = float.Parse(attr[8],CultureInfo.InvariantCulture);
                SamplesMax = Mathf.Max(Mathf.Clamp(int.Parse(attr[6]), 64, 8192), SamplesMax);
                PlayB.SetActive(true); PlayB.GetComponent<Osciloscope>().SelectedMod = SelM; PlayB.GetComponent<Osciloscope>().ChangeLay();
                int llc = lc; PlayB.name = lc + "";
            } else if(attr[0] == "5") { // Playback Text - POS X - POS Y - RSIZE X - RSIZE Y - TEXT COLOR - BACK COLOR - FONT - ANIMATION - BACKGROUND - EDGE BLUR - SCROLL SPEED - FONT SIZE
                GameObject PlayB = Instantiate(ClonerObjs[4], myobj.transform.GetChild(5));
                PlayB.GetComponent<RectTransform>().anchoredPosition = new Vector2(int.Parse(attr[1]), int.Parse(attr[2]));
                PlayB.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(attr[3]), int.Parse(attr[4]));
                string[] attr2 = attr[5].Split(','); string[] attr3 = attr[6].Split(',');
                PlayB.GetComponent<PlaybackText>().actualtext.color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                PlayB.GetComponent<Image>().color = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), byte.Parse(attr3[3]));
                PlayB.SetActive(true); int font = int.Parse(attr[7]);
                int anim = int.Parse(attr[8]);
                bool Back; if (attr[9] == "1") Back = true; else Back = false;
                bool EDBL; if (attr[10] == "1") EDBL = true; else EDBL = false;
                float FSP = float.Parse(attr[11], CultureInfo.InvariantCulture); int FS = int.Parse(attr[12]);
                PlayB.GetComponent<PlaybackText>().AnimMode = anim; PlayB.GetComponent<PlaybackText>().Background = Back;
                PlayB.GetComponent<PlaybackText>().EdgeBlur = EDBL; PlayB.GetComponent<PlaybackText>().font = font;
                PlayB.GetComponent<PlaybackText>().FontSize = FS; PlayB.GetComponent<PlaybackText>().AnimSpeed = FSP;
                PlayB.GetComponent<PlaybackText>().LayChange();
                int llc = lc; PlayB.name = lc + "";
            }
        }
    }

    public void OnImgLoad() {
        if (activetexpath.StartsWith("https://") || activetexpath.StartsWith("http://")) { StartCoroutine(DownloadImage(activetexpath)); }
        else { StartCoroutine(LDImg(activetexpath)); }
        string[] UlSt = Comps[0].Split('/');
    }

    public IEnumerator LDImg(string filePath) {
        if (filePath != "") { activetexpath = filePath;
            Texture2D tex = new Texture2D(2, 2); byte[] imageData = File.ReadAllBytes(filePath); tex.LoadImage(imageData);
            Sprite cranberry = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
            myobj.GetComponent<Image>().sprite = cranberry;
        } yield return null;
    }

    IEnumerator DownloadImage(string url) {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url)) {
            yield return uwr.SendWebRequest();
            if (uwr.result == UnityWebRequest.Result.Success) {
                Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
                Sprite cranberry = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
                myobj.GetComponent<Image>().sprite = cranberry; activetexpath = url;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate() { string[] ita = Comps[0].Split('/'); 
        if(SceneMode) { if(SelM != 0) { if (asasd.isPlaying) {
            if (!MOD) { foreach (Transform ts in myobj.transform.GetChild(5)) if (ts.GetComponent<Slider>()) ts.GetComponent<Slider>().value = (asasd.time / asasd.clip.length); }
            if (Looping) { if (asasd.clip.length - asasd.time <= 0.06f) asasd.time = 0f; }
            else if (asasd.clip.length - asasd.time <= 0.06f) TrackB(0);
            if (ita[3] == "5") { string[] ita2 = ita[4].Split(','); string[] ita3 = ita[5].Split(',');
                float[] smlpl = new float[64]; asasd.GetSpectrumData(smlpl, 0, FFTWindow.BlackmanHarris);
                Color32 c1 = new Color32(byte.Parse(ita2[0]), byte.Parse(ita2[1]), byte.Parse(ita2[2]), 255);
                Color32 c2 = new Color32(byte.Parse(ita3[0]), byte.Parse(ita3[1]), byte.Parse(ita3[2]), 255);
                Datas[0] += Mathf.Min((smlpl[0] + smlpl[1] + smlpl[2] + smlpl[3]) * 1.5f, 1) / 10f; Datas[0] /= 1.1f; 
                Datas[1] += Mathf.Min((smlpl[12] + smlpl[13] + smlpl[14] + smlpl[15]) * 10f, 1) / 10f; Datas[1] /= 1.1f; 
                Datas[2] += Mathf.Min((smlpl[24] + smlpl[25] + smlpl[26] + smlpl[27]) * 30f, 1) / 10f; Datas[2] /= 1.1f; 
                Datas[3] += Mathf.Min((smlpl[36] + smlpl[37] + smlpl[38] + smlpl[39]) * 50f, 1) / 10f; Datas[3] /= 1.1f; 
                myobj.GetComponent<Image>().color = Color32.Lerp(c1, c2, Datas[0]);
                myobj.transform.GetChild(2).GetComponent<Image>().color = Color32.Lerp(c1, c2, Datas[1]);
                myobj.transform.GetChild(3).GetComponent<Image>().color = Color32.Lerp(c1, c2, Datas[2]);
                myobj.transform.GetChild(4).GetComponent<Image>().color = Color32.Lerp(c1, c2, Datas[3]);
            }
        } } else {
            if(savedplay) { currentValue += Time.fixedDeltaTime;
            refcool -= Time.fixedDeltaTime;
            if (refcool < 0f) { refcool = 20f; currentValue = (bridge.GetPosition() / 1000f); }
            if (!MOD) { foreach (Transform ts in myobj.transform.GetChild(5)) if (ts.GetComponent<Slider>()) ts.GetComponent<Slider>().value = (float)(currentValue / (double)(savedDuration / 1000d)); } }
            if (ita[3] == "5") {
                string[] ita2 = ita[4].Split(','); string[] ita3 = ita[5].Split(',');
                float[] smlpl = mab.GetFFTSamples(64);
                Color32 c1 = new Color32(byte.Parse(ita2[0]), byte.Parse(ita2[1]), byte.Parse(ita2[2]), 255);
                Color32 c2 = new Color32(byte.Parse(ita3[0]), byte.Parse(ita3[1]), byte.Parse(ita3[2]), 255);
                Datas[0] += Mathf.Min((smlpl[8] + smlpl[9] + smlpl[10] + smlpl[11]) * 2f, 1) / 10f; Datas[0] /= 1.1f; 
                Datas[1] += Mathf.Min((smlpl[12] + smlpl[13] + smlpl[14] + smlpl[15]) * 2.3f, 1) / 10f; Datas[1] /= 1.1f;
                Datas[2] += Mathf.Min((smlpl[24] + smlpl[25] + smlpl[26] + smlpl[27]) * 2.7f, 1) / 10f; Datas[2] /= 1.1f;
                Datas[3] += Mathf.Min((smlpl[36] + smlpl[37] + smlpl[38] + smlpl[39]) * 3f, 1) / 10f; Datas[3] /= 1.1f;
                myobj.GetComponent<Image>().color = Color32.Lerp(c1, c2, Datas[0]);
                myobj.transform.GetChild(2).GetComponent<Image>().color = Color32.Lerp(c1, c2, Datas[1]);
                myobj.transform.GetChild(3).GetComponent<Image>().color = Color32.Lerp(c1, c2, Datas[2]);
                myobj.transform.GetChild(4).GetComponent<Image>().color = Color32.Lerp(c1, c2, Datas[3]);
            }
        } }
    }

    void Update() {
        
    }
}
