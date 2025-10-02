using Godot;
using System;

public partial class GameOver : Control
{
	private void OnRetryPressed(){
		GetTree().ReloadCurrentScene();
		Engine.TimeScale = 1;
	}
	
	private void OnMainMenuPressed(){
		GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
	}
	
	private void OnQuitPressed(){
		GetTree().Quit();
	}
}
