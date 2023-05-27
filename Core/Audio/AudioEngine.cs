using System;
using System.IO;
using System.Runtime.InteropServices;
using ArchEngine.GUI.Editor;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;
namespace ArchEngine.Core.Audio
{


public class AudioEngine : IDisposable
{
    private int buffer;
    private int source;

    public bool Loaded = false;

    public void Load(string filePath)
    {

        buffer = AL.GenBuffer();
        source = AL.GenSource();
        
        ALFormat format;
        int size;
        int frequency;
        
        byte[] data = LoadWave(filePath, out format, out size, out frequency);
        
        var dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
        IntPtr dataPtr = dataHandle.AddrOfPinnedObject();
        
        AL.BufferData(buffer, format, dataPtr, size, frequency);

        AL.Source(source, ALSourcei.Buffer, buffer);
        
    }

    public void Play()
    {
        AL.SourcePlay(source);
    }

    public static void StartAudioEngine()
    {
        var device = ALC.OpenDevice(null);
        var context = ALC.CreateContext(device, new int[]{});

        ALC.MakeContextCurrent(context);

        var version = AL.Get(ALGetString.Version);
        var vendor = AL.Get(ALGetString.Vendor);
        var renderer = AL.Get(ALGetString.Renderer);
        Console.WriteLine("Loaded OpenAL " + version + " (" + renderer + ")");
        
    }

    public void Play3D(Vector3 position)
    {   
        AL.Listener(ALListener3f.Position, 0.0f, 0.0f, 0.0f); // Listener position (0, 0, 0)
        AL.Source(source, ALSource3f.Position, position.X, position.Y, position.Z);
        AL.DistanceModel(ALDistanceModel.InverseDistance); //distance model
        AL.SourcePlay(source);
    }

    public void SetLooping(bool looping)
    {
        AL.Source(source, ALSourceb.Looping, looping);
    }


    public void SetVolume(float volume)
    {
        AL.Source(source, ALSourcef.MaxGain, volume);
    }
    
    public void Dispose()
    {
        AL.DeleteSource(source);
        AL.DeleteBuffer(buffer);
    }

    private byte[] LoadWave(string filePath, out ALFormat format, out int size, out int frequency)
    {
        
        Stream stream;
        if (filePath.Contains(":"))
        {
            stream = new ResourceStream(filePath).GetStream();
        }
        else
        {
            stream = new ResourceStream(filePath, null).GetStream();
        }

        if (stream == null)
        {
            Console.WriteLine("Audio not found: " + filePath);
            format = ALFormat.Mono8;
            size = 0;
            frequency = 0;
            return null;
        }
        using (var reader = new BinaryReader(stream))
        {
            // RIFF header
            string signature = new string(reader.ReadChars(4));
            if (signature != "RIFF")
                throw new NotSupportedException("Specified file is not a valid WAV file.");

            int riffChunkSize = reader.ReadInt32();

            string formatSignature = new string(reader.ReadChars(4));
            if (formatSignature != "WAVE")
                throw new NotSupportedException("Specified file is not a valid WAV file.");

            // FMT subchunk
            string formatChunkSignature = new string(reader.ReadChars(4));
            if (formatChunkSignature != "fmt ")
                throw new NotSupportedException("Specified file is not a valid WAV file.");

            int formatChunkSize = reader.ReadInt32();
            int audioFormat = reader.ReadInt16();
            int numChannels = reader.ReadInt16();
            int sampleRate = reader.ReadInt32();
            int byteRate = reader.ReadInt32();
            int blockAlign = reader.ReadInt16();
            int bitsPerSample = reader.ReadInt16();

            // Data subchunk
            string dataChunkSignature = new string(reader.ReadChars(4));
            if (dataChunkSignature != "data")
                throw new NotSupportedException("Specified file is not a valid WAV file.");

            int dataChunkSize = reader.ReadInt32();
            byte[] data = reader.ReadBytes(dataChunkSize);

            // Set output parameters
            format = GetSoundFormat(numChannels, bitsPerSample);
            size = dataChunkSize;
            frequency = sampleRate;
            Loaded = true;
            return data;
        }
    }

    ALFormat GetSoundFormat(int channels, int bitsPerSample)
    {
        switch (channels)
        {
            case 1:
                return bitsPerSample == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
            case 2:
                return bitsPerSample == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
            default:
                throw new NotSupportedException("The specified number of channels is not supported.");
        }
    }
}
}