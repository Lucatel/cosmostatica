using Godot;
using System;

public partial class MainMenu : Control
{
	private void OnPlayPressed(){
		GetTree().ChangeSceneToFile("res://scenes/game.tscn");
	}
	
	private void OnSettingsPressed(){
		GetTree().ChangeSceneToFile("res://scenes/Settings.tscn");
	}
	
	private void OnQuitPressed(){
		GetTree().Quit();
	}
}
