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
			upButton = GetNode<Button>("%UpButton");
			upButton.Pressed += MoveUp;

			downButton = GetNode<Button>("%DownButton");
			downButton.Pressed += MoveDown;

			delButton = GetNode<Button>("%DelButton");
			delButton.Pressed += DeletePressed;

			character = GetNode<TextEdit>("%CharactgerEdit");
			character.TextChanged += ChangedCharacter;

			dialogue = GetNode<TextEdit>("%DialogueEdit");
			dialogue.TextChanged += ChangedDialogue;

			_ID = GetNode<SpinBox>("%IDBox");
			_ID.ValueChanged += ChangedID;

			nextID = GetNode<SpinBox>("%NextIDBox");
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