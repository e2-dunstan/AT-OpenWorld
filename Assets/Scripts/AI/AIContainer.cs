using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("AICollection")]
public class AIContainer
{
    [XmlArray("Objects")]
    [XmlArrayItem("Object")]
    public List<AI> ai;

    public static ObjectContainer Load(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ObjectContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as ObjectContainer;
        }
    }
    //Loads the xml directly from the given string
    public static ObjectContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(ObjectContainer));
        return serializer.Deserialize(new StringReader(text)) as ObjectContainer;
    }
    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(ObjectContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

}
