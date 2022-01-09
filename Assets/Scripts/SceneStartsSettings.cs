using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneStartsSettings : MonoBehaviour {
    [SerializeField]
    [Tooltip("Required when preview in scene")]
    public ViewerManager viewerManager;
    [SerializeField]
    [Tooltip("Required to define assets for serialization")]
    public SerializerManager serializerManager;
    [SerializeField]
    [Tooltip("Required to define type of controls")]
    public ControllerMode controllerMode;
    [SerializeField]
    [Tooltip("Required to Multisel file managements")]
    public MultiSel multiSel;
    private string tempPatch, mainFolder;

    // verifica que a cena corrente e realiza os precarregamentos
    public void Awake() { 
        //verifica se existe a pasta principal, caso não haja, cria
        mainFolder = Application.persistentDataPath;
        if (!Directory.Exists(mainFolder))
            Directory.CreateDirectory(mainFolder);
        tempPatch = mainFolder + "/temp/";
        //verifica se existe a pasta de arquivos temporarios como de serialização, caso não haja, cria
        if (!Directory.Exists(tempPatch))
            Directory.CreateDirectory(tempPatch);
        //retorna o endereço para serialização
        //serializerManager.path = tempPatch + SceneManager.GetActiveScene().name + ".xml";
        //verifica que tipo de cena foi iniciada
        if (SceneManager.GetActiveScene().name.Equals("Main")) {
            //nao faz nada
        }
        if (SceneManager.GetActiveScene().name.StartsWith("Scene")) {
            viewerManager.setPreview(false);
            serializerManager.serializeLoader(SceneManager.GetActiveScene().name);
        }
        if (SceneManager.GetActiveScene().name.StartsWith("Presentation")) {//verifica se é uma apresntação
            viewerManager.setPreview(true);
            //converte o nome presentation para scene relacionado e carrega as midias
            serializerManager.serializeLoader("Scene " + SceneManager.GetActiveScene().name.Split(' ')[1]);
        }
        //inicia os dados do multisel
        //multiSel.setURLs(mainFolder, fileProject);
        //inicia os controles
        //controllerMode.startControl(SceneManager.GetActiveScene().name);
    }
}
