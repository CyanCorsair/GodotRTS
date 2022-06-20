using Godot;
using System.Collections.Generic;

public struct ShipComponentSlot {
	public BaseShipComponent componentInSlot;
	public int maxSize;
	public string socketName;
}

enum ShipState {
	IDLE,
	TURNING,
	MOVING,
	DEAD = -1
}

enum ShipOrders {
	MOVE,
	ATTACK,
	NONE = -1
}

public class BaseShip : KinematicBody2D
{
	[Export] public int speed = 100;
	[Export] public float turnSpeed = 0.05f;
	[Export] public string shipName = "Template cruiser";
	private Dictionary<ShipComponentSlot, BaseShipComponent> shipComponents = new Dictionary<ShipComponentSlot, BaseShipComponent>();

	private int baseHealth = 100;
	private float baseArmor = 0.1f;

	private Vector2 targetLocation2d;
	private Vector2 velocity2d;
	private ShipState shipState = ShipState.IDLE;
	private ShipOrders currentOrder = ShipOrders.NONE;
	private BaseShip targetShip = null;

	public int owningPlayerId = 0;
	public bool isSelected = false;

	public void MoveTo(Vector2 TargetLocation) {
		this.targetLocation2d = TargetLocation;
		this.currentOrder = ShipOrders.MOVE;
	}
	
	public override void _Ready() {
		this.targetLocation2d = this.Position;
	}

	public override void _Draw() {
		if (this.isSelected) {
			Transform2D nodePosition = this.GetCanvasTransform();
			this.DrawCircle(nodePosition.origin, 45, new Color(0,1,0));

			if (this.currentOrder == ShipOrders.MOVE) {
				Vector2 targetTransform = (this.targetLocation2d * this.GetGlobalTransform());
				this.DrawCircle(targetTransform, 25, new Color(0,0,1));
				this.DrawLine(nodePosition.origin, targetTransform, new Color(0,0,1));
			}
		}
	}

	public override void _Process(float delta) {
		Update();
	}

	public override void _PhysicsProcess(float delta) {
		this.velocity2d = new Vector2(0,0);
		if (this.currentOrder == ShipOrders.MOVE) {
			float turnFrom = this.Rotation;
			Vector2 directionToTarget = this.Position.DirectionTo(this.targetLocation2d);
			float turnTo = directionToTarget.Angle();

			if (turnFrom != turnTo) {
				this.shipState = ShipState.TURNING;
				this.Rotation = Godot.Mathf.LerpAngle(turnFrom, turnTo, this.turnSpeed);
			}

			if (this.Position.DistanceTo(this.targetLocation2d) > 5) {
				this.shipState = ShipState.MOVING;
				this.velocity2d = this.Position.DirectionTo(this.targetLocation2d) * this.speed;
				this.MoveAndSlide(this.velocity2d).Rotated(this.Rotation);
			}
		}

		if (this.velocity2d.Length() == 0) {
			this.shipState = ShipState.IDLE;
			this.currentOrder = ShipOrders.NONE;
		}
	}
}
