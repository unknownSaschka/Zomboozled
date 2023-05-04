using System;
using static Model.Audio.AudioManager;
using static Model.WorldLogic.TileLogic.Types;

namespace Model.Audio
{
    public enum AudioType { Effect, BGMusic }
    public delegate void StateAudioChanged(AudioType musicType, int id);

    public enum BackgroundSongTheme { Fight, Peace, Attack, Ambush, Boss };
    public enum BackgroundSongName { Dead, OnePunch, MorArdain, Tantal, Leftheria, Teriyaky, PepsiMan, BoomBoom, DanceTillYouDead, Megalovania, MadMax, ShootingStars, Guile }

    public class AudioManager
    {
        
        public enum EffectName { Pistol, MG, Shotgun, Damage, Win}
        //private ChunkType oldChunkType;
        public StateAudioChanged AudioEvent;
        public CallbackMusic callbackMusic;

        private BackgroundSongName _nextSong = BackgroundSongName.Teriyaky;

        public BackgroundSongName NextSong
        {
            get { return _nextSong; }
        }

        public AudioManager()
        {
        }
        public void ChangeBackgroundMusic(ChunkType chunkType, int distance)
        {
            //Console.WriteLine($"ChunkType: {chunkType}");
            
                
                //oldChunkType = chunkType;

                switch (chunkType)
                {
                    case ChunkType.Start:
                        _nextSong = BackgroundSongName.Leftheria;
                        AudioEvent?.Invoke(AudioType.BGMusic, (int)BackgroundSongName.Leftheria);
                        break;
                    case ChunkType.Park:
                        _nextSong = BackgroundSongName.Leftheria;
                        AudioEvent?.Invoke(AudioType.BGMusic, (int)BackgroundSongName.Leftheria);
                        break;
                    case ChunkType.CityStreet:
                        BackgroundCityChange(distance);
                        break;
                    case ChunkType.Boss:
                        BackgroundMusicBoss(distance);
                        break;
                }
            
        }
        public void PlaySong(BackgroundSongName song)
        {
                _nextSong = song;
                AudioEvent?.Invoke(AudioType.BGMusic, (int)song);
        }
        public void PlayEffect(EffectName effectName)
        {
            AudioEvent?.Invoke(AudioType.Effect, (int)effectName);
        }
        private void BackgroundCityChange(int distance)
        {
            //
            //.WriteLine("Change City Music");
            //Console.WriteLine(distance);
            if (distance < 25 * 5)
            {
                _nextSong = BackgroundSongName.MorArdain;
                AudioEvent?.Invoke(AudioType.BGMusic, (int)BackgroundSongName.MorArdain);
            }
            else if(distance < 45 * 5)
            {
                _nextSong = BackgroundSongName.OnePunch;
                AudioEvent?.Invoke(AudioType.BGMusic, (int)BackgroundSongName.OnePunch);
            }
            else if(distance < 65 * 5)
            {
                _nextSong = BackgroundSongName.BoomBoom;
                AudioEvent?.Invoke(AudioType.BGMusic, (int)BackgroundSongName.BoomBoom);
            }
            else if (distance < 85 * 5)
            {
                _nextSong = BackgroundSongName.ShootingStars;
                AudioEvent?.Invoke(AudioType.BGMusic, (int)BackgroundSongName.ShootingStars);
            }
            else if(distance < 105 * 5)
            {
                _nextSong = BackgroundSongName.MadMax;
                AudioEvent?.Invoke(AudioType.BGMusic, (int)BackgroundSongName.MadMax);
            }
        }

        private void BackgroundMusicBoss(int distance)
        {
            //Console.WriteLine(distance);
            if(distance < 15 * 5)
            {
                _nextSong = BackgroundSongName.MorArdain;
                AudioEvent?.Invoke(AudioType.BGMusic, (int)BackgroundSongName.Tantal);
            }
            else if(distance < 30 * 5)  //erster BossChunk
            {
                _nextSong = BackgroundSongName.Tantal;
                AudioEvent?.Invoke(AudioType.BGMusic, (int)BackgroundSongName.Tantal);
            }
            else if (distance < 50 * 5)  //zweiter BossChunk
            {
                _nextSong = BackgroundSongName.PepsiMan;
                AudioEvent?.Invoke(AudioType.BGMusic, (int)BackgroundSongName.PepsiMan);
            }
            else if (distance < 70 * 5)  //dritter BossChunk
            {
                _nextSong = BackgroundSongName.DanceTillYouDead;
                AudioEvent?.Invoke(AudioType.BGMusic, (int)BackgroundSongName.DanceTillYouDead);
            }
            else if (distance < 90 * 5)
            {
                _nextSong = BackgroundSongName.Guile;
                AudioEvent?.Invoke(AudioType.BGMusic, (int)BackgroundSongName.Guile);
            }
            else if(distance < 110 * 5)
            {
                _nextSong = BackgroundSongName.Megalovania;
                AudioEvent?.Invoke(AudioType.BGMusic, (int)BackgroundSongName.Megalovania);
            }
        }

        

    }
    //Beispiel: https://docs.microsoft.com/de-de/dotnet/standard/threading/creating-threads-and-passing-data-at-start-time

    public delegate void CallbackMusic(BackgroundSongName backgroundSongName);

    
}
