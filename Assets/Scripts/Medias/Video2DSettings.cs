﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.Video;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class Video2DSettings: MonoBehaviour {

    string mediaType = "VIDEO2D";
    private FileInfo[] files;
    private string path, folderMidia, uploadFile = "";
    private DirectoryInfo folder;
    private List<string> names, completPaths, tempList;
    private RenderTexture rt2D;
    private List<GameObject> listMidias = new List<GameObject>();
    private bool updateRelMedias = true, updateInteract = true;
    private ControllerMode controller;

    public string durationMinutes, durationSeconds, startMinutes, startSeconds,
        endMinutes, endSeconds, delayMinutes, delaySeconds;
    public Canvas canvas, setings;
    private SceneManagement sceneManager;
    private GameObject start, end, interactiveIcon, interativeGroup;
    private RawImage rawImage;
    private VideoPlayer videoPlayer;
    private Button playButton, uploadButton;
    private Text duration;
    private Button buttonCanvas;
    private Stepper stepperDelayMinutes, stepperDelaySecond;
    private Toggle lookAt, loopToggle, isLinkToggle, muteToggle, startTargetToggle, endTargetToggle, isInteractToggle;
    private Dropdown chooseTargetDropdown, folderDropdown, startDropdown, startMediaDropdown, endDropdown, endMediaDropdown;
    private Slider scaleSlider, volumeSlider;
    private Vector3 originalMenuScale;
    private SerializerManager serializerManager;
    private Text buttonMessage;
    private ArduinoControl arduino;

    void Start() {
        //Settings Getters
        folderMidia = "Video2D/";
        controller = GameObject.Find("/Management/Controller Mode Management").GetComponent<ControllerMode>();
        sceneManager = GameObject.Find("/Management/Scene Management").GetComponent<SceneManagement>();
        serializerManager = GameObject.Find("/Management/Data Management").GetComponent<SerializerManager>();
        rawImage = canvas.transform.Find("Button").GetComponent<RawImage>();
        videoPlayer = canvas.GetComponent<VideoPlayer>();
        start = setings.transform.Find("Start").gameObject;
        end = setings.transform.Find("End").gameObject;
        interativeGroup = setings.transform.Find("Interact").gameObject;
        lookAt = setings.transform.Find("LookAt").GetComponent<Toggle>();
        interactiveIcon = canvas.transform.Find("InteractiveIcon").gameObject;
        startTargetToggle = setings.transform.Find("Interact/ToggleGroup/StartTargetToggle").GetComponent<Toggle>();
        endTargetToggle = setings.transform.Find("Interact/ToggleGroup/EndTargetToggle").GetComponent<Toggle>();
        chooseTargetDropdown = setings.transform.Find("Interact/ChooseTargetDropdown").GetComponent<Dropdown>();
        playButton = setings.transform.Find("PlayButton").GetComponent<Button>();
        uploadButton = setings.transform.Find("UploadFileButton").GetComponent<Button>();
        loopToggle = setings.transform.Find("LoopToggle").GetComponent<Toggle>();
        folderDropdown = setings.transform.Find("FolderDropdown").GetComponent<Dropdown>();
        duration = setings.transform.Find("Duration/DurationTimeText").GetComponent<Text>();
        startDropdown = setings.transform.Find("Start/StartDropdown").GetComponent<Dropdown>();
        startMediaDropdown = setings.transform.Find("Start/StartMediaDropdown").GetComponent<Dropdown>();
        stepperDelayMinutes = setings.transform.Find("Start/DelaySteppers/StepperMinutes").GetComponent<Stepper>();
        stepperDelaySecond = setings.transform.Find("Start/DelaySteppers/StepperSeconds").GetComponent<Stepper>();
        endDropdown = setings.transform.Find("End/EndDropdown").GetComponent<Dropdown>();
        endMediaDropdown = setings.transform.Find("End/EndMediaDropdown").GetComponent<Dropdown>();
        muteToggle = setings.transform.Find("MuteToggle").GetComponent<Toggle>();
        volumeSlider = setings.transform.Find("VolumeSlider").GetComponent<Slider>();
        scaleSlider = setings.transform.Find("ScaleSlider").GetComponent<Slider>();
        buttonCanvas = canvas.transform.Find("Button").GetComponent<Button>();
        isInteractToggle = setings.transform.Find("Interact/IsInteractToggle").GetComponent<Toggle>();
        buttonMessage = canvas.GetComponentInChildren<Text>();
        arduino = GameObject.Find("/Management/Instantiate Midia Management").GetComponent<ArduinoControl>();
        //Settings Setters
        //Folder File Manager
        names = new List<string>();
        completPaths = new List<string>();
        folderDropdown.options.Clear();
        files = GetFolderFiles();
        names.Add("Choose your 2D Video");
        foreach (FileInfo data in files) {
            names.Add(data.Name);
            completPaths.Add(data.FullName);
        }
        folderDropdown.AddOptions(names);
        //adiciona a primeira lista de cenas ao interact, removendo a cena atual
        tempList = new List<string>();
        for (int i = 1; i <= 5; i++)
            tempList.Add("Scene " + i);
        tempList.Remove(SceneManager.GetActiveScene().name);
        chooseTargetDropdown.AddOptions(tempList);
    }
    public void Update() {
        if (folderDropdown.value > 0 && videoPlayer.isPlaying)
            setDuration();
        updateShowComponents();
        updateTimes();
        updateHideTimes();
        if (lookAt.isOn) {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }
    }
    public void setVideo2D() {
        if (folderDropdown.value > 0) {
            videoPlayer.Stop();
            videoPlayer.url = completPaths[folderDropdown.value - 1];
            rt2D = new RenderTexture(1920, 1080, 24, RenderTextureFormat.ARGB32);
            rt2D.name = "RenderTextureDynamic";
            rt2D.Create();
            rawImage.texture = rt2D; ;
            videoPlayer.targetTexture = rt2D;
            videoPlayer.Prepare();
            playButton.gameObject.SetActive(true);
            playButton.GetComponentInChildren<Text>().text = "Stop";
            buttonMessage.text = "";
            loopToggle.gameObject.SetActive(true);
            scaleSlider.gameObject.SetActive(true);
            interativeGroup.SetActive(true);
            volumeSlider.gameObject.SetActive(true);
            muteToggle.gameObject.SetActive(true);
        } else {
            videoPlayer.Stop();
            loopToggle.gameObject.SetActive(false);
            volumeSlider.gameObject.SetActive(false);
            muteToggle.gameObject.SetActive(false);
            scaleSlider.gameObject.SetActive(false);
            interativeGroup.SetActive(false);
            playButton.GetComponentInChildren<Text>().text = "Play";
            playButton.gameObject.SetActive(false);
            rawImage.texture = new Texture2D(2048, 1024, TextureFormat.RGB24, false);
            buttonMessage.text = "BLANK";
        }
    }
    public bool isPlay() {
        return videoPlayer.isPlaying;
    }
    private FileInfo[] GetFolderFiles() {
        path = Application.persistentDataPath;
        folder = new DirectoryInfo(@path);
        FileInfo[] Files = folder.GetFiles().Where(f => f.Extension == ".mp4" || f.Extension == ".avi").ToArray(); ;
        return Files;
    }
    public void uploadFiles() {
        StartCoroutine(showTextFuntion());
    }
    private IEnumerator showTextFuntion() {
        uploadButton.transform.Find("Message").gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        //uploadFile = EditorUtility.OpenFilePanel("Open your video", "", "mp4,avi");
        if (!uploadFile.Equals("")) {
            FileInfo fileinfo = new FileInfo(uploadFile);
            //FileUtil.CopyFileOrDirectory(uploadFile, Application.dataPath + "/Resources/" + folderMidia + fileinfo.Name);
            //AssetDatabase.Refresh();
            yield return new WaitForSeconds(1);
            folderDropdown.options.Clear();
            completPaths.Add(fileinfo.FullName);
            names.Add(fileinfo.Name);
            folderDropdown.AddOptions(names);
        }
        yield return new WaitForSeconds(2);
        uploadButton.transform.Find("Message").gameObject.SetActive(false);
    }
    public void updateShowComponents() {
        //ativa ou desativa o icone interactive
        interactiveIcon.gameObject.SetActive(isInteractToggle.isOn);
        if (sceneManager.getMidias().Count < 2) {//verifica se há mais de uma mídia
            start.SetActive(false);
            end.SetActive(false);
        } else {//caso tenha mais que uma mídia
            start.SetActive(true);
            if (loopToggle.isOn) {//caso esteja em loop 
                end.SetActive(false);
            } else { //caso tenha mais que 1 midia e nao esteja em loop, ativa o start e end além de exibir as midias concorrentes
                end.SetActive(true);
                //caso não necessite atualizar o dropbox
                if (startMediaDropdown.options.Count != (sceneManager.getMidias().Count - 1) && updateRelMedias) {
                    listMidias = sceneManager.getMidias();
                    tempList = new List<string>();
                    foreach (GameObject namesMidia in listMidias)
                        if (!namesMidia.name.Equals(this.gameObject.name))
                            tempList.Add(namesMidia.name);
                    startMediaDropdown.ClearOptions();
                    startMediaDropdown.AddOptions(tempList);
                    startMediaDropdown.RefreshShownValue();
                    endMediaDropdown.ClearOptions();
                    endMediaDropdown.AddOptions(tempList);
                    endMediaDropdown.RefreshShownValue();
                    start.SetActive(true);
                    end.SetActive(true);
                    updateRelMedias = false;
                } else //caso necessite atualizar o dropbox
                    updateRelMedias = true;
            }
            //para gerar as midias e cenas no interact
            if(isInteractToggle.isOn && chooseTargetDropdown.options.Count != (sceneManager.getMidias().Count + 3) && updateInteract) {
                chooseTargetDropdown.ClearOptions();
                listMidias = sceneManager.getMidias();
                tempList = new List<string>();
                for (int i = 1; i <= 5; i++)
                    tempList.Add("Scene " + i);
                foreach (GameObject namesMedia in listMidias) {
                    if (!namesMedia.name.Equals(this.gameObject.name))
                        tempList.Add(namesMedia.name);
                }
                tempList.Remove(SceneManager.GetActiveScene().name);
                chooseTargetDropdown.AddOptions(tempList);
                chooseTargetDropdown.RefreshShownValue();
                interactiveWaitStart();
                updateInteract = false;
            } else
                updateInteract = true;
        }
        //relações de start e end
        if (startDropdown.value == 0 || startDropdown.value == 3)
            startMediaDropdown.gameObject.SetActive(false);
        else
            startMediaDropdown.gameObject.SetActive(true);
        if(endDropdown.value == 0) 
            endMediaDropdown.gameObject.SetActive(false);
        else 
            endMediaDropdown.gameObject.SetActive(true);
    }
    public void updateTimes() {
        if (sceneManager.getMidias().Count > 1) {
            delayMinutes = stepperDelayMinutes.transform.Find("TimeMinutesText").GetComponent<Text>().text;
            delaySeconds = stepperDelaySecond.transform.Find("TimeSecondText").GetComponent<Text>().text;
            if (loopToggle.isOn) { //Se não estver em loop, atribui tempo maximo a todos 
                durationMinutes = durationSeconds = endMinutes = endSeconds = "99";
                startMinutes = delayMinutes;
                startSeconds = delaySeconds;
            } else { //se nao tiver em loop pega o conteudo da duração
                setDuration();
                delayMinutes = stepperDelayMinutes.transform.Find("TimeMinutesText").GetComponent<Text>().text;
                delaySeconds = stepperDelaySecond.transform.Find("TimeSecondText").GetComponent<Text>().text;
                startMinutes = (0 + int.Parse(delayMinutes)).ToString();
                startSeconds = (0 + int.Parse(delaySeconds)).ToString();
                endMinutes = (int.Parse(durationMinutes) + int.Parse(delayMinutes)).ToString();
                endSeconds = (int.Parse(durationSeconds) + int.Parse(delaySeconds)).ToString();
                //para o inicio da midia
                if (!start.activeSelf) { //Atribui 0 ao inicio da mídia
                    startMinutes = startSeconds = "0";
                }
                if (startDropdown.value == 0 && start.activeSelf) { //Atribui 0 ao inicio da mídia
                    setings.transform.Find("Start/DelaySteppers").gameObject.SetActive(true);
                    startMinutes = (0 + int.Parse(delayMinutes)).ToString();
                    startSeconds = (0 + int.Parse(delaySeconds)).ToString();
                }
                //caso necessite ter relacionamento com outras midias para o inicio da midia
                if (startDropdown.value == 1 && start.activeSelf) {//caso seja onBegin
                    setings.transform.Find("Start/DelaySteppers").gameObject.SetActive(false);
                    foreach (GameObject namesMidia in sceneManager.getMidias()) {//listas as midias existentes
                        if (!namesMidia.name.Equals(this.gameObject.name)) {//exclui a midia local
                            //busca a midia equivalente a selecionada localmente
                            if (namesMidia.name.Equals(startMediaDropdown.options[startMediaDropdown.value].text)) {
                                Dropdown tempStartDropdown = namesMidia.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>();
                                Dropdown tempStartMediaDropdown = namesMidia.transform.Find("EditMenu/Start/StartMediaDropdown").GetComponent<Dropdown>();
                                //em caso de relacionamento cruzado, reseta um deles
                                if (tempStartDropdown.value == 1 &&
                                    tempStartMediaDropdown.options[tempStartMediaDropdown.value].text.Equals(this.gameObject.name)) {
                                    startDropdown.value = 0;
                                    startDropdown.Select();
                                } else { //importa os valores
                                    startMinutes = namesMidia.transform.Find("EditMenu/Hide/StartMinutes").GetComponent<Text>().text;
                                    startSeconds = namesMidia.transform.Find("EditMenu/Hide/StartSeconds").GetComponent<Text>().text;
                                }
                            }
                        }
                    }
                }
                if (startDropdown.value == 2 && start.activeSelf) {//caso seja onEnd
                    foreach (GameObject namesMidia in sceneManager.getMidias()) { //listas as midias existentes
                        setings.transform.Find("Start/DelaySteppers").gameObject.SetActive(false);
                        if (!namesMidia.name.Equals(this.gameObject.name)) {//exclui a midia local
                            //busca a midia equivalente a selecionada localmente
                            if (namesMidia.name.Equals(startMediaDropdown.options[startMediaDropdown.value].text)) { //busca a midia equivalente  a selecionada localmente
                                Dropdown tempStartDropdown = namesMidia.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>();
                                Dropdown tempStartMediaDropdown = namesMidia.transform.Find("EditMenu/Start/StartMediaDropdown").GetComponent<Dropdown>();
                                //em caso de relacionamento cruzado, reseta um deles
                                if (tempStartDropdown.value == 2 &&
                                    tempStartMediaDropdown.options[tempStartMediaDropdown.value].text.Equals(this.gameObject.name)) {
                                    startDropdown.value = 0;
                                    startDropdown.Select();
                                } else { //importa os valores
                                    startMinutes = namesMidia.transform.Find("EditMenu/Hide/EndMinutes").GetComponent<Text>().text;
                                    startSeconds = namesMidia.transform.Find("EditMenu/Hide/EndSeconds").GetComponent<Text>().text;
                                }
                            }
                        }
                    }
                }
                //para o termino da mídia
                //caso necessite ter relacionamento com outras midias para o termino da midia
                if (endDropdown.value == 1 && end.activeSelf) {//caso seja onBegin
                    foreach (GameObject namesMidia in sceneManager.getMidias()) {//listas as midias existentes
                        if (!namesMidia.name.Equals(this.gameObject.name)) {//exclui a midia local
                            if (namesMidia.name.Equals(endMediaDropdown.options[endMediaDropdown.value].text)) { //busca a midia equivalente  a selecionada localmente
                                Dropdown tempEndDropdown = namesMidia.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>();
                                Dropdown tempEndMediaDropdown = namesMidia.transform.Find("EditMenu/End/EndMediaDropdown").GetComponent<Dropdown>();
                                if (tempEndDropdown.value == 1 &&
                                    tempEndMediaDropdown.options[tempEndMediaDropdown.value].text.Equals(this.gameObject.name)) {//se a outra já estiver selecionad esta torna tempo local
                                    endDropdown.value = 0;
                                    endDropdown.Select();
                                } else { //importa os valores
                                    endMinutes = namesMidia.transform.Find("EditMenu/Hide/StartMinutes").GetComponent<Text>().text;
                                    endSeconds = namesMidia.transform.Find("EditMenu/Hide/StartSeconds").GetComponent<Text>().text;
                                }
                            }
                        }
                    }
                }
                if (endDropdown.value == 2 && end.activeSelf) {//caso seja onEnd
                    foreach (GameObject namesMidia in sceneManager.getMidias()) { //listas as midias existentes
                        if (!namesMidia.name.Equals(this.gameObject.name)) {//exclui a midia local
                            if (namesMidia.name.Equals(endMediaDropdown.options[endMediaDropdown.value].text)) { //busca a midia equivalente  a selecionada localmente
                                Dropdown tempEndDropdown = namesMidia.transform.Find("EditMenu/End/EndDropdown").GetComponent<Dropdown>();
                                Dropdown tempEndMediaDropdown = namesMidia.transform.Find("EditMenu/End/EndMediaDropdown").GetComponent<Dropdown>();
                                if (tempEndDropdown.value == 2 &&
                                    tempEndMediaDropdown.options[tempEndMediaDropdown.value].text.Equals(this.gameObject.name)) {//se a outra já estiver selecionad esta torna tempo local
                                    endDropdown.value = 0;
                                    endDropdown.Select();
                                } else { //importa os valores
                                    endMinutes = namesMidia.transform.Find("EditMenu/Hide/EndMinutes").GetComponent<Text>().text;
                                    endSeconds = namesMidia.transform.Find("EditMenu/Hide/EndSeconds").GetComponent<Text>().text;
                                }
                            }
                        }
                    }
                }
            }
        } else { // caso tenha somente uma mmídia
            if (loopToggle.isOn) {//caso esteja em loop
                startMinutes = startSeconds = delayMinutes = delaySeconds = "0";
                durationMinutes = durationSeconds = endMinutes = endSeconds = "99";
            } else {
                setDuration();
                startMinutes = startSeconds = delayMinutes = delaySeconds = "0";
                endMinutes = (int.Parse(durationMinutes) + int.Parse(delayMinutes)).ToString();
                endSeconds = (int.Parse(durationSeconds) + int.Parse(delaySeconds)).ToString();
            }
        }
    }
    public void updateHideTimes() {
        setings.transform.Find("Hide/DurationMinutes").GetComponent<Text>().text = durationMinutes;
        setings.transform.Find("Hide/DurationSeconds").GetComponent<Text>().text = durationSeconds;
        setings.transform.Find("Hide/DelayMinutes").GetComponent<Text>().text = delayMinutes;
        setings.transform.Find("Hide/DelaySeconds").GetComponent<Text>().text = delaySeconds;
        setings.transform.Find("Hide/StartMinutes").GetComponent<Text>().text = startMinutes;
        setings.transform.Find("Hide/StartSeconds").GetComponent<Text>().text = startSeconds;
        setings.transform.Find("Hide/EndMinutes").GetComponent<Text>().text = endMinutes;
        setings.transform.Find("Hide/EndSeconds").GetComponent<Text>().text = endSeconds;
        setings.transform.Find("Hide/MediaType").GetComponent<Text>().text = mediaType;
    }
    public void setDuration() {
        if (videoPlayer.frameCount > 0 && videoPlayer.frameRate > 0) {
            float time = videoPlayer.frameCount / videoPlayer.frameRate;
            System.TimeSpan VideoUrlLength = System.TimeSpan.FromSeconds(time);
            string minutes = (VideoUrlLength.Minutes).ToString("00");
            string seconds = (VideoUrlLength.Seconds).ToString("00");
            duration.text = minutes + ":" + seconds;
            durationMinutes = minutes;
            durationSeconds = seconds;
        } else {
            durationMinutes = "00";
            durationSeconds = "00";
        }
    }
    public void setPlayStop() {
        if (folderDropdown.value > 0) {
            if (videoPlayer.isPlaying) {
                videoPlayer.Stop();
                playButton.GetComponentInChildren<Text>().text = "Play";
            } else {
                videoPlayer.Play();
                playButton.GetComponentInChildren<Text>().text = "Stop";
            }
        }
    }
    public void setLoop() {
        if (loopToggle.gameObject.activeSelf && videoPlayer.isPlaying)
            videoPlayer.isLooping = loopToggle.isOn;
    }
    public void interactiveWaitStart() {
        if (isInteractToggle.isOn && startTargetToggle.isOn) {
            string target = chooseTargetDropdown.options[chooseTargetDropdown.value].text;
            if (!target.StartsWith("Scene") || !target.Equals(""))  //tratar-se de interação com outra mídia
                foreach (GameObject namesMedia in sceneManager.getMidias())
                    if (target.Equals(namesMedia.name)) {//altera para o valor Not Defined
                        namesMedia.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().value = 3;
                        namesMedia.transform.Find("EditMenu/Start/StartDropdown").GetComponent<Dropdown>().RefreshShownValue();
                    }
        }
    }
    public void scaleVolume() {
        if (volumeSlider.gameObject.activeSelf)
            videoPlayer.SetDirectAudioVolume(0, volumeSlider.value);
    }
    public void setMute() {
        if (loopToggle.gameObject.activeSelf && videoPlayer.isPlaying)
            for (int i = 0; i < videoPlayer.length; i++)
                videoPlayer.SetDirectAudioMute((ushort)i, muteToggle.isOn);
    }
    public void ScaleCanvas() {
        StartCoroutine(createScaleLate());
    }
    private IEnumerator createScaleLate() {
        yield return new WaitForSeconds(0.1f);
        if (scaleSlider.gameObject.activeSelf)
            buttonCanvas.transform.localScale = new Vector3(scaleSlider.value, scaleSlider.value, scaleSlider.value);
    }
    public void interact() {
        //feedback condicionado aos cliques
        if (controller.getMode() == controller.EDIT)
            setings.gameObject.SetActive(true);
        if (controller.getMode() == controller.DELETE)
            foreach (GameObject namesMedia in sceneManager.getMidias())
                if (namesMedia.name.Equals(gameObject.name)) {
                    sceneManager.deleteMidia(namesMedia);
                    Destroy(gameObject);
                }
        if (controller.getMode() == controller.VIEWER && isInteractToggle.isOn) {
            string target = chooseTargetDropdown.options[chooseTargetDropdown.value].text;
            if (target.StartsWith("Scene")) { //se tratar-se de interação com cena
                //destroi a porta de serial dos atuadores
                arduino.desligaatuadores();
                arduino.destroirPorta();
                if (SceneManager.GetActiveScene().name.StartsWith("Presentation")) //trata-se de apresentação
                    SceneManager.LoadScene("Presentation " + target.Split(' ')[1]); //carrega a cena da apresentação
                else {
                    serializerManager.serializeSave(); //salva a cena atual
                    SceneManager.LoadScene(target); //carrega a cena
                }
            } else
                foreach (GameObject namesMedia in sceneManager.getMidias())
                    if (target.Equals(namesMedia.name))
                        if (endTargetToggle.isOn) {
                            //desativa a midia
                            namesMedia.SetActive(false);
                            //se em 360, limpa a textura de tela
                            if (target.Equals("VIDEO360") || target.Equals("IMAGE360")) {
                                Camera.main.clearFlags = CameraClearFlags.Color;
                                Camera.main.backgroundColor = Color.black;
                            }
                        } else {//ativa a midia
                            namesMedia.SetActive(true);
                            //startar midias especiais ou que precisem de play
                            if (namesMedia.name.Equals("Image360"))
                                namesMedia.GetComponentInChildren<Image360Settings>().setImage360();
                            if (namesMedia.name.Equals("Video360")) 
                                namesMedia.GetComponentInChildren<Video360Settings>().setVideo360();
                            if (namesMedia.name.Equals("PIP"))
                                namesMedia.GetComponentInChildren<PIPSettings>().setPIP();
                            if (namesMedia.name.StartsWith("Video2D"))
                                namesMedia.GetComponentInChildren<Video2DSettings>().setVideo2D();
                            //oculta todos os menus de edição
                            namesMedia.transform.Find("EditMenu").gameObject.SetActive(false);
                        }
        }
    }
    public void setInteractiveIcon() {
        interactiveIcon.gameObject.SetActive(isInteractToggle.isOn);
    }
}
