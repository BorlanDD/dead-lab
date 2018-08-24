using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(AudioSource))]
public class Flashlight : MonoBehaviour {

	#region SINGLETON PATTERN
		private static Flashlight instance;
		void Awake() {
			instance = this;
		}

		public static Flashlight GetInstance() {
			return instance;
		}

	#endregion

	public enum Status {
		Charge = 0,
		Discharge = 1
	}
		
	[SerializeField] private AudioClip lightSwitchOnSound;
	[SerializeField] private AudioClip lightSwitchOffSound;

	public float BatteryLevel {get; set;}
	public bool Active {get; set;}
	public bool IsLowBattery {get; set;}
	private float maxRange;

	public bool RaysActive
    {
        get
        {
            return _spotlight.enabled;
        }
    }


    private Light _spotlight;
	private AudioSource audioSource;


	public float dischargingRate = 15f;
	public float chargingRate = 20f;
	public float lowLevel = 0.3f;
	public float criticalLevel = 0.1f;
	public float minRange = 0.4f;


    // Use this for initialization
    void Start () {
		BatteryLevel = 1.0f;
		Active = true;
		_spotlight = GetComponent<Light>();
		maxRange = _spotlight.range;
		audioSource = GetComponent<AudioSource>();
	}

	//private 
	void Update() {
		if (isBlinked) {
			Blink();
			ChangeBatteryLevel(Status.Discharge, Time.deltaTime / dischargingRate);
		} else {
			if (_spotlight.enabled) {
				if (!ChangeBatteryLevel(Status.Discharge, Time.deltaTime / dischargingRate)) {
					_spotlight.enabled = false;
				}
			} else {
				ChangeBatteryLevel(Status.Charge, Time.deltaTime / chargingRate);
				if (Active && !IsLowBattery) {
					_spotlight.enabled = true;
				}
			}
		}

		//UserInterface.GetInstance().ChangeBatteryLevel(BatteryLevel);
	}

	private bool ChangeBatteryLevel(Status status, float value) {
		if (status == Status.Charge) {
			if (BatteryLevel >= 1.0f) {
				return false;
			}
			if (IsLowBattery && BatteryLevel >= lowLevel) {
				IsLowBattery = false;
			}
			BatteryLevel += value;
		} else if (status == Status.Discharge) {
			if (BatteryLevel <= 0.0f) {
				IsLowBattery = true;
				isBlinked = false;
				return false;
			}
			BatteryLevel -= value;
		}
		if (BatteryLevel >= minRange) {
			_spotlight.range = BatteryLevel * maxRange;
		}

		if (BatteryLevel <= criticalLevel && !IsLowBattery && Active) {
			isBlinked = true;
		} else {
			isBlinked = false;
		}
		
		return true;
	}

	public void Switch() {
		if (!Active) {
			audioSource.PlayOneShot(lightSwitchOnSound);
		} else {
			audioSource.PlayOneShot(lightSwitchOffSound);
		}
		if (!IsLowBattery) {
			//isBlinked = false;
			_spotlight.enabled = Active = !Active;
		} else {
			Active = !Active;
		}
	}

	#region BLINKING LIGHT
		private float current = 0f;
		private float delayBetweenBlinks = 1f;
		private float incrementStep = 0.14f;
		private bool isBlinked = false;
		private void Blink() {
			if (current >= delayBetweenBlinks) {
				_spotlight.enabled = !_spotlight.enabled;
				current = 0;
			}
			current += incrementStep;
		}
		
	#endregion
}
