using Godot;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Tool]
public partial class DiaLogoMenu : Control
{
	private VBoxContainer dialogueLineVBox;

	private Button addButton;
	private Button saveButton;
	private Button saveAsButton;
	private Button loadButton;
	private OptionButton dialogueOptions;

	private FileDialog fileDialog;

	private List<DialogueFlow> flowList = [];

	private readonly PackedScene _simpleDialogue = GD.Load<PackedScene>("uid://c1dn0y7i6ahk8");
	//private readonly JsonSerializerOptions options = new() { WriteIndented = true };

	public override void _Ready()
	{
		//Evil stuff here, I actually hate it, should probably fix eventually before I break it
		dialogueLineVBox = GetNode<VBoxContainer>("MarginContainer/VBoxContainer/MarginContainer/ScrollContainer/DialogueLineVBox");

		dialogueOptions = GetNode<OptionButton>("MarginContainer/VBoxContainer/TopBar/MarginContainer/Panel/HBoxContainer/OptionButton");

		addButton = GetNode<Button>("MarginContainer/VBoxContainer/TopBar/MarginContainer/Panel/HBoxContainer/AddButton");
		addButton.Pressed += AddDialogue;

		saveButton = GetNode<Button>("MarginContainer/VBoxContainer/TopBar/MarginContainer/Panel/HBoxContainer/SaveButton");
		saveButton.Pressed += SaveDialogue;

		saveAsButton = GetNode<Button>("MarginContainer/VBoxContainer/TopBar/MarginContainer/Panel/HBoxContainer/SaveAsButton");
		saveAsButton.Pressed += SaveDialogueAs;

		loadButton = GetNode<Button>("MarginContainer/VBoxContainer/TopBar/MarginContainer/Panel/HBoxContainer/LoadButton");
		loadButton.Pressed += LoadDialogue;

		fileDialog = GetNode<FileDialog>("FileDialog");
		fileDialog.AddFilter("*.xml", "Choose an xml file");
		fileDialog.FileSelected += DialogueFileSelected;
	}

	private void AddDialogue()
	{
		flowList ??= [];

		string optionSelected = dialogueOptions.Text;

		switch (optionSelected)
		{
			case "SimpleDialogue":
				SimpleDialogue dialogueItem = new();
				flowList.Add(dialogueItem);

				PackedScene SceneToAdd = GD.Load<PackedScene>(dialogueItem.SceneUID);
				FlowMenu instance = (FlowMenu)SceneToAdd.Instantiate();

				instance.Parent = dialogueItem;
				instance.DeleteSignal += DeleteDialogue;

				dialogueLineVBox.AddChild(instance);

				break;
		}
	}

	private void DialogueFileSelected(string path)
	{
		//GD.Print("File Selected: ", path);

		if (fileDialog.FileMode == FileDialog.FileModeEnum.SaveFile)
		{
			Save();
			return;
		}
		else if (fileDialog.FileMode == FileDialog.FileModeEnum.OpenFile)
		{
			Load(path);
		}
	}

	private void DeleteDialogue(FlowMenu flowMenu)
	{
		flowList.Remove(flowMenu.Parent);
		flowMenu.QueueFree();
	}

	private void Load(string path)
	{
		flowList = [];
		foreach (Node child in dialogueLineVBox.GetChildren())
		{
			child.QueueFree();
		}

		using FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
		string content = file.GetAsText();

		if (content != string.Empty && content != "null")
		{
			//flowList = JsonSerializer.Deserialize<List<DialogueFlow>>(content);

			using (var stringReader = new System.IO.StringReader(content))
			{
				var serializer = new XmlSerializer(typeof(DialogueFlow[]));
				DialogueFlow[] dialogueList = serializer.Deserialize(stringReader) as DialogueFlow[];
				flowList = [.. dialogueList];
				//var serializer = new XmlSerializer(typeof(List<DialogueFlow>));
				//flowList = serializer.Deserialize(stringReader) as List<DialogueFlow>;
			}//*/

			flowList ??= [];

			//Add children to Vbox and populate fields
			foreach (DialogueFlow flow in flowList)
			{
				switch (flow)
				{
					case SimpleDialogue t:
						PackedScene SceneToAdd = GD.Load<PackedScene>(flow.SceneUID);
						SimpleDialogueMenu instance = (SimpleDialogueMenu)SceneToAdd.Instantiate();

						instance.Parent = flow;
						instance.DeleteSignal += DeleteDialogue;

						dialogueLineVBox.AddChild(instance);

						instance.Action.Text = ((SimpleDialogue)flow).Action;
						instance.Character.Text = ((SimpleDialogue)flow).Character;
						instance.Dialogue.Text = ((SimpleDialogue)flow).DialogueText;
						instance.ID.Value = ((SimpleDialogue)flow).DialogueID; //This sends a signal when updated to set DialogueId to ID.Value, and yes its stupid, I blame the engine
						instance.NextID.Value = ((SimpleDialogue)flow).NextID; //Same as above, I could have it so the objects only update when save is hit

						break;
				}
			}
		}
	}

	private void LoadDialogue()
	{
		GD.Print("Loading In Testing");

		fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
		fileDialog.Show();
	}

	private void Save()
	{
		//string dialogueString = JsonSerializer.Serialize(flowList, options);
		string dialogueString;
		using (var stringwriter = new System.IO.StringWriter())
		{
			DialogueFlow[] dialogueList = [.. flowList];
			var serializer = new XmlSerializer(typeof(DialogueFlow[]));
			serializer.Serialize(stringwriter, dialogueList);
			//var serializer = new XmlSerializer(typeof(List<DialogueFlow>));
			//serializer.Serialize(stringwriter, flowList);
			dialogueString = stringwriter.ToString();
		}//*/

		using var file = FileAccess.Open(fileDialog.CurrentPath, FileAccess.ModeFlags.Write);
		file.StoreString(dialogueString);
	}

	private void SaveDialogue()
	{
		GD.Print("Saving In Testing");

		//null check CurrentFile
		if (fileDialog.CurrentPath == "res://")
		{
			GD.Print("No File Selected, Opening Save Dialog");
			fileDialog.FileMode = FileDialog.FileModeEnum.SaveFile;
			fileDialog.Show();
			return;
		}

		Save();
	}

	private void SaveDialogueAs()
	{
		GD.Print("Save As In Testing");

		fileDialog.FileMode = FileDialog.FileModeEnum.SaveFile;
		fileDialog.Show();
	}
}