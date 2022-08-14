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
        
    }
}