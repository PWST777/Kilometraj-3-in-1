using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortOrderOrg : MonoBehaviour
{
    public int ElementID;
    public int SortOtd;
    public Transform allElem;

    public void Apply() {
        SortOtd = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "SO");
        gameObject.transform.SetSiblingIndex(Mathf.Max(Mathf.Min(SortOtd, gameObject.transform.parent.childCount - 1), 0));
        for(int e = 0; e < allElem.childCount; e++)
            if(allElem.GetChild(e).gameObject != gameObject)
                PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + allElem.GetChild(e).gameObject.name + "SO", allElem.GetChild(e).GetSiblingIndex());
    }
}
