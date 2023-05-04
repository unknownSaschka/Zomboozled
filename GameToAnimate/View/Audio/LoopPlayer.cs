using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View
{
    public class LoopPlayer : IDisposable
    {
        private WaveOut waveOut;
        private bool _running = true;
        private bool _disposed = false;

        public LoopPlayer(string fileName)
        {

            //WaveFileReader reader = new WaveFileReader(fileName);
            AudioFileReader reader = new AudioFileReader(fileName);
            LoopStream loop = new LoopStream(reader);
            waveOut = new WaveOut();
            waveOut.Init(loop);
            waveOut.Play();
        }
        public LoopPlayer(string fileName, float volume)
        {
            AudioFileReader reader = new AudioFileReader(fileName)
            {
                Volume = volume
            };
            LoopStream loop = new LoopStream(reader);
            waveOut = new WaveOut();
            waveOut.Init(loop);
            waveOut.Play();
        }
        /*
        public LoopPlayer(CachedSound cachedSound)
        {

            WaveFileReader reader = new WaveFileReader(fileName);
            LoopStream loop = new LoopStream(reader);
            waveOut = new WaveOut();
            waveOut.Init(loop);
            waveOut.Play();
        }
        */

        public void StartMusic()
        {
            waveOut.Play();
        }

        public void StopMusic()
        {
            
        }

        public void StopAndDispose()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                StopAndDispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        ~LoopPlayer()
        {
            Dispose(false);
        }


        public bool Running
        {
            get { return _running; }
            set
            {
                if (_running != value)
                {
                    if(value == true)
                    {
                        waveOut.Resume();
                        
                    }
                    else
                    {
                        waveOut.Pause();
                    }
                    _running = value;
                }
            }
        }






    }
}
