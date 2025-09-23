using Godot;
using System;

public partial class Player : Node3D{
	public override void _Ready(){
		
	}

	public override void _Process(double delta){
		if(Input.IsActionPressed("move_up") && Position.Y < 0){
			Position *= new Vector3(1,-1,1);
		}else if(Input.IsActionPressed("move_down") && Position.Y > 0){
			Position *= new Vector3(1,-1,1);
		}else if(Input.IsActionPressed("move_left") && Position.X > 0){
			Position *= new Vector3(-1,1,1);
		}else if(Input.IsActionPressed("move_right") && Position.X < 0){
			Position *= new Vector3(-1,1,1);
		}
	}
}
