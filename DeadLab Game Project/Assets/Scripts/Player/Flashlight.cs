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

	private float batteryLevel;
	private bool active;
	private bool isLowBattery;
	private float maxRange;

	
	private Light _spotlight;
	private AudioSource audioSource;


	public float dischargingRate = 15f;
	public float chargingRate = 20f;
	public float lowLevel = 0.3f;
	public float criticalLevel = 0.1f;
	public float minRange = 4f;


    // Use this for initialization
    void Start () {
		batteryLevel = 1.0f;
		active = true;
		_spotlight = GetComponent<Light>();
		maxRange = _spotlight.range;
		audioSource = GetComponent<AudioSource>();
	}

	//private 
	void Update() {
		if (isBlinked) {
			StartBlink();
			ChangeBatteryLevel(Status.Discharge, Time.deltaTime / dischargingRate);
		} else {
			if (_spotlight.enabled) {
				if (!ChangeBatteryLevel(Status.Discharge, Time.deltaTime / dischargingRate)) {
					_spotlight.enabled = false;
				}
			} else {
				ChangeBatteryLevel(Status.Charge, Time.deltaTime / chargingRate);
				if (active && !isLowBattery) {
					_spotlight.enabled = true;
				}
			}
		}
	}

	private bool ChangeBatteryLevel(Status status, float value) {
		if (status == Status.Charge) {
			if (batteryLevel >= 1.0f) {
				return false;
			}
			if (isLowBattery && batteryLevel >= lowLevel) {
				isLowBattery = false;
			}
			batteryLevel += value;
		} else if (status == Status.Discharge) {
			if (batteryLevel <= 0.0f) {
				isLowBattery = true;
				isBlinked = false;
				return false;
			}
			batteryLevel -= value;
		}
		if (batteryLevel >= minRange / 10) {
			_spotlight.range = batteryLevel * maxRange;
		}

		if (batteryLevel <= criticalLevel && !isLowBattery) {
			isBlinked = true;
		} else {
			isBlinked = false;
		}
		
		return true;
	}

	public void Switch() {
		if (!active) {
			audioSource.PlayOneShot(lightSwitchOnSound);
		} else {
			audioSource.PlayOneShot(lightSwitchOffSound);
		}
		if (!isLowBattery) {
			_spotlight.enabled = active = !active;
		} else {
			active = !active;
		}
	}

	#region BLINKING LIGHT
		private float current = 0f;
		private float delayBetweenBlinks = 1f;
		private float incrementStep = 0.14f;
		private bool isBlinked = false;
		private void StartBlink() {
			if (current >= delayBetweenBlinks) {
				_spotlight.enabled = !_spotlight.enabled;
				current = 0;
			}
			current += incrementStep;
		}
		
	#endregion
}
