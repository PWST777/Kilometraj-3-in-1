using System.Collections;
using System.Globalization;
using System.Xml;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class MapSystem : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa System de harta
    // Folosit pentru un element nefolosit ce functiona ca System de harta
    // momentan nu este folosita nicaeri 3:
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public SPEEDMANAGER SM;
    public SelectDrag Sdrg;
    public ColorPicker CoPi;
    public bool SceneMode;
    public int ElementID;
    public bool LoadMode;
    public GameObject EditorModPanel;
    public GameObject LoadIcon;
    public RectTransform mapContainer;
    public UILineRenderer roadPrefab;
    public Text labelPrefab;
    public double latCenter;
    public double lonCenter;
    public double areaSize = 0.01;
    public float zoomSpeed = 0.01f;
    public float minZoom = 0.5f;
    public float maxZoom = 3f;
    private float currentZoom = 1f;
    private float lastDistance = 0f;
    public float refCooldown = 0f;
    bool RequestRef;
    public int Width; public InputField WDT;
    public int Height; public InputField HEI;
    public int RefreshRate; public InputField REFR;
    public Color BackColor; public Image BkCl;
    public Button DelObj; public InputField soo;
    public float Timel; public bool Touchy;
    bool locationReadyCalled = false;
    public Vector2 activePos; public bool ON;

    void Start() {
        SM = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>();
        StartCoroutine(GainData());
    }

     public void OnSelectOfElement() { if (!SceneMode) { 
        WDT.onSubmit.RemoveAllListeners(); WDT.onSubmit.AddListener((_) => OnComponentMapSysModdify());
        HEI.onSubmit.RemoveAllListeners(); HEI.onSubmit.AddListener((_) => OnComponentMapSysModdify());
        REFR.onSubmit.RemoveAllListeners(); REFR.onSubmit.AddListener((_) => OnComponentMapSysModdify());
        DelObj.onClick.RemoveAllListeners(); DelObj.onClick.AddListener(() => DeleteObj(69));
        soo.onSubmit.RemoveAllListeners(); soo.onSubmit.AddListener((_) => ModSOrtO());
        BkCl.GetComponent<Button>().onClick.RemoveAllListeners(); BkCl.GetComponent<Button>().onClick.AddListener(() => TriggerColorChange("MSBACKCL")); }
        RefreshRate = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MSREFR", 20);
        Width = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MSWIDT", 700);
        Height = PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MSHEIG", 1000);
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BackColor = new Color(PlayerPrefs.GetFloat(colorstart + "TXTEXTCLR", 1f), PlayerPrefs.GetFloat(colorstart + "TXTEXTCLG", 1f), PlayerPrefs.GetFloat(colorstart + "TXTEXTCLB", 1f));
        if (!SceneMode) { WDT.text = Width + ""; HEI.text = Height + ""; REFR.text = RefreshRate + ""; } LoadMode = false;
    }

    public void OnComponentMapSysModdify() {
        if (!SceneMode && !LoadMode) { Width = int.Parse(WDT.text); Height = int.Parse(HEI.text); RefreshRate = int.Parse(REFR.text); }
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Width, Height);
        if(!SceneMode && !LoadMode) {
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MSWIDT", Width);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MSHEIG", Height);
            PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "MSREFR", RefreshRate);
        }
    }

    public void Toucher(bool yes) {
        Touchy = yes;
    }

    public void TriggerColorChange(string EndKey) {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        CoPi.RequestChangeColor(colorstart + EndKey, gameObject);
    }

    public void OnColorModdify() {
        string colorstart = "PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID;
        BackColor = new Color(PlayerPrefs.GetFloat(colorstart + "MSBACKCLR", 1f), PlayerPrefs.GetFloat(colorstart + "MSBACKCLG", 0.92f), PlayerPrefs.GetFloat(colorstart + "MSBACKCLB", 0.72f));
        gameObject.GetComponent<Image>().color = BackColor; if (!SceneMode) {BkCl.color = BackColor;}
    }

    public void DeleteObj(int wf) {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "TYPE", 69696);
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ", PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "DELOBJ") + 1);
        Sdrg.OnElemSelect(72769); Destroy(gameObject);
    }

    public void ModSOrtO() {
        PlayerPrefs.SetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "ELEM" + ElementID + "SO", int.Parse(soo.text));
        gameObject.GetComponent<SortOrderOrg>().Apply();
    }

    IEnumerator GainData() {
        LoadIcon.SetActive(true); if(SceneMode && ON) { LocationInfo userPos = Input.location.lastData;
        latCenter = userPos.latitude; lonCenter = userPos.longitude; }
        double latMin = latCenter - areaSize / 2;
        double latMax = latCenter + areaSize / 2;
        double lonMin = lonCenter - areaSize / 2;
        double lonMax = lonCenter + areaSize / 2;
        string query = $"[out:xml][timeout:25];way[\"highway\"]({latMin},{lonMin},{latMax},{lonMax});out geom;";
        string url = "https://overpass-api.de/api/interpreter?data=" + UnityWebRequest.EscapeURL(query);
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        foreach (Transform a in mapContainer) Destroy(a.gameObject);
        if (www.result == UnityWebRequest.Result.Success) {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(www.downloadHandler.text);
            XmlNodeList wayNodes = xmlDoc.GetElementsByTagName("way");
            foreach (XmlNode way in wayNodes) {
                string highwayType = null;
                string name = null;
                foreach (XmlNode child in way.ChildNodes) {
                    if (child.Name == "tag") {
                        string key = child.Attributes["k"].Value;
                        string value = child.Attributes["v"].Value;

                        if (key == "highway")
                            highwayType = value;
                        else if (key == "name")
                            name = value;
                    }
                }
                for(int f = 0; f < 2; f++) {
                if (string.IsNullOrEmpty(highwayType)) continue;
                UILineRenderer newRoad = Instantiate(roadPrefab, mapContainer);
                newRoad.color = GetColorForHighway(highwayType);
                    if (f == 0) newRoad.color = new Color(newRoad.color.r / 2f, newRoad.color.g / 2f, newRoad.color.b / 2f);
                    if (f == 0) newRoad.LineThickness = GetThicknessForHighway(highwayType) + 8;
                    else newRoad.LineThickness = GetThicknessForHighway(highwayType);
                var points = new List<Vector2>();
                foreach (XmlNode child in way.ChildNodes) {
                    if (child.Name == "nd") {
                        double lat = double.Parse(child.Attributes["lat"].Value, CultureInfo.InvariantCulture);
                        double lon = double.Parse(child.Attributes["lon"].Value, CultureInfo.InvariantCulture);
                        float xNorm = (float)((lon - lonMin) / (lonMax - lonMin));
                        float yNorm = (float)((lat - latMin) / (latMax - latMin));
                        points.Add(new Vector2(
                            xNorm * mapContainer.rect.width,
                            yNorm * mapContainer.rect.height
                        ));
                    }
                } if (points.Count >= 2) {
                    newRoad.Points = points.ToArray();
                    newRoad.SetAllDirty();
                    if (!string.IsNullOrEmpty(name) && labelPrefab != null) {
                        Vector2 center = Vector2.zero;
                        foreach (var p in points)
                            center += p;
                        center /= points.Count;

                        Text newLabel = Instantiate(labelPrefab, mapContainer);
                        newLabel.text = name;
                        newLabel.rectTransform.anchoredPosition = center;
                        newLabel.fontSize = GetFontSizeForHighway(highwayType);
                    }
                } else {
                    Destroy(newRoad.gameObject);
                }
                }
            }
        }
        LoadIcon.SetActive(false);
    }

    float GetThicknessForHighway(string type) {
        switch (type) {
            case "motorway": return 30f;
            case "trunk": return 25f;
            case "primary": return 20f;
            case "secondary": return 17f;
            case "tertiary": return 15f;
            case "residential": return 14f;
            case "service": return 12f;
            case "footway":
            case "path":
            case "cycleway": return 10f;
            default: return 20f;
        }
    }

    Color GetColorForHighway(string type) {
        switch (type) {
            case "motorway": return new Color(0.2f, 0.2f, 0.2f);
            case "trunk": return new Color(0.6f, 0.4f, 0.2f);
            case "primary": return new Color(0.6f, 0.6f, 0.3f);
            case "secondary": return new Color(0.6f, 0.6f, 0.6f);
            case "footway":
            case "cycleway": return new Color(0.7f, 0.7f, 0.7f);
            default: return Color.gray;
        }
    }

    int GetFontSizeForHighway(string highwayType) {
        switch (highwayType) {
            case "motorway":
            case "trunk": return 28;
            case "primary":
            case "secondary": return 24;
            default: return 20;
        }
    }

    void Update()
    {
        if (SceneMode) { 
            if (SM.ActiveSpeed > 0f || RequestRef)
            refCooldown += Time.deltaTime;
        if (Input.touchCount == 2 && Touchy) {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);
            float currentDistance = Vector2.Distance(t0.position, t1.position);
            if (lastDistance > 0f) {
                float delta = currentDistance - lastDistance;
                currentZoom += delta * zoomSpeed;
                currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
                if (mapContainer != null) mapContainer.localScale = Vector3.one * currentZoom;
                if (Mathf.Abs(delta) > 0.01f) RequestRef = true;
            }
            lastDistance = currentDistance;
            areaSize = 0.01f * currentZoom;
        } else {
            lastDistance = 0f;
        }
        if (RequestRef && refCooldown >= 1.6f) {
            StartCoroutine(GainData());
            RequestRef = false;
            refCooldown = 0f;
        } else if (!RequestRef && refCooldown >= Mathf.Clamp(10f * currentZoom, 5f, 50f)) {
            StartCoroutine(GainData());
            refCooldown = 0f;
        }
        if (!locationReadyCalled && Input.location.isEnabledByUser &&
        Input.location.status == LocationServiceStatus.Running) {
            locationReadyCalled = true;
            StartCoroutine(GainData());
        } Timel += Time.deltaTime;
        if (Timel > (1f / RefreshRate)) {
            if (SM.ActiveSpeed > 0f || Input.location.isEnabledByUser) {
                if(ON) { LocationInfo userPos = Input.location.lastData;
                activePos = new Vector2(userPos.longitude, userPos.latitude); }
                double latDiff = activePos.x - latCenter;
                double lonDiff = activePos.y - lonCenter;
                float xOffset = (float)(lonDiff / areaSize) * mapContainer.rect.width;
                float yOffset = (float)(latDiff / areaSize) * mapContainer.rect.height;
                mapContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(-xOffset, -yOffset);
                if (Mathf.Abs(xOffset) > mapContainer.rect.width / 4f ||
                    Mathf.Abs(yOffset) > mapContainer.rect.height / 4f) {
                    latCenter = activePos.x;
                    lonCenter = activePos.y;
                    RequestRef = true;
                }
            } Timel -= (1f / RefreshRate);
        }
    } }
}
