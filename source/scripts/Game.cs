using Godot;
using System;

public partial class Game : Node3D{
	const int AMOUNT_CUBES_IN_LINE = 6;
	const float CUBE_SIZE = 1f;
	PackedScene CubeScene = (PackedScene)ResourceLoader.Load("res://scenes/Objects/StageBox.tscn");
	Node3D player;
	
	private static readonly char[] SIDES = {'N','S','W','E'};
	private static Vector3 player_base_position;
	
	private void generate_map(){
		
		foreach(char s in SIDES){
		
			float x=0,y=0,z=0;
			bool first_round=true;
			for(int row_idx=-2;row_idx<AMOUNT_CUBES_IN_LINE-2;row_idx++){
				StageBox sb = (StageBox)CubeScene.Instantiate();
				AddChild(sb);
				
				float offset = (float)AMOUNT_CUBES_IN_LINE*CUBE_SIZE/2;
				float half_cube = CUBE_SIZE/2;
				
				if(first_round) {
					first_round=false;
					player_base_position = new Vector3(row_idx* CUBE_SIZE-half_cube, row_idx* CUBE_SIZE-half_cube,0);
				}
				switch(s){
					case 'N':
						x = row_idx* CUBE_SIZE-half_cube;
						y = offset+CUBE_SIZE/2;
						break;
					case 'S':
						x = (row_idx) * CUBE_SIZE - half_cube;
						y = -offset-CUBE_SIZE/2;
						break;
					case 'W':
						y = (row_idx) * CUBE_SIZE - half_cube;
						x = -offset-CUBE_SIZE/2;
						break;
					case 'E':
						y = (row_idx) * CUBE_SIZE - half_cube;
						x = offset+CUBE_SIZE/2;
						break;
				}
				
				
				sb.GlobalPosition = new Vector3(x,y,z);
			}
		}
	}
	
	public override void _Ready(){
		generate_map();
		player = GetNode<Node3D>("Player");
		player.GlobalPosition = player_base_position;
		GD.Print("HERE");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){
	}
}
