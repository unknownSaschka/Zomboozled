using System.Numerics;
using Extension;
using Model.Audio;
using Model.InterfaceCollection;

namespace Model.PawnLogic.WeaponLogic
{
    public class EmptyWeapon : IWeaponLogic
    {
        public int Ammo => 0;

        public uint MagazineAmmo { get; private set; } = 0;

        public uint MagazineAmmoMax { get; private set; } = 0;

        public float Damage { get; private set; } = 50;

        public WeaponClass.FireMode FireMode => WeaponClass.FireMode.None;

        public WeaponClass.MeeleeMode MeeleeMode => WeaponClass.MeeleeMode.None;
        public WeaponManager.AmmoType PrimaryAmmo => WeaponManager.AmmoType.none;

        public WeaponManager.AmmoType SecondaryAmmo => WeaponManager.AmmoType.none;

        public bool NeedsReload { get; private set; }

        public void Update(InputData inputData, Pawn pawn, float aspectRatio, StateAudioChanged AudioEvent, float deltaT)
        {
        }

        public void UpdateCooldown(float deltaT)
        {
        }

        public bool Use(Vector2 originPos, Vector2 originChunk, Vector2 dir)
        {
            return false;
        }

        public void Reload(uint ammo)
        {
            NeedsReload = false;
            MagazineAmmo = ammo;
        }
    }


    public static class WeaponClass
    {
        public enum FireMode { None, BoltAction, Semi, ThreeShot, FullAuto, FullerAuto };
        public enum MeeleeMode { None, Thrust, Slash};


        public static Vector2 ScreenPosToVectorFromMiddle(Vector2 mousePos, float aspectRatio)
        {
            mousePos.X = mousePos.X.Map(0, 1, -1, 1);
            mousePos.Y = mousePos.Y.Map(0, 1, 1, -1);

            mousePos.X *= aspectRatio;
            mousePos = Vector2.Normalize(mousePos);
            return mousePos;
        }
    }
}
