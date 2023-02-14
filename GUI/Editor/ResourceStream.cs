using System.IO;

namespace ArchEngine.GUI.Editor
{
    using System.Drawing;
    using System.Reflection;

    public class ResourceStream
    {
        private readonly string _path;
        private readonly Assembly _assembly;

        public ResourceStream(string path)
        {
            _path = path;
        }

        public ResourceStream(string path, Assembly? assembly)
        {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();
            // Convert the resource name to the format used by Assembly.GetManifestResourceStream
            _path = assembly.GetName().Name + "." + path.Replace("/", ".");
            _assembly = assembly;
        }

        public Stream GetStream()
        {
            if (_assembly != null)
            {
                return _assembly.GetManifestResourceStream(_path);
            }

            return new FileStream(_path, FileMode.Open, FileAccess.Read);
        }

        public byte[] GetBytes()
        {
            using (var stream = GetStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public string GetString()
        {
            using (var stream = GetStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public Bitmap GetBitmap()
        {
            using (var stream = GetStream())
            {
                return new Bitmap(stream);
            }
        }
    }
}