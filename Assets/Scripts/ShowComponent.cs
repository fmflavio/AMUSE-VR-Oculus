using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowComponent: MonoBehaviour {
    public GameObject[] ShowMyObject;
    public ControllerMode controllerMode;
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
    //exibir texto no botão prewiew
    public void switchButtonText(Text textoButton) {
        if (textoButton.text.Equals("Preview Mode"))
            textoButton.text = "Author Mode";
        else
            textoButton.text = "Preview Mode";
    }
    //exibir texto no header
    public void switchHeaderText(Text textoHeader) {
        if (textoHeader.text.Equals("CHOOSE YOUR OPTION"))
            textoHeader.text = "PREVIEW MODE";
        else
            textoHeader.text = "CHOOSE YOUR OPTION";
    }
    public void switchControlText(Text textoButton) {
        if (textoButton.text.Equals("Preview Mode"))
            controllerMode.getModeMessage(controllerMode.NOME);
        else
            controllerMode.getModeMessage(controllerMode.VIEWER);
    }
}
