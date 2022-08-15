using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Drawing;
using ArchEngine.Core.ECS;
using ArchEngine.GUI.Editor;
using OpenTK.Mathematics;

namespace ArchEngine
{
    public class ACustomScript : Script
    {
        
        [Inspector] public float myFloat;
        [Inspector] public Vector3 myVector3;
        [Inspector][Range(0, 20)] public int myIntSlider;
        [Inspector] public bool check;
        [Inspector] public Color4 color;

        public override void Update()
        {

        }

       
        public override void Start()
        {
           
        }

        public override void FixedUpdate()
        {
            gameObject.Transform = Matrix4.Identity * Matrix4.CreateRotationY(Runtime.CurrentRuntime);
            myFloat = Runtime.CurrentRuntime;
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