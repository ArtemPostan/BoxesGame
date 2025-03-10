using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{  

    public abstract void IfDetected(PlayerController player);
    public abstract void Interact(PlayerController player);
}
