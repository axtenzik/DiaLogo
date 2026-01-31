using Godot;
using System;
using System.Collections.Generic;

namespace DiaLogo
{
	[Tool]
	public partial class ActionDialogueResponseItem : BaseItem
	{
		public TextEdit ActionBox;
		public TextEdit Character;
		public TextEdit Dialogue;
		public SpinBox ID;

		VBoxContainer responsesVBox;

		Button addResponseButton;
		Button upButton;
		Button downButton;
		Button delButton;

		string responseUID = "uid://chwqjs7rp4jjj";

		//List<ResponseLine> responseList = [];

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			responsesVBox = GetNode<VBoxContainer>("%ResponsesVBox");

			addResponseButton = GetNode<Button>("%AddResponseButton");
			addResponseButton.Pressed += AddResponse;

			upButton = GetNode<Button>("%UpButton");
			upButton.Pressed += MoveUp;

			downButton = GetNode<Button>("%DownButton");
			downButton.Pressed += MoveDown;

			delButton = GetNode<Button>("%DelButton");
			delButton.Pressed += DeletePressed;

			ActionBox = GetNode<TextEdit>("%ActionEdit");
			ActionBox.TextChanged += ChangedAction;

			Character = GetNode<TextEdit>("%CharactgerEdit");
			Character.TextChanged += ChangedCharacter;

			Dialogue = GetNode<TextEdit>("%DialogueEdit");
			Dialogue.TextChanged += ChangedDialogue;

			ID = GetNode<SpinBox>("%IDEdit");
			ID.ValueChanged += ChangedID;
		}

		private void AddResponse()
		{
			((ActionDialogueResponse)Parent).ResponseList ??= [];

			ResponseLine responseObject = new();
			((ActionDialogueResponse)Parent).ResponseList.Add(responseObject);

			PackedScene SceneToAdd = GD.Load<PackedScene>(responseUID);
			ResponseItem instance = (ResponseItem)SceneToAdd.Instantiate();

			instance.Parent = responseObject;
			instance.DeleteSignal += DeleteResponse;

			responsesVBox.AddChild(instance);
		}

		private void DeleteResponse(ResponseItem responseItem)
		{
			((ActionDialogueResponse)Parent).ResponseList.Remove(responseItem.Parent);
			responseItem.QueueFree();
		}

		public void ChangedAction()
		{
			((ActionDialogueResponse)Parent).Action = ActionBox.Text;
		}

		public void ChangedCharacter()
		{
			((ActionDialogueResponse)Parent).Character = Character.Text;
		}

		public void ChangedDialogue()
		{
			((ActionDialogueResponse)Parent).DialogueText = Dialogue.Text;
		}

		public void ChangedID(double id)
		{
			((ActionDialogueResponse)Parent).DialogueID = (float)id;
		}

		public void MoveUp()
		{
			if (GetIndex() > 0)
			{
				Node parent = GetParent();
				parent.MoveChild(this, GetIndex() - 1);
			}
		}

		public void MoveDown()
		{
			Node parent = GetParent();
			if (GetIndex() < parent.GetChildCount() - 1)
			{
				parent.MoveChild(this, GetIndex() + 1);
			}
		}

		public override void SetFields(DialogueBase dialogueBase)
		{
			ActionBox.Text = ((ActionDialogueResponse)dialogueBase).Action;
			Character.Text = ((ActionDialogueResponse)dialogueBase).Character;
			Dialogue.Text = ((ActionDialogueResponse)dialogueBase).DialogueText;
			ID.Value = ((ActionDialogueResponse)dialogueBase).DialogueID;
			//responseList = ((ActionDialogueResponse)dialogueBase).ResponseList;

			PackedScene SceneToAdd = GD.Load<PackedScene>(responseUID);
			foreach (ResponseLine rl in ((ActionDialogueResponse)dialogueBase).ResponseList)
			{
				ResponseItem instance = (ResponseItem)SceneToAdd.Instantiate();
				responsesVBox.AddChild(instance);

				instance.Parent = rl;
				instance.DeleteSignal += DeleteResponse;

				instance.NextID.Value = rl.NextID;
				instance.Response.Text = rl.ResponseText;
			}
		}
	}
}