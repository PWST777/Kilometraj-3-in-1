using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectDrag : MonoBehaviour
{

    public int ElementSelected;
    public ColorPicker CoPi;
    public RectTransform ElementS;
    public GameObject AllPanels;
    public Transform AllElements;
    public TutorialHints touhou;
    public List<GameObject> PrefsElements;
    public Toggle Snappr;
    public Slider SnapprAmount;
    public InputField SnapprAmountIN;
    public Toggle GSnappr;
    public Slider GSnapprOpa;
    public InputField GSnapprAmountIN;
    public List<RawImage> Preview;
    public RectTransform LrCan;
    public Toggle Move;
    public Toggle Scale;
    public Toggle Rotate;
    public bool InputFMode;
    public int BackMode; public Dropdown bm;
    public Color SColorBack; public Image scb;
    public bool Snapp;
    public int SnappAmount;
    public bool GridSnap;
    public int GridSpace;
    public float Opacity;
    public Transform Orgfel;
    public RectTransform Hor;
    public RectTransform Ver;
    public GameObject SnappH;
    public GameObject SnappV;
    public Vector2 ActualPos;
    public bool Loading; int mpfs = 0;
    public bool Touchy;
    public bool UsingMode;
    public bool LoadMode;
    public List<RectTransform> allmens;
    public Action onSelct;
    public GameObject muswarn;
    public GameObject muswarn2;

    public void OnElemSelect(int ID) {
        ElementSelected = ID;
        if (ID != 72769 && ID != -1) { ElementS = AllElements.Find(ID + "").GetComponent<RectTransform>(); ActualPos = ElementS.GetComponent<RectTransform>().anchoredPosition; }
        else ElementS = null; 
        for (int p = 0; p < AllPanels.transform.childCount; p++)
            AllPanels.transform.GetChild(p).gameObject.SetActive(false);
        for (int e = 0; e < AllElements.childCount; e++)
            AllElements.GetChild(e).GetComponent<Outline>().enabled = false;
        if (ID == 72769) { AllPanels.transform.GetChild(0).gameObject.SetActive(true);
            AllPanels.transform.GetChild(1).gameObject.SetActive(false); }
        else if(ID == -1) { OnBackSelect(); AllPanels.transform.GetChild(0).gameObject.SetActive(true);
            AllPanels.transform.GetChild(1).gameObject.SetActive(true);
        } else ElementS.GetComponent<Outline>().enabled = true;
        if (ID != 72769 && ID != -1) if (ElementS.GetComponent<AnalogSpedometer>()) {
            ElementS.GetComponent<AnalogSpedometer>().OnSelectOfElement();
            ElementS.GetComponent<AnalogSpedometer>().EditorModPanel.SetActive(true);
        } else if (ElementS.GetComponent<AnalogDisplay>()) {
            ElementS.GetComponent<AnalogDisplay>().OnSelectOfElement();
            ElementS.GetComponent<AnalogDisplay>().EditorModPanel.SetActive(true);
        } else if (ElementS.GetComponent<LCDSpedometer>()) {
            ElementS.GetComponent<LCDSpedometer>().OnSelectOfElement();
            ElementS.GetComponent<LCDSpedometer>().EditorModPanel.SetActive(true);
        } else if (ElementS.GetComponent<LCDDisplay>()) {
            ElementS.GetComponent<LCDDisplay>().OnSelectOfElement();
            ElementS.GetComponent<LCDDisplay>().EditorModPanel.SetActive(true);
        } else if (ElementS.GetComponent<AnalogSpeedbar>()) {
            ElementS.GetComponent<AnalogSpeedbar>().OnSelectOfElement();
            ElementS.GetComponent<AnalogSpeedbar>().EditorModPanel.SetActive(true);
        } else if (ElementS.GetComponent<CircSpeedBar>()) {
            ElementS.GetComponent<CircSpeedBar>().OnSelectOfElement();
            ElementS.GetComponent<CircSpeedBar>().EditorModPanel.SetActive(true);
        } else if (ElementS.GetComponent<LCDSpeedbar>()) {
            ElementS.GetComponent<LCDSpeedbar>().OnSelectOfElement();
            ElementS.GetComponent<LCDSpeedbar>().EditorModPanel.SetActive(true);
        } else if (ElementS.GetComponent<LCDCircSpeedbar>()) {
            ElementS.GetComponent<LCDCircSpeedbar>().OnSelectOfElement();
            ElementS.GetComponent<LCDCircSpeedbar>().EditorModPanel.SetActive(true);
        } else if (ElementS.GetComponent<Line>()) {
            ElementS.GetComponent<Line>().OnSelectOfElement();
            ElementS.GetComponent<Line>().EditorModPanel.SetActive(true);
        } else if (ElementS.GetComponent<TextComp>()) {
            ElementS.GetComponent<TextComp>().OnSelectOfElement();
            ElementS.GetComponent<TextComp>().EditorModPanel.SetActive(true);
        } else if (ElementS.GetComponent<ImageComponent>()) {
            ElementS.GetComponent<ImageComponent>().OnSelectOfElement();
            ElementS.GetComponent<ImageComponent>().EditorModPanel.SetActive(true);
        } else if (ElementS.GetComponent<Compass>()) {
            ElementS.GetComponent<Compass>().OnSelectOfElement();
            ElementS.GetComponent<Compass>().EditorModPanel.SetActive(true);
        } else if (ElementS.GetComponent<MusicPlayer>()) {
            ElementS.GetComponent<MusicPlayer>().OnSelectOfElement();
            ElementS.GetComponent<MusicPlayer>().EditorModPanel.SetActive(true);
        } else if (ElementS.GetComponent<MapSystem>()) {
            ElementS.GetComponent<MapSystem>().OnSelectOfElement();
            ElementS.GetComponent<MapSystem>().EditorModPanel.SetActive(true);
        } if (ID != 72769 && ID != -1) { SnapUI(); onSelct?.Invoke(); }
    }

    public void EditorMenuMod() {
        if (!Loading) { Snapp = Snappr.isOn;
            if (!Snappr.isOn) { SnapprAmount.interactable = false; SnapprAmountIN.interactable = false; PlayerPrefs.SetInt("EDITORSNAP", 0); }
            else { SnapprAmount.interactable = true; SnapprAmountIN.interactable = true;
                if (InputFMode) { SnappAmount = int.Parse(SnapprAmountIN.text);
                SnapprAmount.value = SnappAmount; }
                else { SnappAmount = (int)SnapprAmount.value;
                SnapprAmountIN.text = SnappAmount + ""; }
                PlayerPrefs.SetInt("EDITORSNAP", SnappAmount);
            } GridSnap = GSnappr.isOn;
            if (!GSnappr.isOn) { GSnapprOpa.interactable = false; GSnapprAmountIN.interactable = false; Opacity = 0f; PlayerPrefs.SetInt("EDITORGRIDSNAP", 0); PlayerPrefs.SetFloat("EDITORGRIDOPAC", 0f); OnGridRegen(); }
            else { GSnapprOpa.interactable = true; GSnapprAmountIN.interactable = true;
                GridSpace = Mathf.Max(int.Parse(GSnapprAmountIN.text),5);
                Opacity = GSnapprOpa.value;
                PlayerPrefs.SetInt("EDITORGRIDSNAP", GridSpace);
                PlayerPrefs.SetFloat("EDITORGRIDOPAC", Opacity);
                for (int g = 0; g < 4; g++) Preview[g].color = new Color(1f, 1f, 1f, Opacity);
            }
            if(Move.isOn) PlayerPrefs.SetInt("EDITORMOVEA", 1); else PlayerPrefs.SetInt("EDITORMOVEA", 0);
            if(Rotate.isOn) PlayerPrefs.SetInt("EDITORROTA", 1); else PlayerPrefs.SetInt("EDITORROTA", 0);
            if(Scale.isOn) PlayerPrefs.SetInt("EDITORSCALA", 1); else PlayerPrefs.SetInt("EDITORSCALA", 0);
        }
    }

    public void MovePosX(InputField mpx) { if(ElementS != null) {
            ElementS.anchoredPosition = new Vector2(int.Parse(mpx.text),ElementS.anchoredPosition.y); SnapUI();
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementSelected + "BASEPOSX", ElementS.anchoredPosition.x);
        }
    }

    public void MovePosY(InputField mpy) { if(ElementS != null) {
            ElementS.anchoredPosition = new Vector2(ElementS.anchoredPosition.x, int.Parse(mpy.text));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementSelected + "BASEPOSY", ElementS.anchoredPosition.y);
        }
    }

    public void Rotation(InputField rot) { if(ElementS != null) {
            ElementS.transform.rotation = Quaternion.Euler(0f, 0f, int.Parse(rot.text));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementSelected + "BASEROT", ElementS.transform.rotation.eulerAngles.z);
        }
    }

    public void Sizeation(InputField siz) { if(ElementS != null) {
            ElementS.transform.localScale = new Vector2(float.Parse(siz.text, CultureInfo.InvariantCulture), float.Parse(siz.text, CultureInfo.InvariantCulture));
            PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementSelected + "BASESIZE", ElementS.transform.localScale.x);
        }
    }

    public void SettrIn(bool IN) {
        InputFMode = IN;
    }

    public void OnBackSelect() {
        BackMode = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "BACKMODE", 0);
        if (!UsingMode) { bm.value = BackMode; } LoadMode = false;
    }

    public void ModBackgrd() {
        if (!UsingMode && !LoadMode) { BackMode = bm.value; }
        if (BackMode == 0) Camera.main.backgroundColor = SColorBack;
    }

    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET");
        SColorBack = new Color(PlayerPrefs.GetFloat(colorstart + "BACKSCR", 0.25f), PlayerPrefs.GetFloat(colorstart + "BACKSCG", 0.3f), PlayerPrefs.GetFloat(colorstart + "BACKSCB", 0.56f));
        if (!UsingMode) { scb.color = SColorBack; } ModBackgrd();
    }

    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET");
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject);
    }

    public void TOuchMepls(bool HellNO) {
        Touchy = HellNO;
    }

    void Start() { Loading = true;
        PrefsElements[0].GetComponent<AnalogSpedometer>().LoadMode = true; PrefsElements[1].GetComponent<AnalogDisplay>().LoadMode = true;
        PrefsElements[2].GetComponent<LCDSpedometer>().LoadMode = true; PrefsElements[3].GetComponent<LCDDisplay>().LoadMode = true;
        PrefsElements[4].GetComponent<AnalogSpeedbar>().LoadMode = true; PrefsElements[5].GetComponent<CircSpeedBar>().LoadMode = true;
        PrefsElements[6].GetComponent<LCDSpeedbar>().LoadMode = true; PrefsElements[7].GetComponent<LCDCircSpeedbar>().LoadMode = true;
        PrefsElements[8].GetComponent<Line>().LoadMode = true; PrefsElements[9].GetComponent<TextComp>().LoadMode = true;
        PrefsElements[10].GetComponent<ImageComponent>().LoadMode = true; PrefsElements[11].GetComponent<Compass>().LoadMode = true;
        PrefsElements[12].GetComponent<MusicPlayer>().LoadMode = true; PrefsElements[13].GetComponent<MapSystem>().LoadMode = true;
        if (UsingMode) { PrefsElements[0].GetComponent<AnalogSpedometer>().SceneMode = true; PrefsElements[1].GetComponent<AnalogDisplay>().SceneMode = true;
        PrefsElements[2].GetComponent<LCDSpedometer>().SceneMode = true; PrefsElements[3].GetComponent<LCDDisplay>().SceneMode = true;
        PrefsElements[4].GetComponent <AnalogSpeedbar>().SceneMode = true; PrefsElements[5].GetComponent<CircSpeedBar>().SceneMode = true;
        PrefsElements[6].GetComponent<LCDSpeedbar>().SceneMode = true; PrefsElements[7].GetComponent<LCDCircSpeedbar>().SceneMode = true;
        PrefsElements[8].GetComponent<Line>().SceneMode = true; PrefsElements[9].GetComponent<TextComp>().SceneMode = true;
        PrefsElements[10].GetComponent<ImageComponent>().SceneMode = true; PrefsElements[11].GetComponent<Compass>().SceneMode = true;
        PrefsElements[12].GetComponent<MusicPlayer>().SceneMode = true; PrefsElements[13].GetComponent<MapSystem>().SceneMode = true;
        } int ActivePests = 0; while (true) {
            if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + mpfs + "TYPE", 99999) == 99999) break;
            if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + mpfs + "TYPE", 99999) == 69696) mpfs++;
            else { AddElementOfType(PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + mpfs + "TYPE"));
            mpfs++; ActivePests++; } }
        for (int f = 0; f < ActivePests; f++) AllElements.GetChild(f).GetComponent<SortOrderOrg>().Apply();
        PrefsElements[0].GetComponent<AnalogSpedometer>().LoadMode = false; PrefsElements[1].GetComponent<AnalogDisplay>().LoadMode = false;
        PrefsElements[2].GetComponent<LCDSpedometer>().LoadMode = false; PrefsElements[3].GetComponent<LCDDisplay>().LoadMode = false;
        PrefsElements[4].GetComponent<AnalogSpeedbar>().LoadMode = false; PrefsElements[5].GetComponent<CircSpeedBar>().LoadMode = false;
        PrefsElements[6].GetComponent<LCDSpeedbar>().LoadMode = false; PrefsElements[7].GetComponent<LCDCircSpeedbar>().LoadMode = false;
        PrefsElements[8].GetComponent<Line>().LoadMode = false; PrefsElements[9].GetComponent<TextComp>().LoadMode = false;
        PrefsElements[10].GetComponent<ImageComponent>().LoadMode = false; PrefsElements[11].GetComponent<Compass>().LoadMode = false;
        PrefsElements[12].GetComponent<MusicPlayer>().LoadMode = false; PrefsElements[13].GetComponent<MapSystem>().LoadMode = false;
        if (!UsingMode) { if (PlayerPrefs.GetInt("EDITORSNAP", 8) == 0) Snapp = false; else Snapp = true; SnappAmount = PlayerPrefs.GetInt("EDITORSNAP", 8);
        Snappr.isOn = Snapp; SnapprAmount.value = SnappAmount; SnapprAmountIN.text = SnappAmount + "";
        if (PlayerPrefs.GetInt("EDITORMOVEA", 1) == 0) Move.isOn = false; else Move.isOn = true;
        if (PlayerPrefs.GetInt("EDITORROTA", 1) == 0) Rotate.isOn = false; else Rotate.isOn = true;
        if (PlayerPrefs.GetInt("EDITORSCALA", 1) == 0) Scale.isOn = false; else Scale.isOn = true;
        if (PlayerPrefs.GetInt("EDITORGRIDSNAP", 0) == 0) GridSnap = false; else GridSnap = true; GridSpace = PlayerPrefs.GetInt("EDITORGRIDSNAP", 0);
        Opacity = PlayerPrefs.GetFloat("EDITORGRIDOPAC", 0.06f); GSnappr.isOn = GridSnap; GSnapprOpa.value = Opacity; GSnapprAmountIN.text = GridSpace + ""; OnGridRegen();
        Loading = false; EditorMenuMod(); }
        if (LoadMode) OnBackSelect();
        ModBackgrd(); OnColorModdify();
        if (!UsingMode) if (PlayerPrefs.GetInt("HASHINT4", 0) == 0) {
            if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(0f, 0f), "Editor", "Welcome to the editor, here you can create or edit presets. You can add new elements by tapping the + button or change preset settings with the top menu", 1000f, 0.3f, 4); 
            if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(0f, 0f), "Éditeur", "Bienvenue dans l'éditeur ! Ici, vous pouvez créer ou modifier des préréglages. Vous pouvez ajouter de nouveaux éléments en appuyant sur le bouton + ou modifier les paramètres des préréglages via le menu du haut", 1100f, 0.3f, 4); 
            if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(0f, 0f), "Editor", "Willkommen im Editor! Hier können Sie Voreinstellungen erstellen oder bearbeiten. Sie können neue Elemente hinzufügen, indem Sie auf die Schaltfläche „+“ tippen, oder die Einstellungen der Voreinstellungen über das obere Menü ändern", 1200f, 0.3f, 4); 
            if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(0f, 0f), "Editor", "Bun venit în editor, aici puteți crea sau edita presetări. Puteți adăuga elemente noi apăsând butonul + sau puteți modifica setările presetărilor cu meniul superior", 1000f, 0.3f, 4); 
        }
    }

    public void OnGridRegen() { 
        foreach (Transform wow in Orgfel) Destroy(wow.gameObject); if(Opacity > 0f && GridSpace > 5) { 
        int HorP = 0; while (HorP < LrCan.rect.width) {
            GameObject HorL = Instantiate(Hor.gameObject, Orgfel); HorP += GridSpace;
            HorL.SetActive(true); HorL.GetComponent<RectTransform>().anchoredPosition = new Vector2(HorP, 0f);
            HorL.GetComponent<RawImage>().color = new Color(1f, 1f, 1f, Opacity);
        } int VerP = 0; while(VerP < LrCan.rect.height) {
            GameObject VerL = Instantiate(Ver.gameObject, Orgfel); VerP += GridSpace;
            VerL.SetActive(true); VerL.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, VerP);
            VerL.GetComponent<RawImage>().color = new Color(1f, 1f, 1f, Opacity);
        } }
    }

    void Update() {
        if (ElementS != null)
        if (Input.touchCount == 2 && (Scale.isOn || Rotate.isOn)) {
            AllPanels.SetActive(false);
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);
            Vector2 t0Prev = t0.position - t0.deltaPosition;
            Vector2 t1Prev = t1.position - t1.deltaPosition;
            float prevDist = Vector2.Distance(t0Prev, t1Prev);
            float currDist = Vector2.Distance(t0.position, t1.position);
            if(Scale.isOn) ElementS.localScale *= currDist / prevDist;
            if(Rotate.isOn) { float pAngle = Mathf.Atan2(t1Prev.y - t0Prev.y, t1Prev.x - t0Prev.x) * Mathf.Rad2Deg;
            float cAngle = Mathf.Atan2(t1.position.y - t0.position.y, t1.position.x - t0.position.x) * Mathf.Rad2Deg;
            ElementS.Rotate(0, 0, (cAngle - pAngle));
            if (ElementS.eulerAngles.z > 180) ElementS.Rotate(0f, 0f, -360f);
            float snapple = Mathf.Round(ElementS.eulerAngles.z / 45f) * 45f;
            if (Mathf.Abs(ElementS.eulerAngles.z - snapple) <= 5f)
                ElementS.rotation = Quaternion.Euler(0, 0, snapple); }
            if(Rotate.isOn) PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementSelected + "BASEROT", ElementS.transform.rotation.eulerAngles.z);
            if(Scale.isOn) PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementSelected + "BASESIZE", ElementS.transform.localScale.x);
        } if (ElementS != null) if(Input.touchCount == 1 || Input.touchCount == 2) {
            if (Input.touchCount == 1)  AllPanels.SetActive(true);
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved && Touchy && Move.isOn) { 
            ElementS.anchoredPosition += touch.deltaPosition; ActualPos += touch.deltaPosition;
            if (GridSnap) ElementS.anchoredPosition = new Vector2(((int)ActualPos.x / GridSpace) * GridSpace, ((int)ActualPos.y / GridSpace) * GridSpace);
            if (Snapp) { SnappH.SetActive(false); SnappV.SetActive(false);
            RectTransform self = ElementS;
            Vector2 selfPos = self.anchoredPosition;
            Vector2 selfSize = self.rect.size;
            Vector2 selfScaledSize = Vector2.Scale(self.rect.size, self.localScale);
            if(self.localRotation.eulerAngles.z % 360f == 90f || self.localRotation.eulerAngles.z % 360f == 270f) selfScaledSize = new Vector2(selfScaledSize.y, selfScaledSize.x);
            float selfTop = selfPos.y + (selfScaledSize.y / 2);
            float selfBottom = selfPos.y - (selfScaledSize.y / 2);
            float selfLeft = selfPos.x - (selfScaledSize.x / 2);
            float selfRight = selfPos.x + (selfScaledSize.x / 2);
            foreach (Transform alr in AllElements) { 
            RectTransform other = alr as RectTransform;
            if (other != null && other != self) {
                Vector2 otherPos = other.anchoredPosition;
                Vector2 otherSize = other.rect.size;
                Vector2 otherScaledSize = Vector2.Scale(other.rect.size, other.localScale);
                if(other.localRotation.eulerAngles.z % 360f == 90f || other.localRotation.eulerAngles.z % 360f == 270f) otherScaledSize = new Vector2(otherScaledSize.y, otherScaledSize.x);
                float otherLeft = otherPos.x - (otherScaledSize.x / 2);
                float otherRight = otherPos.x + (otherScaledSize.x / 2);
                float dx = Mathf.Abs(selfPos.x - otherPos.x);
                if (dx <= SnappAmount) {
                    self.anchoredPosition = new Vector2(otherPos.x, self.anchoredPosition.y);
                    SnappH.SetActive(true); RectTransform sh = SnappH.GetComponent<RectTransform>();
                    sh.anchoredPosition = new Vector2(otherPos.x, (selfPos.y + otherPos.y) / 2f);
                    sh.sizeDelta = new Vector2(8f, Mathf.Abs(selfPos.y - otherPos.y)); break;
                } if(Mathf.RoundToInt(self.localRotation.eulerAngles.z) % 90 == 0 && Mathf.RoundToInt(other.localRotation.eulerAngles.z) % 90 == 0) { if (Mathf.Abs(selfLeft - otherLeft) <= SnappAmount) {
                    self.anchoredPosition = new Vector2(otherLeft + selfScaledSize.x / 2, self.anchoredPosition.y);
                    SnappH.SetActive(true); RectTransform sh = SnappH.GetComponent<RectTransform>();
                    sh.anchoredPosition = new Vector2(otherLeft, self.anchoredPosition.y);
                    sh.sizeDelta = new Vector2(8f, selfScaledSize.y); break;
                } if (Mathf.Abs(selfLeft - otherRight) <= SnappAmount) {
                    self.anchoredPosition = new Vector2(otherRight + selfScaledSize.x / 2, self.anchoredPosition.y);
                    SnappH.SetActive(true); RectTransform sh = SnappH.GetComponent<RectTransform>();
                    sh.anchoredPosition = new Vector2(otherRight, self.anchoredPosition.y);
                    sh.sizeDelta = new Vector2(8f, selfScaledSize.y); break;
                } if (Mathf.Abs(selfRight - otherLeft) <= SnappAmount) {
                    self.anchoredPosition = new Vector2(otherLeft - selfScaledSize.x / 2, self.anchoredPosition.y);
                    SnappH.SetActive(true); RectTransform sh = SnappH.GetComponent<RectTransform>();
                    sh.anchoredPosition = new Vector2(otherLeft, self.anchoredPosition.y);
                    sh.sizeDelta = new Vector2(8f, selfScaledSize.y); break;
                } if (Mathf.Abs(selfRight - otherRight) <= SnappAmount) {
                    self.anchoredPosition = new Vector2(otherRight - selfScaledSize.x / 2, self.anchoredPosition.y);
                    SnappH.SetActive(true); RectTransform sh = SnappH.GetComponent<RectTransform>();
                    sh.anchoredPosition = new Vector2(otherRight, self.anchoredPosition.y);
                    sh.sizeDelta = new Vector2(8f, selfScaledSize.y); break;
                } }
            }
        } foreach (Transform alr in AllElements) { 
            RectTransform other = alr as RectTransform;
            if (other != null && other != self) {
                Vector2 otherPos = other.anchoredPosition;
                Vector2 otherSize = other.rect.size;
                Vector2 otherScaledSize = Vector2.Scale(other.rect.size, other.localScale);
                if(other.localRotation.eulerAngles.z % 360f == 90f || other.localRotation.eulerAngles.z % 360f == 270f) otherScaledSize = new Vector2(otherScaledSize.y, otherScaledSize.x);
                float otherTop = otherPos.y + (otherScaledSize.y / 2);
                float otherBottom = otherPos.y - (otherScaledSize.y / 2);
                float dy = Mathf.Abs(selfPos.y - otherPos.y);
                if (dy <= SnappAmount) {
                    self.anchoredPosition = new Vector2(self.anchoredPosition.x, otherPos.y);
                    SnappV.SetActive(true); RectTransform sv = SnappV.GetComponent<RectTransform>();
                    sv.anchoredPosition = new Vector2((selfPos.x + otherPos.x) / 2f, otherPos.y);
                    sv.sizeDelta = new Vector2(Mathf.Abs(selfPos.x - otherPos.x), 8f); break;
                } if(Mathf.RoundToInt(self.localRotation.eulerAngles.z) % 90 == 0 && Mathf.RoundToInt(other.localRotation.eulerAngles.z) % 90 == 0) { if (Mathf.Abs(selfTop - otherTop) <= SnappAmount) {
                    self.anchoredPosition = new Vector2(self.anchoredPosition.x, otherTop - selfScaledSize.y / 2);
                    SnappV.SetActive(true); RectTransform sv = SnappV.GetComponent<RectTransform>();
                    sv.anchoredPosition = new Vector2(self.anchoredPosition.x, otherTop);
                    sv.sizeDelta = new Vector2(selfScaledSize.x, 8f); break;
                } if (Mathf.Abs(selfTop - otherBottom) <= SnappAmount) {
                    self.anchoredPosition = new Vector2(self.anchoredPosition.x, otherBottom - selfScaledSize.y / 2);
                    SnappV.SetActive(true); RectTransform sv = SnappV.GetComponent<RectTransform>();
                    sv.anchoredPosition = new Vector2(self.anchoredPosition.x, otherBottom);
                    sv.sizeDelta = new Vector2(selfScaledSize.x, 8f); break;
                } if (Mathf.Abs(selfBottom - otherTop) <= SnappAmount) {
                    self.anchoredPosition = new Vector2(self.anchoredPosition.x, otherTop + selfScaledSize.y / 2);
                    SnappV.SetActive(true); RectTransform sv = SnappV.GetComponent<RectTransform>();
                    sv.anchoredPosition = new Vector2(self.anchoredPosition.x, otherTop);
                    sv.sizeDelta = new Vector2(selfScaledSize.x, 8f); break;
                } if (Mathf.Abs(selfBottom - otherBottom) <= SnappAmount) {
                    self.anchoredPosition = new Vector2(self.anchoredPosition.x, otherBottom + selfScaledSize.y / 2);
                    SnappV.SetActive(true); RectTransform sv = SnappV.GetComponent<RectTransform>();
                    sv.anchoredPosition = new Vector2(self.anchoredPosition.x, otherBottom);
                    sv.sizeDelta = new Vector2(selfScaledSize.x, 8f); break;
                } }
            }
        }
    } PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementSelected + "BASEPOSX", ElementS.anchoredPosition.x);
      PlayerPrefs.SetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementSelected + "BASEPOSY", ElementS.anchoredPosition.y); SnapUI();
    } else if (!Touchy) { SnappH.SetActive(false); SnappV.SetActive(false); }
}  if (Input.GetKeyDown(KeyCode.Escape)) OnExitOfEdit();
    }

    public void SnapUI() {
        if (ElementS.anchoredPosition.x <= 0f) {
            for(int u = 0; u < allmens.Count; u++) {
                allmens[u].offsetMin = new Vector2(30, 30); allmens[u].offsetMax = new Vector2(-430, -30);
            } if (ElementS.GetComponent<AnalogSpedometer>()) {
            ElementS.GetComponent<AnalogSpedometer>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } else if (ElementS.GetComponent<AnalogDisplay>()) {
            ElementS.GetComponent<AnalogDisplay>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } else if (ElementS.GetComponent<LCDSpedometer>()) {
            ElementS.GetComponent<LCDSpedometer>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } else if (ElementS.GetComponent<LCDDisplay>()) {
            ElementS.GetComponent<LCDDisplay>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } else if (ElementS.GetComponent<AnalogSpeedbar>()) {
            ElementS.GetComponent<AnalogSpeedbar>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } else if (ElementS.GetComponent<CircSpeedBar>()) {
            ElementS.GetComponent<CircSpeedBar>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } else if (ElementS.GetComponent<LCDSpeedbar>()) {
            ElementS.GetComponent<LCDSpeedbar>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } else if (ElementS.GetComponent<LCDCircSpeedbar>()) {
            ElementS.GetComponent<LCDCircSpeedbar>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } else if (ElementS.GetComponent<Line>()) {
            ElementS.GetComponent<Line>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } else if (ElementS.GetComponent<TextComp>()) {
            ElementS.GetComponent<TextComp>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } else if (ElementS.GetComponent<ImageComponent>()) {
            ElementS.GetComponent<ImageComponent>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } else if (ElementS.GetComponent<Compass>()) {
            ElementS.GetComponent<Compass>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } else if (ElementS.GetComponent<MusicPlayer>()) {
            ElementS.GetComponent<MusicPlayer>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } else if (ElementS.GetComponent<MapSystem>()) {
            ElementS.GetComponent<MapSystem>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        } } else { for(int u = 0; u < allmens.Count; u++) {
                allmens[u].offsetMin = new Vector2(430, 30); allmens[u].offsetMax = new Vector2(-30, -30);
            } if (ElementS.GetComponent<AnalogSpedometer>()) {
            ElementS.GetComponent<AnalogSpedometer>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } else if (ElementS.GetComponent<AnalogDisplay>()) {
            ElementS.GetComponent<AnalogDisplay>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } else if (ElementS.GetComponent<LCDSpedometer>()) {
            ElementS.GetComponent<LCDSpedometer>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } else if (ElementS.GetComponent<LCDDisplay>()) {
            ElementS.GetComponent<LCDDisplay>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } else if (ElementS.GetComponent<AnalogSpeedbar>()) {
            ElementS.GetComponent<AnalogSpeedbar>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } else if (ElementS.GetComponent<CircSpeedBar>()) {
            ElementS.GetComponent<CircSpeedBar>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } else if (ElementS.GetComponent<LCDSpeedbar>()) {
            ElementS.GetComponent<LCDSpeedbar>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } else if (ElementS.GetComponent<LCDCircSpeedbar>()) {
            ElementS.GetComponent<LCDCircSpeedbar>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } else if (ElementS.GetComponent<Line>()) {
            ElementS.GetComponent<Line>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } else if (ElementS.GetComponent<TextComp>()) {
            ElementS.GetComponent<TextComp>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } else if (ElementS.GetComponent<ImageComponent>()) {
            ElementS.GetComponent<ImageComponent>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } else if (ElementS.GetComponent<Compass>()) {
            ElementS.GetComponent<Compass>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } else if (ElementS.GetComponent<MusicPlayer>()) {
            ElementS.GetComponent<MusicPlayer>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } else if (ElementS.GetComponent<MapSystem>()) {
            ElementS.GetComponent<MapSystem>().EditorModPanel.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-(LrCan.rect.width - 400f), 0f);
        } }
    }

    public void BackMenuEnable() {
        for (int u = 0; u < allmens.Count; u++) {
            allmens[u].offsetMin = new Vector2(30, 30); allmens[u].offsetMax = new Vector2(-430, -30);
        }
    }

    public void OnExitOfEdit() {
        if (!UsingMode) {
            for (int p = 0; p < AllPanels.transform.childCount; p++)
                AllPanels.transform.GetChild(p).gameObject.SetActive(false);
            Orgfel.gameObject.SetActive(false); StartCoroutine(ScreenShtASJepejepeg());
        }else SceneManager.LoadScene(0);
    }

    public void AddElementOfType(int Type) {
        if (!Loading) {
            GameObject NewELement = Instantiate(PrefsElements[Type], AllElements);
            if (Type == 0) NewELement.GetComponent<AnalogSpedometer>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            if (Type == 1) NewELement.GetComponent<AnalogDisplay>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            if (Type == 2) NewELement.GetComponent<LCDSpedometer>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            if (Type == 3) NewELement.GetComponent<LCDDisplay>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            if (Type == 4) NewELement.GetComponent<AnalogSpeedbar>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            if (Type == 5) NewELement.GetComponent<CircSpeedBar>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            if (Type == 6) NewELement.GetComponent<LCDSpeedbar>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            if (Type == 7) NewELement.GetComponent<LCDCircSpeedbar>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            if (Type == 8) NewELement.GetComponent<Line>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            if (Type == 9) NewELement.GetComponent<TextComp>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            if (Type == 10) NewELement.GetComponent<ImageComponent>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            if (Type == 11) { NewELement.GetComponent<Compass>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
                PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "PRESHASGPSELEMS", 1); }
            if (Type == 12) NewELement.GetComponent<MusicPlayer>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            if (Type == 13) { NewELement.GetComponent<MapSystem>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
                PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "PRESHASGPSELEMS", 1); }
            NewELement.GetComponent<SortOrderOrg>().ElementID = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            NewELement.SetActive(true); int NEWVAR = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ");
            NewELement.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            if(!UsingMode) NewELement.transform.GetChild(NewELement.transform.childCount - 1).GetComponent<Button>().onClick.AddListener(() => OnElemSelect(NEWVAR));
            NewELement.name = NEWVAR + ""; OnElemSelect(NEWVAR);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ") + "SO", AllElements.childCount - 1);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ") + "TYPE", Type);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "TOTALOBJ") + 1);
            if (PlayerPrefs.GetInt("HASHINT17", 0) == 0) { 
                if(PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(0f, 0f), "Element Settings", "Each element in the editor has transform and object properties, you can move, scale and rotate objects by dragging them or using the transform tab and their properties can be moddified by the properties tab, each element has it's own properties, however all elements have sort order and can be deleted from that tab", 1200f, 0.3f, 17);
                if(PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(0f, 0f), "Paramètres des éléments", "Chaque élément de l'éditeur possède des propriétés de transformation et d'objet. Vous pouvez déplacer, redimensionner et faire pivoter les objets en les faisant glisser ou en utilisant l'onglet Transformation. Leurs propriétés peuvent être modifiées via l'onglet Propriétés. Chaque élément possède ses propres propriétés, mais tous les éléments ont un ordre de tri et peuvent être supprimés depuis cet onglet", 1400f, 0.3f, 17);
                if(PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(0f, 0f), "Elementeinstellungen", "Jedes Element im Editor verfügt über Transformations- und Objekteigenschaften. Objekte lassen sich per Drag & Drop oder über den Transformations-Tab verschieben, skalieren und drehen. Ihre Eigenschaften können über den Eigenschaften-Tab angepasst werden. Jedes Element hat seine eigenen Eigenschaften, ist aber auch sortiert und kann über diesen Tab gelöscht werden", 1300f, 0.3f, 17);
                if(PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(0f, 0f), "Setări element", "Fiecare element din editor are proprietăți de transformare și de obiect, puteți muta, scala și roti obiecte trăgându-le sau utilizând fila transformare, iar proprietățile lor pot fi modificate din fila proprietăți. Fiecare element are propriile proprietăți, însă toate elementele au o ordine de sortare și pot fi șterse din acea filă", 1200f, 0.3f, 17);
            } if (Type == 0) if (PlayerPrefs.GetInt("HASHINT5", 0) == 0) {
                if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Analog Speedometer", "This is an element that functions like a regular car speedometer, you can modify it's limits like max speed, color scheme etc.", 800f, 0.3f, 5);
                if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(-400f, 0f), "Tachymètre analogique", "Il s'agit d'un élément qui fonctionne comme un compteur de vitesse de voiture classique ; vous pouvez en modifier les limites telles que la vitesse maximale, le schéma de couleurs, etc.", 1000f, 0.3f, 5);
                if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Analoger Tachometer", "Es handelt sich um ein Element, das wie ein normaler Tachometer im Auto funktioniert; seine Grenzen wie Höchstgeschwindigkeit, Farbschema usw. können angepasst werden", 900f, 0.3f, 5);
                if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Vitezometru analogic", "Acesta este un element care funcționează ca un vitezometru de mașină obișnuit, puteți modifica limitele sale, cum ar fi viteza maximă, schema de culori etc.", 800f, 0.3f, 5);
            } if (Type == 1) if (PlayerPrefs.GetInt("HASHINT6", 0) == 0) {
                if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Analog Display", "This is an element that functions like an odometer found on a regular car speedometer, you can modify what's being displayed, it's color scheme etc.", 800f, 0.3f, 6);
                if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Affichage analogique", "Cet élément fonctionne comme un compteur kilométrique que l'on trouve sur un compteur de vitesse de voiture classique ; vous pouvez modifier ce qui est affiché, son schéma de couleurs, etc.", 900f, 0.3f, 6);
                if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Analoganzeige", "Es handelt sich um ein Element, das wie ein Kilometerzähler in einem normalen Autotacho funktioniert; man kann die angezeigten Daten, das Farbschema usw. anpassen", 800f, 0.3f, 6);
                if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Display analogic", "Acesta este un element care funcționează ca un odometru găsit pe un vitezometru de mașină obișnuit, puteți modifica ceea ce este afișat, schema de culori etc.", 800f, 0.3f, 6);
            } if (Type == 2) if (PlayerPrefs.GetInt("HASHINT7", 0) == 0) {
                if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(-500f, 0f), "LCD Speedometer", "This is an element that functions like a speedometer that works with LCD, typically found in motorcycles, you can modify it's color scheme and LCD properties", 800f, 0.3f, 7);
                if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Tachymètre LCD", "Il s'agit d'un élément fonctionnant comme un compteur de vitesse avec écran LCD, généralement présent sur les motos. Vous pouvez modifier son schéma de couleurs et ses propriétés LCD", 900f, 0.3f, 7);
                if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(-400f, 0f), "LCD-Tachometer", "Es handelt sich um ein Bauteil, das wie ein Geschwindigkeitsmesser mit LCD-Display funktioniert und typischerweise in Motorrädern zu finden ist. Farbschema und LCD-Eigenschaften lassen sich anpassen", 1000f, 0.3f, 7);
                if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Vitezometru LCD", "Acesta este un element care funcționează ca un vitezometru cu ecran LCD, de obicei întâlnit la motociclete, puteți modifica schema de culori și proprietățile LCD-ului", 800f, 0.3f, 7);
            } if (Type == 3) if (PlayerPrefs.GetInt("HASHINT8", 0) == 0) {
                if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(-500f, 0f), "LCD Display", "This is an element that functions like a digital odometer found on car speedometers, you can modify what's being displayed, it's color scheme etc.", 800f, 0.3f, 8);
                if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(-400f, 0f), "Affichage LCD", "Il s'agit d'un élément qui fonctionne comme un compteur kilométrique numérique, comme ceux que l'on trouve sur les compteurs de vitesse des voitures ; vous pouvez modifier ce qui est affiché, son schéma de couleurs, etc.", 1000f, 0.3f, 8);
                if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(-450f, 0f), "LCD-Display", "Es handelt sich um ein Element, das wie ein digitaler Kilometerzähler in Autotachometern funktioniert; man kann die angezeigten Informationen, das Farbschema usw. anpassen", 900f, 0.3f, 8);
                if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Display LCD", "Acesta este un element care funcționează ca un odometru digital găsit pe vitezometrele auto, puteți modifica ceea ce este afișat, schema de culori etc.", 800f, 0.3f, 8);
            } if (Type == 4) if (PlayerPrefs.GetInt("HASHINT9", 0) == 0) {
                if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Analog Speedbar", "This is an element that functions like a speedometer, however the needle moves in a linear direction, you can modify it's limits like max speed, it's color scheme etc.", 800f, 0.3f, 9);
                if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Barre de vitesse analogique", "Il s'agit d'un élément qui fonctionne comme un compteur de vitesse, mais l'aiguille se déplace de manière linéaire ; vous pouvez modifier ses limites, comme la vitesse maximale, son schéma de couleurs, etc.", 900f, 0.3f, 9);
                if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Analoge Geschwindigkeitsleiste", "Es handelt sich um ein Element, das wie ein Tachometer funktioniert, allerdings bewegt sich die Nadel linear. Man kann seine Grenzwerte wie Höchstgeschwindigkeit, sein Farbschema usw. anpassen", 900f, 0.3f, 9);
                if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Bară de viteză analogică", "Acesta este un element care funcționează ca un vitezometru, însă acul se mișcă într-o direcție liniară, puteți modifica limitele sale, cum ar fi viteza maximă, schema de culori etc.", 800f, 0.3f, 9);
            } if (Type == 5) if (PlayerPrefs.GetInt("HASHINT10", 0) == 0) {
                if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Circular Analog Speedbar", "This is an element that functions like a speedometer, however it has a donut shape, not a full circle, you can modify it's limits like max speed, it's color scheme etc.", 800f, 0.3f, 10);
                if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(-400f, 0f), "Barre de vitesse analogique circulaire", "Il s'agit d'un élément qui fonctionne comme un compteur de vitesse, mais il a la forme d'un anneau et non d'un cercle complet ; vous pouvez modifier ses limites comme la vitesse maximale, son schéma de couleurs, etc.", 1000f, 0.3f, 10);
                if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(-400f, 0f), "Kreisförmige analoge Geschwindigkeitsleiste", "Es handelt sich um ein Element, das wie ein Tachometer funktioniert, allerdings hat es die Form eines Donuts und ist kein vollständiger Kreis. Man kann seine Grenzen wie Höchstgeschwindigkeit, sein Farbschema usw. anpassen", 1000f, 0.3f, 10);
                if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Bară de viteză analogică circulară", "Acesta este un element care funcționează ca un vitezometru, însă are o formă de gogoașă, nu un cerc complet, iar limitele sale pot fi modificate, cum ar fi viteza maximă, schema de culori etc.", 900f, 0.3f, 10);
            } if (Type == 6) if (PlayerPrefs.GetInt("HASHINT11", 0) == 0) {
                if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(-500f, 0f), "LCD Speedbar", "This is an element that functions like a digital speedometer, where LCD segments are lit based on your speed arranged in a line, you can modify it's color scheme, LCD properties etc.", 800f, 0.3f, 11);
                if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Barre de vitesse LCD", "Il s'agit d'un élément fonctionnant comme un compteur de vitesse numérique, où les segments LCD s'allument en fonction de votre vitesse, disposés en ligne ; vous pouvez modifier son schéma de couleurs, les propriétés LCD, etc.", 900f, 0.3f, 11);
                if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(-400f, 0f), "LCD-Geschwindigkeitsbalken", "Es handelt sich um ein Element, das wie ein digitaler Tachometer funktioniert, bei dem LCD-Segmente in einer Reihe angeordnet sind und je nach Geschwindigkeit aufleuchten. Sie können das Farbschema, die LCD-Eigenschaften usw. anpassen", 1000f, 0.3f, 11);
                if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Bară de viteză LCD", "Acesta este un element care funcționează ca un vitezometru digital, unde segmentele LCD sunt iluminate în funcție de viteza dvs., aranjate într-o linie, puteți modifica schema de culori, proprietățile LCD etc.", 900f, 0.3f, 11);
            } if (Type == 7) if (PlayerPrefs.GetInt("HASHINT12", 0) == 0) {
                if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Circular LCD Speedbar", "This is an element that functions like a digital speedometer, where LCD segments are lit based on your speed arranged in a circle, you can modify it's color scheme, LCD properties etc.", 800f, 0.3f, 12);
                if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(-400f, 0f), "Barre de vitesse circulaire LCD", "Il s'agit d'un élément fonctionnant comme un compteur de vitesse numérique, où des segments LCD s'allument en fonction de votre vitesse, disposés en cercle ; vous pouvez modifier son schéma de couleurs, les propriétés de l'écran LCD, etc.", 1000f, 0.3f, 12);
                if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(-400f, 0f), "Runde LCD-Geschwindigkeitsleiste", "Es handelt sich um ein Element, das wie ein digitaler Tachometer funktioniert, bei dem LCD-Segmente in einem Kreis angeordnet sind und je nach Geschwindigkeit aufleuchten. Sie können das Farbschema, die LCD-Eigenschaften usw. anpassen", 1000f, 0.3f, 12);
                if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Bară de viteză LCD circulară", "Acesta este un element care funcționează ca un vitezometru digital, unde segmentele LCD sunt iluminate în funcție de viteza dvs., aranjate într-un cerc, puteți modifica schema de culori, proprietățile LCD etc.", 900f, 0.3f, 12);
            } if (Type == 8) if (PlayerPrefs.GetInt("HASHINT21", 0) == 0) {
                if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Line Component", "This is exactly what you think it is, an object that can function as a line, you can moddify the color, length and width", 800f, 0.3f, 21);
                if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Composant de ligne", "Cet élément est exactement ce que vous imaginez : un objet qui peut servir de ligne, dont vous pouvez modifier la couleur, la longueur et la largeur", 900f, 0.3f, 21);
                if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Linienkomponente", "Dieses Element ist genau das, was Sie denken: ein Objekt, das als Linie fungieren kann. Sie können Farbe, Länge und Breite anpassen", 800f, 0.3f, 21);
                if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Componenta linie", "Acest element este exact ce crezi că este, un obiect care poate funcționa ca o linie, poți modifica culoarea, lungimea și lățimea", 800f, 0.3f, 21);
            } if (Type == 9) if (PlayerPrefs.GetInt("HASHINT13", 0) == 0) {
                if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Text Component", "This is a text element, you can modify it's general settings like font used, font size etc. and can also have special effects", 800f, 0.3f, 13); 
                if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(-400f, 0f), "Composant texte", "Il s'agit d'un élément texte ; vous pouvez modifier ses paramètres généraux tels que la police utilisée, la taille de la police, etc., et lui appliquer des effets spéciaux", 1000f, 0.3f, 13); 
                if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Textkomponente", "Dies ist ein Textelement, dessen allgemeine Einstellungen wie Schriftart, Schriftgröße usw. geändert werden können und das auch Spezialeffekte aufweisen kann", 900f, 0.3f, 13); 
                if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Componenta text", "Acesta este un element text, puteți modifica setările sale generale, cum ar fi fontul utilizat, dimensiunea fontului etc. și poate avea și efecte speciale", 900f, 0.3f, 13); 
            } if (Type == 10) if (PlayerPrefs.GetInt("HASHINT14", 0) == 0) {
                if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Image Component", "This is an image that you can select from your device, and can apply settings to it like color and it's layout with tiling and slicing", 800f, 0.3f, 14);
                if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(-400f, 0f), "Composant d'image", "Il s'agit d'une image que vous pouvez sélectionner sur votre appareil et à laquelle vous pouvez appliquer des paramètres tels que la couleur et sa mise en page (modélisation et découpage)", 1000f, 0.3f, 14);
                if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Bildkomponente", "Dies ist ein Bild, das Sie von Ihrem Gerät auswählen und auf das Sie Einstellungen wie Farbe und Layout mit Kachelung und Teilung anwenden können", 800f, 0.3f, 14);
                if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Componenta imagine", "Aceasta este o imagine pe care o puteți selecta de pe dispozitiv și îi puteți aplica setări precum culoarea și aspectul cu împărțire în dale și feliere", 800f, 0.3f, 14);
            } if (Type == 11) if (PlayerPrefs.GetInt("HASHINT15", 0) == 0) {
                if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Compass", "This is an element that functions like a regular compass, you can modify it's appearance like color scheme and 3D aspect", 800f, 0.3f, 15);
                if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Boussole", "Cet élément fonctionne comme une boussole classique ; vous pouvez en modifier l’apparence, notamment les couleurs et l’aspect 3D", 800f, 0.3f, 15);
                if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Kompass", "Dieses Element funktioniert wie ein normaler Kompass; sein Aussehen, z. B. Farbschema und 3D-Ansicht, lässt sich anpassen", 800f, 0.3f, 15);
                if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Busolă", "Acesta este un element care funcționează ca o busolă obișnuită, îi puteți modifica aspectul, cum ar fi schema de culori și aspectul 3D", 900f, 0.3f, 15);
            } if (Type == 12) if (PlayerPrefs.GetInt("HASHINT16", 0) == 0) {
                if (PlayerPrefs.GetInt("MAINLANG") == 0) touhou.AddTutorialHint(new Vector2(-500f, 0f), "Music Player", "This is an element that can function as a music player, you can modify it's settings, color scheme and layout", 800f, 0.3f, 16);
                if (PlayerPrefs.GetInt("MAINLANG") == 1) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Lecteur de musique", "Cet élément peut servir de lecteur de musique ; vous pouvez modifier ses paramètres, son thème de couleurs et sa mise en page", 900f, 0.3f, 16);
                if (PlayerPrefs.GetInt("MAINLANG") == 2) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Musik-Player", "Dies ist ein Element, das als Musikplayer fungieren kann; Sie können seine Einstellungen, sein Farbschema und sein Layout anpassen", 900f, 0.3f, 16);
                if (PlayerPrefs.GetInt("MAINLANG") == 3) touhou.AddTutorialHint(new Vector2(-450f, 0f), "Player muzical", "Acesta este un element care poate funcționa ca player muzical, puteți modifica setările, schema de culori și aspectul acestuia", 900f, 0.3f, 16);
            } if(Type == 12) {
                if (PlayerPrefs.GetInt("MUSPLAYWARN") == 0) { muswarn.SetActive(true);
                PlayerPrefs.SetInt("MUSPLAYWARN", 1); 
                }
            }
        } else { int lmpws = mpfs;
            GameObject NewELement = Instantiate(PrefsElements[Type], AllElements);
            if (Type == 0) NewELement.GetComponent<AnalogSpedometer>().ElementID = mpfs;
            if (Type == 1) NewELement.GetComponent<AnalogDisplay>().ElementID = mpfs;
            if (Type == 2) NewELement.GetComponent<LCDSpedometer>().ElementID = mpfs;
            if (Type == 3) NewELement.GetComponent<LCDDisplay>().ElementID = mpfs;
            if (Type == 4) NewELement.GetComponent<AnalogSpeedbar>().ElementID = mpfs;
            if (Type == 5) NewELement.GetComponent<CircSpeedBar>().ElementID = mpfs;
            if (Type == 6) NewELement.GetComponent<LCDSpeedbar>().ElementID = mpfs;
            if (Type == 7) NewELement.GetComponent<LCDCircSpeedbar>().ElementID = mpfs;
            if (Type == 8) NewELement.GetComponent<Line>().ElementID = mpfs;
            if (Type == 9) NewELement.GetComponent<TextComp>().ElementID = mpfs;
            if (Type == 10) NewELement.GetComponent<ImageComponent>().ElementID = mpfs;
            if (Type == 11) NewELement.GetComponent<Compass>().ElementID = mpfs;
            if (Type == 12) NewELement.GetComponent<MusicPlayer>().ElementID = mpfs;
            if (Type == 13) NewELement.GetComponent<MapSystem>().ElementID = mpfs;
            NewELement.GetComponent<SortOrderOrg>().ElementID = mpfs; NewELement.SetActive(true);
            NewELement.GetComponent<RectTransform>().anchoredPosition = new Vector2(PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + mpfs + "BASEPOSX",0f),
                PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + mpfs + "BASEPOSY", 0f));
            NewELement.transform.rotation = Quaternion.Euler(0f, 0f, PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + mpfs + "BASEROT", 0f));
            NewELement.transform.localScale = new Vector2(PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + mpfs + "BASESIZE", 1f), 
                PlayerPrefs.GetFloat("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + mpfs + "BASESIZE", 1f));
            if (!UsingMode) NewELement.transform.GetChild(NewELement.transform.childCount - 1).GetComponent<Button>().onClick.AddListener(() => OnElemSelect(lmpws));
            NewELement.name = mpfs + "";
        }
    }

    public void ContinueMusic() {
        muswarn.SetActive(false);
        muswarn2.SetActive(true);
    }

    public void GetPermisionForNTFCT() {
#if UNITY_ANDROID && !UNITY_EDITOR

    using (AndroidJavaClass unityPlayer =
           new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
    {
        AndroidJavaObject activity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject intent =
            new AndroidJavaObject(
                "android.content.Intent",
                "android.settings.ACTION_NOTIFICATION_LISTENER_SETTINGS");

        activity.Call("startActivity", intent);
    }

#endif
    
    }

    private IEnumerator ScreenShtASJepejepeg() {
        yield return new WaitForEndOfFrame();
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0); tex.Apply();
        byte[] jepejepeg = tex.EncodeToJPG(50);
        string path = Path.Combine(Application.persistentDataPath, "MYCOOLSCREENSHT" + PlayerPrefs.GetInt("SELECTEDPRESET") + ".jpg");
        File.WriteAllBytes(path, jepejepeg); Destroy(tex);
        PlayerPrefs.SetString("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "IMAGEPATH", path);
        SceneManager.LoadScene(0);
    }
}
