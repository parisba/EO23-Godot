using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	
	Vector2 enemyDirection = Vector2.Right;

	AnimatedSprite2D enemySprite;
	RayCast2D leftEdge, rightEdge;

	public void _on_test_bonk_body_entered(Node body)
	{
		GD.Print("Bonk: " + body);
		enemyDie();
	}

	public async void enemyDie()
	{
		enemySprite.Play("Die");
		await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
		QueueFree();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemySprite = GetNode<AnimatedSprite2D>("EnemySprite");
		leftEdge = GetNode<RayCast2D>("Edge1");
		rightEdge = GetNode<RayCast2D>("Edge2");
		//GD.Print("Starting Velocity: " + Velocity);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 velocity = Vector2.Right;

		// Check if we hit a wall
		if(IsOnWall()) {
			//GD.Print("Enemy hit wall");
			enemyDirection.X = -enemyDirection.X;
		}

		// There's nothing to the left, but there is to the right.
		if(!leftEdge.IsColliding() && rightEdge.IsColliding())
		{
			enemyDirection = Vector2.Right;
		} else if(leftEdge.IsColliding() && !rightEdge.IsColliding()) {
			enemyDirection = Vector2.Left;
		}
		
		velocity = enemyDirection * 25;

		//GD.Print("Velocity: " + velocity);
		//GD.Print("Direction: " + enemyDirection);

		velocity.Y += gravity * (float)delta;

		if(velocity.X < 0)
		{
			enemySprite.FlipH = true;
		} 
		else 
		{
			enemySprite.FlipH = false;
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
