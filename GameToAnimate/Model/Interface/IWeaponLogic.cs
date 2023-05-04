using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Model.PawnLogic;
using Model.PawnLogic.WeaponLogic;

namespace Model.InterfaceCollection
{
    public interface IWeaponLogic
    {
        bool Use(Vector2 originPos, Vector2 originChunk, Vector2 dir);
        int Ammo { get; } // Greater 0 is the Ammo count, -1 is infinte Ammo 
        uint MagazineAmmo { get; }
        uint MagazineAmmoMax { get; }
        float Damage { get; } // Greater 0 is the Ammo count, -1 is infinte Ammo 
        void UpdateCooldown(float deltaT);
        void Update(InputData inputData, Pawn pawn, float aspectRatio, Audio.StateAudioChanged audioEvent, float deltaT);

        bool NeedsReload { get; }

        void Reload(uint ammo);

        WeaponClass.FireMode FireMode { get; }
        WeaponClass.MeeleeMode MeeleeMode { get; }

        WeaponManager.AmmoType PrimaryAmmo { get; }
        WeaponManager.AmmoType SecondaryAmmo { get; }
    }

    

}
