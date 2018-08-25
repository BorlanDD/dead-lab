using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{

    public static string SINGLE_FIRE = "Fire_Single";
    public static string BURST_FIRE = "Fire_Burst";
    public static string AUTOMATIC_FIRE = "Fire_Automatic";

    public enum ShootingMode
    {
        Locked,
        Single,
        Burst,
        Automatic
    }

    public enum Type
    {
        Pistol,
        Automat
    }

    public Rigidbody bulletPrefab;
    public Transform spawnPoint;

    public ShootingMode currentShootingMode { get; protected set; }
    protected IList<ShootingMode> availableShootingModes;

    protected Animator animator;

    public GameObject magazin;

    public Type weaponType { get; protected set; }
    public int slot;
    protected int maxbulletCounts;
    public int bulletCounts { get; protected set; }
    public int damage { get; protected set; }

    public bool lockedShoot;
    private float lockedShootTimeLeft;

    protected int bulletSpeed;
    protected int burstBulletCount;
    protected int burstShootMade;
    protected float afterSingleDelay;
    public bool singleShootLock { get; protected set; }


    protected float afterAutomaticDelay;

    //Delay beetween bullets
    protected float burstModeDelay;
    public float afterBurstDelay;
    protected float burstModeTimeLeft;
    private bool makeBurstShoot;

    protected float afterAutomaticShotDelay;


    public override void OnAwake()
    {
        animator = GetComponent<Animator>();

        type = ItemType.Weapon;
        availableShootingModes = new List<ShootingMode>();
        currentShootingMode = ShootingMode.Locked;
        availableShootingModes.Add(ShootingMode.Locked);
        currentMode = 0;
        lockedShoot = true;

        singleShootLock = false;

        burstBulletCount = 1;
        makeBurstShoot = false;

        afterAutomaticShotDelay = 0;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Player.GetInstance().usingWeapon != this)
        {
            return;
        }

        if (lockedShoot)
        {
            if (currentShootingMode == ShootingMode.Single)
            {
                if (lockedShootTimeLeft >= afterSingleDelay)
                {
                    lockedShoot = false;
                    lockedShootTimeLeft = 0;
                }
                else
                {
                    lockedShootTimeLeft += Time.deltaTime;
                }
            }
            else if (currentShootingMode == ShootingMode.Automatic)
            {
                if (lockedShootTimeLeft >= afterAutomaticDelay)
                {
                    lockedShoot = false;
                    lockedShootTimeLeft = 0;
                }
                else
                {
                    lockedShootTimeLeft += Time.deltaTime;
                }
            }
            else if (currentShootingMode == ShootingMode.Burst && !makeBurstShoot)
            {
                if (lockedShootTimeLeft >= afterBurstDelay)
                {
                    lockedShoot = false;
                    lockedShootTimeLeft = 0;
                }
                else
                {
                    lockedShootTimeLeft += Time.deltaTime;
                }
            }
        }
    }

    private void ResetBurstShoot()
    {
        makeBurstShoot = false;
        burstShootMade = 0;
        burstModeTimeLeft = 0;
    }

    private ShootingMode shotMode;
    public void Fire()
    {
        if (bulletCounts == 0 || currentShootingMode == ShootingMode.Locked || lockedShoot
         || (currentShootingMode == ShootingMode.Single && singleShootLock))
        {
            return;
        }

        Player player = Player.GetInstance();
        if (animator != null)
        {
            lockedShoot = true;
            if (currentShootingMode == ShootingMode.Single && !singleShootLock)
            {
                animator.SetTrigger("SingleFire");
                singleShootLock = true;
            }
            else if (currentShootingMode == ShootingMode.Burst)
            {
                animator.SetTrigger("BurstFire");
            }
            else if (currentShootingMode == ShootingMode.Automatic)
            {
                MakeShoot();
                if (!Player.GetInstance().animator.GetBool(itemName + "_" + AUTOMATIC_FIRE))
                {
                    Player.GetInstance().animator.SetBool(itemName + "_" + AUTOMATIC_FIRE, true);
                }
            }
        }
        shotMode = currentShootingMode;
    }

    public void StopShooting()
    {
        if (availableShootingModes.Contains(ShootingMode.Automatic) &&  Player.GetInstance().animator.GetBool(itemName + "_" + AUTOMATIC_FIRE))
        {
            Player.GetInstance().animator.SetBool(itemName + "_" + AUTOMATIC_FIRE, false);
        }

        if (singleShootLock)
        {
            singleShootLock = false;
        }
    }

    public void MakeSingleShot()
    {
        Player.GetInstance().animator.SetTrigger(itemName + "_" + SINGLE_FIRE);
    }

    private void MakeShoot()
    {

        Bullet bullet = BulletsPull.GetInstnace().GetBullet();
        bullet.transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.transform.rotation);
        bullet.gameObject.SetActive(true);
        bullet.damage = damage;

        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.velocity = spawnPoint.TransformDirection(new Vector3(0, 0, bulletSpeed));
        bulletCounts--;

        //UserInterface.GetInstance().bulletCounteUpdate(bulletCounts);
        WeaponUI.GetInstance().BulletCountUpdate(bulletCounts);

        if (bulletCounts <= 0)
        {
            Player.GetInstance().ReloadWeapon();
        }

        shotMode = ShootingMode.Locked;
    }

    public void SetShootingMode(ShootingMode mode)
    {
        for (int i = 0; i < availableShootingModes.Count; i++)
        {
            if (mode == availableShootingModes[i])
            {
                currentShootingMode = mode;
                break;
            }
        }
    }

    private int currentMode;

    public void SetNextMode()
    {
        currentMode++;
        if (currentMode >= availableShootingModes.Count)
        {
            currentMode = 0;
        }
        currentShootingMode = availableShootingModes[currentMode];
    }
    public void SetPreviousMode()
    {
        currentMode--;
        if (currentMode < 0)
        {
            currentMode = availableShootingModes.Count - 1;
        }
        currentShootingMode = availableShootingModes[currentMode];
    }

    public void Reload()
    {
        bulletCounts = maxbulletCounts;
        WeaponUI.GetInstance().BulletCountUpdate(bulletCounts);
        //UserInterface.GetInstance().bulletCounteUpdate(bulletCounts);
    }

    public bool NeedToReload()
    {
        if (bulletCounts < maxbulletCounts)
        {
            return true;
        }
        return false;
    }

    public void Setted()
    {
        if (currentShootingMode != ShootingMode.Locked)
        {
            lockedShoot = false;
        }
    }
}
