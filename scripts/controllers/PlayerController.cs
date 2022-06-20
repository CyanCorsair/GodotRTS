using Godot;
using System.Collections.Generic;

public class PlayerController
{
	private List<BaseShip> _selectedShips = new List<BaseShip>();
	private PlayerInputComponent _inputComponent = null;
	private int _playerControllerId;
	private BaseBattle _battleInstance = null;

	public PlayerController(BaseBattle BattleInstance) {
		this._battleInstance = BattleInstance;
		this._playerControllerId = 0;
	}

	public int GetPlayerControllerId() {
		return this._playerControllerId;
	}

	public void HandleUnitSelection(KinematicBody2D DetectedUnit) {
		BaseShip detectedShip = (BaseShip)DetectedUnit;
		detectedShip.isSelected = true;

		foreach (BaseShip shipToDeselect in this._selectedShips) {
			shipToDeselect.isSelected = false;
		}

		this._selectedShips.Clear();
		this._selectedShips.Add(detectedShip);
	}

	public void HandleUnitMoveOrder(Vector2 TargetLocation) {
		if (this._selectedShips.Count > 0) {
			this._selectedShips[0].MoveTo(TargetLocation);
		}
	}
}
