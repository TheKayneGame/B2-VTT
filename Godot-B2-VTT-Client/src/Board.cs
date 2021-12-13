using Godot;
using System;

public class Board : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	private PackedScene DebugMenuScene;

	private CanvasLayer UILayer;
	public override void _Ready()
	{
		UILayer = GetNode<CanvasLayer>("UILayer");
		
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("debug_3")){
			UILayer.AddChild(GD.Load<PackedScene>("res://src/Debug/DebugMenu.tscn").Instance());
		}
	}
}
