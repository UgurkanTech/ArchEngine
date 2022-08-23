using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

namespace ArchEngine.GUI.Editor.Windows
{
    public class Hierarchy
    {
        private static int selected = -1;
        private static int index;
        
        private static ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.OpenOnArrow |  ImGuiTreeNodeFlags.SpanAvailWidth | ImGuiTreeNodeFlags.DefaultOpen;
        
        
        public static void Draw()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.IndentSpacing, 15f);
            
           
            
            ImGui.SetNextWindowPos(new Vector2(25,100), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new Vector2(150, 300), ImGuiCond.FirstUseEver);
            ImGui.Begin("Hierarchy");

            if (ImGui.BeginPopupContextWindow())
            {
                if (ImGui.MenuItem("Add New Gameobject"))
                {
                    Core.ECS.Scene.SpawnObject();
                }
                if (ImGui.MenuItem("Delete selected Gameobject"))
                {
                    if (Editor.selectedGameobject.parent != null)
                    {
                        Editor.selectedGameobject.parent.RemoveChild(Editor.selectedGameobject);
                    }
                    else
                    {
                        Window.activeScene.RemoveGameObject(Editor.selectedGameobject);
                    }

                    selected = -1;

                }

                ImGui.EndPopup();
            }
            
            index = 0;
            
            if (!ImGui.IsItemHovered() && ImGui.IsWindowHovered() && !ImGui.IsAnyItemHovered() && ImGui.IsMouseClicked(ImGuiMouseButton.Left))
            {
                selected = -1;
                Editor.selectedGameobject = null;

            }

            
            
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(0,0f));
            try
            {
                arrayModifiedWait = false;
                for (int i = 0; i < Window.activeScene.gameObjects.Count; i++)
                {
                    AddToHierarchyRecursively(Window.activeScene.gameObjects[i], i);
                    
                    if (arrayModifiedWait)
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
            
            if (!ImGui.IsAnyItemHovered() && ImGui.IsMouseReleased(ImGuiMouseButton.Left))
            {
                DragDrop(null, -1, outside:true);
            }
            
            ImGui.PopStyleVar();
            
            //ImGui.SliderFloat("Scale", ref Window.f, 0.0f, 1.0f);
            ImGui.End();

            ImGui.PopStyleVar();
            
            

        }

        private static bool dragging = false;
        private static int dragIndex = -1;
        private static GameObject dragObj = null;

        private static bool arrayModifiedWait = false;

        public static bool needsSelectUpdate = false;

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
        
        public static void DragDrop(GameObject gameObject, int sceneIndex, bool move = false, bool outside = false)
        {
            
            
            
            if (dragging && !ImGui.IsWindowHovered(ImGuiHoveredFlags.AllowWhenBlockedByActiveItem)) //cancelled
            {
                dragging = false;
                Console.WriteLine("drag cancelled");
                dragObj = null;
                return;
            }

            if (move && dragging)
            {
                dragging = false;

                
                Window.activeScene.MoveGameObjecTo(dragObj, sceneIndex);
                Console.WriteLine(dragObj?.name + " moved to " + sceneIndex);
                dragObj = null;
                return;
            }
            

            if (outside && dragging) //nulled
            {
                dragging = false;
                Console.WriteLine("drag nulled");
                if (dragObj.parent != null)
                {
                    dragObj.parent.RemoveChild(dragObj);
                }
                else
                {
                    Window.activeScene.RemoveGameObject(dragObj);
                }
                arrayModifiedWait = true;
                
                dragObj.parent = null;
                Window.activeScene.AddGameObject(dragObj);
                dragObj = null;
                
            }
            
            if (ImGui.IsMouseDown(ImGuiMouseButton.Left) && !dragging) //drag
            {
                dragging = true;
                
                dragObj = gameObject;
                dragIndex = sceneIndex;
                //Console.WriteLine("drag " + gameObject.name);
                
            }

            if (ImGui.IsMouseReleased(ImGuiMouseButton.Left) && dragging) //drop
            {
                dragging = false;
                
                //Check parent loop
                if (isParentLooped(dragObj, gameObject))
                {
                    ImGui.EndDragDropTarget();
                    Console.WriteLine("drop invalid. parent loop " + gameObject.name);
                    dragObj = null;
                    return;
                }
                
                //Check same target
                if (gameObject.Equals(dragObj))
                {
                    ImGui.EndDragDropTarget();
                    //Console.WriteLine("drop invalid. same object " + gameObject.name);
                    dragObj = null;
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
                
                dragObj = null;
            }

        }

        private static void AddToHierarchyRecursively(GameObject gameObject, int sceneIndex)
        {
            index++;

            if (ImGui.ImageButtonEx(ImGui.GetID(index + "HierarchyEyeButton"), Icons.Texture, new Vector2(12, 12), Icons.GetUV0FromID(gameObject.isActive ? 231 : 230),
                    Icons.GetUV1FromID(gameObject.isActive ? 231 : 230),
                    Vector2.Zero, Vector4.Zero, Vector4.One))
            {
                gameObject.isActive = false;
                Console.WriteLine("clicked");
            }

             ImGui.SameLine();
            if (ImGui.TreeNodeEx(gameObject.name, (selected == index ? flags | ImGuiTreeNodeFlags.Selected : flags) | (gameObject._childs.Count == 0 ? ImGuiTreeNodeFlags.Bullet : ImGuiTreeNodeFlags.None)))
            {
                if (ImGui.BeginDragDropSource( ImGuiDragDropFlags.SourceNoPreviewTooltip | ImGuiDragDropFlags.SourceNoDisableHover)){}
                
                
                if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenBlockedByActiveItem))
                {
                    DragDrop(gameObject, sceneIndex);
                }
                
                if (ImGui.IsItemClicked(ImGuiMouseButton.Left) || ImGui.IsItemClicked(ImGuiMouseButton.Right))
                {
                    Console.WriteLine("selection changed");
                    selected = index;
                    Editor.selectedGameobject = gameObject;
                }

                if (needsSelectUpdate)
                {
                    if (Editor.selectedGameobject == null)
                    {
                        selected = -1;
                    }
                    else if (Editor.selectedGameobject.Equals(gameObject))
                    {
                        selected = index;
                        needsSelectUpdate = false;
                    }
                    
                }
                


                for (int i = 0; i < gameObject._childs.Count; i++)
                {
                    AddToHierarchyRecursively(gameObject._childs[i], -1);
                }
                    
                if (!ImGui.IsItemToggledOpen())
                {
                    ImGui.TreePop();
                    
                }
                
            }
            else
            {
                if (ImGui.BeginDragDropSource( ImGuiDragDropFlags.SourceNoPreviewTooltip | ImGuiDragDropFlags.SourceNoDisableHover)){}
                if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenBlockedByActiveItem))
                {
                    DragDrop(gameObject, sceneIndex);
                }
            }


        }
    }
}