using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUI : MonoBehaviour
{

    public Image staminaLevel;

    private Player _player;

    // Use this for initialization
    void Start()
    {
        _player = Player.GetInstance();
    }

    // Update is called once per frame
    void Update()
    {
        staminaLevel.fillAmount = _player.stamina;
        if (_player.stamina <= _player.staminaCriticalLevel && _player.tired)
        {
            staminaLevel.color = Color.red;
            if (_player.CurrentStatus == Player.Status.Run)
            {
                BlinkStaminaColor();
            }
            else 
            {
                staminaLevel.enabled = true;
            }
        }
        else
        {
            staminaLevel.color = Color.white;
            staminaLevel.enabled = true;
        }
    }

    private float current = 0f;
    private float delayBetweenBlinks = 0.8f;
    private float incrementStep = 0.1f;
    public void BlinkStaminaColor()
    {
        if (current >= delayBetweenBlinks)
        {
            staminaLevel.enabled = !staminaLevel.enabled;
            current = 0;
        }
        current += incrementStep;
    }
}
