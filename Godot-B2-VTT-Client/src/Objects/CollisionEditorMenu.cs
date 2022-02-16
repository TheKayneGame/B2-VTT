using Godot;
using Godot.Collections;

public class CollisionEditorMenu : WindowDialog
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    PackedScene collisionEditorScene;

    [Export]
    NodePath collsionSelectorDropdownPath;

    [Export]
    NodePath testBoxPath;

    TextEdit testBox;

    OptionButton collsionSelectorDropdown;
    CollisionEditor collisionEditor;
    Array<Node> collidables;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        collisionEditor = collisionEditorScene.Instance<CollisionEditor>();
        collsionSelectorDropdown = GetNode<OptionButton>(collsionSelectorDropdownPath);
        testBox = GetNode<TextEdit>(testBoxPath);
        UpdateCollisionDropdown();

        GetParent().AddChild(collisionEditor);
        SetPosition(GetGlobalMousePosition());
        Visible = true;

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void _onCheckButtonToggled(bool state)
    {
        collisionEditor.Active = state;
    }

    public void _onCollisionEditorMenuClose()
    {
        QueueFree();
        collisionEditor.QueueFree();
    }

    public void _onSavePressed()
    {
        GD.Print(JSON.Print(collisionEditor.currentCollision.toDict()));
    }

    public void _onLoadPressed()
    {
        Godot.Collections.Dictionary<string, object> dict = new Godot.Collections.Dictionary<string, object>((Godot.Collections.Dictionary)JSON.Parse(testBox.Text).Result);
        GD.Print(dict);
        collisionEditor.currentCollision.fromDict(dict);
    }

    public void _onColissionSelected(){
        collisionEditor.SetCurrentCollision(collidables[collsionSelectorDropdown.Selected]);
    }

    private void UpdateCollisionDropdown(){
        collidables = new Array<Node>(GetTree().GetNodesInGroup("HasCustomCollision"));

        foreach (var item in collidables)
        {
            collsionSelectorDropdown.AddItem(item.Name);
        }

    }
}


