using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml.Linq;
using System.Linq;
using System.Data;
using System;
using System.Text;
using System.Xml.XPath;

public class MultiSel : MonoBehaviour{
    private string folder, file;
    private Presentation pre;
    private void Start() {
        folder = Application.persistentDataPath + "/temp/";
        file = "ProjetoMultiSel.xml";
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        importProject();
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            exportProject();
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            importProject();
        }
    }
    //le o arquivo xml multisel
    public void importProject() {
        //Debug.LogError(elementScene.GetAttributeNode("primaryComponent").Value);
        Debug.LogError("****************************");

        XmlDocument doc = new XmlDocument();
        doc.Load(folder + file);

        XmlNodeList elemBody = doc.GetElementsByTagName("body");//retorna 1
        XmlNode nodeBody = elemBody[0];
        //Debug.LogWarning(nodeBody.Name);
        //Debug.LogWarning(nodeBody.Attributes["primaryComponent"].Value);
        //Debug.Log("*******************");
        XmlNodeList elemScene = doc.GetElementsByTagName("scene");//retorna 2
        for (int i = 0; i < elemScene.Count; i++) {
            XmlNode nodeScene = elemScene[i];
            //Debug.LogWarning(nodeScene.Name);
            Debug.LogWarning(nodeScene.Attributes["id"].Value);
            //Debug.LogWarning(nodeScene.Attributes["primaryComponent"].Value);
            for (int j = 0; j < nodeScene.ChildNodes.Count; j++) {//17 e 12 vezes
                //Debug.Log(nodeScene.ChildNodes[j].InnerText);// assim imprime os comentáris da cena
                if (!nodeScene.ChildNodes[j].Name.Equals("#comment")) {//se não for comentario
                    if (nodeScene.ChildNodes[j].Name.Equals("port")) {//se for porta da cena
                        //imprime o nome e atributos do no
                        Debug.LogWarning("no: " + nodeScene.ChildNodes[j].Name);
                        Debug.Log("id: " + nodeScene.ChildNodes[j].Attributes["id"]?.Value);
                        Debug.Log("component: " + nodeScene.ChildNodes[j].Attributes["component"]?.Value);
                    }
                    if (nodeScene.ChildNodes[j].Name.Equals("media")) {
                        Debug.LogWarning("no: " + nodeScene.ChildNodes[j].Name);
                        Debug.Log("id: " + nodeScene.ChildNodes[j].Attributes["id"]?.Value);
                        Debug.Log("src: " + nodeScene.ChildNodes[j].Attributes["src"]?.Value);
                        Debug.Log("type: " + nodeScene.ChildNodes[j].Attributes["type"]?.Value);
                        //imprime os nos dos filhos de media com seus atributos
                        for (int x = 0; x < nodeScene.ChildNodes[j].ChildNodes.Count; x++) {
                            XmlNode nodeChild = nodeScene.ChildNodes[j].ChildNodes[x];
                            Debug.Log("filho-property-name: " + nodeChild.Name);
                            Debug.Log("filho-name: " + nodeChild.Attributes["name"]?.Value);
                            Debug.Log("filho-value: " + nodeChild.Attributes["value"]?.Value);
                        }
                    }
                    if (nodeScene.ChildNodes[j].Name.Equals("effect")) {
                        Debug.LogWarning("no: " + nodeScene.ChildNodes[j].Name);
                        Debug.Log("id: " + nodeScene.ChildNodes[j].Attributes["id"]?.Value);
                        Debug.Log("type: " + nodeScene.ChildNodes[j].Attributes["type"]?.Value);
                        for (int x = 0; x < nodeScene.ChildNodes[j].ChildNodes.Count; x++) {
                            XmlNode nodeChild = nodeScene.ChildNodes[j].ChildNodes[x];
                            Debug.Log("filho-property-name: " + nodeChild.Name);
                            Debug.Log("filho-name: " + nodeChild.Attributes["name"]?.Value);
                            Debug.Log("filho-value: " + nodeChild.Attributes["value"]?.Value);
                        }
                    }
                    if (nodeScene.ChildNodes[j].Name.Equals("relation")) {
                        Debug.LogWarning("no: " + nodeScene.ChildNodes[j].Name);
                        Debug.Log("id: " + nodeScene.ChildNodes[j].Attributes["id"]?.Value);
                        Debug.Log("type: " + nodeScene.ChildNodes[j].Attributes["type"]?.Value);
                        Debug.Log("delay: " + nodeScene.ChildNodes[j].Attributes["delay"]?.Value);
                        Debug.Log("keyCode: " + nodeScene.ChildNodes[j].Attributes["keyCode"]?.Value);
                        for (int x = 0; x < nodeScene.ChildNodes[j].ChildNodes.Count; x++) {
                            XmlNode nodeChild = nodeScene.ChildNodes[j].ChildNodes[x];
                            Debug.Log("filho-property-name: " + nodeChild.Name);
                            Debug.Log("filho-name: " + nodeChild.Attributes["component"]?.Value);
                            //Debug.Log("filho-name: " + nodeChild.Attributes["interface"]?.Value);
                        }
                    }
                }
            }
        }
        /*
        //imprime os relacionamentos externos a cena
        XmlNodeList elemsRelOut = doc.GetElementsByTagName("relation");
        for (int i = 0; i < elemsRelOut.Count; i++) {
            XmlNode elemRelOut = elemsRelOut[i];
            if (elemRelOut.HasChildNodes) {
                Debug.LogWarning(elemRelOut.Name);
                Debug.LogWarning(elemRelOut.Attributes["id"]?.Value);
                Debug.LogWarning(elemRelOut.Attributes["type"]?.Value);
                Debug.LogWarning(elemRelOut.Attributes["keyCode"]?.Value);
            }
        }
        */
        

        //Debug.Log(nodeScene.ChildNodes[j].InnerText);// assim imprime os comentáris da cena
        /*
        XmlDocument document = new XmlDocument();
        document.Load(folder + file);
        XPathNavigator navigator = document.CreateNavigator();

        navigator.MoveToRoot();
        //navigator.DeleteSelf();

        Debug.LogWarning("Position after delete: "+ navigator.Name);
        Console.WriteLine(navigator.OuterXml);
        */
        /*
        string filePath = folder + file;
        List<string> strList = File.ReadAllLines(filePath).ToList();
        StringBuilder sb = new StringBuilder();
        int ctr = 0;
        foreach (string str in strList) {
            ctr++;
            if (ctr == 1 || ctr == strList.Count)
                continue;
            sb.Append(str);
        }
        Debug.LogWarning(sb);
        */
        /*
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreWhitespace = true;
        using (var fileStream = File.OpenText(folder + file))
        using (XmlReader reader = XmlReader.Create(fileStream, settings)) {
            while (reader.Read()) {
                switch (reader.NodeType) {
                    case XmlNodeType.Element:
                        Debug.Log($"Start Element: {reader.Name}. Has Attributes? : {reader.HasAttributes}");
                        break;
                    case XmlNodeType.Text:
                        Debug.Log($"Inner Text: {reader.Value}");
                        break;
                    case XmlNodeType.EndElement:
                        Debug.Log($"End Element: {reader.Name}");
                        break;
                    default:
                        Debug.Log($"Unknown: {reader.NodeType}");
                        break;
                }
            }
        }
        */
        /*
         //bom, mas não le profundamente
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreWhitespace = true;
        using (var fileStream = File.OpenText(folder + file))
        using (XmlReader reader = XmlReader.Create(fileStream, settings)) {
            while (reader.Read()) {
                switch (reader.NodeType) {
                    case XmlNodeType.Element:
                        Debug.Log($"Start Element: {reader.Name}. Has Attributes? : {reader.HasAttributes}");
                        break;
                    case XmlNodeType.Text:
                        Debug.Log($"Inner Text: {reader.Value}");
                        break;
                    case XmlNodeType.EndElement:
                        Debug.Log($"End Element: {reader.Name}");
                        break;
                    default:
                        Debug.Log($"Unknown: {reader.NodeType}");
                        break;
                }
            }
        }
        */
        /*
        var elements = XElement.Load(folder + file);
        var data = elements.Descendants("body").Where(st => (int)st.Element("scene").Attribute("id") == 1);
        foreach(var no in data) {
            Debug.Log(no.Elements("relation"));
        }
        */
        /*
        xdoc.Descendants("media").Select(pre => new {
            id = pre.Attribute("id").Value,
            name = pre.Attribute("name").Value,
            value = pre.Attribute("value").Value
        }).ToList().ForEach(p => {
            Debug.Log("id:" + p.id);
            Debug.Log("name:" + p.name);
            Debug.Log("value:" + p.value);
        });
        */
        /*
        List<string> lstlevel = new List<string>();
        XmlDocument doc = new XmlDocument();
        doc.Load(folder + file);
        doc.GetElementsByTagName("multisel");
        XmlNode root = doc.FirstChild;
        */

        /*
        XmlTextReader xmlReader = new XmlTextReader(folder + file);
        while (xmlReader.Read()) {
            switch (xmlReader.NodeType) {
                case XmlNodeType.Element:
                    Debug.Log("<" + xmlReader.Name + ">");
                    break;
                case XmlNodeType.Text:
                    Debug.Log(xmlReader.Value);
                    break;
                case XmlNodeType.EndElement:
                    Debug.Log("<" + xmlReader.Name + ">");
                    break;
            }
        }

        */
        /*
        XmlNode root = doc.FirstChild;
        if (root.HasChildNodes) {
            for (int i = 0; i < root.ChildNodes.Count; i++) {
                Debug.Log(root.ChildNodes[i].InnerText);
            }
        }
        */
        /*
        XmlNodeList nodes = doc.SelectNodes("//multisel");
        foreach (XmlNode node in nodes) {
            XmlNode body = node.SelectSingleNode("body");
            if (body != null) {
                Debug.Log(body.InnerText);
            }
            XmlNode port = node.SelectSingleNode("port");
            if(port != null) {
                Debug.Log(port.InnerText);
            }
            XmlNode relation = node.SelectSingleNode("relation");
            if (relation != null) {
                Debug.Log(relation.InnerText);
            }
            XmlNode scene = node.SelectSingleNode("scene");
            if (scene != null) {
                Debug.Log(scene.InnerText);
            }
        }
        */
        /*
        //XmlNode root = doc.FirstChild;
        //Debug.Log(root.Name);

        if (root.HasChildNodes) {
            // get all nodes with tag name "Level"
            foreach (XmlNode node in root.ChildNodes) {
                Debug.Log(node.Name);
                foreach (XmlNode node1 in node.ChildNodes) {
                    Debug.Log(node1.Name);
                    foreach (XmlNode node2 in node.ChildNodes) {
                        Debug.Log(node2.Name);
                        if (node2.Name == "id") {
                            lstlevel.Add(node2.InnerText);

                        }
                    }
                }
            }
        }

        */

        Debug.LogError("****************************");
        /*
        XmlTextReader reader = new XmlTextReader(folder + file);
        reader.MoveToContent();
        //reader.Skip();
        while (reader.MoveToNextAttribute()) {
            XmlNode a = doc.ReadNode(reader);
            Debug.Log(a.Name);
            Debug.Log(a.Value);
            Debug.Log(a.Attributes);
        }

        XmlTextReader xmlReader = new XmlTextReader(folder + file);
        while (xmlReader.Read()) {
            switch (xmlReader.NodeType) {
                case XmlNodeType.Element:
                    Debug.Log("<" + xmlReader.Name + ">");
                    break;
                case XmlNodeType.Text:
                    Debug.Log(xmlReader.Value);
                    break;
                case XmlNodeType.EndElement:
                    Debug.Log("<" + xmlReader.Name + ">");
                    break;
            }
        }
        */
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
            if (File.Exists(folder + "Scene " + s + ".xml")) {
                //cria scene
                XmlElement elementScene = doc.CreateElement(string.Empty, "scene", string.Empty);
                elementScene.SetAttribute("id", "scene "+s);
                //^abre a cena^
                //carrega o xml serializado da cena no objeto pre
                pre = SerializeOp.Deserialize<Presentation>(folder + "Scene " + s + ".xml");
                //agrupa as midias para imprimir depois
                List<XmlElement> mediaEffects = new List<XmlElement>();
                //agrupa as relações para imprimir depois
                List<XmlElement> relacoes = new List<XmlElement>();
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
                        elementProperty11.SetAttribute("value", ((((int)pre.Media[i].mDuration * 60) + ((int)pre.Media[i].sDuration))) + "");
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
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", ((((int)pre.Media[i].mDuration * 60) + ((int)pre.Media[i].sDuration))) + "");
                        elementMedia.PrependChild(elementProperty11);
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
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", ((((int)pre.Media[i].mDuration * 60) + ((int)pre.Media[i].sDuration))) + "");
                        elementMedia.PrependChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("INTERACT")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("type", "image/x-icon");
                        mediaEffects.Add(elementMedia);
                        XmlElement elementProperty = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty.SetAttribute("name", "background");
                        elementProperty.SetAttribute("value", "true");
                        elementMedia.AppendChild(elementProperty);
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
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", ((((int)pre.Media[i].mDuration * 60) + ((int)pre.Media[i].sDuration))) + "");
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
                        elementProperty11.SetAttribute("value", ((((int)pre.Media[i].mDuration * 60) + ((int)pre.Media[i].sDuration))) + "");
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
                        XmlElement elementProperty9 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty9.SetAttribute("name", "intensity");
                        elementProperty9.SetAttribute("value", (float.Parse(pre.Media[i].intensity) / 10).ToString().Replace(",","."));
                        elementEffect.AppendChild(elementProperty9);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", ((((int)pre.Media[i].mDuration * 60) + ((int)pre.Media[i].sDuration))) + "");
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
                        XmlElement elementProperty9 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty9.SetAttribute("name", "intensity");
                        elementProperty9.SetAttribute("value", (float.Parse(pre.Media[i].intensity) / 10).ToString().Replace(",","."));
                        elementEffect.AppendChild(elementProperty9);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", ((((int)pre.Media[i].mDuration * 60) + ((int)pre.Media[i].sDuration))) + "");
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
                        XmlElement elementProperty9 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty9.SetAttribute("name", "intensity");
                        elementProperty9.SetAttribute("value", (float.Parse(pre.Media[i].intensity) / 10).ToString().Replace(",", "."));
                        elementEffect.AppendChild(elementProperty9);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", ((((int)pre.Media[i].mDuration * 60) + ((int)pre.Media[i].sDuration))) + "");
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
                        XmlElement elementProperty8 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty8.SetAttribute("name", "text");
                        elementProperty8.SetAttribute("value", pre.Media[i].textMessage.ToString());
                        elementMedia.AppendChild(elementProperty8);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", ((((int)pre.Media[i].mDuration * 60) + ((int)pre.Media[i].sDuration))) + "");
                        elementMedia.PrependChild(elementProperty11);
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
                        elementProperty11.SetAttribute("value", ((((int)pre.Media[i].mDuration * 60) + ((int)pre.Media[i].sDuration))) + "");
                        elementMedia.PrependChild(elementProperty11);
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
                        elementProperty11.SetAttribute("value", ((((int)pre.Media[i].mDuration * 60) + ((int)pre.Media[i].sDuration))) + "");
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
                            elementRelationDelay.SetAttribute("delay", ((((int)pre.Media[i].mDelay * 60) + ((int)pre.Media[i].sDelay))) + "");
                            XmlElement elementRelationPrimaryDelay = doc.CreateElement(string.Empty, "primary", string.Empty);
                            elementRelationPrimaryDelay.SetAttribute("component", "scene " + s);
                            elementRelationDelay.AppendChild(elementRelationPrimaryDelay);
                            XmlElement elementRelationSecondaryDelay = doc.CreateElement(string.Empty, "secondary", string.Empty);
                            elementRelationSecondaryDelay.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                            elementRelationDelay.AppendChild(elementRelationSecondaryDelay);
                            relacoes.Add(elementRelationDelay);
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
                            relacoes.Add(elementRelation1);
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
                        relacoes.Add(elementRelation1);
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
                        relacoes.Add(elementRelation1);
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
                        relacoes.Add(elementRelation2);
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
                        relacoes.Add(elementRelation2);
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
                        relacoes.Add(elementRelation2);
                    }
                    //mudança de cena
                    if (pre.Media[i].interactive) {//se é uma mídia interativa
                        //criação de relacionamento e portas
                        if (pre.Media[i].startTarget) {//se é uma interação de start
                            if (pre.Media[i].interactiveTarget.ToLower().StartsWith("scene")) {//é uma cena

                                /*
                                 * incluir o meets em futura atualização
	                              <relation id="xx-7" type="meets" >// sobre a mudança de cenas automaticamente
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
                                elementRelation3.SetAttribute("id", "Relation-" + i);
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
                                relacoes.Add(elementRelation3);
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
                            relacoes.Add(elementRelation3);
                        } 
                    }
                    //fecha a cena
                    //primaryComponent com a primeira mídia da cena sem delay
                    if (!elementScene.HasAttribute("primaryComponent") && (((int)pre.Media[i].mDelay + (int)pre.Media[i].sDelay) == 0)) {
                        elementScene.SetAttribute("primaryComponent", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        //excluir o relacionamento equivalente
                    }
                    elementBody.AppendChild(elementScene);
                }
                /*
                //primaryComponent caso todas as midias tenham delay, pega a primeira sem tratamento de menor
                if (!elementScene.HasAttribute("primaryComponent") && pre.Media.Count > 0) {
                    elementScene.SetAttribute("primaryComponent", pre.Media[0].name.Replace(" ", "").ToLower() + "_s" + s);
                    elementScene.SetAttribute("delay", ((((int)pre.Media[0].mDelay * 60) + ((int)pre.Media[0].sDelay))) + "");
                }
                */
                //imprimindo as midias e efeitos sensoriais
                elementScene.AppendChild(doc.CreateComment("Mídias e SE desta cena"));
                foreach (XmlElement m in mediaEffects)
                    elementScene.AppendChild(m);
                //imprimindo os relacionamentos da cena
                elementScene.AppendChild(doc.CreateComment("Relações desta cena"));
                foreach (XmlElement m in relacoes)
                    elementScene.AppendChild(m);
            }
        }
        //imprimindo os relacionamentos da cena
        elementBody.AppendChild(doc.CreateComment("Relacionamentos entre cenas"));
        foreach (XmlElement m in relacoesExternas)
            elementBody.AppendChild(m);
        //salvamento do arquivo
        doc.Save(folder + file);
        if (File.Exists(folder + "ProjetoMultiSel.xml"))
            Debug.LogWarning("PROJETO MULTISEL CRIADO!");
        else
            Debug.LogError("ERRO AO CRIAR O PROJETO");
    }
}
