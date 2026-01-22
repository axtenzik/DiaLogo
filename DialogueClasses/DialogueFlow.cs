using Godot;
using System;
using System.Xml.Serialization;
//using System.Text.Json.Serialization;

//[JsonDerivedType(typeof(SimpleDialogue), "SimpleDialogue")]
[XmlInclude(typeof(SimpleDialogue))]
public abstract class DialogueFlow
{
	public abstract string SceneUID { get; }

	public float DialogueID { get; set; }
}