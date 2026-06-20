using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class LCDStretch : Shadow
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Intindere LCD
    // Folosita pentru a intinde font-ul folosit pentru elementele LCD
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    // Variabile de marime
    public float WidthS;
    public float HeightS;
    public int FAS;
    public int MakeSensitive;
    Vector3 LastSize;

    protected LCDStretch()
    { }

    // Functie principala folosita pentru crearea efectului
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;
        FAS = gameObject.GetComponent<Text>().fontSize; float Sens = 1f;
        if ((MakeSensitive == 2 || MakeSensitive == 3) && gameObject.transform.parent.parent.GetComponent<LCDSpedometer>().FontStSet == 1) Sens = 1.38f;
        if (MakeSensitive == 1) Sens *= gameObject.transform.parent.GetComponent<LCDSpedometer>().DecimalSize;
        if (MakeSensitive == 3) Sens *= gameObject.transform.parent.parent.GetComponent<LCDSpedometer>().DecimalSize;
        if(MakeSensitive == 2 || MakeSensitive == 3) {
            WidthS = ((0.65f / (gameObject.transform.parent.localScale.x / Sens)) - 0.65f) * (FAS / 10);
            HeightS = ((0.65f / (gameObject.transform.parent.localScale.y / Sens)) - 0.65f) * (FAS / 10);
        } else {
            WidthS = ((0.65f / (gameObject.transform.localScale.x / Sens)) - 0.65f) * (FAS / 10);
            HeightS = ((0.65f / (gameObject.transform.localScale.y / Sens)) - 0.65f) * (FAS / 10);
        }
        var verts = ListPool<UIVertex>.Get();
        vh.GetUIVertexStream(verts);
        var neededCpacity = verts.Count * 9;
        if (verts.Capacity < neededCpacity)
            verts.Capacity = neededCpacity;
        if (HeightS == 0 && WidthS != 0) {
            var start = 0;
            var end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, WidthS, 0f);
            // ^ Functie ce genereaza o copie grafica a textului ce este pozitionata astfel incat sa creeze efectul dorit
            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, WidthS / 2f, 0f);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -WidthS / 2f, 0f);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -WidthS, 0f);
        } else if (HeightS != 0 && WidthS == 0) {
            var start = 0;
            var end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0f, HeightS);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0f, HeightS / 2f);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0f, -HeightS / 2f);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0f, -HeightS);
        } else if (HeightS != 0 && WidthS != 0) {
            var start = 0;
            var end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0f, HeightS);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0f, HeightS / 2f);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0f, -HeightS / 2f);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, 0f, -HeightS);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, WidthS, 0f);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, WidthS / 2f, 0f);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -WidthS / 2f, 0f);

            start = end;
            end = verts.Count;
            ApplyShadowZeroAlloc(verts, effectColor, start, verts.Count, -WidthS, 0f);
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(verts);
        ListPool<UIVertex>.Release(verts);
    }

    // Functie chemata in fiecare cadru | Functie folosita pentru a actualiza efectul in timp real
    void Update() {
        if(LastSize != gameObject.transform.localScale)
        gameObject.GetComponent<Text>().SetVerticesDirty();
        LastSize = gameObject.transform.localScale;
    }
}
