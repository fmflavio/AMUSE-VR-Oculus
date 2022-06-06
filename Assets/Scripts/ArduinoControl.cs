using UnityEngine;
using System.IO.Ports;

public class ArduinoControl : MonoBehaviour {
    public int AQUECEDOR = 10, VENTILADOR = 11, LIGAR = 1, DESLIGAR = 0, frequencia = 9600;
    public string portaCom = "\\\\.\\COM6";
    private static SerialPort porta;

    void Start() {
        try {
            porta = new SerialPort(portaCom, frequencia);
            porta.Open();
            porta.Write("b");
            porta.Write("d");
        } catch (System.Exception e) {
            Debug.LogError("Deu ruim no inicio da abertura da porta: " + e.ToString());
        }
    }
    public void setMessage(int portaEscolhida, int acao) {
        if (portaEscolhida == AQUECEDOR && acao == LIGAR) {
            if (!porta.IsOpen) {
                try {
                    porta.Open();
                    porta.WriteTimeout = 100;
                    porta.Write("a");
                } catch (System.Exception e) {
                    Debug.LogError("Deu ruim: " + e.ToString());
                }
            } else
                porta.Write("a");
        }
        if (portaEscolhida == AQUECEDOR && acao == DESLIGAR) {
            if (!porta.IsOpen) {
                try {
                    porta.Open();
                    porta.WriteTimeout = 100;
                    porta.Write("b");
                } catch (System.Exception e) {
                    Debug.LogError("Deu ruim: " + e.ToString());
                }
            } else
                porta.Write("b");
        }
        if (portaEscolhida == VENTILADOR && acao == LIGAR) {
            if (!porta.IsOpen) {
                try {
                    porta.Open();
                    porta.WriteTimeout = 100;
                    porta.Write("c");
                } catch (System.Exception e) {
                    Debug.LogError("Deu ruim: " + e.ToString());
                }
            } else
                porta.Write("c");
        }
        if (portaEscolhida == VENTILADOR && acao == DESLIGAR) {
            if (!porta.IsOpen) {
                try {
                    porta.Open();
                    porta.WriteTimeout = 100;
                    porta.Write("d");
                } catch (System.Exception e) {
                    Debug.LogError("Deu ruim: " + e.ToString());
                }
            } else
                porta.Write("d");
        }
    }
    public void destroirPorta() {
        porta.Close();
    }
    public void desligaatuadores() {
        if (porta.IsOpen) {
            try {
                porta.Write("b");
                porta.Write("d");
                OVRInput.SetControllerVibration(0.0f, 0.0f, OVRInput.Controller.RTouch);
                OVRInput.SetControllerVibration(0.0f, 0.0f, OVRInput.Controller.LTouch);
            } catch (System.Exception e) {
                Debug.LogError("Deu ruim ao desligar: " + e.ToString());
            }
        }
    }
}
