using Godot;
using System;

namespace DiaLogo
{
	[Tool]
	public partial class DialogueItem : BaseItem
	{
		public TextEdit character;
		public TextEdit dialogue;
		public SpinBox _ID;
		public SpinBox nextID;

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

			character = GetNode<TextEdit>("MarginContainer/Panel/VBoxContainer/HBoxContainer/CharactgerEdit");
			character.TextChanged += ChangedCharacter;

			dialogue = GetNode<TextEdit>("MarginContainer/Panel/VBoxContainer/HBoxContainer2/DialogueEdit");
			dialogue.TextChanged += ChangedDialogue;

			_ID = GetNode<SpinBox>("MarginContainer/Panel/VBoxContainer/HBoxContainer/SpinBox");
			_ID.ValueChanged += ChangedID;

			nextID = GetNode<SpinBox>("MarginContainer/Panel/VBoxContainer/HBoxContainer2/SpinBox2");
			nextID.ValueChanged += ChangednextID;
		}

		public void ChangedCharacter()
		{
			((DialogueLine)Parent).Character = character.Text;
		}

		public void ChangedDialogue()
		{
			((DialogueLine)Parent).DialogueText = dialogue.Text;
		}

		//Connect through editor, float is fine, through code? Nope has to be double
		//Why Godot why?
		//Whats stupid is only ints can be selected, yep you heard that right, to save an int godot wants a double
		public void ChangedID(double id)
		{
			((DialogueLine)Parent).DialogueID = (float)id;
		}

		public void ChangednextID(double id)
		{
			((DialogueLine)Parent).NextID = (float)id;
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
			character.Text = ((DialogueLine)dialogueBase).Character;
			dialogue.Text = ((DialogueLine)dialogueBase).DialogueText;
			_ID.Value = ((DialogueLine)dialogueBase).DialogueID;
			nextID.Value = ((DialogueLine)dialogueBase).NextID;
		}

	}
}