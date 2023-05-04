using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Model.PawnLogic.ProjectileLogic;
using Extension;
using Model.InterfaceCollection;

namespace Model.PawnLogic.WeaponLogic 
{
    public class SimplePistol : IWeaponLogic
    {
        //private float _size = 0.2f;
        private ProjectileManager _manager;

        public int Ammo => -1;

        public float Damage { get; } = 25;

        public uint MagazineAmmo { get; private set; } = 30;

        public uint MagazineAmmoMax { get; } = 30;

        public WeaponClass.FireMode FireMode => WeaponClass.FireMode.Semi;

        public WeaponClass.MeeleeMode MeeleeMode => WeaponClass.MeeleeMode.None;

        public WeaponManager.AmmoType PrimaryAmmo => WeaponManager.AmmoType.mm9;

        public WeaponManager.AmmoType SecondaryAmmo => WeaponManager.AmmoType.none;

        public bool NeedsReload { get; private set; } = false;

        public SimplePistol(ProjectileManager manager)
        {
            _manager = manager;
        }


        public bool Use(Vector2 originPos, Vector2 originChunk, Vector2 direction)
        {
            _manager.Add(originPos, originChunk, direction, Damage, 0.03f, true);

            return true;

        }

        public void UpdateCooldown(float deltaT)
        {
           
        }

        public void Update(InputData inputData, Pawn pawn, float aspectRatio, Audio.StateAudioChanged audioEvent, float deltaT)
        {
            if (inputData.shoot)
            {
                var dir = WeaponClass.ScreenPosToVectorFromMiddle(inputData.MousePos, aspectRatio);
                Use(pawn.Position, pawn.Chunk, dir);
                pawn.ViewDirection = dir;
            }
        }

        public void Reload(uint ammo)
        {
            NeedsReload = false;
            MagazineAmmo = ammo;
        }
    }
}
