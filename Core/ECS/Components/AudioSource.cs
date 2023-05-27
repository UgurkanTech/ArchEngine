using System;
using System.ComponentModel.DataAnnotations;
using ArchEngine.Core.Audio;
using ArchEngine.GUI.Editor;

namespace ArchEngine.Core.ECS.Components
{
    public class AudioSource : Component
    {
        public void Dispose()
        {
            engine.Dispose();
        }

        public GameObject gameObject { get; set; }
        public bool initialized { get; set; }

        public AudioEngine engine;

        [Inspector()] public string audioFile = "Resources/Audio/honk.wav";
        [Inspector()] public InspectorButton playButton;
        [Inspector()] public bool playOnStart = false; 
        [Inspector()] public bool looping = false;
        [Inspector()][Range(0, 1)] public float volume = 1f;


        private bool wasLooping = false;
        public void Init()
        {
            engine = new AudioEngine();
            
            engine.Load(audioFile);

            playButton = new InspectorButton();
            playButton.ButtonClicked += HandleButtonClick;
        }
        
        void HandleButtonClick(object sender, EventArgs e)
        {
           PlaySound3D();
        }

        public void PlaySound()
        {
            
            if (engine.Loaded)
            {
                engine.SetVolume(volume);
                engine.Play();
            }
            else
            {
                Window._log.Error("Cannot play audio file:" + audioFile);
            }
        }

        public void PlaySound3D()
        {
            if (engine.Loaded)
            {
                engine.SetVolume(volume);
                engine.Play3D(gameObject.Transform.ExtractTranslation());
            }
            else
            {
                Window._log.Error("Cannot play audio file:" + audioFile);
            }
            
        }

        public void Start()
        {
            if (playOnStart)
            {
                PlaySound3D();
            }

            engine.SetLooping(looping);
        }

        public void Update()
        {
            if (looping)
                wasLooping = true;

            if (wasLooping & !looping)
            {
                wasLooping = false;
                engine.SetLooping(false);
            }
        }
    }
}