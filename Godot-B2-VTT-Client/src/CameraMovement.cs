using Godot;
using System;

public class CameraMovement : Godot.Camera2D
{
    [Export]
	public int speed = 40;

	public override void _PhysicsProcess(float delta)
	{
		Vector2 newPosition = Position;

		if (Input.IsActionPressed("camera_move_right"))
			newPosition.x += speed * delta;
		if (Input.IsActionPressed("camera_move_left"))
			newPosition.x -= speed * delta;
		if (Input.IsActionPressed("camera_move_up"))
			newPosition.y -= speed * delta;
		if (Input.IsActionPressed("camera_move_down"))
			newPosition.y += speed * delta;

		Position = newPosition;
	}
}
