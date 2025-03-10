using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        
            Destroy(gameObject);

    }

    public void MoveShapeToBasket(GameObject shape, Vector3 basketCenter, InteractableBox box)
    {
        StartCoroutine(MoveShapeCoroutine(shape, basketCenter, box));
    }

    private IEnumerator MoveShapeCoroutine(GameObject shape, Vector3 basketCenter, InteractableBox box)
    {
        if (shape.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
        }

        Vector3 startPosition = shape.transform.position;
        Vector3 raisedPosition = startPosition + Vector3.up * 2f;
        float duration = 0.5f;

        float elapsedTime = 0f;
        while (elapsedTime < duration && shape != null)
        {
            shape.transform.position = Vector3.Lerp(startPosition, raisedPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < duration && shape != null)
        {
            shape.transform.position = Vector3.Lerp(raisedPosition, basketCenter, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(shape);

        if (box.figuresCount == 0 && box != null)
        {
            Destroy(box.gameObject);
        }
    }
}
