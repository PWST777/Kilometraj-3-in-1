using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugA1 : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa debug
    // Folosit pentru debug / momentan nu este folosita nicaeri 3:
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public MainAudioBuffer mab;
    public Text textBox;

    void Update()
    {
        if (mab == null) { mab = GameObject.Find("0")?.GetComponent<MainAudioBuffer>(); }
        else {
            float[] samples = mab.GetSamples(100);
            if (samples == null) return;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < samples.Length; i++) {
                sb.Append(samples[i].ToString("F4"));
                sb.Append(',');
            } textBox.text = sb.ToString();
        }
    }
}
