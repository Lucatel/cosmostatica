using Godot;
using System;

public partial class Wall : Node3D{
	private Game game;
	private float end_zone;
	private float movement_speed;
	private bool is_init = false;
	private const int SCORE = 4;
	public void init(Vector3 position, float z_zone, float speed, Game parent){
		end_zone = z_zone;
		game = parent;
		movement_speed = speed;
		Position = position;
		is_init = true;
	}
	
	public override void _Process(double delta){
		if(is_init){
			Position = new Vector3(Position.X,Position.Y, Position.Z + (movement_speed * (float)delta));
			if(Position.Z > end_zone){
				game.inc_score(SCORE);
				QueueFree();
			}
		}
	}
}
