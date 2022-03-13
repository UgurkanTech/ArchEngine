using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.IO;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using SharpFont;

namespace ArchEngine
{
    /// <summary>
    ///
    /// [SharpFont](https://www.nuget.org/packages/SharpFont/)
    /// [Robmaister/SharpFont](https://github.com/Robmaister/SharpFont)
    ///
    /// [LearnOpenGL - Text Rendering](https://learnopengl.com/In-Practice/Text-Rendering)
    ///
    /// x64 SharpFont.dll and freetype6.dll from [MonoGame.Dependencies](https://github.com/MonoGame/MonoGame.Dependencies)
    ///
    /// </summary>

    // TODO [...] use SharpFontCore rather than SpaceWizards.SharpFont

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

            Face face = new Face(lib, "FreeSans.ttf");

            
            
            //Face face = new Face(lib, ms.ToArray(), 0);

            face.SetPixelSizes(0, pixelheight);

            // set 1 byte pixel alignment 
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            // set texture unit
            GL.ActiveTexture(TextureUnit.Texture0);

            // Load first 128 characters of ASCII set
            for (uint c = 0; c < 128; c++)
            {
                try
                {
                    // load glyph
                    //face.LoadGlyph(c, LoadFlags.Render, LoadTarget.Normal);
                    face.LoadChar(c, LoadFlags.Render, LoadTarget.Normal);
                    GlyphSlot glyph = face.Glyph;
                    FTBitmap bitmap = glyph.Bitmap;

                    // create glyph texture
                    int texObj = GL.GenTexture();
                    GL.BindTexture(TextureTarget.Texture2D, texObj);
                    GL.TexImage2D(TextureTarget.Texture2D, 0,
                                  PixelInternalFormat.R8, bitmap.Width, bitmap.Rows, 0,
                                  PixelFormat.Red, PixelType.UnsignedByte, bitmap.Buffer);

                    // set texture parameters
                    //GL.TextureParameter(texObj, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    //GL.TextureParameter(texObj, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    //GL.TextureParameter(texObj, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                    //GL.TextureParameter(texObj, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                    Console.WriteLine(bitmap.Buffer);
                    // add character
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

            // bind default texture
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

        public void RenderText(ref Shader shader, string text, float x, float y, float scale, Vector2 dir)
        {
            
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            
            shader.Use();
            shader.SetVector3("textColor", new Vector3(1,1,1));
            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindVertexArray(_vao);

            float angle_rad = (float)Math.Atan2(dir.Y, dir.X);
            Matrix4 rotateM = Matrix4.CreateRotationZ(angle_rad);
            Matrix4 transOriginM = Matrix4.CreateTranslation(new Vector3(x, y, 0f));
            
            foreach (var c in text) 
            {
                if (_characters.ContainsKey(c) == false)
                    continue;
                Character ch = _characters[c];

                float xpos = x + ch.Bearing.X * scale;
                float ypos = 600 - y - (32 + (ch.Size.Y - ch.Bearing.Y)) * scale;

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
                
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, sizeof(float),vertices);
                
                GL.BindBuffer(BufferTarget.ArrayBuffer,0);
                
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
                x += (ch.Advance >> 6) * scale;
            }

            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            
            GL.Disable(EnableCap.Blend);
        }
    }
}