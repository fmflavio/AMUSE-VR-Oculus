using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateMidia: MonoBehaviour
{
    private GameObject ob;
    public GameObject sceneManger, parentMidias, front, image2DPrefab, video2DPrefab, textMessagePrefab,
        audioPrefab, interactPrefab, sEWindPrefab, sESteamPrefab, sELightPrefab;
    private float x = 0, y = 1, z = 5;

    public void Start() {
        Camera.main.clearFlags = CameraClearFlags.Color;
        Camera.main.backgroundColor = Color.black;
    }

    public void intantiateVideo360() {
        Camera.main.clearFlags = CameraClearFlags.Color;
        Camera.main.backgroundColor = Color.white;
        ob = parentMidias.transform.Find("Video360").gameObject;
        List<GameObject> list = sceneManger.GetComponent<SceneManagement>().video360;
        sceneManger.GetComponent<SceneManagement>().video360.Add(ob);
        sceneManger.GetComponent<SceneManagement>().image360.Clear();
    }

    public void intantiateImage360() {
        Camera.main.clearFlags = CameraClearFlags.Color;
        Camera.main.backgroundColor = Color.white;
        ob = parentMidias.transform.Find("Image360").gameObject;
        List<GameObject> list = sceneManger.GetComponent<SceneManagement>().image360;
        sceneManger.GetComponent<SceneManagement>().image360.Add(ob);
        sceneManger.GetComponent<SceneManagement>().video360.Clear();
    }
    public void intantiatePIP() {
        ob = front.transform.Find("PIP").gameObject;
        List<GameObject> list = sceneManger.GetComponent<SceneManagement>().pip;
        sceneManger.GetComponent<SceneManagement>().pip.Add(ob);
    }

    public void intantiateVideo2D() {
        x++;
        ob = Instantiate(video2DPrefab, new Vector3(x, y, z), Quaternion.identity);    
        ob.transform.SetParent(parentMidias.transform, false);
        List<GameObject> list = sceneManger.GetComponent<SceneManagement>().video2D;
        ob.name = "Video2D - " + (list.Count + 1);
        ob.transform.Find("EditMenu/MenuHeader").GetComponent<Text>().text = "Edit Video 2D - " + (list.Count + 1);     
        sceneManger.GetComponent<SceneManagement>().video2D.Add(ob);
    }

    public void intantiateImage2D() {
        x++;
        ob = Instantiate(image2DPrefab, new Vector3(x, y, z), Quaternion.identity);
        ob.transform.SetParent(parentMidias.transform, false);
        List<GameObject> list = sceneManger.GetComponent<SceneManagement>().images2D;
        ob.name = "Image2D - "+(list.Count+1);
        ob.transform.Find("EditMenu/MenuHeader").GetComponent<Text>().text = "Edit Image 2D - "+(list.Count+1);
        sceneManger.GetComponent<SceneManagement>().images2D.Add(ob);
    }

    public void intantiateAudio3D() {
        x++;
        ob = Instantiate(audioPrefab, new Vector3(x, y, z), Quaternion.identity);
        ob.transform.SetParent(parentMidias.transform, false);
        List<GameObject> list = sceneManger.GetComponent<SceneManagement>().audio3D;
        ob.name = "Audio3D - " + (list.Count + 1);
        ob.transform.Find("EditMenu/MenuHeader").GetComponent<Text>().text = "Edit Audio 3D - " + (list.Count + 1);
        sceneManger.GetComponent<SceneManagement>().audio3D.Add(ob);
    }

    public void intantiateInteract() {
        x++;
        ob = Instantiate(interactPrefab, new Vector3(x, y, z), Quaternion.identity);
        ob.transform.SetParent(parentMidias.transform, false);
        List<GameObject> list = sceneManger.GetComponent<SceneManagement>().interact;
        ob.name = "Interact - " + (list.Count + 1);
        ob.transform.Find("EditMenu/MenuHeader").GetComponent<Text>().text = "Edit Interact - " + (list.Count + 1);
        sceneManger.GetComponent<SceneManagement>().interact.Add(ob);
    }
    public void intantiateSESteam() {
        x++;
        ob = Instantiate(sESteamPrefab, new Vector3(x, y, z), Quaternion.identity);
        ob.transform.SetParent(parentMidias.transform, false);
        List<GameObject> list = sceneManger.GetComponent<SceneManagement>().sESteam;
        ob.name = "SESteam - " + (list.Count + 1);
        ob.transform.Find("EditMenu/MenuHeader").GetComponent<Text>().text = "Edit Sensory Effects Steam - " + (list.Count + 1);
        sceneManger.GetComponent<SceneManagement>().sESteam.Add(ob);
    }
    public void intantiateSEWind() {
        x++;
        ob = Instantiate(sEWindPrefab, new Vector3(x, y, z), Quaternion.identity);
        ob.transform.SetParent(parentMidias.transform, false);
        List<GameObject> list = sceneManger.GetComponent<SceneManagement>().sEWind;
        ob.name = "SEWind - " + (list.Count + 1);
        ob.transform.Find("EditMenu/MenuHeader").GetComponent<Text>().text = "Edit Sensory Effects Wind - " + (list.Count + 1);
        sceneManger.GetComponent<SceneManagement>().sEWind.Add(ob);
    }
    public void intantiateSELight() {
        x++;
        ob = Instantiate(sELightPrefab, new Vector3(x, y, z), Quaternion.identity);
        ob.transform.SetParent(parentMidias.transform, false);
        List<GameObject> list = sceneManger.GetComponent<SceneManagement>().sELight;
        ob.name = "SELight - " + (list.Count + 1);
        ob.transform.Find("EditMenu/MenuHeader").GetComponent<Text>().text = "Edit Sensory Effects Light - " + (list.Count + 1);
        sceneManger.GetComponent<SceneManagement>().sELight.Add(ob);
    }
    public void intantiateTextMessage() {
        x++;
        ob = Instantiate(textMessagePrefab, new Vector3(x, y, z), Quaternion.identity);
        ob.transform.SetParent(parentMidias.transform, false);
        List<GameObject> list = sceneManger.GetComponent<SceneManagement>().textMessage;
        ob.name = "TextMessage - " + (list.Count + 1);
        ob.transform.Find("EditMenu/MenuHeader").GetComponent<Text>().text = "Edit Text Message - " + (list.Count + 1);
        sceneManger.GetComponent<SceneManagement>().textMessage.Add(ob);
    }
}
