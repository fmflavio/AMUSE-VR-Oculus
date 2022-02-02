using UnityEngine;
using System.Collections;
using Uduino; // adding Uduino NameSpace 

public class BlinkLed : MonoBehaviour
{
    UduinoManager u; // The instance of Uduino is initialized here
    public int blinkPin = 10;
    [Range(0,5)]
    public float blinkSpeed = 1;
    void Start()
    {
        UduinoManager.Instance.pinMode(blinkPin, PinMode.Output);
        StartCoroutine(BlinkLoop());
    }

    public void onoff() {
        UduinoManager.Instance.digitalWrite(blinkPin, State.HIGH);
    }

    IEnumerator BlinkLoop()
    {
        while (true)
        {
            UduinoManager.Instance.digitalWrite(blinkPin, State.HIGH);
            yield return new WaitForSeconds(blinkSpeed);
            UduinoManager.Instance.digitalWrite(blinkPin, State.LOW);
            yield return new WaitForSeconds(blinkSpeed);
        }
    }
}