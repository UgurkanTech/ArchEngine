using System.ComponentModel.DataAnnotations;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Utils;
using ArchEngine.GUI.Editor;
using OpenTK.Mathematics;

namespace ArchEngine
{
    public class ACustomScript : Script
    {
        [Inspector] public float myFloat1;
        [Inspector] public float myFloat2;
        [Inspector] public Vector3 myVector3;
        [Inspector][Range(0, 20)] public int myIntSlider;
        [Inspector] public bool myCheckbox;
        [Inspector] public Color4 myColor;
        public override void Update() {}
        public override void Start() {}
        public override void FixedUpdate()
        {
            //Matrix4 mat = Matrix4.Identity;
            
            
            //mat = Matrix4.CreateScale( gameObject.Transform.ExtractScale());
            //mat *= Matrix4.CreateRotationZ(Runtime.CurrentRuntime);
            //mat *= Matrix4.CreateTranslation(gameObject.Transform.ExtractTranslation());

            var t = gameObject.Transform;
                
            t.RotateY(1);

            gameObject.Transform = t;
            
            //gameObject.Transform = mat;
            myFloat1 = Runtime.CurrentRuntime;
        }
    }
}

public static class Runtime
{
    static Runtime() 
    {
        var ThisProcess = System.Diagnostics.Process.GetCurrentProcess(); LastSystemTime = (long)(System.DateTime.Now - ThisProcess.StartTime).TotalMilliseconds; ThisProcess.Dispose();
        StopWatch = new System.Diagnostics.Stopwatch(); StopWatch.Start();
    }
    private static long LastSystemTime;
    private static System.Diagnostics.Stopwatch StopWatch;

    //Public.
    public static float CurrentRuntime { get { return (StopWatch.ElapsedMilliseconds + LastSystemTime) / 1000f; } }
}