using System;
using System.ComponentModel.DataAnnotations;
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
        
        
        public override void Update()
        {
           
        }
    }
}