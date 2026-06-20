using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class URLSong : MonoBehaviour
{

    public GameObject cloner1;
    public RectTransform content1;

    public GameObject AddMenu;
    public InputField NameAdd;
    public InputField URLAdd;

    public GameObject ModMenu;
    public InputField NameMod;
    public InputField URLMod;

    public GameObject DelWarn;
    public GameObject URLInvalidWarn;

    public List<int> Indx;

    public int TempID;

    public void Start() {
        UpdateContent();
    }

    public void UpdateContent() { Indx.Clear();
        foreach (Transform a in content1) Destroy(a.gameObject); int usedS = 0;
        for(int s = 0; s < PlayerPrefs.GetInt("GETURLSONGS", 0); s++) {
            if (PlayerPrefs.GetString("URLSONG" + s, "nn") != "nn") {
                GameObject newr = Instantiate(cloner1, content1); newr.SetActive(true); int DEFINEDS = s;
                newr.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -14f - (usedS * 105f));
                newr.transform.GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("URLSONGNAME" + s, "Nothing!!!!");
                newr.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { WarnDelObj(DEFINEDS); });
                newr.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(() => { InitMod(DEFINEDS); });
                Indx.Add(s); usedS++;
            }
        } content1.sizeDelta = new Vector2(content1.sizeDelta.x, 14f + (105f * usedS));
    }

    public void WarnDelObj(int ID) {
        TempID = ID; int ID2 = Indx.IndexOf(ID);
        content1.GetChild(ID2).GetComponent<Button>().Select();
        DelWarn.SetActive(true);
    }

    public void DeleteObj() {
        PlayerPrefs.SetString("URLSONG" + TempID, "nn");
        UpdateContent();
        DelWarn.SetActive(false);
    }

    public void InitMod(int ID) {
        TempID = ID; int ID2 = Indx.IndexOf(ID);
        content1.GetChild(ID2).GetComponent<Button>().Select();
        ModMenu.SetActive(true);
        NameMod.text = PlayerPrefs.GetString("URLSONGNAME" + ID, "Nothing!!!!");
        URLMod.text = PlayerPrefs.GetString("URLSONG" + ID, "nn");
    }

    public void ModObj() {
        if (ConvertSyntax(URLMod.text) == null) URLInvalidWarn.SetActive(true);
        else {  PlayerPrefs.SetString("URLSONG" + TempID, ConvertSyntax(URLMod.text));
        PlayerPrefs.SetString("URLSONGNAME" + TempID, NameMod.text);
        int ID2 = Indx.IndexOf(TempID);
        content1.GetChild(ID2).GetChild(0).GetComponent<Text>().text = PlayerPrefs.GetString("URLSONGNAME" + TempID, "Nothing!!!!");
        ModMenu.SetActive(false); }
    }

    public void AddSng() {
        if (ConvertSyntax(URLAdd.text) == null) URLInvalidWarn.SetActive(true);
        else { int NextID = PlayerPrefs.GetInt("GETURLSONGS", 0);
        PlayerPrefs.SetString("URLSONG" + NextID, ConvertSyntax(URLAdd.text));
        PlayerPrefs.SetString("URLSONGNAME" + NextID, NameAdd.text);
        PlayerPrefs.SetInt("GETURLSONGS", PlayerPrefs.GetInt("GETURLSONGS") + 1);
        UpdateContent(); }
    }

    public string ConvertSyntax(string inp) {
        if (string.IsNullOrWhiteSpace(inp)) return null;
        if (inp.StartsWith("https://drive.google.com/file/d/")) {
            string data = inp.Substring(32, inp.Length - 32);
            data = data.Split('/')[0];
            return "https://drive.google.com/uc?export=download&id=" + data;
        }
        else return null;
    }
}
