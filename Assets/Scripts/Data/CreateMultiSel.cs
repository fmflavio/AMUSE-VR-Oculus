using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreateMultiSel : MonoBehaviour{
    private string folder;
    private Presentation pre;
    private int id = 0;
    private void Start() {
        folder = Application.persistentDataPath + "/temp/";
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        create();
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            create();
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            ;
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            ;
        }
    }
    //cria o arquivo xml multisel
    public void create() {
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
        elementMultiSel.AppendChild(elementBody);

        //scenes do body
        elementBody.AppendChild(doc.CreateComment("Inicia as cenas do projeto"));
        //percorre as cenas existentes
        for (int s = 1; s <= 5; s++) {
            if (File.Exists(folder + "Scene " + s + ".xml")) {
                //cria scene
                XmlElement elementScene = doc.CreateElement(string.Empty, "scene", string.Empty);
                elementScene.SetAttribute("id", "scene "+s);
                //elementScene.SetAttribute("primaryComponent", "Video360-1");
                elementBody.AppendChild(elementScene);

                //carrega o xml serializado da cena no objeto pre
                pre = SerializeOp.Deserialize<Presentation>(folder + "Scene " + s + ".xml");
                elementScene.AppendChild(doc.CreateComment("Mídias desta cena"));
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
                        elementScene.AppendChild(elementMedia);
                        //insere as propriedades da mídia                    
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString());
                        elementMedia.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString());
                        elementMedia.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString());
                        elementMedia.AppendChild(elementProperty3);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty4);
                        XmlElement elementProperty5 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty5.SetAttribute("name", "volume");
                        elementProperty5.SetAttribute("value", (float.Parse(pre.Media[i].volume) * 100) + "%");
                        elementMedia.AppendChild(elementProperty5);
                        XmlElement elementProperty6 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty6.SetAttribute("name", "mute");
                        elementProperty6.SetAttribute("value", pre.Media[i].mute.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty6);
                        XmlElement elementProperty10 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty10.SetAttribute("name", "delay");
                        elementProperty10.SetAttribute("value", pre.Media[i].mDelay + ":" + pre.Media[i].sDelay);
                        elementMedia.AppendChild(elementProperty10);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", pre.Media[i].mDuration + ":" + pre.Media[i].sDuration);
                        elementMedia.AppendChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("IMAGE2D")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("src", pre.Media[i].src);
                        elementMedia.SetAttribute("type", "image");
                        elementScene.AppendChild(elementMedia);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString());
                        elementMedia.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString());
                        elementMedia.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString());
                        elementMedia.AppendChild(elementProperty3);
                        XmlElement elementProperty7 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty7.SetAttribute("name", "scale");
                        elementProperty7.SetAttribute("value", pre.Media[i].scale.ToString());
                        //elementMedia.AppendChild(elementProperty7);
                        XmlElement elementProperty10 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty10.SetAttribute("name", "delay");
                        elementProperty10.SetAttribute("value", pre.Media[i].mDelay + ":" + pre.Media[i].sDelay);
                        elementMedia.AppendChild(elementProperty10);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", pre.Media[i].mDuration + ":" + pre.Media[i].sDuration);
                        elementMedia.AppendChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("IMAGE360")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("src", pre.Media[i].src);
                        elementMedia.SetAttribute("type", "image");
                        elementScene.AppendChild(elementMedia);
                        XmlElement elementProperty = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty.SetAttribute("name", "background");
                        elementProperty.SetAttribute("value", "true");
                        elementMedia.AppendChild(elementProperty);
                        XmlElement elementProperty10 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty10.SetAttribute("name", "delay");
                        elementProperty10.SetAttribute("value", pre.Media[i].mDelay + ":" + pre.Media[i].sDelay);
                        elementMedia.AppendChild(elementProperty10);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", pre.Media[i].mDuration + ":" + pre.Media[i].sDuration);
                        elementMedia.AppendChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("INTERACT")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("type", "image/x-icon");
                        elementScene.AppendChild(elementMedia);
                        XmlElement elementProperty = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty.SetAttribute("name", "background");
                        elementProperty.SetAttribute("value", "true");
                        elementMedia.AppendChild(elementProperty);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString());
                        elementMedia.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString());
                        elementMedia.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString());
                        elementMedia.AppendChild(elementProperty3);
                        XmlElement elementProperty10 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty10.SetAttribute("name", "delay");
                        elementProperty10.SetAttribute("value", pre.Media[i].mDelay + ":" + pre.Media[i].sDelay);
                        elementMedia.AppendChild(elementProperty10);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", pre.Media[i].mDuration + ":" + pre.Media[i].sDuration);
                        elementMedia.AppendChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("PIP")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("src", pre.Media[i].src);
                        elementMedia.SetAttribute("type", "video");
                        elementScene.AppendChild(elementMedia);
                        XmlElement elementProperty = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty.SetAttribute("name", "fixedforward");
                        elementProperty.SetAttribute("value", "true");
                        elementMedia.AppendChild(elementProperty);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty4);
                        XmlElement elementProperty5 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty5.SetAttribute("name", "volume");
                        elementProperty5.SetAttribute("value", (float.Parse(pre.Media[i].volume) * 100) + "%");
                        elementMedia.AppendChild(elementProperty5);
                        XmlElement elementProperty6 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty6.SetAttribute("name", "mute");
                        elementProperty6.SetAttribute("value", pre.Media[i].mute.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty6);
                        XmlElement elementProperty10 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty10.SetAttribute("name", "delay");
                        elementProperty10.SetAttribute("value", pre.Media[i].mDelay + ":" + pre.Media[i].sDelay);
                        elementMedia.AppendChild(elementProperty10);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", pre.Media[i].mDuration + ":" + pre.Media[i].sDuration);
                        elementMedia.AppendChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("SELIGHT")) {//segundo NCL o type se define assim application/x−sensory−effect-LightType
                        elementEffect.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementEffect.SetAttribute("type", "application/x−sensory−effect-LightType");
                        elementScene.AppendChild(elementEffect);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString());
                        elementEffect.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString());
                        elementEffect.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString());
                        elementEffect.AppendChild(elementProperty3);
                        XmlElement elementProperty9 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty9.SetAttribute("name", "intensity");
                        elementProperty9.SetAttribute("value", pre.Media[i].intensity.ToString());
                        elementEffect.AppendChild(elementProperty9);
                        XmlElement elementProperty10 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty10.SetAttribute("name", "delay");
                        elementProperty10.SetAttribute("value", pre.Media[i].mDelay + ":" + pre.Media[i].sDelay);
                        elementEffect.AppendChild(elementProperty10);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", pre.Media[i].mDuration + ":" + pre.Media[i].sDuration);
                        elementEffect.AppendChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("SESTEAM")) {
                        elementEffect.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementEffect.SetAttribute("type", "application/x−sensory−effect-SteamType");
                        elementScene.AppendChild(elementEffect);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString());
                        elementEffect.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString());
                        elementEffect.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString());
                        elementEffect.AppendChild(elementProperty3);
                        XmlElement elementProperty9 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty9.SetAttribute("name", "intensity");
                        elementProperty9.SetAttribute("value", pre.Media[i].intensity.ToString());
                        elementEffect.AppendChild(elementProperty9);
                        XmlElement elementProperty10 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty10.SetAttribute("name", "delay");
                        elementProperty10.SetAttribute("value", pre.Media[i].mDelay + ":" + pre.Media[i].sDelay);
                        elementEffect.AppendChild(elementProperty10);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", pre.Media[i].mDuration + ":" + pre.Media[i].sDuration);
                        elementEffect.AppendChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("SEWIND")) {
                        elementEffect.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementEffect.SetAttribute("type", "application/x−sensory−effect-WindType");
                        elementScene.AppendChild(elementEffect);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString());
                        elementEffect.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString());
                        elementEffect.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString());
                        elementEffect.AppendChild(elementProperty3);
                        XmlElement elementProperty9 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty9.SetAttribute("name", "intensity");
                        elementProperty9.SetAttribute("value", pre.Media[i].intensity.ToString());
                        elementEffect.AppendChild(elementProperty9);
                        XmlElement elementProperty10 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty10.SetAttribute("name", "delay");
                        elementProperty10.SetAttribute("value", pre.Media[i].mDelay + ":" + pre.Media[i].sDelay);
                        elementEffect.AppendChild(elementProperty10);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", pre.Media[i].mDuration + ":" + pre.Media[i].sDuration);
                        elementEffect.AppendChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("TEXTMESSAGE")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("type", "text");
                        elementScene.AppendChild(elementMedia);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString());
                        elementMedia.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString());
                        elementMedia.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString());
                        elementMedia.AppendChild(elementProperty3);
                        XmlElement elementProperty7 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty7.SetAttribute("name", "scale");
                        elementProperty7.SetAttribute("value", pre.Media[i].scale.ToString());
                        //elementMedia.AppendChild(elementProperty7);
                        XmlElement elementProperty8 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty8.SetAttribute("name", "text");
                        elementProperty8.SetAttribute("value", pre.Media[i].textMessage.ToString());
                        elementMedia.AppendChild(elementProperty8);
                        XmlElement elementProperty10 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty10.SetAttribute("name", "delay");
                        elementProperty10.SetAttribute("value", pre.Media[i].mDelay + ":" + pre.Media[i].sDelay);
                        elementMedia.AppendChild(elementProperty10);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", pre.Media[i].mDuration + ":" + pre.Media[i].sDuration);
                        elementMedia.AppendChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("VIDEO2D")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("src", pre.Media[i].src);
                        elementMedia.SetAttribute("type", "video");
                        elementScene.AppendChild(elementMedia);
                        XmlElement elementProperty1 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty1.SetAttribute("name", "x");
                        elementProperty1.SetAttribute("value", pre.Media[i].px.ToString());
                        elementMedia.AppendChild(elementProperty1);
                        XmlElement elementProperty2 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty2.SetAttribute("name", "y");
                        elementProperty2.SetAttribute("value", pre.Media[i].py.ToString());
                        elementMedia.AppendChild(elementProperty2);
                        XmlElement elementProperty3 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty3.SetAttribute("name", "z");
                        elementProperty3.SetAttribute("value", pre.Media[i].pz.ToString());
                        elementMedia.AppendChild(elementProperty3);
                        XmlElement elementProperty4 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty4.SetAttribute("name", "loop");
                        elementProperty4.SetAttribute("value", pre.Media[i].loop.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty4);
                        XmlElement elementProperty5 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty5.SetAttribute("name", "volume");
                        elementProperty5.SetAttribute("value", (float.Parse(pre.Media[i].volume) * 100) + "%");
                        elementMedia.AppendChild(elementProperty5);
                        XmlElement elementProperty6 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty6.SetAttribute("name", "mute");
                        elementProperty6.SetAttribute("value", pre.Media[i].mute.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty6);
                        XmlElement elementProperty7 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty7.SetAttribute("name", "scale");
                        elementProperty7.SetAttribute("value", pre.Media[i].scale.ToString());
                        //elementMedia.AppendChild(elementProperty7);
                        XmlElement elementProperty10 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty10.SetAttribute("name", "delay");
                        elementProperty10.SetAttribute("value", pre.Media[i].mDelay + ":" + pre.Media[i].sDelay);
                        elementMedia.AppendChild(elementProperty10);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", pre.Media[i].mDuration + ":" + pre.Media[i].sDuration);
                        elementMedia.AppendChild(elementProperty11);
                    }
                    if (pre.Media[i].type.Equals("VIDEO360")) {
                        elementMedia.SetAttribute("id", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementMedia.SetAttribute("src", pre.Media[i].src);
                        elementMedia.SetAttribute("type", "video");
                        elementScene.AppendChild(elementMedia);
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
                        elementProperty5.SetAttribute("value", (float.Parse(pre.Media[i].volume) * 100) + "%");
                        elementMedia.AppendChild(elementProperty5);
                        XmlElement elementProperty6 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty6.SetAttribute("name", "mute");
                        elementProperty6.SetAttribute("value", pre.Media[i].mute.ToString().ToLower());
                        elementMedia.AppendChild(elementProperty6);
                        XmlElement elementProperty10 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty10.SetAttribute("name", "delay");
                        elementProperty10.SetAttribute("value", pre.Media[i].mDelay + ":" +pre.Media[i].sDelay);
                        elementMedia.AppendChild(elementProperty10);
                        XmlElement elementProperty11 = doc.CreateElement(string.Empty, "property", string.Empty);
                        elementProperty11.SetAttribute("name", "duration");
                        elementProperty11.SetAttribute("value", pre.Media[i].mDuration + ":" + pre.Media[i].sDuration);
                        elementMedia.AppendChild(elementProperty11);
                    }
                    //insere as relações
                    //temos 4 opções de relacionamento para o start,
                    //sendo "OnBegin This Scene" para relacionar com a cena
                    //"OnBegin" para iniciarem juntos
                    //"OnEnd" para iniciar ao termino
                    //"Not Defined" para não iniciar, tambem usado quando a midia for iniciada por interações
                    if (pre.Media[i].rStart.Equals("OnBegin This Scene")) {
                        XmlElement elementRelation1 = doc.CreateElement(string.Empty, "relation", string.Empty);
                        elementRelation1.SetAttribute("id", "Relation-" + id++);
                        if((pre.Media[i].mDelay + pre.Media[i].sDelay) == 0)
                            elementRelation1.SetAttribute("type", "starts");
                        else
                            elementRelation1.SetAttribute("type", "startsDelay");
                        elementScene.AppendChild(elementRelation1);
                        //primary relation
                        XmlElement elementRelationPrimary1 = doc.CreateElement(string.Empty, "primary", string.Empty);
                        elementRelationPrimary1.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementRelation1.AppendChild(elementRelationPrimary1);
                        //secondary relation
                        XmlElement elementRelationSecondary1 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                        elementRelationSecondary1.SetAttribute("component", "scene " + s);
                        elementRelation1.AppendChild(elementRelationSecondary1);
                        elementScene.PrependChild(elementRelation1);
                    }
                    if (pre.Media[i].rStart.Equals("OnBegin")) {
                        XmlElement elementRelation1 = doc.CreateElement(string.Empty, "relation", string.Empty);
                        elementRelation1.SetAttribute("id", "Relation-" + id++);
                        elementRelation1.SetAttribute("type", "starts");
                        elementScene.AppendChild(elementRelation1);
                        XmlElement elementRelationPrimary1 = doc.CreateElement(string.Empty, "primary", string.Empty);
                        elementRelationPrimary1.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementRelation1.AppendChild(elementRelationPrimary1);
                        XmlElement elementRelationSecondary1 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                        elementRelationSecondary1.SetAttribute("component", pre.Media[i].rMediaStart.ToLower());
                        elementRelation1.AppendChild(elementRelationSecondary1);
                        elementScene.PrependChild(elementRelation1);
                    }
                    if (pre.Media[i].rStart.Equals("OnEnd")) {
                        XmlElement elementRelation1 = doc.CreateElement(string.Empty, "relation", string.Empty);
                        elementRelation1.SetAttribute("id", "Relation-" + id++);
                        elementRelation1.SetAttribute("type", "meet");
                        XmlElement elementRelationPrimary1 = doc.CreateElement(string.Empty, "primary", string.Empty);
                        elementRelationPrimary1.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementRelation1.AppendChild(elementRelationPrimary1);
                        XmlElement elementRelationSecondary1 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                        elementRelationSecondary1.SetAttribute("component", pre.Media[i].rMediaStart.ToLower());
                        elementRelation1.AppendChild(elementRelationSecondary1);
                        elementScene.PrependChild(elementRelation1);
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
                        elementRelation2.SetAttribute("id", "Relation-" + id++);
                        elementRelation2.SetAttribute("type", "finishes");
                        elementScene.AppendChild(elementRelation2);
                        //primary relation
                        XmlElement elementRelationPrimary2 = doc.CreateElement(string.Empty, "primary", string.Empty);
                        elementRelationPrimary2.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementRelation2.AppendChild(elementRelationPrimary2);
                        //secondary relation
                        XmlElement elementRelationSecondary2 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                        elementRelationSecondary2.SetAttribute("component", "scene " + s);
                        elementRelation2.AppendChild(elementRelationSecondary2);
                        elementScene.PrependChild(elementRelation2);
                    }
                    if (pre.Media[i].rEnd.Equals("OnBegin")) {
                        XmlElement elementRelation2 = doc.CreateElement(string.Empty, "relation", string.Empty);
                        elementRelation2.SetAttribute("id", "Relation-" + id++);
                        elementRelation2.SetAttribute("type", "metBy");
                        elementScene.AppendChild(elementRelation2);
                        XmlElement elementRelationPrimary2 = doc.CreateElement(string.Empty, "primary", string.Empty);
                        elementRelationPrimary2.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementRelation2.AppendChild(elementRelationPrimary2);
                        XmlElement elementRelationSecondary2 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                        elementRelationSecondary2.SetAttribute("component", pre.Media[i].rMediaStart.ToLower());
                        elementRelation2.AppendChild(elementRelationSecondary2);
                        elementScene.PrependChild(elementRelation2);
                    }
                    if (pre.Media[i].rEnd.Equals("OnEnd")) {
                        XmlElement elementRelation2 = doc.CreateElement(string.Empty, "relation", string.Empty);
                        elementRelation2.SetAttribute("id", "Relation-" + id++);
                        elementRelation2.SetAttribute("type", "finishes");
                        elementScene.AppendChild(elementRelation2);
                        XmlElement elementRelationPrimary2 = doc.CreateElement(string.Empty, "primary", string.Empty);
                        elementRelationPrimary2.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                        elementRelation2.AppendChild(elementRelationPrimary2);
                        XmlElement elementRelationSecondary2 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                        elementRelationSecondary2.SetAttribute("component", pre.Media[i].rMediaEnd.ToLower());
                        elementRelation2.AppendChild(elementRelationSecondary2);
                        elementScene.PrependChild(elementRelation2);
                    }
                    //mudança de cena
                    if (pre.Media[i].interactive) {//se é uma mídia interativa
                        //criação de portas
                        if (pre.Media[i].startTarget) {//se é uma interação de start
                            if (pre.Media[i].interactiveTarget.StartsWith("Scene")) {//é uma cena
                                //relacionamento
                                XmlElement elementRelation3 = doc.CreateElement(string.Empty, "relation", string.Empty);
                                elementRelation3.SetAttribute("id", "Relation-" + id++);
                                elementRelation3.SetAttribute("type", "onSelectionStarts");
                                elementRelation3.SetAttribute("keyCode", "trigger");
                                elementBody.AppendChild(elementRelation3);
                                XmlElement elementRelationPrimary3 = doc.CreateElement(string.Empty, "primary", string.Empty);
                                elementRelationPrimary3.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                                elementRelationPrimary3.SetAttribute("interface", "portChangeScene-" + i);
                                elementRelation3.AppendChild(elementRelationPrimary3);
                                //XmlElement elementRelationSecondary3 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                                //elementRelationSecondary3.SetAttribute("component", pre.Media[i].interactiveTarget.ToLower());
                                //elementRelation3.AppendChild(elementRelationSecondary3);
                                elementBody.PrependChild(elementRelation3);
                                //porta
                                XmlElement elementPort1 = doc.CreateElement(string.Empty, "port", string.Empty);
                                elementPort1.SetAttribute("id", "portChangeScene-" + i);
                                elementPort1.SetAttribute("component", pre.Media[i].interactiveTarget.ToLower());
                                elementBody.AppendChild(elementPort1);
                                elementBody.PrependChild(elementPort1);
                            } else { //é um objeto de midia
                                XmlElement elementRelation3 = doc.CreateElement(string.Empty, "relation", string.Empty);
                                elementRelation3.SetAttribute("id", "Relation-" + i);
                                elementRelation3.SetAttribute("type", "onSelectionStarts");
                                elementRelation3.SetAttribute("keyCode", "trigger");
                                elementScene.AppendChild(elementRelation3);
                                XmlElement elementRelationPrimary3 = doc.CreateElement(string.Empty, "primary", string.Empty);
                                elementRelationPrimary3.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                                elementRelation3.AppendChild(elementRelationPrimary3);
                                XmlElement elementRelationSecondary3 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                                elementRelationSecondary3.SetAttribute("component", pre.Media[i].interactiveTarget.ToLower());
                                elementRelation3.AppendChild(elementRelationSecondary3);
                                elementScene.PrependChild(elementRelation3);
                            }
                        } else { //se é uma interação de end, só pode finalizar objetos de mídia
                            XmlElement elementRelation3 = doc.CreateElement(string.Empty, "relation", string.Empty);
                            elementRelation3.SetAttribute("id", "Relation-" + id++);
                            elementRelation3.SetAttribute("type", "onSelectionEnds");
                            elementRelation3.SetAttribute("keyCode", "trigger");
                            elementScene.AppendChild(elementRelation3);
                            XmlElement elementRelationPrimary3 = doc.CreateElement(string.Empty, "primary", string.Empty);
                            elementRelationPrimary3.SetAttribute("component", pre.Media[i].name.Replace(" ", "").ToLower() + "_s" + s);
                            elementRelation3.AppendChild(elementRelationPrimary3);
                            XmlElement elementRelationSecondary3 = doc.CreateElement(string.Empty, "secondary", string.Empty);
                            elementRelationSecondary3.SetAttribute("component", pre.Media[i].interactiveTarget.ToLower());
                            elementRelation3.AppendChild(elementRelationSecondary3);
                            elementScene.PrependChild(elementRelation3);
                        } 
                    }
                }
                elementScene.PrependChild(doc.CreateComment("Relacionamentos desta cena"));
            }
        }
        elementBody.PrependChild(doc.CreateComment("Portas e Relacionamentos do projeto entre as cenas"));
        doc.Save(folder + "ProjetoMultiSel.xml");
        if (File.Exists(folder + "ProjetoMultiSel.xml"))
            Debug.Log("PROJETO MULTISEL CRIADO!");
        else
            Debug.Log("ERRO AO CRIAR O PROJETO");
    }
}
