using Godot;
using System;

namespace DiaLogo
{
	[Tool]
	public partial class ResponseItem : Control
	{
		[Signal]
		public delegate void DeleteSignalEventHandler(ResponseItem node);

		public ResponseLine Parent { get; set; }
		
		Button delButton;
		public TextEdit Response;
		public SpinBox NextID;

		public override void _Ready()
		{
			delButton = GetNode<Button>("%DelButton");
			delButton.Pressed += DeletePressed;

			NextID = GetNode<SpinBox>("%NextIDEdit");
			NextID.ValueChanged += ChangedNextID;

			Response = GetNode<TextEdit>("%ResponseEdit");
			Response.TextChanged += ChangedResponse;
		}

		public void ChangedNextID(double id)
		{
			Parent.NextID = (float)id;
		}

		public void ChangedResponse()
		{
			Parent.ResponseText = Response.Text;
		}

		public virtual void DeletePressed()
		{
			EmitSignal(SignalName.DeleteSignal, this);
		}
	}
}