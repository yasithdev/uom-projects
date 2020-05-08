public class Cell
{
    public CellDamage damage;
    public CellType type;
    public int x;
    public int y;

    public Cell(int x, int y, int type, int damage)
    {
        this.x = x;
        this.y = y;
        this.type = (CellType) type;
        this.damage = (CellDamage) damage;
    }

	public override string ToString ()
	{
		return string.Format ("Cell@" + x + "," + y);
	}
}