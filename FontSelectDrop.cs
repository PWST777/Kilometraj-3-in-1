using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FontSelectDrop : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa schimbare font
    // Folosita pe elemente DropDown in unity pentru a seta font-uri dinamic
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public Text activeTxt;
    public Transform orgfel;
    public List<Font> fontsss;

    // Functie executata cand elementul DropDown este generat
    public void OnFontTouch() {
        StartCoroutine(apllft());
    }

    // Functie propriu-zisa pentru a seta font-uri unice pentru fiecare element din DropDown
    public IEnumerator apllft() {
        yield return null; orgfel = null;
        orgfel = gameObject.transform.GetChild(3);
        if(orgfel != null) for(int c = 0; c < 5; c++) {
            orgfel.GetChild(0).GetChild(0).GetChild(c + 1).transform.GetChild(2).GetComponent<Text>().font = fontsss[c];
        }
    }

    // Functie executata cand font-ul selectat este schimbat
    public void OnChangeFont() {
        activeTxt.font = fontsss[gameObject.GetComponent<Dropdown>().value];
    }
}
