using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AssemblyCSharp;

public class Game
{
	public List<Cell> cells;
	public List<CoinPack> coinpacks;
	public List<LifePack> lifepacks;
	public Dictionary<int, Tank> tanks;
	private int? playerNo;

	public Game ()
	{
		cells = new List<Cell> ();
		tanks = new Dictionary<int, Tank> ();
		coinpacks = new List<CoinPack> ();
		lifepacks = new List<LifePack> ();
	}

	#region Client Request Execution

	public void ExecuteClientRequest (ClientRequest request)
	{
		switch (request) {
		case ClientRequest.Down:
			break;
		case ClientRequest.Join:
			break;
		case ClientRequest.Left:
			break;
		case ClientRequest.Right:
			break;
		case ClientRequest.Shoot:
			break;
		case ClientRequest.Up:
			break;

		default:
			Debug.LogError ("Unidentified Request - " + request);
			break;
		}
	}

	#endregion

	#region Server Command Execution

	public void ExecuteServerCommand (string command)
	{
		// Errors
		switch (command.TrimEnd ('#')) {
		case Constants.S2C_ALREADYADDED:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_CELLOCCUPIED:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_CONTESTANTSFULL:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_FALLENTOPIT:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_GAMEJUSTFINISHED:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_GAMEOVER:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_GAMESTARTED:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_HITONOBSTACLE:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_INVALIDCELL:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_NOTACONTESTANT:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_NOTALIVE:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_NOTSTARTED:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_REQUESTERROR:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_SERVERERROR:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		case Constants.S2C_TOOEARLY:
			Debug.Log ("Processing Command -> " + command.TrimEnd ('#'));
			break;
		}

		var identifier = command.Substring (0, 2);

		// Broadcasts
		switch (identifier) {
		// Game Acceptance
		case "S:":
			Debug.Log ("Identified Command -> " + identifier + command.TrimEnd ('#'));
			GameAcceptanceExec (command.TrimEnd ('#'));
			break;

		// Game Initiation
		case "I:":
			Debug.Log ("Identified Command -> " + identifier + command.TrimEnd ('#'));
			GameInitExec (command.TrimEnd ('#'));
			break;

		// Game Update
		case "G:":
			Debug.Log ("Identified Command -> " + identifier + command.TrimEnd ('#'));
			GameUpdateExec (command.TrimEnd ('#'));
			break;

		// Coin Spawn
		case "C:":
			Debug.Log ("Identified Command -> " + identifier + command.TrimEnd ('#'));
			CoinSpawnExec (command.TrimEnd ('#'));
			break;

		// Lifepack Spawn
		case "L:":
			Debug.Log ("Identified Command -> " + identifier + command.TrimEnd ('#'));
			LifepackSpawnExec (command.TrimEnd ('#'));
			break;

		// Unidentified Message
		default:
			Debug.LogError ("Command Not Identified -> " + command.TrimEnd ('#'));
			break;
		}
	}

	/// <summary>
	///     FORMAT = [S : Pn;x,y;d : Pn;x,y;d#]
	/// </summary>
	/// <param name="command">Command.</param>
	private void GameAcceptanceExec (string command)
	{
		try {
			var positions = command.Substring (2).Split (':');

			foreach (var position in positions) {
				var fields = position.Split (';');
				var number = int.Parse (fields [0].Substring (1));
				tanks [number] = new Tank (number, number == playerNo, int.Parse (fields [1].Split (',') [0]),
					int.Parse (fields [1].Split (',') [1]), int.Parse (fields [2]));
			}

			// Generate All Tanks
			GenerateGameObjects.GetInstance ().GenerateTanks (tanks);

		} catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	/// <summary>
	///     FORMAT = [I : Pn : x,y; ... ;x,y : x,y; ... ;x,y : x,y; ... ;x,y#]
	/// </summary>
	/// <param name="command">Command.</param>
	private void GameInitExec (string command)
	{
		try {
			var data = command.Substring (2).Split (':');

			// Set player number variable (Does not update tanks dictionary)
			playerNo = int.Parse (data [0].Substring (1));

			// Cells Initiation
			cells.Clear ();
			for (var i = 1; i <= 3; i++) {
				foreach (var coordinatePair in data[i].Split(';')) {
					var coordinates = coordinatePair.Split (',');
					cells.Add (new Cell (int.Parse (coordinates [0]), int.Parse (coordinates [1]), i - 1, 0));
				}
			}

			// Generate All Cells
			GenerateGameObjects.GetInstance ().GenerateCells (cells);

		} catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	/// <summary>
	///     FORMAT = [G : Pn;x,y;d;shot;health;coins;points : ... : Pn;x,y;d;shot;health;coins;points : x,y,damage; ...
	///     ;x,y,damage#]
	/// </summary>
	/// <param name="command">Command.</param>
	private void GameUpdateExec (string command)
	{
		try {
			var data = command.Substring (2).Split (':');
			var consumedCoinPacks = new List<CoinPack>();
			var consumedLifePacks = new List<LifePack>();


			foreach (var d in data) {
				if (d [0] == 'P') {
					// Tank Status Update [Pn;x,y;d;shot;health;coins;points]
					var state = d.Split (';');
					var tank = tanks [int.Parse (state [0].Substring (1))];
					tank.x = int.Parse (state [1].Split (',') [0]);
					tank.y = int.Parse (state [1].Split (',') [1]);
					tank.direction = (Direction)int.Parse (state [2]);
					tank.isShot = int.Parse (state [3]) == 1;
					tank.health = int.Parse (state [4]);
					tank.coins = int.Parse (state [5]);
					tank.points = int.Parse (state [6]);
					consumedCoinPacks.AddRange(coinpacks.Where(p => p.x == tank.x && p.y == tank.y));
					consumedLifePacks.AddRange(lifepacks.Where(p => p.x == tank.x && p.y == tank.y));

				} else {
					// Cell status update [x,y,damage ; ... ; x,y,damage]
					foreach (var locationupdate in d.Split(';')) {
						var info = locationupdate.Split (',');
						var cell = cells.FirstOrDefault (c => c.x == int.Parse (info [0]) && c.y == int.Parse (info [1]));
						if (cell != null)
							cell.damage = (CellDamage)int.Parse (info [2]);
					}
				}
			}

			// Generate all Tanks and Cells
			GenerateGameObjects.GetInstance ().GenerateCells (cells);
			GenerateGameObjects.GetInstance ().GenerateTanks (tanks);

			RemoveExpiredPacks ();
			RemoveConsumedPacks(consumedCoinPacks, consumedLifePacks);

            ScoreManager.GetInstance().updateScore(tanks);

            var thread = new System.Threading.Thread(() =>
            {
                var nextMove = new AI(playerNo.Value, 2).GetNextBestMove(tanks, cells, lifepacks, coinpacks).ToString();
                Debug.LogWarning("Result for next Move - " + nextMove.ToString());
                ServerConnect.MakeC2SRequest(nextMove.ToString().ToUpper() + "#");
            });
            thread.Priority = System.Threading.ThreadPriority.Highest;
            thread.Start();

        } catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	/// <summary>
	///     FORMAT = [C:x,y:lt:val#]
	/// </summary>
	/// <param name="command">Command.</param>
	private void CoinSpawnExec (string command)
	{
		try {
			var coins = command.Substring (2).Split (':');
			var location = coins [0].Split (',');
			var coinPack = new CoinPack (int.Parse (location [0]), int.Parse (location [1]), int.Parse (coins [1]),
				               int.Parse (coins [2]));
			coinpacks.Add(coinPack);
			// Generate Coin Packs
			GenerateGameObjects.GetInstance ().GenerateCoinPacks (coinPack);

		} catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	/// <summary>
	///     FORMAT = [L:x,y:lt#]
	/// </summary>
	/// <param name="command">Command.</param>
	private void LifepackSpawnExec (string command)
	{
		try {
			var lifes = command.Substring (2).Split (':');
			var location = lifes [0].Split (',');
			var lifePack = new LifePack (int.Parse (location [0]), int.Parse (location [1]), int.Parse (lifes [1]));
			lifepacks.Add(lifePack);

			// Generate Life Packs
			GenerateGameObjects.GetInstance ().GenerateLifePacks (lifePack);

		} catch (Exception ex) {
			Debug.LogException (ex);
		}
	}

	/// <summary>
	/// 	Remove expired coin packs from list and update UI
	/// </summary>
	void RemoveExpiredPacks ()
	{
		var expiredCoinPacks = coinpacks.Where (p => p.isExpired ());
		var expiredLifePacks = lifepacks.Where (p => p.isExpired ());

		GenerateGameObjects.GetInstance ().DestroyCoinPacks (expiredCoinPacks);
		foreach (var pack in expiredCoinPacks) {
			coinpacks.Remove (pack);
		}

		GenerateGameObjects.GetInstance ().DestroyLifePacks (expiredLifePacks);
		foreach (var pack in expiredLifePacks) {
			lifepacks.Remove (pack);
		}

	}

	void RemoveConsumedPacks (List<CoinPack> consumedCoinPacks, List<LifePack> consumedLifePacks)
	{
		GenerateGameObjects.GetInstance ().DestroyCoinPacks (consumedCoinPacks);
		foreach (var pack in consumedCoinPacks) {
			coinpacks.Remove (pack);
		}

		GenerateGameObjects.GetInstance ().DestroyLifePacks (consumedLifePacks);
		foreach (var pack in consumedLifePacks) {
			lifepacks.Remove (pack);
		}

	}
	#endregion
}