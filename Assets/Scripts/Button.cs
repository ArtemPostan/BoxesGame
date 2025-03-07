using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    Animator animator;
    [SerializeField] Spawner spawner;
    public bool isPressed = false;    

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {      
        if (collision.gameObject.CompareTag("Player"))
        {            
            ContactPoint contact = collision.contacts[0];
            Vector3 normal = contact.normal;
           
            if (normal.y < -0.5f) 
            {
                collision.gameObject.transform.SetParent(transform, true);
                isPressed = true;                
                animator.SetBool("IsPressed", true);
                spawner.SpawnRandomBox();               
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {      
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.SetParent(null);
            animator.SetBool("IsPressed", false);
            isPressed = false;            
            Debug.Log("Button Released!");
        }
    }

}
