public static class BlockGenerator
{
    static FastNoise noise = new FastNoise("Simplex");
    static Block b1 = new Block(1);
    static Block b2 = new Block(2);
    static Block b3 = new Block(3);
    static Block b4= new Block(4, true);
    public static void generateBlocks(ref Block[] blocks, float x, float y, float z)
    {
        blocks = new Block[Globals.XYZTotal];
        
        int n = 0;
        int height = 0;

        float a = 0, b = 0;
        
        for (int i = 0; i < Globals.XYZTotal; i++)
        {
            a = ((i % Globals.XZMax) + x);
            b = ((i / ( Globals.XYMaxMul)) + z);
            
            height = ((i / Globals.XZMax) % Globals.YMax);
            n = (int)((noise.GenSingle2D(a* 0.02f, b * 0.02f , 128) + 1) * 8 + 64);
            n += (int)((noise.GenSingle2D(a* 0.005f, b * 0.005f , 128) + 1) * 16 + 0);

            //n = Random.Range(0, 10);
            //n = 10;

            if (n == height )
            { 
                blocks[i] = b3;
                //blocks[i].id = 1;
                //blocks[i].isOpaque = true;
            }
            else if (n > height + 4)
            {
                blocks[i] = b1;
            }
            else if (n > height)
            {
                blocks[i] = b2;
            }
            else if (80 > height && n < height)
            {
                blocks[i] = b4;
            }
        }
    }
}
