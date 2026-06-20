using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class Osciloscope : MonoBehaviour
{
    public AudioSource wow;
    public MainAudioBuffer mab;
    public UILineRenderer aoka;

    public bool Working;
    public int RefRate;
    public int Samples;
    public float LNTHICK;
    public float Itm;
    public Color clr;
    public bool Vectorscope;
    public int SelectedMod;

    void Start() {
        ChangeLay();
    }

    public void ChangeLay() {
        if (!Working) { Vector2[] ptss = new Vector2[2]; if (!Vectorscope)
            ptss = new Vector2[2] {new Vector2(-(gameObject.GetComponent<RectTransform>().sizeDelta.x / 2),
                0f), new Vector2((gameObject.GetComponent<RectTransform>().sizeDelta.x / 2), 0f) };
        else ptss = new Vector2[2] {new Vector2(-(gameObject.GetComponent<RectTransform>().sizeDelta.x / 2), -(gameObject.GetComponent<RectTransform>().sizeDelta.y / 2)),
            new Vector2((gameObject.GetComponent<RectTransform>().sizeDelta.x / 2), (gameObject.GetComponent<RectTransform>().sizeDelta.y / 2)) };
            aoka.Points = ptss; aoka.LineThickness = LNTHICK;
            aoka.color = clr;
        } else {  Vector2[] pts = new Vector2[Samples];
        for (int p = 0; p < Samples; p++) pts[p] = Vector2.zero; 
        aoka.Points = pts; aoka.LineThickness = LNTHICK;
        aoka.color = clr; } Samples = Mathf.Clamp(Samples, 64, 8192);
    }

    void Update() {
        if ((wow.isPlaying || SelectedMod == 0) && Working) {
            Itm += Time.deltaTime;
            if(Itm >= (1f / RefRate)) {
                float Prog = gameObject.GetComponent<RectTransform>().sizeDelta.x / Samples;
                float[] wowers;
                if (SelectedMod != 0) { wowers = new float[Samples];
                    wow.GetOutputData(wowers, 0); }
                else wowers = mab.GetSamples(Samples);
                if (!Vectorscope) {
                    Vector2[] pts = new Vector2[Samples];
                    for (int f = 0; f < Samples; f++) {
                        pts[f] = new Vector2(-(gameObject.GetComponent<RectTransform>().sizeDelta.x / 2) + (Prog * f),
                            Mathf.Clamp(wowers[f], -1f, 1f) * (gameObject.GetComponent<RectTransform>().sizeDelta.y / 2));
                    } Itm -= (1f / RefRate); aoka.Points = pts;
                } else { float[] wowers2 = new float[Samples];
                    wow.GetOutputData(wowers2, 1);
                    Vector2[] pts2 = new Vector2[Samples];
                    for (int f = 0; f < Samples; f++) {
                        pts2[f] = new Vector2(Mathf.Clamp(wowers[f], -1f, 1f) * (gameObject.GetComponent<RectTransform>().sizeDelta.x / 2),
                            Mathf.Clamp(wowers[f] + Random.Range(-0.1f, 0.1f), -1f, 1f) * (gameObject.GetComponent<RectTransform>().sizeDelta.y / 2));
                    } Itm -= (1f / RefRate); aoka.Points = pts2;
                }
            }
        }
    }
}
