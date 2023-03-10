using Godot;
using System;

public partial class Spikes : Node2D
{
	Node2D thePlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//thePlayer = GetNode<Node2D>("Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
