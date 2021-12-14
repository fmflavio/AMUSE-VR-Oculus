using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ChangeNumUI : MonoBehaviour {
    public Text text;
    private float num;

    [SerializeField]
    [Tooltip("The current step value of the control")]
    private int _value = 0;
    [SerializeField]
    [Tooltip("The minimum step value allowed by the control. When reached it will disable the '-' button")]
    private int _minimum = 0;
    [SerializeField]
    [Tooltip("The maximum step value allowed by the control. When reached it will disable the '+' button")]
    private int _maximum = 100;
    [SerializeField]
    [Tooltip("The step increment used to increment / decrement the step value")]
    private int _step = 1;

    //não é utilizado
    void Start() {
        //this.gameObject.GetComponent<Stepper>().value = int.Parse(text.text);
        if (_value < 10 && _value > 0)
            text.text = "00" + (int)_value;
        else
            text.text = "" + (int)_value;
    }
    public void up() {
        num = int.Parse(text.text) + _step;
        if (num > _maximum)
            num = _maximum;
        if (num < 10)
            text.text = "0" + (int)num;
        else
            text.text = "" + (int)num;
    }
    public void down() {
        num = int.Parse(text.text) - _step;
        if (num < _minimum)
            num = _minimum;
        if (num < 10)
            text.text = "0" + (int)num;
        else
            text.text = "" + (int)num;
    }

}
