using Godot;
using System;
using System.Collections.Generic;

namespace DiaLogo
{
	public partial class ActionDialogueResponse : DialogueBase
	{
		public string Action { get; set; }
		public string Character { get; set; }
		public string DialogueText { get; set; }

		public List<ResponseLine> ResponseList = [];

		private readonly string sceneUID = "uid://w4ybnyok6ktn";
		public override string SceneUID => sceneUID;
	}
}