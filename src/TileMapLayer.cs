using Godot;
using System;

public partial class TileMapLayer : Godot.TileMapLayer
{
	enum Tiles
	{
		EMPTY = 0,
		FLOOR,
		BOT_LEFT_WALL,
		BOT_WALL,
		TOP_RIGHT_WALL,
		TOP_LEFT_WALL,
		BOT_RIGHT_WALL,
		RIGHT_WALL,
		LEFT_WALL,
		TOP_WALL,
		FLOOR_BONE_1,
		FLOOR_BONE_2
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Vector2I coords = GetCellAtlasCoords(new Vector2I(1,1));
		GD.Print(coords);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
