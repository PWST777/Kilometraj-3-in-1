using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SkinSelect : MonoBehaviour
{

    public ColorPicker nocp;
    public CustomTextureUniversal ctu;
    public List<GameObject> metalics;
    public List<Vector2> positions;
    public List<Vector2> sizes;
    public List<Color> alpha;
    public List<Sprite> metalicTs;
    public List<string> namesSkins;
    public Sprite defalutSprite;
    public Text nameDis;
    public float progress;
    public int animations;
    public int selctIndex;
    public int lastselctIndex;
    public float rainbowProgress;

    public List<Image> texModes;
    public List<Sprite> spritesGLOSS;
    public List<Image> preview;
    public Image mainColor;
    public Image glossColor;
    public List<Image> directions;
    public List<Sprite> pr2;
    public int glossS;
    public int glossDir;
    public int lastSentKey;

    void Start() {
        selctIndex = PlayerPrefs.GetInt("SKINMETAL");
        nameDis.text = namesSkins[selctIndex];
        ctu = GameObject.Find("UNIVERSALSKIN").GetComponent<CustomTextureUniversal>();
        UpdateTextures(); LoadDataStart();
    }

    public void MoveLeft() { UpdateTextures();
        animations = 0; progress = 0; lastselctIndex = selctIndex;
        selctIndex++; selctIndex %= metalicTs.Count;
    }

    public void MoveRight() { UpdateTextures();
        animations = 1; progress = 0; lastselctIndex = selctIndex;
        selctIndex--; if (selctIndex < 0) selctIndex += metalicTs.Count;
    }

    public void ApplyMetalic() {
        PlayerPrefs.SetInt("USKIN", 0);
        PlayerPrefs.SetInt("SKINMETAL", selctIndex);
        if (selctIndex == 10) { PlayerPrefs.SetInt("UCTEXTUREEXIST", 1); ctu.RegenCustomTexture(); }
        SceneManager.LoadScene(0);
    }

    public void LoadDataStart() {
        ChangeGlossTex(PlayerPrefs.GetInt("UTEXTUREGLOSSTEX", 0));
        ChangeDirection(PlayerPrefs.GetInt("UTEXTUREGLOSSDIR", 2));
        mainColor.color = new Color(PlayerPrefs.GetFloat("MYUWUWU0R", 0f), PlayerPrefs.GetFloat("MYUWUWU0G", 0f), PlayerPrefs.GetFloat("MYUWUWU0B", 0f));
        glossColor.color = new Color(PlayerPrefs.GetFloat("MYUWUWU1R", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1G", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1B", 0.4f));
        RefPreview();
    }

    public void RefPreview() {
        preview[0].color = mainColor.color;
        preview[1].color = glossColor.color;
        preview[1].gameObject.SetActive((glossS == 0));
        preview[2].sprite = spritesGLOSS[glossS];
        if (glossS == 1) preview[2].type = Image.Type.Tiled; else preview[2].type = Image.Type.Sliced;
        preview[2].GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, 180f - (glossDir * 90f));
    }

    public void ChangeGlossTex(int glossSs) { glossS = glossSs;
        int SavedTexture = PlayerPrefs.GetInt("SKINMETAL");
        if(glossSs == 0) {
            texModes[0].sprite = metalicTs[SavedTexture];
            if (SavedTexture == 3 || SavedTexture == 5 || SavedTexture == 7) texModes[0].type = Image.Type.Tiled; else texModes[0].type = Image.Type.Sliced;
            texModes[1].sprite = defalutSprite; texModes[1].type = Image.Type.Sliced;
            glossColor.transform.parent.GetComponent<Button>().interactable = true;
        } else if(glossSs == 1) {
            texModes[1].sprite = metalicTs[SavedTexture];
            if (SavedTexture == 3 || SavedTexture == 5 || SavedTexture == 7) texModes[1].type = Image.Type.Tiled; else texModes[1].type = Image.Type.Sliced;
            texModes[0].sprite = defalutSprite; texModes[0].type = Image.Type.Sliced;
            glossColor.color = Color.Lerp(mainColor.color, Color.white, 0.7f);
            PlayerPrefs.SetFloat("MYUWUWU1R", Mathf.Lerp(mainColor.color.r, 1f, 0.7f));
            PlayerPrefs.SetFloat("MYUWUWU1G", Mathf.Lerp(mainColor.color.g, 1f, 0.7f));
            PlayerPrefs.SetFloat("MYUWUWU1B", Mathf.Lerp(mainColor.color.b, 1f, 0.7f));
            glossColor.transform.parent.GetComponent<Button>().interactable = false;
        } PlayerPrefs.SetInt("UTEXTUREGLOSSTEX", glossSs);
        RefPreview();
    }

    public void ChangeDirection(int Direction) {
        for(int i = 0; i < 4; i++) {
            if (Direction == i) directions[i].sprite = pr2[1];
            else directions[i].sprite = pr2[0];
        } PlayerPrefs.SetInt("UTEXTUREGLOSSDIR", Direction);
        glossDir = Direction; RefPreview();
    }

    public void SetColor(int key) {
        string reolkey = "MYUWUWU" + key;
        nocp.RequestChangeColor(reolkey, gameObject);
        lastSentKey = key;
    }

    public void OnColorModdify() {
        if (lastSentKey == 0) mainColor.color = new Color(PlayerPrefs.GetFloat("MYUWUWU0R", 0f), PlayerPrefs.GetFloat("MYUWUWU0G", 0f), PlayerPrefs.GetFloat("MYUWUWU0B", 0f));
        if (lastSentKey == 1) glossColor.color = new Color(PlayerPrefs.GetFloat("MYUWUWU1R", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1G", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1B", 0.4f));
        if (glossS == 1) { glossColor.color = Color.Lerp(mainColor.color, Color.white, 0.7f);
            PlayerPrefs.SetFloat("MYUWUWU1R", Mathf.Lerp(mainColor.color.r, 1f, 0.7f));
            PlayerPrefs.SetFloat("MYUWUWU1G", Mathf.Lerp(mainColor.color.g, 1f, 0.7f));
            PlayerPrefs.SetFloat("MYUWUWU1B", Mathf.Lerp(mainColor.color.b, 1f, 0.7f)); }
        RefPreview();
    }

    public void UpdateTextures() {
        metalics[0].GetComponent<RectTransform>().anchoredPosition = positions[0];
        metalics[0].GetComponent<RectTransform>().localScale = sizes[0];
        metalics[0].transform.GetChild(0).GetComponent<Image>().color = alpha[0];
        metalics[1].GetComponent<RectTransform>().anchoredPosition = positions[1];
        metalics[1].GetComponent<RectTransform>().localScale = sizes[1];
        metalics[1].transform.GetChild(0).GetComponent<Image>().color = alpha[1];
        metalics[2].GetComponent<RectTransform>().anchoredPosition = positions[2];
        metalics[2].GetComponent<RectTransform>().localScale = sizes[2];
        metalics[2].transform.GetChild(0).GetComponent<Image>().color = alpha[2];
        metalics[3].GetComponent<RectTransform>().anchoredPosition = positions[3];
        metalics[3].GetComponent<RectTransform>().localScale = sizes[3];
        metalics[3].transform.GetChild(0).GetComponent<Image>().color = alpha[3];
        metalics[4].GetComponent<RectTransform>().anchoredPosition = positions[4];
        metalics[4].GetComponent<RectTransform>().localScale = sizes[4];
        metalics[4].transform.GetChild(0).GetComponent<Image>().color = alpha[4];
        metalics[5].GetComponent<RectTransform>().anchoredPosition = positions[5];
        metalics[5].GetComponent<RectTransform>().localScale = sizes[5];
        metalics[5].transform.GetChild(0).GetComponent<Image>().color = alpha[5];
        metalics[6].GetComponent<RectTransform>().anchoredPosition = positions[6];
        metalics[6].GetComponent<RectTransform>().localScale = sizes[6];
        metalics[6].transform.GetChild(0).GetComponent<Image>().color = alpha[6];
        metalics[2].transform.GetChild(0).GetComponent<Image>().sprite = metalicTs[selctIndex];
        if(selctIndex == 3 || selctIndex == 5 || selctIndex == 7) { metalics[2].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Tiled; }
        else { metalics[2].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced; } int calc = (selctIndex < 1) ? ((selctIndex - 1) + metalicTs.Count) : (selctIndex - 1);
        if (calc == 3 || calc == 5 || calc == 7) { metalics[1].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Tiled; }
        else { metalics[1].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced; }
        metalics[1].transform.GetChild(0).GetComponent<Image>().sprite = metalicTs[calc]; calc = (selctIndex < 2) ? ((selctIndex - 2) + metalicTs.Count) : (selctIndex - 2);
        if (calc == 3 || calc == 5 || calc == 7) { metalics[0].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Tiled; }
        else { metalics[0].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced; }
        metalics[0].transform.GetChild(0).GetComponent<Image>().sprite = metalicTs[calc]; calc = (selctIndex < 3) ? ((selctIndex - 3) + metalicTs.Count) : (selctIndex - 3);
        if (calc == 3 || calc == 5 || calc == 7) { metalics[5].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Tiled; }
        else { metalics[5].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced; }
        metalics[5].transform.GetChild(0).GetComponent<Image>().sprite = metalicTs[calc]; calc = (selctIndex + 1) % metalicTs.Count;
        if (calc == 3 || calc == 5 || calc == 7) { metalics[3].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Tiled; }
        else { metalics[3].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced; }
        metalics[3].transform.GetChild(0).GetComponent<Image>().sprite = metalicTs[calc]; calc = (selctIndex + 2) % metalicTs.Count;
        if (calc == 3 || calc == 5 || calc == 7) { metalics[4].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Tiled; }
        else { metalics[4].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced; }
        metalics[4].transform.GetChild(0).GetComponent<Image>().sprite = metalicTs[calc]; calc = (selctIndex + 3) % metalicTs.Count;
        if (calc == 3 || calc == 5 || calc == 7) { metalics[6].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Tiled; }
        else { metalics[6].transform.GetChild(0).GetComponent<Image>().type = Image.Type.Sliced; }
        metalics[6].transform.GetChild(0).GetComponent<Image>().sprite = metalicTs[calc];
    }

    void Update() { progress += Time.deltaTime;
        if(progress < 0.25f) {
            if (progress < 0.105f) { nameDis.text = namesSkins[lastselctIndex]; nameDis.color = new Color(1f, 1f, 1f, 1f - (progress * 11f)); }
            else if (progress < 0.21f) { nameDis.text = namesSkins[selctIndex]; nameDis.color = new Color(1f, 1f, 1f, (progress * 10f) - 1f); }
            if(animations == 0) {
                float progresssqr = 1f - Mathf.Pow(0.6f, (progress * 40f));
                metalics[0].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(positions[0], positions[5], progresssqr);
                metalics[0].GetComponent<RectTransform>().localScale = Vector2.Lerp(sizes[0], sizes[5], progresssqr);
                metalics[0].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(alpha[0], alpha[5], progresssqr);
                metalics[1].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(positions[1], positions[0], progresssqr);
                metalics[1].GetComponent<RectTransform>().localScale = Vector2.Lerp(sizes[1], sizes[0], progresssqr);
                metalics[1].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(alpha[1], alpha[0], progresssqr);
                metalics[2].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(positions[2], positions[1], progresssqr);
                metalics[2].GetComponent<RectTransform>().localScale = Vector2.Lerp(sizes[2], sizes[1], progresssqr);
                metalics[2].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(alpha[2], alpha[1], progresssqr);
                metalics[3].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(positions[3], positions[2], progresssqr);
                metalics[3].GetComponent<RectTransform>().localScale = Vector2.Lerp(sizes[3], sizes[2], progresssqr);
                metalics[3].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(alpha[3], alpha[2], progresssqr);
                metalics[4].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(positions[4], positions[3], progresssqr);
                metalics[4].GetComponent<RectTransform>().localScale = Vector2.Lerp(sizes[4], sizes[3], progresssqr);
                metalics[4].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(alpha[4], alpha[3], progresssqr);
                metalics[6].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(positions[6], positions[4], progresssqr);
                metalics[6].GetComponent<RectTransform>().localScale = Vector2.Lerp(sizes[6], sizes[4], progresssqr);
                metalics[6].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(alpha[6], alpha[4], progresssqr);
            } else if(animations == 1) {
                float progresssqr = 1f - Mathf.Pow(0.6f, (progress * 40f));
                metalics[0].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(positions[0], positions[1], progresssqr);
                metalics[0].GetComponent<RectTransform>().localScale = Vector2.Lerp(sizes[0], sizes[1], progresssqr);
                metalics[0].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(alpha[0], alpha[1], progresssqr);
                metalics[1].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(positions[1], positions[2], progresssqr);
                metalics[1].GetComponent<RectTransform>().localScale = Vector2.Lerp(sizes[1], sizes[2], progresssqr);
                metalics[1].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(alpha[1], alpha[2], progresssqr);
                metalics[2].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(positions[2], positions[3], progresssqr);
                metalics[2].GetComponent<RectTransform>().localScale = Vector2.Lerp(sizes[2], sizes[3], progresssqr);
                metalics[2].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(alpha[2], alpha[3], progresssqr);
                metalics[3].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(positions[3], positions[4], progresssqr);
                metalics[3].GetComponent<RectTransform>().localScale = Vector2.Lerp(sizes[3], sizes[4], progresssqr);
                metalics[3].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(alpha[3], alpha[4], progresssqr);
                metalics[4].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(positions[4], positions[6], progresssqr);
                metalics[4].GetComponent<RectTransform>().localScale = Vector2.Lerp(sizes[4], sizes[6], progresssqr);
                metalics[4].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(alpha[4], alpha[6], progresssqr);
                metalics[5].GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(positions[5], positions[0], progresssqr);
                metalics[5].GetComponent<RectTransform>().localScale = Vector2.Lerp(sizes[5], sizes[0], progresssqr);
                metalics[5].transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(alpha[5], alpha[0], progresssqr);
            } 
        } else { 
            if(animations != -1) {
                UpdateTextures();
            }  animations = -1;
            if(selctIndex == 10) {
                if(progress % 2f > 1.75f) {
                    if(progress % 4f < 2f) { 
                    if (progress % 2f < 1.855f) { nameDis.text = "Custom Skin"; nameDis.color = new Color(1f, 1f, 1f, 1f - (((progress % 4f) - 1.75f) * 11f)); }
                    else if (progress % 2f < 1.97f) { nameDis.text = "Tap to moddify"; nameDis.color = new Color(1f, 1f, 1f, (((progress % 4f) - 1.75f) * 10f) - 1f); } }
                    else { if (progress % 2f < 1.855f) { nameDis.text = "Tap to moddify"; nameDis.color = new Color(1f, 1f, 1f, 1f - (((progress % 4f) - 3.75f) * 11f)); }
                    else if (progress % 2f < 1.97f) { nameDis.text = "Custom Skin"; nameDis.color = new Color(1f, 1f, 1f, (((progress % 4f) - 3.75f) * 10f) - 1f); } }
                }
            }
        } rainbowProgress += Time.deltaTime * 0.3f; rainbowProgress %= 1f; 
        for(int i = 0; i < metalics.Count; i++) {
            Image comp = metalics[i].transform.GetChild(0).GetComponent<Image>();
            if (comp.sprite == metalicTs[9]) { float compalpha = comp.color.a;
                comp.color = Color.HSVToRGB(rainbowProgress, 0.6f, 1f);
                comp.color = new Color(comp.color.r, comp.color.g, comp.color.b, compalpha); } 
            GameObject comp2 = metalics[i].transform.GetChild(1).gameObject;
            if (comp.sprite == metalicTs[10]) { if (!comp2.gameObject.activeInHierarchy) comp2.gameObject.SetActive(true); metalics[i].GetComponent<CustomTextureMenu>().Selected = true; }
            else { metalics[i].GetComponent<CustomTextureMenu>().Selected = false; if (comp2.gameObject.activeInHierarchy) { comp2.gameObject.SetActive(false); comp.gameObject.SetActive(true); } } 
        }
    }
}
