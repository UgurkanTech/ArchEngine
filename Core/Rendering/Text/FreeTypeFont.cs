using System;
using System.Collections.Generic;
using ArchEngine.Core.Rendering;
using ArchEngine.GUI.Editor;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SharpFont;

namespace ArchEngine.GUI
{
    public struct Character
    {
        public int TextureID { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Bearing { get; set; }
        public int Advance { get; set; }
    }
    
    public class FreeTypeFont
    {
        Dictionary<uint, Character> _characters = new Dictionary<uint, Character>();
        int _vao;
        int _vbo;

        public FreeTypeFont(uint pixelheight)
        {
            // initialize library
            
            Library lib = new Library();

            var bytes = new ResourceStream("Resources/Fonts/FreeSans.ttf", null).GetBytes();
            Face face = new Face(lib, bytes, 0);

            face.SetPixelSizes(0, pixelheight);

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);
            
            GL.ActiveTexture(TextureUnit.Texture0);
            
            for (uint c = 0; c < 128; c++)
            {
                try
                {
                    face.LoadChar(c, LoadFlags.Render, LoadTarget.Normal);
                    GlyphSlot glyph = face.Glyph;
                    FTBitmap bitmap = glyph.Bitmap;

                    
                    int texObj = GL.GenTexture();
                    
                    GL.BindTexture(TextureTarget.Texture2D, texObj);
                    GL.TexImage2D(TextureTarget.Texture2D, 0,
                                  PixelInternalFormat.R8, bitmap.Width, bitmap.Rows, 0,
                                  PixelFormat.Red, PixelType.UnsignedByte, bitmap.Buffer);

                    
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                    
                    
                    Character ch = new Character();
                    ch.TextureID = texObj;
                    ch.Size = new Vector2(bitmap.Width, bitmap.Rows);
                    ch.Bearing = new Vector2(glyph.BitmapLeft, glyph.BitmapTop);
                    ch.Advance = (int)glyph.Advance.X.Value;
                    _characters.Add(c, ch);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            
            GL.BindTexture(TextureTarget.Texture2D, 0);
            
            GL.GenVertexArrays(1, out _vao);
            GL.GenBuffers(1, out _vbo);
            GL.BindVertexArray(_vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 6 * 4, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0,4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer,0);
            GL.BindVertexArray(0);
            
        }

        public void RenderText(Shader shader, string text, float x, float y, float scale)
        {
            
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            
            GL.Enable(EnableCap.CullFace);
            
            shader.Use();
            shader.SetVector3("textColor", new Vector3(0,1,0));
            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindVertexArray(_vao);
            
            foreach (var c in text) 
            {
                if (_characters.ContainsKey(c) == false)
                    continue;
                Character ch = _characters[c];

                float xpos = x + ch.Bearing.X * scale;
                float ypos = y - (ch.Size.Y - ch.Bearing.Y) * scale;

                float w = ch.Size.X * scale;
                float h = ch.Size.Y * scale;

                // update VBO for each character
                float[] vertices = {
                     xpos,     ypos + h,   0.0f, 0.0f ,
                     xpos,     ypos,       0.0f, 1.0f ,
                     xpos + w, ypos,       1.0f, 1.0f ,

                     xpos,     ypos + h,   0.0f, 0.0f ,
                     xpos + w, ypos,       1.0f, 1.0f ,
                     xpos + w, ypos + h,   1.0f, 0.0f 
                };
                GL.BindTexture(TextureTarget.Texture2D, ch.TextureID);
                
                GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
                
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, sizeof(float) * 24, vertices);
                
                GL.BindBuffer(BufferTarget.ArrayBuffer,0);
                
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                
                x += (ch.Advance >> 6) * scale;
            }

            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);
        }
    }
}