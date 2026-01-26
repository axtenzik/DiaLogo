using Godot;
using System;

namespace DiaLogo
{
	public partial class ActionDialogue : DialogueBase
	{
		public string Character { get; set; }
		public string Action { get; set; }
		public string DialogueText { get; set; }
		public float NextID { get; set; }

		private readonly string sceneUID = "uid://c1dn0y7i6ahk8";
		public override string SceneUID => sceneUID;
	}
}