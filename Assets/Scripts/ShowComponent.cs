using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowComponent: MonoBehaviour
{
    public GameObject[] ShowMyObject;

    public void ToggleShowComponents(bool visible) {
        foreach (GameObject components in ShowMyObject) {
            if(visible)
                components.SetActive(this.gameObject.GetComponent<Toggle>().isOn);
            else
                components.SetActive(!this.gameObject.GetComponent<Toggle>().isOn);
        }
    }
    public void ButtonShowComponents() {
        foreach (GameObject components in ShowMyObject) {
            components.SetActive(!components.activeSelf);
        }
    }
}
