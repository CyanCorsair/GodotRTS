using Godot;
using System.Collections.Generic;

public class TestBattle : Node2D
{
	private PlayerController _playerControllerInstance = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this._playerControllerInstance = new PlayerController(this);
		GetTree().Root.AddChildBelowNode(this, new PlayerInputComponent());
	}

	public PlayerController GetPlayerController() {
		return this._playerControllerInstance;
	}

}
