using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class InteractableBox : MonoBehaviour, IInteractable
{
    Vector3 originalScale;
    Rigidbody rb;    
    Box box;
    public ShapeType shapeType;

    public bool _isOpen;
    private bool IsFirstOpen;

    public int figuresCount = 4;

    PlayerController controller;

    Animator textAnimator; 

    private Transform playerTransform;

    private void Start()
    {
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody>();        
        box = GetComponent<Box>();
        shapeType = box.InnerShapeType;
        
    }
    public void Interact(Transform playerTransform)
    {       
        transform.localScale = new Vector3(transform.localScale.x/2, transform.localScale.y/2, transform.localScale.z / 2);
        controller = playerTransform.GetComponent<PlayerController>();
        controller.isBusy = true;
        this.playerTransform = playerTransform;
        textAnimator = controller.OpenCloseBoxText.GetComponent<Animator>();        
        transform.SetParent(playerTransform);
        rb.isKinematic = true;
        rb.detectCollisions = false;
        StartCoroutine(MoveAndRotate(0.5f));
    }   

    public bool IsOpen()
    {
        if (!box.IsOpen)
        {
            box.animator.SetTrigger("Open");           
            controller.OpenCloseBoxText.text = "Закрыть";

            if (IsFirstOpen)
            {
                textAnimator.SetTrigger("Open");
                IsFirstOpen = false;
            }
            _isOpen = true;
            return box.IsOpen = true;
           
        } else
        {
            box.animator.SetTrigger("Close");
            controller.OpenCloseBoxText.text = "Открыть";
            _isOpen = false;
            return box.IsOpen = false;
            
        }
    }

    private IEnumerator MoveAndRotate(float duration)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Динамически обновляем целевую позицию и вращение
            Vector3 targetPosition = controller.boxPlace.position;
            Quaternion targetRotation = playerTransform.rotation;

            // Интерполируем позицию
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            // Интерполируем вращение
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null; // Ждём следующего кадра
        }

        // Убедимся, что объект точно оказался в целевой позиции и вращении
        transform.position = controller.boxPlace.position;
        transform.rotation = playerTransform.rotation;
    }

    public void DieAnimation()
    {
        box.animator.SetTrigger("Die");

    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
