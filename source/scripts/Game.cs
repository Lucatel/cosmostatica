using Godot;
using System;
using Godot.Collections;

public partial class Game : Node3D{
	const int AMOUNT_CUBES_IN_LINE = 7;
	const int DEPTH_BOX = 20;
	const float CUBE_SIZE = 1f;
	Label score;
	private int score_value = 0;
	PackedScene CubeScene = (PackedScene)ResourceLoader.Load("res://scenes/Objects/StageBox.tscn");
	PlayerSC player;
	Control gameOver;
	
	private SpawnManager spawner;

	private StageBox[] boxes_near_player = new StageBox[ AMOUNT_CUBES_IN_LINE * 4];
	private static int box_near_player_by_idx = -1;

	private static readonly char[] SIDES = { 'N', 'S', 'W', 'E' };
	private static Vector3 player_base_position;
	private static Vector3 enemy_base_position;
	private static float z_spawn_position;
	

	private void generate_map(){
		float offset = (float)AMOUNT_CUBES_IN_LINE * CUBE_SIZE / 2f;
		float half_cube = CUBE_SIZE / 2;
		int player_map_idx = 0;
		player_base_position = new Vector3((float)(AMOUNT_CUBES_IN_LINE - 1) * (-half_cube), offset - half_cube, CUBE_SIZE * (-3));

		float x = 0, y = 0, z = 0;
		for (int i = 0; i < DEPTH_BOX; i++){
			z = i * (-CUBE_SIZE);
			foreach (char s in SIDES){
				for (int row_idx = 0; row_idx < AMOUNT_CUBES_IN_LINE; row_idx++){
					StageBox sb = (StageBox)CubeScene.Instantiate();
					AddChild(sb);
					
					if(player_base_position.Z * -1 == i){
						boxes_near_player[player_map_idx++] = sb;
					}
					
					switch (s){
						case 'N':
							x = row_idx * CUBE_SIZE + (float)(AMOUNT_CUBES_IN_LINE - 1) * (-half_cube);
							y = offset + half_cube;
							break;
						case 'S':
							x = row_idx * CUBE_SIZE + (float)(AMOUNT_CUBES_IN_LINE - 1) * (-half_cube);
							y = -offset - half_cube;
							break;
						case 'W':
							y = row_idx * CUBE_SIZE + (float)(AMOUNT_CUBES_IN_LINE - 1) * (-half_cube);
							x = -offset - half_cube;
							break;
						case 'E':
							y = row_idx * CUBE_SIZE + (float)(AMOUNT_CUBES_IN_LINE - 1) * (-half_cube);
							x = offset + half_cube;
							break;
					}


					sb.GlobalPosition = new Vector3(x, y, z);
				}
			}
		}
	}

	public override void _Ready(){

		generate_map();
		z_spawn_position = (-DEPTH_BOX) * CUBE_SIZE;
		enemy_base_position = new Vector3(player_base_position.X, player_base_position.Y, z_spawn_position);
		

		score = GetNode<Label>("Score");
		player = GetNode<PlayerSC>("Player");
		gameOver = GetNode<GameOver>("GameOver");
		player.init();
		player.GlobalPosition = player_base_position;
		spawner = new SpawnManager();		
		spawner.init(this, enemy_base_position);
	}

	public override void _Process(double delta){
		update_env_colors();
		spawner.update(delta, score_value);
	}

	public void inc_score(int v){
		score_value += v;
		score.Text = " " + score_value;
	}

	

	public void game_over(){
		GD.Print("Game Over");
		Engine.TimeScale = 0;
		gameOver.Visible = true;
	}
	
	public void update_env_colors(){
		if(player.is_player_moving()){
			float nearest_position=-1f;
			int current_box_near_player_by_idx=-1;
			for(int i=0 ; i<boxes_near_player.Length ; i++){
				float current_distance = player.GlobalPosition.DistanceTo(boxes_near_player[i].GlobalPosition);
				if(nearest_position==-1f || current_distance < nearest_position){
					nearest_position = current_distance;
					current_box_near_player_by_idx = i;
				}
			}
			if(current_box_near_player_by_idx != box_near_player_by_idx){
				boxes_near_player[current_box_near_player_by_idx].start_glow();
				box_near_player_by_idx = current_box_near_player_by_idx;
			}
		}
	}
}
