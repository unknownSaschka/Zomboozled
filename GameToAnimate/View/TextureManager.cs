using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenseless.ExampleFramework;
using Zenseless.HLGL;
using Zenseless.OpenGL;

namespace View
{
    public class TextureManager
    {
        public ITexture playerTexture;
        public ITexture youDiedTexture;
        public ITexture streetTexture;
        public ITexture healthItemTexture;
        public ITexture wallTexture;
        public ITexture grassTexture;
        public ITexture building1Texture;
        public ITexture car1Texture;
        public ITexture enemyTexture1;
        public ITexture shopGroundTexture;
        public ITexture heartTexture;
        public ITexture resistanceTexture;
        public ITexture ammoTexture;
        public ITexture ammoMaxTexture;
        public ITexture damageUpTexture;
        public ITexture moneyTexture;
        public ITexture speedTexture;
        public ITexture birdTexture;
        public ITexture tank1Texture;
        public ITexture thomasTexture;
        public ITexture BulletTexture;
        public ITexture TootTexture;
        public ITexture ammoItemTexture;
        public ITexture pepsiManTexture;
        public ITexture kellerZeldaCDITexture;
        public ITexture sanicTexture;
        public ITexture theSomethingTexture;
        public ITexture sansTexture;
        public ITexture GrenadeTexture { get; internal set; }
        public ITexture ButtonTexture;
        public ITexture LogoTexture;
        public ITexture TootIntense;
        public ITexture ArrowTexture;
        public ITexture FogTexture;
        public SpriteSheet _streetConnectedSheet;
        public SpriteSheetAnimation explosion;
        public SpriteSheetAnimation granate;

        public void LoadImages(ExampleWindow window)
        {
            youDiedTexture = window.ContentLoader.Load<ITexture2D>("YouDied.png");
            streetTexture = window.ContentLoader.Load<ITexture2D>("Street.png");
            healthItemTexture = window.ContentLoader.Load<ITexture2D>("health.png");
            resistanceTexture = window.ContentLoader.Load<ITexture2D>("resistance.png");
            ammoTexture = window.ContentLoader.Load<ITexture2D>("ammo.png");
            ammoMaxTexture = window.ContentLoader.Load<ITexture2D>("ammomax.png");
            damageUpTexture = window.ContentLoader.Load<ITexture2D>("damageup.png");
            wallTexture = window.ContentLoader.Load<ITexture2D>("anotherBrickInAWall.png");
            grassTexture = window.ContentLoader.Load<ITexture2D>("grass.png");
            building1Texture = window.ContentLoader.Load<ITexture2D>("WTHDach.png");
            car1Texture = window.ContentLoader.Load<ITexture2D>("car.png");
            //enemyTexture1 = window.ContentLoader.Load<ITexture2D>("Zombie64.png");
            enemyTexture1 = window.ContentLoader.Load<ITexture2D>("AnotherZombie.png");
            shopGroundTexture = window.ContentLoader.Load<ITexture2D>("Holzboden.png");
            heartTexture = window.ContentLoader.Load<ITexture2D>("heart.png");
            moneyTexture = window.ContentLoader.Load<ITexture2D>("money.png");
            speedTexture = window.ContentLoader.Load<ITexture2D>("speed.png");
            birdTexture = window.ContentLoader.Load<ITexture2D>("Burp2.png");
            playerTexture = window.ContentLoader.Load<ITexture2D>("Player.png");
            GrenadeTexture = window.ContentLoader.Load<ITexture2D>("Grenade.png");
            tank1Texture = window.ContentLoader.Load<ITexture2D>("tank.png");
            thomasTexture = window.ContentLoader.Load<ITexture2D>("thomas.png");
            BulletTexture = window.ContentLoader.Load<ITexture2D>("Bullet.png");
            TootTexture = window.ContentLoader.Load<ITexture2D>("tootIItoot.png");
            TootIntense = window.ContentLoader.Load<ITexture2D>("TOOTINTESIFIES13.png");
            ButtonTexture = window.ContentLoader.Load<ITexture2D>("Button.png");
            LogoTexture = window.ContentLoader.Load<ITexture2D>("Logo.png");
            ArrowTexture = window.ContentLoader.Load<ITexture2D>("Arrow3.png");
            ammoItemTexture = window.ContentLoader.Load<ITexture2D>("ammoItem.png");
            FogTexture = window.ContentLoader.Load<ITexture2D>("nebel3.png");
            pepsiManTexture = window.ContentLoader.Load<ITexture2D>("pepsiman.png");
            kellerZeldaCDITexture = window.ContentLoader.Load<ITexture2D>("BOI.png");
            sanicTexture = window.ContentLoader.Load<ITexture2D>("GOTTAGOFST.png");
            theSomethingTexture = window.ContentLoader.Load<ITexture2D>("Anotherboss.png");
            sansTexture = window.ContentLoader.Load<ITexture2D>("SANS.png");
            //animation using a single SpriteSheet

            //explosion = new SpriteSheetAnimation(new SpriteSheet(window.ContentLoader.Load<ITexture2D>("explosion"), 5, 5), 0, 24, 1f);
            explosion = new SpriteSheetAnimation(new SpriteSheet(window.ContentLoader.Load<ITexture2D>("explo.png"), 4, 2), 0, 7, 1f);
            granate = new SpriteSheetAnimation(new SpriteSheet(window.ContentLoader.Load<ITexture2D>("granate.png"), 10, 1), 0, 7, 1f);
            _streetConnectedSheet = new SpriteSheet(window.ContentLoader.Load<ITexture2D>("Street3x5.png"), 3, 5, 1 - 2 / 66f, 1 - 2 / 66f);
            _streetConnectedSheet.Tex.Filter = TextureFilterMode.Nearest;
        }

    }    
}
