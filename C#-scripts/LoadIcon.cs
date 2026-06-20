using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadIcon : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Iconita de incarcare
    // Folosita pentru animarea iconitei de incarcare
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public float TimeProg;
    public float SinProg;

    // Functie chemata in fiecare cadru | Functie folosita pentru a actualiza efectul in timp real
    void FixedUpdate() { Image comp = gameObject.GetComponent<Image>();
        TimeProg += 0.025f;
        SinProg = TimeProg * Mathf.PI;
        if (TimeProg % 1f <= 0.025f) { comp.fillClockwise = !comp.fillClockwise;
            if (comp.fillClockwise) gameObject.transform.Rotate(0f, 0f, 18f);
            else gameObject.transform.Rotate(0f, 0f, 54f);
        } comp.fillAmount = 0.45f + (Mathf.Cos(SinProg) * 0.4f);
        gameObject.transform.Rotate(0f, 0f, -2.5f);
    }
}
