package com.example.unitymediabridge;

import android.media.MediaMetadata;
import android.media.session.MediaController;
import android.media.session.PlaybackState;
import com.unity3d.player.UnityPlayer;
import android.app.Activity;
import android.content.Intent;

public class MediaBridge {

    public static MediaController currentController;
    public static String unityObject = "MediaManager";

    public static void SetController(MediaController controller) {
        currentController = controller;
        if (currentController == null) return;
        currentController.registerCallback(new MediaController.Callback() {

            @Override
            public void onMetadataChanged(MediaMetadata metadata) {
                UnityPlayer.UnitySendMessage(unityObject, "OnSongChanged", "");
            }

            @Override
            public void onPlaybackStateChanged(PlaybackState state) {
                UnityPlayer.UnitySendMessage(unityObject, "OnPlaybackStateChanged", "");
            }
        });
    }

   // public static void StartAudioCapture(Activity activity) {
 //       Intent intent = new Intent(activity, AudioCaptureActivity.class);
 //       activity.startActivity(intent);
 //   }

    public static boolean IsConnected() {
        return currentController != null;
    }

    public static void SetUnityObject(String obj) {
        unityObject = obj;
    }

    public static String GetTitle() {
        try {
            if (currentController == null) return "";
            MediaMetadata m = currentController.getMetadata();
            if (m == null) return "";
            CharSequence s = m.getText(MediaMetadata.METADATA_KEY_TITLE);
            return s != null ? s.toString() : "";
        } catch (Exception e) {
            return "";
        }
    }

    public static String GetArtist() {
        try {
            if (currentController == null) return "";
            MediaMetadata m = currentController.getMetadata();
            if (m == null) return "";
            CharSequence s = m.getText(MediaMetadata.METADATA_KEY_ARTIST);
            return s != null ? s.toString() : "";
        } catch (Exception e) {
            return "";
        }
    }

    public static String GetAlbum() {
        try {
            if (currentController == null) return "";
            MediaMetadata m = currentController.getMetadata();
            if (m == null) return "";
            CharSequence s = m.getText(MediaMetadata.METADATA_KEY_ALBUM);
            return s != null ? s.toString() : "";
        } catch (Exception e) {
            return "";
        }
    }

    public static long GetDuration() {
        try {
            if (currentController == null) return 0;
            MediaMetadata m = currentController.getMetadata();
            if (m == null) return 0;
            return m.getLong(MediaMetadata.METADATA_KEY_DURATION);
        } catch (Exception e) {
            return 0;
        }
    }

    public static long GetPosition() {
        try {
            if (currentController == null) return 0;
            PlaybackState s = currentController.getPlaybackState();
            if (s == null) return 0;
            return s.getPosition();
        } catch (Exception e) {
            return 0;
        }
    }

    public static boolean IsPlaying() {
        try {
            if (currentController == null) return false;
            PlaybackState s = currentController.getPlaybackState();
            return s != null && s.getState() == PlaybackState.STATE_PLAYING;
        } catch (Exception e) {
            return false;
        }
    }

    public static void Play() {
        try {
            if (currentController != null)
                currentController.getTransportControls().play();
        } catch (Exception ignored) {}
    }

    public static void Pause() {
        try {
            if (currentController != null)
                currentController.getTransportControls().pause();
        } catch (Exception ignored) {}
    }

    public static void TogglePlayPause() {
        if (IsPlaying()) Pause();
        else Play();
    }

    public static void Next() {
        try {
            if (currentController != null)
                currentController.getTransportControls().skipToNext();
        } catch (Exception ignored) {}
    }

    public static void Previous() {
        try {
            if (currentController != null)
                currentController.getTransportControls().skipToPrevious();
        } catch (Exception ignored) {}
    }

    public static void Seek(long ms) {
        try {
            if (currentController != null)
                currentController.getTransportControls().seekTo(ms);
        } catch (Exception ignored) {}
    }
}