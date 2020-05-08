using System.Collections.Generic;
using UnityEngine;
using System.Linq;

internal class GenerateGameObjects : MonoBehaviour
{
	public GameObject Brick;
	public GameObject CoinPack;
	public GameObject LifePack;
	public GameObject Stone;
	public GameObject EnemyTank;
	public GameObject PlayerTank;
	public GameObject Water;

	private static GenerateGameObjects instance;
	private static Quaternion originalRot = Quaternion.identity;
	private const int xFactor = 1;
	private const int yFactor = -1;
	private const int zPos = 0;

	public static GenerateGameObjects GetInstance ()
	{
		return instance;
	}

	void Start ()
	{
		instance = this;
	}

	public void GenerateCells (IEnumerable<Cell> cellList)
	{
		GameObject prototype = null;

		foreach (var cell in cellList) {
			if (cell.type == CellType.Brick) {
				prototype = Brick;
			}
			if (cell.type == CellType.Water) {
				prototype = Water;
			}
			if (cell.type == CellType.Stone) {
				prototype = Stone;
			}
			UpdateGameObject (cell.ToString (), prototype, new Vector3 (cell.x * xFactor, cell.y * yFactor, zPos), originalRot);
		}
	}

	public void GenerateTanks (Dictionary<int,Tank> tanks)
	{
		foreach (var tank in tanks) {
			
			int rotation = 0;

			switch (tank.Value.direction) {
			case Direction.Up:
				rotation = 0;
				break;
			case Direction.Right:
				rotation = -90;
				break;
			case Direction.Down:
				rotation = 180;
				break;
			case Direction.Left:
				rotation = 90;
				break;
			}

			UpdateGameObject (tank.ToString (), tank.Value.isPlayer ? PlayerTank : EnemyTank, 
				new Vector3 (tank.Value.x * xFactor, tank.Value.y * yFactor, zPos), 
				Quaternion.Euler (0, 0, rotation));
		}
	}

	public void GenerateCoinPacks (CoinPack coinPack)
	{
		var obj = UpdateGameObject (coinPack.ToString (), CoinPack, new Vector3 (coinPack.x * xFactor, coinPack.y * yFactor, zPos), originalRot);
		Destroy (obj, ((float)coinPack.lifetime) / 1000);
        
	}

	public void DestroyCoinPacks (IEnumerable<CoinPack> coinPacks)
	{
		foreach (var coinPack in coinPacks) {
			Destroy (GameObject.Find (coinPack.ToString()));
		}
	}

	public void GenerateLifePacks (LifePack lifePack)
	{
		var obj = UpdateGameObject (lifePack.ToString (), LifePack, new Vector3 (lifePack.x * xFactor, lifePack.y * yFactor, zPos), originalRot);
		Destroy (obj, ((float)lifePack.lifetime) / 1000);
	}

	public void DestroyLifePacks (IEnumerable<LifePack> lifePacks)
	{
		foreach (var lifePack in lifePacks) {
			Destroy (GameObject.Find (lifePack.ToString()));
		}
	}


	private GameObject UpdateGameObject (string name, GameObject gameObj, Vector3 pos, Quaternion rot)
	{
		var obj = GameObject.Find (name);
		if (obj == null) {
			obj = Instantiate (gameObj, pos, rot);
			obj.name = name;
		} else {
			obj.transform.position = pos;
			obj.transform.rotation = rot;
		}
		return obj;
	}
}