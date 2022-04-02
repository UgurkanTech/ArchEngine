using System.Collections.Generic;

namespace ArchEngine.Core.ECS
{
    public class Scene
    {
        public List<GameObject> gameObjects = new List<GameObject>(5);
        private bool isRunning = false;
        
        public void Init()
        {
            foreach (var c in gameObjects)
            {
                c.Init();
            }
            
        }
        
        public void Start()
        {
            foreach (var c in gameObjects)
            {
                c.Start();
            }

            isRunning = true;
        }

        public void Update()
        {
            foreach (var c in gameObjects)
            {
                c.Update();
            }
        }
        
        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            if (isRunning)
                gameObject.Start();
            

        }
    }
}