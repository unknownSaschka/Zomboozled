using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace View.Audio
{
    public class AudioView
    {
        private View.CachedSound _shutGunSound;
        private View.CachedSound _damage;
        private View.CachedSound _winSound;
        private Thread _musicThread;
        private Dictionary<int, LoopPlayer> _backgroundMusic = new Dictionary<int, LoopPlayer>();

        public AudioView()
        {
            _damage = new View.CachedSound(@"Content/Audio/Roblox Death Sound Effect.mp3", 1f);
            _shutGunSound = new View.CachedSound(@"Content/Audio/ShotGun1.mp3", 0.2f);
            _winSound = new View.CachedSound(@"Content/Audio/Siegesgeheul.mp3", 0.2f);


        }

        public void PlayAudioEffect(int id)
        {
            switch ((Model.Audio.AudioManager.EffectName)id)
            {
                case Model.Audio.AudioManager.EffectName.Damage:
                    View.AudioPlaybackEngine.Instance.PlaySound(_damage);
                    break;
                case Model.Audio.AudioManager.EffectName.Shotgun:
                    View.AudioPlaybackEngine.Instance.PlaySound(_shutGunSound);
                    break;
                case Model.Audio.AudioManager.EffectName.Pistol:

                    break;
                case Model.Audio.AudioManager.EffectName.Win:
                    View.AudioPlaybackEngine.Instance.PlaySound(_winSound);
                    break;
            }

        }
        public void LoadAudio()
        {
            _backgroundMusic.Add(0, new LoopPlayer(@"../../Content/Audio/Leftheria.mp3"));
            _backgroundMusic.Add(1, new LoopPlayer(@"../../Content/Audio/Sad Violin.mp3"));
            _backgroundMusic.Add(2, new LoopPlayer(@"../../Content/Audio/fight.mp3"));
            _backgroundMusic.Add(3, new LoopPlayer(@"../../Content/Audio/Tantal.mp3"));
            _backgroundMusic.Add(4, new LoopPlayer(@"../../Content/Audio/Mor Ardain.mp3"));
        }
        public void StartMusicThread(MainModel _mainModel)
        {
            MusicLoadThread musicLoadThread = new MusicLoadThread(_mainModel.ManagerHolder.AudioManager);
            _musicThread = new Thread(new ThreadStart(musicLoadThread.WaitMethod));
            _musicThread.Start();

            //musicThread.Interrupt();
        }


        public void Interrupt()
        {
            _musicThread.Interrupt();
        }
        public void Abort()
        {
            _musicThread.Abort();
        }
    }
}