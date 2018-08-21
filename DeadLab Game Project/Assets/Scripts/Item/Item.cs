using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : InteractionObject {

	public int id {get; protected set;}
    public string itemName {get; protected set;}
    public InterfactionObjectEnum type {get; protected set;}
}
