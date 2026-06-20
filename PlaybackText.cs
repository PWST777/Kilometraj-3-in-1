using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaybackText : MonoBehaviour
{

    public RectTransform container;
    public RectTransform textRect;
    public Text actualtext;
    public Image Backg;
    public List<Image> EdgeBlurs;
    public float PreferedWidth;
    public int AnimMode;
    public float AnimSpeed;
    public bool EdgeBlur;
    public bool Background;
    public int FontSize;
    int f2cool; bool dir1; int coold;
    public int font; public List<Font> fontsssa;

    // Start is called before the first frame update
    void Start() {
        LayChange();
    }

    public void LayChange() {
        actualtext.fontSize = FontSize;
        actualtext.font = fontsssa[font];
        PreferedWidth = actualtext.preferredWidth;
        if (!Background) { EdgeBlur = false; EdgeBlurs[0].color = new Color(EdgeBlurs[0].color.r, EdgeBlurs[0].color.g, EdgeBlurs[0].color.b, 0);
            EdgeBlurs[1].color = new Color(EdgeBlurs[1].color.r, EdgeBlurs[1].color.g, EdgeBlurs[1].color.b, 0);
            Backg.color = new Color(Backg.color.r, Backg.color.g, Backg.color.b, 0);
        } else {  EdgeBlurs[0].color = new Color(EdgeBlurs[0].color.r, EdgeBlurs[0].color.g, EdgeBlurs[0].color.b, 1);
            EdgeBlurs[1].color = new Color(EdgeBlurs[1].color.r, EdgeBlurs[1].color.g, EdgeBlurs[1].color.b, 1);
            Backg.color = new Color(Backg.color.r, Backg.color.g, Backg.color.b, 1); }
        if(EdgeBlur) { EdgeBlurs[0].color = Backg.color; EdgeBlurs[1].color = Backg.color; } else { EdgeBlurs[0].color = new Color(0, 0, 0, 0); EdgeBlurs[1].color = new Color(0, 0, 0, 0); }
    }

    void FixedUpdate() {
        f2cool++; if(font <= 4) { 
        if(f2cool > 1) { f2cool -= 2;
            if(PreferedWidth > container.rect.width) { actualtext.alignment = TextAnchor.MiddleLeft;
                if(AnimMode == 0) {
                    textRect.anchoredPosition -= new Vector2(AnimSpeed, 0f);
                    if(textRect.anchoredPosition.x < -(PreferedWidth)) {
                        textRect.anchoredPosition += new Vector2(PreferedWidth + container.rect.width + 10f, 0f);
                    }
                } if(AnimMode == 1) { if(coold < 0) { if(!dir1) { 
                    textRect.anchoredPosition += new Vector2(AnimSpeed, 0f);
                        if (textRect.anchoredPosition.x > 20f) { dir1 = true; coold = 30; }
                    } else { 
                    textRect.anchoredPosition -= new Vector2(AnimSpeed, 0f);
                        if (textRect.anchoredPosition.x < -(PreferedWidth - container.rect.width + 20f)) { dir1 = false; coold = 30; }
                    } } coold--;
                }
            } else { textRect.anchoredPosition = new Vector2(0f, 0f); actualtext.alignment = TextAnchor.MiddleCenter; }
        } } else if (font <= 12) { if (f2cool > (int)(60f / AnimSpeed)) { f2cool -= (int)(60f / AnimSpeed);
            if(PreferedWidth > container.rect.width) { actualtext.alignment = TextAnchor.MiddleLeft;
                if(AnimMode == 0) {
                    textRect.anchoredPosition -= new Vector2((0.82f * FontSize), 0f);
                    if(textRect.anchoredPosition.x < -(PreferedWidth)) { int segs = (int)(container.rect.width / (0.82f * FontSize));
                        textRect.anchoredPosition += new Vector2(PreferedWidth + ((segs+2) * (0.82f * FontSize)), 0f);
                    }
                } if(AnimMode == 1) { if(coold < 0) { if(!dir1) { 
                    textRect.anchoredPosition += new Vector2((0.82f * FontSize), 0f);
                        if (textRect.anchoredPosition.x > 0f) { dir1 = true; coold = (int)(AnimSpeed); }
                    } else { 
                    textRect.anchoredPosition -= new Vector2((0.82f * FontSize), 0f);
                        if (textRect.anchoredPosition.x < -(PreferedWidth - container.rect.width)) { dir1 = false; coold = (int)(AnimSpeed); }
                    } } coold--;
                }
            } else { textRect.anchoredPosition = new Vector2(0f, 0f); actualtext.alignment = TextAnchor.MiddleCenter; }
        } } else if (font == 13) { if (f2cool > (int)(60f / AnimSpeed)) { f2cool -= (int)(60f / AnimSpeed);
            if(PreferedWidth > container.rect.width) { actualtext.alignment = TextAnchor.MiddleLeft;
                if(AnimMode == 0) {
                    textRect.anchoredPosition -= new Vector2((0.6f * FontSize), 0f);
                    if(textRect.anchoredPosition.x < -(PreferedWidth)) { int segs = (int)(container.rect.width / (0.6f * FontSize));
                        textRect.anchoredPosition += new Vector2(PreferedWidth + ((segs+2) * (0.6f * FontSize)), 0f);
                    }
                } if(AnimMode == 1) { if(coold < 0) { if(!dir1) { 
                    textRect.anchoredPosition += new Vector2((0.6f * FontSize), 0f);
                        if (textRect.anchoredPosition.x > 0f) { dir1 = true; coold = (int)(AnimSpeed); }
                    } else { 
                    textRect.anchoredPosition -= new Vector2((0.6f * FontSize), 0f);
                        if (textRect.anchoredPosition.x < -(PreferedWidth - container.rect.width)) { dir1 = false; coold = (int)(AnimSpeed); }
                    } } coold--;
                }
            } else { textRect.anchoredPosition = new Vector2(0f, 0f); actualtext.alignment = TextAnchor.MiddleCenter; }
        } } 
    }
}
