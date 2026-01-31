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

			ID = GetNode<SpinBox>("%IDBox");
			ID.ValueChanged += ChangedID;

			NextID = GetNode<SpinBox>("%NextIDBox");
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