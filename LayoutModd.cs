using System.IO;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.Networking;

public class LayoutModd : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa modificator de layout
    // Folosita pentru a modifica aspectul elementului 'Music Player's
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public ColorPicker CoPi; // Clasa universala folosita pentru selectarea culorilor
    public ImagePicker ImPk; // Clasa universala folosita pentru selectarea imaginilor

    // Obiecte folosite de catre clasa
    public GameObject sender;
    public string kety;

    public GameObject cloner1;
    public RectTransform content1;

    public RectTransform view2;
    public GameObject myobj;

    public List<GameObject> Menus;
    public RectTransform content3;

    public List<GameObject> ClonerObjs;
    public List<string> Comps;

    public List<GameObject> Object1MenuComps;
    public List<GameObject> Object2MenuComps;
    public List<GameObject> Object3MenuComps;
    public List<GameObject> Object4MenuComps;
    public List<GameObject> Object5MenuComps;
    public List<GameObject> Object6MenuComps;
    public List<GameObject> Object7MenuComps;
    public List<GameObject> Object8MenuComps;
    public List<GameObject> Object9MenuComps;
    public List<GameObject> Object10MenuComps;
    public List<GameObject> Object11MenuComps;
    public int SlectedObj; public GameObject myobje;
    public GameObject POD;
    public List<InputField> OWO;
    public bool Touchy; public int reqtyoe;
    public bool LOADMODE; public Dropdown Persetss;
    public string activetexpath; public bool inrs;
    
    // Functie executata cand un element este creat / sters
    public void TriggerChange(GameObject senders, string key) {
        sender = senders; kety = key; Comps.Clear(); OpenMenu(-56);
        foreach (Transform a in content1) Destroy(a.gameObject);
        content1.sizeDelta = new Vector2(content1.sizeDelta.x, 0f); for (int c = 0; c < 999; c++) {
            if(PlayerPrefs.GetString(kety + "LCOMP" + c,":(") != ":(") {
                Comps.Add(PlayerPrefs.GetString(kety + "LCOMP" + c));
            } else break;
        } Content1Refresh(); LoadComps();
    }

    // Functie executata cand lista de elemente este schimbata
    public void Content1Refresh() {
        foreach (Transform a in content1) Destroy(a.gameObject);
        content1.sizeDelta = new Vector2(content1.sizeDelta.x, 0f); for (int c = 0; c < Comps.Count; c++) {
            GameObject wow = Instantiate(cloner1, content1);
            content1.sizeDelta += new Vector2(0f, 90f); wow.SetActive(true);
            wow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -(c * 90f));
            if (Comps[c].StartsWith("0/")) wow.transform.GetChild(0).GetComponent<Text>().text = "Background";
            else if (Comps[c].StartsWith("1/") && (Comps[c].EndsWith("0/0") || Comps[c].EndsWith("0/1"))) wow.transform.GetChild(0).GetComponent<Text>().text = "Play Button";
            else if (Comps[c].StartsWith("1/") && (Comps[c].EndsWith("1/0") || Comps[c].EndsWith("1/1"))) wow.transform.GetChild(0).GetComponent<Text>().text = "Skip Button";
            else if (Comps[c].StartsWith("1/") && (Comps[c].EndsWith("2/0") || Comps[c].EndsWith("2/1"))) wow.transform.GetChild(0).GetComponent<Text>().text = "Track Button";
            else if (Comps[c].StartsWith("1/") && (Comps[c].EndsWith("3/0") || Comps[c].EndsWith("3/1"))) wow.transform.GetChild(0).GetComponent<Text>().text = "Back Button";
            else if (Comps[c].StartsWith("1/") && (Comps[c].EndsWith("4/0") || Comps[c].EndsWith("4/1"))) wow.transform.GetChild(0).GetComponent<Text>().text = "Loop Button";
            else if (Comps[c].StartsWith("2/")) wow.transform.GetChild(0).GetComponent<Text>().text = "Playback Slider";
            else if (Comps[c].StartsWith("3/")) wow.transform.GetChild(0).GetComponent<Text>().text = "Spectrum";
            else if (Comps[c].StartsWith("4/") && Comps[c].EndsWith("0")) wow.transform.GetChild(0).GetComponent<Text>().text = "Scope";
            else if (Comps[c].StartsWith("4/") && Comps[c].EndsWith("1")) wow.transform.GetChild(0).GetComponent<Text>().text = "Vectorscope";
            else if (Comps[c].StartsWith("5/")) wow.transform.GetChild(0).GetComponent<Text>().text = "Playback Text";
            int lc = c; wow.GetComponent<Button>().onClick.AddListener(() => { SelectSelf(lc); });
        }
    }

    // Functie executata cand aspectul este aplicat player-ului muzical
    public void Applyer() {
        for (int d = 0; d < 999; d++) if (PlayerPrefs.GetString(kety + "LCOMP" + d, "./") == "./") break; else PlayerPrefs.DeleteKey(kety + "LCOMP" + d);
        for (int l = 0; l < Comps.Count; l++) PlayerPrefs.SetString(kety + "LCOMP" + l, Comps[l]);
        sender.GetComponent<MusicPlayer>().LoadSaved();
    }
    
    // Functie executata cand un obiect este mutat prin tragerea acestuia
    public void AttempTouchy(GameObject obj) {
        if (myobje == obj) Touchy = true;
    }

    // Functie executata cand un obiect nu mai este mutat prin tragerea acestuia
    public void DeActiveTouchy() {
        Touchy = false; OnComponentModdify();
    }

    // Functie executata cand data elementelor este incarcata in obiecte propriu-zise
    public void LoadComps() {
        for (int c = 0; c < myobj.transform.GetChild(5).childCount; c++) Destroy(myobj.transform.GetChild(5).GetChild(c).gameObject);
        for (int lc = 0; lc < Comps.Count; lc++) { string[] attr = Comps[lc].Split('/');
            if (attr[0] == "0") { // Background - SIZE X - SIZE Y - MODE - COLOR 1 - COLOR 2 - COLOR 3 - COLOR 4 - IMGPATH
                view2.sizeDelta = new Vector2(int.Parse(attr[1]), int.Parse(attr[2])); string[] attr2 = attr[4].Split(','); string[] attr3 = attr[5].Split(','); string[] attr4 = attr[6].Split(','); string[] attr5 = attr[7].Split(',');
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
                    if (activetexpath.StartsWith("https://") || activetexpath.StartsWith("http://")) Object11MenuComps[18].GetComponent<Dropdown>().value = 2; 
                    else if(activetexpath != "p" || activetexpath != "") Object11MenuComps[18].GetComponent<Dropdown>().value = 1; 
                    else Object11MenuComps[18].GetComponent<Dropdown>().value = 0; if(Object11MenuComps[18].GetComponent<Dropdown>().value != 0) OnImgLoad();
                } else if (attr[3] == "5") { myobj.GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), 255); 
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
                PlayB.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<RawImage>().color = mycol; }
                if (attrtyp == 1) { PlayB.transform.GetChild(1).GetChild(0).GetComponent<RawImage>().color = mycol;
                PlayB.transform.GetChild(1).GetChild(1).GetComponent<RawImage>().color = mycol; }
                if (attrtyp == 2) { PlayB.transform.GetChild(2).GetChild(0).GetComponent<RawImage>().color = mycol;
                PlayB.transform.GetChild(2).GetChild(1).GetComponent<RawImage>().color = mycol; }
                if (attrtyp == 3) {  PlayB.transform.GetChild(3).GetChild(0).GetComponent<Image>().color = mycol;
                PlayB.transform.GetChild(3).GetChild(1).GetComponent<Image>().color = mycol;
                PlayB.transform.GetChild(3).GetChild(2).GetComponent<Image>().color = mycol; }
                if (attrtyp == 4) {  PlayB.transform.GetChild(4).GetChild(0).GetComponent<Image>().color = mycol; }
                PlayB.transform.GetChild(attrtyp).localScale = new Vector2(float.Parse(attr[7], CultureInfo.InvariantCulture), float.Parse(attr[7], CultureInfo.InvariantCulture));
                if (attr[9] == "1") PlayB.transform.GetChild(attrtyp).rotation = Quaternion.Euler(0f, 0f, 180f); else PlayB.transform.GetChild(attrtyp).rotation = Quaternion.Euler(0f, 0f, 0f);
                int llc = lc; PlayB.GetComponent<Button>().onClick.AddListener(() => { SelectSelf(llc); }); PlayB.name = lc + "";
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
                int llc = lc; PlayB.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { SelectSelf(llc); }); PlayB.name = lc + "";
            } else if(attr[0] == "3") { // Spectrum - POS X - POS Y - RSIZE X - RSIZE Y - COLOR - SEGMENTS - REFRESH RATE - AUDIO MULT - GAPS - GAP SIZE - BOTTOMED - SMOOTH
                GameObject PlayB = Instantiate(ClonerObjs[2], myobj.transform.GetChild(5));
                PlayB.GetComponent<RectTransform>().anchoredPosition = new Vector2(int.Parse(attr[1]), int.Parse(attr[2]));
                PlayB.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(attr[3]), int.Parse(attr[4]));
                string[] attr2 = attr[5].Split(',');
                PlayB.transform.GetChild(1).GetComponent<RawImage>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                PlayB.SetActive(true); PlayB.GetComponent<Spectrum>().Start();
                int llc = lc; PlayB.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { SelectSelf(llc); }); PlayB.name = lc + "";
            } else if(attr[0] == "4") { // Scope - POS X - POS Y - RSIZE X - RSIZE Y - COLOR - SAMPLES - REFRESH RATE - LINE SIZE - VECTOR
                GameObject PlayB = Instantiate(ClonerObjs[3], myobj.transform.GetChild(5));
                PlayB.GetComponent<RectTransform>().anchoredPosition = new Vector2(int.Parse(attr[1]), int.Parse(attr[2]));
                PlayB.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(attr[3]), int.Parse(attr[4]));
                string[] attr2 = attr[5].Split(',');
                PlayB.GetComponent<UILineRenderer>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                PlayB.SetActive(true); PlayB.GetComponent<Osciloscope>().ChangeLay();
                int llc = lc; PlayB.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { SelectSelf(llc); }); PlayB.name = lc + "";
            } else if(attr[0] == "5") { // Playback Text - POS X - POS Y - RSIZE X - RSIZE Y - TEXT COLOR - BACK COLOR - FONT - ANIMATION - BACKGROUND - EDGE BLUR - SCROLL SPEED - FONT SIZE
                GameObject PlayB = Instantiate(ClonerObjs[4], myobj.transform.GetChild(5));
                PlayB.GetComponent<RectTransform>().anchoredPosition = new Vector2(int.Parse(attr[1]), int.Parse(attr[2]));
                PlayB.GetComponent<RectTransform>().sizeDelta = new Vector2(int.Parse(attr[3]), int.Parse(attr[4]));
                string[] attr2 = attr[5].Split(','); string[] attr3 = attr[6].Split(',');
                PlayB.GetComponent<PlaybackText>().actualtext.color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                PlayB.GetComponent<Image>().color = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), byte.Parse(attr3[3]));
                PlayB.SetActive(true); int font = int.Parse(attr[7]); int anim = int.Parse(attr[8]);
                bool Back; if (attr[9] == "1") Back = true; else Back = false;
                bool EDBL; if (attr[10] == "1") EDBL = true; else EDBL = false;
                float FSP = float.Parse(attr[11], CultureInfo.InvariantCulture); int FS = int.Parse(attr[12]);
                PlayB.GetComponent<PlaybackText>().AnimMode = anim; PlayB.GetComponent<PlaybackText>().Background = Back;
                PlayB.GetComponent<PlaybackText>().EdgeBlur = EDBL; PlayB.GetComponent<PlaybackText>().font = font;
                PlayB.GetComponent<PlaybackText>().FontSize = FS; PlayB.GetComponent<PlaybackText>().AnimSpeed = FSP;
                PlayB.GetComponent<PlaybackText>().LayChange();
                int llc = lc; PlayB.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { SelectSelf(llc); }); PlayB.name = lc + "";
            }
        } inrs = true;
    }

    // Functie executata cand pozitia unui obiect pe axa X este modificata
    public void PODXMod(InputField xd) {
        myobje.GetComponent<RectTransform>().anchoredPosition = new Vector2(int.Parse(xd.text),
            myobje.GetComponent<RectTransform>().anchoredPosition.y); OnComponentModdify();
    }

    // Functie executata cand pozitia unui obiect pe axa Y este modificata
    public void PODYMod(InputField yd) {
        myobje.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            myobje.GetComponent<RectTransform>().anchoredPosition.x, int.Parse(yd.text)); OnComponentModdify();
    }

    // Functie executata cand ordinea obiectului este modificata in aspect
    public void PODSOMod(InputField sod) { string temp = Comps[SlectedObj];
        Comps.RemoveAt(SlectedObj); Comps.Insert(Mathf.Max(int.Parse(sod.text) + 1, 0), temp);
        Content1Refresh(); LoadComps(); SelectSelf(int.Parse(sod.text) + 1);
    }

    // Functie executata cand un element este sters
    public void RemoveElement() {
        Comps.RemoveAt(SlectedObj); OpenMenu(-56);
        Content1Refresh(); LoadComps();
    }

    // Functie executata cand presetarea aspectului este schimbata
    public void OnPresetChange() {
        if(Persetss.value == 0) { Comps.Clear();
            Comps.Add("0/" + sender.GetComponent<MusicPlayer>().Width + "/" + sender.GetComponent<MusicPlayer>().Height + "/5/0,0,0/255,255,255/255,255,255/255,255,255/p");
            Comps.Add("1/0/-60/1.55/1.55/255,255,255,255/0,0,0,255/1/0/0");
            Comps.Add("1/185/-69/1.25/1.25/255,255,255,255/0,0,0,255/1/1/0");
            Comps.Add("1/350/-69/1.25/1.25/255,255,255,255/0,0,0,255/1/2/0");
            Comps.Add("1/-185/-69/1.25/1.25/255,255,255,255/0,0,0,255/1/1/1");
            Comps.Add("1/-350/-69/1.25/1.25/255,255,255,255/0,0,0,255/1/2/1");
            Comps.Add("2/0/-217/840/100/0,0,0,255/255,255,255,255/55/255,255,255,255");
            Comps.Add("3/0/128/800/140/255,255,255,255/32/50/0.5/1/20/1/1");
            Comps.Add("1/-395/220/0.8/0.8/0,0,0,255/255,255,255,255/1.25/3/0");
            Comps.Add("1/395/220/0.8/0.8/0,0,0,255/255,255,255,255/1/4/0");
            Comps.Add("5/0/235/680/50/255,255,255,255/0,0,0,255/0/0/1/1/3.0/33");
            Content1Refresh(); LoadComps();
        } if(Persetss.value == 1) { Comps.Clear();
            Comps.Add("0/" + sender.GetComponent<MusicPlayer>().Width + "/" + sender.GetComponent<MusicPlayer>().Height + "/5/0,0,0/255,255,255/255,255,255/255,255,255/p");
            Comps.Add("1/0/-60/1.55/1.55/255,255,255,255/0,0,0,255/1/0/0");
            Comps.Add("1/185/-69/1.25/1.25/255,255,255,255/0,0,0,255/1/1/0");
            Comps.Add("1/350/-69/1.25/1.25/255,255,255,255/0,0,0,255/1/2/0");
            Comps.Add("1/-185/-69/1.25/1.25/255,255,255,255/0,0,0,255/1/1/1");
            Comps.Add("1/-350/-69/1.25/1.25/255,255,255,255/0,0,0,255/1/2/1");
            Comps.Add("2/0/-217/840/100/0,0,0,255/255,255,255,255/55/255,255,255,255");
            Comps.Add("4/0/128/800/140/255,255,255,255/1024/50/4/0");
            Comps.Add("1/-395/220/0.8/0.8/0,0,0,255/255,255,255,255/1.25/3/0");
            Comps.Add("1/395/220/0.8/0.8/0,0,0,255/255,255,255,255/1/4/0");
            Comps.Add("5/0/235/680/50/255,255,255,255/0,0,0,255/0/0/1/1/3.0/33");
            Content1Refresh(); LoadComps();
        } if(Persetss.value == 2) { Comps.Clear();
            Comps.Add("0/" + sender.GetComponent<MusicPlayer>().Width + "/" + sender.GetComponent<MusicPlayer>().Height + "/5/0,0,0/255,255,255/255,255,255/255,255,255/p");
            Comps.Add("1/-220/-60/1.25/1.25/255,255,255,255/0,0,0,255/1/0/0");
            Comps.Add("1/-370/-69/1.05/1.05/255,255,255,255/0,0,0,255/1/2/1");
            Comps.Add("1/-70/-69/1.05/1.05/255,255,255,255/0,0,0,255/1/2/0");
            Comps.Add("2/0/-217/840/100/0,0,0,255/255,255,255,255/55/255,255,255,255");
            Comps.Add("4/220/50/400/400/255,255,255,255/1024/50/4/1");
            Comps.Add("1/-395/220/0.8/0.8/0,0,0,255/255,255,255,255/1.25/3/0");
            Comps.Add("1/-30/220/0.8/0.8/0,0,0,255/255,255,255,255/1/4/0");
            Comps.Add("5/-220/48/400/50/255,255,255,255/0,0,0,255/0/0/1/1/3.0/33");
            Content1Refresh(); LoadComps();
        }
    }

    // Functie executata can un element este creat
    public void AddElement(string ID) { Persetss.value = 3;
        if (ID.StartsWith("1/")) {
            Comps.Add("1/0/0/1/1/255,255,255,255/0,0,0,255/1/" + ID[2] + "/0");
            GameObject PlayB = Instantiate(ClonerObjs[0], myobj.transform.GetChild(5)); PlayB.SetActive(true);
            PlayB.transform.GetChild(int.Parse(ID[2].ToString())).gameObject.SetActive(true); PlayB.name = (Comps.Count - 1) + "";
            int wow = (Comps.Count - 1); PlayB.GetComponent<Button>().onClick.AddListener(() => { SelectSelf(wow); }); SelectSelf(wow);
        } if (ID.StartsWith("2/")) { Comps.Add("2/0/0/500/80/255,255,255,255/255,255,255,255/45/255,255,255,255");
            GameObject PlayB = Instantiate(ClonerObjs[1], myobj.transform.GetChild(5)); PlayB.SetActive(true);
            PlayB.name = (Comps.Count - 1) + ""; int wow = (Comps.Count - 1); PlayB.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { SelectSelf(wow); });
        } if (ID.StartsWith("3/")) { Comps.Add("3/0/0/500/200/255,255,255,255/32/50/0.5/1/20/1/1");
            GameObject PlayB = Instantiate(ClonerObjs[2], myobj.transform.GetChild(5)); PlayB.SetActive(true);
            PlayB.name = (Comps.Count - 1) + ""; int wow = (Comps.Count - 1); PlayB.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { SelectSelf(wow); });
        } if (ID.StartsWith("4/")) { Comps.Add("4/0/0/400/200/255,255,255,255/1024/30/3/1/" + ID[2]);
            GameObject PlayB = Instantiate(ClonerObjs[3], myobj.transform.GetChild(5)); PlayB.SetActive(true);
            PlayB.name = (Comps.Count - 1) + ""; int wow = (Comps.Count - 1); PlayB.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { SelectSelf(wow); });
        } if (ID.StartsWith("5/")) { Comps.Add("5/0/0/400/50/255,255,255,255/0,0,0,255/0/0/1/1/3.0/33");
            GameObject PlayB = Instantiate(ClonerObjs[4], myobj.transform.GetChild(5)); PlayB.SetActive(true);
            PlayB.name = (Comps.Count - 1) + ""; int wow = (Comps.Count - 1); PlayB.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { SelectSelf(wow); });
        } GameObject wow2 = Instantiate(cloner1, content1); int c = (Comps.Count - 1);
        content1.sizeDelta += new Vector2(0f, 90f); wow2.SetActive(true);
        wow2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -(c * 90f));
        if (Comps[c].StartsWith("0/")) wow2.transform.GetChild(0).GetComponent<Text>().text = "Background";
        else if (Comps[c].StartsWith("1/") && (Comps[c].EndsWith("0/0") || Comps[c].EndsWith("0/1"))) wow2.transform.GetChild(0).GetComponent<Text>().text = "Play Button";
        else if (Comps[c].StartsWith("1/") && (Comps[c].EndsWith("1/0") || Comps[c].EndsWith("1/1"))) wow2.transform.GetChild(0).GetComponent<Text>().text = "Skip Button";
        else if (Comps[c].StartsWith("1/") && (Comps[c].EndsWith("2/0") || Comps[c].EndsWith("2/1"))) wow2.transform.GetChild(0).GetComponent<Text>().text = "Track Button";
        else if (Comps[c].StartsWith("1/") && (Comps[c].EndsWith("3/0") || Comps[c].EndsWith("3/1"))) wow2.transform.GetChild(0).GetComponent<Text>().text = "Back Button";
        else if (Comps[c].StartsWith("1/") && (Comps[c].EndsWith("4/0") || Comps[c].EndsWith("4/1"))) wow2.transform.GetChild(0).GetComponent<Text>().text = "Loop Button";
        else if (Comps[c].StartsWith("2/")) wow2.transform.GetChild(0).GetComponent<Text>().text = "Playback Slider";
        else if (Comps[c].StartsWith("3/")) wow2.transform.GetChild(0).GetComponent<Text>().text = "Spectrum";
        else if (Comps[c].StartsWith("4/") && Comps[c].EndsWith("0")) wow2.transform.GetChild(0).GetComponent<Text>().text = "Scope";
        else if (Comps[c].StartsWith("4/") && Comps[c].EndsWith("1")) wow2.transform.GetChild(0).GetComponent<Text>().text = "Vectorscope";
        else if (Comps[c].StartsWith("5/")) wow2.transform.GetChild(0).GetComponent<Text>().text = "Playback Text";
        int lc = c; wow2.GetComponent<Button>().onClick.AddListener(() => { SelectSelf(lc); }); SelectSelf(c);
    }

    // Functie executata cand meniul unui element este deschis
    public void OpenMenu(int MenuN) { if(MenuN == -56) { SlectedObj = -1; myobje = null; POD.SetActive(false);
            foreach (Transform a in myobj.transform.GetChild(5)) a.GetComponent<Outline>().enabled = false; } else if(MenuN != 0) POD.SetActive(true);
        for (int m = 0; m < Menus.Count; m++) {
            if (m != MenuN) Menus[m].SetActive(false);
            else Menus[m].SetActive(true);
        }
    }

    // Functie executata cand o culoare este incarcata din selectorul de culori
    public void LoadColor() {
        Color32 mycol = new Color32((byte)PlayerPrefs.GetFloat("TEMPCL" + reqtyoe + "R"), (byte)PlayerPrefs.GetFloat("TEMPCL" + reqtyoe + "G"), 
            (byte)PlayerPrefs.GetFloat("TEMPCL" + reqtyoe + "B"), (byte)PlayerPrefs.GetFloat("TEMPCL" + reqtyoe + "A"));
        if (Comps[SlectedObj].StartsWith("0/")) {
            if(Comps[0].Split('/')[3] == "1") {
                if (reqtyoe == 0) { myobj.transform.GetChild(0).GetComponent<Image>().color = mycol; Object11MenuComps[3].GetComponent<Image>().color = mycol; Object11MenuComps[5].GetComponent<Image>().color = mycol; }
                if (reqtyoe == 1) { myobj.GetComponent<Image>().color = mycol; Object11MenuComps[4].GetComponent<Image>().color = mycol; Object11MenuComps[6].GetComponent<Image>().color = mycol; }
            } else if(Comps[0].Split('/')[3] == "2") {
                if (reqtyoe == 0) { myobj.transform.GetChild(1).GetComponent<Image>().color = mycol; Object11MenuComps[3].GetComponent<Image>().color = mycol; Object11MenuComps[5].GetComponent<Image>().color = mycol; }
                if (reqtyoe == 1) { myobj.GetComponent<Image>().color = mycol; Object11MenuComps[4].GetComponent<Image>().color = mycol; Object11MenuComps[6].GetComponent<Image>().color = mycol; }
            } else if(Comps[0].Split('/')[3] == "3") {
                if (reqtyoe == 0) { myobj.transform.GetChild(2).GetComponent<Image>().color = mycol; Object11MenuComps[7].GetComponent<Image>().color = mycol; Object11MenuComps[11].GetComponent<Image>().color = mycol; }
                if (reqtyoe == 1) { myobj.transform.GetChild(3).GetComponent<Image>().color = mycol; Object11MenuComps[8].GetComponent<Image>().color = mycol; Object11MenuComps[12].GetComponent<Image>().color = mycol; }
                if (reqtyoe == 2) { myobj.transform.GetChild(4).GetComponent<Image>().color = mycol; Object11MenuComps[9].GetComponent<Image>().color = mycol; Object11MenuComps[13].GetComponent<Image>().color = mycol; }
                if (reqtyoe == 3) { myobj.GetComponent<Image>().color = mycol; Object11MenuComps[10].GetComponent<Image>().color = mycol; Object11MenuComps[14].GetComponent<Image>().color = mycol; }
            } else if(Comps[0].Split('/')[3] == "4") {
                if (reqtyoe == 0) { myobj.GetComponent<Image>().color = mycol; Object11MenuComps[19].GetComponent<Image>().color = mycol; }
            } else if(Comps[0].Split('/')[3] == "5") {
                if (reqtyoe == 0) { myobj.GetComponent<Image>().color = mycol; myobj.transform.GetChild(2).GetComponent<Image>().color = mycol;
                    myobj.transform.GetChild(3).GetComponent<Image>().color = mycol; Object11MenuComps[23].GetComponent<Image>().color = mycol; }
                if (reqtyoe == 1) { myobj.transform.GetChild(4).GetComponent<Image>().color = mycol; Object11MenuComps[24].GetComponent<Image>().color = mycol; }
            }
        } if (Comps[SlectedObj].StartsWith("1/") && (Comps[SlectedObj].EndsWith("/0/0") || Comps[SlectedObj].EndsWith("/0/1"))) {
            if (reqtyoe == 0) { myobje.GetComponent<Image>().color = mycol; Object1MenuComps[3].GetComponent<Image>().color = mycol; }
            if (reqtyoe == 1) { myobje.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color = mycol;
                myobje.transform.GetChild(0).GetChild(1).GetComponent<RawImage>().color = mycol;
                myobje.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<RawImage>().color = mycol;
                Object1MenuComps[4].GetComponent<Image>().color = mycol;
            }
        } if (Comps[SlectedObj].StartsWith("1/") && (Comps[SlectedObj].EndsWith("/1/0") || Comps[SlectedObj].EndsWith("/1/1"))) {
            if (reqtyoe == 0) { myobje.GetComponent<Image>().color = mycol; Object2MenuComps[3].GetComponent<Image>().color = mycol; }
            if (reqtyoe == 1) { myobje.transform.GetChild(1).GetChild(0).GetComponent<RawImage>().color = mycol;
                myobje.transform.GetChild(1).GetChild(1).GetComponent<RawImage>().color = mycol;
                Object2MenuComps[4].GetComponent<Image>().color = mycol;
            }
        } if (Comps[SlectedObj].StartsWith("1/") && (Comps[SlectedObj].EndsWith("/2/0") || Comps[SlectedObj].EndsWith("/2/1"))) {
            if (reqtyoe == 0) { myobje.GetComponent<Image>().color = mycol; Object3MenuComps[3].GetComponent<Image>().color = mycol; }
            if (reqtyoe == 1) { myobje.transform.GetChild(2).GetChild(0).GetComponent<RawImage>().color = mycol;
                myobje.transform.GetChild(2).GetChild(1).GetComponent<RawImage>().color = mycol;
                Object3MenuComps[4].GetComponent<Image>().color = mycol;
            }
        } if (Comps[SlectedObj].StartsWith("1/") && (Comps[SlectedObj].EndsWith("/3/0") || Comps[SlectedObj].EndsWith("/3/1"))) {
            if (reqtyoe == 0) { myobje.GetComponent<Image>().color = mycol; Object6MenuComps[3].GetComponent<Image>().color = mycol; }
            if (reqtyoe == 1) { myobje.transform.GetChild(3).GetChild(0).GetComponent<Image>().color = mycol;
                myobje.transform.GetChild(3).GetChild(1).GetComponent<Image>().color = mycol;
                myobje.transform.GetChild(3).GetChild(2).GetComponent<Image>().color = mycol;
                Object6MenuComps[4].GetComponent<Image>().color = mycol; }
        } if (Comps[SlectedObj].StartsWith("1/") && (Comps[SlectedObj].EndsWith("/4/0") || Comps[SlectedObj].EndsWith("/4/1"))) {
            if (reqtyoe == 0) { myobje.GetComponent<Image>().color = mycol; Object7MenuComps[3].GetComponent<Image>().color = mycol; }
            if (reqtyoe == 1) { myobje.transform.GetChild(4).GetChild(0).GetComponent<Image>().color = mycol;
                Object7MenuComps[4].GetComponent<Image>().color = mycol; }
        } if (Comps[SlectedObj].StartsWith("2/")) {
            if (reqtyoe == 0) { myobje.transform.GetChild(0).GetComponent<Image>().color = mycol; myobje.transform.GetChild(2).GetChild(0).GetComponent<Image>().color = mycol; Object4MenuComps[3].GetComponent<Image>().color = mycol; }
            if (reqtyoe == 1) { myobje.transform.GetChild(2).GetChild(0).GetComponent<Outline>().effectColor = mycol; myobje.transform.GetChild(0).GetComponent<Outline>().effectColor = mycol; Object4MenuComps[4].GetComponent<Image>().color = mycol; }
            if (reqtyoe == 2) { myobje.transform.GetChild(3).GetChild(0).GetComponent<Image>().color = mycol; Object4MenuComps[5].GetComponent<Image>().color = mycol; }
        } if (Comps[SlectedObj].StartsWith("3/")) {
            if (reqtyoe == 0) { myobje.transform.GetChild(1).GetComponent<RawImage>().color = mycol; Object5MenuComps[2].GetComponent<Image>().color = mycol; }
            myobje.GetComponent<Spectrum>().Start();
        } if (Comps[SlectedObj].StartsWith("4/") && Comps[SlectedObj].EndsWith("0")) {
            if (reqtyoe == 0) { myobje.GetComponent<UILineRenderer>().color = mycol; Object8MenuComps[2].GetComponent<Image>().color = mycol; }
        } if (Comps[SlectedObj].StartsWith("4/") && Comps[SlectedObj].EndsWith("1")) {
            if (reqtyoe == 0) { myobje.GetComponent<UILineRenderer>().color = mycol; Object9MenuComps[2].GetComponent<Image>().color = mycol; }
        } if (Comps[SlectedObj].StartsWith("5/")) {
            if (reqtyoe == 0) { myobje.GetComponent<PlaybackText>().actualtext.color = mycol; Object10MenuComps[2].GetComponent<Image>().color = mycol; }
            if (reqtyoe == 1) { myobje.GetComponent<Image>().color = mycol; Object10MenuComps[3].GetComponent<Image>().color = mycol; }
        } OnComponentModdify();
    }

    // Functie executata cand o culoare trebuie schimbata
    public void RequestColor(int type) { reqtyoe = type;
        CoPi.RequestChangeColor("TEMPCL" + type, gameObject);
    }

    // Functie executata cand o proprietate unui element este schimbata
    public void OnComponentModdify() { if(!LOADMODE && inrs) { Persetss.value = 3; int xpos = 0, ypos = 0;
        if (SlectedObj != 0) { xpos = (int)myobje.GetComponent<RectTransform>().anchoredPosition.x;
        ypos = (int)myobje.GetComponent<RectTransform>().anchoredPosition.y;
            OWO[0].text = xpos + ""; OWO[1].text = ypos + ""; OWO[2].text = myobje.transform.GetSiblingIndex() + ""; } else { 
        OWO[0].text = "-"; OWO[1].text = "-"; OWO[2].text = "-"; } 
        if (Comps[SlectedObj].StartsWith("0/")) { OWO[0].interactable = false;
            OWO[1].interactable = false; OWO[2].interactable = false;
            float xsiz = myobj.GetComponent<RectTransform>().rect.width;
            float ysiz = myobj.GetComponent<RectTransform>().rect.height;
            int mode = Object11MenuComps[0].GetComponent<Dropdown>().value;
            if (mode == 1) { mode += Object11MenuComps[1].GetComponent<Dropdown>().value; }
            else if (mode == 2) { mode = 4; } 
            else if (mode == 3) { mode = 5; } 
            Color32 backcolor = Color.white; Color32 icocolor = Color.white; Color32 icocolor3 = Color.white; Color32 icocolor4 = Color.white;
            if (mode == 0) { backcolor = myobj.GetComponent<Image>().color; }
            else if (mode == 1) { backcolor = myobj.transform.GetChild(0).GetComponent<Image>().color;
            icocolor = myobj.GetComponent<Image>().color; }
            else if (mode == 2) { backcolor = myobj.transform.GetChild(1).GetComponent<Image>().color;
            icocolor = myobj.GetComponent<Image>().color; }
            else if (mode == 3) { backcolor = myobj.transform.GetChild(2).GetComponent<Image>().color;
            icocolor = myobj.transform.GetChild(3).GetComponent<Image>().color;
            icocolor3 = myobj.transform.GetChild(4).GetComponent<Image>().color;
            icocolor4 = myobj.GetComponent<Image>().color; }
            else if (mode == 4) { backcolor = myobj.GetComponent<Image>().color; if(Object11MenuComps[18].GetComponent<Dropdown>().value == 0) myobj.GetComponent<Image>().sprite = null;
                else myobj.GetComponent<Image>().sprite = Object11MenuComps[20].GetComponent<Image>().sprite;
                if (Object11MenuComps[18].GetComponent<Dropdown>().value == 0) { Object11MenuComps[21].GetComponent<Button>().interactable = false;
                        Object11MenuComps[21].transform.GetChild(0).GetComponent<Text>().text = "Select Mode"; } else
                    { Object11MenuComps[21].GetComponent<Button>().interactable = true; Object11MenuComps[21].GetComponent<Button>().onClick.RemoveAllListeners(); }
                if (Object11MenuComps[18].GetComponent<Dropdown>().value == 1) { Object11MenuComps[21].GetComponent<Button>().onClick.AddListener(() => { ImPk.InitSelection(gameObject, "SPECIAL", 1); });
                    Object11MenuComps[21].transform.GetChild(0).GetComponent<Text>().text = "Load Image"; }
                else if (Object11MenuComps[18].GetComponent<Dropdown>().value == 2) { Object11MenuComps[21].GetComponent<Button>().onClick.AddListener(() => { ImPk.InitSelection(gameObject, "SPECIAL", 2); });
                    Object11MenuComps[21].transform.GetChild(0).GetComponent<Text>().text = "Load URL"; } }
            else if (mode == 5) { backcolor = myobj.GetComponent<Image>().color; icocolor = myobj.transform.GetChild(4).GetComponent<Image>().color; }
                Comps[SlectedObj] = "0/" + xsiz + '/' + ysiz + '/' + mode + '/' + backcolor.r + ',' + backcolor.g + ',' + backcolor.b + '/'
                + icocolor.r + ',' + icocolor.g + ',' + icocolor.b + '/' + icocolor3.r + ',' + icocolor3.g + ',' + icocolor3.b + '/'
                + icocolor4.r + ',' + icocolor4.g + ',' + icocolor4.b + '/' + activetexpath;
            if(mode == 1 || mode == 2) { Object11MenuComps[15].SetActive(true); Object11MenuComps[16].SetActive(false); }
            else if (mode == 3) { Object11MenuComps[16].SetActive(true); Object11MenuComps[15].SetActive(false); }
            else if (mode == 0) { Object11MenuComps[15].SetActive(false); Object11MenuComps[16].SetActive(false); }
            string[] attr = Comps[SlectedObj].Split('/'); string[] attr2 = attr[4].Split(','); string[] attr3 = attr[5].Split(','); string[] attr4 = attr[6].Split(','); string[] attr5 = attr[7].Split(',');
            Color32 c1 = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), 255);
            Color32 c2 = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), 255);
            Color32 c3 = new Color32(byte.Parse(attr4[0]), byte.Parse(attr4[1]), byte.Parse(attr4[2]), 255);
            Color32 c4 = new Color32(byte.Parse(attr5[0]), byte.Parse(attr5[1]), byte.Parse(attr5[2]), 255);
                if (mode == 1 || mode == 2 || mode == 3) { Object11MenuComps[2].SetActive(true); Object11MenuComps[17].SetActive(false); Object11MenuComps[22].SetActive(false); } else if(mode == 4)
                { Object11MenuComps[2].SetActive(false); Object11MenuComps[17].SetActive(true); Object11MenuComps[22].SetActive(false); } else if (mode == 5)
                { Object11MenuComps[2].SetActive(false); Object11MenuComps[17].SetActive(false); Object11MenuComps[22].SetActive(true); }
                else { Object11MenuComps[2].SetActive(false); Object11MenuComps[17].SetActive(false); Object11MenuComps[22].SetActive(false); }
                view2.sizeDelta = new Vector2(int.Parse(attr[1]), int.Parse(attr[2]));
            if (mode == 0) { myobj.GetComponent<Image>().color = c1;
                myobj.transform.GetChild(0).gameObject.SetActive(false); myobj.transform.GetChild(1).gameObject.SetActive(false);
                myobj.transform.GetChild(2).gameObject.SetActive(false); myobj.transform.GetChild(3).gameObject.SetActive(false);
                myobj.transform.GetChild(4).gameObject.SetActive(false); }
            else if (mode == 1) { myobj.GetComponent<Image>().color = c2;
                myobj.transform.GetChild(0).GetComponent<Image>().color = c1;
                myobj.transform.GetChild(0).gameObject.SetActive(true); myobj.transform.GetChild(1).gameObject.SetActive(false);
                myobj.transform.GetChild(2).gameObject.SetActive(false); myobj.transform.GetChild(3).gameObject.SetActive(false);
                myobj.transform.GetChild(4).gameObject.SetActive(false); Object11MenuComps[3].GetComponent<Image>().color = c1; Object11MenuComps[4].GetComponent<Image>().color = c2;
                Object11MenuComps[5].GetComponent<Image>().color = c1; Object11MenuComps[6].GetComponent<Image>().color = c2;
                } else if (mode == 2) { myobj.GetComponent<Image>().color = c2;
                myobj.transform.GetChild(1).GetComponent<Image>().color = c1;
                myobj.transform.GetChild(0).gameObject.SetActive(false); myobj.transform.GetChild(1).gameObject.SetActive(true);
                myobj.transform.GetChild(2).gameObject.SetActive(false); myobj.transform.GetChild(3).gameObject.SetActive(false);
                myobj.transform.GetChild(4).gameObject.SetActive(false); Object11MenuComps[3].GetComponent<Image>().color = c1; Object11MenuComps[4].GetComponent<Image>().color = c2;
                Object11MenuComps[5].GetComponent<Image>().color = c1; Object11MenuComps[6].GetComponent<Image>().color = c2;
                } else if (mode == 3) { myobj.GetComponent<Image>().color = c4;
                myobj.transform.GetChild(2).GetComponent<Image>().color = c1;
                myobj.transform.GetChild(3).GetComponent<Image>().color = c2;
                myobj.transform.GetChild(4).GetComponent<Image>().color = c3;
                myobj.transform.GetChild(0).gameObject.SetActive(false); myobj.transform.GetChild(1).gameObject.SetActive(false);
                myobj.transform.GetChild(2).gameObject.SetActive(true); myobj.transform.GetChild(3).gameObject.SetActive(true);
                myobj.transform.GetChild(4).gameObject.SetActive(true); Object11MenuComps[7].GetComponent<Image>().color = c1; Object11MenuComps[8].GetComponent<Image>().color = c2;
                Object11MenuComps[9].GetComponent<Image>().color = c3; Object11MenuComps[10].GetComponent<Image>().color = c4;
                Object11MenuComps[11].GetComponent<Image>().color = c1; Object11MenuComps[12].GetComponent<Image>().color = c2;
                Object11MenuComps[13].GetComponent<Image>().color = c3; Object11MenuComps[14].GetComponent<Image>().color = c4;
                } else if (mode == 4) { myobj.GetComponent<Image>().color = c1;
                myobj.transform.GetChild(0).gameObject.SetActive(false); myobj.transform.GetChild(1).gameObject.SetActive(false);
                myobj.transform.GetChild(2).gameObject.SetActive(false); myobj.transform.GetChild(3).gameObject.SetActive(false);
                myobj.transform.GetChild(4).gameObject.SetActive(false); Object11MenuComps[7].GetComponent<Image>().color = c1;
                } else if (mode == 5) { myobj.GetComponent<Image>().color = c1;
                myobj.transform.GetChild(2).GetComponent<Image>().color = c1;
                myobj.transform.GetChild(3).GetComponent<Image>().color = c1;
                myobj.transform.GetChild(4).GetComponent<Image>().color = c2;
                myobj.transform.GetChild(0).gameObject.SetActive(false); myobj.transform.GetChild(1).gameObject.SetActive(false);
                myobj.transform.GetChild(2).gameObject.SetActive(true); myobj.transform.GetChild(3).gameObject.SetActive(true);
                myobj.transform.GetChild(4).gameObject.SetActive(true); Object11MenuComps[23].GetComponent<Image>().color = c1;
                Object11MenuComps[24].GetComponent<Image>().color = c2;
                } if (attr[3] == "1" || attr[3] == "2") { Object11MenuComps[15].SetActive(true); Object11MenuComps[16].SetActive(false); }
            else if (attr[3] == "3") { Object11MenuComps[16].SetActive(true); Object11MenuComps[15].SetActive(false); }
            else if (attr[3] == "0") { Object11MenuComps[15].SetActive(false); Object11MenuComps[16].SetActive(false); }
            OWO[0].interactable = false; OWO[1].interactable = false; OWO[2].interactable = false;
        } else { OWO[0].interactable = true; OWO[1].interactable = true; OWO[2].interactable = true;
        } if (Comps[SlectedObj].StartsWith("1/") && (Comps[SlectedObj].EndsWith("/0/0") || Comps[SlectedObj].EndsWith("/0/1"))) {
            float xsiz = float.Parse(Object1MenuComps[0].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            float ysiz = float.Parse(Object1MenuComps[1].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            float lsiz = float.Parse(Object1MenuComps[2].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            Color32 backcolor = myobje.GetComponent<Image>().color;
            Color32 icocolor = myobje.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().color;
            myobje.transform.localScale = new Vector2(xsiz, ysiz); myobje.transform.GetChild(0).localScale = new Vector2(lsiz, lsiz);
            Comps[SlectedObj] = "1/" + xpos + '/' + ypos + '/' + xsiz + '/' + ysiz + '/' + backcolor.r + ',' + backcolor.g + ',' + backcolor.b + ',' + backcolor.a + '/'
               + icocolor.r + ',' + icocolor.g + ',' + icocolor.b + ',' + icocolor.a + '/' + lsiz + "/0/0";
        } if(Comps[SlectedObj].StartsWith("1/") && (Comps[SlectedObj].EndsWith("/1/0") || Comps[SlectedObj].EndsWith("/1/1"))) {
            float xsiz = float.Parse(Object2MenuComps[0].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            float ysiz = float.Parse(Object2MenuComps[1].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            float lsiz = float.Parse(Object2MenuComps[2].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            Color32 backcolor = myobje.GetComponent<Image>().color;
            Color32 icocolor = myobje.transform.GetChild(1).GetChild(0).GetComponent<RawImage>().color;
            int hasrev; if (Object2MenuComps[5].GetComponent<Toggle>().isOn) hasrev = 1; else hasrev = 0;
            myobje.transform.localScale = new Vector2(xsiz, ysiz); myobje.transform.GetChild(1).localScale = new Vector2(lsiz, lsiz);
            if (hasrev == 1) myobje.transform.GetChild(1).rotation = Quaternion.Euler(0f, 0f, 180f); else myobje.transform.GetChild(1).rotation = Quaternion.Euler(0f, 0f, 0f);
            Comps[SlectedObj] = "1/" + xpos + '/' + ypos + '/' + xsiz + '/' + ysiz + '/' + backcolor.r + ',' + backcolor.g + ',' + backcolor.b + ',' + backcolor.a + '/'
               + icocolor.r + ',' + icocolor.g + ',' + icocolor.b + ',' + icocolor.a + '/' + lsiz + "/1/" + hasrev; ;
        } if(Comps[SlectedObj].StartsWith("1/") && (Comps[SlectedObj].EndsWith("/2/0") || Comps[SlectedObj].EndsWith("/2/1"))) {
            float xsiz = float.Parse(Object3MenuComps[0].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            float ysiz = float.Parse(Object3MenuComps[1].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            float lsiz = float.Parse(Object3MenuComps[2].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            Color32 backcolor = myobje.GetComponent<Image>().color;
            Color32 icocolor = myobje.transform.GetChild(2).GetChild(0).GetComponent<RawImage>().color;
            int hasrev; if (Object3MenuComps[5].GetComponent<Toggle>().isOn) hasrev = 1; else hasrev = 0;
            myobje.transform.localScale = new Vector2(xsiz, ysiz); myobje.transform.GetChild(2).localScale = new Vector2(lsiz, lsiz);
            if (hasrev == 1) myobje.transform.GetChild(2).rotation = Quaternion.Euler(0f, 0f, 180f); else myobje.transform.GetChild(2).rotation = Quaternion.Euler(0f, 0f, 0f);
            Comps[SlectedObj] = "1/" + xpos + '/' + ypos + '/' + xsiz + '/' + ysiz + '/' + backcolor.r + ',' + backcolor.g + ',' + backcolor.b + ',' + backcolor.a + '/'
               + icocolor.r + ',' + icocolor.g + ',' + icocolor.b + ',' + icocolor.a + '/' + lsiz + "/2/" + hasrev;
        } if(Comps[SlectedObj].StartsWith("1/") && (Comps[SlectedObj].EndsWith("/3/0") || Comps[SlectedObj].EndsWith("/3/1"))) {
            float xsiz = float.Parse(Object6MenuComps[0].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            float ysiz = float.Parse(Object6MenuComps[1].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            float lsiz = float.Parse(Object6MenuComps[2].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            Color32 backcolor = myobje.GetComponent<Image>().color;
            Color32 icocolor = myobje.transform.GetChild(3).GetChild(0).GetComponent<Image>().color;
            myobje.transform.localScale = new Vector2(xsiz, ysiz); myobje.transform.GetChild(3).localScale = new Vector2(lsiz, lsiz);
            Comps[SlectedObj] = "1/" + xpos + '/' + ypos + '/' + xsiz + '/' + ysiz + '/' + backcolor.r + ',' + backcolor.g + ',' + backcolor.b + ',' + backcolor.a + '/'
               + icocolor.r + ',' + icocolor.g + ',' + icocolor.b + ',' + icocolor.a + '/' + lsiz + "/3/0";
        } if(Comps[SlectedObj].StartsWith("1/") && (Comps[SlectedObj].EndsWith("/4/0") || Comps[SlectedObj].EndsWith("/4/1"))) {
            int nameID = int.Parse(myobje.name); 
            float xsiz = float.Parse(Object7MenuComps[0].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            float ysiz = float.Parse(Object7MenuComps[1].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            float lsiz = float.Parse(Object7MenuComps[2].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            Color32 backcolor = myobje.GetComponent<Image>().color;
            Color32 icocolor = myobje.transform.GetChild(4).GetChild(0).GetComponent<Image>().color;
            myobje.transform.localScale = new Vector2(xsiz, ysiz); myobje.transform.GetChild(4).localScale = new Vector2(lsiz, lsiz);
            Comps[SlectedObj] = "1/" + xpos + '/' + ypos + '/' + xsiz + '/' + ysiz + '/' + backcolor.r + ',' + backcolor.g + ',' + backcolor.b + ',' + backcolor.a + '/'
               + icocolor.r + ',' + icocolor.g + ',' + icocolor.b + ',' + icocolor.a + '/' + lsiz + "/4/0";
        } if(Comps[SlectedObj].StartsWith("2/")) {
            int xsiz = int.Parse(Object4MenuComps[0].GetComponent<InputField>().text);
            int ysiz = int.Parse(Object4MenuComps[1].GetComponent<InputField>().text);
            int hsiz = int.Parse(Object4MenuComps[2].GetComponent<InputField>().text);
            Color32 backcolor = myobje.transform.GetChild(0).GetComponent<Image>().color;
            Color32 icocolor = myobje.transform.GetChild(0).GetComponent<Outline>().effectColor;
            Color32 fillcolor = myobje.transform.GetChild(1).GetChild(0).GetComponent<Image>().color;
            myobje.GetComponent<RectTransform>().sizeDelta = new Vector2(xsiz, ysiz);
            myobje.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(hsiz, myobje.transform.GetChild(2).GetChild(0).GetComponent<RectTransform>().sizeDelta.y);
            Comps[SlectedObj] = "2/" + xpos + '/' + ypos + '/' + xsiz + '/' + ysiz + '/' + backcolor.r + ',' + backcolor.g + ',' + backcolor.b + ',' + backcolor.a + '/'
               + icocolor.r + ',' + icocolor.g + ',' + icocolor.b + ',' + icocolor.a + '/' + hsiz + '/' + fillcolor.r + ',' + fillcolor.g + ',' + fillcolor.b + ',' + fillcolor.a;
        } if(Comps[SlectedObj].StartsWith("3/")) {
            int xsiz = int.Parse(Object5MenuComps[0].GetComponent<InputField>().text);
            int ysiz = int.Parse(Object5MenuComps[1].GetComponent<InputField>().text);
            Color32 backcolor = myobje.transform.GetChild(1).GetComponent<RawImage>().color;
            int Segments = Mathf.ClosestPowerOfTwo(int.Parse(Object5MenuComps[3].GetComponent<InputField>().text));
            int Refresh = int.Parse(Object5MenuComps[4].GetComponent<InputField>().text);
            float mult = float.Parse(Object5MenuComps[5].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            int gaps = 0; if (Object5MenuComps[6].GetComponent<Toggle>().isOn) gaps = 1; else gaps = 0;
            int gapsize = int.Parse(Object5MenuComps[7].GetComponent<InputField>().text);
            int bottom = 0; if (Object5MenuComps[8].GetComponent<Toggle>().isOn) bottom = 1; else bottom = 0;
            int smooth = 0; if (Object5MenuComps[9].GetComponent<Toggle>().isOn) smooth = 1; else smooth = 0;
            myobje.GetComponent<RectTransform>().sizeDelta = new Vector2(xsiz, ysiz);
            myobje.GetComponent<Spectrum>().Gapped = Object5MenuComps[6].GetComponent<Toggle>().isOn;
            myobje.GetComponent<Spectrum>().Bottomed = Object5MenuComps[8].GetComponent<Toggle>().isOn;
            myobje.GetComponent<Spectrum>().Smoothing = Object5MenuComps[9].GetComponent<Toggle>().isOn;
            myobje.GetComponent<Spectrum>().GapSize = gapsize; myobje.GetComponent<Spectrum>().Multiplier = mult;
            myobje.GetComponent<Spectrum>().Segments = Segments; myobje.GetComponent<Spectrum>().RefreshRate = Refresh;
            myobje.GetComponent<Spectrum>().Start();
                Comps[SlectedObj] = "3/" + xpos + '/' + ypos + '/' + xsiz + '/' + ysiz + '/' + backcolor.r + ',' + backcolor.g + ',' + backcolor.b + ',' + backcolor.a + '/'
                        + Segments + '/' + Refresh + '/' + mult + '/' + gaps + '/' + gapsize + '/' + bottom + '/' + smooth;
        } if(Comps[SlectedObj].StartsWith("4/") && Comps[SlectedObj].EndsWith("0")) {
            int nameID = int.Parse(myobje.name);
            float xsiz = float.Parse(Object8MenuComps[0].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            float ysiz = float.Parse(Object8MenuComps[1].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            int RefRate = int.Parse(Object8MenuComps[5].GetComponent<InputField>().text);
            int Samples = int.Parse(Object8MenuComps[4].GetComponent<InputField>().text);
            float lnsiz = float.Parse(Object8MenuComps[3].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            Color32 backcolor = myobje.GetComponent<UILineRenderer>().color;
            myobje.GetComponent<Osciloscope>().RefRate = RefRate; myobje.GetComponent<Osciloscope>().Samples = Samples;
            myobje.GetComponent<Osciloscope>().LNTHICK = lnsiz; myobje.GetComponent<Osciloscope>().ChangeLay();
            myobje.GetComponent<RectTransform>().sizeDelta = new Vector2(xsiz, ysiz);
            Comps[SlectedObj] = "4/" + xpos + '/' + ypos + '/' + xsiz + '/' + ysiz + '/' + backcolor.r + ',' + 
                    backcolor.g + ',' + backcolor.b + ',' + backcolor.a + '/' + Samples + '/' + RefRate + '/' + lnsiz + "/0";
        } if(Comps[SlectedObj].StartsWith("4/") && Comps[SlectedObj].EndsWith("1")) {
            int nameID = int.Parse(myobje.name);
            float xsiz = float.Parse(Object9MenuComps[0].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            float ysiz = float.Parse(Object9MenuComps[1].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            int RefRate = int.Parse(Object9MenuComps[5].GetComponent<InputField>().text);
            int Samples = int.Parse(Object9MenuComps[4].GetComponent<InputField>().text);
            float lnsiz = float.Parse(Object9MenuComps[3].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            myobje.GetComponent<Osciloscope>().Vectorscope = true;
            myobje.GetComponent<Osciloscope>().RefRate = RefRate; myobje.GetComponent<Osciloscope>().Samples = Samples;
            myobje.GetComponent<Osciloscope>().LNTHICK = lnsiz; myobje.GetComponent<Osciloscope>().ChangeLay();
            Color32 backcolor = myobje.GetComponent<UILineRenderer>().color; 
            myobje.GetComponent<RectTransform>().sizeDelta = new Vector2(xsiz, ysiz);
            Comps[SlectedObj] = "4/" + xpos + '/' + ypos + '/' + xsiz + '/' + ysiz + '/' + backcolor.r + ',' + 
                    backcolor.g + ',' + backcolor.b + ',' + backcolor.a + '/' + Samples + '/' + RefRate + '/' + lnsiz + "/1";
        } if(Comps[SlectedObj].StartsWith("5/")) {
            int nameID = int.Parse(myobje.name);
            float xsiz = float.Parse(Object10MenuComps[0].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            float ysiz = float.Parse(Object10MenuComps[1].GetComponent<InputField>().text, CultureInfo.InvariantCulture); int font = 0;
            if (Object10MenuComps[10].GetComponent<Dropdown>().value == 0) { Object10MenuComps[12].SetActive(true); Object10MenuComps[13].SetActive(false); Object10MenuComps[14].SetActive(false); }
            else if (Object10MenuComps[10].GetComponent<Dropdown>().value == 1) { Object10MenuComps[12].SetActive(false); Object10MenuComps[13].SetActive(true); Object10MenuComps[14].SetActive(false); }
            else { Object10MenuComps[12].SetActive(false); Object10MenuComps[13].SetActive(false); Object10MenuComps[14].SetActive(true); }
            if (Object10MenuComps[10].GetComponent<Dropdown>().value == 0)
            font = Object10MenuComps[4].GetComponent<Dropdown>().value;
            if (Object10MenuComps[10].GetComponent<Dropdown>().value == 1)
            font = Object10MenuComps[11].GetComponent<Dropdown>().value + 5;
            if (Object10MenuComps[10].GetComponent<Dropdown>().value == 2) font = 13;
            int anim = Object10MenuComps[5].GetComponent<Dropdown>().value;
            bool Back = Object10MenuComps[6].GetComponent<Toggle>().isOn;
            bool EDBL = Object10MenuComps[7].GetComponent<Toggle>().isOn;
            float FSP = float.Parse(Object10MenuComps[8].GetComponent<InputField>().text, CultureInfo.InvariantCulture);
            int FS = int.Parse(Object10MenuComps[9].GetComponent<InputField>().text);
            Color32 textcolor = myobje.GetComponent<PlaybackText>().actualtext.color; 
            Color32 backcolor = myobje.GetComponent<Image>().color; 
            myobje.GetComponent<RectTransform>().sizeDelta = new Vector2(xsiz, ysiz);
            myobje.GetComponent<PlaybackText>().AnimMode = anim; myobje.GetComponent<PlaybackText>().Background = Back;
            myobje.GetComponent<PlaybackText>().EdgeBlur = EDBL; myobje.GetComponent<PlaybackText>().font = font;
            myobje.GetComponent<PlaybackText>().FontSize = FS; myobje.GetComponent<PlaybackText>().AnimSpeed = FSP;
            myobje.GetComponent<PlaybackText>().LayChange(); int Backc; int Edblc; 
            if (Back) Backc = 1; else Backc = 0; if (EDBL) Edblc = 1; else Edblc = 0;
            Comps[SlectedObj] = "5/" + xpos + '/' + ypos + '/' + xsiz + '/' + ysiz + '/' + textcolor.r + ',' + 
                    textcolor.g + ',' + textcolor.b + ',' + textcolor.a + '/' + backcolor.r + ',' +
                    backcolor.g + ',' + backcolor.b + ',' + backcolor.a + '/' + font + '/' + anim + '/' + Backc + '/' + Edblc + '/' + FSP + '/' + FS;
        } }
    }

    // Functie executata cand un element este selectat
    public void SelectSelf(int idt) { string[] attr = Comps[idt].Split('/'); LOADMODE = true; Persetss.value = 3;
        if (Comps[idt].StartsWith("0/")) { OpenMenu(11);
            Object11MenuComps[0].GetComponent<Dropdown>().value = int.Parse(attr[3]); if(attr[3] == "1" || attr[3] == "2" || attr[3] == "3") { 
                Object11MenuComps[1].GetComponent<Dropdown>().value = int.Parse(attr[3]) - 1; Object11MenuComps[0].GetComponent<Dropdown>().value = 1;
                Object11MenuComps[2].SetActive(true); } else Object11MenuComps[2].SetActive(false);
            if (attr[3] == "4") { Object11MenuComps[17].SetActive(true); } else Object11MenuComps[17].SetActive(false);
            if (attr[3] == "5") { Object11MenuComps[22].SetActive(true); } else Object11MenuComps[22].SetActive(false);
            string[] attr2 = attr[4].Split(','); string[] attr3 = attr[5].Split(','); string[] attr4 = attr[6].Split(','); string[] attr5 = attr[7].Split(',');
            Color32 c1 = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), 255);
            Color32 c2 = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), 255);
            Color32 c3 = new Color32(byte.Parse(attr4[0]), byte.Parse(attr4[1]), byte.Parse(attr4[2]), 255);
            Color32 c4 = new Color32(byte.Parse(attr5[0]), byte.Parse(attr5[1]), byte.Parse(attr5[2]), 255);
            if (attr[3] == "1" || attr[3] == "2") { Object11MenuComps[3].GetComponent<Image>().color = c1; Object11MenuComps[4].GetComponent<Image>().color = c2;
                Object11MenuComps[5].GetComponent<Image>().color = c1; Object11MenuComps[6].GetComponent<Image>().color = c2; }
            else if (attr[3] == "3") { Object11MenuComps[7].GetComponent<Image>().color = c1; Object11MenuComps[8].GetComponent<Image>().color = c2;
                Object11MenuComps[9].GetComponent<Image>().color = c3; Object11MenuComps[10].GetComponent<Image>().color = c4;
                Object11MenuComps[11].GetComponent<Image>().color = c1; Object11MenuComps[12].GetComponent<Image>().color = c2;
                Object11MenuComps[13].GetComponent<Image>().color = c3; Object11MenuComps[14].GetComponent<Image>().color = c4; }
            else if (attr[3] == "4") { Object11MenuComps[19].GetComponent<Image>().color = c1; }
            if (attr[3] == "1" || attr[3] == "2") { Object11MenuComps[15].SetActive(true); Object11MenuComps[16].SetActive(false); }
            else if (attr[3] == "3") { Object11MenuComps[16].SetActive(true); Object11MenuComps[15].SetActive(false); }
            else if (attr[3] == "0") { Object11MenuComps[15].SetActive(false); Object11MenuComps[16].SetActive(false); } }
        if (Comps[idt].StartsWith("1/") && (Comps[idt].EndsWith("/0/0") || Comps[idt].EndsWith("/0/1"))) { OpenMenu(1);
            Object1MenuComps[0].GetComponent<InputField>().text = attr[3]; Object1MenuComps[1].GetComponent<InputField>().text = attr[4];
            Object1MenuComps[2].GetComponent<InputField>().text = attr[7]; }
        if (Comps[idt].StartsWith("1/") && (Comps[idt].EndsWith("/1/0") || Comps[idt].EndsWith("/1/1"))) { OpenMenu(2); 
            Object2MenuComps[0].GetComponent<InputField>().text = attr[3]; Object2MenuComps[1].GetComponent<InputField>().text = attr[4];
            Object2MenuComps[2].GetComponent<InputField>().text = attr[7]; if(attr[9] == "1") Object2MenuComps[5].GetComponent<Toggle>().isOn = true; else Object2MenuComps[5].GetComponent<Toggle>().isOn = false; }
        if (Comps[idt].StartsWith("1/") && (Comps[idt].EndsWith("/2/0") || Comps[idt].EndsWith("/2/1"))) { OpenMenu(3); 
            Object3MenuComps[0].GetComponent<InputField>().text = attr[3]; Object3MenuComps[1].GetComponent<InputField>().text = attr[4];
            Object3MenuComps[2].GetComponent<InputField>().text = attr[7]; if(attr[9] == "1") Object3MenuComps[5].GetComponent<Toggle>().isOn = true; else Object3MenuComps[5].GetComponent<Toggle>().isOn = false; }
        if (Comps[idt].StartsWith("1/") && (Comps[idt].EndsWith("/3/0") || Comps[idt].EndsWith("/3/1"))) { OpenMenu(6); 
            Object6MenuComps[0].GetComponent<InputField>().text = attr[3]; Object6MenuComps[1].GetComponent<InputField>().text = attr[4];
            Object6MenuComps[2].GetComponent<InputField>().text = attr[7]; }
        if (Comps[idt].StartsWith("1/") && (Comps[idt].EndsWith("/4/0") || Comps[idt].EndsWith("/4/1"))) { OpenMenu(7); 
            Object7MenuComps[0].GetComponent<InputField>().text = attr[3]; Object7MenuComps[1].GetComponent<InputField>().text = attr[4];
            Object7MenuComps[2].GetComponent<InputField>().text = attr[7]; }
        if (Comps[idt].StartsWith("1/")) { string[] attr2 = attr[5].Split(','); string[] attr3 = attr[6].Split(',');
            if (attr[8] == "0") { Object1MenuComps[3].GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                Object1MenuComps[4].GetComponent<Image>().color = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), byte.Parse(attr3[3])); }
                if (attr[8] == "1") { Object2MenuComps[3].GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                Object2MenuComps[4].GetComponent<Image>().color = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), byte.Parse(attr3[3])); }
                if (attr[8] == "2") { Object3MenuComps[3].GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                Object3MenuComps[4].GetComponent<Image>().color = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), byte.Parse(attr3[3])); }
                if (attr[8] == "3") { Object6MenuComps[3].GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                Object6MenuComps[4].GetComponent<Image>().color = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), byte.Parse(attr3[3])); }
                if (attr[8] == "4") { Object7MenuComps[3].GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
                Object7MenuComps[4].GetComponent<Image>().color = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), byte.Parse(attr3[3])); } }
        if (Comps[idt].StartsWith("2/")) { OpenMenu(4); 
            Object4MenuComps[0].GetComponent<InputField>().text = attr[3]; Object4MenuComps[1].GetComponent<InputField>().text = attr[4];
            Object4MenuComps[2].GetComponent<InputField>().text = attr[7];
            string[] attr2 = attr[5].Split(','); string[] attr3 = attr[6].Split(','); string[] attr4 = attr[8].Split(',');
            Object4MenuComps[3].GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
            Object4MenuComps[4].GetComponent<Image>().color = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), byte.Parse(attr3[3]));
            Object4MenuComps[5].GetComponent<Image>().color = new Color32(byte.Parse(attr4[0]), byte.Parse(attr4[1]), byte.Parse(attr4[2]), byte.Parse(attr4[3])); }
        if (Comps[idt].StartsWith("3/")) { OpenMenu(5);
            Object5MenuComps[0].GetComponent<InputField>().text = attr[3]; Object5MenuComps[1].GetComponent<InputField>().text = attr[4];
            Object5MenuComps[3].GetComponent<InputField>().text = attr[6]; Object5MenuComps[4].GetComponent<InputField>().text = attr[7];
            Object5MenuComps[5].GetComponent<InputField>().text = attr[8]; Object5MenuComps[7].GetComponent<InputField>().text = attr[10];
            string[] attr2 = attr[5].Split(',');
            Object5MenuComps[2].GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
            if (attr[9] == "1") Object5MenuComps[6].GetComponent<Toggle>().isOn = true; else Object5MenuComps[6].GetComponent<Toggle>().isOn = false;
            if (attr[11] == "1") Object5MenuComps[8].GetComponent<Toggle>().isOn = true; else Object5MenuComps[8].GetComponent<Toggle>().isOn = false;
            if (attr[12] == "1") Object5MenuComps[9].GetComponent<Toggle>().isOn = true; else Object5MenuComps[9].GetComponent<Toggle>().isOn = false;
        } if (Comps[idt].StartsWith("4/") && Comps[idt].EndsWith("0")) { OpenMenu(8);
            Object8MenuComps[0].GetComponent<InputField>().text = attr[3]; Object8MenuComps[1].GetComponent<InputField>().text = attr[4];
            Object8MenuComps[3].GetComponent<InputField>().text = attr[8]; Object8MenuComps[4].GetComponent<InputField>().text = attr[6];
            Object8MenuComps[5].GetComponent<InputField>().text = attr[7]; string[] attr2 = attr[5].Split(',');
            Object8MenuComps[2].GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
        } if (Comps[idt].StartsWith("4/") && Comps[idt].EndsWith("1")) { OpenMenu(9);
            Object9MenuComps[0].GetComponent<InputField>().text = attr[3]; Object9MenuComps[1].GetComponent<InputField>().text = attr[4];
            Object9MenuComps[3].GetComponent<InputField>().text = attr[8]; Object9MenuComps[4].GetComponent<InputField>().text = attr[6];
            Object9MenuComps[5].GetComponent<InputField>().text = attr[7]; string[] attr2 = attr[5].Split(',');
            Object9MenuComps[2].GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
        } if (Comps[idt].StartsWith("5/")) { OpenMenu(10);
            Object10MenuComps[0].GetComponent<InputField>().text = attr[3]; Object10MenuComps[1].GetComponent<InputField>().text = attr[4];
            if (int.Parse(attr[7]) <= 4) { Object10MenuComps[10].GetComponent<Dropdown>().value = 0; Object10MenuComps[4].GetComponent<Dropdown>().value = int.Parse(attr[7]); Object10MenuComps[12].SetActive(true); Object10MenuComps[13].SetActive(false); Object10MenuComps[14].SetActive(false); }
            else if (int.Parse(attr[7]) <= 12) { Object10MenuComps[10].GetComponent<Dropdown>().value = 1; Object10MenuComps[11].GetComponent<Dropdown>().value = int.Parse(attr[7]); Object10MenuComps[12].SetActive(false); Object10MenuComps[13].SetActive(true); Object10MenuComps[14].SetActive(false); }
            else { Object10MenuComps[10].GetComponent<Dropdown>().value = 2; Object10MenuComps[12].SetActive(false); Object10MenuComps[13].SetActive(false); Object10MenuComps[14].SetActive(true); }
            Object10MenuComps[5].GetComponent<Dropdown>().value = int.Parse(attr[8]);
            if (attr[9] == "1") Object10MenuComps[6].GetComponent<Toggle>().isOn = true; else Object10MenuComps[6].GetComponent<Toggle>().isOn = true;
            if (attr[10] == "1") Object10MenuComps[7].GetComponent<Toggle>().isOn = true; else Object10MenuComps[7].GetComponent<Toggle>().isOn = true;
            Object10MenuComps[8].GetComponent<InputField>().text = attr[11]; Object10MenuComps[9].GetComponent<InputField>().text = attr[12];
            string[] attr2 = attr[5].Split(','); string[] attr3 = attr[6].Split(',');
            Object10MenuComps[2].GetComponent<Image>().color = new Color32(byte.Parse(attr2[0]), byte.Parse(attr2[1]), byte.Parse(attr2[2]), byte.Parse(attr2[3]));
            Object10MenuComps[3].GetComponent<Image>().color = new Color32(byte.Parse(attr3[0]), byte.Parse(attr3[1]), byte.Parse(attr3[2]), byte.Parse(attr3[3]));
        } SlectedObj = idt; if(idt != 0) myobje = myobj.transform.GetChild(5).Find(SlectedObj + "").gameObject;
        foreach (Transform a in myobj.transform.GetChild(5)) a.GetComponent<Outline>().enabled = false;
        if (myobje != null) myobje.GetComponent<Outline>().enabled = true; StartCoroutine(TogWLOADMODE(idt));
    }

    // Functie executata pentru a astepta un cadru si a actualiza interfata inainte de executarea logicii
    public IEnumerator TogWLOADMODE(int a) {
        yield return null;
        LOADMODE = false; OnComponentModdify();
        content1.GetChild(a).GetComponent<Button>().Select();
    }

    // Functie executata pentru a incarca o imagine din dispozitiv
    public void OnImgLoad() {
        if (Object11MenuComps[18].GetComponent<Dropdown>().value == 1) { StartCoroutine(LDImg(activetexpath)); }
        else if (Object11MenuComps[18].GetComponent<Dropdown>().value == 2) { StartCoroutine(DownloadImage(activetexpath)); }
        string[] UlSt = Comps[0].Split('/');
        string UltraStart = UlSt[0] + '/' + UlSt[1] + '/' + UlSt[2] + '/' + UlSt[3] + '/' + UlSt[4] + '/' + UlSt[5] + '/' + UlSt[6] + '/' + UlSt[7] + '/' + activetexpath;
        Comps[0] = UltraStart;
    }

    // Functie propriu-zisa folosita pentru a incarca o imagine din dispozitiv
    public IEnumerator LDImg(string filePath) {
        if (filePath != "") { activetexpath = filePath;
            Texture2D tex = new Texture2D(2, 2); byte[] imageData = File.ReadAllBytes(filePath); tex.LoadImage(imageData);
            Sprite cranberry = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
            Object11MenuComps[20].GetComponent<Image>().sprite = cranberry; myobj.GetComponent<Image>().sprite = cranberry;
        } yield return null;
    }

    // [FUNCTIE NEFOLOSITA] Functie propriu-zisa folosita pentru a incarca o imagine printr-un URL
    IEnumerator DownloadImage(string url) {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url)) {
            yield return uwr.SendWebRequest();
            if (uwr.result == UnityWebRequest.Result.Success){
                Texture2D tex = DownloadHandlerTexture.GetContent(uwr);
                Sprite cranberry = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect);
                myobj.GetComponent<Image>().sprite = cranberry; Object11MenuComps[20].GetComponent<Image>().sprite = cranberry; activetexpath = url;
            }
        }
    }

    // Functie chemata in fiecare cadru | Functie folosita pentru a actualiza pozitia elementelor cand sunt mutate
    void Update() {
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved && Touchy) {
                myobje.GetComponent<RectTransform>().anchoredPosition += (touch.deltaPosition / view2.localScale);
                myobje.GetComponent<RectTransform>().anchoredPosition = new Vector2((int)myobje.GetComponent<RectTransform>().anchoredPosition.x, (int)myobje.GetComponent<RectTransform>().anchoredPosition.y);
            }
        } if (Input.touchCount == 2) {
            Touch t0 = Input.GetTouch(0); Touch t1 = Input.GetTouch(1);
            Vector2 t0Prev = t0.position - t0.deltaPosition; Vector2 t1Prev = t1.position - t1.deltaPosition;
            float prevDist = Vector2.Distance(t0Prev, t1Prev); float currDist = Vector2.Distance(t0.position, t1.position);
            view2.localScale *= currDist / prevDist;
        }
    }

}
