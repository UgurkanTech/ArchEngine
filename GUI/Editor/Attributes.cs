using System;
using System.Collections.Generic;
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

    public static class MemberInfoUtil
    {
        public static object GetValue(this MemberInfo memberInfo, object forObject)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(forObject);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(forObject);
                default:
                    throw new NotImplementedException();
            }
        } 
        
        public static Type GetVariableType(this MemberInfo memberInfo)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).PropertyType;
                default:
                    throw new NotImplementedException();
            }
        } 
        
        public static void SetValue(this MemberInfo memberInfo, object forObject, object value)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    ((FieldInfo)memberInfo).SetValue(forObject, value);
                    break;
                case MemberTypes.Property:
                    ((PropertyInfo)memberInfo).SetValue(forObject, value);
                    break;
                default:
                    throw new NotImplementedException();
            }
        } 
    }
    
    public class Attributes
    {
        public static List<MemberInfo> ScanFields(object instance)
        {
            Type type = instance.GetType();

            FieldInfo[] types = type.GetFields();
           
            List<MemberInfo> result = new List<MemberInfo>(5);
            
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
        
        public static List<MemberInfo> ScanProperties(object instance)
        {
            Type type = instance.GetType();

            PropertyInfo[] types = type.GetProperties();
           
            List<MemberInfo> result = new List<MemberInfo>(5);
            
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