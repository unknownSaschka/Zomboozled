using Model.Audio;
using Model.InterfaceCollection;
using Model.PawnLogic.ProjectileLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Model.PawnLogic.WeaponLogic
{
    public class SimpleGrenade : IWeaponLogic
    {
        public int Ammo => -1;
        private static Random ran = new Random();

        private float _cooldownTime = 3;

        private float _cooldown;


        private float _reloadTime = 30;



        public WeaponClass.FireMode FireMode => WeaponClass.FireMode.FullAuto;

        public WeaponClass.MeeleeMode MeeleeMode => WeaponClass.MeeleeMode.None;

        public float Damage { get; } = 700;

        public uint MagazineAmmo { get; private set; } = 1;

        public uint MagazineAmmoMax { get; } = 1;

        public WeaponManager.AmmoType PrimaryAmmo => WeaponManager.AmmoType.grenade;

        public WeaponManager.AmmoType SecondaryAmmo => WeaponManager.AmmoType.none;

        public bool NeedsReload { get; private set; } = false;

        private ProjectileManager _manager;

        public SimpleGrenade(ProjectileManager manager)
        {
            _manager = manager;
        }

        public bool Use(Vector2 originPos, Vector2 originChunk, Vector2 direction)
        {
            if (_cooldown >= _cooldownTime)
            {
                _manager.AddGrenade(originPos, originChunk, direction, Damage, 5, 0.005f, 0.5f, false);
                --MagazineAmmo;
                _cooldown = 0;
                return true;
            }
            return false;
        }

        public void Update(InputData inputData, Pawn pawn, float aspectRatio, Audio.StateAudioChanged audioEvent, float deltaT)
        {
            UpdateCooldown(deltaT);
            if (MagazineAmmo == 0)
            {
                if (_cooldown > _reloadTime)
                {
                    NeedsReload = true;
                }
                return;
            }
            if (inputData.shoot)
            {
                Use(pawn.Position, pawn.Chunk, WeaponClass.ScreenPosToVectorFromMiddle(inputData.MousePos, aspectRatio));
            }
        }
        public void UpdateCooldown(float deltaT)
        {
            if (_cooldown < _cooldownTime + _reloadTime)
            {
                ++_cooldown;
            }
        }

        public void Reload(uint ammo)
        {
            NeedsReload = false;
            MagazineAmmo = ammo;
        }
    }
}
