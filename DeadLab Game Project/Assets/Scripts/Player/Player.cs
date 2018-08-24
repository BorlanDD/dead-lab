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


    public float staminaUse = 0.12f;
    public float staminaIdleRestore = 0.1f;
    public float staminaWalkRestore = 0.05f;

    private float prevDistance;

    void Update()
    {
        currentDistance += Vector3.Distance(prevPositionPlayer, transform.position);
        if (currentDistance >= prevDistance) {

        } else {

        }
        prevDistance = currentDistance;
        
        prevPositionPlayer = transform.position;

        if (stamina < 1f)
        {
            float staminaRestore = 0;
            if (CurrentStatus == Status.Stand)
            {
                staminaRestore = staminaIdleRestore;
            }
            else if (CurrentStatus == Status.Walk)
            {
                staminaRestore = staminaWalkRestore;
            }
            if (staminaRestore != 0)
            {
                player.Resting(Time.deltaTime / staminaRestore);
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

            if (CurrentStatus == Status.Run && !player.tired)
            {
                float staminaNeed = Time.deltaTime * staminaUse * stepLenght / 10;
                player.Sprinting(staminaNeed);
            }
            currentDistance = 0f;
        }

        UserInterface.GetInstance().SetNeededStatus(CurrentStatus, tired);
        UserInterface.GetInstance().ChangeStaminaLevel(stamina, tired, CurrentStatus);
    }


    #endregion

    private int health;
    public float stamina { get; set; }
    public float staminaCriticalLevel { get; private set; }
    public bool tired { get; private set; }

    public Weapon usingWeapon { get; private set; }

    private static Player player;
    public Inventory inventory;

    public Transform handPosition;


    void Awake()
    {
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

    public void SetWeapon(Weapon weapon)
    {
        if (weapon == null)
        {
            return;
        }
        if (usingWeapon != null)
        {
            inventory.AddItem(usingWeapon);
        }

        weapon.transform.SetParent(handPosition);
        weapon.transform.position = handPosition.transform.position;
        weapon.transform.rotation = new Quaternion(0, 0, 0, 0);
        weapon.gameObject.SetActive(true);
        usingWeapon = weapon;
        UserInterface.GetInstance().bulletCounteUpdate(usingWeapon.bulletCounts);
    }

    public void Fire()
    {
        if (usingWeapon != null)
        {
            usingWeapon.Fire();
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
        if (!usingWeapon.NeedToReload())
        {
            return;
        }
        usingWeapon.Reload();
    }

}
