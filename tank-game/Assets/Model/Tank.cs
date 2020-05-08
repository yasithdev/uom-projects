using System.Runtime.ConstrainedExecution;

public class Tank
{
	public int coins;
	public Direction direction;
	public int health;
	public int id;
	public bool isPlayer;
	public bool isShot;
	public int points;
	public int x;
	public int y;

	public Tank (int id, bool isPlayer, int x, int y, int direction, bool isShot = false, int health = 0, int coins = 0, int points = 0)
	{
		this.id = id;
		this.isPlayer = isPlayer;
		this.x = x;
		this.y = y;
		this.direction = (Direction)direction;
		this.isShot = isShot;
		this.health = health;
		this.coins = coins;
		this.points = points;
	}

	public override string ToString ()
	{
		return string.Format ("Tank#" + id);
	}

	/// <summary>
	/// 	Returns the next state after a client request. if that state can exist. Else return null
	/// </summary>
	/// <returns>The next state.</returns>
	/// <param name="request">Request.</param>
	public Tank GetNextState (ClientRequest request)
	{
		switch (request) {
		case ClientRequest.Shoot:
			return new Tank (id, isPlayer, x, y, (int)direction, true, health, coins, points);
		case ClientRequest.Up:
			if (direction == Direction.Up)
				return y + 1 >= Constants.MAP_SIZE ? null : new Tank (id, isPlayer, x, y + 1, (int)direction, isShot, health, coins, points);
			else
				return new Tank (id, isPlayer, x, y, (int)Direction.Up, isShot, health, coins, points);
		case ClientRequest.Down:
			if (direction == Direction.Down)
				return y - 1 < 0 ? null : new Tank (id, isPlayer, x, y - 1, (int)direction, isShot, health, coins, points);
			else
				return new Tank (id, isPlayer, x, y, (int)Direction.Down, isShot, health, coins, points);
		case ClientRequest.Left:
			if (direction == Direction.Left)
				return x - 1 < 0 ? null : new Tank (id, isPlayer, x - 1, y, (int)direction, isShot, health, coins, points);
			else
				return new Tank (id, isPlayer, x, y, (int)Direction.Left, isShot, health, coins, points);
		case ClientRequest.Right:
			if (direction == Direction.Right)
				return x + 1 >= Constants.MAP_SIZE ? null : new Tank (id, isPlayer, x + 1, y, (int)direction, isShot, health, coins, points);
			else
				return new Tank (id, isPlayer, x, y, (int)Direction.Right, isShot, health, coins, points);
		default:
			return null;
		}
	}

	public bool WillTankChangeLocation (ClientRequest request)
	{
		return (request == ClientRequest.Down && direction == Direction.Down) || (request == ClientRequest.Left && direction == Direction.Left) ||
		(request == ClientRequest.Right && direction == Direction.Right) || (request == ClientRequest.Up && direction == Direction.Up);
	}
}