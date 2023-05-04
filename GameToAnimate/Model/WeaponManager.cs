using Model.InterfaceCollection;
using Model.PawnLogic;
using Model.PawnLogic.ProjectileLogic;
using Model.PawnLogic.WeaponLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class WeaponManager
    {
        private IWeaponLogic[] _weaponLogic;
        private uint _activeWeaponLogic = 1;

        public enum AmmoType { none, mm9, mm8short, mm8Rifle, shotgun, rocket, grenade, thomas, length };

        public uint CurrentMagazineAmmo { get => _weaponLogic[_activeWeaponLogic].MagazineAmmo; }

        public uint CurrentMagazineAmmoMax { get => _weaponLogic[_activeWeaponLogic].MagazineAmmoMax; }

        public uint CurrentAmmoOfActiveWeapon { get => _ammoCount[(int)_weaponLogic[_activeWeaponLogic].PrimaryAmmo]; }

        private uint[] _ammoCount;
        private uint[] _defaultAmmoCount;

        private Random random;

        public WeaponManager(ProjectileManager projectileManager)
        {
            InitWeaponLogic(projectileManager);
        }

        private void InitWeaponLogic(ProjectileManager projectileManager)
        {
            _weaponLogic = new IWeaponLogic[9];
            _weaponLogic[0] = new EmptyWeapon();
            _weaponLogic[1] = new SimplePistol(projectileManager);
            _weaponLogic[2] = new SimpleMP(projectileManager);
            _weaponLogic[3] = new SimpleShottie(projectileManager);
            _weaponLogic[4] = new SimpleGrenade(projectileManager);
            _weaponLogic[5] = new SimpleThomas(projectileManager);

            _ammoCount = new uint[(int)AmmoType.length];
            _defaultAmmoCount = new uint[(int)AmmoType.length];
            _defaultAmmoCount[(int)AmmoType.none] = 0;
            _defaultAmmoCount[(int)AmmoType.mm9] = 1000;
            _defaultAmmoCount[(int)AmmoType.shotgun] = 30;
            _defaultAmmoCount[(int)AmmoType.mm8Rifle] = 100;
            _defaultAmmoCount[(int)AmmoType.mm8short] = 1000;
            _defaultAmmoCount[(int)AmmoType.rocket] = 3;
            _defaultAmmoCount[(int)AmmoType.thomas] = 30;
            _defaultAmmoCount[(int)AmmoType.grenade] = 6;

            for (int i = 0; i < (int)AmmoType.length; i++){
                _ammoCount[i] = _defaultAmmoCount[i];
            }

            random = new Random();
        }
        public void SwitchToWeapon(ushort weaponNumber)
        {
            //Console.WriteLine("Weapon " + weaponNumber);
            if(weaponNumber < _weaponLogic.Count())
            {
                _activeWeaponLogic = weaponNumber;
            }
        }

        public void ResetAmmoCount()
        {
            for (int i = 0; i < (int)AmmoType.length; i++)
            {
                _ammoCount[i] = _defaultAmmoCount[i];
            }
        }

        public void Update(InputData inputData, Pawn pawn, float aspectRatio, Audio.StateAudioChanged audioEvent, float deltaT)
        {
            _weaponLogic[_activeWeaponLogic].Update(inputData, pawn, aspectRatio, audioEvent, deltaT);
            if (_weaponLogic[_activeWeaponLogic].NeedsReload)
            {
                var weapon = _weaponLogic[_activeWeaponLogic];
                if (_ammoCount[(int)weapon.PrimaryAmmo] >=  weapon.MagazineAmmoMax )
                {
                    weapon.Reload(weapon.MagazineAmmoMax);
                    _ammoCount[(int)weapon.PrimaryAmmo] -= weapon.MagazineAmmoMax;
                }
                else
                {
                    weapon.Reload(_ammoCount[(int)weapon.PrimaryAmmo]);
                    _ammoCount[(int)weapon.PrimaryAmmo] = 0;
                }
            }
        }

        public void AddAmmo (AmmoType ammoType, uint amount)
        {
            //Andere Funktionsweise von AddAmmo
            _ammoCount[(int)ammoType] += amount;
        }

        public void AddAmmo()
        {
            double ran = random.NextDouble();

            if(ran < 0.02)
            {
                //Console.WriteLine("Thomas");
                if((int)_ammoCount[(int)AmmoType.thomas] < (int)_defaultAmmoCount[(int)AmmoType.thomas])
                {
                    //Console.WriteLine("ThomasAdd");
                    _ammoCount[(int)AmmoType.thomas] += 1; //Thomas
                }
            }
            if(ran >= 0.02 && ran < 0.2)
            {
                //Console.WriteLine("Grenades");
                if ((int)_ammoCount[(int)AmmoType.grenade] <= (int)_defaultAmmoCount[(int)AmmoType.grenade] - 3)
                {
                    //Console.WriteLine("Grenades Add");
                    _ammoCount[(int)AmmoType.grenade] += 3; //Grenades
                }
            }
            if(ran >= 0.2 && ran < 0.60)
            {
                //Console.WriteLine("Shotgun");
                if ((int)_ammoCount[(int)AmmoType.shotgun] <= (int)_defaultAmmoCount[(int)AmmoType.shotgun] - 6)
                {
                    //Console.WriteLine("Shotgun Current-8 " + ((int)_ammoCount[(int)AmmoType.shotgun] - 8));
                    _ammoCount[(int)AmmoType.shotgun] += 8;    //Shotgun
                }
            }
            if(ran >= 0.60 && ran <= 1.0)
            {
                //Console.WriteLine("MP");
                if ((int)_ammoCount[(int)AmmoType.mm9] <= (int)_defaultAmmoCount[(int)AmmoType.mm9] - 200)
                {
                    //Console.WriteLine("MP Add");
                    _ammoCount[(int)AmmoType.mm9] += 200;   //MP
                }
            }
        }
        

        public void IncreaseAmmoCount()
        {

            _defaultAmmoCount[1] += 200;
            _defaultAmmoCount[2] += 200;
            _defaultAmmoCount[3] += 40;
            _defaultAmmoCount[4] += 15;
            _defaultAmmoCount[5] += 2;
            _defaultAmmoCount[6] += 50;
            _defaultAmmoCount[7] += 1;

            /*
            _ammoCount[1] += 200;
            _ammoCount[2] += 200;
            _ammoCount[3] += 40;
            _ammoCount[4] += 15;
            _ammoCount[5] += 2;
            _ammoCount[6] += 50;
            _ammoCount[7] += 1;
            */
        }

        public String GetCurerntWeaponName()
        {
            switch (_activeWeaponLogic)
            {
                case 1: return "PISTOL";
                case 2: return "MP";
                case 3: return "SHOTGUN";
                case 4: return "GRENADES";
                case 5: return "THOMAS";
                default: return "NICHTS";
            }
        }
    }
}
