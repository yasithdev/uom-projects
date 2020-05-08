using System;

public class CoinPack
{
    public int lifetime;
    public int value;
    public int x;
    public int y;
	public DateTime creationTime;

    public CoinPack(int x, int y, int lifetime, int value)
    {
        this.x = x;
        this.y = y;
        this.lifetime = lifetime;
        this.value = value;
		this.creationTime = DateTime.Now;
    }

	public override string ToString ()
	{
		return string.Format ("CoinPack@" + x + "," + y);
	}

	public bool isExpired(){
		return DateTime.Now.Subtract (creationTime).Milliseconds >= lifetime;
	}
}