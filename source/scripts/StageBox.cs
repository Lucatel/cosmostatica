using Godot;
using System;
using System.Threading.Tasks;

public partial class StageBox : Node3D{
	private Color original_color;
	private Color glow_color = new Color(0.451f, 0f, 0.617f);
	private MeshInstance3D mesh;
	private int delay_ms = 200;
	
	public override void _Ready(){
		mesh = GetNode<MeshInstance3D>("MeshInstance3D");
		StandardMaterial3D mat = mesh.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		mat = (StandardMaterial3D)mat.Duplicate();
		mesh.SetSurfaceOverrideMaterial(0, mat);
		original_color = mat.AlbedoColor;
	
	}
	
	private StandardMaterial3D change_color(Color c){
		StandardMaterial3D mat = mesh.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		mat.EmissionEnabled = true;
		mat.Emission = new Color(0f, 0f, 0f);
		mat.AlbedoColor = c;
		mesh.SetSurfaceOverrideMaterial(0, mat);
		return mat;
	}
	
	public void reset_color(){
		StandardMaterial3D mat = mesh.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		mat.EmissionEnabled = false;
		mat.AlbedoColor = original_color;
		mesh.SetSurfaceOverrideMaterial(0, mat);
	}
	
	public async void start_glow(){
		StandardMaterial3D mat = change_color(glow_color);
		const int STEPS=10;
		
		for(int i=STEPS-1;i>=0;i--){
			await Task.Delay(delay_ms/STEPS);
			mat.Emission = new Color(glow_color.R * ((float)i/STEPS),glow_color.G * (float)i/STEPS,glow_color.B * (float)i/STEPS);
		}
		reset_color();
	}
	
	public void OnPlayerCollided(CharacterBody3D player){
		GD.Print("Collision with: "+player);
	}
	
}
