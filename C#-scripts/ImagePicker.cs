using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using System.Xml.XPath;

public class ImagePicker : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa selector de imagini
    // Folosita pentru a a selecta o imagine din dispozitiv
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    // Lista de elenete si proprietati pentru clasa
    static string[] ImageExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".webp" };
    public GameObject SelectrPanel;
    public GameObject ClonerObjIMG;
    public GameObject ClonerObjFOL;
    public Transform orgfelfiles;
    public GameObject ClonerRFOL;
    public Transform orgfelfolds;
    public InputField URLInsert;
    public RawImage PrevURL;
    public GameObject URLLoad;
    public string currentfilepath;
    public string wowimgpath;
    public Texture cranberry;
    public RectTransform can;
    public List<string> UndoLocations;
    public int UndoIndex;
    public Sprite unselected;
    public Sprite slected;
    public GameObject confirmb;
    public GameObject Snd;
    public GameObject SafeMode;
    public InputField Patah;
    public List<string> PathNames;
    public string keyr;
    public string SafePath;
    public int SafeType;
    public bool HasMusic;

    // Functie executata cand elementul este activat pentru prima data
    void Start() {
        Patah.onSubmit.AddListener((_) => { OnLocationChange(Patah.text, false); });
    }

    // Functie folosita pentru a gasi bazele bazelor de stocare a dispozitivului
    public static string[] GetStorageRoots() {
#if UNITY_ANDROID && !UNITY_EDITOR
    AndroidJavaClass versionClass = new AndroidJavaClass("android.os.Build$VERSION");
    int sdkLevel = versionClass.GetStatic<int>("SDK_INT");
    if (sdkLevel <= 32) {
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead)) {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        } List<string> roots = new List<string>();
        if (Directory.Exists("/storage/emulated/0")) { roots.Add("/storage/emulated/0"); }
        if (Directory.Exists("/storage")) {
            string[] storageDirs = Directory.GetDirectories("/storage");
            foreach (var dir in storageDirs) {
                string name = Path.GetFileName(dir);
                if (name == "emulated" || name == "self") continue;
                if (Directory.Exists(dir)) roots.Add(dir);
            }
        }
        if (roots.Count > 0) return roots.ToArray();
        return new string[] { Application.persistentDataPath };
    } else {
        return new string[] { Application.persistentDataPath };
    }

#else
        DriveInfo[] drives = DriveInfo.GetDrives();
        string[] drivePaths = new string[drives.Length];
        for (int i = 0; i < drives.Length; i++) {
            drivePaths[i] = drives[i].Name;
        }
        return drivePaths;
#endif
    }


    // Functie pentru a prelua foldere si file dintr-un directoriu
    public static string[] GetFoldersAndImages(string path) {
        if (!Directory.Exists(path)) return new string[0];
        var folders = Directory.GetDirectories(path);
        var files = Directory.GetFiles(path).Where(file => ImageExtensions.Contains(Path.GetExtension(file).ToLower())).ToArray();
        return folders.Concat(files).ToArray();
    }

    // Functie pentru a prelua DOAR foldere dintr-un directoriu
    public static string[] GetFolders(string path) {
        if (!Directory.Exists(path)) return new string[0];
        var folders = Directory.GetDirectories(path);
        return folders;
    }

    // Functie pentru a incepe sistemul de selectie pentru imagini / foldere
    public void InitSelection(GameObject sender, string key, int Mode) { Snd = sender; keyr = key;
        if(sender.GetComponent<MusicPlayer>()) HasMusic = false; else HasMusic = true;
        PathNames.Clear(); string[] MainD = GetStorageRoots();
        for (int i = 0; i < MainD.Length; i++) { if (i == 0) PathNames.Add("Storage"); else PathNames.Add("SD Card"); }
        if (MainD != null) { currentfilepath = MainD[0];
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaClass versionClass = new AndroidJavaClass("android.os.Build$VERSION");
            int sdkLevel = versionClass.GetStatic<int>("SDK_INT");
            if (sdkLevel <= 32) OnLocationChange(currentfilepath, false);
            else SafeModePick();
#else
            OnLocationChange("C:/", false);
#endif
        }
    }

    // Functie executata pentru a activa meniul Safe Mode inainte de selectie
    public void SafeModePick() {
        SelectrPanel.SetActive(false); SafeMode.SetActive(true);
    }

    // Functie pentru a incepe selectia dintr-un directoriu sigur din dispozitiv inloc de baza directoriului
    public void SelectSafe(string Selected) {
        PathNames.Clear();
        AndroidJavaClass env = new AndroidJavaClass("android.os.Environment");
        if (Selected == "Downloads") {
            string downloadsPath = env.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory",
                env.GetStatic<string>("DIRECTORY_DOWNLOADS")).Call<string>("getAbsolutePath");
            SafePath = downloadsPath; SafeType = 1; currentfilepath = SafePath;
            PathNames.Add("Downloads"); OnLocationChange(currentfilepath, false);
        }
        if (Selected == "Pictures") {
            string picturesPath = env.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory",
            env.GetStatic<string>("DIRECTORY_PICTURES")).Call<string>("getAbsolutePath");
            SafePath = picturesPath; SafeType = 2; currentfilepath = SafePath;
            PathNames.Add("Pictures"); OnLocationChange(currentfilepath, false);
        } SelectrPanel.SetActive(true); SafeMode.SetActive(false);
    }

    // Functie folosita pentru a te duce la ultimul directoriu inainte de cel curent
    public void Undo() {
        if(UndoIndex < 19) { UndoIndex++;
            if(UndoLocations[UndoIndex] != "") OnLocationChange(UndoLocations[UndoIndex], true);
            else UndoIndex--;
        }
    }

    // Functie folosita pentru a te duce la urmatorul directoriu daca ai folosit functia Undo()
    public void Redo() {
        if(UndoIndex > 0) { UndoIndex--;
            if (UndoLocations[UndoIndex] != "") OnLocationChange(UndoLocations[UndoIndex], true);
            else UndoIndex++;
        }
    }

    // Functia folosita pentru a te duce in folderul inainte de cel in care esti in momentul de fata
    public void OneUp() {
        if(currentfilepath != SafePath) { 
        string[] parts = currentfilepath.Split(Path.DirectorySeparatorChar);
        if (parts.Length > 1) { currentfilepath = "";
            for (int p = 0; p < parts.Length - 1; p++) if(currentfilepath != "") currentfilepath = currentfilepath + Path.DirectorySeparatorChar + parts[p];
                else currentfilepath = parts[p]; if(parts.Length == 2) OnLocationChange((currentfilepath + Path.DirectorySeparatorChar), false);
            else OnLocationChange(currentfilepath, false); } }
    }

    // Functie executata cand directoriul este schimbat | Aceasta functie incarca interfata pentru imaginile si folderele gasite
    public void OnLocationChange(string path, bool unDoing) { currentfilepath = path; confirmb.SetActive(false); Patah.text = path; Patah.MoveTextEnd(false);
        if (!unDoing && UndoIndex != 0) { UndoLocations = new List<string> { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" }; UndoIndex = 0; }
        if (!unDoing) for (int i = 19; i >= 0; i--) if (i == 0) UndoLocations[i] = path; else UndoLocations[i] = UndoLocations[i - 1];
        SelectrPanel.SetActive(true); orgfelfiles.GetComponent<RectTransform>().sizeDelta = new Vector2(can.rect.width - 608f, 300f);
        foreach (Transform d in orgfelfolds) Destroy(d.gameObject); string[] MainD;
        if (SafePath == "") { PathNames.Clear(); MainD = GetStorageRoots(); for (int i = 0; i < MainD.Length; i++) { if (i == 0) PathNames.Add("Storage"); else PathNames.Add("SD Card"); }
        } else { MainD = new string[] { SafePath }; }
        for (int d = 0; d < MainD.Length; d++) { int bld = d;
            GameObject NewFold = Instantiate(ClonerRFOL, orgfelfolds); NewFold.SetActive(true);
            NewFold.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f - (120f * d));
            NewFold.transform.GetChild(0).GetComponent<Text>().text = PathNames[d];
            NewFold.GetComponent<Button>().onClick.AddListener(() => OnLocationChange(MainD[bld], false));
        } orgfelfolds.GetComponent<RectTransform>().sizeDelta = new Vector2(orgfelfolds.GetComponent<RectTransform>().sizeDelta.x, 120f * MainD.Length);
        foreach (Transform d in orgfelfiles) Destroy(d.gameObject); string[] MainFiles;
        if(HasMusic) MainFiles = GetFoldersAndImages(currentfilepath); else MainFiles = GetFolders(currentfilepath);
        int MaxPerRow = (int)((orgfelfiles.GetComponent<RectTransform>().sizeDelta.x - 15f) / 250f);
        if(HasMusic) { 
        for(int f = 0; f < MainFiles.Length; f++) { int Row = f / MaxPerRow; int blf = f;
            if(ImageExtensions.Contains(Path.GetExtension(MainFiles[f]).ToLower())) {
                GameObject NewFold = Instantiate(ClonerObjIMG, orgfelfiles); NewFold.SetActive(true);
                NewFold.GetComponent<RectTransform>().anchoredPosition = new Vector2(15f + ((f % MaxPerRow) * 250f), -15f - (Row * 310f));
                NewFold.transform.GetChild(0).GetComponent<Text>().text = Path.GetFileName(MainFiles[f]);
                NewFold.GetComponent<Button>().onClick.AddListener(() => OnImgSelect(NewFold, MainFiles[blf]));
                StartCoroutine(LDImg(MainFiles[f])); NewFold.transform.GetChild(1).GetComponent<RawImage>().texture = cranberry;
            } else {
                GameObject NewFold = Instantiate(ClonerObjFOL, orgfelfiles); NewFold.SetActive(true);
                NewFold.GetComponent<RectTransform>().anchoredPosition = new Vector2(15f + ((f % MaxPerRow) * 250f), -15f - (Row * 310f));
                NewFold.transform.GetChild(0).GetComponent<Text>().text = Path.GetFileName(MainFiles[f]);
                NewFold.GetComponent<Button>().onClick.AddListener(() => OnLocationChange(Path.Combine(currentfilepath, Path.GetFileName(MainFiles[blf])), false));
            }
        } } else { 
        for(int f = 0; f < MainFiles.Length; f++) { int Row = f / MaxPerRow; int blf = f;
                GameObject NewFold = Instantiate(ClonerObjFOL, orgfelfiles); NewFold.SetActive(true);
                NewFold.GetComponent<RectTransform>().anchoredPosition = new Vector2(15f + ((f % MaxPerRow) * 250f), -15f - (Row * 310f));
                NewFold.transform.GetChild(0).GetComponent<Text>().text = Path.GetFileName(MainFiles[f]);
                NewFold.GetComponent<Button>().onClick.AddListener(() => OnFloSelect(NewFold, Path.Combine(currentfilepath, Path.GetFileName(MainFiles[blf]))));
        } } orgfelfiles.GetComponent<RectTransform>().sizeDelta = new Vector2(orgfelfiles.GetComponent<RectTransform>().sizeDelta.x, 340f + (((MainFiles.Length - 1) / MaxPerRow) * 310f));
    }

    // Functie executata cand o imagine este selectata
    public void OnImgSelect(GameObject Slected, string imgpath) {
        foreach (Transform b in orgfelfiles) if (b.GetComponent<Image>().sprite == slected) b.GetComponent<Image>().sprite = unselected;
        Slected.GetComponent<Image>().sprite = slected; wowimgpath = imgpath; confirmb.SetActive(true);
    }

    // Functie executata cand un folder este selectat
    public void OnFloSelect(GameObject Slected, string imgpath) {
        if(Slected.GetComponent<Image>().sprite == slected) {
            OnLocationChange(imgpath, false);
            confirmb.SetActive(false);
        } else { 
        foreach (Transform b in orgfelfiles) if (b.GetComponent<Image>().sprite == slected) b.GetComponent<Image>().sprite = unselected;
        Slected.GetComponent<Image>().sprite = slected; wowimgpath = imgpath; confirmb.SetActive(true); }
    }

    // Functie executata cand butonul de confirmare este apasat iar imaginea sau folderul este salvat
    public void ConfirmSelection() {
        if(keyr == "SPECIAL") {
            if (Snd.GetComponent<LayoutModd>()) {
                Snd.GetComponent<LayoutModd>().activetexpath = wowimgpath;
                Snd.GetComponent<LayoutModd>().OnImgLoad(); }
        } else { 
        PlayerPrefs.SetString(keyr, wowimgpath);
        if (Snd.GetComponent<ImageComponent>()) Snd.GetComponent<ImageComponent>().OnImgApply();
        if (Snd.GetComponent<BackgroundT>()) Snd.GetComponent<BackgroundT>().OnImgApply(); 
        if (Snd.GetComponent<MusicPlayer>()) Snd.GetComponent<MusicPlayer>().OnBrowseFinish(wowimgpath); }
        SelectrPanel.SetActive(false);
    }

    // [FUNCTIE NEFOLOSITA] Functie executata cand componenta incearca sa incarce o imagine printr-un URL
    public void SendURLLoadR() {
        StartCoroutine(DownloadImage(URLInsert.text));
    }

    // Functie propriu-zisa pentru incarcarea imaginii dintr-un directoriu al dispozitivului
    public IEnumerator LDImg(string filePath) {
        if (filePath != "") {
        byte[] imageData = File.ReadAllBytes(filePath);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(imageData); cranberry = tex; 
        yield return null;
        }
    }

    // [FUNCTIE NEFOLOSITA] Functie propriu-zisa pentru incarcarea imaginii printr-un URL
    IEnumerator DownloadImage(string url) {
        yield return null;
    }
}
