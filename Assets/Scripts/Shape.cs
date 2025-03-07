using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UIElements;

public class Shape : MonoBehaviour
{
    InteractableBox box;

    private void Start()
    {
        box = GetComponentInParent<InteractableBox>();
    }
    public void StartAnimationCoroutine(InteractableBasket basket)
    {
        StartCoroutine(MoveShapeToBasket(this.gameObject, basket.transform.position));
        
    }

    private IEnumerator MoveShapeToBasket(GameObject shape, Vector3 basketCenter)
    {      
        if (shape.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
        }

        Vector3 startPosition = shape.transform.position;
        Vector3 raisedPosition = startPosition + Vector3.up * 2f;
        float duration = 0.5f;

        // Поднимаем фигуру вверх
        float elapsedTime = 0f;
        while (elapsedTime < duration && shape != null)
        {
            shape.transform.position = Vector3.Lerp(startPosition, raisedPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Ждём следующего кадра
        }

        // Перемещаем фигуру в корзину
        elapsedTime = 0f;
        while (elapsedTime < duration && shape != null)
        {
            shape.transform.position = Vector3.Lerp(raisedPosition, basketCenter, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Ждём следующего кадра
        }
        Debug.Log(box.figuresCount);
        if (box.figuresCount == 0)
        {
            box.DieAnimation();
        }
        Destroy(gameObject);
    }
}
