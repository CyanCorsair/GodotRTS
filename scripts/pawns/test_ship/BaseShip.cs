using Godot;
using System;

enum ShipState {
	IDLE,
	TURNING,
	MOVING,
	DEAD
}

public class BaseShip : KinematicBody2D
{
	[Export] public int speed = 100;

	private Vector2 targetLocation2d;
	private Vector2 velocity2d;
	private ShipState _shipState = ShipState.IDLE;

	public int owningPlayerId = 0;
	public bool isSelected = false;

	public void MoveTo(Vector2 TargetLocation) {
		this.targetLocation2d = TargetLocation;
	}
	
	public override void _Ready() {
		this.targetLocation2d = this.Position;
	}

	public override void _Draw() {
		if (this.isSelected) {
			Transform2D nodePosition = this.GetCanvasTransform();
			this.DrawCircle(nodePosition.origin, 45, new Color(0,1,0));
		}
	}

	public override void _Process(float delta) {
		Update();
	}

	public override void _PhysicsProcess(float delta) {
		this.velocity2d = new Vector2(0,0);
		if (this._shipState == ShipState.IDLE) {
			float turnFrom = this.Rotation;
			Vector2 testVel = this.Position.DirectionTo(this.targetLocation2d) * this.speed;
			float turnTo = testVel.Angle();

			if (turnFrom != turnTo) {
				this._shipState = ShipState.TURNING;
				this.Rotation = Godot.Mathf.LerpAngle(turnFrom, turnTo, 0.1f);
			}

			if (this.Position.DistanceTo(this.targetLocation2d) > 5) {
				this._shipState = ShipState.MOVING;
				this.velocity2d = this.Position.DirectionTo(this.targetLocation2d) * this.speed;
				this.MoveAndSlide(this.velocity2d).Rotated(this.Rotation);
			}

			this._shipState = ShipState.IDLE;
		}
	}
}
