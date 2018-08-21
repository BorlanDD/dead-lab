using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour {

    public int intractableLayerMask;
    public float normalSpeed = 3.0f;
    public float boostSpeed = 4.5f;
    public float gravity = -9.8f;
    public float stepLenght = 1.5f;

    public float staminaUse = 12;
    public float staminaIdleRestore = 10;
    public float staminaWalkRestore = 6;

    private Player player;
    private CharacterController _charController;
    private AudioSource _source;

    [SerializeField] private AudioClip leftLegSound;
    [SerializeField] private AudioClip rightLegSound;

    [SerializeField] private AudioClip lightSwitchOnSound;
    [SerializeField] private AudioClip lightSwitchOffSound;

    private float currentDistance;
    private bool isLeftLeg;
    private Flashlight _flashlight;

    // Use this for initialization
    void Start() {
        intractableLayerMask = 1 << 9;
        player = GetComponent<Player>();

        _charController = GetComponent<CharacterController>();
		_flashlight = Flashlight.GetInstance();
		_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        float currentSpeed = normalSpeed;

        Interaction();

        if (Input.GetKey(KeyCode.LeftShift) && !player.tired) {
            currentSpeed = boostSpeed;
        }

        float deltaX = Input.GetAxis("Horizontal") * currentSpeed;
        float deltaZ = Input.GetAxis("Vertical") * currentSpeed;

        if (player.stamina < 100) {
            float staminaRestore = 0;
            if (deltaX == 0 && deltaZ == 0) {
                staminaRestore = staminaIdleRestore;
            } else if (currentSpeed != boostSpeed) {
                staminaRestore = staminaWalkRestore;
            }
            if (staminaRestore != 0) {
                player.Resting(Time.deltaTime * staminaRestore);
            }
        }

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        movement = Vector3.ClampMagnitude(movement, currentSpeed);

        movement.y = gravity;

        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        Vector3 startingPos = _charController.transform.position;
        _charController.Move(movement);

        currentDistance += Vector3.Distance(startingPos, _charController.transform.position);

        if (currentDistance >= stepLenght) {
            _source.volume = 1.0f;
            if (isLeftLeg) {
                _source.PlayOneShot(leftLegSound);
                isLeftLeg = false;
            } else {
                _source.PlayOneShot(rightLegSound);
                isLeftLeg = true;
            }
            currentDistance = 0f;

            if (Input.GetKey(KeyCode.LeftShift) && !player.tired)
            {
                float staminaNeed = Time.deltaTime * staminaUse * stepLenght * 10;
                player.Sprinting(staminaNeed);
            }
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            _flashlight.Switch();
		}

        if (Input.GetKey(KeyCode.Tab) && !TasksManager.GetInstance().taskHintShowing) {
            TasksManager.GetInstance().ShowTaskHint(true);
        } else if (!Input.GetKey(KeyCode.Tab) && TasksManager.GetInstance().taskHintShowing) {
            TasksManager.GetInstance().ShowTaskHint(false);
        }
    }

    private void Interaction() {
        RaycastHit hit;
        Camera cam = Camera.main;
        float width = cam.pixelWidth / 2;
        float height = cam.pixelHeight / 2;
        Ray ray = cam.ScreenPointToRay(new Vector3(width, height, 0));

        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, intractableLayerMask)) {

            InteractionObject io = hit.transform.GetComponent<InteractionObject>();
            if (io.targetInto) {
                UserInterface.GetInstance().InteractionHintUIState(true);
                if (Input.GetKeyDown(KeyCode.E)) {
                    io.interract();
                }
            } else {
                UserInterface.GetInstance().InteractionHintUIState(false);
            }
            //Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
        } else if (UserInterface.GetInstance().hintInteractionUI.activeSelf) {
            UserInterface.GetInstance().InteractionHintUIState(false);
        }
    }
}
