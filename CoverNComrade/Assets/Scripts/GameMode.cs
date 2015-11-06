using UnityEngine;
using System.Collections;

public interface GameMode {
	// 
	void Setup(GameManager gm, int nbPlayers);

	// PlayerKilled announces a player's death and its killer
	void PlayerKilled(PlayerController killer, PlayerController killed);

	// Check whether or not the game is over
	bool IsGameOver();

	// Get the winner of this game, game should be over before calling this method
	int Winner();
}
