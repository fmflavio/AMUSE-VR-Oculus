using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagement: MonoBehaviour
{
    public List<GameObject> audio3D, images2D, image360, interact, pip, sELight,
        sESteam, sEWind, video2D, video360, textMessage = new List<GameObject>();
    public List<GameObject> getMidias() {
        List<GameObject> tempList = new List<GameObject>();
        if (video360.Count > 0) tempList.Add(video360[0]);
        if (image360.Count > 0) tempList.Add(image360[0]);
        if (pip.Count > 0) tempList.Add(pip[0]);
        if (video2D.Count > 0) foreach (GameObject midia in video2D) tempList.Add(midia);
        if (images2D.Count>0) foreach (GameObject midia in images2D) tempList.Add(midia);
        if (audio3D.Count > 0) foreach (GameObject midia in audio3D) tempList.Add(midia);
        if (textMessage.Count > 0) foreach (GameObject midia in textMessage) tempList.Add(midia);
        if (sELight.Count > 0) foreach (GameObject midia in sELight) tempList.Add(midia);
        if (sESteam.Count > 0) foreach (GameObject midia in sESteam) tempList.Add(midia);
        if (sEWind.Count > 0) foreach (GameObject midia in sEWind) tempList.Add(midia);
        if (interact.Count > 0) foreach (GameObject midia in interact) tempList.Add(midia);
        return tempList;
    }
    public void mediaAttach() {
        //observar aqui!!!!!!!!!!
        //List<GameObject> tempList = getMidias();
        //if(tempList.Count > 0) 
            //foreach(GameObject localMedias in tempList) {
            //if(!localMedias.name.Equals("Video360") && !localMedias.name.Equals("Image360") && !localMedias.name.Equals("PIP"))
                //localMedias.transform.Find("Canvas/Button").GetComponent<DragDrop>().enabled = !localMedias.transform.Find("Canvas/Button").GetComponent<DragDrop>().enabled;
        //} 
    }
    public void deleteSpecial(string midia) {
        if (midia.Equals("Video360")) {
            video360.Clear(); 
            Camera.main.clearFlags = CameraClearFlags.Color;
            Camera.main.backgroundColor = Color.black;
        }
        if (midia.Equals("Image360")) {
            image360.Clear();
            Camera.main.clearFlags = CameraClearFlags.Color;
            Camera.main.backgroundColor = Color.black;
        }
        if (midia.Equals("PIP")) {
            pip.Clear();
        }
    }
    public bool deleteMidia(GameObject obj) {
            audio3D.Remove(obj);
            images2D.Remove(obj);
            interact.Remove(obj);
            sELight.Remove(obj);
            sESteam.Remove(obj);
            sEWind.Remove(obj);
            video2D.Remove(obj);
            textMessage.Remove(obj);
            return true;
    }
}
