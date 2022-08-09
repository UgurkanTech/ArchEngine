using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
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
            ImGui.PushStyleVar(ImGuiStyleVar.IndentSpacing, 13f);
           
            
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(25,100), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(150, 300), ImGuiCond.FirstUseEver);
            ImGui.Begin("Hierarchy");

            if (ImGui.BeginPopupContextWindow())
            {
                if (ImGui.MenuItem("Add New Gameobject"))
                {
                    Material mat = new Material();
                    mat.LoadTextures("Resources/Textures/wall");
                    mat.Shader = ShaderManager.PbrShader;
            
                    MeshRenderer mr = new MeshRenderer();
                    mr.mesh = new Cube();
                    mr.mesh.Material = mat;

                    GameObject go = new GameObject("Gameobject");
                    go.AddComponent(mr);
                    
                    Window.activeScene.AddGameObject(go);
                }
                if (ImGui.MenuItem("Delete selected Gameobject"))
                {
                    if (Editor.selectedGameobject != null)
                    {
                        if (Editor.selectedGameobject.parent != null)
                        {
                            Editor.selectedGameobject.parent.RemoveChild(Editor.selectedGameobject);
                        }
                        else
                        {
                            Window.activeScene.RemoveGameObject(Editor.selectedGameobject);
                        }
                    
                        Editor.selectedGameobject.Dispose();
                        Editor.selectedGameobject = null;
                        selected = -1;
                    }
                    
                }
                ImGui.EndPopup();
            }
            
            index = 0;
            
            if (!ImGui.IsItemHovered() && ImGui.IsWindowHovered() && !ImGui.IsAnyItemHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
            {
                selected = -1;
                Editor.selectedGameobject = null;
                
            }
            
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, Vector2.Zero);
            try
            {
                arrayModifiedWait = false;
                for (int i = 0; i < Window.activeScene.gameObjects.Count; i++)
                {
                    AddToHierarchyRecursively(Window.activeScene.gameObjects[i]);
                    if (arrayModifiedWait)
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
            
            ImGui.PopStyleVar();
            
            //ImGui.SliderFloat("Scale", ref Window.f, 0.0f, 1.0f);
            ImGui.End();

            ImGui.PopStyleVar();

        }

        private static bool dragging = false;
        private static bool nulled = false;
        private static GameObject dragObj = null;
        private static GameObject lastNullParent = null;
        private static bool arrayModifiedWait = false;

        private static bool isParentLooped(GameObject source, GameObject target)
        {
            GameObject p = target.parent;
            while (p != null)
            {
                if (p.Equals(source))
                {
                    return true;
                }
                p = p.parent;
                
            }
            return false;
        }
        
        public static void DragDrop(GameObject gameObject)
        {
            if (ImGui.BeginDragDropSource(ImGuiDragDropFlags.SourceNoDisableHover |
                                          ImGuiDragDropFlags.SourceNoPreviewTooltip) && !dragging)
            {
                dragging = true;
                nulled = false;
                dragObj = gameObject;

                //ig.SetDragDropPayload("ITEMN",anchor.data, ffi.sizeof"int", C.ImGuiCond_Once);
                Console.WriteLine("drag " + gameObject.name);
                ImGui.EndDragDropSource();
            }

            if (ImGui.BeginDragDropTarget() && ImGui.IsMouseReleased(ImGuiMouseButton.Left) && dragging)
            {
                dragging = false;

                //Check parent loop
                if (isParentLooped(dragObj, gameObject))
                {
                    ImGui.EndDragDropTarget();
                    Console.WriteLine("drop invalid. parent loop " + gameObject.name);
                    return;
                }
                
                //Check same target
                if (gameObject.Equals(dragObj))
                {
                    ImGui.EndDragDropTarget();
                    Console.WriteLine("drop invalid. same object " + gameObject.name);
                    return;
                }

                arrayModifiedWait = true;

                if (dragObj.parent != null)
                {
                    dragObj.parent.RemoveChild(dragObj);
                }
                else
                {
                    Window.activeScene.RemoveGameObject(dragObj);
                }
                
                
                gameObject.AddChild(dragObj);
                
                Console.WriteLine("drop " + gameObject.name);
                ImGui.EndDragDropTarget();
            }
            if (ImGui.IsMouseReleased(ImGuiMouseButton.Left)  && dragging && !nulled)
            {
                nulled = true;
                if (gameObject.Equals(dragObj))
                {
                    ImGui.EndDragDropTarget();
                    Console.WriteLine("drop null invalid same object " + gameObject.name);
                    return;
                }
                
                if (dragObj.parent != null)
                {
                    dragObj.parent.RemoveChild(dragObj);
                    lastNullParent = dragObj.parent;
                }
                else
                {
                    Window.activeScene.RemoveGameObject(dragObj);
                }
                arrayModifiedWait = true;
                
                dragObj.parent = null;
                Window.activeScene.AddGameObject(dragObj);
                

                Console.WriteLine("drop null ");
                
            }

            

        }
//TODO child gameobjects are not components
        private static void AddToHierarchyRecursively(GameObject gameObject)
        {
            index++;
            if (gameObject._childs.Count > 0)
            {
                if (selected == index)
                    flagsCur = flagsS;
                else
                    flagsCur = flagsNotS;
                
                
                if (ImGui.TreeNodeEx(gameObject.name, flagsCur))
                {
                    DragDrop(gameObject);
                    if (arrayModifiedWait)
                        return;
                    if (ImGui.IsItemHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                    {
                        selected = index;
                        Editor.selectedGameobject = gameObject;
                    }

                    for (int i = 0; i < gameObject._childs.Count; i++)
                    {
                        AddToHierarchyRecursively(gameObject._childs[i]);
                    }
                    
                    if (!ImGui.IsItemToggledOpen())
                    {
                        ImGui.TreePop();
                    }

                    
                }
                else
                {
                    DragDrop(gameObject);
                    if (arrayModifiedWait)
                        return;
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
                    DragDrop(gameObject);
                    if (arrayModifiedWait)
                        return;
                    if (ImGui.IsItemHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
                    {
                        selected = index;
                        Editor.selectedGameobject = gameObject;
                        Console.WriteLine("@@Selected: " + Editor.selectedGameobject.name);
                    }
                    
                }
                else
                {
                    DragDrop(gameObject);
                }
            }
            
        }
    }
}