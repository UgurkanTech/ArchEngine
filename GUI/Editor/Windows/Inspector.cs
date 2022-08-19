using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Rendering.Camera;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.Core.Utils;
using ArchEngine.GUI.Editor.Windows;
using ImGuiNET;
using Microsoft.VisualBasic.CompilerServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Component = ArchEngine.Core.ECS.Component;
using Quaternion = OpenTK.Mathematics.Quaternion;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;
using Vector4 = System.Numerics.Vector4;

namespace ArchEngine.GUI.Editor.Windows
{

    
    public class Inspector
    {
        private static ImGuiTreeNodeFlags nodeFlags = ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen |
                                              ImGuiTreeNodeFlags.SpanFullWidth;
        
        private static ImGuiTreeNodeFlags flagsNotS = ImGuiTreeNodeFlags.OpenOnArrow |
                                                      ImGuiTreeNodeFlags.SpanFullWidth | ImGuiTreeNodeFlags.DefaultOpen; 
        
        static byte[] nameBuffer = new byte[50];
        
        
        public static void Draw()
        {
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(400,100), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(370, 220), ImGuiCond.FirstUseEver);
            
            ImGui.Begin("Inspector");

            nodeFlags &= ~ImGuiTreeNodeFlags.Selected;
            
            if (Editor.selectedGameobject != null)
            {
                Matrix4 mat = Matrix4.Identity;
                
                mat = Editor.selectedGameobject.Transform;
                nameBuffer = Encoding.ASCII.GetBytes(Editor.selectedGameobject.name);
                
            
            
                //ImGui.Columns(2, "naming", false);
                // ImGui.SetColumnWidth(0, 100);


                if (ImGui.TreeNodeEx("Gameobject", flagsNotS))
                {
                    ImGui.Columns(3, "gameobjects", false);
                    ImGui.SetColumnWidth(0, 55);
                    ImGui.SetColumnWidth(1, ImGui.GetWindowSize().X);
                    ImGui.TextColored(new Vector4(0, 200, 0, 255), "Name");

                    ImGui.NextColumn();
                    //ImGui.NextColumn();

                    unsafe
                    {
                        if (ImGui.InputText("##Name", nameBuffer, 50,
                                ImGuiInputTextFlags.CallbackAlways | ImGuiInputTextFlags.EnterReturnsTrue, Callback)) ;
                        ImGui.SameLine();
                        ImGui.Checkbox("Is Active", ref Editor.selectedGameobject.isActive);
                    }
                }

                ImGui.TreePop();

                ImGui.Dummy(new Vector2(0, 10));
                ImGui.Separator();
                ImGui.Columns(1);
                if (ImGui.TreeNodeEx("Transform", flagsNotS))
                {
                    Vector3 pos = mat.ExtractTranslation().ToSystemVector3();
                    Quaternion rot = mat.ExtractRotation();
                    Vector3 scal = mat.ExtractScale().ToSystemVector3();


                    Vector3 roteuler = rot.ToEulerAngles().RadiansToAngles().ToSystemVector3();
                    
               
                    ImGui.Columns(2,"transforms", false);
                    //mGui.SetColumnWidth(0, 90);
                    ImGui.SetColumnWidth(0, 55);
                    ImGui.SetColumnWidth(1, ImGui.GetWindowWidth() - 55);
                    
                    ImGui.TextColored(new Vector4(0,200,0,255), "Position");
                    ImGui.NextColumn();
                    ImGui.SetNextItemWidth(ImGui.GetWindowWidth() - 50);
                    ImGui.DragFloat3("##POS", ref pos, 0.01f);
                    ImGui.NextColumn();
                    ImGui.TextColored(new Vector4(0,200,0,255), "Rotation");
                    ImGui.NextColumn();
                    ImGui.SetNextItemWidth(ImGui.GetWindowWidth() - 50);
                    ImGui.DragFloat3("##ROT", ref roteuler, 0.01f);
                    ImGui.NextColumn();
                    ImGui.TextColored(new Vector4(0,200,0,255), "Scale");
                    ImGui.NextColumn();
                    ImGui.SetNextItemWidth(ImGui.GetWindowWidth() - 50);
                    ImGui.DragFloat3("##SCL", ref scal, 0.01f);
                    
                   // ImGuizmo.Enable(true);
                    //ImGuizmo.SetOrthographic(false);
                    //ImGuizmo.SetDrawlist();
                    //ImGuizmo.SetRect(ImGui.GetWindowPos().X, ImGui.GetWindowPos().Y, 800, 600);
                
                   // OPERATION operation = OPERATION.TRANSLATE;
                    Matrix4 camV = CameraManager.EditorCamera.GetViewMatrix();
                    Matrix4 camP = CameraManager.EditorCamera.GetProjectionMatrix();
                    
                    //ImGuizmo.Manipulate(ref camV.Row0.X, ref camP.Row0.X, operation, MODE.LOCAL, ref mat.Row0.X);
                    
                    mat = Matrix4.CreateScale( scal.ToOpenTkVector3().GetNonZero());
                    mat *= Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(roteuler.ToOpenTkVector3().DegreesToRadians()));
                    mat *= Matrix4.CreateTranslation(pos.ToOpenTkVector3());
                
                    Editor.selectedGameobject.Transform = mat;
                    
                    ImGui.TreePop();
                }
                ImGui.Dummy(new Vector2(0, 15));
                ImGui.Columns();
                
                
                
                Editor.selectedGameobject._components.ForEach(component =>
                {
                    if (component.GetType() == typeof(GameObject)) return;
                    ImGui.Separator();
                    if (ImGui.TreeNodeEx(component.GetType().Name + "", flagsNotS))
                    {
                        List<MemberInfo> fields = Attributes.ScanFields(component);
                        List<MemberInfo> properties = Attributes.ScanProperties(component);
                        
                        DrawInspectorComponents(fields, component);
                        DrawInspectorComponents(properties, component);
                        
                        if (fields.Count == 0 && properties.Count == 0)
                            ImGui.Text("No options.");
                        ImGui.TreePop();
                    }
                    
                
                });
                
                if (ImGui.BeginPopup("Components"))
                {
                    
                    foreach (var component in Component.GetAllComponents())
                    {
                        if (ImGui.Button(component.Name))
                        {
                            Type type = component;
                            Component o = Activator.CreateInstance(type) as Component;
                            o.Init();
                            Editor.selectedGameobject.AddComponent(o);
                        }
                        
                    }
                    ImGui.EndPopup();
                }
                ImGui.Dummy(new Vector2(0, 10));
                ImGui.Separator();
                if (ImGui.Button("Add Component", new Vector2(ImGui.GetContentRegionAvail().X, 25)))
                {
                    ImGui.OpenPopup("Components");
                }
                
            }

            if (ImGui.IsWindowFocused() && ImGui.IsAnyItemHovered() && ImGui.IsMouseDragging(ImGuiMouseButton.Left))
            {
                Window.LockCursor = true;
            }
            else if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
            {
                Window.LockCursor = false;
            }
            

            
            ImGui.End();
        }

        private static void DrawInspectorComponents(List<MemberInfo> variables, Component component)
        {
            variables.ForEach(info =>
            {

                 
                 Type type = info.GetVariableType();
                 String name = "";
                 bool hasRange = false;
                 int rangeMin = 0;
                 int rangeMax = 0;
                            
                 foreach (Attribute attr in Attribute.GetCustomAttributes(info))
                 {
                     if (attr.GetType() == typeof(InspectorAttribute))
                     {
                         InspectorAttribute ia = (InspectorAttribute)attr;
                         name = ia.name ?? info.Name;
                                    
                     }
                     else if (attr.GetType() == typeof(RangeAttribute))
                     {
                         RangeAttribute ra = (RangeAttribute)attr;
                         hasRange = true;
                         rangeMin = (int)ra.Minimum;
                         rangeMax = (int)ra.Maximum;

                     }
                                
                 }

                 if (!hasRange)
                 {
                     if (type == typeof(float))
                     {
                         var value = (float) info.GetValue(component);
                         ImGui.DragFloat(name, ref value, 0.01f);
                         info.SetValue(component, value);
                     }
                     else if (type == typeof(int))
                     {
                         var value = (int) info.GetValue(component);
                         ImGui.DragInt(name, ref value, 1f);
                         info.SetValue(component, value);
                     }
                     else if (type == typeof(Vector3))
                     {
                         var value = (Vector3) info.GetValue(component);
                         ImGui.DragFloat3(name, ref value, 0.01f);
                         info.SetValue(component, value);
                     }
                     else if (type == typeof(OpenTK.Mathematics.Vector3))
                     {
                         var value = ((OpenTK.Mathematics.Vector3) info.GetValue(component)).ToSystemVector3();
                         ImGui.DragFloat3(name, ref value, 0.01f);
                         info.SetValue(component, value.ToOpenTkVector3());
                     }
                     else if (type == typeof(bool))
                     {
                         var value = (bool) info.GetValue(component);
                         ImGui.Checkbox(name, ref value);
                         info.SetValue(component, value);
                     }
                     else if (type == typeof(Color4))
                     {
                         var c = (Color4) info.GetValue(component);
                         var value = new Vector4(c.R, c.G, c.B, c.A);
                         //ImGui.ColorPicker4(name, ref value);
                         ImGui.ColorEdit4(name, ref value);
                         info.SetValue(component, new Color4(value.X, value.Y, value.Z, value.W));
                     }
                     else if (type == typeof(Material))
                     {
                         var mat = (Material) info.GetValue(component);
                         ImGui.Indent(5);
                         ImGui.Text("Material:");
                         ImGui.Indent(10);
                         ImGui.Button(mat?.MaterialHash, new Vector2(ImGui.GetColumnWidth(), 15));
                         ImGui.Unindent(10);
                         ImGui.Text("Shaders:");
                         ImGui.Indent(10);
                         ImGui.Button(mat?.Shader?.hash.Split("-")[0] + "", new Vector2(ImGui.GetColumnWidth(), 15));
                         ImGui.Button(mat?.Shader?.hash.Split("-")[1] + "", new Vector2(ImGui.GetColumnWidth(), 15));
                         ImGui.Unindent(15);
                     }
                     else if (type == typeof(Mesh))
                     {
                         var mesh = (Mesh) info.GetValue(component);
                         
                         ImGui.Indent(5);
                         ImGui.Text("Mesh:");
                         ImGui.Indent(10);
                         ImGui.Button(mesh?.MeshHash + "", new Vector2(ImGui.GetColumnWidth(), 15));
                         ImGui.Unindent(15);
                     }
                     
                     
                 }
                 else
                 {
                     if (type == typeof(float))
                     {
                         float value = (float)info.GetValue(component);
                         ImGui.SliderFloat(name, ref value, rangeMin, rangeMax);
                         info.SetValue(component, value);
                     }
                     else if (type == typeof(int))
                     {
                         int value = (int)info.GetValue(component);
                         ImGui.SliderInt(name, ref value, rangeMin,  rangeMax);
                         info.SetValue(component, value);

                     }
                 }

                
                ImGui.Dummy(new Vector2(0, 5));
            });

        }

       
        
        
        private static unsafe int Callback(ImGuiInputTextCallbackData* data)
        {
            if (ImGui.IsKeyPressed(ImGuiKey.Enter))
            {
                if (Editor.selectedGameobject == null)
                    return -1;
                Editor.selectedGameobject.name = Encoding.ASCII.GetString(data->Buf, data->BufTextLen);
            }

            return -1;
        }
    }
    
}