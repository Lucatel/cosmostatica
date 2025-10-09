using Godot;
using System;
using System.Threading.Tasks;

public partial class StageBox : Node3D{
	private Color original_color;
	private MeshInstance3D mesh;
	private int delay_ms = 100;
	
	public override void _Ready(){
		mesh = GetNode<MeshInstance3D>("MeshInstance3D");
		StandardMaterial3D mat = mesh.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		mat = (StandardMaterial3D)mat.Duplicate();
		mesh.SetSurfaceOverrideMaterial(0, mat);
		original_color = mat.AlbedoColor;
	
	}
	
	private void change_color(Color c){
		StandardMaterial3D mat = mesh.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		mat.AlbedoColor = c;
		mesh.SetSurfaceOverrideMaterial(0, mat);
	}
	
	public async void start_glow(){
		change_color(new Color(0, 0, 1));
		await Task.Delay(delay_ms);
		change_color(original_color);
	}
	
	public void OnPlayerCollided(CharacterBody3D player){
		GD.Print("Collision with: "+player);
	}
	
}
