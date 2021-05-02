using Godot;
using System;

public class Tokens : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    
    private PackedScene _tokenScene = GD.Load<PackedScene>("res://Prefabs/Token.tscn");
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("debug_1")){
            Area2D token = _tokenScene.Instance() as Area2D;
            AddChild(token);
            token.Position = GetViewport().GetMousePosition();
        }
    }

}
