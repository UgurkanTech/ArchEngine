using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Text;
using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Utils;
using ArchEngine.GUI.Editor.Windows;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Quaternion = OpenTK.Mathematics.Quaternion;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;
using Vector4 = System.Numerics.Vector4;

namespace ArchEngine.GUI.Editor.Windows
{
    public class Hierarchy
    {
        private static int selected = -1;
        private static int index;
        
        private static ImGuiTreeNodeFlags flagsCur;
        
        static ImGuiTreeNodeFlags flagsS = ImGuiTreeNodeFlags.OpenOnArrow |
                                           ImGuiTreeNodeFlags.SpanFullWidth | ImGuiTreeNodeFlags.DefaultOpen | ImGuiTreeNodeFlags.Selected;

        private static ImGuiTreeNodeFlags flagsNotS = ImGuiTreeNodeFlags.OpenOnArrow |
                                                      ImGuiTreeNodeFlags.SpanFullWidth | ImGuiTreeNodeFlags.DefaultOpen; 
        
        static ImGuiTreeNodeFlags nodeFlagsS = ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen |
                                               ImGuiTreeNodeFlags.SpanFullWidth | ImGuiTreeNodeFlags.Selected;
        
        static ImGuiTreeNodeFlags nodeFlags = ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen |
                                              ImGuiTreeNodeFlags.SpanFullWidth;
        
        public static void Draw()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.IndentSpacing, 10f);
            
            
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(25,100), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(150, 300), ImGuiCond.FirstUseEver);
            ImGui.Begin("Hierarchy");
            
            index = 0;
            
            if (!ImGui.IsItemHovered() && ImGui.IsWindowHovered() && !ImGui.IsAnyItemHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
            {
                selected = -1;
                Editor.selectedGameobject = null;
                
            }
            
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, Vector2.Zero);
            Window.activeScene.gameObjects.ForEach(AddToHierarchyRecursively);
            ImGui.PopStyleVar();
            
            //ImGui.SliderFloat("Scale", ref Window.f, 0.0f, 1.0f);
            ImGui.End();

            ImGui.PopStyleVar();

        }
        
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
                        Editor.selectedGameobject = gameObject;
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
                        Editor.selectedGameobject = gameObject;
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
                        Editor.selectedGameobject = gameObject;
                        Console.WriteLine(Editor.selectedGameobject.name);
                    }
                }
            }
        }
    }
}