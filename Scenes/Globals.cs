
public static class Globals
{
    public const int XZMax = 16;
    public const int XZShift = 4;
    
    public const int YMax = 256;
    public const int YShift = 8;
    
    public const int XZMax2 = XZMax * XZMax;
    public const int XZMaxSub1 = XZMax - 1;
    public const int YMaxSub1 = YMax - 1;
    
    public const int XYMaxMul = XZMax * YMax;
    public const int XYMaxMulShift = XZShift + YShift;
    
    public const int XYZTotal = XZMax2 * YMax;
    public const int XYZTotalHexa = XYZTotal * 6;
    
   
    public const int XZYShift = XZShift + YShift;
    public const float tileSize = 4;
}
