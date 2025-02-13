﻿using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene: MonoBehaviour {
    public SerializerManager serializerManager;
    private GameObject menuMessage;

    //carrega a proxima cena do modo autor
    public void nextScene() {
        serializerManager.serializeSave();
        if(SceneManager.GetActiveScene().name.Equals("Scene 1"))
            SceneManager.LoadScene("Scene 2");
        if(SceneManager.GetActiveScene().name.Equals("Scene 2"))
            SceneManager.LoadScene("Scene 3");
        if(SceneManager.GetActiveScene().name.Equals("Scene 3"))
            SceneManager.LoadScene("Scene 4");
        if(SceneManager.GetActiveScene().name.Equals("Scene 4"))
            SceneManager.LoadScene("Scene 5");
    }
    //carrega a cena anterior do modo autor
    public void previousScene() {
        serializerManager.serializeSave();
        if(SceneManager.GetActiveScene().name.Equals("Scene 5"))
            SceneManager.LoadScene("Scene 4");
        if(SceneManager.GetActiveScene().name.Equals("Scene 4"))
            SceneManager.LoadScene("Scene 3");
        if(SceneManager.GetActiveScene().name.Equals("Scene 3"))
            SceneManager.LoadScene("Scene 2");
        if(SceneManager.GetActiveScene().name.Equals("Scene 2"))
            SceneManager.LoadScene("Scene 1");
    }
    public void loadNewProjectAuthor() { //novo projeto
        serializerManager.DeleteFiles();
        SceneManager.LoadScene("Scene 1");
    }
    public void loadProjectAuthor() { //carrega projeto multisel salvo
        SceneManager.LoadScene("Scene 1");
    }
    public void LoadMain() { // retorna ao menu principal
        SceneManager.LoadScene("Main");
    }
    public void LoadPresentation() { //inicia o modo apresentação
        SceneManager.LoadScene("Presentation 1");
    }
    public void exit() { //fecha a aplicação
        Application.Quit();
    }
}
