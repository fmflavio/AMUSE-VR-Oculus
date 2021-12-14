using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//[RequireComponent(typeof(SceneManagement))]
//[RequireComponent(typeof(InstantiateMidia))]
public class SerializerManager : MonoBehaviour {
    public string path;
    private Presentation pre;
    private Media media;
    public SceneManagement sceneManager;
    public InstantiateMidia instantiate;
    private static int i = 0;

    private void Start() {
        path = Application.persistentDataPath + "/temp/" + SceneManager.GetActiveScene().name + ".xml";
    }
    void Update() {
        if(Input.GetKeyDown(KeyCode.S)) {
            //serializeSave();
        }
        if(Input.GetKeyDown(KeyCode.L)) {
            serializeLoader();
        }
        if(Input.GetKeyDown(KeyCode.C)) {
            //DeleteFiles();
        }
    }
    public bool fileExists() {
        if (!File.Exists(path))
            return false;
        else {
            pre = SerializeOp.Deserialize<Presentation>(path);
            Debug.LogError("midias: " + pre.Media.Count);
            if (pre.Media.Count > 0) return true; else return false;
        }
    }
    public void serializeLoader(string newPatch) {
        path = Application.persistentDataPath + "/temp/" + newPatch + ".xml";
        serializeLoader();
    }
    //carrega do arquivo
    #region Loader file
    public void serializeLoader() {
        if(path.Equals("") || path == null) //verifica se o endereço foi carregado
            pre = SerializeOp.Deserialize<Presentation>(Application.persistentDataPath + "/temp/" + SceneManager.GetActiveScene().name + ".xml");
        else 
            pre = SerializeOp.Deserialize<Presentation>(path);
        //if(File.Exists(path)) Debug.Log("XML FOUNDED"); else Debug.LogError("NOT FOUNDED XML FILE");
        for(int i=0; i< pre.Media.Count; i++) { //lista todas as mídias encontradas no arquivo
            if(pre.Media[i].type.Equals("AUDIO3D")) { //individualmente carrega cada uma seguindo as caracteristicas do tipo
                instantiate.intantiateAudio3D();
                foreach(GameObject ob in sceneManager.getMidias()) {
                    if(ob.name.Equals(pre.Media[i].name)) {
                        //para preencher o dropdown do arquivo
                        StartCoroutine(chooseFileLate(ob, pre.Media[i].src)); //algumas informações necessitam de delay para ser apresentadas na tela
                        //Relacionamento Start e End
                        if(pre.Media[i].rStart.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        } else if(pre.Media[i].rStart.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        }
                        if(pre.Media[i].rEnd.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        } else if(pre.Media[i].rEnd.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        }
                        //position
                        ob.transform.position = new Vector3(pre.Media[i].px, pre.Media[i].py, pre.Media[i].pz);
                        //para o LookAt
                        ob.transform.Find("EditMenu/LookAt").GetComponent<Toggle>().isOn = pre.Media[i].lookAt;
                        //para o delay
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDelay.ToString();
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDelay.ToString();
                        //para o loop
                        StartCoroutine(chooseLoopLate(ob, pre.Media[i].loop));
                        //para o mudo e volume
                        StartCoroutine(chooseMuteVolumeLate(ob, pre.Media[i].mute, pre.Media[i].volume));
                    }
                }
            }
            if(pre.Media[i].type.Equals("IMAGE2D")) {
                instantiate.intantiateImage2D();
                foreach(GameObject ob in sceneManager.getMidias()) {
                    if(ob.name.Equals(pre.Media[i].name)) {
                        //para preencher o dropdown do arquivo
                        StartCoroutine(chooseFileLate(ob, pre.Media[i].src));
                        //Relacionamento Start e End
                        if(pre.Media[i].rStart.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        } else if(pre.Media[i].rStart.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        }
                        if(pre.Media[i].rEnd.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        } else if(pre.Media[i].rEnd.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        }
                        //position
                        ob.transform.position = new Vector3(pre.Media[i].px, pre.Media[i].py, pre.Media[i].pz);
                        //para o LookAt
                        ob.transform.Find("EditMenu/LookAt").GetComponent<Toggle>().isOn = pre.Media[i].lookAt;
                        //para o duração
                        ob.transform.Find("EditMenu/DurationSteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDuration.ToString();
                        ob.transform.Find("EditMenu/DurationSteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDuration.ToString();
                        //para o delay
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDelay.ToString();
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDelay.ToString();
                        //para o loop
                        StartCoroutine(chooseLoopLate(ob, pre.Media[i].loop));
                        //para interact
                        ob.transform.Find("EditMenu/Interact/IsInteractToggle").GetComponent<Toggle>().isOn = pre.Media[i].interactive;
                        ob.transform.Find("Canvas/InteractiveIcon").gameObject.SetActive(pre.Media[i].interactiveIcon);
                        if(pre.Media[i].interactive) {
                            if(pre.Media[i].startTarget)
                                ob.transform.Find("EditMenu/Interact/ToggleGroup/StartTargetToggle").GetComponent<Toggle>().isOn = true;
                            else 
                                ob.transform.Find("EditMenu/Interact/ToggleGroup/EndTargetToggle").GetComponent<Toggle>().isOn = true;
                            StartCoroutine(chooseInteractLate(ob, pre.Media[i].interactiveTarget));
                        }
                        //scale
                        ob.transform.Find("Canvas/Button").gameObject.transform.localScale = new Vector3(pre.Media[i].scale, pre.Media[i].scale, pre.Media[i].scale);
                        ob.transform.Find("EditMenu/ScaleSlider").GetComponent<Slider>().value = pre.Media[i].scale;
                    }
                }
            }
            if(pre.Media[i].type.Equals("IMAGE360")) {
                instantiate.intantiateImage360();
                foreach(GameObject ob in sceneManager.getMidias()) {
                    if(ob.name.Equals(pre.Media[i].name)) {
                        ob.SetActive(true);//ativar o edit
                        //para preencher o dropdown do arquivo
                        StartCoroutine(chooseFileLate(ob, pre.Media[i].src));
                        //Relacionamento Start e End
                        if(pre.Media[i].rStart.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        } else if(pre.Media[i].rStart.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        }
                        if(pre.Media[i].rEnd.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        } else if(pre.Media[i].rEnd.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        }
                        //para o duração
                        ob.transform.Find("EditMenu/DurationSteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDuration.ToString();
                        ob.transform.Find("EditMenu/DurationSteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDuration.ToString();
                        //para o delay
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDelay.ToString();
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDelay.ToString();
                        //para o loop
                        StartCoroutine(chooseLoopLate(ob, pre.Media[i].loop));
                    }
                }
            }
            if(pre.Media[i].type.Equals("INTERACT")) {
                instantiate.intantiateInteract();
                foreach(GameObject ob in sceneManager.getMidias()) {
                    if(ob.name.Equals(pre.Media[i].name)) {
                        //ob.SetActive(true);//ativar o edit
                        //Relacionamento Start e End
                        if(pre.Media[i].rStart.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        } else if(pre.Media[i].rStart.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        }
                        if(pre.Media[i].rEnd.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        } else if(pre.Media[i].rEnd.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        }
                        //position
                        ob.transform.position = new Vector3(pre.Media[i].px, pre.Media[i].py, pre.Media[i].pz);
                        //para o LookAt
                        ob.transform.Find("EditMenu/LookAt").GetComponent<Toggle>().isOn = pre.Media[i].lookAt;
                        //para o duração
                        ob.transform.Find("EditMenu/DurationSteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDuration.ToString();
                        ob.transform.Find("EditMenu/DurationSteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDuration.ToString();
                        //para o delay
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDelay.ToString();
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDelay.ToString();
                        //para o loop
                        StartCoroutine(chooseLoopLate(ob, pre.Media[i].loop));
                        //para interact
                        ob.transform.Find("EditMenu/Interact/IsInteractToggle").GetComponent<Toggle>().isOn = pre.Media[i].interactive;
                        if(pre.Media[i].interactive) {
                            if(pre.Media[i].startTarget)
                                ob.transform.Find("EditMenu/Interact/ToggleGroup/StartTargetToggle").GetComponent<Toggle>().isOn = true;
                            else
                                ob.transform.Find("EditMenu/Interact/ToggleGroup/EndTargetToggle").GetComponent<Toggle>().isOn = true;
                            StartCoroutine(chooseInteractLate(ob, pre.Media[i].interactiveTarget));
                        }
                    }
                }
            }
            if(pre.Media[i].type.Equals("PIP")) {
                instantiate.intantiatePIP();
                foreach(GameObject ob in sceneManager.getMidias()) {
                    if(ob.name.Equals(pre.Media[i].name)) {
                        ob.SetActive(true);//ativar o edit
                        //para preencher o dropdown do arquivo
                        StartCoroutine(chooseFileLate(ob, pre.Media[i].src));
                        //para o start
                        if(pre.Media[i].rStart.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        } else if(pre.Media[i].rStart.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        }
                        //para o end
                        if(pre.Media[i].rEnd.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        } else if(pre.Media[i].rEnd.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        }
                        //para o delay
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDelay.ToString();
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDelay.ToString();
                        //para o loop
                        StartCoroutine(chooseLoopLate(ob, pre.Media[i].loop));
                        //para o mudo e volume
                        StartCoroutine(chooseMuteVolumeLate(ob, pre.Media[i].mute, pre.Media[i].volume));
                    }
                }
            }
            if(pre.Media[i].type.Equals("SELIGHT")) {
                instantiate.intantiateSELight();
                foreach(GameObject ob in sceneManager.getMidias()) {
                    if(ob.name.Equals(pre.Media[i].name)) {
                        //ob.SetActive(true);//ativar o edit
                        //Relacionamento Start e End
                        if(pre.Media[i].rStart.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        } else if(pre.Media[i].rStart.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        }
                        if(pre.Media[i].rEnd.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        } else if(pre.Media[i].rEnd.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        }
                        //position
                        ob.transform.position = new Vector3(pre.Media[i].px, pre.Media[i].py, pre.Media[i].pz);
                        //para o LookAt
                        ob.transform.Find("EditMenu/LookAt").GetComponent<Toggle>().isOn = pre.Media[i].lookAt;
                        //para o duração
                        ob.transform.Find("EditMenu/DurationSteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDuration.ToString();
                        ob.transform.Find("EditMenu/DurationSteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDuration.ToString();
                        //para o delay
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDelay.ToString();
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDelay.ToString();
                        //para o loop
                        StartCoroutine(chooseLoopLate(ob, pre.Media[i].loop));
                        //para intensidade
                        ob.transform.Find("EditMenu/IntensitySlider").GetComponent<Slider>().value = float.Parse(pre.Media[i].intensity);
                    }
                }
            }
            if(pre.Media[i].type.Equals("SESTEAM")) {
                instantiate.intantiateSESteam();
                foreach(GameObject ob in sceneManager.getMidias()) {
                    if(ob.name.Equals(pre.Media[i].name)) {
                        //ob.SetActive(true);//ativar o edit
                        //Relacionamento Start e End
                        if(pre.Media[i].rStart.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        } else if(pre.Media[i].rStart.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        }
                        if(pre.Media[i].rEnd.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        } else if(pre.Media[i].rEnd.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        }
                        //position
                        ob.transform.position = new Vector3(pre.Media[i].px, pre.Media[i].py, pre.Media[i].pz);
                        //para o LookAt
                        ob.transform.Find("EditMenu/LookAt").GetComponent<Toggle>().isOn = pre.Media[i].lookAt;
                        //para o duração
                        ob.transform.Find("EditMenu/DurationSteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDuration.ToString();
                        ob.transform.Find("EditMenu/DurationSteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDuration.ToString();
                        //para o delay
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDelay.ToString();
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDelay.ToString();
                        //para o loop
                        StartCoroutine(chooseLoopLate(ob, pre.Media[i].loop));
                        //para intensidade
                        ob.transform.Find("EditMenu/IntensitySlider").GetComponent<Slider>().value = float.Parse(pre.Media[i].intensity);
                    }
                }
            }
            if(pre.Media[i].type.Equals("SEWIND")) {
                instantiate.intantiateSEWind();
                foreach(GameObject ob in sceneManager.getMidias()) {
                    if(ob.name.Equals(pre.Media[i].name)) {
                        //ob.SetActive(true);//ativar o edit
                        //Relacionamento Start e End
                        if(pre.Media[i].rStart.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        } else if(pre.Media[i].rStart.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        }
                        if(pre.Media[i].rEnd.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        } else if(pre.Media[i].rEnd.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        }
                        //position
                        ob.transform.position = new Vector3(pre.Media[i].px, pre.Media[i].py, pre.Media[i].pz);
                        //para o LookAt
                        ob.transform.Find("EditMenu/LookAt").GetComponent<Toggle>().isOn = pre.Media[i].lookAt;
                        //para o duração
                        ob.transform.Find("EditMenu/DurationSteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDuration.ToString();
                        ob.transform.Find("EditMenu/DurationSteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDuration.ToString();
                        //para o delay
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDelay.ToString();
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDelay.ToString();
                        //para o loop
                        StartCoroutine(chooseLoopLate(ob, pre.Media[i].loop));
                        //para intensidade
                        ob.transform.Find("EditMenu/IntensitySlider").GetComponent<Slider>().value = float.Parse(pre.Media[i].intensity);
                    }
                }
            }
            if(pre.Media[i].type.Equals("TEXTMESSAGE")) {
                instantiate.intantiateTextMessage();
                foreach(GameObject ob in sceneManager.getMidias()) {
                    if(ob.name.Equals(pre.Media[i].name)) {
                        //ob.SetActive(true);//ativar o edit
                        //Relacionamento Start e End
                        if(pre.Media[i].rStart.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        } else if(pre.Media[i].rStart.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        }
                        if(pre.Media[i].rEnd.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        } else if(pre.Media[i].rEnd.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        }
                        //position
                        ob.transform.position = new Vector3(pre.Media[i].px, pre.Media[i].py, pre.Media[i].pz);
                        //para o LookAt
                        ob.transform.Find("EditMenu/LookAt").GetComponent<Toggle>().isOn = pre.Media[i].lookAt;
                        //para o duração
                        ob.transform.Find("EditMenu/DurationSteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDuration.ToString();
                        ob.transform.Find("EditMenu/DurationSteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDuration.ToString();
                        //para o delay
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDelay.ToString();
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDelay.ToString();
                        //para o loop
                        StartCoroutine(chooseLoopLate(ob, pre.Media[i].loop));
                        //para mensagem
                        ob.transform.Find("Canvas/Button/Text").GetComponent<Text>().text = pre.Media[i].textMessage;
                        //para interact
                        ob.transform.Find("EditMenu/Interact/IsInteractToggle").GetComponent<Toggle>().isOn = pre.Media[i].interactive;
                        if(pre.Media[i].interactive) {
                            if(pre.Media[i].startTarget)
                                ob.transform.Find("EditMenu/Interact/ToggleGroup/StartTargetToggle").GetComponent<Toggle>().isOn = true;
                            else
                                ob.transform.Find("EditMenu/Interact/ToggleGroup/EndTargetToggle").GetComponent<Toggle>().isOn = true;
                            StartCoroutine(chooseInteractLate(ob, pre.Media[i].interactiveTarget));
                        }
                        //scale
                        ob.transform.Find("Canvas/Button").gameObject.transform.localScale = new Vector3(pre.Media[i].scale, pre.Media[i].scale, pre.Media[i].scale);
                        ob.transform.Find("EditMenu/ScaleSlider").GetComponent<Slider>().value = pre.Media[i].scale;
                    }
                }
            }
            if(pre.Media[i].type.Equals("VIDEO2D")) {
                instantiate.intantiateVideo2D();
                foreach(GameObject ob in sceneManager.getMidias()) {
                    if(ob.name.Equals(pre.Media[i].name)) {
                        //para preencher o dropdown do arquivo
                        StartCoroutine(chooseFileLate(ob, pre.Media[i].src));
                        //Relacionamento Start e End
                        if(pre.Media[i].rStart.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        } else if(pre.Media[i].rStart.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        }
                        if(pre.Media[i].rEnd.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        } else if(pre.Media[i].rEnd.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        }
                        //position
                        ob.transform.position = new Vector3(pre.Media[i].px, pre.Media[i].py, pre.Media[i].pz);
                        //para o LookAt
                        ob.transform.Find("EditMenu/LookAt").GetComponent<Toggle>().isOn = pre.Media[i].lookAt;
                        //para o delay
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDelay.ToString();
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDelay.ToString();
                        //para o loop
                        StartCoroutine(chooseLoopLate(ob, pre.Media[i].loop));
                        //para o mudo e volume
                        StartCoroutine(chooseMuteVolumeLate(ob, pre.Media[i].mute, pre.Media[i].volume));
                        //para interact
                        ob.transform.Find("EditMenu/Interact/IsInteractToggle").GetComponent<Toggle>().isOn = pre.Media[i].interactive;
                        ob.transform.Find("Canvas/InteractiveIcon").gameObject.SetActive(pre.Media[i].interactiveIcon);
                        if(pre.Media[i].interactive) {
                            if(pre.Media[i].startTarget)
                                ob.transform.Find("EditMenu/Interact/ToggleGroup/StartTargetToggle").GetComponent<Toggle>().isOn = true;
                            else
                                ob.transform.Find("EditMenu/Interact/ToggleGroup/EndTargetToggle").GetComponent<Toggle>().isOn = true;
                            StartCoroutine(chooseInteractLate(ob, pre.Media[i].interactiveTarget));
                        }
                        //scale
                        ob.transform.Find("Canvas/Button").gameObject.transform.localScale = new Vector3(pre.Media[i].scale, pre.Media[i].scale, pre.Media[i].scale);
                        ob.transform.Find("EditMenu/ScaleSlider").GetComponent<Slider>().value = pre.Media[i].scale;
                    }
                }
            }
            if(pre.Media[i].type.Equals("VIDEO360")) {
                instantiate.intantiateVideo360();
                foreach(GameObject ob in sceneManager.getMidias()) {
                    if(ob.name.Equals(pre.Media[i].name)) {
                        ob.SetActive(true);//ativar o edit
                        StartCoroutine(chooseFileLate(ob, pre.Media[i].src));//para preencher o dropdown do arquivo
                        //para o start
                        if(pre.Media[i].rStart.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        } else if(pre.Media[i].rStart.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseStartMediaLate(ob, pre.Media[i].rMediaStart));
                        }
                        //para o end
                        if(pre.Media[i].rEnd.Equals("OnBegin")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 1;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        } else if(pre.Media[i].rEnd.Equals("OnEnd")) {
                            ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value = 2;
                            StartCoroutine(chooseEndMediaLate(ob, pre.Media[i].rMediaEnd));
                        }
                        //para o delay
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperMinutes/TimeMinutesText").GetComponent<Text>().text = pre.Media[i].mDelay.ToString();
                        ob.transform.Find("EditMenu/Start/DelaySteppers/StepperSeconds/TimeSecondText").GetComponent<Text>().text = pre.Media[i].sDelay.ToString();
                        //para o loop
                        StartCoroutine(chooseLoopLate(ob, pre.Media[i].loop));
                        //para o mudo e volume
                        StartCoroutine(chooseMuteVolumeLate(ob, pre.Media[i].mute, pre.Media[i].volume));
                    }
                }
            }
        }
    }
#endregion
    //salva o arquivo
#region save file
    public void serializeSave(){
        //cria o cabeçario da apresentação
        pre = new Presentation();
        pre.nScene = SceneManager.GetActiveScene().name.Last<char>().ToString();
        if(sceneManager.getMidias().Count > 0) {
            //varre todas as midias atribuindo os atributos
            foreach(GameObject ob in sceneManager.getMidias()) {
                media = new Media();
                string type = ob.transform.Find("EditMenu/Hide/MediaType").GetComponent<Text>().text;
                media.type = type;
                media.name = ob.name;
                if(!type.Equals("INTERACT") && !type.Equals("SELIGHT") && !type.Equals("SESTEAM") && !type.Equals("SEWIND") && !type.Equals("TEXTMESSAGE")) {
                    string temp = ob.transform.Find("EditMenu/FolderDropdown").GetComponent<Dropdown>().options[ob.transform.Find("EditMenu/FolderDropdown").GetComponent<Dropdown>().value].text;
                    if(temp.StartsWith("Choose ")) media.src = "null"; else media.src = temp;
                }
                media.px = ob.transform.position.x;
                media.py = ob.transform.position.y;
                media.pz = ob.transform.position.z;
                if (!type.Equals("VIDEO360") && !type.Equals("IMAGE360") && !type.Equals("PIP"))
                    media.lookAt = ob.transform.Find("EditMenu/LookAt").GetComponent<Toggle>().isOn;
                media.rStart = ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().options[ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value].text;
                if(ob.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value != 0)
                    media.rMediaStart = ob.transform.Find("EditMenu/Start/StartMediaDropdown").GetComponent<Dropdown>().options[ob.transform.Find("EditMenu/Start/StartMediaDropdown").GetComponent<Dropdown>().value].text;
                media.mStart = int.Parse(ob.transform.Find("EditMenu/Hide/StartMinutes").GetComponent<Text>().text);
                media.sStart = int.Parse(ob.transform.Find("EditMenu/Hide/StartSeconds").GetComponent<Text>().text);
                media.rEnd = ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().options[ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value].text;
                if(ob.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>().value != 0)
                    media.rMediaEnd = ob.transform.Find("EditMenu/End/EndMediaDropdown").GetComponent<Dropdown>().options[ob.transform.Find("EditMenu/End/EndMediaDropdown").GetComponent<Dropdown>().value].text;
                media.mEnd = int.Parse(ob.transform.Find("EditMenu/Hide/EndMinutes").GetComponent<Text>().text);
                media.sEnd = int.Parse(ob.transform.Find("EditMenu/Hide/EndSeconds").GetComponent<Text>().text);
                media.mDuration = int.Parse(ob.transform.Find("EditMenu/Hide/DurationMinutes").GetComponent<Text>().text);
                media.sDuration = int.Parse(ob.transform.Find("EditMenu/Hide/DurationSeconds").GetComponent<Text>().text);
                media.mDelay = int.Parse(ob.transform.Find("EditMenu/Hide/DelayMinutes").GetComponent<Text>().text);
                media.sDelay = int.Parse(ob.transform.Find("EditMenu/Hide/DelaySeconds").GetComponent<Text>().text);
                media.loop = ob.transform.Find("EditMenu/LoopToggle").GetComponent<Toggle>().isOn;
                if(type.Equals("VIDEO360") || type.Equals("PIP") || type.Equals("VIDEO2D") || type.Equals("AUDIO3D")) {
                    media.mute = ob.transform.Find("EditMenu/MuteToggle").GetComponent<Toggle>().isOn;
                    media.volume = ob.transform.Find("EditMenu/VolumeSlider").GetComponent<Slider>().value.ToString();
                }
                if(type.Equals("IMAGE2D") || type.Equals("VIDEO2D") || type.Equals("TEXTMESSAGE"))
                    media.scale = ob.transform.Find("EditMenu/ScaleSlider").GetComponent<Slider>().value;
                if (type.Equals("IMAGE2D") || type.Equals("INTERACT") || type.Equals("VIDEO2D") || type.Equals("TEXTMESSAGE")) {
                    media.interactive = ob.transform.Find("EditMenu/Interact/IsInteractToggle").GetComponent<Toggle>().isOn;
                    media.interactiveIcon = ob.transform.Find("EditMenu/Interact/IsInteractToggle").GetComponent<Toggle>().isOn;
                    if(media.interactive) {
                        media.startTarget = ob.transform.Find("EditMenu/Interact/ToggleGroup/StartTargetToggle").GetComponent<Toggle>().isOn;
                        media.interactiveTarget = ob.transform.Find("EditMenu/Interact/ChooseTargetDropdown").GetComponent<Dropdown>().options[ob.transform.Find("EditMenu/Interact/ChooseTargetDropdown").GetComponent<Dropdown>().value].text;
                    }
                }
                if(type.Equals("SELIGHT") || type.Equals("SESTEAM") || type.Equals("SEWIND"))
                    media.intensity = ob.transform.Find("EditMenu/IntensitySlider").GetComponent<Slider>().value.ToString();
                if(type.Equals("TEXTMESSAGE"))
                    media.textMessage = ob.transform.Find("Canvas/Button/Text").GetComponent<Text>().text;
                //salva os atibutos na classe serializada
                pre.Media.Add(media);
            }
        }
        SerializeOp.Serialize(pre, path);
        //if(File.Exists(path)) Debug.Log("XML SAVE"); else Debug.LogError("XML NOT SAVE");
    }
#endregion
    public void DeleteFiles() {
        string filePath;
        for(int i = 1; i <= 5; i++) {
            filePath = Application.persistentDataPath + "/temp/" + "Scene " + i + ".xml";
            if(File.Exists(filePath))
                File.Delete(filePath);
        }
    }
    //carregamento com atrasos propositais
    #region Late loads
    private IEnumerator chooseFileLate(GameObject ob, string src) {
        yield return new WaitForSeconds(1);
        int index = ob.transform.Find("EditMenu/FolderDropdown").GetComponent<Dropdown>().options.FindIndex((option) => { return option.text.Equals(src); });
        ob.transform.Find("EditMenu/FolderDropdown").GetComponent<Dropdown>().value = index;
        //abrir o menu é opcional
        //ob.transform.Find("EditMenu").gameObject.SetActive(true);
    }
    private IEnumerator chooseStartMediaLate(GameObject ob, string op) {
        yield return new WaitForSeconds(1);
        int index = ob.transform.Find("EditMenu/Start/StartMediaDropdown").GetComponent<Dropdown>().options.FindIndex((option) => { return option.text.Equals(op); });
        ob.transform.Find("EditMenu/Start/StartMediaDropdown").GetComponent<Dropdown>().value = index;
    }
    private IEnumerator chooseEndMediaLate(GameObject ob, string op) {
        yield return new WaitForSeconds(1);
        int index = ob.transform.Find("EditMenu/End/EndMediaDropdown").GetComponent<Dropdown>().options.FindIndex((option) => { return option.text.Equals(op); });
        ob.transform.Find("EditMenu/End/EndMediaDropdown").GetComponent<Dropdown>().value = index;
    }
    private IEnumerator chooseMuteVolumeLate(GameObject ob, bool mute, string volume) {
        yield return new WaitForSeconds(1.5f);
        if (mute) {
            ob.transform.Find("EditMenu/MuteToggle").GetComponent<Toggle>().isOn = true;
            if (pre.Media[i].type.Equals("AUDIO3D"))
                ob.GetComponent<AudioSettings>().setMute();
            if (pre.Media[i].type.Equals("VIDEO2D"))
                ob.GetComponent<Video2DSettings>().setMute();
            //if (pre.Media[i].type.Equals("VIDEO360"))
                //ob.GetComponent<Video360Settings>().setMute();
            if (pre.Media[i].type.Equals("PIP"))
                ob.GetComponent<PIPSettings>().setMute();
        } else {
            ob.transform.Find("EditMenu/MuteToggle").GetComponent<Toggle>().isOn = false;
            ob.transform.Find("EditMenu/VolumeSlider").GetComponent<Slider>().value = float.Parse(volume);
        }
    }
    private IEnumerator chooseLoopLate(GameObject ob, bool op) {
        yield return new WaitForSeconds(1.5f);
        if(op) {
            ob.transform.Find("EditMenu/LoopToggle").GetComponent<Toggle>().isOn = true;
            if (pre.Media[i].type.Equals("AUDIO3D"))
                ob.GetComponent<AudioSettings>().setLoop();
            if (pre.Media[i].type.Equals("VIDEO2D"))
                ob.GetComponent<Video2DSettings>().setLoop();
            if (pre.Media[i].type.Equals("VIDEO360"))
                ob.GetComponent<Video360Settings>().setLoop();
            if (pre.Media[i].type.Equals("PIP"))
                ob.GetComponent<PIPSettings>().setLoop();
        } else
            ob.transform.Find("EditMenu/LoopToggle").GetComponent<Toggle>().isOn = false;
    }
    private IEnumerator chooseInteractLate(GameObject ob, string op) {
        yield return new WaitForSeconds(1);
        int index = ob.transform.Find("EditMenu/Interact/ChooseTargetDropdown").GetComponent<Dropdown>().options.FindIndex((option) => { return option.text.Equals(op); });
        ob.transform.Find("EditMenu/Interact/ChooseTargetDropdown").GetComponent<Dropdown>().value = index;
    }
    #endregion
}
