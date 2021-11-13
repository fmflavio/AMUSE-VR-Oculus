using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Wacki;
using System.Linq;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControllerMode : MonoBehaviour {
    
    public GameObject menuEdit, menuDelete, MiniMenuEdit, MiniMenuDelete, ViwerMenu, LeftControl, RightControl;
    public Canvas rightMessage;
    public Text sceneTitleMenu1, sceneTitleMenu2;
    public Button previus1, previus2, next1, next2;
    public int NOME = 0, EDIT = 1, DELETE = 2, VIEWER = 3;
    public bool controllerHand = true;
    private List<GameObject> listMidias = new List<GameObject>();
    public SceneManagement sceneManager;

    public void Start() {
        string scene = SceneManager.GetActiveScene().name;
        if (!scene.Equals("Main")) {
            sceneTitleMenu1.text = sceneTitleMenu2.text = scene;
            if (scene.Last() == '1') {
                previus1.gameObject.SetActive(false);
                previus2.gameObject.SetActive(false);
            } else {
                if (scene.Last() == '5') {
                    next1.gameObject.SetActive(false);
                    next2.gameObject.SetActive(false);
                } else {
                    previus1.gameObject.SetActive(true);
                    previus2.gameObject.SetActive(true);
                    next1.gameObject.SetActive(true);
                    next2.gameObject.SetActive(true);
                }
            }
        }
    }
    public int getMode() {
        if (menuEdit.activeSelf || MiniMenuEdit.activeSelf) {
            getModeMessage(EDIT);
            return EDIT;
        } else {
            if (menuDelete.activeSelf || MiniMenuDelete.activeSelf) {
                getModeMessage(DELETE);
                return DELETE;
            } else {
                if (ViwerMenu.activeSelf) {
                    getModeMessage(VIEWER);
                    return VIEWER;
                } else {
                    getModeMessage(NOME);
                    return NOME;
                }
            }
        }
    }
    //mensagens para exibir no controle e listar nos menus
    public void getModeMessage(int mode) {
        if (mode == EDIT) {
            //exibe mensagem no controle
            rightMessage.GetComponentInChildren<Text>().text = "Edit Mode";
            //lista nos menus os botões
            listMidias = sceneManager.getMidias();
            foreach (GameObject namesMedia in listMidias) {
                if (namesMedia.name.Equals("Video360")) {
                    menuEdit.transform.Find("Video360Button").gameObject.SetActive(true);
                    MiniMenuEdit.transform.Find("Video360Button").gameObject.SetActive(true);
                }
                if (namesMedia.name.Equals("Image360")) {
                    menuEdit.transform.Find("Image360Button").gameObject.SetActive(true);
                    MiniMenuEdit.transform.Find("Image360Button").gameObject.SetActive(true);
                }
                if (namesMedia.name.Equals("PIP")) {
                    menuEdit.transform.Find("PIPButton").gameObject.SetActive(true);
                    MiniMenuEdit.transform.Find("PIPButton").gameObject.SetActive(true);
                }
            }
        }        
        if (mode == DELETE) {
            rightMessage.GetComponentInChildren<Text>().text = "Delete Mode";
            listMidias = sceneManager.getMidias();
            foreach (GameObject namesMedia in listMidias) {
                if (namesMedia.name.Equals("Video360")) {
                    menuDelete.transform.Find("Video360Button").gameObject.SetActive(true);
                    MiniMenuDelete.transform.Find("Video360Button").gameObject.SetActive(true);
                }
                if (namesMedia.name.Equals("Image360")) {
                    menuDelete.transform.Find("Image360Button").gameObject.SetActive(true);
                    MiniMenuDelete.transform.Find("Image360Button").gameObject.SetActive(true);
                }
                if (namesMedia.name.Equals("PIP")) {
                    menuDelete.transform.Find("PIPButton").gameObject.SetActive(true);
                    MiniMenuDelete.transform.Find("PIPButton").gameObject.SetActive(true);
                }
            }
        }
        if (mode == VIEWER) {
            rightMessage.GetComponentInChildren<Text>().text = "Preview Mode";
        }
        if (mode == NOME) {
            rightMessage.GetComponentInChildren<Text>().text = "Author Mode";
        }       
    }
    //referente a desligar e ligar os lazers
    public void controllerLeftOnOff() {
        if(LeftControl.activeSelf)
            LeftControl.SetActive(false);
        else
            LeftControl.SetActive(true);
    }
    public void controllerRightOnOff() {
        if (RightControl.activeSelf)
            RightControl.SetActive(false);
        else
            RightControl.SetActive(true);
    }
    //referente a determinar em que posição fica o menu principal
    public void controllerOnHand(bool value) {
        controllerHand = value;
    }
}
