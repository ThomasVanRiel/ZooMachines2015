using UnityEngine;
using System.Collections;

public interface GameMode {
	// PlayerKilled
	void PlayerKilled(PlayerController killer, PlayerController killed);

	// Check whether or not the game is over
	bool IsGameOver();

	// Get the winners of this game
	PlayerController Winner();
}
