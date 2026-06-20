using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinTextureReplace : MonoBehaviour
{
    
    public enum ModeOperation {
        TextureDark, TextureLight, TextureDetail,
        Title_W_Version, TitleReg, TitleTransReg,
        TitleP1, TitleP2
    }

    public ModeOperation mainMode;
    public List<Sprite> Metalics;
    public CustomTextureUniversal ctu;
    List<Color32> colrs;
    List<Color32> colrsSecondary;
    string version = "v1.3a";
    float rainBowprogress;
    int skinSelected;

    void Start() {
        #region Color Define
        colrs = new List<Color32>();
        colrsSecondary = new List<Color32>();
        colrs.Add(new Color32(108, 46, 21, 255));
        colrs.Add(new Color32(255, 235, 50, 255));
        colrs.Add(new Color32(5, 188, 8, 255));
        colrs.Add(new Color32(0, 195, 255, 255));
        colrs.Add(new Color32(0, 73, 172, 255));
        colrs.Add(new Color32(163, 0, 254, 255));
        colrs.Add(new Color32(255, 68, 240, 255));
        colrs.Add(new Color32(100, 179, 152, 255));
        colrs.Add(new Color32(255, 0, 0, 255));
        colrs.Add(new Color32(255, 255, 255, 255));
        colrs.Add(new Color32(0, 0, 0, 255));
        colrs.Add(new Color32(231, 5, 5, 255));
        colrsSecondary.Add(new Color32(250, 216, 197, 255));
        colrsSecondary.Add(new Color32(255, 244, 196, 255));
        colrsSecondary.Add(new Color32(136, 252, 130, 255));
        colrsSecondary.Add(new Color32(255, 255, 255, 255));
        colrsSecondary.Add(new Color32(130, 183, 254, 255));
        colrsSecondary.Add(new Color32(255, 255, 255, 255));
        colrsSecondary.Add(new Color32(255, 147, 243, 255));
        colrsSecondary.Add(new Color32(255, 255, 255, 255));
        colrsSecondary.Add(new Color32(0, 255, 0, 255));
        colrsSecondary.Add(new Color32(180, 180, 180, 255));
        colrsSecondary.Add(new Color32(100, 100, 100, 255));
        colrsSecondary.Add(new Color32(251, 129, 129, 255));
        #endregion
        ctu = GameObject.Find("UNIVERSALSKIN").GetComponent<CustomTextureUniversal>();
        if (mainMode == ModeOperation.TextureDetail) {
            if(PlayerPrefs.GetInt("USKIN") == 0) {
                int Textured = PlayerPrefs.GetInt("SKINMETAL"); skinSelected = Textured;
                if (Textured != 10) { 
                gameObject.GetComponent<Image>().sprite = Metalics[Textured];
                if(Textured == 3 || Textured == 5 || Textured == 7) {
                    gameObject.GetComponent<Image>().type = Image.Type.Tiled;
                } else { gameObject.GetComponent<Image>().type = Image.Type.Sliced; } }
                else {
                    gameObject.GetComponent<Image>().sprite = ctu.CustomImgTexture;
                    if(PlayerPrefs.GetInt("UTEXTUREGLOSSTEX", 0) == 1) {
                        gameObject.GetComponent<Image>().type = Image.Type.Tiled;
                    } else { gameObject.GetComponent<Image>().type = Image.Type.Sliced; }
                } 
            }
        } else if(mainMode == ModeOperation.Title_W_Version) {
            if(PlayerPrefs.GetInt("USKIN") == 0) {
                int Textured = PlayerPrefs.GetInt("SKINMETAL"); skinSelected = Textured; bool ct = (Textured == 10);
                Color mainColor = new Color(PlayerPrefs.GetFloat("MYUWUWU0R", 0f), PlayerPrefs.GetFloat("MYUWUWU0G", 0f), PlayerPrefs.GetFloat("MYUWUWU0B", 0f));
                Color glossColor = new Color(PlayerPrefs.GetFloat("MYUWUWU1R", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1G", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1B", 0.4f));
                int texture = PlayerPrefs.GetInt("UTEXTUREGLOSSTEX", 0);
                gameObject.transform.GetChild(0).GetComponent<Image>().color = ct ? mainColor : colrs[Textured];
                if((!ct && (Textured == 3 || Textured == 5 || Textured == 7)) || (ct && texture == 1)) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    gameObject.transform.GetChild(2).gameObject.SetActive(true);
                } else {
                    gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gameObject.transform.GetChild(1).GetComponent<Image>().color = ct ? glossColor : colrsSecondary[Textured];
                }
                gameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>().colorGradient =
                    new VertexGradient(ct ? mainColor : colrs[Textured], ct ? glossColor : colrsSecondary[Textured], ct ? glossColor : colrsSecondary[Textured], ct ? mainColor : colrs[Textured]);
                gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = version;
                gameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = version;
                gameObject.GetComponent<Image>().color = Color.Lerp(ct ? mainColor : colrs[Textured], Color.black, 0.7f);
                gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.Lerp(ct ? mainColor : colrs[Textured], Color.black, 0.7f);
            }
        } else if(mainMode == ModeOperation.TitleReg) {
            if(PlayerPrefs.GetInt("USKIN") == 0) {
                int Textured = PlayerPrefs.GetInt("SKINMETAL"); skinSelected = Textured; bool ct = (Textured == 10);
                Color mainColor = new Color(PlayerPrefs.GetFloat("MYUWUWU0R", 0f), PlayerPrefs.GetFloat("MYUWUWU0G", 0f), PlayerPrefs.GetFloat("MYUWUWU0B", 0f));
                Color glossColor = new Color(PlayerPrefs.GetFloat("MYUWUWU1R", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1G", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1B", 0.4f));
                int texture = PlayerPrefs.GetInt("UTEXTUREGLOSSTEX", 0);
                gameObject.transform.GetChild(0).GetComponent<Image>().color = ct ? mainColor : colrs[Textured];
                if((!ct && (Textured == 3 || Textured == 5 || Textured == 7)) || (ct && texture == 1)) {
                    gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    gameObject.transform.GetChild(2).gameObject.SetActive(true);
                } else {
                    gameObject.transform.GetChild(2).gameObject.SetActive(false);
                    gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gameObject.transform.GetChild(1).GetComponent<Image>().color = ct ? glossColor : colrsSecondary[Textured];
                } gameObject.GetComponent<Image>().color = Color.Lerp(ct ? mainColor : colrs[Textured], Color.black, 0.7f);
            }
        } else if(mainMode == ModeOperation.TitleTransReg) {
            if(PlayerPrefs.GetInt("USKIN") == 0) {
                int Textured = PlayerPrefs.GetInt("SKINMETAL"); skinSelected = Textured; bool ct = (Textured == 10);
                Color mainColor = new Color(PlayerPrefs.GetFloat("MYUWUWU0R", 0f), PlayerPrefs.GetFloat("MYUWUWU0G", 0f), PlayerPrefs.GetFloat("MYUWUWU0B", 0f));
                Color glossColor = new Color(PlayerPrefs.GetFloat("MYUWUWU1R", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1G", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1B", 0.4f));
                gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontMaterial.SetColor(ShaderUtilities.ID_FaceColor, Color.Lerp(ct ? mainColor : colrs[Textured], Color.black, 0.7f)); 
                gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().colorGradient =
                    new VertexGradient(ct ? mainColor : colrs[Textured], ct ? glossColor : colrsSecondary[Textured], ct ? glossColor : colrsSecondary[Textured], ct ? mainColor : colrs[Textured]);
            }
        } else if(mainMode == ModeOperation.TitleP1) {
            if(PlayerPrefs.GetInt("USKIN") == 0) {
                int Textured = PlayerPrefs.GetInt("SKINMETAL"); skinSelected = Textured; bool ct = (Textured == 10);
                Color mainColor = new Color(PlayerPrefs.GetFloat("MYUWUWU0R", 0f), PlayerPrefs.GetFloat("MYUWUWU0G", 0f), PlayerPrefs.GetFloat("MYUWUWU0B", 0f));
                Color glossColor = new Color(PlayerPrefs.GetFloat("MYUWUWU1R", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1G", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1B", 0.4f));
                gameObject.GetComponent<TextMeshProUGUI>().fontMaterial.SetColor(ShaderUtilities.ID_FaceColor, Color.Lerp(ct ? mainColor : colrs[Textured], Color.black, 0.7f)); 
            }
        } else if(mainMode == ModeOperation.TitleP2) {
            if(PlayerPrefs.GetInt("USKIN") == 0) {
                int Textured = PlayerPrefs.GetInt("SKINMETAL"); skinSelected = Textured; bool ct = (Textured == 10);
                Color mainColor = new Color(PlayerPrefs.GetFloat("MYUWUWU0R", 0f), PlayerPrefs.GetFloat("MYUWUWU0G", 0f), PlayerPrefs.GetFloat("MYUWUWU0B", 0f));
                Color glossColor = new Color(PlayerPrefs.GetFloat("MYUWUWU1R", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1G", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1B", 0.4f));
                gameObject.GetComponent<TextMeshProUGUI>().colorGradient =
                    new VertexGradient(ct ? mainColor : colrs[Textured], ct ? glossColor : colrsSecondary[Textured], ct ? glossColor : colrsSecondary[Textured], ct ? mainColor : colrs[Textured]);
            }
        }
    }

    // Update is called once per frame
    void Update() {
        if(skinSelected == 9) { rainBowprogress += Time.deltaTime * 0.3f; if (rainBowprogress >= 1f) rainBowprogress -= 1f;
            if (mainMode == ModeOperation.TextureDetail) { Color h = Color.HSVToRGB(rainBowprogress, 0.7f, 1f); 
            gameObject.GetComponent<Image>().color = new Color(h.r, h.g, h.b, gameObject.GetComponent<Image>().color.a); } 
            else if(mainMode == ModeOperation.Title_W_Version) {
                Color primary = Color.HSVToRGB(rainBowprogress, 0.8f, 0.8f); Color secondary = Color.HSVToRGB(rainBowprogress, 0.4f, 1f);
                gameObject.transform.GetChild(0).GetComponent<Image>().color = primary;
                gameObject.transform.GetChild(1).GetComponent<Image>().color = secondary;
                    gameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>().colorGradient =
                    new VertexGradient(primary, secondary, secondary, primary);
                gameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = version;
                gameObject.GetComponent<Image>().color = Color.Lerp(primary, Color.black, 0.7f);
                gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>().color = Color.Lerp(primary, Color.black, 0.7f);
        } else if(mainMode == ModeOperation.TitleReg) {
                Color primary = Color.HSVToRGB(rainBowprogress, 0.8f, 0.8f); Color secondary = Color.HSVToRGB(rainBowprogress, 0.4f, 1f);
                gameObject.transform.GetChild(0).GetComponent<Image>().color = primary;
                gameObject.transform.GetChild(1).GetComponent<Image>().color = secondary;
                gameObject.GetComponent<Image>().color = Color.Lerp(primary, Color.black, 0.7f);
        } else if(mainMode == ModeOperation.TitleTransReg) {
                Color primary = Color.HSVToRGB(rainBowprogress, 0.8f, 0.8f); Color secondary = Color.HSVToRGB(rainBowprogress, 0.4f, 1f);
                gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontMaterial.SetColor(ShaderUtilities.ID_FaceColor, Color.Lerp(primary, Color.black, 0.7f)); 
                gameObject.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().colorGradient =
                    new VertexGradient(primary, secondary, secondary, primary);
        } else if(mainMode == ModeOperation.TitleP1) {
                Color primary = Color.HSVToRGB(rainBowprogress, 0.8f, 0.8f); Color secondary = Color.HSVToRGB(rainBowprogress, 0.4f, 1f);
                gameObject.GetComponent<TextMeshProUGUI>().fontMaterial.SetColor(ShaderUtilities.ID_FaceColor, Color.Lerp(primary, Color.black, 0.7f)); 
        } else if(mainMode == ModeOperation.TitleP2) {
                Color primary = Color.HSVToRGB(rainBowprogress, 0.8f, 0.8f); Color secondary = Color.HSVToRGB(rainBowprogress, 0.4f, 1f);
                gameObject.GetComponent<TextMeshProUGUI>().colorGradient =
                    new VertexGradient(primary, secondary, secondary, primary);
            }
        }
    }
}
