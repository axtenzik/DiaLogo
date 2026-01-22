using Godot;
using System;

[Tool]
public abstract partial class FlowMenu : Control
{
	public DialogueFlow Parent { get; set; }

	[Signal]
	public delegate void DeleteSignalEventHandler(FlowMenu node);
}