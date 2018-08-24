using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightUI : MonoBehaviour {
	public Image batteryLevel;
    public Image switchOnButton;
    public Image switchOffButton;
    public Image rays;


	public void ChangeBatteryLevel()
    {
        batteryLevel.fillAmount = _flashlight.BatteryLevel;
        if (_flashlight.IsLowBattery)
        {
            batteryLevel.color = Color.red;
            if (_flashlight.Active)
            {
                BlinkColor();
            }
            else
            {
                batteryLevel.enabled = true;
            }
        }
        else
        {
            if (_flashlight.BatteryLevel <= _flashlight.criticalLevel)
            {
                batteryLevel.color = Color.red;
            }
            else
            {
                batteryLevel.color = Color.white;
                batteryLevel.enabled = true;
            }
        }
        rays.enabled = _flashlight.RaysActive;

        switchOnButton.enabled = _flashlight.Active;
        switchOffButton.enabled = !_flashlight.Active;

        if (_flashlight.BatteryLevel >= _flashlight.minRange)
        {
            rays.fillAmount = _flashlight.BatteryLevel;
        }

    }

	private float current = 0f;
    private float delayBetweenBlinks = 1f;
    private float incrementStep = 0.1f;
    public void BlinkColor()
    {
        if (current >= delayBetweenBlinks)
        {
            batteryLevel.enabled = !batteryLevel.enabled;
            current = 0;
        }
        current += incrementStep;
    }

	private Flashlight _flashlight;
	// Use this for initialization
	void Start () {
		_flashlight = Flashlight.GetInstance();
	}
	
	// Update is called once per frame
	void Update () {
		ChangeBatteryLevel();
	}
}
