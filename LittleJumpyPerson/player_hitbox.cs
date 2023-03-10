using Godot;
using System;

public partial class player_hitbox : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_body_entered(Node2D node)
	{
		GD.Print("Hitbox touched by node named " + node.Name + " of type " + node.GetType());

		if(node is Player player) 
		{
			player.onDeath();
		} 
	}
}
