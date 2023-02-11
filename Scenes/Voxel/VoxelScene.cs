using ArchEngine.Core.ECS;

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