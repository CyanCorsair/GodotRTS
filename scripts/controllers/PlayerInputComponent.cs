using Godot;
using System;

public class PlayerInputComponent : Node
{
	private PlayerController owningPlayerController = null;

	private Godot.Collections.Array GetHitUnderCursor(Vector2 CursorLocation) {
		Physics2DDirectSpaceState worldSpace = GetViewport().World2d.DirectSpaceState;
		Godot.Collections.Array hitResults = worldSpace.IntersectPoint(CursorLocation, 1, null, 2);
		GD.Print("Hit results: " + hitResults.Count);

		return hitResults;
	}

	private void OnLeftMouseClick(Vector2 ClickCoordinates) {
		Godot.Collections.Array hitResults = this.GetHitUnderCursor(ClickCoordinates);
		
		if (hitResults.Count == 1) {
			Godot.Collections.Dictionary firstHitResult = (Godot.Collections.Dictionary)hitResults[0];
			this.owningPlayerController.HandleUnitSelection((KinematicBody2D)firstHitResult["collider"]);
		}

	}

	private void OnRightMouseClick(Vector2 ClickCoordinates) {
		this.owningPlayerController.HandleUnitMoveOrder(ClickCoordinates);
	}

	public override void _Ready() {
		TestBattle parentNodeInstance = (TestBattle)GetNode("/root/BattleMapRoot");
		this.owningPlayerController = parentNodeInstance.GetPlayerController();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("UnitSelect")) {
			Vector2 mouseViewportLocation = GetViewport().GetMousePosition();
			this.OnLeftMouseClick(mouseViewportLocation);
		}

		if(@event.IsActionPressed("GiveUnitOrder")) {
			Vector2 mouseViewportLocation = GetViewport().GetMousePosition();
			this.OnRightMouseClick(mouseViewportLocation);
		}
	}
}
