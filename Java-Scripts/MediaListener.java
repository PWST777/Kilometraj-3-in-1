package com.example.unitymediabridge;

import android.content.ComponentName;
import android.media.MediaMetadata;
import android.media.session.MediaController;
import android.media.session.MediaSessionManager;
import android.media.session.PlaybackState;
import android.service.notification.NotificationListenerService;

import com.unity3d.player.UnityPlayer;

import java.util.List;

public class MediaListener
        extends NotificationListenerService {

    MediaSessionManager manager;

    MediaController.Callback callback =
            new MediaController.Callback() {

                @Override
                public void onMetadataChanged(MediaMetadata metadata) {
                    SendUnityMessage("OnSongChanged");
                }

                @Override
                public void onPlaybackStateChanged(PlaybackState state) {
                    SendUnityMessage("OnPlaybackStateChanged");
                }
            };

    @Override
    public void onCreate() {
        super.onCreate();
        try {
            manager = (MediaSessionManager) getSystemService(MEDIA_SESSION_SERVICE);
            RefreshController();
        } catch(Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public void onNotificationPosted(android.service.notification.StatusBarNotification sbn) {
        RefreshController();
    }

    @Override
    public void onNotificationRemoved(android.service.notification.StatusBarNotification sbn) {
        RefreshController();
    }

    void RefreshController() {
        try {
            if(manager == null) return;
            List<MediaController> sessions = manager.getActiveSessions(new ComponentName(this, MediaListener.class));
            if(sessions == null) return;
            if(sessions.size() <= 0) return;
            MediaController controller = sessions.get(0);
            if(controller == null) return;
            MediaBridge.SetController(controller);
            controller.registerCallback(callback);
            SendUnityMessage("OnMediaSessionConnected");
        } catch(Exception e) {
            e.printStackTrace();
        }
    }

    void SendUnityMessage(String method) {
        try { UnityPlayer.UnitySendMessage(MediaBridge.unityObject,method,"");
        } catch(Exception e) {
            e.printStackTrace();
        }
    }
}