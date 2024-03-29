﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Physics;
using ArchEngine.Core.Rendering.Camera;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.Core.Utils;
using BulletSharp;
using BulletSharp.Math;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Component = ArchEngine.Core.ECS.Component;
using Quaternion = OpenTK.Mathematics.Quaternion;
using Vector3 = OpenTK.Mathematics.Vector3;
using Vector4 = OpenTK.Mathematics.Vector4;
using Window = ArchEngine.Core.Window;

namespace ArchEngine.GUI.Editor.Windows
{


    public class Inspector
    {
        private static ImGuiTreeNodeFlags nodeFlags = ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen |
                                              ImGuiTreeNodeFlags.SpanFullWidth;
        
        private static ImGuiTreeNodeFlags flagsNotS = ImGuiTreeNodeFlags.OpenOnArrow |
                                                      ImGuiTreeNodeFlags.SpanAvailWidth | ImGuiTreeNodeFlags.DefaultOpen; 
        
        static byte[] nameBuffer = new byte[50];

        public static void Draw()
        {
            ImGui.SetNextWindowPos(new Vector2(400,100), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new Vector2(370, 220), ImGuiCond.FirstUseEver);
            
            //ImGui.Begin("Inspector");
            Icons.ImguiBeginIcon("Inspector", 175);

            nodeFlags &= ~ImGuiTreeNodeFlags.Selected;
            
            if (Editor.selectedGameobject != null)
            {
                Matrix4 mat = Matrix4.Identity;
                
                mat = Editor.selectedGameobject.Transform;
                
                
                if (Editor.selectedGameobject.HasComponent<RigidObject>() && Window.started)
                {
                    var _rb = Editor.selectedGameobject.GetComponent<RigidObject>()._rb;
                    _rb.MotionState.GetWorldTransform(out Matrix mm);
                    mat = mm.MatrixToMatrix4();
                }
                
                nameBuffer = Encoding.ASCII.GetBytes(Editor.selectedGameobject.name);
                
            
            
                //ImGui.Columns(2, "naming", false);
                // ImGui.SetColumnWidth(0, 100);


                ImGui.Columns(2, "gameobjects", false);
                ImGui.SetColumnWidth(0, 75);
                ImGui.SetColumnWidth(1, ImGui.GetWindowSize().X);
                //ImGui.TextColored(new Vector4(0, 200, 0, 255), "Name");
                if (ImGui.ImageButton((IntPtr) AssetManager.cube.handle, new Vector2(25, 25)))
                {
                    Window._log.Info("Benchmark started for gameobject Start(): " + Editor.selectedGameobject.name);
                    Clock.BenchmarkCpu(() =>
                    {
                        Editor.selectedGameobject.Start();
                    }, 1);
                    Window._log.Info("Benchmark finished for gameobject: " + Editor.selectedGameobject.name);
                }
                ImGui.SameLine();
                ImGui.Checkbox("##SetActiveInspector", ref Editor.selectedGameobject.isActive);
                ImGui.NextColumn();
                unsafe
                {
                    ImGui.InputTextEx("##NameInspector", "Name", Editor.selectedGameobject.name,50,
                        new Vector2(ImGui.GetWindowSize().X - 75, 20), ImGuiInputTextFlags.CallbackAlways | ImGuiInputTextFlags.EnterReturnsTrue,
                        Callback);
                    
                }

                ImGui.TreePop();

                ImGui.Dummy(new Vector2(0, 10));
                ImGui.Separator();
                ImGui.Columns(1);
                if (ImGui.TreeNodeEx("Transform", flagsNotS))
                {
                    Vector3 pos = mat.ExtractTranslation();
                    Quaternion rot = mat.ExtractRotation();
                    Vector3 scal = mat.ExtractScale();
                    
                    Vector3 roteuler = rot.ToEulerAngles().RadiansToAngles();
                    
               
                    //mGui.SetColumnWidth(0, 90);
                    
                    
                    
                    ImGui.TextColored(new Vector4(0,200,0,255), "Position");
                    ImGui.SameLine(50);
                    ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
                    ImGui.DragFloat3("##POS", ref pos, 0.01f);
                    
                    
                    ImGui.TextColored(new Vector4(0,200,0,255), "Rotation");
                    ImGui.SameLine(50);
                    ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
                    ImGui.DragFloat3("##ROT", ref roteuler, 0.01f);
                    
                    
                    ImGui.TextColored(new Vector4(0,200,0,255), "Scale");
                    ImGui.SameLine(50);
                    ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
                    ImGui.DragFloat3("##SCL", ref scal, 0.01f);
                    
                   // ImGuizmo.Enable(true);
                    //ImGuizmo.SetOrthographic(false);
                    //ImGuizmo.SetDrawlist();
                    //ImGuizmo.SetRect(ImGui.GetWindowPos().X, ImGui.GetWindowPos().Y, 800, 600);
                
                   // OPERATION operation = OPERATION.TRANSLATE;
                    Matrix4 camV = CameraManager.EditorCamera.GetViewMatrix();
                    Matrix4 camP = CameraManager.EditorCamera.GetProjectionMatrix();
                    
                    //ImGuizmo.Manipulate(ref camV.Row0.X, ref camP.Row0.X, operation, MODE.LOCAL, ref mat.Row0.X);
                    
                    mat = Matrix4.CreateScale( scal.GetNonZero());
                    mat *= Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(roteuler.DegreesToRadians()));
                    mat *= Matrix4.CreateTranslation(pos);
                
                    Editor.selectedGameobject.Transform = mat;
                    if (Editor.selectedGameobject.HasComponent<RigidObject>())
                    {
                        var _rb = Editor.selectedGameobject.GetComponent<RigidObject>()._rb;
                        Matrix matr = mat.Matrix4ToMatrix();
                        _rb.MotionState = new DefaultMotionState(matr);
                        _rb.WorldTransform = matr;
                        _rb.MotionState.WorldTransform = matr;
                        _rb.LinearVelocity = BulletSharp.Math.Vector3.Zero;
                        _rb.AngularVelocity = BulletSharp.Math.Vector3.Zero;
                        
                    }

                    
                    ImGui.TreePop();
                }
                ImGui.Dummy(new Vector2(0, 15));
                

                for (int i = 0; i < Editor.selectedGameobject._components.Count; i++)
                {
                    var component = Editor.selectedGameobject._components[i];
                    if (component.GetType() == typeof(GameObject)) return;
                    ImGui.Separator();

                    ImGui.Columns(2, "inspectorTreeTitle", false);
                    ImGui.SetColumnWidth(0, ImGui.GetWindowSize().X - 30);
                    ImGui.SetColumnWidth(1, 30);
                    if (ImGui.TreeNodeEx(component.GetType().Name + "", flagsNotS))
                    {
                        ImGui.NextColumn();
                        ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.25f, 0.25f,0.25f,1));
                        if (ImGui.ImageButtonEx(ImGui.GetID("DeleteComponent" + component.GetType().Name), Icons.Texture, new Vector2(15, 15), Icons.GetUV0FromID(21), Icons.GetUV1FromID(21),
                                Vector2.Zero, Vector4.Zero, Vector4.One))
                        {
                            ImGui.PopStyleColor();
                            Editor.selectedGameobject.RemoveComponent(component);
                            break;
                        }
                        else
                        {
                            ImGui.PopStyleColor();
                        }
                        
                        ImGui.Columns();
                        List<MemberInfo> fields = Attributes.ScanFields(component);
                        List<MemberInfo> properties = Attributes.ScanProperties(component);
                        
                        DrawInspectorComponents(fields, component);
                        DrawInspectorComponents(properties, component);
                        
                        if (fields.Count == 0 && properties.Count == 0)
                            ImGui.Text("No options.");
                        ImGui.TreePop();
                    }
                    
                }
                

                
                if (ImGui.BeginPopup("Components"))
                {
                    
                    foreach (var component in Component.GetAllComponents())
                    {
                        if (ImGui.Button(component.Name))
                        {
                            Type type = component;
                            Component o = Activator.CreateInstance(type) as Component;
                            o.gameObject = Editor.selectedGameobject;
                            o.Init();
                            Editor.selectedGameobject.AddComponent(o);
                            ImGui.CloseCurrentPopup();
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
                     else if (type == typeof(string))
                     {
                         var value = (string) info.GetValue(component);
                         ImGui.Indent(15);
                         ImGui.Text(info.Name + " ");
                         ImGui.SameLine();
                         ImGui.Button(value + "", new Vector2(ImGui.GetColumnWidth(), 20));
                         //info.SetValue(component, value);
                     }
                     else if (type == typeof(InspectorButton))
                     {
                         var value = (InspectorButton) info.GetValue(component);
                         ImGui.Indent(15);
                         if (ImGui.Button(info.Name + "", new Vector2(ImGui.GetColumnWidth(), 20)))
                         {
                             value.Click();
                         }
                         ImGui.Unindent(20);
                     }
                     else if (type == typeof(Vector3))
                     {
                         var value = (Vector3) info.GetValue(component);
                         ImGui.DragFloat3(name, ref value, 0.01f);
                         info.SetValue(component, value);
                     }
                     else if (type == typeof(OpenTK.Mathematics.Vector3))
                     {
                         var value = ((OpenTK.Mathematics.Vector3) info.GetValue(component));
                         ImGui.DragFloat3(name, ref value, 0.01f);
                         info.SetValue(component, value);
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
                         ImGui.Image(Icons.Texture, new Vector2(15, 15), Icons.GetUV0FromID(215), Icons.GetUV1FromID(215)); ImGui.SameLine();
                         if (ImGui.Button(mat?.MaterialHash + ((mat != null && mat.isTextureMissing) ? " [Missing]" : ""), new Vector2(ImGui.GetColumnWidth(), 20)))
                         {
                             
                             Console.WriteLine("clicked to " + component.gameObject.name);
                         }
                         if (ImGuiNET.ImGui.BeginDragDropTarget())
                         {
                             // Create the drag and drop payload
                             var payload = ImGuiNET.ImGui.AcceptDragDropPayload("FILE");
                             unsafe
                             {
                                 if (payload.NativePtr != null)
                                 {
                                     byte[] bytes = new byte[payload.DataSize];
                                     Marshal.Copy(payload.Data, bytes, 0, payload.DataSize);
                                     string data = Encoding.ASCII.GetString(bytes);
                                     
                                     if (Directory.Exists(data))
                                     {
                                         mat.LoadTextures(data.Substring(Editor.projectDir.Length+1));
                                     }
                                     
                                 }
                             }
                             
                             // Send the payload
                             ImGuiNET.ImGui.EndDragDropTarget();
                         }
                         ImGui.Unindent(10);
                         ImGui.Text("Shaders:");
                         ImGui.Indent(10);
                         ImGui.Image(Icons.Texture, new Vector2(15, 15), Icons.GetUV0FromID(215), Icons.GetUV1FromID(215)); ImGui.SameLine();
                         ImGui.Button(mat?.Shader?.hash.Split("-")[0] + "", new Vector2(ImGui.GetColumnWidth(), 20));
                         ImGui.Image(Icons.Texture, new Vector2(15, 15), Icons.GetUV0FromID(215), Icons.GetUV1FromID(215)); ImGui.SameLine();
                         ImGui.Button(mat?.Shader?.hash.Split("-")[1] + "", new Vector2(ImGui.GetColumnWidth(), 20));
                         ImGui.Unindent(15);
                     }
                     else if (type == typeof(Mesh))
                     {
                         var mesh = (Mesh) info.GetValue(component);
                         
                         ImGui.Indent(5);
                         ImGui.Text("Mesh:");
                         ImGui.Indent(10);
                         ImGui.Image(Icons.Texture, new Vector2(15, 15), Icons.GetUV0FromID(215), Icons.GetUV1FromID(215)); ImGui.SameLine();
                         ImGui.Button(mesh?.MeshHash + "", new Vector2(ImGui.GetColumnWidth(), 20));
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
            if (ImGui.IsKeyPressed((int)Keys.Enter))
            {
                if (Editor.selectedGameobject == null)
                    return -1;
                Editor.selectedGameobject.name = Encoding.ASCII.GetString(data->Buf, data->BufTextLen);
            }

            return -1;
        }
    }
    
}