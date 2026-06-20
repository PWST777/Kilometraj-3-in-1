using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteBordMod : MonoBehaviour
{

    public GameObject spliceMenu;
    public RawImage slicer; public Image prevslicer;
    public Vector2 imgbounds; public Vector4 bounds;
    public RectTransform Left; public InputField LeftI;
    public RectTransform Right; public InputField RightI;
    public RectTransform Top; public InputField TopI;
    public RectTransform Bottom; public InputField BottomI;
    public bool InputM; bool Chng; public int Touchy;
    public string keyr; public GameObject sendf;
    public bool Qu; public float TimeTillUp;

    public void RequestBChange(string key, GameObject sender, Texture wowo) {
        slicer.texture = wowo; keyr = key; sendf = sender;
        imgbounds = new Vector2(wowo.width, wowo.height); RefPrev();
    }

    public void ConfirmChanges() {
        if(sendf.GetComponent<ImageComponent>()) {
            PlayerPrefs.SetInt(keyr + "1", Mathf.RoundToInt(bounds.x)); PlayerPrefs.SetInt(keyr + "2", Mathf.RoundToInt(bounds.y));
            PlayerPrefs.SetInt(keyr + "3", Mathf.RoundToInt(bounds.z)); PlayerPrefs.SetInt(keyr + "4", Mathf.RoundToInt(bounds.w));
            sendf.GetComponent<ImageComponent>().OnSpliceRecieve(); spliceMenu.SetActive(false);
        } if(sendf.GetComponent<BackgroundT>()) {
            PlayerPrefs.SetInt(keyr + "1", Mathf.RoundToInt(bounds.x)); PlayerPrefs.SetInt(keyr + "2", Mathf.RoundToInt(bounds.y));
            PlayerPrefs.SetInt(keyr + "3", Mathf.RoundToInt(bounds.z)); PlayerPrefs.SetInt(keyr + "4", Mathf.RoundToInt(bounds.w));
            sendf.GetComponent<BackgroundT>().OnSpliceRecieve(); spliceMenu.SetActive(false);
        }
    }

    public void OnTouchy(int lrtb) {
        Touchy = lrtb;
    }

    public void OnValueMod() {
        bounds = new Vector4(Mathf.Clamp(int.Parse(LeftI.text),0,imgbounds.x), Mathf.Clamp(int.Parse(BottomI.text), 0, imgbounds.y),
             Mathf.Clamp(int.Parse(RightI.text), 0, imgbounds.x), Mathf.Clamp(int.Parse(TopI.text), 0, imgbounds.y));
        LeftI.text = bounds.x + ""; RightI.text = bounds.z + ""; TopI.text = bounds.w + ""; BottomI.text = bounds.y + "";
        Left.anchoredPosition = new Vector2(((float)bounds.x / imgbounds.x) * 784f, 0f);
        Right.anchoredPosition = new Vector2((1f - ((float)bounds.z / imgbounds.x)) * 784f, 0f);
        Top.anchoredPosition = new Vector2(0f, (1f - ((float)bounds.w / imgbounds.y)) * 784f);
        Bottom.anchoredPosition = new Vector2(0f, ((float)bounds.y / imgbounds.y) * 784f);
        RefPrev();
    }

    public void RefPrev() {
        Texture2D tex = (Texture2D)slicer.texture; prevslicer.type = Image.Type.Sliced;
        prevslicer.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, bounds);
        prevslicer.pixelsPerUnitMultiplier = Mathf.Max(tex.width / prevslicer.rectTransform.rect.width, tex.height / prevslicer.rectTransform.rect.height) / 2;
    }

    void Update() {
        if(Input.touchCount == 1 && Touchy > 0) { Touch wow = Input.GetTouch(0); InputM = false; if (wow.phase == TouchPhase.Moved) { Chng = true; Qu = true;
            if (Touchy == 1) { Left.anchoredPosition += new Vector2(wow.deltaPosition.x, 0f); Left.anchoredPosition = new Vector2(Mathf.Clamp(Left.anchoredPosition.x, 0f, 784f), 0f); }
            if (Touchy == 2) { Right.anchoredPosition += new Vector2(wow.deltaPosition.x, 0f); Right.anchoredPosition = new Vector2(Mathf.Clamp(Right.anchoredPosition.x, 0f, 784f), 0f); }
            if (Touchy == 3) { Top.anchoredPosition += new Vector2(0f, wow.deltaPosition.y); Top.anchoredPosition = new Vector2(0f, Mathf.Clamp(Top.anchoredPosition.y, 0f, 784f)); }
            if (Touchy == 4) { Bottom.anchoredPosition += new Vector2(0f, wow.deltaPosition.y); Bottom.anchoredPosition = new Vector2(0f, Mathf.Clamp(Bottom.anchoredPosition.y, 0f, 784f)); } }
        } else InputM = true;
        if (Chng) { if(!InputM) { bounds = new Vector4(((Left.anchoredPosition.x / 784f) * imgbounds.x), ((Bottom.anchoredPosition.y / 784f) * imgbounds.y),
                    ((1f - (Right.anchoredPosition.x / 784f)) * imgbounds.x), ((1f - (Top.anchoredPosition.y / 784f)) * imgbounds.y)); Chng = false;
                LeftI.text = Mathf.RoundToInt(bounds.x) + ""; RightI.text = Mathf.RoundToInt(bounds.z) + ""; TopI.text = Mathf.RoundToInt(bounds.w) + ""; BottomI.text = Mathf.RoundToInt(bounds.y) + "";
            }
        } if(TimeTillUp <= 0f && Qu) {
            RefPrev(); TimeTillUp = 0.1f; Qu = false;
        }
    }
}
