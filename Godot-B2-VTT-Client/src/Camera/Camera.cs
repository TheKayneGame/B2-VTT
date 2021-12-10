using Godot;
using System;

public class Camera : Godot.Camera2D
{
	[Export]
	public int speed = 40;
	[Export]
	public float zoomSpeed = 0.2f;

	private bool _dragging;
	private Vector2 oldPos;
	private Vector2 mousePos;
	private Vector2 targetPos;
	private float zoomFactor = 0;
	private float zoomTarget = 1;

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

		if (_dragging)
		{
			targetPos = -(GetViewport().GetMousePosition() - mousePos) * Zoom + oldPos;
		}

		Position = Position.LinearInterpolate(targetPos, 0.2f);

		Zoom = Zoom.LinearInterpolate(Vector2.One * zoomTarget, zoomSpeed); 
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		InputEventMouse eventMouse = @event as InputEventMouse;
		if (@event.IsActionPressed("camera_drag"))
		{
			mousePos = GetViewport().GetMousePosition();
			oldPos = Position;
			_dragging = true;
		}

		if (@event.IsActionReleased("camera_drag"))
		{
			_dragging = false;
		}

		if (@event.IsActionReleased("camera_zoom_in"))
		{
			zoomFactor -= zoomSpeed;
			zoomTarget = Mathf.Pow(2,zoomFactor);
		}

		if (@event.IsActionReleased("camera_zoom_out"))
		{
			zoomFactor += zoomSpeed;
			zoomTarget = Mathf.Pow(2,zoomFactor);
		}
	}
}
