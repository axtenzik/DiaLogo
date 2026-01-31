using Godot;
using System;

namespace DiaLogo
{
	[Tool]
	public partial class DialogueResponseItem : BaseItem
	{
		public TextEdit Character;
		public TextEdit Dialogue;
		public SpinBox ID;

		VBoxContainer responsesVBox;

		Button addResponseButton;
		Button upButton;
		Button downButton;
		Button delButton;

		string responseUID = "uid://chwqjs7rp4jjj";

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

			Character = GetNode<TextEdit>("%CharactgerEdit");
			Character.TextChanged += ChangedCharacter;

			Dialogue = GetNode<TextEdit>("%DialogueEdit");
			Dialogue.TextChanged += ChangedDialogue;

			ID = GetNode<SpinBox>("%IDEdit");
			ID.ValueChanged += ChangedID;
		}

		private void AddResponse()
		{
			((DialogueResponse)Parent).ResponseList ??= [];

			ResponseLine responseObject = new();
			((DialogueResponse)Parent).ResponseList.Add(responseObject);

			PackedScene SceneToAdd = GD.Load<PackedScene>(responseUID);
			ResponseItem instance = (ResponseItem)SceneToAdd.Instantiate();

			instance.Parent = responseObject;
			instance.DeleteSignal += DeleteResponse;

			responsesVBox.AddChild(instance);
		}

		private void DeleteResponse(ResponseItem responseItem)
		{
			((DialogueResponse)Parent).ResponseList.Remove(responseItem.Parent);
			responseItem.QueueFree();
		}

		public void ChangedCharacter()
		{
			((DialogueResponse)Parent).Character = Character.Text;
		}

		public void ChangedDialogue()
		{
			((DialogueResponse)Parent).DialogueText = Dialogue.Text;
		}

		public void ChangedID(double id)
		{
			((DialogueResponse)Parent).DialogueID = (float)id;
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
			Character.Text = ((DialogueResponse)dialogueBase).Character;
			Dialogue.Text = ((DialogueResponse)dialogueBase).DialogueText;
			ID.Value = ((DialogueResponse)dialogueBase).DialogueID;

			PackedScene SceneToAdd = GD.Load<PackedScene>(responseUID);
			foreach (ResponseLine responseLine in ((DialogueResponse)dialogueBase).ResponseList)
			{
				ResponseItem instance = (ResponseItem)SceneToAdd.Instantiate();
				responsesVBox.AddChild(instance);

				instance.Parent = responseLine;
				instance.DeleteSignal += DeleteResponse;

				instance.NextID.Value = responseLine.NextID;
				instance.Response.Text = responseLine.ResponseText;
			}
		}
	}
}