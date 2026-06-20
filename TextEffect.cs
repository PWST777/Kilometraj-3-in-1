using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextEffect : MonoBehaviour
{

    public string key;
    public ColorPicker CoPi;
    public string TexturePathB;
    public Texture2D wowtexB;
    public string TexturePathS;
    public Texture2D wowtexS;

    public TMP_Text FontBody;
    public TMP_Text FontStroke;
    public RectTransform canvas;
    public GameObject BodyUI;
    public Dropdown BColMod;
    public Dropdown BGradintT;
    public List<GameObject> BGradintModes;
    public List<RectTransform> BLeftObjects;
    public List<GameObject> BUIColorModes;
    public List<InputField> BTextureInputs;
    public RectTransform StorkeUIButton;
    public GameObject StrokeUI;
    public Dropdown SColMod;
    public Dropdown SGradintT;
    public List<GameObject> SGradintModes;
    public List<RectTransform> SLeftObjects;
    public List<GameObject> SUIColorModes;
    public List<InputField> STextureInputs;

    public Dropdown BodyColorMode;
    public Image BColorSolid;
    public Dropdown GradientTyper;
    public List<Image> Gradient2Cols;
    public List<Image> G2Colors;
    public List<Image> Gradient4Cols;
    public List<Image> G4Colors;
    public Slider BDilate;
    public Slider BSoftness;

    public Toggle StrokeT;
    public Dropdown StrokeColorMode;
    public Image SColorSolid;
    public Dropdown SGradientTyper;
    public List<Image> SGradient2Cols;
    public List<Image> SG2Colors;
    public List<Image> SGradient4Cols;
    public List<Image> SG4Colors;
    public Slider SDilate;
    public Slider SSoftness;

    public float RefrTime;
    public float LimitRef;
    public bool Qued;
    public bool LoadMode;
    public bool HasModded;
    public bool SceneMode;
    public TMP_Text prewarm;

    public List<TMP_FontAsset> regular;

    public GameObject xsend;

    public void OnTextEffectModTrigger(GameObject sender, int fontIndex, bool HasStroke, string sendKey) { LoadMode = true;
        xsend = sender; key = sendKey; gameObject.SetActive(true);
        FontBody.font = RequestRegular(fontIndex, false); FontStroke.font = RequestRegular(fontIndex, true);
        LoadMode = false;
    }

    void Start() {
        if(!SceneMode) OnColorModdify();
    }

    public TMP_FontAsset RequestRegular(int Index, bool Stroke) {
        TMP_FontAsset baseFont;
        if (!Stroke) baseFont = regular[Index * 2];
        else baseFont = regular[(Index * 2) + 1];
        TMP_FontAsset clone = Instantiate(baseFont);
        clone.material = new Material(baseFont.material);
        prewarm.font = clone;
        prewarm.text = " !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
        prewarm.ForceMeshUpdate();
        return clone;
    }

    public void OnUIConfigChanged(GameObject objt) { if(!LoadMode) { objt.SetActive(!objt.activeInHierarchy);
        canvas.sizeDelta = new Vector2(400f, 240f); StorkeUIButton.anchoredPosition = new Vector2(0f, -120f); StrokeUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -200f);
        BodyUI.GetComponent<RectTransform>().sizeDelta = new Vector2(400f, 660f); StrokeUI.GetComponent<RectTransform>().sizeDelta = new Vector2(400f, 660f);
        BLeftObjects[0].anchoredPosition = new Vector2(0f, -420f); BLeftObjects[1].anchoredPosition = new Vector2(0f, -570f);
        if (BodyUI.activeInHierarchy) { canvas.sizeDelta += new Vector2(0f, 620f); StorkeUIButton.anchoredPosition -= new Vector2(0f, 620f); StrokeUI.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 620f); 
        if (BColMod.value == 0) { BUIColorModes[0].SetActive(true); BUIColorModes[1].SetActive(false); BUIColorModes[2].SetActive(false); }
        if (BColMod.value == 1) { BUIColorModes[1].SetActive(true); BUIColorModes[0].SetActive(false); BUIColorModes[2].SetActive(false); canvas.sizeDelta += new Vector2(0f, 500f);
            BodyUI.GetComponent<RectTransform>().sizeDelta += new Vector2(0f, 500f); StorkeUIButton.anchoredPosition -= new Vector2(0f, 500f); StrokeUI.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 500f);
            foreach (RectTransform assx in BLeftObjects) assx.anchoredPosition -= new Vector2(0f, 500f); }
            if (GradientTyper.value == 2) { BGradintModes[1].SetActive(true); BGradintModes[0].SetActive(false); } else { BGradintModes[0].SetActive(true); BGradintModes[1].SetActive(false); }
            if (BColMod.value == 2) { BUIColorModes[2].SetActive(true); BUIColorModes[0].SetActive(false); BUIColorModes[1].SetActive(false); canvas.sizeDelta += new Vector2(0f, 650f);
            BodyUI.GetComponent<RectTransform>().sizeDelta += new Vector2(0f, 650f); StorkeUIButton.anchoredPosition -= new Vector2(0f, 650f); StrokeUI.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 650f);
            foreach (RectTransform assx in BLeftObjects) assx.anchoredPosition -= new Vector2(0f, 650f); } }
        if (StrokeUI.activeInHierarchy) { canvas.sizeDelta += new Vector2(0f, 620f); 
        SLeftObjects[0].anchoredPosition = new Vector2(0f, -420f); SLeftObjects[1].anchoredPosition = new Vector2(0f, -570f);
        if (SColMod.value == 0) { SUIColorModes[0].SetActive(true); SUIColorModes[1].SetActive(false); SUIColorModes[2].SetActive(false); }
        if (SColMod.value == 1) { SUIColorModes[1].SetActive(true); SUIColorModes[0].SetActive(false); SUIColorModes[2].SetActive(false); canvas.sizeDelta += new Vector2(0f, 500f);
            StrokeUI.GetComponent<RectTransform>().sizeDelta += new Vector2(0f, 500f); foreach (RectTransform assx in SLeftObjects) assx.anchoredPosition -= new Vector2(0f, 500f); }
            if (SGradientTyper.value == 2) { SGradintModes[1].SetActive(true); SGradintModes[0].SetActive(false); } else { SGradintModes[0].SetActive(true); SGradintModes[1].SetActive(false); }
            if (SColMod.value == 2) { SUIColorModes[2].SetActive(true); SUIColorModes[0].SetActive(false); SUIColorModes[1].SetActive(false); canvas.sizeDelta += new Vector2(0f, 650f);
            StrokeUI.GetComponent<RectTransform>().sizeDelta += new Vector2(0f, 650f); foreach (RectTransform assx in SLeftObjects) assx.anchoredPosition -= new Vector2(0f, 650f); } } }
    }

    public void OnFontSetChanged() { Qued = true; if(RefrTime >= LimitRef) { if(Qued) { HasModded = true;
        if (BodyColorMode.value == 0) { FontBody.color = BColorSolid.color; FontBody.enableVertexGradient = false; } Qued = false; RefrTime = 0f;
        if (BodyColorMode.value == 1) { FontBody.enableVertexGradient = true; FontBody.color = Color.white;
            if (GradientTyper.value == 0) FontBody.colorGradient = new VertexGradient(Gradient2Cols[0].color, Gradient2Cols[1].color, Gradient2Cols[0].color, Gradient2Cols[1].color);
            if (GradientTyper.value == 1) FontBody.colorGradient = new VertexGradient(Gradient2Cols[0].color, Gradient2Cols[0].color, Gradient2Cols[1].color, Gradient2Cols[1].color);
            if (GradientTyper.value == 2) FontBody.colorGradient = new VertexGradient(Gradient4Cols[0].color, Gradient4Cols[1].color, Gradient4Cols[2].color, Gradient4Cols[3].color); }
                FontBody.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, BDilate.value); FontBody.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, BSoftness.value);
            FontBody.UpdateMeshPadding(); FontBody.havePropertiesChanged = true; FontBody.SetMaterialDirty();
        if (StrokeT.isOn) { FontStroke.gameObject.SetActive(true);
            if (StrokeColorMode.value == 0) { FontStroke.color = SColorSolid.color; FontStroke.enableVertexGradient = false; }
            if (StrokeColorMode.value == 1) { FontStroke.enableVertexGradient = true; FontStroke.color = Color.white;
                if (SGradientTyper.value == 0) FontStroke.colorGradient = new VertexGradient(SGradient2Cols[0].color, SGradient2Cols[1].color, SGradient2Cols[0].color, SGradient2Cols[1].color);
                if (SGradientTyper.value == 1) FontStroke.colorGradient = new VertexGradient(SGradient2Cols[0].color, SGradient2Cols[0].color, SGradient2Cols[1].color, SGradient2Cols[1].color);
                if (SGradientTyper.value == 2) FontStroke.colorGradient = new VertexGradient(SGradient4Cols[0].color, SGradient4Cols[1].color, SGradient4Cols[2].color, SGradient4Cols[3].color); }
                    FontStroke.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, SDilate.value); FontStroke.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineSoftness, SSoftness.value);
                FontStroke.UpdateMeshPadding(); FontStroke.havePropertiesChanged = true; FontStroke.SetMaterialDirty();
                } else FontStroke.gameObject.SetActive(false);
            } }
    }

    public void OnColorModdify() {
        BColorSolid.color = new Color(PlayerPrefs.GetFloat(key + "BTXTCOLR", 1f), PlayerPrefs.GetFloat(key + "BTXTCOLG", 1f), PlayerPrefs.GetFloat(key + "BTXTCOLB", 1f));
        Gradient2Cols[0].color = new Color(PlayerPrefs.GetFloat(key + "BTXTG21R", 1f), PlayerPrefs.GetFloat(key + "BTXTG21G", 0f), PlayerPrefs.GetFloat(key + "BTXTG21B", 0f));
        Gradient2Cols[1].color = new Color(PlayerPrefs.GetFloat(key + "BTXTG22R", 1f), PlayerPrefs.GetFloat(key + "BTXTG22G", 1f), PlayerPrefs.GetFloat(key + "BTXTG22B", 0f));
        Gradient4Cols[0].color = new Color(PlayerPrefs.GetFloat(key + "BTXTG41R", 1f), PlayerPrefs.GetFloat(key + "BTXTG41G", 0f), PlayerPrefs.GetFloat(key + "BTXTG41B", 0f));
        Gradient4Cols[1].color = new Color(PlayerPrefs.GetFloat(key + "BTXTG42R", 1f), PlayerPrefs.GetFloat(key + "BTXTG42G", 1f), PlayerPrefs.GetFloat(key + "BTXTG42B", 0f));
        Gradient4Cols[2].color = new Color(PlayerPrefs.GetFloat(key + "BTXTG43R", 0f), PlayerPrefs.GetFloat(key + "BTXTG43G", 1f), PlayerPrefs.GetFloat(key + "BTXTG43B", 0f));
        Gradient4Cols[3].color = new Color(PlayerPrefs.GetFloat(key + "BTXTG44R", 0f), PlayerPrefs.GetFloat(key + "BTXTG44G", 1f), PlayerPrefs.GetFloat(key + "BTXTG44B", 1f));
        G2Colors[0].color = Gradient2Cols[0].color; G2Colors[1].color = Gradient2Cols[1].color; G4Colors[0].color = Gradient4Cols[0].color;
        G4Colors[1].color = Gradient4Cols[1].color; G4Colors[2].color = Gradient4Cols[2].color; G4Colors[3].color = Gradient4Cols[3].color;
        if (BodyColorMode.value == 0) { FontBody.color = BColorSolid.color; FontBody.enableVertexGradient = false; }
        if (BodyColorMode.value == 1) { FontBody.enableVertexGradient = true;
            if (GradientTyper.value == 0) FontBody.colorGradient = new VertexGradient(Gradient2Cols[0].color, Gradient2Cols[1].color, Gradient2Cols[0].color, Gradient2Cols[1].color);
            if (GradientTyper.value == 1) FontBody.colorGradient = new VertexGradient(Gradient2Cols[0].color, Gradient2Cols[0].color, Gradient2Cols[1].color, Gradient2Cols[1].color);
            if (GradientTyper.value == 2) FontBody.colorGradient = new VertexGradient(Gradient4Cols[0].color, Gradient4Cols[1].color, Gradient4Cols[2].color, Gradient4Cols[3].color);
        }

        SColorSolid.color = new Color(PlayerPrefs.GetFloat(key + "STXTCOLR", 1f), PlayerPrefs.GetFloat(key + "STXTCOLG", 1f), PlayerPrefs.GetFloat(key + "STXTCOLB", 1f));
        SGradient2Cols[0].color = new Color(PlayerPrefs.GetFloat(key + "STXTG21R", 1f), PlayerPrefs.GetFloat(key + "STXTG21G", 0f), PlayerPrefs.GetFloat(key + "STXTG21B", 0f));
        SGradient2Cols[1].color = new Color(PlayerPrefs.GetFloat(key + "STXTG22R", 1f), PlayerPrefs.GetFloat(key + "STXTG22G", 1f), PlayerPrefs.GetFloat(key + "STXTG22B", 0f));
        SGradient4Cols[0].color = new Color(PlayerPrefs.GetFloat(key + "STXTG41R", 1f), PlayerPrefs.GetFloat(key + "STXTG41G", 0f), PlayerPrefs.GetFloat(key + "STXTG41B", 0f));
        SGradient4Cols[1].color = new Color(PlayerPrefs.GetFloat(key + "STXTG42R", 1f), PlayerPrefs.GetFloat(key + "STXTG42G", 1f), PlayerPrefs.GetFloat(key + "STXTG42B", 0f));
        SGradient4Cols[2].color = new Color(PlayerPrefs.GetFloat(key + "STXTG43R", 0f), PlayerPrefs.GetFloat(key + "STXTG43G", 1f), PlayerPrefs.GetFloat(key + "STXTG43B", 0f));
        SGradient4Cols[3].color = new Color(PlayerPrefs.GetFloat(key + "STXTG44R", 0f), PlayerPrefs.GetFloat(key + "STXTG44G", 1f), PlayerPrefs.GetFloat(key + "STXTG44B", 1f));
        SG2Colors[0].color = SGradient2Cols[0].color; SG2Colors[1].color = SGradient2Cols[1].color; SG4Colors[0].color = SGradient4Cols[0].color;
        SG4Colors[1].color = SGradient4Cols[1].color; SG4Colors[2].color = SGradient4Cols[2].color; SG4Colors[3].color = SGradient4Cols[3].color;
        if (StrokeColorMode.value == 0) { FontStroke.color = SColorSolid.color; FontStroke.enableVertexGradient = false; }
        if (StrokeColorMode.value == 1) { FontStroke.enableVertexGradient = true;
            if (SGradientTyper.value == 0) FontStroke.colorGradient = new VertexGradient(SGradient2Cols[0].color, SGradient2Cols[1].color, SGradient2Cols[0].color, SGradient2Cols[1].color);
            if (SGradientTyper.value == 1) FontStroke.colorGradient = new VertexGradient(SGradient2Cols[0].color, SGradient2Cols[0].color, SGradient2Cols[1].color, SGradient2Cols[1].color);
            if (SGradientTyper.value == 2) FontStroke.colorGradient = new VertexGradient(SGradient4Cols[0].color, SGradient4Cols[1].color, SGradient4Cols[2].color, SGradient4Cols[3].color);
        }
    }

    public void ApplyMaterial() {
        PlayerPrefs.SetInt(key + "TMPMBCOLMO", BodyColorMode.value); PlayerPrefs.SetInt(key + "TMPMSCOLMO", StrokeColorMode.value);
        PlayerPrefs.SetInt(key + "TMPMBGRTYP", GradientTyper.value); PlayerPrefs.SetInt(key + "TMPMSGRTYP", SGradientTyper.value);
        PlayerPrefs.SetFloat(key + "TMPMBDILAT", BDilate.value); PlayerPrefs.SetFloat(key + "TMPMSDILAT", SDilate.value);
        PlayerPrefs.SetFloat(key + "TMPMBSOFTN", BSoftness.value); PlayerPrefs.SetFloat(key + "TMPMSSOFTN", SSoftness.value);
        if(StrokeT.isOn) PlayerPrefs.SetInt(key + "TMPMSTROKE", 1); else PlayerPrefs.SetInt(key + "TMPMSTROKE", 0);
        if (HasModded) PlayerPrefs.SetInt(key + "TMPMEFFECT", 1);
        if (xsend.GetComponent<TextComp>()) xsend.GetComponent<TextComp>().LoadEffect();
    }

    public void TriggerColorChange(string EndKey) { HasModded = true;
        CoPi.RequestChangeColor(key + EndKey, gameObject);
    }

    void Update() {
        RefrTime += Time.deltaTime; if (Qued) if (RefrTime >= LimitRef) OnFontSetChanged();
    }
}
