using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ArchEngine.GUI.Editor
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class InspectorAttribute : Attribute
    {
        public string name;
        public InspectorAttribute(String name = null)
        {
            this.name = name;
        }

    }
    
    public class Attributes
    {
        public static List<FieldInfo> ScanFields(object instance)
        {
            Type type = instance.GetType();

            FieldInfo[] types = type.GetFields();
           
            List<FieldInfo> result = new List<FieldInfo>(5);
            
            foreach(FieldInfo mInfo in types) {
                foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo)) {

                    if (attr.GetType() == typeof(InspectorAttribute))
                    {
                        //Console.WriteLine("Field {0} with name {1} has {2}", mInfo.FieldType.Name, mInfo.Name, mInfo.GetValue(instance).ToString());
                        result.Add(mInfo);
                    }
                        
                }

            }

            return result;
        }
        
        public static List<PropertyInfo> ScanProperties(object instance)
        {
            Type type = instance.GetType();

            PropertyInfo[] types = type.GetProperties();
           
            List<PropertyInfo> result = new List<PropertyInfo>(5);
            
            foreach(PropertyInfo mInfo in types) {
                foreach (Attribute attr in Attribute.GetCustomAttributes(mInfo)) {

                    if (attr.GetType() == typeof(InspectorAttribute))
                    {
                        //Console.WriteLine("Field {0} with name {1} has {2}", mInfo.FieldType.Name, mInfo.Name, mInfo.GetValue(instance).ToString());
                        result.Add(mInfo);
                    }
                        
                }

            }

            return result;
        }
    }
}