using System;
using System.Collections.Generic;

/// <summary>
/// 	Contains the tanks, cells, lifepacks, coinpacks resulting from a move, 
/// 	and depth of that particular move relative to current state of the game.
/// 	This class is used to simplify AI calculation 
/// </summary>
public class GameData
{
	/// <summary>
	/// 	Tank Objects with their health, position, direction, and shot
	/// </summary>
	public Dictionary<int, Tank> tanks;

	/// <summary>
	/// 	Cell Objects with their type, damage, and position
	/// </summary>
	public List<Cell> cells;

	/// <summary>
	/// 	Life Packs with their position and lifetime
	/// </summary>
	public List<LifePack> lifepacks;

	/// <summary>
	/// 	Coin Packs with their position, lifetime and coin value
	/// </summary>
	public List<CoinPack> coinpacks;

	/// <summary>
	/// 	Construct a DataObject corresponding to each possibility. Use the updated parameters resulting from that possibility.
	/// </summary>
	/// <param name="tanks">Tanks.</param>
	/// <param name="cells">Cells.</param>
	/// <param name="lifepacks">Lifepacks.</param>
	/// <param name="coinpacks">Coinpacks.</param>
	public GameData (Dictionary<int, Tank> tanks, List<Cell> cells, List<LifePack> lifepacks, List<CoinPack> coinpacks)
	{
		this.tanks = tanks;
		this.cells = cells;
		this.lifepacks = lifepacks;
		this.coinpacks = coinpacks;
	}

	public GameData()
	{
		tanks = new Dictionary<int, Tank> ();
		cells = new List<Cell> ();
		lifepacks = new List<LifePack> ();
		coinpacks = new List<CoinPack> ();
	}
}

