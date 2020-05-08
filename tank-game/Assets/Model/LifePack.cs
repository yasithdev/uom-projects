using System;

public class LifePack
{
    public int lifetime;
    public int x;
    public int y;
	public DateTime creationTime;

    public LifePack(int x, int y, int lifetime)
    {
        this.x = x;
        this.y = y;
        this.lifetime = lifetime;
		this.creationTime = DateTime.Now;
    }

	public override string ToString ()
	{
		return string.Format ("LifePack@" + x + "," + y);
	}


	public bool isExpired(){
		return DateTime.Now.Subtract (creationTime).Milliseconds >= lifetime;
	}
}