using Godot;
using System;

public partial class Score : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	Label score;

	public override void _Ready()
	{
		score = GetNode<Label>("ScoreUI/VBoxContainer/Coins");
		//GD.Print("Got node: " + score);
	}

	public void updateScore(int amount)
	{
		score.Text = "Coins: " + amount;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
