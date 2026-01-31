using Godot;
using System;
using System.Xml.Serialization;
//using System.Text.Json.Serialization;

namespace DiaLogo
{
	//[JsonDerivedType(typeof(SimpleDialogue), "SimpleDialogue")]
	[XmlInclude(typeof(ActionDialogue))]
	[XmlInclude(typeof(ActionDialogueResponse))]
	[XmlInclude(typeof(DialogueLine))]
	public abstract class DialogueBase
	{
		public abstract string SceneUID { get; }

		public float DialogueID { get; set; }
	}
}