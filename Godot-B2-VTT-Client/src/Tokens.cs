using Godot;
using System;

public class Tokens : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    
    private PackedScene _tokenScene = GD.Load<PackedScene>("res://src/Token/Token.tscn");
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("debug_1")){
            Token token = _tokenScene.Instance() as Token;
            AddChild(token);
            token.TargetPos = GetGlobalMousePosition();
            token.Position = GetGlobalMousePosition();
            
        }
    }

}
