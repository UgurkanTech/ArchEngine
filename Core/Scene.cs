using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;

namespace ArchEngine.Core.ECS
{
    
    public class Scene : IDisposable
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        
        public List<GameObject> gameObjects = new List<GameObject>(5);
        private bool isRunning = false;
        
        public void Init()
        {
            
            foreach (var c in gameObjects)
            {
                _log.Info("Initializing gameobject: " + c.name);
                c.Init();
            }
            
        }
        
        public void Start()
        {
            foreach (var c in gameObjects)
            {
                _log.Info("Starting gameobject: " + c.name);
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
        
        public void FixedUpdate()
        {
            foreach (var c in gameObjects)
            {
                c.FixedUpdate();
            }
        }
        
        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            if (isRunning)
            {
                gameObject.Init();
                gameObject.Start();
            }
   
        }
        
        public void MoveGameObjecTo(GameObject gameObject, int index)
        {
            if (gameObject.parent == null)
            {
                RemoveGameObject(gameObject);
            }
            else
            {
                gameObject.parent.RemoveChild(gameObject);
            }

            if (index < gameObjects.Count)
            {
                gameObjects.Insert(index, gameObject);
            }
            else
            {
                gameObjects.Add(gameObject);
            }
            

        }
        
        public void RemoveGameObject(GameObject child)
        {
            for(int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].GetType() == typeof(GameObject) && gameObjects[i].Equals(child))
                {
                    gameObjects.RemoveAt(i);
                    return;
                }
            }
        }

        public GameObject GameObjectFind(String name)
        {
            foreach (var gameObject in gameObjects)
            {
                if (gameObject.name.Equals(name))
                    return gameObject;
            }

            return null;
        }
        
        public static void SpawnObject()
        {
            GameObject go = new GameObject("Gameobject");
                        
            Window.activeScene.AddGameObject(go);
        }

        public void Dispose()
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.Dispose();
            }
            gameObjects.Clear();
        }
    }
}