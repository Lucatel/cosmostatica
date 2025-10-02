using Godot;
using System;

public partial class StageBox : Node3D{
	Color originalColor;
	MeshInstance3D mesh;
	
	public override void _Ready(){
		mesh = GetNode<MeshInstance3D>("MeshInstance3D");
		StandardMaterial3D mat = mesh.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		originalColor = mat.AlbedoColor;
	}
	
	private void change_color(){
		StandardMaterial3D mat = mesh.GetSurfaceOverrideMaterial(0) as StandardMaterial3D;
		mat.AlbedoColor = new Color(0, 0, 1);
		mesh.SetSurfaceOverrideMaterial(0, mat);
	}
	
	public void start_glow(){
		
	}
	
}
