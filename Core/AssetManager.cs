using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OpenTK.Windowing.Common.Input;

namespace ArchEngine.Core
{
    public class AssetManager
    {
        
        public static WindowIcon LoadWindowIconFromFile(string path)
        {
            using var image = new Bitmap(path);
            var data = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            byte[] data2 = new byte[Math.Abs(data.Stride * data.Height)];
            Marshal.Copy(data.Scan0, data2, 0, data2.Length);
            
            OpenTK.Windowing.Common.Input.Image[] images = new OpenTK.Windowing.Common.Input.Image[1];
            images[0] = new OpenTK.Windowing.Common.Input.Image(image.Width, image.Height, data2);
            
            return new WindowIcon(images);

        }
        
    }
}