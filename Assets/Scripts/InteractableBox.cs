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

    PlayerController player;

    Animator textAnimator;     

    private void Start()
    {
        originalScale = transform.localScale;
        rb = GetComponent<Rigidbody>();        
        box = GetComponent<Box>();
        shapeType = box.InnerShapeType;
        
    }
    public void Interact(PlayerController player)
    {       
        transform.localScale = new Vector3(transform.localScale.x/2, transform.localScale.y/2, transform.localScale.z / 2);        
        player.isBusy = true;
        UIManager.Instance.ShowOpenTip();             
        transform.SetParent(player.transform);
        rb.isKinematic = true;
        rb.detectCollisions = false;
        StartCoroutine(MoveAndRotate(0.5f));
    }   

    public void IfDetected(PlayerController player)
    {
        if (player.isBusy)
        {            
            return;
        } 
        UIManager.Instance.ShowInteractTip();
        
    }

    public bool IsOpen()
    {
        if (!box.IsOpen)
        {
            box.animator.SetTrigger("Open");
          
            UIManager.Instance.TextClose();
            //player.boxText.text = "Закрыть";

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
            UIManager.Instance.TextOpen();
           // player.boxText.text = "Открыть";
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
            Vector3 targetPosition = player.boxPlace.position;
            Quaternion targetRotation = player.transform.rotation;

            // Интерполируем позицию
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);

            // Интерполируем вращение
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null; // Ждём следующего кадра
        }

        // Убедимся, что объект точно оказался в целевой позиции и вращении
        transform.position = player.boxPlace.position;
        transform.rotation = player.transform.rotation;
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
