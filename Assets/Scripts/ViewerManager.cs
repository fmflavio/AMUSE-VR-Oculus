using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewerManager : MonoBehaviour {
    public Text globalMinutes, globalSeconds;
    public GameObject viewer;
    private float time = 0, hours=0, minutes=0, seconds=0;
    private List<GameObject> medias;
    private bool hide = true;
    static float minutesLocal = -1, secondsLocal = -1;

    void Update() {
        hideAllMedias();
        updateGlobalTime();
        showMidiasInTime();
    }
    public void switchViewerMode() {
        viewer.SetActive(!viewer.activeSelf);
    }
    public void hideAllMedias() {
        if(viewer.activeSelf && hide) {
            medias = this.GetComponent<SceneManagement>().getMidias();
            foreach(GameObject localMedias in medias) {
                localMedias.SetActive(false);
            }
            Camera.main.clearFlags = CameraClearFlags.Color;
            Camera.main.backgroundColor = Color.black;
            hide = false;
        } else {
            if(!viewer.activeSelf) {
                hide = true;
            }
        }
    }
    public void showAllMedias() {
        if(!viewer.activeSelf) {
            medias = this.GetComponent<SceneManagement>().getMidias();
            foreach(GameObject localMedias in medias) {
                localMedias.SetActive(false);
                localMedias.SetActive(true);
            }
        }
    }
    private void updateGlobalTime() {
        if(viewer.activeSelf) {
            time += Time.deltaTime;
            seconds = (int)(time % 60);
            minutes = (int)((time / 60) % 60);
            //hours = (int)(time / 3600);
            globalMinutes.text = minutes.ToString();
            globalSeconds.text = seconds.ToString();
        } else {
            time = hours = minutes = seconds = 0;
            minutesLocal = secondsLocal = 0;
        }
    }
    private void showMidiasInTime() {
        if((secondsLocal+(minutesLocal*60)) < (seconds+(minutes*60))) {
            minutesLocal = minutes;
            secondsLocal = seconds;
            if(viewer.activeSelf) {
                medias = this.GetComponent<SceneManagement>().getMidias();
                foreach(GameObject localMedias in medias) { // lista todas a midias
                    //Debug.Log("Seconds: " + seconds);
                    //pega os tempos da midia do loop
                    float tempStartMinutes = float.Parse(localMedias.transform.Find("EditMenu/Hide/StartMinutes").GetComponent<Text>().text),
                        tempStartSecunds = float.Parse(localMedias.transform.Find("EditMenu/Hide/StartSeconds").GetComponent<Text>().text),
                        tempEndMinutes = float.Parse(localMedias.transform.Find("EditMenu/Hide/EndMinutes").GetComponent<Text>().text),
                        tempEndSeconds = float.Parse(localMedias.transform.Find("EditMenu/Hide/EndSeconds").GetComponent<Text>().text);
                    //Debug.Log("Sminutes: " + tempStartMinutes+" Sseconds: "+ tempStartSecunds+" Eminutes: "+ tempEndMinutes+" Eseconds: "+ tempEndSeconds);
                    //forçando a iniciar ou terminar com pelo menos 1 segundo
                    if(tempStartMinutes == 0 && tempStartSecunds == 0) tempStartSecunds = 1;
                    //forçando a terminar um segundo apos o inicio
                    //if(tempStartMinutes == tempEndMinutes && tempStartSecunds == tempEndSeconds) tempEndSeconds++;
                    //gerencia o iniciar da midia
                    if(tempStartMinutes == minutes && tempStartSecunds == seconds && !localMedias.activeSelf) {
                        //Debug.Log("Start: " + localMedias.name);
                        //Debug.Log("Sminutes: " + tempStartMinutes + " Sseconds: " + tempStartSecunds + " Eminutes: " + tempEndMinutes + " Eseconds: " + tempEndSeconds);
                        localMedias.SetActive(true);
                        if(localMedias.name.Equals("Image360")) {
                            Camera.main.clearFlags = CameraClearFlags.Skybox;
                            Camera.main.backgroundColor = Color.white;
                            localMedias.GetComponentInChildren<Image360Settings>().setImage360();
                        }
                        if(localMedias.name.Equals("Video360")) {
                            Camera.main.clearFlags = CameraClearFlags.Skybox;
                            Camera.main.backgroundColor = Color.white;
                            localMedias.GetComponentInChildren<Video360Settings>().setVideo360();
                        }
                        if(localMedias.name.Equals("PIP")) {
                            localMedias.GetComponentInChildren<PIPSettings>().setPIP();
                        }
                        if(localMedias.name.StartsWith("Video2D")) {
                            localMedias.GetComponentInChildren<Video2DSettings>().setVideo2D();
                        }
                        localMedias.transform.Find("EditMenu").gameObject.SetActive(false);
                    } else {
                        //gerencia o termino da midia
                        if(tempEndMinutes == minutes && tempEndSeconds == seconds && localMedias.activeSelf) {
                            //Debug.Log("End: " + localMedias.name);
                            //Debug.Log("Sminutes: " + tempStartMinutes + " Sseconds: " + tempStartSecunds + " Eminutes: " + tempEndMinutes + " Eseconds: " + tempEndSeconds);
                            if(localMedias.name.Equals("Image360") || localMedias.name.Equals("Video360")) {
                                Camera.main.clearFlags = CameraClearFlags.Color;
                                Camera.main.backgroundColor = Color.black;
                            }
                            localMedias.SetActive(false);
                        }
                    }

                }
            }
        }
    }
}
