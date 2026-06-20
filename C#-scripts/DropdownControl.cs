using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownControl : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa controler dropdown
    // Folosit pentru a controla marimea textului elementelor DropDown din unity
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public float scaleCoef;

    // Functie executata cand elementul este activat
    void OnEnable() {
        UpdateScale();
    }

    // Functie executata cand marimea marcajului text de pe dropdown este modificata
    public void UpdateScale() {
        if (!gameObject.activeSelf) return;
        StartCoroutine(UpdateScaleI());
    }

    // Functie propriu-zisa pentru modificarea marimei marcajelor a elementelor dropdown
    public IEnumerator UpdateScaleI() {
        yield return null; try {
            Dropdown control = gameObject.GetComponent<Dropdown>();
            Text labelMain = gameObject.transform.GetChild(0).GetComponent<Text>();
            labelMain.rectTransform.localScale = new Vector2(scaleCoef, 1f);
            Transform listObj = gameObject.transform.GetChild(3);
            Transform actList = listObj.GetChild(0).GetChild(0);
            for (int i = 1; i < actList.childCount; i++) {
                Text labelMainL = actList.transform.GetChild(i).GetChild(2).GetComponent<Text>();
                labelMainL.rectTransform.localScale = new Vector2(scaleCoef, 1f);
            }
        } catch { // koyu was here
        }
    }
}
