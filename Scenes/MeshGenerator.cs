using System;
using OpenTK.Mathematics;

public class MeshGenerator
{

    private static float triangleSize = 1f;
    
    private static readonly Vector3[] v =
    {
        new Vector3 (0, 0, 0) * triangleSize, // vertices 0
        new Vector3 (1, 0, 0) * triangleSize, // vertices 1
        new Vector3 (1, 1, 0) * triangleSize, // vertices 2
        new Vector3 (0, 1, 0) * triangleSize, // vertices 3
        new Vector3 (0, 1, 1) * triangleSize, // vertices 4
        new Vector3 (1, 1, 1) * triangleSize, // vertices 5
        new Vector3 (1, 0, 1) * triangleSize, // vertices 6
        new Vector3 (0, 0, 1) * triangleSize // vertices 7
    };
    
    private static readonly Vector3[] faces =
    {
        v[6], v[5], v[4], v[7],   // back:    from 16 to 19
        v[2], v[3], v[4], v[5],   // top:     from 4 to 7
        v[7], v[4], v[3], v[0],   // left:    from 8 to 11
        v[1], v[2], v[5], v[6],   // right:   from 12 to 15
        
        v[0], v[3], v[2], v[1],   // front:   from 0 to 3
        v[6], v[7], v[0], v[1]    // bottom:  from 20 to 23
        
    };
    
    
    int to1D( float x, float y, float z ) { return (int)(x + y * Globals.XZMax + z * Globals.XZMax * Globals.YMax); }
    int toX(int i) { return i % Globals.XZMax; }
    int toY(int i) { return  (i / Globals.XZMax) % Globals.YMax; }
    int toZ( int i) { return i / (Globals.XZMax * Globals.YMax); }

    public static void generateMesh(ref Block[] blocks, ref Vector3[] vertices, ref int[] triangles, ref Vector2[] UVs)
    {
        int vCount = 0;
        int tCount = 0;
        calculateLength(ref blocks, ref vCount, ref tCount);
        
        vertices = new Vector3[vCount];
        triangles = new int[tCount];
        UVs = new Vector2[vCount];

        Vector2 uv3 = new Vector2(0, 0);
        Vector2 uv1 = new Vector2(1 / Globals.tileSize, 1 / Globals.tileSize);
        Vector2 uv2 = new Vector2(0, 1 / Globals.tileSize);
       
        Vector2 uv0 = new Vector2(1 / Globals.tileSize, 0);
        
        int uvID = 0;
        Vector2 pos;
        bool inRange;
        int vertCount = 0;
        int trisCount = 0;
        
        int tempPosx = 0;
        int tempPosy = 0;
        int tempPosz = 0;
        Vector3i tempVector = new Vector3i();
        for (int i = 0; i < Globals.XYZTotal; i++)
        {
            
            
            if (blocks[i] == null) continue;
            for (int f = 0; f < 6; f++)
            {
                //tempPos.x = i % Globals.XZMax;
                //tempPos.y = ( i / Globals.XZMax ) % Globals.YMax;
                //tempPos.z = i / ( Globals.XZMax * Globals.YMax );
                
                
                switch (f)
                {
                    case 0:
                        tempPosx = i & Globals.XZMaxSub1;
                        tempPosy = ( i >> Globals.XZShift ) & Globals.YMaxSub1;
                        tempPosz = (i >> Globals.XYMaxMulShift) + 1;
                        //tempPos += Vector3Int.forward;
                        break;
                    case 1:
                        tempPosx = i & Globals.XZMaxSub1;
                        tempPosy = (( i >> Globals.XZShift ) & Globals.YMaxSub1) + 1;
                        tempPosz = i >> Globals.XYMaxMulShift;
                        //tempPos += Vector3Int.up;
                        break;
                    case 2:
                        tempPosx = (i & Globals.XZMaxSub1) - 1;
                        tempPosy = ( i >> Globals.XZShift ) & Globals.YMaxSub1;
                        tempPosz = i >> Globals.XYMaxMulShift;
                        //tempPos += Vector3Int.left;
                        break;
                    case 3:
                        tempPosx = (i & Globals.XZMaxSub1) + 1;
                        tempPosy = ( i >> Globals.XZShift ) & Globals.YMaxSub1;
                        tempPosz = i >> Globals.XYMaxMulShift;
                        //tempPos += Vector3Int.right;
                        break;
                    case 4:
                        tempPosx = i & Globals.XZMaxSub1;
                        tempPosy = (i >> Globals.XZShift ) & Globals.YMaxSub1;
                        tempPosz = (i >> Globals.XYMaxMulShift) - 1;
                        // tempPos += Vector3Int.back;
                        break;
                    case 5:
                        tempPosx = i & Globals.XZMaxSub1;
                        tempPosy = (( i >> Globals.XZShift ) & Globals.YMaxSub1) - 1;
                        tempPosz = i >> Globals.XYMaxMulShift;
                        //tempPos += Vector3Int.down;
                        break;
                }

                inRange = (tempPosx < Globals.XZMax && tempPosx > -1 &&
                                tempPosz < Globals.XZMax && tempPosz > -1 &&
                                tempPosy < Globals.YMax && tempPosy > -1);
                if (inRange &&
                    ((blocks[(int) (tempPosx + tempPosy * Globals.XZMax + tempPosz * Globals.XZMax * Globals.YMax)] !=
                        null && blocks[
                                (int) (tempPosx + tempPosy * Globals.XZMax + tempPosz * Globals.XZMax * Globals.YMax)]
                            .isOpaque))) continue;
                tempVector.X = i % Globals.XZMax;
                tempVector.Y = ( i / Globals.XZMax ) % Globals.YMax;
                tempVector.Z = i / ( Globals.XZMax * Globals.YMax );
                        
                vertices[vertCount] = (tempVector + (faces[f*4])).;
                vertices[vertCount+1] = (tempVector + (faces[f*4+1]));
                vertices[vertCount+2] = (tempVector + (faces[f*4+2]));
                vertices[vertCount+3] = (tempVector + (faces[f*4+3]));

                triangles[trisCount] = (0 + vertCount);
                triangles[trisCount+1] = (1 + vertCount);
                triangles[trisCount+2] = (2 + vertCount);
                triangles[trisCount+3] = (vertCount);
                triangles[trisCount+4] = (2 + vertCount);
                triangles[trisCount+5] = (3 + vertCount);
                        
                uvID = BlockLib.blocks[blocks[i].id].uvs[f];
                pos = new Vector2(uvID % (int)Globals.tileSize, uvID / (int)Globals.tileSize) / Globals.tileSize;

                UVs[vertCount] = uv0 + pos;
                UVs[vertCount+1] = uv1 + pos;
                UVs[vertCount+2] = uv2 + pos;
                UVs[vertCount+3] = uv3 + pos;
                        
                vertCount += 4;
                trisCount += 6;
            }

        }

    }
    
    public static void calculateLength(ref Block[] blocks, ref int vertCount, ref int trisCount)
    {
        bool inRange;

        int tempPosx = 0;
        int tempPosy = 0;
        int tempPosz = 0;
        
        for (int i = 0; i < Globals.XYZTotalHexa; i++)
        {
            int oldI = i / 6;
            if (blocks[oldI] == null) continue;
            
            int c = i % 6;
 
            switch (c)
            {
                case 0:
                    tempPosx = oldI & Globals.XZMaxSub1;
                    tempPosy = ( oldI >> Globals.XZShift ) & Globals.YMaxSub1;
                    tempPosz = (oldI >> Globals.XYMaxMulShift) + 1;
                    //tempPos += Vector3Int.forward;
                    break;
                case 1:
                    tempPosx = oldI & Globals.XZMaxSub1;
                    tempPosy = (( oldI >> Globals.XZShift ) & Globals.YMaxSub1) + 1;
                    tempPosz = oldI >> Globals.XYMaxMulShift;
                    //tempPos += Vector3Int.up;
                    break;
                case 2:
                    tempPosx = (oldI & Globals.XZMaxSub1) - 1;
                    tempPosy = ( oldI >> Globals.XZShift ) & Globals.YMaxSub1;
                    tempPosz = oldI >> Globals.XYMaxMulShift;
                    //tempPos += Vector3Int.left;
                    break;
                case 3:
                    tempPosx = (oldI & Globals.XZMaxSub1) + 1;
                    tempPosy = ( oldI >> Globals.XZShift ) & Globals.YMaxSub1;
                    tempPosz = oldI >> Globals.XYMaxMulShift;
                    //tempPos += Vector3Int.right;
                    break;
                case 4:
                    tempPosx = oldI & Globals.XZMaxSub1;
                    tempPosy = (oldI >> Globals.XZShift ) & Globals.YMaxSub1;
                    tempPosz = (oldI >> Globals.XYMaxMulShift) - 1;
                    // tempPos += Vector3Int.back;
                    break;
                case 5:
                    tempPosx = oldI & Globals.XZMaxSub1;
                    tempPosy = (( oldI >> Globals.XZShift ) & Globals.YMaxSub1) - 1;
                    tempPosz = oldI >> Globals.XYMaxMulShift;
                    //tempPos += Vector3Int.down;
                    break;
            }
                
                inRange = (tempPosx < Globals.XZMax && tempPosx > -1 &&
                                tempPosz < Globals.XZMax && tempPosz > -1 &&
                                tempPosy < Globals.YMax && tempPosy > -1);
                if (inRange &&
                    ((blocks[(int) (tempPosx + tempPosy * Globals.XZMax + tempPosz * Globals.XZMax * Globals.YMax)] !=
                        null && blocks[
                                (int) (tempPosx + tempPosy * Globals.XZMax + tempPosz * Globals.XZMax * Globals.YMax)]
                            .isOpaque))) continue;

                        
                vertCount += 4;
                trisCount += 6;
                
        }
        //Debug.Log("Verts: " + vertCount + ", Tris: " + trisCount);

    }
    
}
