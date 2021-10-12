using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene: MonoBehaviour
{
    private string path;
    public SerializerManager serializerManager;

    public void Awake() {
        path = SceneManager.GetActiveScene().name;
        if(!serializerManager.isEmptyList())
            serializerManager.serializeLoader();
    }
    public void nextScene() {
        serializerManager.serializeSave();
        if(path.Equals("Scene 1"))
            SceneManager.LoadScene("Scene 2");
        if(path.Equals("Scene 2"))
            SceneManager.LoadScene("Scene 3");
        if(path.Equals("Scene 3"))
            SceneManager.LoadScene("Scene 4");
        if(path.Equals("Scene 4"))
            SceneManager.LoadScene("Scene 5");
    }
    public void previousScene() {
        serializerManager.serializeSave();
        if(path.Equals("Scene 5"))
            SceneManager.LoadScene("Scene 4");
        if(path.Equals("Scene 4"))
            SceneManager.LoadScene("Scene 3");
        if(path.Equals("Scene 3"))
            SceneManager.LoadScene("Scene 2");
        if(path.Equals("Scene 2"))
            SceneManager.LoadScene("Scene 1");
    }
    public void newAuthor() {
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
