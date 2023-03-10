using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	public int coins;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	Godot.AnimatedSprite2D playerSprite;

	Vector2 spawnPosition;

	CanvasLayer score;

	bool dead;

	bool respawning;

	Timer coyote;

	public override void _Ready()
	{
		playerSprite = GetNode<Godot.AnimatedSprite2D>("AnimatedSprite2D");
		score = GetTree().Root.GetNode("Level").GetNode<CanvasLayer>("ScoreBoard");
		coyote = GetNode<Timer>("Coyote");
		GD.Print("Got the Scoreboard: " + score);
		spawnPosition = GlobalPosition;
		dead = false;
		respawning = false;
		coins = 0;
	}

	public async void onDeath()
	{
		dead = true;
		playerSprite.Play("Death");
		await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
		respawn();
	}

	public void addCoins(int amount)
	{
		coins = coins + amount;
		GD.Print("Coints: " + coins);

		if(score is Score sc) 
		{
			sc.updateScore(coins);
		}
	}

	public async void respawn()
	{
		dead = false;
		respawning = true;
		coins = 0;

		if(score is Score sc) 
		{
			sc.updateScore(coins);
		}

		playerSprite.FlipH = false;
		GlobalPosition = spawnPosition;
		playerSprite.Play("Spawn");
		await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
		playerSprite.Play("Idle");
		respawning = false;

	}

	public override void _PhysicsProcess(double delta)
	{
		if(!dead && !respawning)
		{
			Vector2 velocity = Velocity;

			// Add the gravity.
			if (!IsOnFloor())
			{
				velocity.Y += gravity * (float)delta;
				// the player is not on the floor

				// are they jumping/
				if (velocity.Y <= 0)
				{
					playerSprite.Play("Jump");
				}
				else 
				{
					playerSprite.Play("Fall");
				}
			} 
			else 
			{
				// the player is probably on a floor

				// are they standing still?
				if(velocity.X == 0) 
				{
					playerSprite.Play("Idle");
				}
				else
				{
					// or are they running?
					playerSprite.Play("Move");
				}
			}


			// Handle Jump.
			if (Input.IsActionJustPressed("jump") && (IsOnFloor() || !coyote.IsStopped()))
			{
				velocity.Y = JumpVelocity;
			}
				

			// Get the input direction and handle the movement/deceleration.
			// As good practice, you should replace UI actions with custom gameplay actions.
			Vector2 playerDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			if (playerDirection != Vector2.Zero)
			{
				velocity.X = playerDirection.X * Speed;

				if(velocity.X < 0)
				{
					playerSprite.FlipH = true;
					//playerSprite.hflip = true;
				}
				else if(velocity.X > 0)
				{
					playerSprite.FlipH = false;
					//playerSprite.hflip = false;
				}
				
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			}

			Velocity = velocity;

			bool wasOnFloor = IsOnFloor();
			MoveAndSlide();

			if(wasOnFloor && !IsOnFloor())
			{
				coyote.Start();
			}
		}
	}
}
