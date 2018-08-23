using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int health;
    void Awake()
    {
        OnAwake();
    }

    protected virtual void OnAwake() { }

    // Use this for initialization
    void Start()
    {
        OnStart();
    }

    protected virtual void OnStart() { }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate() { }

    public void Die()
	{
		Destroy(gameObject);
	}
}
