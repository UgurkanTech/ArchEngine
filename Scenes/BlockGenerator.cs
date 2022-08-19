public static class BlockGenerator
{
    static FastNoise noise = new FastNoise("Simplex");
    

    public static Block[] generateBlocks(float x, float y, float z)
    {
        Block[] blocks = new Block[Globals.XYZTotal];
        
        int n = 0;
        int height = 0;
        for (int i = 0; i < Globals.XYZTotal; i++)
        {
            height = ((i / Globals.XZMax) % Globals.YMax);
            n = (int)((noise.GenSingle2D(((i % Globals.XZMax) + x)* 0.02f, ((i / ( Globals.XYMaxMul)) + z) * 0.02f , 128) + 1) * 8 + 64);
            n += (int)((noise.GenSingle2D(((i % Globals.XZMax) + x)* 0.005f, ((i / ( Globals.XYMaxMul)) + z) * 0.005f , 128) + 1) * 16 + 0);

            //n = Random.Range(0, 10);
            //n = 10;

            if (n == height )
            { 
                blocks[i] = new Block(3);
                //blocks[i].id = 1;
                //blocks[i].isOpaque = true;
            }
            else if (n > height + 4)
            {
                blocks[i] = new Block(1);
            }
            else if (n > height)
            {
                blocks[i] = new Block(2);
            }
            else if (80 > height && n < height)
            {
                blocks[i] = new Block(4, true);
            }

            
        }

        
        
        return blocks;
        
    }
}
