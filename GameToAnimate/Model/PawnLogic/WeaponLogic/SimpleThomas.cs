using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Model.PawnLogic.ProjectileLogic;
using Extension;
using Model.InterfaceCollection;
using System.Diagnostics;

namespace Model.PawnLogic.WeaponLogic
{
    public class SimpleThomas : IWeaponLogic
    {
        //private float _size = 0.2f;
        private ProjectileManager _manager;

        public int Ammo => -1;

        public float Damage { get; } = 1000;

        public uint MagazineAmmo { get; private set; } = 1;

        public uint MagazineAmmoMax { get; } = 1;

        public WeaponClass.FireMode FireMode => WeaponClass.FireMode.Semi;

        public WeaponClass.MeeleeMode MeeleeMode => WeaponClass.MeeleeMode.None;

        public WeaponManager.AmmoType PrimaryAmmo => WeaponManager.AmmoType.thomas;

        public WeaponManager.AmmoType SecondaryAmmo => WeaponManager.AmmoType.none;

        public bool NeedsReload { get; private set; } = false;

        private float _cooldown;
        private float _cooldownTime = 300;
        private float _reloadTime = 5;

        private Stopwatch stopwatch;

        public SimpleThomas(ProjectileManager manager)
        {
            _manager = manager;
            _cooldown = _cooldownTime / 2;
            stopwatch = new Stopwatch();
        }


        public bool Use(Vector2 originPos, Vector2 originChunk, Vector2 direction)
        {
            //_manager.Add(originPos, originChunk, direction, Damage);
            Console.WriteLine("Use Thomas");
            if (MagazineAmmo <= 0) return false;
            _manager.AddThomas(originPos, originChunk, direction, 1000, 2, 10);
            --MagazineAmmo;

            return true;

        }

        public void UpdateCooldown(float deltaT)
        {
            if (_cooldown < _cooldownTime + _reloadTime)
            {
                ++_cooldown;
            }
        }

        public void Update(InputData inputData, Pawn pawn, float aspectRatio, Audio.StateAudioChanged audioEvent, float deltaT)
        {
            UpdateCooldown(deltaT);
            if (MagazineAmmo == 0)
            {
                if (!stopwatch.IsRunning)
                {
                    stopwatch.Restart();
                }
                if (stopwatch.Elapsed.Seconds > _reloadTime)
                {
                    NeedsReload = true;
                    stopwatch.Stop();
                }
                return;
            }
            if (inputData.shootDown)
            {
                Use(pawn.Position, pawn.Chunk, WeaponClass.ScreenPosToVectorFromMiddle(inputData.MousePos, aspectRatio));
            }
        }

        public void Reload(uint ammo)
        {
            NeedsReload = false;
            MagazineAmmo = ammo;
        }
    }
}
