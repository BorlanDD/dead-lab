using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DoorScript : MonoBehaviour
{

    private Animator _animator;

    public bool locked {get; set;}

    // Use this for initialization
    void Start()
    {
        locked = false;
        _animator = GetComponent<Animator>();
		OnStart();
    }

    // Update is called once per frame
    void Update()
    {
		OnUpdate();
    }

	protected virtual void OnStart(){}
	protected virtual void OnUpdate(){}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !locked)
        {
            OnOpen();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !locked)
        {
            OnClose();
        }
    }

    protected virtual void OnOpen()
    {
        _animator.SetTrigger("toOpen");
    }

    protected virtual void OnClose()
    {
        _animator.SetTrigger("toClose");
    }
}
