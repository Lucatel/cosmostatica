using Godot;
using System;

public partial class PlayerSC : CharacterBody3D{

	private bool is_init = false;
	private bool is_moving = false;
	private Vector3 destination;
	private Vector3 step_vector;
	private const float SPEED = 12f;
	private int last_moving_cube_idx = -2, current_moving_cube_idx = -1;
	
	public bool is_player_moving(){return is_moving;}
	
	public void init(){
		is_init = true;
	}
	
	private void look_for_key_down(){
		if(Input.IsActionPressed("move_up") && Position.Y < 0){
			destination = Position * new Vector3(1,-1,1);
			is_moving=true;
		}else if(Input.IsActionPressed("move_down") && Position.Y > 0){
			destination = Position * new Vector3(1,-1,1);
			is_moving =true;
		}else if(Input.IsActionPressed("move_left") && Position.X > 0){
			destination = Position * new Vector3(-1,1,1);
			is_moving = true;
		}else if(Input.IsActionPressed("move_right") && Position.X < 0){
			destination = Position * new Vector3(-1,1,1);
			is_moving =true;
		}
		
	}

	public override void _Process(double delta){
		if(!is_init) return;
		//...
	}
	
	public override void _PhysicsProcess(double delta){
		if(!is_init) return;
		
		if(is_moving){
			Vector3 nextStep = Position.MoveToward(destination, SPEED * (float)delta);

			KinematicCollision3D collision = MoveAndCollide(nextStep - Position);
			if(collision != null)
				//GD.Print(collision);
			
			if(Position.X != last_moving_cube_idx){
				
			}
			
			if (GlobalPosition.DistanceTo(destination) < 0.01f){
				is_moving = false;
				GlobalPosition = destination;
			}
		}else{
			look_for_key_down();
		}
	}
}
