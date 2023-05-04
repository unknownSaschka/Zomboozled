using Model.PawnLogic.ProjectileLogic;
using System;
using System.Numerics;
using Extension;
using Model.InterfaceCollection;

namespace Model.PawnLogic.WeaponLogic
{
    internal class SimpleMP : IWeaponLogic
    {
        //private float _size = 0.2f;
        private ProjectileManager _manager;
        private float _cooldownTime = 3;

        private float _cooldown;


        private float _reloadTime = 30;

        public int Ammo => -1;
        private static Random ran = new Random();
        private static float _spray = 0.001f;

        public WeaponClass.FireMode FireMode => WeaponClass.FireMode.FullAuto;

        public WeaponClass.MeeleeMode MeeleeMode => WeaponClass.MeeleeMode.None;

        public float Damage { get; } = 35;

        public uint MagazineAmmo { get; private set; } = 30;

        public uint MagazineAmmoMax { get; } = 30;

        public WeaponManager.AmmoType PrimaryAmmo => WeaponManager.AmmoType.mm9;

        public WeaponManager.AmmoType SecondaryAmmo => WeaponManager.AmmoType.none;

        public bool NeedsReload { get; private set; } = false;

        public SimpleMP(ProjectileManager manager)
        {
            _cooldown = _cooldownTime / 2;
            _manager = manager;
        }

        public bool Use(Vector2 originPos, Vector2 originChunk, Vector2 direction)
        {
            if (_cooldown >= _cooldownTime)
            {
                _manager.Add(originPos, originChunk, direction.Rotate((float)ran.NextDouble().Map(0,1, -_spray, _spray)), Damage, 0.03f, true);
                --MagazineAmmo;
                _cooldown = 0;
                return true;
            }
            return false;
        }

        public void Update(InputData inputData, Pawn pawn, float aspectRatio, Audio.StateAudioChanged audioEvent, float deltaT)
        {
            UpdateCooldown(deltaT);
            if(MagazineAmmo == 0)
            {

                    if (_cooldown > _reloadTime)
                    {
                    NeedsReload = true;
                    }
                return;
            }
            if (inputData.shootDown)
            {
                var dir = WeaponClass.ScreenPosToVectorFromMiddle(inputData.MousePos, aspectRatio);
                Use(pawn.Position, pawn.Chunk, dir);
                pawn.ViewDirection = dir;
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