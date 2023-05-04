using Model.PawnLogic.ProjectileLogic;
using System;
using System.Numerics;
using Extension;
using Model.InterfaceCollection;
namespace Model.PawnLogic.WeaponLogic
{
    public class SimpleShottie : IWeaponLogic
    {
        //private float _size = 0.2f;
        private ProjectileManager _manager;
        private float _cooldownTime = 30.0f;

        private float _cooldown;


        private float _reloadTime = 100;

        public int Ammo => -1;
        private static Random ran = new Random();
        private static float _spray = 0.0015f;

        public WeaponClass.FireMode FireMode => WeaponClass.FireMode.FullAuto;

        public WeaponClass.MeeleeMode MeeleeMode => WeaponClass.MeeleeMode.None;

        public float Damage => 25;

        public uint MagazineAmmo { get; private set; } = 8;

        public uint MagazineAmmoMax { get; private set; } = 8;

        public WeaponManager.AmmoType PrimaryAmmo => WeaponManager.AmmoType.shotgun;

        public WeaponManager.AmmoType SecondaryAmmo => WeaponManager.AmmoType.none;

        private uint _amountProjectiles = 30;

        public SimpleShottie(ProjectileManager manager)
        {
            _cooldown = _cooldownTime / 2;
            _manager = manager;
        }

        public bool NeedsReload { get; private set; } = false;

        public bool Use(Vector2 originPos, Vector2 originChunk, Vector2 direction)
        {
            if (_cooldown >= _cooldownTime)
            {
                for(int i=0; i<_amountProjectiles; ++i)
                {
                    _manager.Add(originPos, originChunk, direction.Rotate((float)ran.NextDouble().Map(0, 1, -_spray, _spray)), Damage, 0.03f, true);
                    
                }
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
                if(_cooldown > _reloadTime)
                {
                    NeedsReload = true;
                }
                return;
            }
            if (inputData.shootDown)
            {
                var dir = WeaponClass.ScreenPosToVectorFromMiddle(inputData.MousePos, aspectRatio);
                if (Use(pawn.Position, pawn.Chunk, dir))
                {
                    
                    Use(pawn.Position, pawn.Chunk, dir);
                    pawn.ViewDirection = dir;
                    audioEvent?.Invoke(Audio.AudioType.Effect ,2);
                }
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