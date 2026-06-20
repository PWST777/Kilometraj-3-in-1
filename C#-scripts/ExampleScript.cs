using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleScript : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa cod exemplu
    // Clasa veche folosita pentru a testa systemul BLE / clasa nu mai este folosita 3:
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public SPEEDMANAGER SM; // Class used to get speed
    float Timer = 0f;
    public float FadeTime = 0.12f;
    public Text Text1, Text2;
    float CurrentSpeed, LastSpeed;

    void Update() {
        Timer += Time.deltaTime;
        if (Timer > FadeTime) { Timer -= FadeTime;
            LastSpeed = CurrentSpeed; CurrentSpeed = SM.InterpSpeed; }
        Text1.text = (int)CurrentSpeed + ""; Text2.text = (int)LastSpeed + "";
        Text2.color = new Color(Text1.color.r, Text1.color.g, Text1.color.b, (1f - (Timer / FadeTime)));
    }
}
