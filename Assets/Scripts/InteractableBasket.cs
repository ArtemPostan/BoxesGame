using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class InteractableBasket : MonoBehaviour, IInteractable
{
    public ShapeType currentShapeType;
    Box boxObj;
    Shape[] shapesInBox;

    [SerializeField] private TextMeshProUGUI SphereText;
    [SerializeField] private TextMeshProUGUI CubeText;
    [SerializeField] private TextMeshProUGUI CylinderText;

    PlayerController player;

    public void Interact(PlayerController player)
    {
        InteractableBox box = player.GetComponentInChildren<InteractableBox>();
        
        if (player.isBusy)
        {
            boxObj = player.GetComponentInChildren<Box>();
            shapesInBox = boxObj.GetComponentsInChildren<Shape>();
            if (box.figuresCount > 0 && box._isOpen && currentShapeType == box.shapeType && box != null)
            {
                shapesInBox[box.figuresCount-1].StartAnimationCoroutine(this);
                box.figuresCount--;
                Progress.Instance.CollectShape(currentShapeType);                
            }
            if (box.figuresCount == 0)
            {
                player.isBusy = false;
            }
        }        
    }  

    public void IfDetected(PlayerController player)
    {
        if (!player.isBusy)
        {
            UIManager.Instance.ShowInteractTip();
        } else
        {
            UIManager.Instance.HideInteractTip();
        }
        
    }


}