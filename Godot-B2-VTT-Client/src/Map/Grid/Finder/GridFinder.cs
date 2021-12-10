using Godot;
using System;

public class GridFinder : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    [Export]
    private NodePath primaryPointerPath;
    [Export]
    private NodePath secondaryPointerPath;
    [Export]
    private NodePath originPointerPath;

    public Pointer primaryPointer;
    public Pointer secondaryPointer;
    public Pointer originPointer;

    private Vector2 primaryPosition;
    private Vector2 secondaryPosition;
    private Vector2 originPosition;
    public Vector2 gridOrigin;
    public Vector2 GridOrigin
    {
        get => gridOrigin;

        set
        {
            originPointer.Position = value;
            gridOrigin = value;
        }
    }
    public Vector2 GridSize { get; set; }


    private bool gridFinderActive = false;
    private bool originFinderActive = false;

    [Signal]
    public delegate void OnGridSizeSet(Vector2 size);

    [Signal]
    public delegate void OnGridOriginSet(Vector2 offset);

    public override void _Ready()
    {
        primaryPointer = GetNode<Pointer>(primaryPointerPath);
        secondaryPointer = GetNode<Pointer>(secondaryPointerPath);
        originPointer = GetNode<Pointer>(originPointerPath);
    }

    public override void _UnhandledInput(InputEvent @event)
    {

        Vector2 mousePos = GetGlobalMousePosition();
        if (@event.IsActionPressed("token_interact_primary"))
        {

            if (gridFinderActive)
            {
                if (!primaryPointer.Active)
                {
                    primaryPointer.Spawn();
                }
                else if (!secondaryPointer.Active)
                {
                    secondaryPointer.Spawn();
                    secondaryPointer.Position = mousePos;
                }
            }

            if (originFinderActive && !originPointer.IsSet)
            {
                originPointer.Spawn();
                originPointer.Position = mousePos;
            }
        }


    }

    public void ActivateOriginFinder()
    {
        originPointer.Activate();
        originFinderActive = true;

    }

    public void DeactivateOriginFinder()
    {
        originPointer.Deactivate();
        originFinderActive = false;
    }

    public void ActivateSizeFinder()
    {
        primaryPointer.Activate();
        secondaryPointer.Activate();
        gridFinderActive = true;
    }

    public void DeactivateSizeFinder()
    {
        primaryPointer.Deactivate();
        secondaryPointer.Deactivate();
        gridFinderActive = false;
    }

    public void _OnPrimaryPointerMoved(Vector2 pos)
    {
        primaryPosition = pos;
        UpdateGridCalculation();
    }
    public void _OnSecondaryPointerMoved(Vector2 pos)
    {
        secondaryPosition = pos;
        UpdateGridCalculation();
    }

    public void _OnOriginPointerMoved(Vector2 pos)
    {
        originPosition = pos;
        UpdateGridCalculation();
    }

    public Grid GetGrid(){
        return new Grid(GridSize, GridOrigin);
    }



    private void UpdateGridCalculation()
    {
        Vector2 tempSize = primaryPosition - secondaryPosition;
        tempSize = tempSize.Abs();
        float longestAxis = Mathf.Max(tempSize.x, tempSize.y);
        GridSize = new Vector2(longestAxis, longestAxis);
        GridOrigin = originPosition;

        EmitSignal(nameof(OnGridSizeSet), GridSize);
        EmitSignal(nameof(OnGridOriginSet), GridOrigin);
    }




}




