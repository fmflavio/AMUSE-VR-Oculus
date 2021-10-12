/// referencia https://gram.gs/gramlog/xml-serialization-and-deserialization-in-unity/
/// //https://answers.unity.com/questions/293400/how-can-i-use-a-list-inside-a-xml-serializer-class.html
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class SerializeOp : MonoBehaviour {
	public static void Serialize(object item, string path) {
		XmlSerializer serializer = new XmlSerializer(item.GetType());
		StreamWriter writer = new StreamWriter(path);
		serializer.Serialize(writer.BaseStream, item);
		writer.Close();
	}
	public static T Deserialize<T>(string path) {
		XmlSerializer serializer = new XmlSerializer(typeof(T));
		StreamReader reader = new StreamReader(path);
		T deserialized = (T)serializer.Deserialize(reader.BaseStream);
		reader.Close();
		return deserialized;
	}
}
