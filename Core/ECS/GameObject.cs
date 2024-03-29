﻿using System;
using System.Collections.Generic;
using ArchEngine.Core.Rendering.Textures;
using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace ArchEngine.Core.ECS
{
    public class GameObject
    {

        public String name;
        
        public Matrix4 Transform { get; set; }
        public List<Component> _components = new List<Component>(5);
        public List<GameObject> _childs = new List<GameObject>(5);
        [JsonIgnore]
        public bool initialized = false;

        
        public GameObject parent = null;

        public bool isActive = true;

        public Vector3 getWorldPosition()
        {
            return getWorldTransform().ExtractTranslation();
        }
        
        public Matrix4 getWorldTransform()
        {
            Matrix4 world = GetWorldTransform(this);
            return world;
        }

        public static Matrix4 GetWorldTransform(GameObject gameObject)
        {
            Matrix4 transform = gameObject.Transform;

            // If this gameobject doesn't have a parent, its world transform is just its local transform
            if (gameObject.parent == null)
            {
                return transform;
            }

            // Otherwise, recursively compute the parent's world transform and combine it with the local transform
            Matrix4 parentWorldTransform = GetWorldTransform(gameObject.parent);
            return   transform * parentWorldTransform;
        }

        public bool GetActive()
        {
            return GetActive(this);
        }
        
        private bool GetActive(GameObject gameObject)
        {
            bool active = gameObject.isActive;
            
            if (gameObject.parent == null)
            {
                return active;
            }

            bool parentActive = GetActive(gameObject.parent);
            return parentActive && active;
        }
        
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
        public bool HasComponent(Type type)
        {
            foreach (var component in _components)
            {
                if (component.GetType() ==type)
                {
                    return true;
                }
            }
            return false;
        }
        public void AddComponent(Component component)
        {
            if (HasComponent(component.GetType()))
            {
                Window._log.Info("GameObject already has the same component.");
                return;
            }
                
            component.gameObject = this;
            _components.Add(component);
        }

        public void AddChild(GameObject child)
        {
            _childs.Add(child);
            child.parent = this;
        }        
        public void RemoveComponent<T>()
        {
            for(int i = 0; i < _components.Count; i++)
            {
                if (_components[i].GetType() == typeof(T))
                {
                    _components[i].Dispose();
                    _components.RemoveAt(i);
                    return;
                }
            }
        }
        public void RemoveComponent(Component component)
        {
            for(int i = 0; i < _components.Count; i++)
            {
                if (_components[i].GetType() == component.GetType())
                {
                    _components[i].Dispose();
                    _components.RemoveAt(i);
                    return;
                }
            }
        }
        
        public void RemoveChild(GameObject child)
        {
            for(int i = 0; i < _childs.Count; i++)
            {
                if (_childs[i].Equals(child))
                {
                    _childs[i].Dispose();
                    _childs.RemoveAt(i);
                    return;
                }
            }
        }


       

        public void Init()
        {
            if (!initialized)
            {
                foreach (var c in _components)
                {
                    c.gameObject = this;
                    c.Init();
                }

                foreach (var child in _childs)
                {
                    child.Init();
                }
                initialized = true;
            }
        }
        
        public void Start()
        {
            if (!isActive)
                return;
            foreach (var c in _components)
            {
                c.Start();
            }
            foreach (var child in _childs)
            {
                child.Start();
            }
        }

        public void Update()
        {
            if (!isActive)
                return;
            foreach (var c in _components)
            {
                c.Update();
            }
            foreach (var child in _childs)
            {
                child.Update();
            }
        }

        public void FixedUpdate()
        {
            if (!isActive)
                return;
            foreach (var c in _components)
            {
                c.FixedUpdate();
            }
            foreach (var child in _childs)
            {
                child.FixedUpdate();
            }
        }
        public void Dispose()
        {
            foreach (var component in _components)
            {
                component.Dispose();
                component.gameObject = null;

            }
            _components.Clear();
            foreach (var child in _childs)
            {
                child.Dispose();
                child.parent = null;
            }
            _childs.Clear();
        }
    }
}