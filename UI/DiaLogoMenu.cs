using Godot;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DiaLogo
{
	[Tool]
	public partial class DiaLogoMenu : Control
	{
		[Export] Theme ModernTheme { get; set; }
		[Export] Theme ClassicTheme { get; set; 
		}
		private VBoxContainer dialogueLineVBox;

		private Button addButton;
		private Button saveButton;
		private Button saveAsButton;
		private Button loadButton;
		private OptionButton dialogueOptions;

		private FileDialog fileDialog;

		private List<DialogueBase> dialogueList = [];

		//private readonly JsonSerializerOptions options = new() { WriteIndented = true };

		public override void _Ready()
		{
			EditorSettings editorSetting = EditorInterface.Singleton.GetEditorSettings();
			string currentTheme = (string)editorSetting.GetSetting("interface/theme/style");
			
			if (currentTheme == "Modern")
			{
				Theme = ModernTheme;
			}
			else if (currentTheme == "Classic")
			{
				Theme = ClassicTheme;
			}

			//Evil stuff here, I actually hate it, should probably fix eventually before I break it
			dialogueLineVBox = GetNode<VBoxContainer>("MarginContainer/VBoxContainer/MarginContainer/ScrollContainer/DialogueLineVBox");

			dialogueOptions = GetNode<OptionButton>("MarginContainer/VBoxContainer/TopBar/MarginContainer/PanelContainer/HBoxContainer/OptionButton");

			addButton = GetNode<Button>("MarginContainer/VBoxContainer/TopBar/MarginContainer/PanelContainer/HBoxContainer/AddButton");
			addButton.Pressed += AddDialogue;

			saveButton = GetNode<Button>("MarginContainer/VBoxContainer/TopBar/MarginContainer/PanelContainer/HBoxContainer/SaveButton");
			saveButton.Pressed += SaveDialogue;

			saveAsButton = GetNode<Button>("MarginContainer/VBoxContainer/TopBar/MarginContainer/PanelContainer/HBoxContainer/SaveAsButton");
			saveAsButton.Pressed += SaveDialogueAs;

			loadButton = GetNode<Button>("MarginContainer/VBoxContainer/TopBar/MarginContainer/PanelContainer/HBoxContainer/LoadButton");
			loadButton.Pressed += LoadDialogue;

			fileDialog = GetNode<FileDialog>("FileDialog");
			fileDialog.ClearFilters(); //I somehow added the same filter twice once so I'm clearing just in case
			fileDialog.AddFilter("*.xml", "Choose an xml file");
			fileDialog.FileSelected += DialogueFileSelected;
		}

		private void AddDialogue()
		{
			dialogueList ??= [];

			string optionSelected = dialogueOptions.Text;

			DialogueBase dialogueObject = null; //Need to initialise, so make null for now
			switch (optionSelected)
			{
				case "Action Dialogue":
					dialogueObject = new ActionDialogue();
					break;
				case "Action Dialogue with Responses":
					dialogueObject = new ActionDialogueResponse();
					break;
				case "Dialogue Line":
					dialogueObject = new DialogueLine();
					break;
			}

			if (dialogueObject == null)
			{
				//Should never run if I make things correctly, but can never hurt to have a check
				GD.PrintErr("Failed to create Dialogue Item");
				return;
			}

			dialogueList.Add(dialogueObject);

			PackedScene SceneToAdd = GD.Load<PackedScene>(dialogueObject.SceneUID);
			BaseItem instance = (BaseItem)SceneToAdd.Instantiate();

			instance.Parent = dialogueObject;
			instance.DeleteSignal += DeleteDialogue;

			dialogueLineVBox.AddChild(instance);
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

		private void DeleteDialogue(BaseItem baseItem)
		{
			dialogueList.Remove(baseItem.Parent);
			baseItem.QueueFree();
		}

		private void Load(string path)
		{
			dialogueList = [];
			foreach (Node child in dialogueLineVBox.GetChildren())
			{
				child.QueueFree();
			}

			using FileAccess file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
			string content = file.GetAsText();

			if (content == string.Empty)
			{
				return;
			}

			if (content == "null") //Might not need this one, Was included when saving as JSON
			{
				return;
			}

			//flowList = JsonSerializer.Deserialize<List<DialogueFlow>>(content);

			using (var stringReader = new System.IO.StringReader(content))
			{
				var serializer = new XmlSerializer(typeof(DialogueBase[]));
				DialogueBase[] dialogueArray = serializer.Deserialize(stringReader) as DialogueBase[];
				dialogueList = [.. dialogueArray];
				//var serializer = new XmlSerializer(typeof(List<DialogueFlow>));
				//flowList = serializer.Deserialize(stringReader) as List<DialogueFlow>;
			}//*/

			dialogueList ??= [];

			//Add children to Vbox and populate fields
			foreach (DialogueBase dialogueBase in dialogueList)
			{
				PackedScene SceneToAdd = null;
				BaseItem instance = null;

				switch (dialogueBase)
				{
					case ActionDialogue ad:
					case ActionDialogueResponse adr:
					case DialogueLine od:
						SceneToAdd = GD.Load<PackedScene>(dialogueBase.SceneUID);
						instance = (BaseItem)SceneToAdd.Instantiate();
						break;
				}//*/

				instance.Parent = dialogueBase;
				instance.DeleteSignal += DeleteDialogue;

				dialogueLineVBox.AddChild(instance);
				instance.SetFields(dialogueBase);
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
				DialogueBase[] dialogueArray = [.. dialogueList];
				var serializer = new XmlSerializer(typeof(DialogueBase[]));
				serializer.Serialize(stringwriter, dialogueArray);
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
}