using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Spectrum : MonoBehaviour
{

    public AudioSource AudioS;
    public MainAudioBuffer mab;
    public GameObject Clonr;
    public Transform orgfel;
    public int Segments;
    public bool Gapped;
    public float GapSize;
    public int RefreshRate;
    public float Timel;
    public bool Bottomed;
    public bool Smoothing;
    public float Multiplier;
    public int SelectedMod;
    float SmoothMult;
    
    public void Start() {
        if(orgfel.childCount != 0) { foreach (Transform wow in orgfel) Destroy(wow.gameObject); }
        float UngappedMult = Mathf.Sqrt(Mathf.Sqrt(Segments / 10f));
        Segments = Mathf.ClosestPowerOfTwo(Segments); float SegSize; int RendSegs = (int)(Segments / 1.5f);
        float PosDif = gameObject.GetComponent<RectTransform>().sizeDelta.x / (float)RendSegs;
        if (!Gapped) SegSize = (gameObject.GetComponent<RectTransform>().sizeDelta.x * UngappedMult) / (float)RendSegs;
        else SegSize = (gameObject.GetComponent<RectTransform>().sizeDelta.x / (float)RendSegs) - (GapSize / 10f);
        for (int s = 0; s < RendSegs; s++) { GameObject Seg = Instantiate(Clonr, orgfel); Seg.SetActive(true);
            Seg.GetComponent<RectTransform>().sizeDelta = new Vector2(SegSize, 3f);
            if (!Bottomed) { Seg.transform.localPosition = new Vector2(-(gameObject.GetComponent<RectTransform>().sizeDelta.x / 2f) + (PosDif * (float)s), 0f);
                Seg.GetComponent<RectTransform>().pivot = new Vector2(0f, .5f); }
            else { Seg.transform.localPosition = new Vector2(-(gameObject.GetComponent<RectTransform>().sizeDelta.x / 2f) + (PosDif * (float)s), -(gameObject.GetComponent<RectTransform>().sizeDelta.y / 2f));
                Seg.GetComponent<RectTransform>().pivot = new Vector2(0f, 0f); }
        if (Smoothing) SmoothMult = 0.1f * RefreshRate; } 
    }

    // Update is called once per frame
    void Update() {
        Timel += Time.deltaTime;
        if(Timel >= (1f / RefreshRate)) { float val2 = gameObject.GetComponent<RectTransform>().sizeDelta.y; float[] Samples;
            if (SelectedMod != 0) { Samples = new float[Mathf.Max(Segments, 64)]; AudioS.GetSpectrumData(Samples, 0, FFTWindow.Rectangular); }
            else { Samples = mab.GetFFTSamples(Segments); } bool sm = (SelectedMod == 0);
            int Count = 3; foreach(Transform segt in orgfel) { float Val = 0f;
                if (orgfel.childCount >= 42) Val = Mathf.Clamp((Samples[Count] * (sm ? ((Count + 21) / 8) : Count) * Multiplier) * (val2 / 2f), 3f, val2); 
                else if(orgfel.childCount >= 21) Val = Mathf.Clamp(((Samples[Count*2] + Samples[(Count*2) + 1]) * (((sm ? ((Count + 21) / 8) : Count) * 2) + 1)) * Multiplier * (val2 / 2f), 3f, val2);  
                else if(orgfel.childCount >= 10) Val = Mathf.Clamp(((Samples[Count*4] + Samples[(Count*4) + 1] + Samples[(Count * 4) + 2] + Samples[(Count * 4) + 3]) * Multiplier * (((sm ? ((Count + 21) / 8) : Count) * 4) + 2)) * (val2 / 2f), 3f, val2);
                if(Smoothing && segt.GetComponent<RectTransform>().sizeDelta.y > Val) segt.GetComponent<RectTransform>().sizeDelta -= new Vector2(0f, segt.GetComponent<RectTransform>().sizeDelta.y / SmoothMult);
                else segt.GetComponent<RectTransform>().sizeDelta = new Vector2(segt.GetComponent<RectTransform>().sizeDelta.x, Val); Count++;
            } Timel -= (1f / RefreshRate);
        }
    }
}
