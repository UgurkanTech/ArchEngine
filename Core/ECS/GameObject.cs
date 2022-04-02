using System.Collections;
using System.Collections.Generic;
namespace ArchEngine.Core.ECS
{
    public class GameObject
    {
        private List<Component> _components = new List<Component>(5);
        
        public Component GetComponent<T>()
        {
            foreach (var component in _components)
            {
                if (component.GetType() == typeof(T))
                {
                    return component;
                }
            }
            return null;
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
        
        
        public void Init()
        {
            foreach (var c in _components)
            {
                c.Init();
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