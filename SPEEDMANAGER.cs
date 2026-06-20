using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SPEEDMANAGER : MonoBehaviour
{

    public float Diameter;
    public float ActiveSpeed;  
    public float InterpSpeed;
    public long DistanceTicks;
    public double DistanceMade;
    public long TripDistanceTicks;
    public double TripDistanceMade;
    public long TripDistanceTicksSAV;
    public double TripDistanceMadeSAV;
    double TripDistanceMetersAcc;
    public double TimeMade;
    public double TripTimeMade;
    public float MaxSpeed;
    public float AvgSpeed;
    public float RPM;
    public bool MPHMode;

    public float OBDLoad;
    float smoothVelocityL = 0f;
    public float OBDCoolant;
    float smoothVelocityC = 0f;
    public float OBDThrottle;
    float smoothVelocityT = 0f;
    public float OBDFuel;
    float smoothVelocityF = 0f;
    public float OBDRf;
    public DateTime las;

    float smoothVelocity = 0f;
    float smoothVelocity2 = 0f;
    float smoothVelocity3 = 0f;
    float smoothVelocityRPM = 0f;
    float lastcval = 0;
    DateTime lastTick;
    DateTime currentTick;
    bool haspt = false;
    float Timr = 0f;
    float msf = 0f;
    float gpstime;
    public float interpol;
    public int GPSInit;
    public Vector2 lastpos;
    public float CompOrient;

    GameObject Tester0;
    public OBDIIManage obd;
    public DateTime lastspeed;
    public DateTime currentspeed;
    public bool isDueUpdate;
    public int ss;

    public bool AccelerometerAssist = true;
    public float AccelerometerInfluence = 1.0f;
    public float DriftCorrection = 0.005f;
    public bool PortraitMode = false;
    float accelOffset = 0f;
    float referenceSpeed = 0f;
    public Vector3 filteredAccel;
    public Vector3 filteredAccelLAST;
    Vector3 refInterp;
    public float accelCurrent;
    public float idleTime = 0f;
    public List<Vector2> positions;
    public int PrefMode;

    public float lastDiameter;
    public bool LastMPH;

    float curAccel;
    float refAccel;

    public Action onStartTrip;

    void Awake() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void Start() {
        Diameter = PlayerPrefs.GetFloat("DIAMETER"); lastDiameter = Diameter;
        MPHMode = (PlayerPrefs.GetInt("MPHMODE") == 1);
        DistanceTicks = (PlayerPrefs.GetInt("TOTALDISTICKSINT2") * 2147483648L) + PlayerPrefs.GetInt("TOTALDISTICKSINT1");
        TimeMade = ((double)((PlayerPrefs.GetInt("TOTALDISTIMEINT2") * 2147483648d) + PlayerPrefs.GetInt("TOTALDISTIMEINT1")) / 10000d);
        TripDistanceMetersAcc = PlayerPrefs.GetFloat("UTRIPDISTANCE") * 1000.0;
        TripDistanceMade = PlayerPrefs.GetFloat("UTRIPDISTANCE");
        TripTimeMade = PlayerPrefs.GetFloat("UTRIPTIME");
        interpol = PlayerPrefs.GetFloat("SPEEDINTERPOL", 0.7f);
        DriftCorrection = PlayerPrefs.GetFloat("ASSISTINTERPOL", 0.05f);
        AccelerometerInfluence = PlayerPrefs.GetFloat("ASSISTVALUE", 1f) + (DriftCorrection * 2f);
        ss = PlayerPrefs.GetInt("SPEEDSOURCE");
        PrefMode = PlayerPrefs.GetInt("GYROMODE", 0);
        if (SystemInfo.supportsGyroscope) Input.gyro.enabled = true;
        TripDistanceTicksSAV = (long)(TripDistanceMade * 100000 / (Diameter * Mathf.PI));
        if (!MPHMode) TripDistanceTicks = TripDistanceTicksSAV;
        else TripDistanceTicks = Mathf.RoundToInt(TripDistanceTicksSAV * (160900f / 100000f));
        DistanceMade = DistanceTicks * Diameter * Mathf.PI / (MPHMode ? 160900f : 100000f);
        TripDistanceMade = TripDistanceTicks * Diameter * Mathf.PI / (MPHMode ? 160900f : 100000f);
        obd.ontick += GetRf; obd.onSpeed += OnSpeedTick;
        lastspeed = DateTime.Now; lastTick = DateTime.Now;
    }

    public void onModSet(bool allowChange) {
        Diameter = PlayerPrefs.GetFloat("DIAMETER");
        if (Diameter != lastDiameter && allowChange) OnDiameterChange();
        lastDiameter = Diameter;
        DistanceTicks = (PlayerPrefs.GetInt("TOTALDISTICKSINT2") * 2147483648L) + PlayerPrefs.GetInt("TOTALDISTICKSINT1");
        TimeMade = ((double)((PlayerPrefs.GetInt("TOTALDISTIMEINT2") * 2147483648d) + PlayerPrefs.GetInt("TOTALDISTIMEINT1")) / 10000d);
        interpol = PlayerPrefs.GetFloat("SPEEDINTERPOL", 0.7f);
        TripDistanceTicksSAV = (long)(PlayerPrefs.GetFloat("UTRIPDISTANCE") * 100000 / (Diameter * Mathf.PI));
        if (!MPHMode) TripDistanceTicks = TripDistanceTicksSAV;
        else TripDistanceTicks = Mathf.RoundToInt(TripDistanceTicksSAV * (160900f / 100000f));
        DistanceMade = DistanceTicks * Diameter * Mathf.PI / (MPHMode ? 160900f : 100000f);
        TripDistanceMade = TripDistanceTicks * Diameter * Mathf.PI / (MPHMode ? 160900f : 100000f);
        DriftCorrection = PlayerPrefs.GetFloat("ASSISTINTERPOL", 0.05f);
        AccelerometerInfluence = PlayerPrefs.GetFloat("ASSISTVALUE", 1f) + (DriftCorrection * 2f);
        lastspeed = DateTime.Now; lastTick = DateTime.Now;
        ss = PlayerPrefs.GetInt("SPEEDSOURCE"); PrefMode = PlayerPrefs.GetInt("GYROMODE", 0);
    }

    public void TesterTick() {
        OnWheelTick(1, 0.1f);
    }

    public void OnSpeedTick() {
        currentspeed = DateTime.Now;
        isDueUpdate = true;
    }

    public void GetRf() {
        double dt = DateTime.Now.Subtract(las).TotalSeconds;
        if (dt <= 0.0001) return;
        float hz = (float)(1d / dt);
        OBDRf = OBDRf * 0.6667f + (hz / 3f);
        las = DateTime.Now;
    }

    public void OnWheelTick(uint ticks, float secondsSinceLast) {
        if(ss == 1) {
        Diameter = PlayerPrefs.GetFloat("DIAMETER");
        if (haspt) lastTick = currentTick;
        currentTick = DateTime.Now; if (ticks == 0 || secondsSinceLast <= 0.025f) { haspt = true; return; }
        if (SceneManager.GetActiveScene().buildIndex == 2) { DistanceTicks += (int)ticks; TripDistanceTicks += (int)ticks;
            TripDistanceMetersAcc += ticks * Diameter * Math.PI; }
        float distance = ticks * Diameter * Mathf.PI;
        if (!MPHMode) DistanceMade = DistanceTicks * Diameter * Mathf.PI / 100000f;
        else DistanceMade = DistanceTicks * Diameter * Mathf.PI / 160900f;
        if (!MPHMode) TripDistanceMade = TripDistanceTicks * Diameter * Mathf.PI / 100000f;
        else TripDistanceMade = TripDistanceTicks * Diameter * Mathf.PI / 160900f;
        float msf = secondsSinceLast * 1000f; if (msf <= 3000f && SceneManager.GetActiveScene().buildIndex == 2) {
        TimeMade += (msf / 1000f); TripTimeMade += (msf / 1000f);
        } if (msf > 0) {
            float metersPerMs = distance / msf;
            if(!AccelerometerAssist) { 
            if (MPHMode) ActiveSpeed = metersPerMs * 3600f / 160.934f;
            else ActiveSpeed = metersPerMs * 3600f / 100f; }
            else { if (MPHMode) referenceSpeed = metersPerMs * 3600f / 160.934f;
            else referenceSpeed = metersPerMs * 3600f / 100f; accelOffset = 0f;
            } if (SceneManager.GetActiveScene().buildIndex == 2) MaxSpeed = Mathf.Max(MaxSpeed, ActiveSpeed);
            AvgSpeed = (float)(TripDistanceMade / TripTimeMade * 3600); Timr = 0f;
        } haspt = true;
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            if (Tester0 == null) Tester0 = GameObject.Find("MYTESTER");
            Tester0.GetComponent<BluetoothMenu>().OnTickSpeed();
            interpol = PlayerPrefs.GetFloat("SPEEDINTERPOL", 0.33f);
        } if (SceneManager.GetActiveScene().buildIndex == 2) {
            PlayerPrefs.SetInt("TOTALDISTICKSINT1", (int)(DistanceTicks % 2147483648L));
            PlayerPrefs.SetInt("TOTALDISTICKSINT2", (int)(DistanceTicks / 2147483648L));
            TripDistanceTicksSAV = (long)(TripDistanceMetersAcc * 100000.0 / (Diameter * Math.PI));
            PlayerPrefs.SetFloat("UTRIPDISTANCE", (float)(TripDistanceMetersAcc / 1000.0));
            PlayerPrefs.SetFloat("UTRIPTIME", (float)TripTimeMade);
            if (TripTimeMade < 5) onStartTrip?.Invoke();
            long TimeMadeInt = (long)(TimeMade * 10000d);
            int T1 = (int)(TimeMadeInt % 2147483648L); int T2 = (int)(TimeMadeInt / 2147483648L);
            PlayerPrefs.SetInt("TOTALDISTIMEINT1", T1); PlayerPrefs.SetInt("TOTALDISTIMEINT2", T2); }
        }
    }

    public void InitGPS() {
        StartCoroutine(GPSI());
    }

    public IEnumerator GPSI() {
        if (!Input.location.isEnabledByUser) {
            GPSInit = 2; yield break;
        } Input.compass.enabled = true;
        Input.location.Start(1f, 0.1f);
        GPSInit = 3; int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
            yield return new WaitForSeconds(1);
            maxWait--;
        } if (maxWait <= 0) {
            GPSInit = 4; yield break;
        } if (Input.location.status == LocationServiceStatus.Failed) {
            GPSInit = 5; yield break;
        }
        lastpos = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
        lastTick = DateTime.Now; GPSInit = 1;
    }

    public void OnDiameterChange() {
        long dts = (PlayerPrefs.GetInt("TOTALDISTICKSINT2") * 2147483648L) + PlayerPrefs.GetInt("TOTALDISTICKSINT1");
        decimal newTicks = dts * ((decimal)lastDiameter / (decimal)Diameter);
        long NEWdts = (long)newTicks;
        int T1 = (int)(NEWdts % 2147483648L); int T2 = (int)(NEWdts / 2147483648L);
        PlayerPrefs.SetInt("TOTALDISTICKSINT1", T1); PlayerPrefs.SetInt("TOTALDISTICKSINT2", T2);
        lastDiameter = Diameter;
    }

    double CalcDistance(Vector2 pos1, Vector2 pos2) {
        double R = 6371000;
        double lat1Rad = pos1.x * Mathf.Deg2Rad;
        double lat2Rad = pos2.x * Mathf.Deg2Rad;
        double deltaLat = (pos2.x - pos1.x) * Mathf.Deg2Rad;
        double deltaLon = (pos2.y - pos1.y) * Mathf.Deg2Rad;
        double a = Mathf.Sin((float)deltaLat / 2) * Mathf.Sin((float)deltaLat / 2) +
                  Mathf.Cos((float)lat1Rad) * Mathf.Cos((float)lat2Rad) *
                  Mathf.Sin((float)deltaLon / 2) * Mathf.Sin((float)deltaLon / 2);
        double c = 2f * Mathf.Atan2(Mathf.Sqrt((float)a), Mathf.Sqrt(1 - (float)a));
        return R * c;
    }

    void FixedUpdate() {
        if (ss == 0 && Input.location.status == LocationServiceStatus.Running) {
            gpstime += Time.fixedDeltaTime;
            if (gpstime >= 0.033334f) {
                Vector2 currentPosition = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
                if (currentPosition != lastpos) {
                    currentTick = DateTime.Now; float deltaTime = (float)((currentTick - lastTick).TotalSeconds);
                    double distanceLastTickMeters = CalcDistance(lastpos, currentPosition);
                    if (distanceLastTickMeters > 2f && deltaTime < 3f) {
                        if(!AccelerometerAssist) { 
                        if (!MPHMode) ActiveSpeed = (float)(distanceLastTickMeters / deltaTime) * 3.6f;
                        else ActiveSpeed = (float)(distanceLastTickMeters / deltaTime) * 2.236f; }
                        else { if (!MPHMode) referenceSpeed = (float)(distanceLastTickMeters / deltaTime) * 3.6f;
                        else referenceSpeed = (float)(distanceLastTickMeters / deltaTime) * 2.236f; accelOffset = 0f; }
                        if (AccelerometerAssist) referenceSpeed = Mathf.Min(referenceSpeed, 9999.9f);
                        else ActiveSpeed = Mathf.Min(ActiveSpeed, 9999.9f);
                        msf = deltaTime * 1000f; if (SceneManager.GetActiveScene().buildIndex == 2) {
                            TripDistanceMetersAcc += distanceLastTickMeters; TripDistanceMade += (distanceLastTickMeters / 1000f);
                            DistanceMade += (distanceLastTickMeters / 1000d);
                            TimeMade += deltaTime; TripTimeMade += deltaTime;
                            AvgSpeed = (float)(TripDistanceMade / TripTimeMade * 3600); MaxSpeed = Mathf.Max(MaxSpeed, AccelerometerAssist ? referenceSpeed : ActiveSpeed);
                            if (!MPHMode) {
                                double MyVal = ((DistanceMade / Diameter / Mathf.PI) * 100000d); long MyVal2 = (long)(MyVal);
                                int Int1s = (int)(MyVal2 % 2147483648L); int Int2s = (int)(MyVal2 / 2147483648L);
                                PlayerPrefs.SetInt("TOTALDISTICKSINT1", Int1s); PlayerPrefs.SetInt("TOTALDISTICKSINT2", Int2s);
                                DistanceTicks = MyVal2;
                            } else if (MPHMode) {
                                double MyVal = ((DistanceMade / Diameter / Mathf.PI) * 160900d);
                                long MyVal2 = (long)(MyVal);
                                int Int1s = (int)(MyVal2 % 2147483648L); int Int2s = (int)(MyVal2 / 2147483648L);
                                PlayerPrefs.SetInt("TOTALDISTICKSINT1", Int1s); PlayerPrefs.SetInt("TOTALDISTICKSINT2", Int2s);
                                DistanceTicks = MyVal2; }
                            long ValL = (long)(TimeMade * 10000d);
                            int T1 = (int)(ValL % 2147483648L); int T2 = (int)(ValL / 2147483648L);
                            PlayerPrefs.SetInt("TOTALDISTIMEINT1", T1); PlayerPrefs.SetInt("TOTALDISTIMEINT2", T2);
                            TripDistanceTicksSAV = (long)(TripDistanceMetersAcc * 100000.0 / (Diameter * Math.PI));
                            PlayerPrefs.SetFloat("UTRIPDISTANCE", (float)(TripDistanceMetersAcc / 1000.0));
                            PlayerPrefs.SetFloat("UTRIPTIME", (float)TripTimeMade);
                            if (TripTimeMade < 5) onStartTrip?.Invoke();
                        } lastpos = currentPosition; lastTick = currentTick; haspt = true; Timr = 0f;
                    }
                } gpstime -= 0.033334f; lastpos = currentPosition; lastTick = currentTick;
            } if (haspt) Timr += Time.fixedDeltaTime;
            if ((msf / 1000f) + 2.3f > Timr) { InterpSpeed = Mathf.SmoothDamp(InterpSpeed, ActiveSpeed, ref smoothVelocity2, Mathf.Max(interpol * 2.5f, 0f)); }
            else { if (!AccelerometerAssist) ActiveSpeed = 0f; else referenceSpeed = Mathf.Lerp(referenceSpeed, 0f, 0.03f);
                InterpSpeed = Mathf.SmoothDamp(InterpSpeed, ActiveSpeed, ref smoothVelocity2, Mathf.Max(((interpol * 4f) + 1f) - (Timr / 2f), Mathf.Max(interpol * 2.5f, 0f))); }
        } else if (ss == 1) { if (haspt) Timr += Time.fixedDeltaTime;
            if ((msf / 1000f) + 0.9f > Timr) { InterpSpeed = Mathf.SmoothDamp(InterpSpeed, ActiveSpeed, ref smoothVelocity, Mathf.Max(interpol, 0f)); }
            else { if (!AccelerometerAssist) ActiveSpeed = 0f; else referenceSpeed = Mathf.Lerp(referenceSpeed, 0f, 0.03f);
                InterpSpeed = Mathf.SmoothDamp(InterpSpeed, ActiveSpeed, ref smoothVelocity, Mathf.Max(((interpol * 2) + 0.6f) - (Timr / 2f), Mathf.Max(interpol, 0f))); } }
        if (ss == 2) {
            if (isDueUpdate) {
                if (AccelerometerAssist) { referenceSpeed = obd.speed; accelOffset = 0f;
                } else { ActiveSpeed = obd.speed; }
                TimeSpan dif = currentspeed.Subtract(lastspeed);
                lastspeed = currentspeed;
                float speedKmh = AccelerometerAssist ? referenceSpeed : ActiveSpeed;
                float speedMps = speedKmh / 3.6f;
                float calcdDist = speedMps * (float)dif.TotalSeconds;
                DistanceMade += calcdDist / 1000d;
                TripDistanceMade += calcdDist / 1000f;
                TripDistanceMetersAcc += calcdDist;
                TimeMade += dif.TotalSeconds;
                TripTimeMade += (float)dif.TotalSeconds;
                isDueUpdate = false;
                if (!MPHMode) {
                    double MyVal = ((DistanceMade / Diameter / Mathf.PI) * 100000d); long MyVal2 = (long)(MyVal);
                    int Int1s = (int)(MyVal2 % 2147483648L); int Int2s = (int)(MyVal2 / 2147483648L);
                    PlayerPrefs.SetInt("TOTALDISTICKSINT1", Int1s); PlayerPrefs.SetInt("TOTALDISTICKSINT2", Int2s);
                    DistanceTicks = MyVal2;
                } else if (MPHMode) {
                    double MyVal = ((DistanceMade / Diameter / Mathf.PI) * 160900d);
                    long MyVal2 = (long)(MyVal);
                    int Int1s = (int)(MyVal2 % 2147483648L); int Int2s = (int)(MyVal2 / 2147483648L);
                    PlayerPrefs.SetInt("TOTALDISTICKSINT1", Int1s); PlayerPrefs.SetInt("TOTALDISTICKSINT2", Int2s);
                    DistanceTicks = MyVal2; }
                long ValL = (long)(TimeMade * 10000d);
                int T1 = (int)(ValL % 2147483648L); int T2 = (int)(ValL / 2147483648L);
                PlayerPrefs.SetInt("TOTALDISTIMEINT1", T1); PlayerPrefs.SetInt("TOTALDISTIMEINT2", T2);
                TripDistanceTicksSAV = (long)(TripDistanceMetersAcc * 100000.0 / (Diameter * Math.PI));
                PlayerPrefs.SetFloat("UTRIPDISTANCE", (float)(TripDistanceMetersAcc / 1000.0));
                PlayerPrefs.SetFloat("UTRIPTIME", (float)TripTimeMade);
                if (TripTimeMade < 5) onStartTrip?.Invoke();
            } float displaySpeed = ActiveSpeed;
            if (MPHMode) displaySpeed = ActiveSpeed / 1.60934f;
            InterpSpeed = Mathf.SmoothDamp(InterpSpeed, displaySpeed, ref smoothVelocity, Mathf.Max(interpol / 2f, 0f));
            RPM = Mathf.SmoothDamp(RPM, obd.rpm, ref smoothVelocityRPM, Mathf.Max(interpol / 2f, 0f));
            OBDThrottle = Mathf.SmoothDamp(OBDThrottle, obd.throttle, ref smoothVelocityT, Mathf.Max(interpol, 0f));
            OBDLoad = Mathf.SmoothDamp(OBDLoad, obd.engineLoad, ref smoothVelocityL, Mathf.Max(interpol, 0f));
            OBDCoolant = Mathf.SmoothDamp(OBDCoolant, MPHMode ? 32f + (obd.coolantTemp * 1.8f) : obd.coolantTemp, ref smoothVelocityC, Mathf.Max(interpol * 4f, 0f) );
            OBDFuel = Mathf.SmoothDamp(OBDFuel, obd.fuelLevel, ref smoothVelocityF, Mathf.Max(interpol * 8f, 0f));
        } else {
            if (!MPHMode) RPM = (InterpSpeed * 1666.66f) / (3.1415f * Diameter);
            else RPM = (InterpSpeed * 2682f) / (3.1415f * Diameter); }
        if (GPSInit == 1) { lastcval = Input.compass.trueHeading;
            CompOrient = Mathf.SmoothDampAngle(CompOrient, lastcval, ref smoothVelocity3, Mathf.Max(interpol, 0f)); }
        UpdateAccelerometerAssist();
    }

    void UpdateAccelerometerAssist() {
        if (!AccelerometerAssist) {
            accelOffset = 0f; return;
        } float dt = Time.fixedDeltaTime;
        Vector3 accel = SystemInfo.supportsGyroscope ? Input.gyro.userAcceleration : Input.acceleration;
        filteredAccel = Vector3.Lerp(filteredAccel, accel, dt * 6f);
        // filteredAccel = Vector3.SmoothDamp(filteredAccel, Vector3.Lerp(filteredAccel, accel, dt * 6f), ref refInterp, Mathf.Max(0f, DriftCorrection - 0.02f));
        Vector3 forwardDir;
        if (!PortraitMode) {
            forwardDir = new Vector3(0f, 1f, 1f);
        } else {
            forwardDir = new Vector3(1f, 0f, 1f);
        } forwardDir.Normalize();
        float forwardAccel = -Vector3.Dot(filteredAccel, forwardDir);
        if(forwardAccel > 0.0001f) { 
        if (PrefMode == 1) {
            forwardAccel = Mathf.Pow(forwardAccel, 0.7f);
                forwardAccel *= 0.8f;
        } else if(PrefMode == 2) {
            forwardAccel = Mathf.Pow(forwardAccel, 0.2f);
                forwardAccel *= 0.4f;
        } }
        curAccel = Mathf.SmoothDamp(curAccel, forwardAccel, ref refAccel, Mathf.Max(0f, DriftCorrection - 0.02f));
        float sideAccel; // don't ever test like this again
        if (Mathf.Abs(forwardAccel) < 0.005f) { forwardAccel = 0f; sideAccel = 0f; }
        else { sideAccel = -((PortraitMode) ? filteredAccel.y : filteredAccel.x); }
        float sideInfluence = Mathf.Abs(sideAccel) * 0.1f;
        float combinedAccel = forwardAccel + Mathf.Sign(forwardAccel) * sideInfluence;
        accelCurrent = combinedAccel;
        accelOffset += combinedAccel * AccelerometerInfluence;
        if (Mathf.Abs(filteredAccel.x) + Mathf.Abs(filteredAccel.y) + Mathf.Abs(filteredAccel.z) > 0.12f) idleTime = 0f;
        else idleTime += Time.fixedDeltaTime;
        if (idleTime > ((0.12f - DriftCorrection) * 20f)) accelOffset = Mathf.Lerp(accelOffset, 0f, DriftCorrection);
        accelOffset = Mathf.Clamp(accelOffset, -69f, 69f);
        if(filteredAccel != filteredAccelLAST) {
            positions.Add(new Vector2(Mathf.Clamp(sideAccel * 2f, -1f, 1f) * 200f, Mathf.Clamp(forwardAccel * 2f, -1f, 1f) * 200f));
            while (positions.Count > 60) positions.RemoveAt(0);
        } filteredAccelLAST = filteredAccel;
        if (accelOffset < 0f) {
            float speed = Mathf.Max(ActiveSpeed, 1f);
            float recovery = (1f / speed) * 0.5f + (-accelOffset * 0.02f) + 0.002f;
            accelOffset += recovery * Time.fixedDeltaTime * 60f;
        } ActiveSpeed = Mathf.Max(referenceSpeed + accelOffset, 0f);
    }

}
