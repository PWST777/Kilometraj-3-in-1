using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MainAudioBuffer : MonoBehaviour
{

    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--
    // Clasa Buffer Audio
    // Folosita pentru obtinerea datelor de la microfon pentru elementul 'Music Player'
    // Acest cod este activ doar daca modul elementului 'Music Player' este setat pe media player
    // -/-/-/-/-/-/-/-/-/-/-/-/-/-/-/--

    // Date folosite prestabilite
    const int MAX_BUFFER = 8192;
    float[] floatBuffer = new float[MAX_BUFFER];
    AudioClip micClip;
    string micDevice;

    // Functie executata cand elementul este activat pentru prima data 
    void Start() {
        if (Microphone.devices.Length == 0) {
            Debug.LogError("No microphone found");
            return;
        }
        micDevice = Microphone.devices[0];
        micClip = Microphone.Start(micDevice, true, 10, 44100);
    }

    // Functie chemata in fiecare cadru | Functie folosita pentru a obtine data de la microfonul dispozitivului
    void Update() {
        if (micClip == null) return;
        ReadLatestSamples();
    }

    // Functie propriu-zisa folosita pentru a obtine data audio de la microfon
    void ReadLatestSamples() {
        int micPos = Microphone.GetPosition(micDevice);
        if (micPos < MAX_BUFFER) return;
        int startPos = micPos - MAX_BUFFER;
        micClip.GetData(floatBuffer, startPos);
        for(int i = 0; i < floatBuffer.Length; i++) {
            floatBuffer[i] *= 2f;
        }
    }

    // Functie folosita pentru a transforma data obtinuta intr-un format corect
    public float[] GetSamples(int size) {
        size = Mathf.Clamp(size, 64, MAX_BUFFER);
        float[] result = new float[size];
        System.Array.Copy(floatBuffer, MAX_BUFFER - size, result, 0, size);
        return result;
    }

    // Functie folosita pentru a obtine data folosing algoritmul Fast Fourier Transform
    public float[] GetFFTSamples(int size) {
        size = Mathf.Clamp(size, 1024, MAX_BUFFER);
        if (!Mathf.IsPowerOfTwo(size)) size = Mathf.ClosestPowerOfTwo(size);
        return ComputeFFT(size);
    }

    // Functie folosita pentru a transforma data obtinuta prin algoritmul Fast Fourier Transform
    float[] ComputeFFT(int size) {
        int start = MAX_BUFFER - size;
        float[] buffer = new float[size * 2];
        for (int i = 0; i < size; i++) {
            float sample = floatBuffer[start + i];
            float window = 0.5f - 0.5f * Mathf.Cos(2f * Mathf.PI * i / (size - 1));
            buffer[i * 2] = sample * window;
            buffer[i * 2 + 1] = 0f;
        } FFT(buffer, size);
        float[] result = new float[size / 2];
        for (int i = 0; i < result.Length; i++) {
            float real = buffer[i * 2];
            float imag = buffer[i * 2 + 1];
            result[i] = Mathf.Sqrt(real * real + imag * imag);
            result[i] /= 60f;
        } return result;
    }

    // Functie propriu-zisa folosita pentru a transforma data obtinuta prin algoritmul Fast Fourier Transform
    void FFT(float[] buffer, int n) {
        int j = 0;
        for (int i = 0; i < n; i++) {
            if (i < j) {
                int i2 = i << 1;
                int j2 = j << 1;
                (buffer[i2], buffer[j2]) = (buffer[j2], buffer[i2]);
                (buffer[i2 + 1], buffer[j2 + 1]) = (buffer[j2 + 1], buffer[i2 + 1]);
            } int m = n >> 1;
            while (m >= 1 && j >= m) {
                j -= m;
                m >>= 1;
            } j += m;
        }

        for (int len = 2; len <= n; len <<= 1) {
            float angle = -2f * Mathf.PI / len;
            float wLenReal = Mathf.Cos(angle);
            float wLenImag = Mathf.Sin(angle);
            for (int i = 0; i < n; i += len) {
                float wReal = 1f;
                float wImag = 0f;
                for (int k = 0; k < len / 2; k++) {
                    int even = (i + k) << 1;
                    int odd = (i + k + len / 2) << 1;
                    float ur = buffer[even];
                    float ui = buffer[even + 1];
                    float vr = buffer[odd] * wReal - buffer[odd + 1] * wImag;
                    float vi = buffer[odd] * wImag + buffer[odd + 1] * wReal;
                    buffer[even] = ur + vr;
                    buffer[even + 1] = ui + vi;
                    buffer[odd] = ur - vr;
                    buffer[odd + 1] = ui - vi;
                    float nextReal = wReal * wLenReal - wImag * wLenImag;
                    float nextImag = wReal * wLenImag + wImag * wLenReal;
                    wReal = nextReal;
                    wImag = nextImag;
                }
            }
        }
    }
}