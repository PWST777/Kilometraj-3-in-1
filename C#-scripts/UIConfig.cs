using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConfig : MonoBehaviour
{

    public CustomTextureUniversal ctu;
    public SelectDrag sd;
    public GameObject Menu1;
    public GameObject Menu2;
    public Image img1;
    public Image img2;
    public List<Sprite> imgs;
    public List<Sprite> yaim;
    public bool needsTiling;
    public List<InputField> arges;
    public int Tirger;

    int skinS;
    float rainBowprogress;
    int selBox;

    void Awake() { imgs.RemoveAt(0);
        if (PlayerPrefs.GetInt("USKIN") == 0) {
            ctu = GameObject.Find("UNIVERSALSKIN").GetComponent<CustomTextureUniversal>();
            int s = PlayerPrefs.GetInt("SKINMETAL"); skinS = s;
            if (s != 10) { imgs.Insert(0, yaim[s]); needsTiling = (s == 3 || s == 5 | s == 7); }
            else { imgs.Insert(0, ctu.CustomImgTexture); needsTiling = (PlayerPrefs.GetInt("UTEXTUREGLOSSTEX", 0) == 1); }
        } sd.onSelct += ReadData; if(needsTiling) img1.type = Image.Type.Tiled;
        img1.sprite = imgs[0]; img2.sprite = imgs[1];
        rainBowprogress = 0f;
    }

    public void ReadData() {
        arges[0].text = (int)sd.ElementS.anchoredPosition.x + "";
        arges[1].text = (int)sd.ElementS.anchoredPosition.y + "";
        arges[2].text = sd.ElementS.transform.localScale.ToString("F2");
        arges[3].text = (int)sd.ElementS.rotation.eulerAngles.z + "";
    }
    
    public void ChangeMode(bool Trans) {
        if(Trans) { img2.sprite = imgs[1];
            if (needsTiling) img1.type = Image.Type.Tiled; else img1.type = Image.Type.Sliced;
            img1.sprite = imgs[0]; img2.type = Image.Type.Sliced; 
            Menu1.SetActive(true); Menu2.SetActive(false); }
        else { img1.sprite = imgs[1];
            if (needsTiling) img2.type = Image.Type.Tiled; else img2.type = Image.Type.Sliced;
            img2.sprite = imgs[0]; img1.type = Image.Type.Sliced; 
            Menu2.SetActive(true); Menu1.SetActive(false); }
        selBox = Trans ? 0 : 1;
    }

    void Update() { 
        if(skinS == 9) { rainBowprogress += Time.deltaTime * 0.3f; if (rainBowprogress > 1f) rainBowprogress -= 1f;
            Color primary = Color.HSVToRGB(rainBowprogress, 0.7f, 1f);
            if (selBox == 1) { img2.color = primary; img1.color = Color.white; }
            else { img1.color = primary; img2.color = Color.white; }
        }
    }
}
