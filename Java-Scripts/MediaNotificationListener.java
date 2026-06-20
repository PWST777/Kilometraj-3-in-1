package com.example.unitymediabridge;

import android.service.notification.NotificationListenerService;
import android.service.notification.StatusBarNotification;
import android.media.session.MediaController;
import android.media.session.MediaSessionManager;
import android.content.Context;

import java.util.List;

public class MediaNotificationListener extends NotificationListenerService {

    private MediaSessionManager manager;

    @Override
    public void onCreate() {
        super.onCreate();
        manager = (MediaSessionManager) getSystemService(Context.MEDIA_SESSION_SERVICE);
    }

    @Override
    public void onNotificationPosted(StatusBarNotification sbn) {
        updateSessions();
    }

    @Override
    public void onNotificationRemoved(StatusBarNotification sbn) {
        updateSessions();
    }

    private void updateSessions() {
        try {
            if (manager == null) return;
            List<MediaController> controllers = manager.getActiveSessions(null);
            if (controllers == null || controllers.isEmpty()) return;
            MediaController controller = controllers.get(0);
            MediaBridge.SetController(controller);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}