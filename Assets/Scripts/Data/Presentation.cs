using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public class Presentation {
    [XmlAttribute("Scene")]
    public string nScene;
    [XmlArray("Medias"), XmlArrayItem("Media")]
    public List<Media> Media = new List<Media>();
}
[System.Serializable]
public class Media {
    [XmlAttribute("type")]
    public string type;
    [XmlAttribute("name")]
    public string name;
    [XmlAttribute("src")]
    public string src;
    [XmlAttribute("px")]
    public float px;
    [XmlAttribute("py")]
    public float py;
    [XmlAttribute("pz")]
    public float pz;
    [XmlAttribute("rStart")]
    public string rStart;
    [XmlAttribute("rMediaStart")]
    public string rMediaStart;
    [XmlAttribute("mStart")]
    public int mStart;
    [XmlAttribute("sStart")]
    public int sStart;
    [XmlAttribute("rEnd")]
    public string rEnd;
    [XmlAttribute("rMediaEnd")]
    public string rMediaEnd;
    [XmlAttribute("mEnd")]
    public int mEnd;
    [XmlAttribute("sEnd")]
    public int sEnd;
    [XmlAttribute("mDuration")]
    public int mDuration;
    [XmlAttribute("sDuration")]
    public int sDuration;
    [XmlAttribute("mDelay")]
    public int mDelay;
    [XmlAttribute("sDelay")]
    public int sDelay;
    [XmlAttribute("scale")]
    public float scale;
    [XmlAttribute("loop")]
    public bool loop;
    [XmlAttribute("mute")]
    public bool mute;
    [XmlAttribute("volume")]
    public string volume;
    [XmlAttribute("interactive")]
    public bool interactive;
    [XmlAttribute("startTarget")]
    public bool startTarget;
    [XmlAttribute("interactiveTarget")]
    public string interactiveTarget;
    [XmlAttribute("interactiveIcon")]
    public bool interactiveIcon;
    [XmlAttribute("intensity")]
    public string intensity;
    [XmlAttribute("textMessage")]
    public string textMessage;
    [XmlAttribute("sceneTarget")]
    public string sceneTarget;
}
