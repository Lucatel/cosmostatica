using Godot;
using System;

public partial class Wall : CharacterBody3D{
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
	
	public override void _PhysicsProcess(double delta){
		if(is_init){

			Velocity = new Vector3(0,0,1)*movement_speed;

			KinematicCollision3D collision = MoveAndCollide(Velocity * (float)delta);
			if (collision != null)
			{
				if (collision.GetCollider() is PlayerSC)
				{
					game.game_over();
				}
			}
			
			if (Position.Z > end_zone)
			{
				game.inc_score(SCORE);
				QueueFree();
			}
		}
	}
}
