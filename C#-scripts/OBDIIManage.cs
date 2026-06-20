using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class OBDIIManage : MonoBehaviour
{
    public static OBDIIManage _instance;

    private AndroidJavaObject obd;

    private bool isConnected = false;
    private bool isReading = false;

    public float rpm;
    public float speed;
    public float throttle;
    public float engineLoad;
    public float fuelLevel;
    public float coolantTemp;

    public string lastsend;

    private string pattern = "SRTSRLSRTSRLSRTSRLSRCSRLSRTSRLSRTSRLSRTSRLSRFSRC";
    private int patternIndex = 0;

    private char lastCommand;

    public Action ontick;

    public string[] devs;

    public RectTransform deviceList;
    public GameObject deviceLObj;
    public Button refBt;
    public Image ConnectBox;
    public Text CBText;
    public GameObject MMenu;
    public GameObject MMenu2;
    public List<Button> MBts;
    public List<Sprite> Textures;
    float nextSendTime = 0f;
    float sendInterval = 0.01f;
    bool usingKLine = false;

    public Action onConnect;
    public Action onSetUP;
    public Action onSpeed;

    void Awake()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer =
            new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        AndroidJavaObject activity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        obd = new AndroidJavaObject(
            "com.example.obdplugin.BluetoothOBD",
            activity
        );
#endif
        int useK = PlayerPrefs.GetInt("KLINEMODE", 0);
        ToggleKMode(useK == 1, false);
    }

    public string[] GetPairedDevices()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return obd.Call<string[]>("getAvailableDevices");
#else
        return new string[0];
#endif
    }

    public bool Connect(string mac)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        isConnected = obd.Call<bool>("connect", mac);
        if(isConnected) { int useK = PlayerPrefs.GetInt("KLINEMODE", 0);
        ToggleKMode(useK == 1, true); }
        onConnect?.Invoke();
        return isConnected;
#else
        return false;
#endif
    }

    public void InitELM(int uK) {
        usingKLine = (uK == 1);
        nextSendTime = Time.time;
        Send("ATZ");
        Send("ATE0");
        Send("ATL0");
        Send("ATS0");
        Send("ATH0");
        if (usingKLine) {
            sendInterval = 0.15f;
            Send("ATAT0");
            Send("ATSTFF");
            Send("ATSP5");
        } else {
            sendInterval = 0.01f;
            Send("ATAT2");
            Send("ATSP0");
        }
    }

    public void ToggleKMode(bool tr, bool realconnect) {
        return;
        if(tr) { pattern = "SRSRSRSR";
            MBts[0].GetComponent<Image>().sprite = Textures[0];
            MBts[1].GetComponent<Image>().sprite = Textures[1];
            PlayerPrefs.SetInt("KLINEMODE", 1); if (realconnect) { InitELM(1); }
        } else { pattern = "SRTSRLSRTSRLSRTSRLSRCSRLSRTSRLSRTSRLSRTSRLSRFSRC";
            MBts[1].GetComponent<Image>().sprite = Textures[0];
            MBts[0].GetComponent<Image>().sprite = Textures[1];
            PlayerPrefs.SetInt("KLINEMODE", 0); if (realconnect) { InitELM(0); }
        }
    }

    void Update() {
        if (!isReading) return;
        string res = obd.Call<string>("getAndClearResponse");
        if (!string.IsNullOrEmpty(res)) {
            lastsend = res;
            ParseResponse(res);
            ontick?.Invoke();
        } if (Time.time >= nextSendTime) {
            SendNext(); nextSendTime = Time.time + sendInterval;
        }
    }

    void SendNext()
    {
        char p = pattern[patternIndex];
        patternIndex = (patternIndex + 1) % pattern.Length;

        lastCommand = p;

        switch (p)
        {
            case 'R': { Send("010C"); break; }
            case 'S': { Send("010D"); onSpeed?.Invoke(); break; }
            case 'T': { Send("0111"); break; }
            case 'L': { Send("0104"); break; }
            case 'F': { Send("012F"); break; }
            case 'C': { Send("0105"); break; }
        }
    }

    void Send(string cmd) {
        if (obd != null) obd.Call("sendCommand", cmd);
    }

    public void StartReading() {
        if (!isConnected || isReading) return;
        isReading = true; onSetUP?.Invoke();
    }

    public void StopReading()
    {
        isReading = false;
    }

    void ParseResponse(string res) {
        try {
            string[] p = Clean(res).Split(' ');

            if (p.Length < 2)
                return;

            if (p[0] != "41")
                return;

            string pid = p[1];

            switch (pid) {
                case "0C":
                    if (p.Length >= 4) {
                        int A = Convert.ToInt32(p[2], 16);
                        int B = Convert.ToInt32(p[3], 16);
                        rpm = ((A * 256) + B) / 4f;
                    } break;

                case "0D":
                    if (p.Length >= 3) {
                        speed = Convert.ToInt32(p[2], 16);
                    } break;

                case "11":
                    if (p.Length >= 3) {
                        throttle = (Convert.ToInt32(p[2], 16) * 100f) / 255f;
                    } break;

                case "04":
                    if (p.Length >= 3) {
                        engineLoad = (Convert.ToInt32(p[2], 16) * 100f) / 255f;
                    } break;

                case "2F":
                    if (p.Length >= 3) {
                        fuelLevel = (Convert.ToInt32(p[2], 16) * 100f) / 255f;
                    } break;

                case "05":
                    if (p.Length >= 3) {
                        coolantTemp = Convert.ToInt32(p[2], 16) - 40f;
                    } break;
            }
        }
        catch { }
    }

    private string Clean(string res)
    {
        res = res.Replace("\r", "")
                 .Replace("\n", "")
                 .Replace(">", "")
                 .Trim();

        string[] echoes = { "010C", "010D", "0111", "0104", "012F", "0105" };

        foreach (string e in echoes) {
            if (res.StartsWith(e)) {
                res = res.Substring(e.Length).Trim();
                break;
            }
        }

        if (!res.Contains(" ") && res.Length % 2 == 0) {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < res.Length; i += 2) {
                sb.Append(res.Substring(i, 2));
                sb.Append(" ");
            }
            res = sb.ToString().Trim();
        } while (res.Contains("  ")) res = res.Replace("  ", " ");
        return res;
    }

    float ParseRPM(string res)
    {
        try
        {
            string[] p = Clean(res).Split(' ');
            if (p.Length < 4) return float.NaN;
            int A = Convert.ToInt32(p[2], 16);
            int B = Convert.ToInt32(p[3], 16);
            return ((A * 256) + B) / 4f;
        }
        catch { return float.NaN; }
    }

    float ParseSpeed(string res)
    {
        try
        {
            string[] p = Clean(res).Split(' ');
            if (p.Length < 3) return float.NaN;
            int A = Convert.ToInt32(p[2], 16);
            return A;
        }
        catch { return float.NaN; }
    }

    float ParseThrottle(string res)
    {
        try
        {
            string[] p = Clean(res).Split(' ');
            if (p.Length < 3) return float.NaN;
            int A = Convert.ToInt32(p[2], 16);
            return (A * 100f) / 255f;
        }
        catch { return float.NaN; }
    }

    float ParseLoad(string res)
    {
        try
        {
            string[] p = Clean(res).Split(' ');
            if (p.Length < 3) return float.NaN;
            int A = Convert.ToInt32(p[2], 16);
            return (A * 100f) / 255f;
        }
        catch { return float.NaN; }
    }

    float ParseFuel(string res)
    {
        try
        {
            string[] p = Clean(res).Split(' ');
            if (p.Length < 3) return float.NaN;
            int A = Convert.ToInt32(p[2], 16);
            return (A * 100f) / 255f;
        }
        catch { return float.NaN; }
    }

    float ParseCoolant(string res)
    {
        try
        {
            string[] p = Clean(res).Split(' ');
            if (p.Length < 3) return float.NaN;
            int A = Convert.ToInt32(p[2], 16);
            return A - 40f;
        }
        catch { return float.NaN; }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 0) {
            MBts.Clear(); MMenu = GameObject.Find("SettingMenu");
            MMenu2 = GameObject.Find("(OBDMENU");
            deviceList = GameObject.Find("DVL").GetComponent<RectTransform>();
            deviceLObj = GameObject.Find("DVLCL");
            refBt = GameObject.Find("rfbt").GetComponent<Button>();
            ConnectBox = GameObject.Find("DB").GetComponent<Image>();
            CBText = ConnectBox.transform.GetChild(0).GetComponent<Text>();
            MBts.Add(GameObject.Find("KLBTN1")?.GetComponent<Button>());
            MBts.Add(GameObject.Find("KLBTN2")?.GetComponent<Button>());
            MBts[0]?.onClick.AddListener(() => { ToggleKMode(false, isReading); });
            MBts[1]?.onClick.AddListener(() => { ToggleKMode(true, isReading); });

            refBt.onClick.AddListener(() => { RefList(); });

            MMenu2.SetActive(false);
            MMenu.SetActive(false);
            deviceLObj.SetActive(false);

            RefList();
        }
    }

    public void RefList()
    {
        devs = GetPairedDevices();

        foreach (Transform fs in deviceList)
            Destroy(fs);

        for (int f = 0; f < devs.Length; f++)
        {
            GameObject newBtn = Instantiate(deviceLObj, deviceList);
            newBtn.SetActive(true);

            newBtn.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(0f, -10f + (-120f * f));

            int splitpos = devs[f].IndexOf('|');
            int lbf = f;

            newBtn.transform.GetChild(0).GetComponent<Text>().text =
                devs[f].Remove(splitpos);

            newBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(AttemptConnect(devs[lbf]));
            });
        }

        deviceList.sizeDelta =
            new Vector2(deviceList.sizeDelta.x, 20f + (120f * devs.Length));
    }

    public System.Collections.IEnumerator AttemptConnect(string full)
    {
        ConnectBox.color = Color.white;
        CBText.fontSize = 38;
        CBText.text = "Connecting...";

        string[] split = full.Split('|');

        yield return null;

        bool connct = Connect(split[1]);

        if (connct) {
            ConnectBox.color = new Color(0.45f, 1f, 0.5f);
            CBText.fontSize = 60;
            CBText.text = split[0];
            nextSendTime = Time.time;
            int useK = PlayerPrefs.GetInt("KLINEMODE", 0);
            obd.Call<string>("getAndClearResponse");
            StartReading(); ToggleKMode(useK == 1, true);
        }
        else
        {
            ConnectBox.color = Color.red;
            CBText.text = "Connection Failed";
        }
    }
}