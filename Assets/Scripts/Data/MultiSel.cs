using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

public class MultiSel : MonoBehaviour{
    private string folder, file;
    private Presentation pre;
    private Media media;
    private GameObject ob;

    private void Start() {
        folder = Application.persistentDataPath;
        file = "projeto.xml";
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

    }
    void Update() {
        /*
        if (Input.GetKeyDown(KeyCode.S)) {
            exportProject();
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            importProject();
        }
        */
    }
    //le o arquivo xml multisel
    public void importProject() {
        //na importação é necessário criar arquivos de cena        
        XmlDocument doc = new XmlDocument();//cria o documento xml
        if (File.Exists(folder + "/" + file)) {
            doc.Load(folder + "/" + file);//carrega o arquivo
            Debug.Log("Projeto MultiSel carregado!");
        } else {
            Debug.LogError("Projeto MultiSel não encontrado");
            return;
        }
        //carrega o no body e cria-se a lista de nos filhos
        XmlNodeList nodesBody = doc.GetElementsByTagName("body");
        XmlNode nodeBody = nodesBody[0];//como só tem 1 body

        //pegando todos os relacionamentos externos a cena e armazenando
        List<string> relOutList = new List<string>();
        for (int ro = 0; ro < nodeBody?.ChildNodes.Count; ro++) {
            XmlNode nodeRelOut = nodeBody?.ChildNodes[ro];//encurtando
            if (nodeRelOut.Name.ToLower().Equals("relation")) { //obtendo as relações externas a cena
                string strRelOut = nodeRelOut?.Attributes["type"]?.Value;
                for (int nro = 0; nro < nodeRelOut?.ChildNodes.Count; nro++) {//agrupa os atributos a string
                    if (nodeRelOut.ChildNodes[nro].Name.ToLower().Equals("primary")) {
                        strRelOut = strRelOut + "*" + nodeRelOut.ChildNodes[nro]?.Attributes["component"]?.Value;
                        strRelOut = strRelOut + "*" + nodeRelOut.ChildNodes[nro]?.Attributes["interface"]?.Value;
                    }
                    if (nodeRelOut.ChildNodes[nro].Name.ToLower().Equals("secondary"))
                        strRelOut = strRelOut + "*" + nodeRelOut.ChildNodes[nro]?.Attributes["component"]?.Value;
                }
                relOutList.Add(strRelOut);// salva os values do relacionamento externo na lista
            }
        }
        for (int i = 0; i < nodeBody.ChildNodes.Count; i++) {
            pre = new Presentation();//instancia o arquivo se serialização
            //conteudo da cena
            if (nodeBody.ChildNodes[i].Name.ToLower().Equals("scene")) {
                //inclui nome da cena no arquivo
                pre.nScene = "Scene " + (i + 1);
                //se tiver delay, armazena os dados para incluir posteriormente
                string delayScene = "";
                if (nodeBody.ChildNodes[i]?.Attributes["delay"]?.Value != null)
                    delayScene = nodeBody.ChildNodes[i]?.Attributes["primaryComponent"]?.Value + "*" +
                        nodeBody.ChildNodes[i]?.Attributes["delay"]?.Value;
                //pegando todas as portas desta cena
                List<string> portsList = new List<string>();
                for (int p = 0; p < nodeBody.ChildNodes[i]?.ChildNodes.Count; p++) {
                    XmlNode nodePort = nodeBody.ChildNodes[i]?.ChildNodes[p];//encurtando
                    if (nodePort.Name.ToLower().Equals("port")) //obtendo as portas
                        portsList.Add(nodePort?.Attributes["id"]?.Value + "*" +
                            nodePort?.Attributes["component"]?.Value.ToLower());
                }
                //para registrar os IDs novos e antigos
                List<string> idsList = new List<string>();
                //listando os nos filho da cena
                int idVIDEO2D = 1, idAUDIO3D = 1, idIMAGE2D = 1, idTEXT = 1, idINTERACT = 1, idSEWIND = 1, idSELIGHT = 1, idSESTEAM = 1;
                for (int j = 0; j < nodeBody.ChildNodes[i]?.ChildNodes.Count; j++) {
                    XmlNode nodeScene = nodeBody.ChildNodes[i]?.ChildNodes[j];//encurtando, nos da cena
                    media = new Media();//instancia nova midia para serializar
                    if (nodeScene.Name.ToLower().Equals("media") || nodeScene.Name.ToLower().Equals("effect")) {//se for midia         
                        if (nodeScene.Attributes["type"].Value.ToLower().Equals("video")) {//se for video
                            media.type = "VIDEO2D";
                            media.name = "Video2D - " + idVIDEO2D++;
                            idsList.Add("Video2D - " + (idVIDEO2D - 1) + "*" + nodeScene.Attributes["id"].Value.ToLower());//armazena os ids
                            media.rStart = "Not Defined";
                            media.src = nodeScene.Attributes["src"].Value;
                            media.lookAt = true;
                            if (nodeScene.Attributes["id"].Value.ToLower().Equals(delayScene.Split('*')[0])) {
                                int delay = int.Parse(delayScene.Split('*')[1]);
                                int s = delay % 60; delay /= 60; int mins = delay % 60;
                                media.mDelay = mins;
                                media.sDelay = s;
                            }
                            for (int x = 0; x < nodeScene.ChildNodes.Count; x++) {//veridica se é 360
                                XmlNode nodeMedia = nodeScene.ChildNodes[x];
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("background")) {
                                    media.type = "VIDEO360";
                                    media.name = "Video360";
                                    idsList.RemoveAt(idsList.Count-1);//remove o ultimo id
                                    idsList.Add("Video360*" + nodeScene.Attributes["id"].Value.ToLower());
                                }
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("pip")) {
                                    media.type = "PIP";
                                    media.name = "PIP";
                                    idsList.RemoveAt(idsList.Count-1);
                                    idsList.Add("PIP*" + nodeScene.Attributes["id"].Value.ToLower());
                                }
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("x")) 
                                    media.px = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".",","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("y"))
                                    media.py = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("z"))
                                    media.pz = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("loop"))
                                    if (nodeMedia.Attributes["value"].Value.ToLower().Equals("true"))
                                        media.loop = true;
                                    else media.loop = false;
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("duration")) {
                                    int dur = int.Parse(nodeMedia.Attributes["value"]?.Value.ToLower());
                                    int s = dur % 60; dur /= 60; int mins = dur % 60;
                                    media.mDuration = mins;
                                    media.sDuration = s;
                                }
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("mute"))
                                    if(nodeMedia.Attributes["value"].Value.ToLower().Equals("true"))
                                        media.mute = true; else media.mute = false;
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("loop"))
                                    if (nodeMedia.Attributes["value"].Value.ToLower().Equals("true"))
                                        media.loop = true; else media.loop = false;
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("volume"))
                                    media.volume = nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ",");
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("scale"))
                                    media.scale = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                else media.scale = 0.1f;
                            }
                            //verificando se a mídia possui id igual a alguma ao component de alguma porta
                            foreach (string port in portsList) 
                                if (port.Contains(nodeScene.Attributes["id"].Value.ToLower())) 
                                    //verificando se ha relacionamento com esta porta
                                    foreach (string relationOut in relOutList) 
                                        if (relationOut.Contains(port.Split('*')[0])) {
                                            media.interactive = true;
                                            media.startTarget = true;
                                            media.interactiveTarget = relationOut.Split('*')[3].Replace("s", "S");
                                            media.interactiveIcon = true;
                                        }
                        }
                        if (nodeScene.Attributes["type"].Value.ToLower().Equals("audio")) {
                            media.type = "AUDIO3D";
                            media.name = "Audio3D - " + idAUDIO3D++;
                            idsList.Add("Audio3D - " + (idAUDIO3D - 1) + "*" + nodeScene.Attributes["id"].Value.ToLower());
                            media.rStart = "Not Defined";
                            media.src = nodeScene.Attributes["src"].Value;
                            media.lookAt = true;
                            if (nodeScene.Attributes["id"].Value.ToLower().Equals(delayScene.Split('*')[0])) {
                                int delay = int.Parse(delayScene.Split('*')[1]);
                                int s = delay % 60; delay /= 60; int mins = delay % 60;
                                media.mDelay = mins;
                                media.sDelay = s;
                            }
                            for (int x = 0; x < nodeScene.ChildNodes.Count; x++) {
                                XmlNode nodeMedia = nodeScene.ChildNodes[x];
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("x"))
                                    media.px = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("y"))
                                    media.py = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("z"))
                                    media.pz = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("loop"))
                                    if (nodeMedia.Attributes["value"].Value.ToLower().Equals("true"))
                                        media.loop = true;
                                    else media.loop = false;
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("duration")) {
                                    int dur = int.Parse(nodeMedia.Attributes["value"]?.Value.ToLower());
                                    int s = dur % 60; dur /= 60; int mins = dur % 60;
                                    media.mDuration = mins;
                                    media.sDuration = s;
                                }
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("mute"))
                                    if (nodeMedia.Attributes["value"].Value.ToLower().Equals("true"))
                                        media.mute = true; else media.mute = false;
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("loop"))
                                    if (nodeMedia.Attributes["value"].Value.ToLower().Equals("true"))
                                        media.loop = true; else media.loop = false;
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("volume"))
                                    media.volume = nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ",");
                            }
                            foreach (string port in portsList)
                                if (port.Contains(nodeScene.Attributes["id"].Value.ToLower()))
                                    foreach (string relationOut in relOutList)
                                        if (relationOut.Contains(port.Split('*')[0])) {
                                            media.interactive = true;
                                            media.startTarget = true;
                                            media.interactiveTarget = relationOut.Split('*')[3].Replace("s", "S");
                                            media.interactiveIcon = true;
                                        }
                        }
                        if (nodeScene.Attributes["type"].Value.ToLower().Equals("image")) {
                            media.type = "IMAGE2D";
                            media.name = "Image2D - " + idIMAGE2D++;
                            idsList.Add("Image2D - " + (idIMAGE2D - 1) + "*" + nodeScene.Attributes["id"].Value.ToLower());
                            media.rStart = "Not Defined";
                            media.src = nodeScene.Attributes["src"].Value;
                            media.lookAt = true;
                            if (nodeScene.Attributes["id"].Value.ToLower().Equals(delayScene.Split('*')[0])) {
                                int delay = int.Parse(delayScene.Split('*')[1]);
                                int s = delay % 60; delay /= 60; int mins = delay % 60;
                                media.mDelay = mins;
                                media.sDelay = s;
                            }
                            for (int x = 0; x < nodeScene.ChildNodes.Count; x++) {
                                XmlNode nodeMedia = nodeScene.ChildNodes[x];
                                if (nodeScene.ChildNodes[x].Attributes["name"].Value.ToLower().Equals("background")) {
                                    media.type = "IMAGE360";
                                    media.name = "Image360";
                                    idsList.RemoveAt(idsList.Count-1);
                                    idsList.Add("Image360*" + nodeScene.Attributes["id"].Value.ToLower());
                                }
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("x"))
                                    media.px = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("y"))
                                    media.py = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("z"))
                                    media.pz = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("loop"))
                                    if (nodeMedia.Attributes["value"].Value.ToLower().Equals("true"))
                                        media.loop = true;
                                    else media.loop = false;
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("duration")) {
                                    int dur = int.Parse(nodeMedia.Attributes["value"]?.Value.ToLower());
                                    if (dur <= 0) media.loop = true;
                                    int s = dur % 60; dur /= 60; int mins = dur % 60;
                                    media.mDuration = mins;
                                    media.sDuration = s;
                                }
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("scale"))
                                    media.scale = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                else media.scale = 0.1f;
                            }
                            foreach (string port in portsList)
                                if (port.Contains(nodeScene.Attributes["id"].Value.ToLower()))
                                    foreach (string relationOut in relOutList)
                                        if (relationOut.Contains(port.Split('*')[0])) {
                                            media.interactive = true;
                                            media.startTarget = true;
                                            media.interactiveTarget = relationOut.Split('*')[3].Replace("s", "S");
                                            media.interactiveIcon = true;
                                        }
                        }
                        if (nodeScene.Attributes["type"].Value.ToLower().Equals("image/x-icon")) {//interact
                            media.type = "INTERACT";
                            media.name = "Interact - " + idINTERACT++;
                            idsList.Add("Interact - " + (idINTERACT - 1) + "*" + nodeScene.Attributes["id"].Value.ToLower());
                            media.rStart = "Not Defined";
                            media.lookAt = true;
                            if (nodeScene.Attributes["id"].Value.ToLower().Equals(delayScene.Split('*')[0])) {
                                int delay = int.Parse(delayScene.Split('*')[1]);
                                int s = delay % 60; delay /= 60; int mins = delay % 60;
                                media.mDelay = mins;
                                media.sDelay = s;
                            }
                            for (int x = 0; x < nodeScene.ChildNodes.Count; x++) {
                                XmlNode nodeMedia = nodeScene.ChildNodes[x];
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("x"))
                                    media.px = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("y"))
                                    media.py = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("z"))
                                    media.pz = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("loop"))
                                    if (nodeMedia.Attributes["value"].Value.ToLower().Equals("true"))
                                        media.loop = true;
                                    else media.loop = false;
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("duration")) {
                                    int dur = int.Parse(nodeMedia.Attributes["value"]?.Value.ToLower());
                                    if (dur <= 0) media.loop = true;
                                    int s = dur % 60; dur /= 60; int mins = dur % 60;
                                    media.mDuration = mins;
                                    media.sDuration = s;
                                }
                            }
                            foreach (string port in portsList)
                                if (port.Contains(nodeScene.Attributes["id"].Value.ToLower()))
                                    foreach (string relationOut in relOutList)
                                        if (relationOut.Contains(port.Split('*')[0])) {
                                            media.interactive = true;
                                            media.startTarget = true;
                                            media.interactiveTarget = relationOut.Split('*')[3].Replace("s", "S");
                                            media.interactiveIcon = true;
                                        }
                        }
                        if (nodeScene.Attributes["type"].Value.ToLower().Equals("text")) {
                            media.type = "TEXTMESSAGE";
                            media.name = "TextMessage - " + idTEXT++;
                            idsList.Add("TextMessage - " + (idTEXT - 1) + "*" + nodeScene.Attributes["id"].Value.ToLower());
                            media.rStart = "Not Defined";
                            media.lookAt = true;
                            if (nodeScene.Attributes["id"].Value.ToLower().Equals(delayScene.Split('*')[0])) {
                                int delay = int.Parse(delayScene.Split('*')[1]);
                                int s = delay % 60; delay /= 60; int mins = delay % 60;
                                media.mDelay = mins;
                                media.sDelay = s;
                            }
                            for (int x = 0; x < nodeScene.ChildNodes.Count; x++) {
                                XmlNode nodeMedia = nodeScene.ChildNodes[x];
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("x"))
                                    media.px = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("y"))
                                    media.py = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("z"))
                                    media.pz = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("loop"))
                                    if (nodeMedia.Attributes["value"].Value.ToLower().Equals("true"))
                                        media.loop = true;
                                    else media.loop = false;
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("duration")) {
                                    int dur = int.Parse(nodeMedia.Attributes["value"]?.Value.ToLower());
                                    if (dur <= 0) media.loop = true;
                                    int s = dur % 60; dur /= 60; int mins = dur % 60;
                                    media.mDuration = mins;
                                    media.sDuration = s;
                                }
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("text"))
                                    media.textMessage = nodeMedia.Attributes["value"]?.Value;
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("scale"))
                                    media.scale = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                else media.scale = 0.1f;
                            }
                            foreach (string port in portsList)
                                if (port.Contains(nodeScene.Attributes["id"].Value.ToLower()))
                                    foreach (string relationOut in relOutList)
                                        if (relationOut.Contains(port.Split('*')[0])) {
                                            media.interactive = true;
                                            media.startTarget = true;
                                            media.interactiveTarget = relationOut.Split('*')[3].Replace("s", "S");
                                            media.interactiveIcon = true;
                                        }
                        }
                        if (nodeScene.Attributes["type"].Value.Equals("WindType")) {
                            media.type = "SEWIND";
                            media.name = "SEWind - " + idSEWIND++;
                            idsList.Add("SEWind - " + (idSEWIND - 1) + "*" + nodeScene.Attributes["id"].Value.ToLower());
                            media.rStart = "Not Defined";
                            media.lookAt = true;
                            if (nodeScene.Attributes["id"].Value.ToLower().Equals(delayScene.Split('*')[0])) {
                                int delay = int.Parse(delayScene.Split('*')[1]);
                                int s = delay % 60; delay /= 60; int mins = delay % 60;
                                media.mDelay = mins;
                                media.sDelay = s;
                            }
                            for (int x = 0; x < nodeScene.ChildNodes.Count; x++) {
                                XmlNode nodeMedia = nodeScene.ChildNodes[x];
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("x"))
                                    media.px = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("y"))
                                    media.py = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("z"))
                                    media.pz = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("loop"))
                                    if (nodeMedia.Attributes["value"].Value.ToLower().Equals("true"))
                                        media.loop = true;
                                    else media.loop = false;
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("duration")) {
                                    int dur = int.Parse(nodeMedia.Attributes["value"]?.Value.ToLower());
                                    if (dur <= 0) media.loop = true;
                                    int s = dur % 60; dur /= 60; int mins = dur % 60;
                                    media.mDuration = mins;
                                    media.sDuration = s;
                                }
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("intensity"))
                                    media.intensity = nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ",");
                            }
                            foreach (string port in portsList)
                                if (port.Contains(nodeScene.Attributes["id"].Value.ToLower()))
                                    foreach (string relationOut in relOutList)
                                        if (relationOut.Contains(port.Split('*')[0])) {
                                            media.interactive = true;
                                            media.startTarget = true;
                                            media.interactiveTarget = relationOut.Split('*')[3].Replace("s", "S");
                                            media.interactiveIcon = true;
                                        }
                        }
                        if (nodeScene.Attributes["type"].Value.Equals("LightType")) {
                            media.type = "SELIGHT";
                            media.name = "SELight - " + idSELIGHT++;
                            idsList.Add("SELight - " + (idSELIGHT - 1) + "*" + nodeScene.Attributes["id"].Value.ToLower());
                            media.rStart = "Not Defined";
                            media.lookAt = true;
                            if (nodeScene.Attributes["id"].Value.ToLower().Equals(delayScene.Split('*')[0])) {
                                int delay = int.Parse(delayScene.Split('*')[1]);
                                int s = delay % 60; delay /= 60; int mins = delay % 60;
                                media.mDelay = mins;
                                media.sDelay = s;
                            }
                            for (int x = 0; x < nodeScene.ChildNodes.Count; x++) {
                                XmlNode nodeMedia = nodeScene.ChildNodes[x];
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("x"))
                                    media.px = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("y"))
                                    media.py = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("z"))
                                    media.pz = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("loop"))
                                    if (nodeMedia.Attributes["value"].Value.ToLower().Equals("true"))
                                        media.loop = true;
                                    else media.loop = false;
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("duration")) {
                                    int dur = int.Parse(nodeMedia.Attributes["value"]?.Value.ToLower());
                                    if (dur <= 0) media.loop = true;
                                    int s = dur % 60; dur /= 60; int mins = dur % 60;
                                    media.mDuration = mins;
                                    media.sDuration = s;
                                }
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("intensity"))
                                    media.intensity = nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ",");
                            }
                            foreach (string port in portsList)
                                if (port.Contains(nodeScene.Attributes["id"].Value.ToLower()))
                                    foreach (string relationOut in relOutList)
                                        if (relationOut.Contains(port.Split('*')[0])) {
                                            media.interactive = true;
                                            media.startTarget = true;
                                            media.interactiveTarget = relationOut.Split('*')[3].Replace("s", "S");
                                            media.interactiveIcon = true;
                                        }
                        }
                        if (nodeScene.Attributes["type"].Value.Equals("SteamType")) {
                            media.type = "SESTEAM";
                            media.name = "SESteam - " + idSESTEAM++;
                            idsList.Add("SESteam - " + (idSESTEAM - 1) + "*" + nodeScene.Attributes["id"].Value.ToLower());
                            media.rStart = "Not Defined";
                            media.lookAt = true;
                            if (nodeScene.Attributes["id"].Value.ToLower().Equals(delayScene.Split('*')[0])) {
                                int delay = int.Parse(delayScene.Split('*')[1]);
                                int s = delay % 60; delay /= 60; int mins = delay % 60;
                                media.mDelay = mins;
                                media.sDelay = s;
                            }
                            for (int x = 0; x < nodeScene.ChildNodes.Count; x++) {
                                XmlNode nodeMedia = nodeScene.ChildNodes[x];
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("x"))
                                    media.px = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("y"))
                                    media.py = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("z"))
                                    media.pz = float.Parse(nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ","));
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("loop"))
                                    if (nodeMedia.Attributes["value"].Value.ToLower().Equals("true"))
                                        media.loop = true;
                                    else media.loop = false;
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("duration")) {
                                    int dur = int.Parse(nodeMedia.Attributes["value"]?.Value.ToLower());
                                    if (dur <= 0) media.loop = true;
                                    int s = dur % 60; dur /= 60; int mins = dur % 60;
                                    media.mDuration = mins;
                                    media.sDuration = s;
                                }
                                if (nodeMedia.Attributes["name"].Value.ToLower().Equals("intensity"))
                                    media.intensity = nodeMedia.Attributes["value"]?.Value.ToLower().Replace(".", ",");
                            }
                            foreach (string port in portsList)
                                if (port.Contains(nodeScene.Attributes["id"].Value.ToLower()))
                                    foreach (string relationOut in relOutList)
                                        if (relationOut.Contains(port.Split('*')[0])) {
                                            media.interactive = true;
                                            media.startTarget = true;
                                            media.interactiveTarget = relationOut.Split('*')[3].Replace("s","S");
                                            media.interactiveIcon = true;
                                        }
                        }
                        //renacionamento
                        //primeiro o primaryComponent
                        if (nodeBody.ChildNodes[i].Attributes["primaryComponent"].Value.ToLower().Equals(
                            nodeScene.Attributes["id"].Value.ToLower())) {
                            media.rStart = "OnBegin This Scene";
                            media.mStart = 0;
                            media.sStart = 0;
                        }
                        //os demais relacionamentos
                        //lista todas as relações para cruzar os dados com o nó da midia atual
                        for (int y = 0; y < nodeBody.ChildNodes[i].ChildNodes.Count; y++) {
                            string strPri = "", strSec = "";
                            XmlNode nodeRelation = nodeBody.ChildNodes[i].ChildNodes[y];//encurta
                            if (nodeRelation.Name.ToLower().Equals("relation")) {//se for no de relação
                                if (nodeRelation.Attributes["type"].Value.ToLower().Equals("starts")) {//se for do tipo starts
                                    for (int z = 0; z < nodeRelation.ChildNodes.Count; z++) {
                                        //percorre os nos filhos da relação e pega o primary e secundary
                                        if (nodeRelation.ChildNodes[z].Name.ToLower().Equals("primary")) 
                                            strPri = nodeRelation.ChildNodes[z].Attributes["component"].Value.ToLower();
                                        if (nodeRelation.ChildNodes[z].Name.ToLower().Equals("secondary"))
                                            strSec = nodeRelation.ChildNodes[z].Attributes["component"].Value.ToLower();
                                        //se o secundary referencia a midia atual como target e o primary inicia com scene
                                        if (nodeScene.Attributes["id"].Value.ToLower().Equals(strSec) &&
                                            strPri.StartsWith("scene")) {
                                            media.rStart = "OnBegin This Scene";
                                            media.mStart = 0;
                                            media.sStart = 0;
                                        }
                                        //se o secundary referencia a midia atual como target e o primary indica uma midia
                                        if (nodeScene.Attributes["id"].Value.ToLower().Equals(strSec) &&
                                            !strPri.StartsWith("scene")) {
                                            media.rStart = "OnBegin";
                                            media.rMediaStart = strPri;
                                        }
                                        //se a relação tiver delay
                                        if (nodeScene.Attributes["id"].Value.ToLower().Equals(strSec) &&
                                            nodeRelation.Attributes["delay"]?.Value != null &&
                                            !nodeRelation.Attributes["delay"].Value.ToLower().Equals("0")) {
                                            int dur = int.Parse(nodeRelation.Attributes["delay"]?.Value);
                                            int s = dur % 60; dur /= 60; int mins = dur % 60;
                                            media.mDelay = mins;
                                            media.sDelay = s;
                                        }
                                    }
                                }
                                if (nodeRelation.Attributes["type"].Value.ToLower().Equals("finishes")) {
                                    for (int z = 0; z < nodeRelation.ChildNodes.Count; z++) {
                                        if (nodeRelation.ChildNodes[z].Name.ToLower().Equals("primary"))
                                            strPri = nodeRelation.ChildNodes[z].Attributes["component"].Value.ToLower();
                                        if (nodeRelation.ChildNodes[z].Name.ToLower().Equals("secondary"))
                                            strSec = nodeRelation.ChildNodes[z].Attributes["component"].Value.ToLower();
                                        if (nodeScene.Attributes["id"].Value.ToLower().Equals(strSec) &&
                                            strPri.StartsWith("scene")) {
                                            media.rEnd = "End of This Media Time";
                                            media.mEnd = media.mDuration;
                                            media.sEnd = media.sDuration;
                                        }
                                        if (nodeScene.Attributes["id"].Value.ToLower().Equals(strSec) &&
                                            !strPri.StartsWith("scene")) {
                                            media.rEnd = "OnEnd";
                                            media.rMediaEnd = strPri;
                                            media.mEnd = media.mDuration;
                                            media.sEnd = media.sDuration;
                                        }
                                    }
                                }
        //***************rever o meet e metBy**********************************************
                                if (nodeRelation.Attributes["type"].Value.ToLower().Equals("meet")) {//inicio com final da outra midia
                                    for (int z = 0; z < nodeRelation.ChildNodes.Count; z++) {
                                        //percorre os nos filhos da relação e pega o primary e secundary
                                        if (nodeRelation.ChildNodes[z].Name.ToLower().Equals("primary"))
                                            strPri = nodeRelation.ChildNodes[z].Attributes["component"].Value.ToLower();
                                        if (nodeRelation.ChildNodes[z].Name.ToLower().Equals("secondary"))
                                            strSec = nodeRelation.ChildNodes[z].Attributes["component"].Value.ToLower();
                                        //se o secundary referencia a midia atual como target e o primary indica uma midia
                                        if (nodeScene.Attributes["id"].Value.ToLower().Equals(strSec) &&
                                            !strPri.StartsWith("scene")) {
                                            media.rStart = "OnEnd";
                                            media.rMediaStart = strPri;
                                        }
                                    }
                                }
                                if (nodeRelation.Attributes["type"].Value.Equals("metBy")) {//acaba com inicio da outra midia
                                    for (int z = 0; z < nodeRelation.ChildNodes.Count; z++) {
                                        if (nodeRelation.ChildNodes[z].Name.ToLower().Equals("primary"))
                                            strPri = nodeRelation.ChildNodes[z].Attributes["component"].Value.ToLower();
                                        if (nodeRelation.ChildNodes[z].Name.ToLower().Equals("secondary"))
                                            strSec = nodeRelation.ChildNodes[z].Attributes["component"].Value.ToLower();
                                        if (nodeScene.Attributes["id"].Value.ToLower().Equals(strSec) &&
                                            !strPri.StartsWith("scene")) {
                                            media.rEnd = "OnEnd";
                                            media.rMediaEnd = strPri;
                                        }
                                    }
                                }
                                if (nodeRelation.Attributes["type"].Value.Equals("onSelectionStarts")) {
                                    for (int z = 0; z < nodeRelation.ChildNodes.Count; z++) {
                                        if (nodeRelation.ChildNodes[z].Name.ToLower().Equals("primary"))
                                            strPri = nodeRelation.ChildNodes[z].Attributes["component"].Value.ToLower();
                                        if (nodeRelation.ChildNodes[z].Name.ToLower().Equals("secondary"))
                                            strSec = nodeRelation.ChildNodes[z].Attributes["component"].Value.ToLower();
                                        if (nodeScene.Attributes["id"].Value.ToLower().Equals(strPri) &&
                                            !strPri.StartsWith("scene")) {
                                            media.interactive = true;
                                            media.startTarget = true;
                                            media.interactiveTarget = strSec;
                                            media.interactiveIcon = true;
                                        }
                                    }
                                }
                                if (nodeRelation.Attributes["type"].Value.Equals("onSelectionEnds")) {
                                    for (int z = 0; z < nodeRelation.ChildNodes.Count; z++) {
                                        if (nodeRelation.ChildNodes[z].Name.ToLower().Equals("primary"))
                                            strPri = nodeRelation.ChildNodes[z].Attributes["component"].Value.ToLower();
                                        if (nodeRelation.ChildNodes[z].Name.ToLower().Equals("secondary"))
                                            strSec = nodeRelation.ChildNodes[z].Attributes["component"].Value.ToLower();
                                        if (nodeScene.Attributes["id"].Value.ToLower().Equals(strPri) &&
                                            !strPri.StartsWith("scene")) {
                                            media.interactive = true;
                                            media.startTarget = false;
                                            media.interactiveTarget = strSec;
                                            media.interactiveIcon = true;
                                        }
                                    }
                                }
                            }
                        }             
                        //adiciona a mídia
                        pre.Media.Add(media);
                    }
                }
                //pos processamento
                //troca os ids antigos pelos novos onde for necessário
                for (int p = 0; p < pre.Media.Count; p++) {
                    //trocando os ids dos targets de interação
                    if (pre.Media[p]?.interactiveTarget != null) 
                        foreach (string strId in idsList)
                            if (strId.Contains(pre.Media[p]?.interactiveTarget)) 
                                pre.Media[p].interactiveTarget = strId.Split('*')[0];
                    //trocando os ids de onBegin
                    if (pre.Media[p]?.rMediaStart != null)
                        foreach (string strId in idsList)
                            if (strId.Contains(pre.Media[p]?.rMediaStart))
                                pre.Media[p].rMediaStart = strId.Split('*')[0];
                    //trocando os ids de onEnd
                    if (pre.Media[p]?.rMediaEnd != null)
                        foreach (string strId in idsList)
                            if (strId.Contains(pre.Media[p]?.rMediaEnd))
                                pre.Media[p].rMediaEnd = strId.Split('*')[0];
                    //se a duração for 0 o loop fica true
                    if ((pre.Media[p]?.mDuration + pre.Media[p]?.sDuration) <= 0 &&
                        !pre.Media[p].type.Equals("VIDEO360") &&
                        !pre.Media[p].type.Equals("VIDEO2D") &&
                        !pre.Media[p].type.Equals("PIP") &&
                        !pre.Media[p].type.Equals("AUDIO3D")) 
                        pre.Media[p].loop = true;
                }
                //verifica a existencia da pasta e cria
                if (!Directory.Exists(folder + "/temp/")) Directory.CreateDirectory(folder + "/temp/");
                //fecha o arquivo 
                SerializeOp.Serialize(pre, folder + "/temp/Scene " + (i + 1) + ".xml");
                if (File.Exists(folder + "/temp/Scene " + (i + 1) + ".xml")) Debug.Log("XML SAVE - Scene " + (i + 1) + ".xml"); else Debug.LogError("XML NOT SAVE");
            }
        }
    }
    //cria o arquivo xml multisel
    public void exportProject() {
        int idPorta = 0, idRelacao=0;
        //cria o documento xml
        XmlDocument doc = new XmlDocument();
        //the xml declaration is recommended, but not mandatory
        XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
        XmlElement root = doc.DocumentElement;
        doc.InsertBefore(xmlDeclaration, root);
        //cabeçario multisel
        XmlElement elementMultiSel = doc.CreateElement(string.Empty, "multisel", string.Empty);
        elementMultiSel.SetAttribute("id", "Project-1");
        elementMultiSel.SetAttribute("title", "MultiSEL for AMUSE VR");
        elementMultiSel.SetAttribute("xmlns", "DefaultProfile");
        doc.AppendChild(elementMultiSel);
        //header
        XmlElement elementHead = doc.CreateElement(string.Empty, "head", string.Empty);
        elementMultiSel.AppendChild(elementHead);
        //metas do header
        XmlElement elementMeta1 = doc.CreateElement(string.Empty, "meta", string.Empty);
        elementMeta1.SetAttribute("name", "author");
        elementMeta1.SetAttribute("value", "Flávio Miranda de Farias");
        elementHead.AppendChild(elementMeta1);
        XmlElement elementMeta2 = doc.CreateElement(string.Empty, "meta", string.Empty);
        elementMeta2.SetAttribute("name", "year");
        elementMeta2.SetAttribute("value", "2021");
        elementHead.AppendChild(elementMeta2);
        //body
        XmlElement elementBody = doc.CreateElement(string.Empty, "body", string.Empty);
        elementBody.SetAttribute("primaryComponent", "scene 1");
        elementMultiSel.AppendChild(elementBody);
        //agrupa as relações externas a cena para imprimir depois
        List<XmlElement> relacoesExternas = new List<XmlElement>();
        //percorre as cenas existentes
        for (int s = 1; s <= 5; s++) {
            if (File.Exists(folder + "/temp/" + "Scene " + s + ".xml")) {
                Debug.Log("FILE: " + folder + "/temp/" + "Scene " + s + ".xml loaded");
                //cria a cena
                XmlElement elementScene = doc.CreateElement(string.Empty, "scene", string.Empty);
                elementScene.SetAttribute("id", "scene "+s);
                string scenePrimaryComponent = "";
                //^abre a cena^
                //carrega o xml serializado da cena no objeto pre
                pre = SerializeOp.Deserialize<Presentation>(folder + "/temp/" + "Scene " + s + ".xml");
                //agrupa as midias para imprimir depois
                List<XmlElement> mediaEffects = new List<XmlElement>();
                //agrupa as relações para imprimir depois
                List<XmlElement> relacoesInternas = new List<XmlElement>();
                //lista todas as midias desserializadas
                for (int i = 0; i < pre.Media.Count; i++) { 
                    //cria a mídia conforme o tipo
                    XmlElement elementMedia = doc.CreateElement(string.Empty, "media", string.Empty);
                    XmlElement elementEffect = doc.CreateElement(string.Empty, "effect", string.Empty);            
                    if (pre.Media[i].type.Equals("AUDIO3D")) {
                        //insere os atributos da mídia
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("src", pre.Media[i].src);
                        elementMedia.SetAttribute("type", "audio");
                        mediaEffects.Add(elementMedia);
                        //insere as propriedades da mídia                    
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty3);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty4);
                        XmlElement elementProperty5 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty5.SetAttribute("name", "volume");
                        elementProperty5.SetAttribute("value", pre.Media[i].volume.ToString().Replace(",","."));
                        elementMedia.AppendChild(elementProperty5);
                        XmlElement elementProperty6 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty6.SetAttribute("name", "mute");
                        elementProperty6.SetAttribute("value", pre.Media[i].mute.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty6);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", (((pre.Media[i].mDuration * 60) + (pre.Media[i].sDuration))) + "");
                        elementMedia.PrependChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("IMAGE2D")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("src", pre.Media[i].src);
                        elementMedia.SetAttribute("type", "image");
                        mediaEffects.Add(elementMedia);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty3);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty4);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", (((pre.Media[i].mDuration * 60) + (pre.Media[i].sDuration))) + "");
                        elementMedia.PrependChild(elementProperty11);
                        XmlElement elementProperty12 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty12.SetAttribute("name", "scale");
                        elementProperty12.SetAttribute("value", pre.Media[i].scale.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty12);
                    }
                    if (pre.Media[i].type.Equals("IMAGE360")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("src", pre.Media[i].src);
                        elementMedia.SetAttribute("type", "image");
                        mediaEffects.Add(elementMedia);
                        XmlElement elementProperty = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty.SetAttribute("name", "background");
                        elementProperty.SetAttribute("value", "true");
                        elementMedia.AppendChild(elementProperty);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty4);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", (((pre.Media[i].mDuration * 60) + (pre.Media[i].sDuration))) + "");
                        elementMedia.PrependChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("INTERACT")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("type", "image/x-icon");
                        mediaEffects.Add(elementMedia);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty3);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty4);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", (((pre.Media[i].mDuration * 60) + (pre.Media[i].sDuration))) + "");
                        elementMedia.PrependChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("PIP")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("src", pre.Media[i].src);
                        elementMedia.SetAttribute("type", "video");
                        mediaEffects.Add(elementMedia);
                        XmlElement elementProperty = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty.SetAttribute("name", "pip");
                        elementProperty.SetAttribute("value", "true");
                        elementMedia.AppendChild(elementProperty);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty4);
                        XmlElement elementProperty5 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty5.SetAttribute("name", "volume");
                        elementProperty5.SetAttribute("value", pre.Media[i].volume.ToString().Replace(",","."));
                        elementMedia.AppendChild(elementProperty5);
                        XmlElement elementProperty6 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty6.SetAttribute("name", "mute");
                        elementProperty6.SetAttribute("value", pre.Media[i].mute.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty6);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", (((pre.Media[i].mDuration * 60) + (pre.Media[i].sDuration))) + "");
                        elementMedia.PrependChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("SELIGHT")) {
                        elementEffect.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementEffect.SetAttribute("type", "LightType");
                        mediaEffects.Add(elementEffect);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString().Replace(",", "."));
                        elementEffect.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString().Replace(",", "."));
                        elementEffect.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString().Replace(",", "."));
                        elementEffect.AppendChild(elementProperty3);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementEffect.AppendChild(elementProperty4);
                        XmlElement elementProperty9 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty9.SetAttribute("name", "intensity");
                        elementProperty9.SetAttribute("value", pre.Media[i].intensity.ToString().Replace(",","."));
                        elementEffect.AppendChild(elementProperty9);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", (((pre.Media[i].mDuration * 60) + (pre.Media[i].sDuration))) + "");
                        elementEffect.PrependChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("SESTEAM")) {
                        elementEffect.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementEffect.SetAttribute("type", "SteamType");
                        mediaEffects.Add(elementEffect);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString().Replace(",", "."));
                        elementEffect.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString().Replace(",", "."));
                        elementEffect.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString().Replace(",", "."));
                        elementEffect.AppendChild(elementProperty3);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementEffect.AppendChild(elementProperty4);
                        XmlElement elementProperty9 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty9.SetAttribute("name", "intensity");
                        elementProperty9.SetAttribute("value", pre.Media[i].intensity.ToString().Replace(",", "."));
                        elementEffect.AppendChild(elementProperty9);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", (((pre.Media[i].mDuration * 60) + (pre.Media[i].sDuration))) + "");
                        elementEffect.PrependChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("SEWIND")) {
                        elementEffect.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementEffect.SetAttribute("type", "WindType");
                        mediaEffects.Add(elementEffect);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString().Replace(",", "."));
                        elementEffect.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString().Replace(",", "."));
                        elementEffect.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString().Replace(",", "."));
                        elementEffect.AppendChild(elementProperty3);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementEffect.AppendChild(elementProperty4);
                        XmlElement elementProperty9 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty9.SetAttribute("name", "intensity");
                        elementProperty9.SetAttribute("value", pre.Media[i].intensity.ToString().Replace(",", "."));
                        elementEffect.AppendChild(elementProperty9);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", (((pre.Media[i].mDuration * 60) + (pre.Media[i].sDuration))) + "");
                        elementEffect.PrependChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("TEXTMESSAGE")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("type", "text");
                        mediaEffects.Add(elementMedia);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty3);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty4);
                        XmlElement elementProperty8 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty8.SetAttribute("name", "text");
                        elementProperty8.SetAttribute("value", pre.Media[i].textMessage.ToString());
                        elementMedia.AppendChild(elementProperty8);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", (((pre.Media[i].mDuration * 60) + (pre.Media[i].sDuration))) + "");
                        elementMedia.PrependChild(elementProperty11);
                        XmlElement elementProperty12 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty12.SetAttribute("name", "scale");
                        elementProperty12.SetAttribute("value", pre.Media[i].scale.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty12);
                    }
                    if (pre.Media[i].type.Equals("VIDEO2D")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("src", pre.Media[i].src);
                        elementMedia.SetAttribute("type", "video");
                        mediaEffects.Add(elementMedia);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty3);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty4);
                        XmlElement elementProperty5 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty5.SetAttribute("name", "volume");
                        elementProperty5.SetAttribute("value", pre.Media[i].volume.ToString().Replace(",","."));
                        elementMedia.AppendChild(elementProperty5);
                        XmlElement elementProperty6 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty6.SetAttribute("name", "mute");
                        elementProperty6.SetAttribute("value", pre.Media[i].mute.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty6);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", (((pre.Media[i].mDuration * 60) + (pre.Media[i].sDuration))) + "");
                        elementMedia.PrependChild(elementProperty11);
                        XmlElement elementProperty12 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty12.SetAttribute("name", "scale");
                        elementProperty12.SetAttribute("value", pre.Media[i].scale.ToString().Replace(",", "."));
                        elementMedia.AppendChild(elementProperty12);
                    }
                    if (pre.Media[i].type.Equals("VIDEO360")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("src", pre.Media[i].src);
                        elementMedia.SetAttribute("type", "video");
                        mediaEffects.Add(elementMedia);
                        XmlElement elementProperty = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty.SetAttribute("name", "background");
                        elementProperty.SetAttribute("value", "true");
                        elementMedia.AppendChild(elementProperty);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty4);
                        XmlElement elementProperty5 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty5.SetAttribute("name", "volume");
                        elementProperty5.SetAttribute("value", pre.Media[i].volume.ToString().Replace(",","."));
                        elementMedia.AppendChild(elementProperty5);
                        XmlElement elementProperty6 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty6.SetAttribute("name", "mute");
                        elementProperty6.SetAttribute("value", pre.Media[i].mute.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty6);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", (((pre.Media[i].mDuration * 60) + (pre.Media[i].sDuration))) + "");
                        elementMedia.PrependChild(elementProperty11);
                    }
                    //insere as relações
                    //temos 4 opções de relacionamento para o start,
                    //sendo "OnBegin This Scene" para relacionar com a cena
                    //"OnBegin" para iniciarem juntos
                    //"OnEnd" para iniciar ao termino
                    //"Not Defined" para não iniciar, tambem usado quando a midia for iniciada por interações
                    if (pre.Media[i].rStart.Equals("OnBegin This Scene")) {
                        if (((int)pre.Media[i].mDelay + (int)pre.Media[i].sDelay) != 0) {//se possui delai, cria-se uma segunda reação
                            XmlElement elementRelationDelay = doc.CreateElement(string.Empty, "relation", string.Empty);
                            elementRelationDelay.SetAttribute("id", "Relation-" + idRelacao++);
                            elementRelationDelay.SetAttribute("type", "starts");
                            elementRelationDelay.SetAttribute("delay", (((pre.Media[i].mDelay * 60) + (pre.Media[i].sDelay))) + "");
                            XmlElement elementRelationPrimaryDelay = doc.CreateElement(string.Empty, "primary", string.Empty);
                            elementRelationPrimaryDelay.SetAttribute("component", "scene " + s);
                            elementRelationDelay.AppendChild(elementRelationPrimaryDelay);
                            XmlElement elementRelationSecondaryDelay = doc.CreateElement(string.Empty, "secondary", string.Empty);
                            elementRelationSecondaryDelay.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                            elementRelationDelay.AppendChild(elementRelationSecondaryDelay);
                            relacoesInternas.Add(elementRelationDelay);
                        } else {
                            XmlElement elementRelation1 = doc.CreateElement(string.Empty, "relation", string.Empty);
                            elementRelation1.SetAttribute("id", "Relation-" + idRelacao++);
                            elementRelation1.SetAttribute("type", "starts");
                            //primary relation
                            XmlElement elementRelationPrimary1 = doc.CreateElement(string.Empty, "primary", string.Empty);
                            elementRelationPrimary1.SetAttribute("component", "scene " + s);
                            elementRelation1.AppendChild(elementRelationPrimary1);
                            //secondary relation
                            XmlElement elementRelationSecondary1 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                            elementRelationSecondary1.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                            elementRelation1.AppendChild(elementRelationSecondary1);
                            relacoesInternas.Add(elementRelation1);
                        }
                    }
                    if (pre.Media[i].rStart.Equals("OnBegin")) {
                        XmlElement elementRelation1 = doc.CreateElement(string.Empty, "relation", string.Empty);
                        elementRelation1.SetAttribute("id", "Relation-" + idRelacao++);
                        elementRelation1.SetAttribute("type", "starts");
                        XmlElement elementRelationPrimary1 = doc.CreateElement(string.Empty, "primary", string.Empty);
                        elementRelationPrimary1.SetAttribute("component", pre.Media[i].rMediaStart.ToLower() + "_s" + s);
                        elementRelation1.AppendChild(elementRelationPrimary1);
                        XmlElement elementRelationSecondary1 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                        elementRelationSecondary1.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementRelation1.AppendChild(elementRelationSecondary1);
                        relacoesInternas.Add(elementRelation1);
                    }
                    if (pre.Media[i].rStart.Equals("OnEnd")) {
                        XmlElement elementRelation1 = doc.CreateElement(string.Empty, "relation", string.Empty);
                        elementRelation1.SetAttribute("id", "Relation-" + idRelacao++);
                        elementRelation1.SetAttribute("type", "meet");
                        XmlElement elementRelationPrimary1 = doc.CreateElement(string.Empty, "primary", string.Empty);
                        elementRelationPrimary1.SetAttribute("component", pre.Media[i].rMediaStart.ToLower() + "_s" + s);
                        elementRelation1.AppendChild(elementRelationPrimary1);
                        XmlElement elementRelationSecondary1 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                        elementRelationSecondary1.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementRelation1.AppendChild(elementRelationSecondary1);
                        relacoesInternas.Add(elementRelation1);
                    }
                    if (pre.Media[i].rStart.Equals("Not Defined")) {
                        //não faz nada
                    }
                    //temos 3 opções de relacionamento para o ends,
                    //sendo "End of This Media Time" para relacionar com a cena
                    //"OnBegin" para terminar com o inicio
                    //"OnEnd" para terminar com o termino
                    if (pre.Media[i].rEnd.Equals("End of This Media Time")) {
                        XmlElement elementRelation2 = doc.CreateElement(string.Empty, "relation", string.Empty);
                        elementRelation2.SetAttribute("id", "Relation-" + idRelacao++);
                        elementRelation2.SetAttribute("type", "finishes");
                        //primary relation
                        XmlElement elementRelationPrimary2 = doc.CreateElement(string.Empty, "primary", string.Empty);
                        elementRelationPrimary2.SetAttribute("component", "scene " + s);
                        elementRelation2.AppendChild(elementRelationPrimary2);
                        //secondary relation
                        XmlElement elementRelationSecondary2 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                        elementRelationSecondary2.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementRelation2.AppendChild(elementRelationSecondary2);
                        relacoesInternas.Add(elementRelation2);
                    }
                    if (pre.Media[i].rEnd.Equals("OnBegin")) {
                        XmlElement elementRelation2 = doc.CreateElement(string.Empty, "relation", string.Empty);
                        elementRelation2.SetAttribute("id", "Relation-" + idRelacao++);
                        elementRelation2.SetAttribute("type", "metBy"); //incluir na documentação do douglas
                        XmlElement elementRelationPrimary2 = doc.CreateElement(string.Empty, "primary", string.Empty);
                        elementRelationPrimary2.SetAttribute("component", pre.Media[i].rMediaStart.ToLower() + "_s" + s);
                        elementRelation2.AppendChild(elementRelationPrimary2);
                        XmlElement elementRelationSecondary2 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                        elementRelationSecondary2.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementRelation2.AppendChild(elementRelationSecondary2);
                        relacoesInternas.Add(elementRelation2);
                    }
                    if (pre.Media[i].rEnd.Equals("OnEnd")) {
                        XmlElement elementRelation2 = doc.CreateElement(string.Empty, "relation", string.Empty);
                        elementRelation2.SetAttribute("id", "Relation-" + idRelacao++);
                        elementRelation2.SetAttribute("type", "finishes");
                        XmlElement elementRelationPrimary2 = doc.CreateElement(string.Empty, "primary", string.Empty);
                        elementRelationPrimary2.SetAttribute("component", pre.Media[i].rMediaEnd.ToLower() + "_s" + s);
                        elementRelation2.AppendChild(elementRelationPrimary2);
                        XmlElement elementRelationSecondary2 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                        elementRelationSecondary2.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementRelation2.AppendChild(elementRelationSecondary2);
                        relacoesInternas.Add(elementRelation2);
                    }
                    //mudança de cena
                    if (pre.Media[i].interactive) {//se é uma mídia interativa
                        //criação de relacionamento e portas
                        if (pre.Media[i].startTarget) {//se é uma interação de start
                            if (pre.Media[i].interactiveTarget.ToLower().StartsWith("scene")) {//é uma cena
///////////********************************************************************************************************////////
                                /*
                                 * incluir o meet em futura atualização
	                              <relation id="xx-7" type="meet" >// sobre a mudança de cenas automaticamente
		                              <primary component="scene 1" interface="portVideo2d" />
		                              <secondary component="scene 2" />
	                              </relation>
                                 */
                                //porta da cena
                                XmlElement elementPort1 = doc.CreateElement(string.Empty, "port", string.Empty);
                                elementPort1.SetAttribute("id", "portChangeScene-" + (idPorta++));
                                elementPort1.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                                elementScene.PrependChild(elementPort1);
                                elementScene.PrependChild(doc.CreateComment("Portas desta cena"));
                                //relacionamento entre cenas
                                XmlElement elementRelation3 = doc.CreateElement(string.Empty, "relation", string.Empty);
                                elementRelation3.SetAttribute("id", "Relation-" + idRelacao++);
                                elementRelation3.SetAttribute("type", "onSelectionStarts");
                                elementRelation3.SetAttribute("keyCode", "trigger");
                                XmlElement elementRelationPrimary3 = doc.CreateElement(string.Empty, "primary", string.Empty);
                                elementRelationPrimary3.SetAttribute("component", "scene " + s);
                                elementRelationPrimary3.SetAttribute("interface", "portChangeScene-" + (idPorta-1));
                                elementRelation3.AppendChild(elementRelationPrimary3);
                                XmlElement elementRelationSecondary3 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                                if(pre.Media[i].interactiveTarget.ToLower().StartsWith("scene"))
                                    elementRelationSecondary3.SetAttribute("component", pre.Media[i].interactiveTarget.ToLower());
                                else
                                    elementRelationSecondary3.SetAttribute("component", pre.Media[i].interactiveTarget.Replace(" ", "").ToLower() + "_s" + s);
                                elementRelation3.AppendChild(elementRelationSecondary3);
                                relacoesExternas.Add(elementRelation3);
                            } else { //é um objeto de midia
                                XmlElement elementRelation3 = doc.CreateElement(string.Empty, "relation", string.Empty);
                                elementRelation3.SetAttribute("id", "Relation-" + idRelacao++);
                                elementRelation3.SetAttribute("type", "onSelectionStarts");
                                elementRelation3.SetAttribute("keyCode", "trigger");
                                XmlElement elementRelationPrimary3 = doc.CreateElement(string.Empty, "primary", string.Empty);
                                elementRelationPrimary3.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                                elementRelation3.AppendChild(elementRelationPrimary3);
                                XmlElement elementRelationSecondary3 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                                if (pre.Media[i].interactiveTarget.ToLower().StartsWith("scene"))
                                    elementRelationSecondary3.SetAttribute("component", pre.Media[i].interactiveTarget.ToLower());
                                else
                                    elementRelationSecondary3.SetAttribute("component", pre.Media[i].interactiveTarget.Replace(" ", "").ToLower() + "_s" + s);
                                elementRelation3.AppendChild(elementRelationSecondary3);
                                relacoesInternas.Add(elementRelation3);
                            }
                        } else { //se é uma interação de end, só pode finalizar objetos de mídia
                            XmlElement elementRelation3 = doc.CreateElement(string.Empty, "relation", string.Empty);
                            elementRelation3.SetAttribute("id", "Relation-" + idRelacao++);
                            elementRelation3.SetAttribute("type", "onSelectionEnds");
                            elementRelation3.SetAttribute("keyCode", "trigger");
                            XmlElement elementRelationPrimary3 = doc.CreateElement(string.Empty, "primary", string.Empty);
                            elementRelationPrimary3.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                            elementRelation3.AppendChild(elementRelationPrimary3);
                            XmlElement elementRelationSecondary3 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                            if (pre.Media[i].interactiveTarget.ToLower().StartsWith("scene"))
                                elementRelationSecondary3.SetAttribute("component", pre.Media[i].interactiveTarget.ToLower());
                            else
                                elementRelationSecondary3.SetAttribute("component", pre.Media[i].interactiveTarget.Replace(" ", "").ToLower() + "_s" + s);
                            elementRelation3.AppendChild(elementRelationSecondary3);
                            relacoesInternas.Add(elementRelation3);
                        } 
                    }
                    //fecha a cena
                    //se so tiver uma midia salva ela no primary
                    if (!elementScene.HasAttribute("primaryComponent") && //se nao tem um primaryComponent cadastrado
                        !pre.Media[i].rStart.Equals("Not Defined") && //se não tiver o starts "Not Defined"
                        pre.Media.Count == 1) {//se for somente uma midia
                        scenePrimaryComponent = pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s;
                        //adiciona ao filtro pro retirar o relacionamento
                        elementScene.SetAttribute("primaryComponent", scenePrimaryComponent);
                        if((pre.Media[i]?.mDelay + pre.Media[i]?.sDelay) > 0)//se tiver delay inclui
                            elementScene.SetAttribute("delay", ((pre.Media[i]?.mDelay * 60) + pre.Media[i]?.sDelay).ToString());
                    } 
                    //se tiver mais que 1 midia
                    //primaryComponent com a primeira mídia da cena sem delay
                    if (!elementScene.HasAttribute("primaryComponent") &&
                        !pre.Media[i].rStart.Equals("Not Defined") &&
                        ((pre.Media[i].mDelay + pre.Media[i].sDelay) == 0)) {
                        //alem de incluir o primaryComponent na cena, salva para porterior filtro
                        scenePrimaryComponent = pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s;
                        elementScene.SetAttribute("primaryComponent", scenePrimaryComponent);
                    }
                    elementBody.AppendChild(elementScene);
                }
                //se todas as midias apresentarem delay, pega-se a de menor delay 
                if (!elementScene.HasAttribute("primaryComponent")) {
                    int minour = 99999999;
                    for (int pc = 0; pc < pre.Media.Count; pc++) 
                        if ((pre.Media[pc].mDelay + pre.Media[pc].sDelay) <= minour &&
                            !pre.Media[pc].rStart.Equals("Not Defined")) {
                            minour = pre.Media[pc].mDelay + pre.Media[pc].sDelay;
                            scenePrimaryComponent = pre.Media[pc].name.Replace(" ", "").ToLower() + "_s" + s;
                            elementScene.SetAttribute("primaryComponent", scenePrimaryComponent);
                            elementScene.SetAttribute("delay", ((pre.Media[pc]?.mDelay * 60) + pre.Media[pc]?.sDelay).ToString());
                        } 
                }
                /*
                    //primaryComponent caso todas as midias tenham delay, pega a primeira sem tratamento de menor
                    if (!elementScene.HasAttribute("primaryComponent") && pre.Media.Count > 0) {
                        elementScene.SetAttribute("primaryComponent", pre.Media[0].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementScene.SetAttribute("delay", ((((int)pre.Media[0].mDelay * 60) + ((int)pre.Media[0].sDelay))) + "");
                    }
                */
                //imprimindo as midias e efeitos sensoriais
                if(mediaEffects.Count > 0)
                    elementScene.AppendChild(doc.CreateComment("Mídias e Efeitos Sensoriais desta cena"));
                foreach (XmlElement me in mediaEffects)
                    elementScene.AppendChild(me);
                //imprimindo os relacionamentos da cena
                elementScene.AppendChild(doc.CreateComment("Relações desta cena"));
                //filtros de relações
                List<string> nullRelStarts = new List<string>();//midas a passarem por friltro
                List<XmlElement> nullNodeStarts = new List<XmlElement>(); //relações excluidas
                //procura-se os onSelectionStarts que são prioritários em relação aos starts
                foreach (XmlElement ri1 in relacoesInternas) {
                    if (ri1.Attributes["type"].Value.Equals("onSelectionStarts")) {
                        XmlNodeList list = ri1.ChildNodes;
                        for (int i = 0; i < list.Count; i++) {
                            XmlNode no = list[i];
                            if (no.Name.ToLower().Equals("secondary"))//pegase a midia secundária
                                nullRelStarts.Add(no.Attributes["component"].Value.ToLower());
                        }
                    }
                }
                foreach (XmlElement ri2 in relacoesInternas) { //procura-se se existe algum starts que caia no filtro
                    if (ri2.Attributes["type"].Value.Equals("starts")) {
                        XmlNodeList list = ri2.ChildNodes;
                        for (int i = 0; i < list.Count; i++) {
                            XmlNode no = list[i];
                            if (no.Name.ToLower().Equals("secondary")) 
                                if (nullRelStarts.Contains(no.Attributes["component"].Value.ToLower())) 
                                    nullNodeStarts.Add(ri2); //adiciona-se a lista de exclusão          
                        }
                    }
                    //verifica se o primaryComponent da cena equivale tem algum start, se tiver salva na lista de filtro
                    if (ri2.Attributes["type"].Value.ToLower().Equals("starts") ||
                        ri2.Attributes["type"].Value.Equals("onSelectionStarts")) {
                        XmlNodeList list = ri2.ChildNodes;
                        for (int i = 0; i < list.Count; i++) {
                            XmlNode no = list[i];
                            if (no.Name.ToLower().Equals("secondary"))
                                if (no.Attributes["component"].Value.ToLower().Equals(scenePrimaryComponent.ToLower()))
                                    nullNodeStarts.Add(ri2);
                        }
                    }
                }
                //retira da lista as relações excluidas
                foreach (XmlElement ri3 in nullNodeStarts) 
                    relacoesInternas.Remove(ri3);
                //relacionado a salvar as mídias não filtradas
                foreach (XmlElement ri4 in relacoesInternas) 
                    elementScene.AppendChild(ri4);
            }
        }
        //imprimindo os relacionamentos da cena
        if(relacoesExternas.Count > 0)
            elementBody.AppendChild(doc.CreateComment("Relacionamentos entre cenas"));
        foreach (XmlElement ro in relacoesExternas) 
            elementBody.AppendChild(ro);
        //salvamento do arquivo
        doc.Save(folder + "/" + file);
        if (File.Exists(folder + "/" + "projeto.xml"))
            Debug.Log("File projeto.xml written");
        else
            Debug.LogError("File projeto.xml not written!");
    }
    public void deleteProject() {
        if (File.Exists(folder + "/" + "projeto.xml")) {
            File.Delete(folder + "/" + "projeto.xml");
            Debug.Log("File projeto.xml erased");
        } else
            Debug.LogError("File projeto.xml not erased!");
    }
}
