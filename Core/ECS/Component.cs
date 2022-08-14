using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ArchEngine.Core.ECS
{
    public interface Component : IDisposable
    {
        [JsonIgnore]
        public GameObject gameObject  { get; set; }
        [JsonIgnore]
        public bool initialized { get; set; }
        
        public void Init();
        public void Start();
        public void Update();


        public static List<Type> GetAllComponents()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                .Where(mytype => mytype.GetInterfaces().Contains(typeof(Component))).Where(type => !type.GetTypeInfo().IsAbstract).ToList();
        }
    }
    
    
}