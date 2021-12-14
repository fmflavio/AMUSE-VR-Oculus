using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//[RequireComponent(typeof(GameObject))]
//[RequireComponent(typeof(SerializerManager))]
public class ViewerManager : MonoBehaviour {
    public Text globalMinutes, globalSeconds;
    public GameObject viewer;
    private float time = 0, hours = 0, minutes = 0, seconds = 0;
    private List<GameObject> medias;
    private bool hide = true;
    static float minutesLocal = -1, secondsLocal = -1;

    void Update() {
        hideAllMedias(); 
        updateGlobalTime();
        showMidiasInTime();
    }
    //controla a exibição ou não da tela de preview
    public void switchPreviewerModeOnOff() {
        viewer.SetActive(!viewer.activeSelf);
    }
    public void setPreview(bool active) {
        viewer.SetActive(active);
    }
    //esconde todas as midias para exibir somente as agendadas para o modo preview
    public void hideAllMedias() {
        if (viewer.activeSelf && hide) {
            medias = this.GetComponent<SceneManagement>().getMidias();
            foreach (GameObject localMedias in medias)
                localMedias.SetActive(false);
            Camera.main.clearFlags = CameraClearFlags.Color;
            Camera.main.backgroundColor = Color.black;
            hide = false;
        } else
            if (!viewer.activeSelf)
            hide = true;
    }
    //exibi todas as mídias que estejam instanciadas, mesmo que ocultas para o modo autoria
    public void showAllMedias() {
        if(!viewer.activeSelf) {
            medias = this.GetComponent<SceneManagement>().getMidias();
            foreach(GameObject localMedias in medias) {
                localMedias.SetActive(false);
                localMedias.SetActive(true);
            }
        }
    }
    //realiza calculos para exibir o tempo
    private void updateGlobalTime() {
        if(viewer.activeSelf) {
            time += Time.deltaTime;
            seconds = (int)(time % 60);
            minutes = (int)((time / 60) % 60);
            //hours = (int)(time / 3600);
            if (minutes >= 0 && minutes <= 9) globalMinutes.text = "0"+minutes.ToString();
            else globalMinutes.text = minutes.ToString();
            if (seconds >= 0 && seconds <= 9) globalSeconds.text = "0" + seconds.ToString();
            else globalSeconds.text = seconds.ToString();
        } else {
            time = hours = minutes = seconds = 0;
            minutesLocal = secondsLocal = 0;
        }
    }
    //nucleo do controlador de agendamento de exibição de midias
    private void showMidiasInTime() {
        if((secondsLocal+(minutesLocal*60)) < (seconds+(minutes*60))) {
            minutesLocal = minutes;
            secondsLocal = seconds;
            if(viewer.activeSelf) {
                medias = this.GetComponent<SceneManagement>().getMidias();
                foreach(GameObject localMedias in medias) { // lista todas a midias
                    //pega os tempos da midia do loop
                    float tempStartMinutes = float.Parse(localMedias.transform.Find("EditMenu/Hide/StartMinutes").GetComponent<Text>().text),
                        tempStartSecunds = float.Parse(localMedias.transform.Find("EditMenu/Hide/StartSeconds").GetComponent<Text>().text),
                        tempEndMinutes = float.Parse(localMedias.transform.Find("EditMenu/Hide/EndMinutes").GetComponent<Text>().text),
                        tempEndSeconds = float.Parse(localMedias.transform.Find("EditMenu/Hide/EndSeconds").GetComponent<Text>().text);
                    //forçando a iniciar ou terminar com pelo menos 1 segundo
                    if(tempStartMinutes == 0 && tempStartSecunds == 0) tempStartSecunds = 1;
                    //forçando a terminar um segundo apos o inicio
                    //verifica se o relacionamento start está definido pra Not Defined
                    int valueStart = localMedias.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value;
                    //gerencia o iniciar da midia
                    if (tempStartMinutes == minutes && tempStartSecunds == seconds && !localMedias.activeSelf && valueStart != 3) {
                        //ativa a midia ativa no foreach
                        localMedias.SetActive(true);
                        //startar midias especiais ou que precisem de play
                        if(localMedias.name.Equals("Image360")) 
                            localMedias.GetComponentInChildren<Image360Settings>().setImage360();
                        if(localMedias.name.Equals("Video360"))
                            localMedias.GetComponentInChildren<Video360Settings>().setVideo360();
                        if(localMedias.name.Equals("PIP"))
                            localMedias.GetComponentInChildren<PIPSettings>().setPIP();
                        if(localMedias.name.StartsWith("Video2D"))
                            localMedias.GetComponentInChildren<Video2DSettings>().setVideo2D();
                        //oculta todos os menus de edição
                        localMedias.transform.Find("EditMenu").gameObject.SetActive(false);
                    } else
                        //gerencia o termino da midia
                        if(tempEndMinutes == minutes && tempEndSeconds == seconds && localMedias.activeSelf) {
                            //oculta as midias especiais e limpa da apresentação
                            if(localMedias.name.Equals("Image360") || localMedias.name.Equals("Video360")) {
                                Camera.main.clearFlags = CameraClearFlags.Color;
                                Camera.main.backgroundColor = Color.black;
                            }
                            //termina a midia comum
                            localMedias.SetActive(false);
                        }
                }
            }
        }
    }
}
