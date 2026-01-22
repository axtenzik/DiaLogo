using Godot;
using System;

[Tool]
public partial class SimpleDialogueMenu : FlowMenu
{
	public string TestParam;

	public TextEdit Action;
	public TextEdit Character;
	public TextEdit Dialogue;
	public SpinBox ID;
	public SpinBox NextID;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Action = GetNode<TextEdit>("MarginContainer/Panel/VBoxContainer/HBoxContainer/ActionEdit");
		Character = GetNode<TextEdit>("MarginContainer/Panel/VBoxContainer/HBoxContainer/CharactgerEdit");
		Dialogue = GetNode<TextEdit>("MarginContainer/Panel/VBoxContainer/HBoxContainer2/DialogueEdit");
		ID = GetNode<SpinBox>("MarginContainer/Panel/VBoxContainer/HBoxContainer/SpinBox");
		NextID = GetNode<SpinBox>("MarginContainer/Panel/VBoxContainer/HBoxContainer2/SpinBox2");
	}

	public void DeletePressed()
	{
		EmitSignal(SignalName.DeleteSignal, this);
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

	public void ChangedAction()
	{
		((SimpleDialogue)Parent).Action = Action.Text;
	}

	public void ChangedCharacter()
	{
		((SimpleDialogue)Parent).Character = Character.Text;
	}

	public void ChangedDialogue()
	{
		((SimpleDialogue)Parent).DialogueText = Dialogue.Text;
	}

	public void ChangedID(float id)
	{
		((SimpleDialogue)Parent).DialogueID = id;
	}

	public void ChangedNextID(float id)
	{
		((SimpleDialogue)Parent).NextID = id;
	}
}
