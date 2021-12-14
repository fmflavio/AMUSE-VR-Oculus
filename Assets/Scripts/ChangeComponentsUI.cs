using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

//não é mais utilizado
public class ChangeComponentsUI : MonoBehaviour {
    public Text text;
    private float num;
    void Start() {
        this.gameObject.GetComponent<Stepper>().value = int.Parse(text.text);
    }

    public void UpdateTime() {
        num = this.gameObject.GetComponent<Stepper>().value;
        if(num<10)
            text.text = "0" + (int)num;
        else
            text.text = "" + (int)num;
    }
    public void UpdateVolume() {
        num = this.gameObject.GetComponent<Slider>().value;
        if (num < 10)
            text.text = "0" + (int)num;
        else
            text.text = "" + (int)num;
    }
}
