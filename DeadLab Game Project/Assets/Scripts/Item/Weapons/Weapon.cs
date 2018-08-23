using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{

    public enum ShootingMode
    {
        Locked,
        Single,
        Burst,
        Automatic
    }

    public Rigidbody bulletPrefab;
    public Transform spawnPoint;

    protected ShootingMode currentShootingMode;
    protected IList<ShootingMode> availableShootingModes;

    protected Animator animator;
    public int slot;
    protected int maxbulletCounts;
    public int bulletCounts { get; protected set; }
    public int damage { get; protected set; }

    protected bool lockedShoot;
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
        lockedShoot = false;

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

    public void Fire()
    {
        if (currentShootingMode == ShootingMode.Locked || lockedShoot
         || (currentShootingMode == ShootingMode.Single && singleShootLock))
        {
            return;
        }

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
            }
        }
    }

    private bool MakeShoot()
    {
        if (bulletCounts <= 0)
        {
            return false;
        }

        Bullet bullet = BulletsPull.GetInstnace().GetBullet();
        bullet.transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.transform.rotation);
        bullet.gameObject.SetActive(true);
        bullet.damage = damage;

        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.velocity = spawnPoint.TransformDirection(new Vector3(0, 0, bulletSpeed));
        bulletCounts--;

        UserInterface.GetInstance().bulletCounteUpdate(bulletCounts);

        if (bulletCounts <= 0)
        {
            Player.GetInstance().ReloadWeapon();
        }
        return true;
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

    public void SingleShootUnlock()
    {
        singleShootLock = false;
    }

    public void Reload()
    {
        bulletCounts = maxbulletCounts;
        UserInterface.GetInstance().bulletCounteUpdate(bulletCounts);
    }

    public bool NeedToReload()
    {
        if (bulletCounts < maxbulletCounts)
        {
            return true;
        }
        return false;
    }
}
