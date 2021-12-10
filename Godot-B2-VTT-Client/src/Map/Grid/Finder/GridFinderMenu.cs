using Godot;
using System;

public class GridFinderMenu : WindowDialog
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";


    [Export]
    private NodePath offsetSpinXPath;

    [Export]
    private NodePath offsetSpinYPath;

    [Export]
    private NodePath sizeSpinXPath;

    [Export]
    private NodePath sizeSpinYPath;

    [Export]
    private PackedScene gridFinderScene;

    private GridFinder gridFinder;

    private SpinBox originSpinX;

    private SpinBox originSpinY;

    private SpinBox sizeSpinX;

    private SpinBox sizeSpinY;

    private Vector2 gridSize;
    private Vector2 gridOrigin;

    private Map map;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetPosition(GetGlobalMousePosition());
        Visible = true;
        gridFinder = gridFinderScene.Instance() as GridFinder;
        map = GetNode<Map>("/root/Board/Map");

        map.AddChild(gridFinder);

        originSpinX = GetNode<SpinBox>(offsetSpinXPath);
        originSpinY = GetNode<SpinBox>(offsetSpinYPath);

        sizeSpinX = GetNode<SpinBox>(sizeSpinXPath);
        sizeSpinY = GetNode<SpinBox>(sizeSpinYPath);

        gridFinder.Connect("OnGridOriginSet", this, nameof(OnGridOriginSet));
        gridFinder.Connect("OnGridSizeSet", this, nameof(OnGridSizeSet));

    }
    public void _OnGridShowToggleToggled(bool state)
    {
        if (state)
        {
            gridFinder.ActivateSizeFinder();
        }
        else
        {
            gridFinder.DeactivateSizeFinder();
        }
    }

    public void _OnOriginShowToggleToggled(bool state)
    {
        if (state)
        {
            gridFinder.ActivateOriginFinder();
        }
        else
        {
            gridFinder.DeactivateOriginFinder();
        }
    }

    public void OnGridOriginSet(Vector2 offset)
    {
        originSpinX.Value = (double)offset.x;
        originSpinY.Value = (double)offset.y;
        gridOrigin = offset;
    }

    public void OnGridSizeSet(Vector2 size)
    {
        sizeSpinX.Value = size.x;
        sizeSpinY.Value = size.y;
        gridSize = size;
    }

    public void _OnXSizeValueChanged(float x)
    {
        gridFinder.GridSize = new Vector2(x, gridFinder.GridSize.y);
    }

    public void _OnYSizeValueChanged(float y)
    {
        gridFinder.GridSize = new Vector2(gridFinder.GridSize.x, y);
    }

    public void _OnXOriginValueChanged(float x)
    {
        gridFinder.GridOrigin = new Vector2(x, gridFinder.GridOrigin.y);        
    }

    public void _OnYOriginValueChanged(float y)
    {
        gridFinder.GridOrigin = new Vector2(gridFinder.GridOrigin.x, y);
    }


    public void _OnApplyGridButtonPressed(){
        map.grid = gridFinder.GetGrid();
    }


    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}


