using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TranslateScript : MonoBehaviour
{

    public enum transmode {
        Text,
        Text_W_Scale,
        TMP,
        Replace,
        Dropdown
    }
    
    public List<string> langs;
    public List<float> Xscale;
    public List<GameObject> RepObjs;

    public transmode MainMode;

    public List<string> EdropStrings;
    public List<string> FdropStrings;
    public List<string> GdropStrings;
    public List<string> RdropStrings;

    void Start() {
        int Lang = PlayerPrefs.GetInt("MAINLANG");
        if(MainMode == transmode.Text) {
            string fixedst = langs[Lang].Replace("\\n", "\n");
            gameObject.GetComponent<Text>().text = fixedst;
        } else if (MainMode == transmode.Text_W_Scale) {
            string fixedst = langs[Lang].Replace("\\n", "\n");
            gameObject.GetComponent<Text>().text = fixedst;
            gameObject.GetComponent<RectTransform>().localScale = new Vector2(Xscale[Lang], gameObject.GetComponent<RectTransform>().localScale.y);
        } else if (MainMode == transmode.TMP) {
            string fixedst = langs[Lang].Replace("\\n", "\n");
            gameObject.GetComponent<TextMeshProUGUI>().text = fixedst;
        } else if (MainMode == transmode.Replace) {
            for (int g = 0; g < RepObjs.Count; g++) {
                if (g == Lang) RepObjs[g].SetActive(true);
                else RepObjs[g].SetActive(false);
            }
        }  else if (MainMode == transmode.Dropdown) {
            Dropdown dd = gameObject.GetComponent<Dropdown>();
            int selval = dd.value; dd.options.Clear();
            List<string> selList;
            if (Lang == 0) selList = new List<string>(EdropStrings);
            else if (Lang == 1) selList = new List<string>(FdropStrings);
            else if (Lang == 2) selList = new List<string>(GdropStrings);
            else if (Lang == 3) selList = new List<string>(RdropStrings);
            else selList = new List<string>();
            for (int d = 0; d < selList.Count; d++) {
                dd.options.Add(new Dropdown.OptionData(selList[d]));
            } dd.value = selval;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
