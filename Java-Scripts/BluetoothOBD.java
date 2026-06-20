package com.example.obdplugin;

import android.annotation.SuppressLint;
import android.bluetooth.*;
import android.content.*;
import java.io.*;
import java.util.*;

public class BluetoothOBD {

    private BluetoothSocket socket;
    private OutputStream output;
    private InputStream input;

    private BluetoothAdapter adapter;
    private Context context;

    private Thread readThread;
    private volatile boolean running = false;
    private final StringBuilder buffer = new StringBuilder();
    private volatile String lastResponse = "";

    private final UUID SPP_UUID = UUID.fromString("00001101-0000-1000-8000-00805F9B34FB");

    public BluetoothOBD(Context ctx) {
        this.context = ctx;
        this.adapter = BluetoothAdapter.getDefaultAdapter();
    }

    @SuppressLint("MissingPermission")
    public boolean connect(String macAddress) {
        try {
            if (adapter == null || !adapter.isEnabled()) {
                return false;
            }
            BluetoothDevice device = adapter.getRemoteDevice(macAddress);
            socket = device.createRfcommSocketToServiceRecord(SPP_UUID);
            if (adapter.isDiscovering()) {
                adapter.cancelDiscovery();
            }
            socket.connect();
            output = socket.getOutputStream();
            input = socket.getInputStream();
            startReader();
            return true;

        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }

    private void startReader() {
        running = true;

        readThread = new Thread(() -> {
            try {
                while (running && input != null) {
                    int c = input.read();
                    if (c == -1) break;
                    char ch = (char) c;
                    buffer.append(ch);
                    if (ch == '>') {
                        synchronized (buffer) {
                            lastResponse = buffer.toString();
                            buffer.setLength(0);
                        }
                    }
                }
            } catch (Exception e) {
                e.printStackTrace();
            }
        });
        readThread.start();
    }

    public void disconnect() {
        try {
            running = false;
            if (socket != null) socket.close();
            if (readThread != null) {
                readThread.join(200); // optional wait
            }
        } catch (Exception ignored) {}
    }

    public void sendCommand(String command) {
        try {
            if (output != null) {
                command += "\r";
                output.write(command.getBytes());
                output.flush();
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public String getAndClearResponse() {
        String res = lastResponse;
        lastResponse = "";
        return res;
    }

    @SuppressLint("MissingPermission")
    public String[] getPairedDevices() {
        if (adapter == null) return new String[0];
        Set<BluetoothDevice> devices = adapter.getBondedDevices();
        String[] result = new String[devices.size()];
        int i = 0;
        for (BluetoothDevice device : devices) {
            String name = device.getName();
            if (name == null) name = "Unknown";
            result[i++] = name + "|" + device.getAddress();
        } return result;
    }
    @SuppressLint("MissingPermission")
    public String[] getAvailableDevices() {
        if (adapter == null) return new String[0];
        Set<BluetoothDevice> devices = adapter.getBondedDevices();
        ArrayList<String> result = new ArrayList<>();
        for (BluetoothDevice device : devices) {
            String name = device.getName();
            if (name == null) name = "Unknown";
            result.add(name + "|" + device.getAddress());
        } return result.toArray(new String[0]);
    }
}