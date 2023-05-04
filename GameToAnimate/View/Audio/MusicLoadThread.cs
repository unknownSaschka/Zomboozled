using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace View.Audio
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class MusicLoadThread
    {
        private LoopPlayer loop = new LoopPlayer(@"Content/Audio/boomboom.mp3", 0.1f);
        Model.Audio.AudioManager _manager;
        int oldId = 7;
        public MusicLoadThread(Model.Audio.AudioManager audioManager)
        {
            _manager = audioManager;
        }
        public void StopPlayer()
        {
            loop.StopAndDispose();

        }
        public void WaitMethod()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (ThreadInterruptedException)
                {
                    //Console.WriteLine("Music Thread Interrupted");
                    PlayBackgroundMusic((int)_manager.NextSong);
                    //Console.WriteLine($"Next Song: {_manager.NextSong} ID: {(int)_manager.NextSong}");
                }
                catch (ThreadAbortException)
                {
                    //Console.WriteLine("ConsoleThread beendet");
                    loop.StopAndDispose();
                }
            }
        }

        public void PlayBackgroundMusic(int id)
        {
            if(id == oldId)
            {
                return;
            }
            oldId = id;
            //Console.WriteLine(id);
            var ran = new Random();
            loop.StopAndDispose();
            switch (id)
            {
                case 0:
                    loop = new LoopPlayer(@"Content/Audio/Sad Violin.mp3");
                    break;
                case 1:
                    loop = new LoopPlayer(@"Content/Audio/fight.mp3", 0.1f);
                    break;
                case 2:
                    loop = new LoopPlayer(@"Content/Audio/Mor Ardain.mp3", 0.1f);
                    break;
                case 3:
                    loop = new LoopPlayer(@"Content/Audio/Tantal.mp3", 0.1f);
                    break;
                case 4:
                    loop = new LoopPlayer(@"Content/Audio/Leftheria.mp3", 0.1f);
                    break;
                case 5:
                    if (ran.NextDouble() > 0.5)
                    {
                        loop = new LoopPlayer(@"Content/Audio/Teriyaky.mp3", 0.1f);
                    }
                    else
                    {
                        loop = new LoopPlayer(@"Content/Audio/Manuel - Gas Gas Gas.mp3", 0.6f);
                    }
                    break;
                case 6:
                    loop = new LoopPlayer(@"Content/Audio/Pepsi Man.mp3", 0.1f);
                    break;
                case 7:
                    loop = new LoopPlayer(@"Content/Audio/boomboom.mp3", 0.1f);
                    break;
                case 8:
                    loop = new LoopPlayer(@"Content/Audio/Dance.mp3", 0.1f);
                    break;
                case 9:
                    loop = new LoopPlayer(@"Content/Audio/megalovania.mp3", 0.1f);
                    break;
                case 10:
                    loop = new LoopPlayer(@"Content/Audio/madmax.mp3", 0.1f);
                    break;
                case 11:
                    loop = new LoopPlayer(@"Content/Audio/Shooting Stars.mp3", 0.1f);
                    break;
                case 12:
                    loop = new LoopPlayer(@"Content/Audio/Guile Theme.mp3", 0.1f);
                    break;
            }
        }
    }
}
