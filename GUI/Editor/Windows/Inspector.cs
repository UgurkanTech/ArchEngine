using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
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

    
    public class Inspector
    {
        private static ImGuiTreeNodeFlags nodeFlags = ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen |
                                              ImGuiTreeNodeFlags.SpanFullWidth;
        
        private static ImGuiTreeNodeFlags flagsNotS = ImGuiTreeNodeFlags.OpenOnArrow |
                                                      ImGuiTreeNodeFlags.SpanFullWidth | ImGuiTreeNodeFlags.DefaultOpen; 
        
        static byte[] nameBuffer = new byte[100];
        
        
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
                   ImGui.TextColored(new Vector4(0,200,0,255), "          Name    ");
                
                   ImGui.SameLine();
                   //ImGui.NextColumn();
                
                   unsafe
                   {
                       if (ImGui.InputText("##Name", nameBuffer, 100, ImGuiInputTextFlags.CallbackAlways | ImGuiInputTextFlags.EnterReturnsTrue, Callback));
                   }
               }
               ImGui.TreePop();
               
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
                    
                   
                
                    mat = Matrix4.CreateScale( scal.ToOpenTkVector3().GetNonZero());
                    mat *= Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(roteuler.ToOpenTkVector3().DegreesToRadians()));
                    mat *= Matrix4.CreateTranslation(pos.ToOpenTkVector3());
                
                    Editor.selectedGameobject.Transform = mat;
                    ImGui.PopItemWidth();
                    ImGui.TreePop();
                }
                
                ImGui.Columns();
                
                
                
                Editor.selectedGameobject._components.ForEach(component =>
                {
                    if (component.GetType() == typeof(GameObject)) return;

                    if (ImGui.TreeNodeEx(component.GetType().Name + "", flagsNotS))
                    {
                        List<FieldInfo> fields = Attributes.ScanAttiributes(component);
                        fields.ForEach(info =>
                        {
                            Type type = info.FieldType;
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
                                    float value = (float)info.GetValue(component);
                                    ImGui.DragFloat(name, ref value, 0.01f);
                                    info.SetValue(component, value);
                                }
                                else if (type == typeof(int))
                                {
                                    int value = (int)info.GetValue(component);
                                    ImGui.DragInt(name, ref value, 1f);
                                    info.SetValue(component, value);

                                }
                                else if (type == typeof(Vector3))
                                {
                                    Vector3 value = (Vector3)info.GetValue(component);
                                    ImGui.DragFloat3(name, ref value, 0.01f);
                                    info.SetValue(component, value);

                                }
                                else if (type == typeof(OpenTK.Mathematics.Vector3))
                                {
                                    Vector3 value = ((OpenTK.Mathematics.Vector3)info.GetValue(component)).ToSystemVector3();
                                    ImGui.DragFloat3(name, ref value, 0.01f);
                                    info.SetValue(component, value.ToOpenTkVector3());

                                }
                                else if (type == typeof(bool))
                                {
                                    bool value = ((bool)info.GetValue(component));
                                    ImGui.Checkbox(name, ref value);
                                    info.SetValue(component, value);

                                }
                                else if (type == typeof(Color4))
                                {
                                    Color4 c = ((Color4) info.GetValue(component));
                                    Vector4 value = new Vector4(c.R, c.G, c.B, c.A);
                                    ImGui.ColorPicker4(name, ref value);
                                    info.SetValue(component, new Color4(value.X,value.Y,value.Z, value.W));

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


                        });
                        if (fields.Count == 0)
                            ImGui.Text("No options.");
                        ImGui.TreePop();
                    }
                    
                
                });
            }

            if (ImGui.IsWindowHovered() && ImGui.IsAnyItemHovered() && ImGui.IsMouseDragging(ImGuiMouseButton.Left))
            {
                Window.LockCursor = true;
            }
            else if (ImGui.IsMouseReleased(ImGuiMouseButton.Left))
            {
                Window.LockCursor = false;
            }
            ImGui.End();
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