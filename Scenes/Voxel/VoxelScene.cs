using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArchEngine.Scenes.Voxel
{
    public class VoxelScene : Scene
    {
        public VoxelScene()
        {
            GameObject world = new GameObject();
            world.AddComponent(new World());
            AddGameObject(world);
            
        }
        
    }
}