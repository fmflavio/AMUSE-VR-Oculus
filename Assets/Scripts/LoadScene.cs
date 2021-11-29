using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene: MonoBehaviour {
    private string sceneName;
    public SerializerManager serializerManager;

    public void Awake() {
        sceneName = SceneManager.GetActiveScene().name;
        if(!serializerManager.isEmptyList())
            serializerManager.serializeLoader();
    }
    //carrega a proxima cena
    public void nextScene() {
        serializerManager.serializeSave();
        if(sceneName.Equals("Scene 1"))
            SceneManager.LoadScene("Scene 2");
        if(sceneName.Equals("Scene 2"))
            SceneManager.LoadScene("Scene 3");
        if(sceneName.Equals("Scene 3"))
            SceneManager.LoadScene("Scene 4");
        if(sceneName.Equals("Scene 4"))
            SceneManager.LoadScene("Scene 5");
    }
    //carrega a cena anterior
    public void previousScene() {
        serializerManager.serializeSave();
        if(sceneName.Equals("Scene 5"))
            SceneManager.LoadScene("Scene 4");
        if(sceneName.Equals("Scene 4"))
            SceneManager.LoadScene("Scene 3");
        if(sceneName.Equals("Scene 3"))
            SceneManager.LoadScene("Scene 2");
        if(sceneName.Equals("Scene 2"))
            SceneManager.LoadScene("Scene 1");
    }
    public void newAuthor() {
        serializerManager.DeleteFiles();
        SceneManager.LoadScene("Scene 1");
    }
    public void loadProjectAuthor() {
        serializerManager.DeleteFiles();
        SceneManager.LoadScene("Scene 1");
    }
    public void LoadMain() {
        SceneManager.LoadScene("Main");
    }
    private void LoadVisualizer1() {
        SceneManager.LoadScene("Visualizer 1");
    }
    public void exit() {
        Application.Quit();
    }
}
