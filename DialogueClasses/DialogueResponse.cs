using Godot;
using System;
using System.Collections.Generic;

namespace DiaLogo
{
	public partial class DialogueResponse : DialogueBase
	{
		public string Character { get; set; }
		public string DialogueText { get; set; }

		public List<ResponseLine> ResponseList = [];

		private readonly string sceneUID = "uid://c53s0nnshjh0d";
		public override string SceneUID => sceneUID;
	}
}