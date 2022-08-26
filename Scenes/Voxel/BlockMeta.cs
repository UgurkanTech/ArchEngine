
public class BlockMeta
{
    public BlockMeta(int id, string name, int meta, string desc, bool isOpaque, int uvTop, int uvBottom, int uvBack, int uvFront, int uvLeft, int uvRight)
    {
        this.id = id;
        this.name = name;
        this.meta = meta;
        this.desc = desc;
        this.isOpaque = isOpaque;
        this.uvTop = uvTop;
        this.uvBottom = uvBottom;
        this.uvBack = uvBack;
        this.uvFront = uvFront;
        this.uvLeft = uvLeft;
        this.uvRight = uvRight;
        
        uvs[0] = uvBack;
        uvs[1] = uvTop;
        uvs[2] = uvLeft;
        uvs[3] = uvRight;
        uvs[4] = uvFront;
        uvs[5] = uvBottom;
    }


    public BlockMeta(int id, string name, int meta, string desc, bool isOpaque, int uv)
    {
        this.id = id;
        this.name = name;
        this.meta = meta;
        this.desc = desc;
        this.isOpaque = isOpaque;
        this.uvTop = uv;
        this.uvBottom = uv;
        this.uvBack = uv;
        this.uvFront = uv;
        this.uvLeft = uv;
        this.uvRight = uv;

        uvs[0] = uv;
        uvs[1] = uv;
        uvs[2] = uv;
        uvs[3] = uv;
        uvs[4] = uv;
        uvs[5] = uv;
    }

    public int id;
    public string name;
    public int meta;
    public string desc;
    public bool isOpaque;
    
    public int uvTop;
    public int uvBottom;
    public int uvBack;
    public int uvFront;
    public int uvLeft;
    public int uvRight;
    public int[] uvs = new int[6];

}
