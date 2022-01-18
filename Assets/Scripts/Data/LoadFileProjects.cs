using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class LoadFileProjects : MonoBehaviour { 
    //para carregar o dropbox dos projetos
    private string path;
    private DirectoryInfo folder;
    private FileInfo[] files;
    private List<string> names;
    private Dropdown folderDropdown;
    private int PLAYER = 1, AUTHOR = 2, MODE = 0;
    public LoadScene LS;
    public MultiSel MS;
    private GameObject menuMessage;

    void Start() {
        //Folder File Manager
        folderDropdown = transform.Find("FolderDropdown").GetComponent<Dropdown>();
        names = new List<string>();
        folderDropdown.options.Clear();
        files = GetFolderFiles();
        names.Add("Choose your XML project");
        foreach (FileInfo data in files) 
            names.Add(data.Name);
        folderDropdown.AddOptions(names);
    }
    public void setMode(int mode) { //se é modo player ou autor
        MODE = mode;
    }
    private FileInfo[] GetFolderFiles() { //gera e filtra os arquivos da pasta
        path = Application.persistentDataPath;
        folder = new DirectoryInfo(@path);
        FileInfo[] Files = folder.GetFiles().Where(f => f.Extension == ".xml").ToArray();
        return Files;
    }
    public void openProjectFile() {
        if (folderDropdown.value > 0) { // se houver aquivo selecionado
            MS.importProject(folderDropdown.options[folderDropdown.value].text);
            if (MODE == PLAYER) { //para modo player
                LS.LoadPresentation();
            }
            if (MODE == AUTHOR) { //para modo autor
                LS.loadProjectAuthor();
            }
        } else { //mensagem de erro
            menuMessage = this.transform.Find("/MainMenu/MainMenuCanvas/MessageMenuCanvas").gameObject;
            menuMessage.transform.Find("MessageHeader").GetComponent<Text>().text =
                menuMessage.transform.Find("MessageHeader").GetComponent<Text>().text + " " 
                + Application.persistentDataPath + folderDropdown.options[folderDropdown.value].text;
            menuMessage.SetActive(true);
        }
    }
}
