using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(SerializerManager))]
public class LoadScene: MonoBehaviour {
    private string sceneName;
    public SerializerManager serializerManager;
    private GameObject menuMessage;

    public void Awake() {
        sceneName = SceneManager.GetActiveScene().name;
        serializerManager.serializeLoader();
    }
    //carrega a proxima cena do modo autor
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
    //carrega a cena anterior do modo autor
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
    public void loadNewProjectAuthor() { //novo projeto
        serializerManager.DeleteFiles();
        SceneManager.LoadScene("Scene 1");
    }
    public void loadProjectAuthor() { //carrega projeto multisel salvo
        if (File.Exists(Application.persistentDataPath + "/projeto.xml")) //verifica se existe projeto
            SceneManager.LoadScene("Scene 1");
        else { //mensagem de erro
            menuMessage = this.transform.Find("/MainMenu/MainMenuCanvas/MessageMenuCanvas").gameObject;
            menuMessage.transform.Find("MessageHeader").GetComponent<Text>().text =
                menuMessage.transform.Find("MessageHeader").GetComponent<Text>().text + " " + Application.persistentDataPath;
            menuMessage.SetActive(true);
        }
    }
    public void LoadMain() { // retorna ao menu principal
        SceneManager.LoadScene("Main");
    }
    public void LoadPresentation() { //inicia o modo apresentação
        if (File.Exists(Application.persistentDataPath + "/projeto.xml")) //verifica se existe projeto
            SceneManager.LoadScene("Presentation 1");
        else { //mensagem de erro
            menuMessage = this.transform.Find("/MainMenu/MainMenuCanvas/MessageMenuCanvas").gameObject;
            menuMessage.transform.Find("MessageHeader").GetComponent<Text>().text =
                menuMessage.transform.Find("MessageHeader").GetComponent<Text>().text + " " + Application.persistentDataPath;
            menuMessage.SetActive(true);
        }
    }
    //carrega a proxima cena da apresentação
    public void nextPresentation() {
        if (sceneName.Equals("Presentation 1"))
            SceneManager.LoadScene("Presentation 2");
        if (sceneName.Equals("Presentation 2"))
            SceneManager.LoadScene("Presentation 3");
        if (sceneName.Equals("Presentation 3"))
            SceneManager.LoadScene("Presentation 4");
        if (sceneName.Equals("Presentation 4"))
            SceneManager.LoadScene("Presentation 5");
    }
    //carrega a cena anterior da apresentação
    public void previousPresentation() {
        if (sceneName.Equals("Presentation 5"))
            SceneManager.LoadScene("Presentation 4");
        if (sceneName.Equals("Presentation 4"))
            SceneManager.LoadScene("Presentation 3");
        if (sceneName.Equals("Presentation 3"))
            SceneManager.LoadScene("Presentation 2");
        if (sceneName.Equals("Presentation 2"))
            SceneManager.LoadScene("Presentation 1");
    }
    public void exit() { //fecha a aplicação
        Application.Quit();
    }
}
