public class Block
{
    public Block(int id, int meta, bool isOpaque)
    {
        this.id = id;
        this.meta = meta;
        this.isOpaque = isOpaque;
    }

    public Block(int id, bool isOpaque)
    {
        this.id = id;
        this.isOpaque = isOpaque;
    }

    public Block(int id)
    {
        this.id = id;
        this.isOpaque = true;
    }

    public int id;
    public int meta;
    public bool isOpaque;
}
