using Godot;
using System;

public class MainGame : Node2D
{
	private Node CurrentSceneInstance { get; set; } = null;
	private Node NewSceneInstance { get; set; } = null;

	// PRIVATE METHODS
	private void DeferredGotoScene(string path) {
		// It is now safe to remove the current scene
		// so load and instance the next one
		if (CurrentSceneInstance is Node) {
			CurrentSceneInstance.Free();
		}

		GetTree().ChangeScene(path);
	}

	private void LoadMainMenu() {
		GotoScene("res://maps/ui/MainMenu.tscn");
	}

	// PUBLIC METHODS
	public override void _Ready()
	{
		Viewport root = GetTree().Root;

		if (CurrentSceneInstance is null) {
			this.LoadMainMenu();
		} else {
			CurrentSceneInstance = root.GetChild(root.GetChildCount() - 1);
		}
	}

	public void GotoScene(string path) {
		// This function will usually be called from a signal callback,
		// or some other function from the current scene.
		// Deleting the current scene at this point is
		// a bad idea, because it may still be executing code.
		// This will result in a crash or unexpected behavior.

		// The solution is to defer the load to a later time, when
		// we can be sure that no code from the current scene is running:

		CallDeferred(nameof(DeferredGotoScene), path);
	}
}
