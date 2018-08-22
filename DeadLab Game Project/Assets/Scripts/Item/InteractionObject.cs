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

    private void Update()
    {
        OnUpdate();
    }

    public virtual void OnStart()
    {
        targetInto = false;
        locked = false;
    }
    public virtual void OnAwake() { }

    public virtual void OnUpdate() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(targetTag) && !locked)
        {
            targetInto = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(targetTag) && !locked)
        {
            targetInto = false;
        }
    }

    public virtual void Interract()
    {
        if (locked)
        {
            return;
        }
    }

}
