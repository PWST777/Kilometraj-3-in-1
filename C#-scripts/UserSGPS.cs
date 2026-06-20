using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserSGPS : MonoBehaviour
{

    public SPEEDMANAGER sm;
    public GameObject MainP;
    public GameObject LoadingGPS;
    public GameObject FailedGPS;
    public GameObject NoGPS;
    public GameObject TimeOutGPS;
    public int wow;

    void Start() {
        if (PlayerPrefs.GetInt("PRESET" + PlayerPrefs.GetInt("SELECTEDPRESET") + "PRESHASGPSELEMS") == 1) {
            sm = GameObject.Find("MYBLUETOOTH").GetComponent<SPEEDMANAGER>(); sm.InitGPS(); MainP.SetActive(true);
        }
    }

    void FixedUpdate() {
        wow++; if(wow > 16 && MainP.activeInHierarchy) { 
        if (sm.GPSInit == 1) { MainP.SetActive(false); } wow -= 15;
        if (sm.GPSInit == 2) { NoGPS.SetActive(true); LoadingGPS.SetActive(false); FailedGPS.SetActive(false); TimeOutGPS.SetActive(false); }
        if (sm.GPSInit == 3) { NoGPS.SetActive(false); LoadingGPS.SetActive(true); FailedGPS.SetActive(false); TimeOutGPS.SetActive(false); }
        if (sm.GPSInit == 4) { NoGPS.SetActive(false); LoadingGPS.SetActive(false); FailedGPS.SetActive(false); TimeOutGPS.SetActive(true); }
        if (sm.GPSInit == 5) { NoGPS.SetActive(true); LoadingGPS.SetActive(false); FailedGPS.SetActive(false); TimeOutGPS.SetActive(false); } }
    }

    public void TryAgain() {
        sm.InitGPS();
    }
}
