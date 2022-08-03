using System;
using System.Numerics;
using System.Reflection;
using System.Text;
using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Utils;
using ImGuiNET;
using OpenTK.Mathematics;
using Quaternion = OpenTK.Mathematics.Quaternion;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;
using Vector4 = System.Numerics.Vector4;

namespace ArchEngine.GUI.ImGUI
{
    public class Editor
    {
        public static void DrawEditor()
        {
            DrawHierarchy();
        }

        private static ImGuiTreeNodeFlags flagsCur;
        
        static ImGuiTreeNodeFlags flagsS = ImGuiTreeNodeFlags.OpenOnArrow |
                                   ImGuiTreeNodeFlags.SpanFullWidth | ImGuiTreeNodeFlags.DefaultOpen | ImGuiTreeNodeFlags.Selected;

        private static ImGuiTreeNodeFlags flagsNotS = ImGuiTreeNodeFlags.OpenOnArrow |
                                                   ImGuiTreeNodeFlags.SpanFullWidth | ImGuiTreeNodeFlags.DefaultOpen; 
        
        static ImGuiTreeNodeFlags nodeFlagsS = ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen |
                                              ImGuiTreeNodeFlags.SpanFullWidth | ImGuiTreeNodeFlags.Selected;
        
        static ImGuiTreeNodeFlags nodeFlags = ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen |
                                              ImGuiTreeNodeFlags.SpanFullWidth;
        
        private static int selected = -1;
        private static int index;

        public static GameObject selectedGameobject;
        
        private static void AddToHierarchyRecursively(GameObject gameObject)
        {
            index++;
            if (gameObject.HasComponent<GameObject>())
            {
                if (selected == index)
                    flagsCur = flagsS;
                else
                    flagsCur = flagsNotS;
                
                
                if (ImGui.TreeNodeEx(gameObject.name, flagsCur))
                {
                    if (ImGui.IsItemHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                    {
                        selected = index;
                        selectedGameobject = gameObject;
                    }
                    gameObject._components.ForEach(component =>
                    {
                        if (component.GetType() == typeof(GameObject))
                        {
                            
                            AddToHierarchyRecursively(component as GameObject);
                        }
                    });
                    if (!ImGui.IsItemToggledOpen())
                    {
                        ImGui.TreePop();
                    }
                }
                else
                {
                    if (ImGui.IsItemHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                    {
                        selected = index;
                        selectedGameobject = gameObject;
                    }
                }
            }
            else
            {
                if (selected == index)
                    flagsCur = nodeFlagsS;
                else
                    flagsCur = nodeFlags;

                if (ImGui.TreeNodeEx(gameObject.name, flagsCur)){

                    if (ImGui.IsItemHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                    {
                        selected = index;
                        selectedGameobject = gameObject;
                        Console.WriteLine(selectedGameobject.name);
                    }
                }
            }
        }

        public static void DrawHierarchy()
        {
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(25,100), ImGuiCond.Once);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(150, 300), ImGuiCond.Once);
            ImGui.Begin("Hierarchy");
            
            index = 0;
            
            if (!ImGui.IsItemHovered() && ImGui.IsWindowHovered() && !ImGui.IsAnyItemHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
            {
                selected = -1;
                selectedGameobject = null;
                
            }
            
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, Vector2.Zero);
            Window.activeScene.gameObjects.ForEach(AddToHierarchyRecursively);
            ImGui.PopStyleVar();
            
            //ImGui.SliderFloat("Scale", ref Window.f, 0.0f, 1.0f);
            ImGui.End();

            DrawInspector();

        }
  
        static float fs = 4;
        static byte[] nameBuffer = new byte[100];
        public static void DrawInspector()
        {
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(400,100), ImGuiCond.Once);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(370, 220), ImGuiCond.Once);
            ImGui.Begin("Inspector");

            nodeFlags &= ~ImGuiTreeNodeFlags.Selected;
            
            if (selectedGameobject != null)
            {
                Matrix4 mat = Matrix4.Identity;
                
                mat = selectedGameobject.Transform;
                nameBuffer = Encoding.ASCII.GetBytes(selectedGameobject.name);
                
            
            
                //ImGui.Columns(2, "naming", false);
               // ImGui.SetColumnWidth(0, 100);
                
                ImGui.TextColored(new Vector4(0,200,0,255), "          Name    ");
                
                ImGui.SameLine();
                //ImGui.NextColumn();

                unsafe
                {
                    if (ImGui.InputText("##Name", nameBuffer, 100, ImGuiInputTextFlags.CallbackAlways | ImGuiInputTextFlags.EnterReturnsTrue, Callback)) ;
                }
                ImGui.Spacing();
                ImGui.Separator();
                ImGui.Columns(1);
                if (ImGui.TreeNodeEx("Transform", flagsNotS))
                {
                    Vector3 pos = mat.ExtractTranslation().ToSystemVector3();
                    Quaternion rot = mat.ExtractRotation();
                    Vector3 scal = mat.ExtractScale().ToSystemVector3();


                    Vector3 roteuler = rot.ToEulerAngles().RadiansToAngles().ToSystemVector3();
                
               
                    ImGui.Columns(4,"transforms", false);
                    //mGui.SetColumnWidth(0, 90);

                    ImGuiStyleVar styleVar = ImGuiStyleVar.WindowPadding;
                    
                    ImGui.PushStyleVar(styleVar, 0.1f);
                    
                    
                    ImGui.PushItemWidth(-100.0f);
                    ImGui.Text("Position");
                    ImGui.NextColumn();
                    ImGui.DragFloat("X##POS", ref pos.X, 0.01f);
                    ImGui.NextColumn();
                    ImGui.DragFloat("Y##POS", ref pos.Y, 0.01f);
                    ImGui.NextColumn();
                    ImGui.DragFloat("Z##POS", ref pos.Z, 0.01f);
                    ImGui.NextColumn();
                    ImGui.Text("Rotation");
                    ImGui.NextColumn();
                    ImGui.DragFloat("X##ROT", ref roteuler.X, 0.01f);
                    ImGui.NextColumn();
                    ImGui.DragFloat("Y##ROT", ref roteuler.Y, 0.01f);
                    ImGui.NextColumn();
                    ImGui.DragFloat("Z##ROT", ref roteuler.Z, 0.01f);
                    ImGui.NextColumn();
                    ImGui.Text("Scale");
                    ImGui.NextColumn();
                    ImGui.DragFloat("X##SCL", ref scal.X, 0.01f);
                    ImGui.NextColumn();
                    ImGui.DragFloat("Y##SCL", ref scal.Y, 0.01f);
                    ImGui.NextColumn();
                    ImGui.DragFloat("Z##SCL", ref scal.Z, 0.01f);
                    
                    ImGui.PopStyleVar();
                
                    mat = Matrix4.CreateScale( scal.ToOpenTkVector3().GetNonZero());
                    mat *= Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(roteuler.ToOpenTkVector3().DegreesToRadians()));
                    mat *= Matrix4.CreateTranslation(pos.ToOpenTkVector3());
                
                    selectedGameobject.Transform = mat;
                }

            }

            if (ImGui.IsAnyItemHovered() && ImGui.IsMouseDragging(ImGuiMouseButton.Left))
            {
                Window.lockCursor = true;
            }
            else if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
            {
                Window.lockCursor = false;
            }
             
            


            ImGui.End();
        }

        [MyAttribute] private static int a = 5;
        
        [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
        public class MyAttribute : Attribute
        {
            public MyAttribute()
            {
                Console.WriteLine("initiated");
            }
            
            
            public override string ToString()
            {
                return "adsa";
            }
        }

        public static void ScanAttiributes(object obj)
        {
            MemberInfo[] members = obj.GetType().GetMethods();
            foreach (MemberInfo m in members)
            {
                if (m.MemberType == MemberTypes.Method)
                {
                    MethodInfo p = m as MethodInfo;
                    object[] attribs = p.GetCustomAttributes(false);
                    foreach (object attr in attribs)
                    {
                        MyAttribute v = attr as MyAttribute;
                        if (v != null)
                            Console.WriteLine(v.ToString());
                    }
                }
            }
            
        }
        
        
        

        private static unsafe int Callback(ImGuiInputTextCallbackData* data)
        {
            if (ImGui.IsKeyPressed(ImGuiKey.Enter))
            {
                selectedGameobject.name = Encoding.ASCII.GetString(data->Buf, data->BufTextLen);
            }

            return -1;
        }
    }
}