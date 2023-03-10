using Godot;
using System;

public partial class Coin : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_coin_collider_body_entered(Node2D body)
	{
		GD.Print("Coin hit by " + body);

		if(body is Player player) 
		{
			player.addCoins(1);
			QueueFree();
		} 
		
	}
}
