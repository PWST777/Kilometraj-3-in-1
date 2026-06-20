using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseFlipper : MonoBehaviour
{

    public RectTransform tch;
    void Start() {
        if (PlayerPrefs.GetInt("USEFLIP", 0) == 1) tch.localScale = new Vector3(.01f, -.01f, .01f);
        else tch.localScale = new Vector3(.01f, .01f, .01f);
    }
}
