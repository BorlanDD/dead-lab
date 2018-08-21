using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{

    private int health;
    public float stamina { get; set; }
    public int staminaCriticalLevel { get; private set; }
    public bool tired { get; private set; }

    private static Player player;
    public Inventory inventory {get; private set;}

    void Awake(){
        player = this;
        inventory = GetComponent<Inventory>();
    }
    public static Player GetInstance(){
        return player;
    }


    // Use this for initialization
    void Start()
    {
        stamina = 100;
        staminaCriticalLevel = 20;
        tired = false;
    }

    // Update is called once per frame
    void Update()
    {

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
        if (stamina > 100)
        {
            stamina = 100;
        }
        if (stamina >= staminaCriticalLevel)
        {
            tired = false;
        }
    }


}
