using Godot;
using System;

public class Map : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	[Export]
	private NodePath texturePath;
	
	[Export]
	private NodePath colliderPath;
	public Grid grid;

	private Sprite mapTexture;

	private CollisionShape2D collider;

	public Vector2 CentreOffset {get; private set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mapTexture = GetNode<Sprite>(texturePath);
		collider = GetNode<CollisionShape2D>(colliderPath);

		Image image = new Image();
		ImageTexture texture = new ImageTexture();

		image.Load("E:\\Projects\\Programming\\B2-VTT\\Godot-B2-VTT-Client\\bighaus.jpg");
		
		texture.CreateFromImage(image,0);
		mapTexture.Texture = texture;
		(collider.Shape as  RectangleShape2D).Extents = texture.GetSize() /2f;;
		grid = new Grid(new Vector2(64,64),new Vector2(0,0));
	}

	public Vector2 GetClosestGridPosition(Vector2 pos)
	{
		return (pos + grid.CellCentre).Snapped(grid.Size) - grid.CellCentre;
	}




	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
