using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomTextureUniversal : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Textura personalizata
    // Folosit pentru a genera textura personalizata
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public static CustomTextureUniversal _instance; // Instanta classei pentru a nu a aparea de 2 ori

    // Imagini referinta folosite pentru a genera textura finala
    public Sprite CustomImgTexture;
    public Sprite baseLayer;
    public Sprite glossLayer;
    public Sprite textureLayerMetalic;
    public Sprite textureLayerTerra;

    // Functie executata cand elementul este creat
    void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            RegenCustomTexture();
        } else {
            Destroy(gameObject);
            return;
        }
    }

    // Functie folosita pentru a calcula valoarea alpha dintr-un pixel cand este combinata cu o alta culaore
    private static Color AlphaBlend(Color bg, Color fg){
        float outA = fg.a + bg.a * (1f - fg.a);
        if (outA <= 0f)
            return Color.clear;
        return new Color((fg.r * fg.a + bg.r * bg.a * (1f - fg.a)) / outA, (fg.g * fg.a + bg.g * bg.a * (1f - fg.a)) / outA, (fg.b * fg.a + bg.b * bg.a * (1f - fg.a)) / outA, outA );
    }

    // Functie folosita pentru a genera textura personalizata
    public void RegenCustomTexture() {
        Color mainColor = new Color( PlayerPrefs.GetFloat("MYUWUWU0R", 0f), PlayerPrefs.GetFloat("MYUWUWU0G", 0f), PlayerPrefs.GetFloat("MYUWUWU0B", 0f) );
        Color glossColor = new Color( PlayerPrefs.GetFloat("MYUWUWU1R", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1G", 0.4f), PlayerPrefs.GetFloat("MYUWUWU1B", 0.4f) );
        int texture = PlayerPrefs.GetInt("UTEXTUREGLOSSTEX", 0);
        int direction = PlayerPrefs.GetInt("UTEXTUREGLOSSDIR", 2);
        Texture2D baseTex = _instance.baseLayer.texture;
        Texture2D glossTex = _instance.glossLayer.texture;
        Texture2D textureTex = texture == 0 ? _instance.textureLayerMetalic.texture : _instance.textureLayerTerra.texture;
        Texture2D finalTex = new Texture2D(80, 80, TextureFormat.RGBA32, false);
        for (int x = 0; x < 80; x++) {
            for (int y = 0; y < 80; y++) {
                Color basePixel = baseTex.GetPixel(x, y);
                Color glossPixel = glossTex.GetPixel(x, y);
                Color texturePixel = (direction == 2) ? textureTex.GetPixel(x, y) : (direction == 3) ? textureTex.GetPixel(79 - y, x) : (direction == 0) ? textureTex.GetPixel(79 - x, 79 - y) : textureTex.GetPixel(y, 79 - x);
                Color baseCol = new Color(basePixel.r * mainColor.r, basePixel.g * mainColor.g, basePixel.b * mainColor.b, basePixel.a);
                Color glossCol = new Color(glossPixel.r * glossColor.r, glossPixel.g * glossColor.g, glossPixel.b * glossColor.b, glossPixel.a);
                Color texCol = new Color( texturePixel.r, texturePixel.g, texturePixel.b, texturePixel.a);
                Color finalColor = Color.clear;
                finalColor = AlphaBlend(finalColor, baseCol);
                if (texture == 0) finalColor = AlphaBlend(finalColor, glossCol);
                finalColor = AlphaBlend(finalColor, texCol);
                finalTex.SetPixel(x, y, finalColor);
            }
        } finalTex.Apply();
        CustomImgTexture = Sprite.Create( finalTex, new Rect(0, 0, finalTex.width, finalTex.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, new Vector4(16, 16, 16, 16) );
    }
}
