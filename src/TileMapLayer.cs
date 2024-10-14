using Godot;
using System;

public partial class TileMapLayer : Godot.TileMapLayer
{
    readonly Vector2 TOP_LEFT_CORNER = new Vector2(0, 0);
    readonly Vector2 TOP_WALL = new Vector2(1, 0);
    readonly Vector2 TOP_RIGHT_CORNER = new Vector2(2, 0);
    readonly Vector2 RIGHT_WALL = new Vector2(2, 1);
    readonly Vector2 BOT_RIGHT_CORNER = new Vector2(2, 2);
    readonly Vector2 BOT_WALL = new Vector2(1, 2);
    readonly Vector2 BOT_LEFT_CORNER = new Vector2(0, 2);
    readonly Vector2 LEFT_WALL = new Vector2(0, 1);
    readonly Vector2 FLOOR_EMPTY = new Vector2(1,1);
    readonly Vector2 FLOOR_BONE_1 = new Vector2(0,3);
    readonly Vector2 FLOOR_BONE_2 = new Vector2(1,3);


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Vector2I coords = GetCellAtlasCoords(new Vector2I(1,1));
		GD.Print(coords);
	}

    public void RandomizeFloor()
    {
        // TODO randomize tiles on floor
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
