using Godot;
using System;

namespace DiaLogo
{
	[Tool]
	public partial class ActionDialogueItem : BaseItem
	{
		public TextEdit ActionBox;
		public TextEdit Character;
		public TextEdit Dialogue;
		public SpinBox ID;
		public SpinBox NextID;

		Button upButton;
		Button downButton;
		Button delButton;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			upButton = GetNode<Button>("MarginContainer/Panel/VBoxContainer/HBoxContainer/UpButton");
			upButton.Pressed += MoveUp;

			downButton = GetNode<Button>("MarginContainer/Panel/VBoxContainer/HBoxContainer2/DownButton");
			downButton.Pressed += MoveDown;

			delButton = GetNode<Button>("MarginContainer/Panel/VBoxContainer/HBoxContainer/DelButton");
			delButton.Pressed += DeletePressed;

			ActionBox = GetNode<TextEdit>("MarginContainer/Panel/VBoxContainer/HBoxContainer/ActionEdit");
			ActionBox.TextChanged += ChangedAction;

			Character = GetNode<TextEdit>("MarginContainer/Panel/VBoxContainer/HBoxContainer/CharactgerEdit");
			Character.TextChanged += ChangedCharacter;

			Dialogue = GetNode<TextEdit>("MarginContainer/Panel/VBoxContainer/HBoxContainer2/DialogueEdit");
			Dialogue.TextChanged += ChangedDialogue;

			ID = GetNode<SpinBox>("MarginContainer/Panel/VBoxContainer/HBoxContainer/SpinBox");
			ID.ValueChanged += ChangedID;

			NextID = GetNode<SpinBox>("MarginContainer/Panel/VBoxContainer/HBoxContainer2/SpinBox2");
			NextID.ValueChanged += ChangedNextID;
		}

		public void ChangedAction()
		{
			((ActionDialogue)Parent).Action = ActionBox.Text;
		}

		public void ChangedCharacter()
		{
			((ActionDialogue)Parent).Character = Character.Text;
		}

		public void ChangedDialogue()
		{
			((ActionDialogue)Parent).DialogueText = Dialogue.Text;
		}

		public void ChangedID(double id)
		{
			((ActionDialogue)Parent).DialogueID = (float)id;
		}

		public void ChangedNextID(double id)
		{
			((ActionDialogue)Parent).NextID = (float)id;
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
            ActionBox.Text = ((ActionDialogue)dialogueBase).Action;
			Character.Text = ((ActionDialogue)dialogueBase).Character;
			Dialogue.Text = ((ActionDialogue)dialogueBase).DialogueText;
			ID.Value = ((ActionDialogue)dialogueBase).DialogueID; //This sends a signal when updated to set DialogueId to ID.Value, and yes its stupid, I blame the engine
			NextID.Value = ((ActionDialogue)dialogueBase).NextID; //Same as above, I could have it so the objects only update when save is hit
        }

	}
}