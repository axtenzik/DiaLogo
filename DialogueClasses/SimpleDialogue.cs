using Godot;
using System;

public partial class SimpleDialogue : DialogueFlow
{
	//public float DialogueID { get; set; }
	public string Character { get; set; }
	public string Action { get; set; }
	public string DialogueText { get; set; }
	public float NextID { get; set; }

	//public readonly PackedScene dialogueItem = GD.Load<PackedScene>("uid://c1dn0y7i6ahk8");
	private readonly string sceneUID = "uid://c1dn0y7i6ahk8";
	public override string SceneUID => sceneUID;

	/*public SimpleDialogue()
	{
		SceneUID = sceneUID;
	}*/
}