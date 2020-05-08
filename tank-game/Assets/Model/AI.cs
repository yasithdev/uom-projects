using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AssemblyCSharp
{
	public class AI
	{
		private static int PlayerNo;
		private int aiLevel;

		// Weights
		private static int[] weights = new int[]{ 10, 25, 50, 100, 200 };

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyCSharp.AI"/> class.
		/// </summary>
		/// <param name="playerId">Player identifier.</param>
		/// <param name="aiLevel">Ai level.</param>
		public AI (int playerId, int aiLevel)
		{
			PlayerNo = playerId;
			this.aiLevel = aiLevel;
		}

		// <summary>
		/// 	Calculates and returns the next best move.
		/// 	Accuracy of move increases with AI level.
		/// </summary>
		/// <returns>The next best move.</returns>
		/// <param name="tanks">Tanks.</param>
		/// <param name="cells">Cells.</param>
		/// <param name="lifepacks">Lifepacks.</param>
		/// <param name="coinpacks">Coinpacks.</param>/
		public ClientRequest GetNextBestMove (Dictionary<int,Tank> tanks, List<Cell> cells, List<LifePack> lifepacks, List<CoinPack> coinpacks)
		{
			//Creates new data object from the passed data. (Initially. the depth is 0, and all parameters are assigned as-is)
			var dataObject = new GameData (tanks, cells, lifepacks, coinpacks);
			Dictionary<ClientRequest, int> moveCosts = new Dictionary<ClientRequest, int> ();

			// Calculate the å-ß values for each client request, and return the best move
			foreach (var request in (ClientRequest[])Enum.GetValues(typeof(ClientRequest))) {
				// Ignore Join condition :)
				if (request == ClientRequest.Join || request == ClientRequest.Shoot)
					continue;
				if(tanks[PlayerNo].GetNextState(request) == null)
					continue;
				moveCosts [request] = AlphaBeta (dataObject, aiLevel, int.MinValue, int.MaxValue, true);
			}

			// Return Key with max value from dictionary
			var result = moveCosts.Aggregate ((l, r) => l.Value > r.Value ? l : r).Key;

			return result;
		}

		/// <summary>
		/// 	Assess a given state of game and determine the moves, with move-scores
		/// 	MinMax tree goes as far as the AI Level defined in the class
		/// 	Original Call Format - CalculateBestMoveAlphaBeta(gameData, -∞, +∞, true, aiLevel)
		/// </summary>
		/// <returns>The move cost.</returns>
		private int AlphaBeta (GameData data, int depth, int a, int b, bool maxPlayer)
		{
			/* -------------------------------------------------------------------------------------------------
			 * The algorithm recursively checks for the possible chain of outcomes and their relative costs.
			 * The depth of recursion is increased with increasing AI Level, and so is the accuracy of the move 
			 *  and the time taken for calculcation.
			 * 
			 * The algorithm used is the å-ß algorithm, which is an extension if MiniMax algorithm. 
			 * For each move, a cost is calculated depending on player health, points, and coins, and this
			 * helps to achieve an effective selection of moves to progress the game.
			 * -------------------------------------------------------------------------------------------------
			 */

			if (depth == 0 /*|| node == @"Terminal Node"*/)
				return CalculateMoveScore (data, PlayerNo);
			
			if (maxPlayer) {
				var v = int.MinValue;
				var newMoves = new List<GameData> ();

				GetNextMoves (data, 0, newMoves);
				foreach (var nextMove in newMoves) {
					v = Math.Max (v, AlphaBeta (nextMove, depth - 1, a, b, false));
					a = Math.Max (a, v);
					if (b <= a)
						break;
				}
				return v;
			} else {
				var v = int.MaxValue;
				var newMoves = new List<GameData> ();

				GetNextMoves (data, 0, newMoves);
				foreach (var nextMove in newMoves) {
					v = Math.Min (v, AlphaBeta (nextMove, depth - 1, a, b, true));
					b = Math.Min (b, v);
					if (b <= a)
						break;
				}
				return v;
			}
		}

		/* ----------------------------------------------------------- 
		 * MOVE SCORE CALCULATION LOGIC
		 * -----------------------------------------------------------
		 */
		public static int CalculateMoveScore (GameData data, int playerId)
		{
			int result = 0;
			var player = data.tanks [playerId];
			int i = player.x;
			int j = player.y;

			#region Checking Whether Shot

			bool noTanksFacingPlayer = true;
			foreach (var t in data.tanks) {
				
				// Terminating condition if any tank is out of grid scope
				if (t.Value.x < 0 || t.Value.y < 0 || t.Value.x >= Constants.MAP_SIZE || t.Value.y >= Constants.MAP_SIZE)
					return 0;

				// Terminating condition if any tank lies on a cell
				if (data.cells.Any (c => c.x == t.Value.x && c.y == t.Value.y))
					return 0;

				if (t.Value.isPlayer)
					continue;
				
				if (t.Value.x == i) {
					if ((t.Value.y < j && t.Value.direction == Direction.Up) || (t.Value.y > j && t.Value.direction == Direction.Down)/* if enemy tank is facing player*/) {
						noTanksFacingPlayer = false;
						int lo = Math.Min (j, t.Value.y);
						int hi = Math.Max (j, t.Value.y);
						for (int p = lo; p < hi; p++) {
							result += data.cells.Where (cell => cell.y == j && cell.x == p && cell.type != CellType.Water).Sum (cell => weights [1]);
						}
					}
				} else if (t.Value.y == j) {
					if ((t.Value.x < i && t.Value.direction == Direction.Right) || (t.Value.x > i && t.Value.direction == Direction.Left)/* if enemy tank is facing player*/) {
						noTanksFacingPlayer = false;
						int lo = Math.Min (i, t.Value.x);
						int hi = Math.Max (i, t.Value.x);
						for (int p = lo; p < hi; p++) {
							result += data.cells.Where (cell => cell.y == j && cell.x == p && cell.type != CellType.Water).Sum (cell => weights [1]);
						}
					}
				}
				// If no enemy tanks facing player, no risk of being shot, and higher chance of damage to opponent
				if (noTanksFacingPlayer)
					result += weights [3];
			}
			#endregion

			/*Add max number of points to result if lands on lifepack or coinpack*/
			result += 
				data.lifepacks.Where (lifepack => lifepack.x == i && lifepack.y == j).Sum (lifepack => weights [4]) +
			data.coinpacks.Where (coinpack => coinpack.x == i && coinpack.y == j).Sum (coinpack => weights [4]);

			return result;
		}

		/* ------------------------------------------------------------
		 * Method to return all next possibilities from GameData Object
		 * ------------------------------------------------------------
		 */
		private static void GetNextMoves (GameData data, int k, List<GameData> nextMoves)
		{
			DateTime startTime = DateTime.Now;

			try {
				
				/********************************/
				foreach (var request in (ClientRequest[])Enum.GetValues(typeof(ClientRequest))) {

					// Ignore Join and Shoot conditions :)
					if (request == ClientRequest.Join || request == ClientRequest.Shoot)
						continue;

					// Get next state of tank. Continue if newTankState is null
					var newTankState = data.tanks [k].GetNextState (request);
					if (newTankState == null)
						continue;

					// avoid pitfalls and collisions
					if (data.tanks[k].WillTankChangeLocation(request)){
						if(data.cells.Any(c => c.x == newTankState.x && c.y == newTankState.y))
							continue;
						if(data.tanks.Count(t=> t.Value.x == newTankState.x && t.Value.y == newTankState.y) > 1)
							continue;
					}

					// Create new GameData corresponding to the change
					var newData = new GameData (data.tanks, data.cells, data.lifepacks, data.coinpacks);
					newData.tanks [k] = newTankState;

					// Recursively generate all moves
					if (data.tanks.Count > k + 1)
						GetNextMoves (newData, k + 1, nextMoves);
					else {
						// Add newData objects when all tanks have been updated with some action
						nextMoves.Add (newData);
					}
				}
				/********************************/

			} catch (Exception e) {
				Debug.LogWarning ("An exception occured in GetAllNextMoves");
				Debug.LogException (e);
			}

			DateTime endTime = DateTime.Now;
			Debug.LogWarning ("Time taken" + endTime.Subtract (startTime).TotalMilliseconds + "ms");
		}
	}
}
