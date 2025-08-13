using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IWorldInteractable
{
    public void OnWorldInteract()
    {
        Interact();
    }

    public virtual void Interact() 
    {

    }
}
