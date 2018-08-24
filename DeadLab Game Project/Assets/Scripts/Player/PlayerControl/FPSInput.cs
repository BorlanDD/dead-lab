using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{

    public int intractableLayerMask;
    public float normalSpeed = 3.0f;
    public float boostSpeed = 4.5f;
    public float gravity = -9.8f;
    public float stepLenght = 1.5f;

    private Player player;
    private CharacterController _charController;


    // Use this for initialization
    void Start()
    {
        intractableLayerMask = 1 << 9;
        player = GetComponent<Player>();

        _charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        Interaction();
        player.CurrentStatus = Player.Status.Walk;
        float currentSpeed = normalSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!player.tired)
            {
                currentSpeed = boostSpeed;
            }
            player.CurrentStatus = Player.Status.Run;
        }

        float deltaX = Input.GetAxis("Horizontal") * currentSpeed;
        float deltaZ = Input.GetAxis("Vertical") * currentSpeed;

        if (deltaX == 0 && deltaZ == 0)
        {
            player.CurrentStatus = Player.Status.Stand;
        }

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        movement = Vector3.ClampMagnitude(movement, currentSpeed);

        movement.y = gravity;

        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        _charController.Move(movement);

        if (Input.GetKeyDown(KeyCode.F))
        {
            Flashlight.GetInstance().Switch();
        }

        if (Input.GetKey(KeyCode.Tab) && !TasksManager.GetInstance().taskHintShowing)
        {
            TasksManager.GetInstance().ShowTaskHint(true);
        }
        else if (!Input.GetKey(KeyCode.Tab) && TasksManager.GetInstance().taskHintShowing)
        {
            TasksManager.GetInstance().ShowTaskHint(false);
        }

        WeaponInput();
    }

    private void WeaponInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !player.settingWeapon)
        {
            if (player.usingWeapon != null && player.usingWeapon.slot == 1)
            {
                player.UnequipWeapon();
            }
            else
            {
                player.EquipWeapon(WeaponUtils.GetWeaponBySlot(1));
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !player.settingWeapon)
        {
            if (player.usingWeapon != null && player.usingWeapon.slot == 2)
            {
                player.UnequipWeapon();
            }
            else
            {
                player.EquipWeapon(WeaponUtils.GetWeaponBySlot(2));
            }
        }

        if (Input.GetAxis("Fire1") > 0)
        {
            player.Fire();
        }
        else
        {
            if (player.usingWeapon != null && player.usingWeapon.singleShootLock)
            {
                player.usingWeapon.SingleShootUnlock();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player.ReloadWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            player.ChangeFireMode(-1);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            player.ChangeFireMode(1);
        }
    }

    private void Interaction()
    {
        RaycastHit hit;
        Camera cam = Camera.main;
        float width = cam.pixelWidth / 2;
        float height = cam.pixelHeight / 2;
        Ray ray = cam.ScreenPointToRay(new Vector3(width, height, 0));

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 2, intractableLayerMask))
        {
            InteractionObject io = hit.transform.GetComponent<InteractionObject>();
            if (io.targetInto && !io.locked)
            {
                UserInterface.GetInstance().InteractionHintUIState(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    io.Interract();
                }
            }
            else
            {
                UserInterface.GetInstance().InteractionHintUIState(false);
            }
        }
        else if (UserInterface.GetInstance().hintInteractionUI.activeSelf)
        {
            UserInterface.GetInstance().InteractionHintUIState(false);
        }
        //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
    }
}
