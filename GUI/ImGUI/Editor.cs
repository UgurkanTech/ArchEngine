using System;
using System.Text;
using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Utils;
using ImGuiNET;
using OpenTK.Mathematics;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;

namespace ArchEngine.GUI.ImGUI
{
    public class Editor
    {
        public static void DrawEditor()
        {
            DrawHierarchy();
        }
        
        static ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.OpenOnArrow |
                                   ImGuiTreeNodeFlags.SpanFullWidth | ImGuiTreeNodeFlags.DefaultOpen;

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
                    flags |= ImGuiTreeNodeFlags.Selected;
                
                if (ImGui.TreeNodeEx(gameObject.name, flags))
                {
                    if (selected == index) 
                        flags &= ~ImGuiTreeNodeFlags.Selected;
                    
                    if (ImGui.IsItemHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                    {
                        selected = index;
                        selectedGameobject = gameObject;
                        Console.WriteLine(selectedGameobject.name);
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
            }
            else
            {
                if (selected == index)
                    nodeFlags |= ImGuiTreeNodeFlags.Selected;

                if (ImGui.TreeNodeEx(gameObject.name, nodeFlags)){
                    if (selected == index)
                        nodeFlags &= ~ImGuiTreeNodeFlags.Selected;
                    
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
            ImGui.Begin("Hierarchy",  ImGuiWindowFlags.UnsavedDocument);
            
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
  
        
        static byte[] nameBuffer = new byte[100];
        public static void DrawInspector()
        {
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(400,100), ImGuiCond.Once);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(325, 170), ImGuiCond.Once);
            ImGui.Begin("Inspector");

            if (ImGui.TreeNodeEx("Transform", flags) && selectedGameobject != null)
            {
                Matrix4 mat = Matrix4.Identity;
                
                mat = selectedGameobject.Transform;
                nameBuffer = Encoding.ASCII.GetBytes(selectedGameobject.name);
                
            
            
            
                ImGui.Text("Name");
                ImGui.SameLine();

                unsafe
                {
                    if (ImGui.InputText("N", nameBuffer, 100, ImGuiInputTextFlags.CallbackAlways | ImGuiInputTextFlags.AlwaysOverwrite, Callback)) ;
                }
            

                Vector3 pos = mat.ExtractTranslation().ToSystemVector3();
                Quaternion rot = mat.ExtractRotation();
                Vector3 scal = mat.ExtractScale().ToSystemVector3();


                Vector3 roteuler = rot.ToEulerAngles().RadiansToAngles().ToSystemVector3();
            
                ImGui.Text("Position");
                ImGui.SameLine();
                ImGui.InputFloat3("P", ref pos);
                ImGui.Text("Rotation");
                ImGui.SameLine();
                ImGui.InputFloat3("R", ref roteuler);
                ImGui.Text("Scale   ");
                ImGui.SameLine();
                ImGui.InputFloat3("S", ref scal);
                
                mat = Matrix4.CreateScale( scal.ToOpenTkVector3().GetNonZero());
                mat *= Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(roteuler.ToOpenTkVector3().DegreesToRadians()));
                mat *= Matrix4.CreateTranslation(pos.ToOpenTkVector3());
                
                selectedGameobject.Transform = mat;
                
                

            }
            
            
            ImGui.End();
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