using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnna : NPC
{
    [Header("NPCAnna: ")]
    [SerializeField] private List<Dialogue> dialogues1;

    public override void Interact()
    {
        base.Interact();

    }
}
