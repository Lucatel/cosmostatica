using Godot;

public class SpawnManager{
	private Game game;
	PackedScene WallScene = (PackedScene)ResourceLoader.Load("res://scenes/Objects/Wall.tscn");
	private float wall_speed = 8f;
	private const int BOX_ARRAY_SIZE = 4;
	private static int[] last_box_positions = new int[BOX_ARRAY_SIZE];
	private static RandomNumberGenerator rng = new RandomNumberGenerator();
	private double elapsed_time = 0f;
	private double waiting_time = 1.3f;
	private Vector3 enemy_base_position;
	
	
	public void init(Game game, Vector3 enemy_base_position){
		rng.Randomize();
		this.game = game;
		this.enemy_base_position = enemy_base_position;
	}
	
	public void update(double delta, int score_value){
		elapsed_time += delta;
		if((score_value) > 80 && waiting_time != 1f){
			waiting_time = 1f;
		}else if((score_value) > 130 && wall_speed != 10){
			wall_speed = 10;
		}
		
		if (elapsed_time > waiting_time){
			if((score_value) < 10){
				create_wall(1);
			}else if(score_value < 100){
				create_wall(2);
			}else{
				create_wall(3);
			}
			elapsed_time = 0;
		}
	}
	
	private bool are_boxPositions_same(int[] ar1, int[] ar2, uint ar_length){
		bool res = true;
		int[] res_arr = new int[BOX_ARRAY_SIZE];
		for(int i=0;i<ar_length;i++){
			res_arr[ar1[i]]++;
			res_arr[ar2[i]]++;
		}
		for(int i=0;i<ar_length && res;i++){
			if(res_arr[i] != 2 && res_arr[i] != 0) res = false;
		}
		
		return res;
	}
	
	private void create_wall(uint layer_amount){
		if(layer_amount>3 || layer_amount<0) layer_amount=1;
		int[] positions = {0,1,2,3};
		
		do{
			for(int n=0;n<positions.Length;n++)
			for(int i=1;i<positions.Length;i++){
				if(rng.Randi() % 2 == 0){
					(positions[i-1], positions[i]) = (positions[i], positions[i-1]);
				}
			}
		}while(are_boxPositions_same(positions,last_box_positions,layer_amount));
		
		for(int i=0;i<last_box_positions.Length;i++){
			last_box_positions[i] = positions[i];
		}
		
		for(int i=0;i<layer_amount;i++){
			Wall w = (Wall)WallScene.Instantiate();
			Vector3 pos = enemy_base_position;
			
			if(positions[i] < 2) pos.Y *= (-1);
			if(positions[i] % 2 == 0) pos.X *= (-1);
			
			w.init(pos, 0f, wall_speed, game);
			//MeshInstance3D mesh = sb.GetNode<MeshInstance3D>("MeshInstance3D");
			//StandardMaterial3D mat = mesh.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
			//mesh.SetSurfaceOverrideMaterial(0, mat);
			//mat.AlbedoColor = new Color(1, 0, 0);
			game.AddChild(w);
		}
		
	}
}
