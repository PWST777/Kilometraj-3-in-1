using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlueManage : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Bluetooth BLE
    // Folosit pentru primirea datelor prin bluetooth in modul BLE
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    public static BlueManage _instance; // Instanta classei pentru a nu a aparea de 2 ori

    // Lista de elenete si proprietati pentru clasa
    public GameObject myyButton;
    public Transform deviceListPanel;
    public Text statusText;
    public GameObject ConnectedBUtton;
    public GameObject BatteryButton;
    public SPEEDMANAGER SMg; // Clasa universala pentru variabile de viteza, turatie, etc.
    public Image BL;
    public Button FFFF;
    public List<Sprite> states;

    private Dictionary<string, string> discoveredDevices = new Dictionary<string, string>();
    private bool connected = false;
    private string connectedDeviceAddress = "";
    private string connectedDeviceName = "";
    private int Devicdf = 0;

    // Valori folosite pentru obtinerea datelor prin bluetooth
    const string CSC_SERVICE = "1816";
    const string CSC_MEASUREMENT_CHAR = "2A5B";
    const string BATTERY_SERVICE = "180F";
    const string BATTERY_LEVEL_CHAR = "2A19";

    private uint lastRevCount = 0;
    private ushort lastEventTime = 0;

    // Functie executata cand elementul este creat
    void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
            Destroy(gameObject);
            return;
        }
    }

    // Functie executata cand elementul este activat pentru prima data
    void Start() {
        BluetoothLEHardwareInterface.Initialize(true, false,
            () => Debug.Log("BLE Initialized"),
            (error) => Debug.LogError("BLE Init Error: " + error));
        SMg = GetComponent<SPEEDMANAGER>();
        SMg.obd.onConnect += () => { BL.sprite = states[0];
            BL.transform.GetChild(0).GetComponent<Image>().sprite = states[2];
            BL.transform.GetChild(1).GetComponent<Image>().sprite = states[4];
            BL.transform.GetChild(2).GetComponent<Image>().sprite = states[6]; };
        SMg.obd.onSetUP += () => { BL.sprite = states[1];
            BL.transform.GetChild(0).GetComponent<Image>().sprite = states[3];
            BL.transform.GetChild(1).GetComponent<Image>().sprite = states[5];
            BL.transform.GetChild(2).GetComponent<Image>().sprite = states[7]; };
        dothisbullshet();
    }

    // Functie executata pentru a incepe scanarea elementelor
    public void StartScanning() {
        BluetoothLEHardwareInterface.StopScan();
        discoveredDevices.Clear();
        Devicdf = 0;
        foreach (Transform child in deviceListPanel)
            Destroy(child.gameObject);
        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) => {
            if (!discoveredDevices.ContainsKey(address)) {
                discoveredDevices[address] = name;
                CreateDeviceButton(address, name);
                if (SceneManager.GetActiveScene().buildIndex == 0)
                    statusText.text = discoveredDevices.Count + " Devices Found";
            }
        }, null, false);
    }

    // Functie folosita pentru a crea butoane pentru interfata
    void CreateDeviceButton(string address, string name) {
        GameObject newButton = Instantiate(myyButton, deviceListPanel);
        newButton.SetActive(true);
        newButton.transform.GetChild(0).GetComponent<Text>().text = string.IsNullOrEmpty(name) ? "Device" : name;
        newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, Devicdf * -120f);
        deviceListPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(
            deviceListPanel.GetComponent<RectTransform>().sizeDelta.x, 120f * (Devicdf + 1));
        Devicdf++;
        string capturedAddress = address;
        string capturedName = name;
        newButton.GetComponent<Button>().onClick.AddListener(() => {
            ConnectToDevice(capturedAddress, capturedName);
        });
    }

    // Functie executata pentru a conecta aplicatia la un dispozitiv
    void ConnectToDevice(string address, string name) {
        if (connected && !string.IsNullOrEmpty(connectedDeviceAddress)) {
            BluetoothLEHardwareInterface.DisconnectPeripheral(connectedDeviceAddress, (deviceAddress) => {
                connected = false;
                connectedDeviceAddress = "";
                connectedDeviceName = "";
                BL.sprite = states[0];
                BL.transform.GetChild(0).GetComponent<Image>().sprite = states[2];
                BL.transform.GetChild(1).GetComponent<Image>().sprite = states[4];
                BL.transform.GetChild(2).GetComponent<Image>().sprite = states[6];
                BatteryButton.SetActive(false);
                ConnectedBUtton.SetActive(false);
                DoConnect(address, name);
            });
        } else {
            DoConnect(address, name);
        }
    }

    // Functie executata pentru a conecta aplicatia pentru a primi date de la un dispozitiv
    void DoConnect(string address, string name) {
        BluetoothLEHardwareInterface.StopScan();
        BluetoothLEHardwareInterface.ConnectToPeripheral(address, null, null, (deviceAddress, serviceUUID, characteristicUUID) => {
            connected = true; BL.sprite = states[1];
            BL.transform.GetChild(0).GetComponent<Image>().sprite = states[3];
            BL.transform.GetChild(1).GetComponent<Image>().sprite = states[5];
            BL.transform.GetChild(2).GetComponent<Image>().sprite = states[7];
            connectedDeviceAddress = deviceAddress;
            connectedDeviceName = name;
            if (SceneManager.GetActiveScene().buildIndex == 0) {
                ConnectedBUtton.SetActive(true);
                ConnectedBUtton.transform.GetChild(0).GetComponent<Text>().text = name;
            }
            TryReadBatteryLevel(deviceAddress);
            SubscribeToCSC(deviceAddress);
        });
    }

    // Functie executata pentru a primi nivelul de baterie de la senzor
    void TryReadBatteryLevel(string deviceAddress) {
        BluetoothLEHardwareInterface.ReadCharacteristic(deviceAddress, BATTERY_SERVICE, BATTERY_LEVEL_CHAR,
            (characteristic, data) => {
                if (data != null && data.Length > 0) {
                    int batteryLevel = data[0];
                    if (SceneManager.GetActiveScene().buildIndex == 0) {
                        BatteryButton.SetActive(true);
                        BatteryButton.transform.GetChild(2).GetComponent<Text>().text = batteryLevel.ToString();
                    }
                } else {
                    BatteryButton.SetActive(false);
                }
            });
    }

    // Functie executata pentru a conecta aplicatia pentru a primi date de la un dispozitiv
    void SubscribeToCSC(string deviceAddress) {
        BluetoothLEHardwareInterface.SubscribeCharacteristic(deviceAddress, CSC_SERVICE, CSC_MEASUREMENT_CHAR, null,
            (characteristicUUID, data) => {
                if (data != null && data.Length > 6) {
                    byte flags = data[0]; int index = 1;
                    bool hasWheelData = (flags & 0x01) != 0;
                    if (hasWheelData && data.Length >= index + 6) {
                        uint cumulativeRevs = (uint)(data[index] | (data[index + 1] << 8) | (data[index + 2] << 16) | (data[index + 3] << 24));
                        index += 4;
                        ushort eventTime = (ushort)(data[index] | (data[index + 1] << 8));
                        uint tickCount = cumulativeRevs - lastRevCount;
                        int eventTimeDiff = eventTime - lastEventTime;
                        if (eventTimeDiff < 0) eventTimeDiff += 65536;
                        float seconds = eventTimeDiff / 1024f;
                        if (tickCount > 0) {
                            SMg.OnWheelTick(tickCount, seconds);
                            lastRevCount = cumulativeRevs;
                            lastEventTime = eventTime;
                        }
                    }
                }
            });
    }

    // Functie executata cand elementul este dezactivat
    void OnDisable() {
        BluetoothLEHardwareInterface.StopScan();
    }

    // Functie executata cand o alta scena este incarcata
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex == 0) {
            dothisbullshet();
        }
    }

    // Functie executata pentru a identifica elemente din scena neincarcate
    public void dothisbullshet() {
        while (myyButton == null) myyButton = GameObject.Find("myyBu");
        while (deviceListPanel == null) deviceListPanel = GameObject.Find("DevLiPa")?.transform;
        while (statusText == null) statusText = GameObject.Find("StaTe")?.GetComponent<Text>();
        while (ConnectedBUtton == null) ConnectedBUtton = GameObject.Find("ConBu");
        while (BatteryButton == null) BatteryButton = GameObject.Find("BatBu");
        while (BL == null) BL = GameObject.Find("Button (Legacy) bls")?.GetComponent<Image>();
        while (FFFF == null) FFFF = GameObject.Find("Button (Legacy) FFFF")?.GetComponent<Button>();
        FFFF.onClick.AddListener(StartScanning);
        myyButton.SetActive(false);
        ConnectedBUtton.SetActive(false);
        BatteryButton.SetActive(false);
        if (deviceListPanel != null) {
            foreach (Transform child in deviceListPanel)
                Destroy(child.gameObject);
        } discoveredDevices.Clear();
        Devicdf = 0;
        if (connected && !string.IsNullOrEmpty(connectedDeviceAddress)) {
            BluetoothLEHardwareInterface.DisconnectPeripheral(connectedDeviceAddress, null);
            connected = false;
            connectedDeviceAddress = "";
            connectedDeviceName = "";
            BL.sprite = states[0];
            BL.transform.GetChild(0).GetComponent<Image>().sprite = states[2];
            BL.transform.GetChild(1).GetComponent<Image>().sprite = states[4];
            BL.transform.GetChild(2).GetComponent<Image>().sprite = states[6];
        } GameObject.Find("BluetoothMenu")?.SetActive(false);
    }
}
