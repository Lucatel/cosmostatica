using Godot;
using System;
using Godot.Collections;

public partial class Game : Node3D
{
	const int AMOUNT_CUBES_IN_LINE = 7;
	const int DEPTH_BOX = 15;
	const float CUBE_SIZE = 1f;
	Label score;
	int score_value = 0;
	PackedScene CubeScene = (PackedScene)ResourceLoader.Load("res://scenes/Objects/StageBox.tscn");
	PackedScene WallScene = (PackedScene)ResourceLoader.Load("res://scenes/Objects/Wall.tscn");
	PlayerSC player;
	Control gameOver;

	private StageBox[,,] map = new StageBox[4, AMOUNT_CUBES_IN_LINE, DEPTH_BOX];

	private static readonly char[] SIDES = { 'N', 'S', 'W', 'E' };
	private static Vector3 player_base_position;
	private static Vector3 enemy_base_position;
	private static float z_spawn_position;
	private static RandomNumberGenerator rng = new RandomNumberGenerator();

	private void generate_map()
	{
		float offset = (float)AMOUNT_CUBES_IN_LINE * CUBE_SIZE / 2f;
		float half_cube = CUBE_SIZE / 2;
		player_base_position = new Vector3((float)(AMOUNT_CUBES_IN_LINE - 1) * (-half_cube), offset - half_cube, CUBE_SIZE * (-3));

		float x = 0, y = 0, z = 0;
		for (int i = 0; i < DEPTH_BOX; i++)
		{
			z = i * (-CUBE_SIZE);
			foreach (char s in SIDES)
			{
				for (int row_idx = 0; row_idx < AMOUNT_CUBES_IN_LINE; row_idx++)
				{
					StageBox sb = (StageBox)CubeScene.Instantiate();
					AddChild(sb);

					switch (s)
					{
						case 'N':
							map[0, row_idx, i] = sb;
							x = row_idx * CUBE_SIZE + (float)(AMOUNT_CUBES_IN_LINE - 1) * (-half_cube);
							y = offset + half_cube;
							break;
						case 'S':
							map[1, row_idx, i] = sb;
							x = row_idx * CUBE_SIZE + (float)(AMOUNT_CUBES_IN_LINE - 1) * (-half_cube);
							y = -offset - half_cube;
							break;
						case 'W':
							map[2, row_idx, i] = sb;
							y = row_idx * CUBE_SIZE + (float)(AMOUNT_CUBES_IN_LINE - 1) * (-half_cube);
							x = -offset - half_cube;
							break;
						case 'E':
							map[3, row_idx, i] = sb;
							y = row_idx * CUBE_SIZE + (float)(AMOUNT_CUBES_IN_LINE - 1) * (-half_cube);
							x = offset + half_cube;
							break;
					}


					sb.GlobalPosition = new Vector3(x, y, z);
				}
			}
		}
	}

	public override void _Ready()
	{

		generate_map();
		z_spawn_position = (-DEPTH_BOX) * CUBE_SIZE;
		enemy_base_position = new Vector3(player_base_position.X, player_base_position.Y, z_spawn_position);
		rng.Randomize();

		score = GetNode<Label>("Score");
		player = GetNode<PlayerSC>("Player");
		gameOver = GetNode<GameOver>("GameOver");
		player.init(map);
		player.GlobalPosition = player_base_position;
	}
	/*
	StageBox sb = (StageBox)CubeScene.Instantiate();
						MeshInstance3D mesh = sb.GetNode<MeshInstance3D>("MeshInstance3D");
						StandardMaterial3D mat = mesh.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
						mesh.SetSurfaceOverrideMaterial(0, mat);
						mat.AlbedoColor = new Color(1, 0, 0);
	*/
	private float elapsed_time = 0f;
	private float waiting_time = 1f;

	public override void _Process(double delta)
	{
		elapsed_time += (float)delta;
		if (elapsed_time > waiting_time)
		{
			create_wall();
			elapsed_time = 0;
		}
	}

	public void inc_score(int v)
	{
		score_value += v;
		score.Text = " " + score_value;
	}

	private void create_wall()
	{
		bool is_down = rng.Randi() % 2 == 0;
		bool is_up = rng.Randi() % 2 == 0;

		Wall w = (Wall)WallScene.Instantiate();
		Vector3 pos = enemy_base_position;
		if (is_down) pos.Y *= (-1);
		if (is_up) pos.X *= (-1);
		w.init(pos, 0f, 4f, this);
		//MeshInstance3D mesh = sb.GetNode<MeshInstance3D>("MeshInstance3D");
		//StandardMaterial3D mat = mesh.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		//mesh.SetSurfaceOverrideMaterial(0, mat);
		//mat.AlbedoColor = new Color(1, 0, 0);
		AddChild(w);

	}

	public void game_over()
	{
		GD.Print("Game Over");
		Engine.TimeScale = 0;
		gameOver.Visible = true;
	}
}
