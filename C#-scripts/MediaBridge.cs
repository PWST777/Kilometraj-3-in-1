using UnityEngine;

public class MediaBridge : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Buffer Audio
    // Folosita pentru a controla playback-ul dispozitivului folosind functii cum ar fi Play, Pauza, etc.
    // Acest cod este activ doar daca modul elementului 'Music Player' este setat pe media player
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    AndroidJavaClass bridge; // Cod Java ce controleaza la nivelul Android playback-ul media player-ului
    public MusicPlayer mainupdate;

    // Functie executata cand elementul este activat pentru prima data 
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        bridge = new AndroidJavaClass("com.example.unitymediabridge.MediaBridge");
        bridge.CallStatic("SetUnityObject", gameObject.name);
#endif
    }

    // Functie ce returneaza daca aplicatia este conectata la media player-ului dispozitivului
    public bool IsConnected()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return bridge.CallStatic<bool>("IsConnected");
#else
        return false;
#endif
    }

    // Functie ce returneaza daca media player-ul ruleaza sau este in pauza
    public bool IsPlaying()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return bridge.CallStatic<bool>("IsPlaying");
#else
        return false;
#endif
    }

    // Functie ce returneaza numele cantecului prezent in media player
    public string GetTitle()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return bridge.CallStatic<string>("GetTitle");
#else
        return "";
#endif
    }

    // Functie ce returneaza compozitorul cantecului prezent in media player
    public string GetArtist()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return bridge.CallStatic<string>("GetArtist");
#else
        return "";
#endif
    }

    // Functie ce returneaza albumul din care se afla cantecul prezent in media player
    public string GetAlbum()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return bridge.CallStatic<string>("GetAlbum");
#else
        return "";
#endif
    }

    // Functie ce returneaza lungimea cantecului prezent in media player in milisecunde
    public long GetDuration()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return bridge.CallStatic<long>("GetDuration");
#else
        return 0;
#endif
    }

    // Functie ce returneaza pozitia cantecului prezent in media player in milisecunde
    public long GetPosition()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return bridge.CallStatic<long>("GetPosition");
#else
        return 0;
#endif
    }

    // Functie ce da play la cantecul din media player
    public void Play()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        bridge.CallStatic("Play");
#endif
    }

    // Functie ce pune pauza la cantecul din media player
    public void Pause()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        bridge.CallStatic("Pause");
#endif
    }

    // Functie ce schimba intre a rula sau a pune pauza la cantecul din media player
    public void TogglePlayPause()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        bridge.CallStatic("TogglePlayPause");
#endif
    }

    // Functie ce schimba cantecul din media player la urmatorul disponibil
    public void Next()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        bridge.CallStatic("Next");
#endif
    }

    // Functie ce schimba cantecul din media player la cel precedent
    public void Previous()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        bridge.CallStatic("Previous");
#endif
    }

    // Functie ce schimba pozitia cantecului la o valoare specificata in milisecunde
    public void Seek(long ms)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        bridge.CallStatic("Seek", ms);
#endif
    }

    // Functie executata cand cantecul din media player este schimbat
    public void OnSongChanged(string msg) {
        mainupdate.ChangeSong(GetTitle(), GetArtist(), GetAlbum(), GetDuration());
    }

    // Functie executata cand starea de playback din media player este schimbata
    public void OnPlaybackStateChanged(string msg)
    {
        mainupdate.savedplay = IsPlaying();
    }

    // [FUNCTIE NEFOLOSITA] Functie executata pentru debug
    public void OnMediaSessionConnected(string msg) {
        // Debug.Log("[MediaBridge] Media session connected");
    }
}
