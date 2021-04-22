using Godot;

public class Player : KinematicBody2D
{
    [Export]
	public int speed = 40;

	private Vector2 velocity = new Vector2();

	private Label NameLabel { get; set; }

	[Puppet]
	public Vector2 PuppetPosition { get; set; }
	[Puppet]
	public Vector2 PuppetVelocity { get; set; }

	public void GetInput()
	{
		velocity = new Vector2();

		if (Input.IsActionPressed("camera_move_right"))
			velocity.x += 1;
		if (Input.IsActionPressed("camera_move_left"))
			velocity.x -= 1;
		if (Input.IsActionPressed("camera_move_up"))
			velocity.y -= 1;
		if (Input.IsActionPressed("camera_move_down"))
			velocity.y += 1;
		
		velocity = velocity.Normalized() * speed;

		Rset(nameof(PuppetPosition), Position);
		Rset(nameof(PuppetVelocity), velocity);
	}

	public override void _PhysicsProcess(float delta)
	{
		if (IsNetworkMaster())
		{
			GetInput();
		}
		else
		{
			Position = PuppetPosition;
			velocity = PuppetVelocity;
		}

		velocity = MoveAndSlide(velocity);

		if (!IsNetworkMaster())
		{
			PuppetPosition = Position;
		}
	}

	public void SetPlayerName(string name)
	{
		NameLabel = (Label)GetNode("Label");

		PuppetPosition = Position;
		PuppetVelocity = velocity;

		NameLabel.Text = name;
	}
}
