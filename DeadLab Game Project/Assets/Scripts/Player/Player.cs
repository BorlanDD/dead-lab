using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Borland's code
    public enum Status
    {
        Stand,
        Walk,
        Run
    }


    [SerializeField] private AudioClip soundFirstLeg;
    [SerializeField] private AudioClip soundSecondLeg;
    private AudioSource audioSource;

    public Status CurrentStatus { get; set; }

    private Vector3 prevPositionPlayer;

    public float stepLenght = 1.6f;
    private bool isFirstLeg = false;
    private float currentDistance;


    public float staminaUse = 20f;
    public float staminaIdleRestore = 25f;
    public float staminaWalkRestore = 30f;

    private float prevDistance;

    public bool IsInMotion { get; set; }

    private float eps = 0.0001f;

    void FixedUpdate()
    {
        currentDistance += Vector3.Distance(prevPositionPlayer, transform.position);
        if (currentDistance > prevDistance + eps)
        {
            IsInMotion = true;
        }
        else
        {
            IsInMotion = false;
        }

        prevDistance = currentDistance;
        prevPositionPlayer = transform.position;

        if (!animator.GetBool("Walk") && IsInMotion)
        {
            animator.SetBool("Walk", true);
        }
        else if (CurrentStatus == Status.Stand)
        {
            animator.SetBool("Walk", false);
        }


        if (IsInMotion && CurrentStatus == Status.Run && !tired)
        {
            player.Sprinting(Time.deltaTime / staminaUse);
        }
        else
        {
            if (stamina < 1f)
            {
                float staminaRestore = 0;
                if (CurrentStatus == Status.Stand || !IsInMotion)
                {
                    staminaRestore = staminaIdleRestore;
                }
                else if (CurrentStatus == Status.Walk)
                {
                    staminaRestore = staminaWalkRestore;
                }
                else if (CurrentStatus == Status.Run && tired)
                {
                    staminaRestore = staminaWalkRestore;
                }
                if (staminaRestore != 0)
                {
                    player.Resting(Time.deltaTime / staminaRestore);
                }
            }
        }



        if (currentDistance >= stepLenght)
        {
            if (isFirstLeg)
            {
                audioSource.PlayOneShot(soundFirstLeg);
                isFirstLeg = false;
            }
            else
            {
                audioSource.PlayOneShot(soundSecondLeg);
                isFirstLeg = true;
            }
            prevDistance = currentDistance = 0f;
        }
    }



    #endregion

    public static string EQUIP_ANIMATION = "_Equip";
    public static string UNEQUIP_ANIMATION = "_Unequip";

    public Animator animator;

    private int health;
    public float stamina { get; set; }
    public float staminaCriticalLevel { get; private set; }
    public bool tired { get; private set; }

    public Weapon usingWeapon { get; private set; }

    private static Player player;
    public Inventory inventory;

    public Transform automatHandPosition;

    public Transform pistolHandPosition;
    public Transform glock18MagPos;

    void Awake()
    {
        // animator = GetComponent<Animator>();
        usingWeapon = null;
        player = this;
    }
    public static Player GetInstance()
    {
        return player;
    }


    // Use this for initialization
    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        prevPositionPlayer = transform.position;
        stamina = 1f;
        staminaCriticalLevel = 0.3f;
        tired = false;
        settingWeapon = false;
    }

    public void Sprinting(float staminaNeed)
    {
        stamina -= staminaNeed;
        if (stamina <= 0)
        {
            stamina = 0;
            tired = true;
        }
    }

    public void Resting(float staminaRest)
    {
        stamina += staminaRest;
        if (stamina > 1f)
        {
            stamina = 1f;
        }
        if (stamina >= staminaCriticalLevel)
        {
            tired = false;
        }
    }

    public bool settingWeapon;

    public void UnequipWeapon()
    {
        if (lastWeapon == null && usingWeapon != null)
        {
            lastWeapon = usingWeapon;
        }
        else if (lastWeapon == null)
        {
            return;
        }
        usingWeapon = null;
        animator.SetTrigger(lastWeapon.itemName + UNEQUIP_ANIMATION);
    }

    private Weapon lastWeapon;
    public void TakeOffWeapon()
    {
        if (lastWeapon == null || (settingWeapon && lastWeapon == usingWeapon))
        {
            return;
        }

        lastWeapon.gameObject.SetActive(false);
        inventory.AddItem(lastWeapon);
        lastWeapon = null;
    }

    public void SetWeapon()
    {
        if (usingWeapon == null)
        {
            return;
        }

        usingWeapon.gameObject.SetActive(true);
        settingWeapon = false;
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (settingWeapon)
        {
            return;
        }
        settingWeapon = true;

        if (weapon == null)
        {
            return;
        }
        if (usingWeapon != null)
        {
            lastWeapon = usingWeapon;
            UnequipWeapon();
        }


        Transform handPos = null;
        if (weapon.weaponType == Weapon.Type.Pistol)
        {
            handPos = pistolHandPosition;
        }
        else if (weapon.weaponType == Weapon.Type.Automat)
        {
            handPos = automatHandPosition;
        }

        weapon.transform.SetParent(handPos);
        weapon.transform.position = handPos.transform.position;
        weapon.transform.rotation = new Quaternion(0, 0, 0, 0);
        usingWeapon = weapon;
        //UserInterface.GetInstance().bulletCounteUpdate(usingWeapon.bulletCounts);
        WeaponUI.GetInstance().UpdateSprite(weapon);
        WeaponUI.GetInstance().BulletCountUpdate(usingWeapon.bulletCounts);

        animator.SetTrigger(weapon.itemName + EQUIP_ANIMATION);
    }

    public void Fire()
    {
        if (usingWeapon != null)
        {
            usingWeapon.Fire();
        }
    }

    public void StopShooting()
    {
        if (usingWeapon != null)
        {
            usingWeapon.StopShooting();
        }
    }

    public void ChangeFireMode(int direction)
    {
        if (usingWeapon == null)
        {
            return;
        }

        if (direction == 1)
        {
            usingWeapon.SetNextMode();
        }
        else if (direction == -1)
        {
            usingWeapon.SetPreviousMode();
        }
    }

    public void ReloadWeapon()
    {
        if (usingWeapon == null || !usingWeapon.NeedToReload())
        {
            return;
        }
        animator.SetTrigger(usingWeapon.itemName + "_Reload");
    }

    public void ReloadedWeapon()
    {
        usingWeapon.Reload();
    }
}
