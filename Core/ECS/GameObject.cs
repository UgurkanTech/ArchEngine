using System;
using System.Collections;
using System.Collections.Generic;
using OpenTK.Mathematics;

namespace ArchEngine.Core.ECS
{
    public class GameObject : Component
    {

        public String name;
        public Matrix4 Transform { get; set; }
        public List<Component> _components = new List<Component>(5);

        private bool initialized = false;
        
        public GameObject(String name = "GameObject")
        {
            this.name = name;
            Transform = Matrix4.Identity;
        }

        public T GetComponent<T>()
        {
            foreach (var component in _components)
            {
                if (component.GetType() == typeof(T))
                {
                    return ((T)component);
                }
            }
            return default(T);
        }

        public bool HasComponent<T>()
        {
            foreach (var component in _components)
            {
                if (component.GetType() == typeof(T))
                {
                    return true;
                }
            }
            return false;
        }
        
        public void AddComponent(Component component)
        {
            _components.Add(component);
            component.gameObject = this;
        }
        
        public void RemoveComponent<T>()
        {
            for(int i = 0; i < _components.Count; i++)
            {
                if (_components[i].GetType() == typeof(T))
                {
                    _components.RemoveAt(i);
                    return;
                }
            }
        }


        public GameObject gameObject { get; set; }

        public void Init()
        {
            if (!initialized)
            {
                foreach (var c in _components)
                {
                    c.Init();
                }
                initialized = true;
            }
        }
        
        public void Start()
        {
            foreach (var c in _components)
            {
                c.Start();
            }
        }

        public void Update()
        {
            foreach (var c in _components)
            {
                c.Update();
            }
        }
        

        
    }
}