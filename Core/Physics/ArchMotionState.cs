using System;
using BulletSharp;
using BulletSharp.Math;

namespace ArchEngine.Core.Physics
{
    public class ArchMotionState : MotionState
    {
        public Matrix transform;
        
        public ArchMotionState(Matrix initialTransform)
        {
            transform = initialTransform;
        }
        public override void GetWorldTransform(out Matrix worldTrans)
        {
            worldTrans = transform;
            
        }

        public override void SetWorldTransform(ref Matrix worldTrans)
        {
            transform = worldTrans;
            //Console.WriteLine(transform.ScaleVector);
        }
        
        public void SetWorldTransform(Matrix worldTrans)
        {
            transform = worldTrans;
        }
    }
}