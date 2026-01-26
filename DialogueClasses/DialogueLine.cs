using Godot;
using System;

namespace DiaLogo
{
	public partial class DialogueLine : DialogueBase
	{
		public string Character { get; set; }
		public string DialogueText { get; set; }
		public float NextID { get; set; }
		
		private readonly string sceneUID = "uid://kurkd2nqhaaj";
		public override string SceneUID => sceneUID;
	}
}