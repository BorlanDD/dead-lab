using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{

    public string targetTag = "Player";
    public bool targetInto { get; private set; }
    public bool locked { get; set; }

    private void Start()
    {
        OnStart();
    }

    private void Awake()
    {
        OnAwake();
    }

    public virtual void OnStart()
    {
        targetInto = false;
        locked = false;
    }
    public virtual void OnAwake() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(targetTag))
        {
            targetInto = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(targetTag))
        {
            targetInto = false;
        }
    }

    public virtual void Interract() {
        if(locked){
            return;
        }
    }

}
