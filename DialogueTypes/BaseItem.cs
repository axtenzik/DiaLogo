using Godot;
using System;

namespace DiaLogo
{
	[Tool]
	public abstract partial class BaseItem : Control
	{
		public DialogueBase Parent { get; set; }

		[Signal]
		public delegate void DeleteSignalEventHandler(BaseItem node);

		public virtual void DeletePressed()
		{
			EmitSignal(SignalName.DeleteSignal, this);
		}

		public abstract void SetFields(DialogueBase dialogueBase);
	}
}