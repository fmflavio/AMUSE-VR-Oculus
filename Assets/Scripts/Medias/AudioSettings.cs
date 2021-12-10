using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using System.IO;
using System.Linq;

public class AudioSettings: MonoBehaviour {
    /// <summary>
    /// para transladar a camera transform.Translate(Time.deltaTime, 0, 0, Camera.main.transform);
    /// </summary>
    string mediaType = "AUDIO3D";
    private FileInfo[] files;
    private string path, folderMidia, uploadFile="";
    private DirectoryInfo folder;
    private List<string> names, tempList, completPaths;
    private RenderTexture rt2D;
    private List<GameObject> listMidias = new List<GameObject>();
    private bool updateFlag = true;
    private ControllerMode controller;

    public string durationMinutes, durationSeconds, startMinutes, startSeconds,
        endMinutes, endSeconds, delayMinutes, delaySeconds;
    public Canvas canvas, setings;
    private SceneManagement sceneManager;
    private GameObject start, end;
    private RawImage rawImage;
    private AudioSource audioSource;
    private AudioClip clip;
    private Button playButton, uploadButton;
    private Text duration;
    private Stepper stepperDelayMinutes, stepperDelaySecond;
    private Toggle lookAt, loopToggle, muteToggle;
    private Dropdown folderDropdown, startDropdown, startMediaDropdown, endDropdown, endMediaDropdown;
    private Slider volumeSlider;

    void Start() {
        //Settings Getters
        folderMidia = "Audio/";
        controller = GameObject.Find("/Management/Controller Mode Management").GetComponent<ControllerMode>();
        sceneManager = GameObject.Find("/Management/Scene Management").GetComponent<SceneManagement>();
        rawImage = canvas.transform.Find("Button").GetComponent<RawImage>();
        audioSource = canvas.GetComponent<AudioSource>();
        start = setings.transform.Find("Start").gameObject;
        end = setings.transform.Find("End").gameObject;
        lookAt = setings.transform.Find("LookAt").GetComponent<Toggle>();
        playButton = setings.transform.Find("PlayButton").GetComponent<Button>();
        uploadButton = setings.transform.Find("UploadFileButton").GetComponent<Button>();
        folderDropdown = setings.transform.Find("FolderDropdown").GetComponent<Dropdown>();
        duration = setings.transform.Find("Duration/DurationTimeText").GetComponent<Text>();
        loopToggle = setings.transform.Find("LoopToggle").GetComponent<Toggle>();
        startDropdown = setings.transform.Find("Start/StartDropdown").GetComponent<Dropdown>();
        startMediaDropdown = setings.transform.Find("Start/StartMediaDropdown").GetComponent<Dropdown>();
        stepperDelayMinutes = setings.transform.Find("Start/DelaySteppers/StepperMinutes").GetComponent<Stepper>();
        stepperDelaySecond = setings.transform.Find("Start/DelaySteppers/StepperSeconds").GetComponent<Stepper>();
        endDropdown = setings.transform.Find("End/EndDropdown").GetComponent<Dropdown>();
        endMediaDropdown = setings.transform.Find("End/EndMediaDropdown").GetComponent<Dropdown>();
        muteToggle = setings.transform.Find("MuteToggle").GetComponent<Toggle>();
        volumeSlider = setings.transform.Find("VolumeSlider").GetComponent<Slider>();
        //Settings Setters
        //Folder File Manager
        names = new List<string>();
        completPaths = new List<string>();
        folderDropdown.options.Clear();
        files = GetFolderFiles();
        names.Add("Choose your 3D Audio");
        foreach (FileInfo data in files) {
            names.Add(data.Name);
            completPaths.Add(data.FullName);
        }
        folderDropdown.AddOptions(names);
    }
    public void Update() {
        if (folderDropdown.value > 0 && audioSource.isPlaying)
            setDuration();
        updateShowComponents();
        updateTimes();
        updateHideTimes();
        if (lookAt.isOn) {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }
    }
    public void setAudio() {
        audioSource.Stop();
        if (folderDropdown.value > 0) {
            //clip = Resources.Load(folderMidia + names[folderDropdown.value].Substring(0, names[folderDropdown.value].Length - 4)) as AudioClip;
            //audioSource.Play();
            //StartCoroutine(GetAudioClip());

            StartCoroutine(LoadAudio());

            //Text temp = setings.transform.Find("Temp").GetComponent<Text>();
            //temp.text = completPaths[folderDropdown.value - 1];

            loopToggle.gameObject.SetActive(true);
            volumeSlider.gameObject.SetActive(true);
            muteToggle.gameObject.SetActive(true);
            playButton.gameObject.SetActive(true);
            playButton.GetComponentInChildren<Text>().text = "Stop";
        } else {
            loopToggle.gameObject.SetActive(false);
            volumeSlider.gameObject.SetActive(false);
            muteToggle.gameObject.SetActive(false);
            playButton.gameObject.SetActive(false);
            playButton.GetComponentInChildren<Text>().text = "Play";
        }
    }
    //para carregar o audio
    private IEnumerator LoadAudio() {
        WWW request = new WWW(completPaths[folderDropdown.value - 1]);
        yield return request;
        audioSource.clip = request.GetAudioClip();
        audioSource.Play();
    }
    /*
    IEnumerator GetAudioClip() {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(completPaths[folderDropdown.value - 1], AudioType.MPEG)) {
            yield return www.SendWebRequest();
            AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
            audioSource.clip = myClip;
            audioSource.Play();
        }
    }
    private IEnumerator LoadAudio(string soundPath, string  audioName) {
        WWW request = GetAudioFromFile(soundPath, audioName);
        yield return request;
        clip = request.GetAudioClip();
        clip.name = audioName;
        audioSource.clip = clip;
        audioSource.Play();
    }
    private WWW GetAudioFromFile(string soundPath, string filename) {
        string audioToLoad = string.Format(soundPath + "{0}", filename);
        WWW request = new WWW(audioToLoad);
        return request;
    }
    */
    private FileInfo[] GetFolderFiles() {
        //path = Application.dataPath + "/Resources/" + folderMidia;
        path = Application.persistentDataPath;
        folder = new DirectoryInfo(@path);
        FileInfo[] Files = folder.GetFiles().Where(f => f.Extension == ".mp3" || f.Extension == ".wav").ToArray(); 
        return Files;
    }
    public void uploadFiles() {
        StartCoroutine(showTextFuntion());
    }
    public IEnumerator showTextFuntion() {
        uploadButton.transform.Find("Message").gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        //uploadFile = EditorUtility.OpenFilePanel("Open your audio", "", "mp3,wav");
        if (!uploadFile.Equals("")) {
            FileInfo fileinfo = new FileInfo(uploadFile);
            //FileUtil.CopyFileOrDirectory(uploadFile, Application.dataPath + "/Resources/" + folderMidia + fileinfo.Name);
            //AssetDatabase.Refresh();
            yield return new WaitForSeconds(1);
            folderDropdown.options.Clear();
            names.Add(fileinfo.Name);
            folderDropdown.AddOptions(names);
        }
        yield return new WaitForSeconds(1);
        uploadButton.transform.Find("Message").gameObject.SetActive(false);
    }
    public void updateShowComponents() {
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
                if (startMediaDropdown.options.Count != (sceneManager.getMidias().Count - 1) && updateFlag) {
                    listMidias = sceneManager.getMidias();
                    tempList = new List<string>();
                    foreach (GameObject namesMidia in listMidias) {
                        if (!namesMidia.name.Equals(this.gameObject.name))
                            tempList.Add(namesMidia.name);
                    }
                    startMediaDropdown.ClearOptions();
                    startMediaDropdown.AddOptions(tempList);
                    startMediaDropdown.RefreshShownValue();
                    endMediaDropdown.ClearOptions();
                    endMediaDropdown.AddOptions(tempList);
                    endMediaDropdown.RefreshShownValue();
                    start.SetActive(true);
                    end.SetActive(true);
                    updateFlag = false;
                } else { //caso necessite atualizar o dropbox
                    updateFlag = true;
                }
            }
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
        if (audioSource.isPlaying && audioSource.clip.length > 0) {
            float time = audioSource.clip.length;
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
            if (audioSource.isPlaying) {
                audioSource.Stop();
                playButton.GetComponentInChildren<Text>().text = "Play";
            } else {
                audioSource.Play();
                playButton.GetComponentInChildren<Text>().text = "Stop";
            }
        }
    }
    public void setLoop() {
        if(loopToggle.gameObject.activeSelf && audioSource.isPlaying)
            audioSource.loop = loopToggle.isOn;
    }
    public void scaleVolume() {
        if(volumeSlider.gameObject.activeSelf)
            audioSource.volume = volumeSlider.value;
    }
    public void setMute() {
        if(loopToggle.gameObject.activeSelf && audioSource.isPlaying) 
            audioSource.mute = muteToggle.isOn;       
    }
    public void interact() {
        //respostas aos cliques
        if (controller.getMode() == controller.EDIT)
            setings.gameObject.SetActive(true);
        else 
            if (controller.getMode() == controller.DELETE) 
                foreach (GameObject namesMedia in sceneManager.getMidias())
                    if (namesMedia.name.Equals(gameObject.name)) {
                        sceneManager.deleteMidia(namesMedia);
                        Destroy(gameObject);
                    }
    }
}
