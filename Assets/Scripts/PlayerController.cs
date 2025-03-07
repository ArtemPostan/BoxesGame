using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float jumpDelay = 0.0f;

    [Header("Look Settings")]
    public float mouseSensitivity = 100.0f;
    public Transform cameraTransform;
    public Transform boxPlace;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isInteracting;
    private bool isGrounded;
    public bool isBusy = false;

    private PlayerInput playerInput;
    private Animator animator;
    public InteractableBox box;


    [SerializeField] Image InteractableTip;
    [SerializeField] public TextMeshProUGUI OpenCloseBoxText;



    public LayerMask interractLayerMask;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ��������� ��������
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        move.Normalize(); // ����������� ������, ����� �������� ��������� ��� �������� �� ���������
        rb.velocity = new Vector3(move.x * movementSpeed, rb.velocity.y, move.z * movementSpeed);

        // ��������� �������� ������
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        float newRotationX = cameraTransform.localEulerAngles.x - mouseY;
        if (newRotationX > 180) newRotationX -= 360;
        newRotationX = Mathf.Clamp(newRotationX, -90f, 90f);
        cameraTransform.localEulerAngles = new Vector3(newRotationX, 0f, 0f);

        DetectInteractableObjects();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        bool isMoving = moveInput.magnitude > 0;
        animator.SetBool("IsWalking", isMoving);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            animator.SetTrigger("IsJumping");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // ��������� ���� ������                      
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractWithObject();
        }
    }

    private void DetectInteractableObjects()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 3f, interractLayerMask);

        // ���� ����� �� ����� (!isBusy) � ��� ����� � ������ �� ���� interractLayerMask
        if (!isBusy && isHit)
        {
            
            InteractableTip.gameObject.SetActive(true); // ���������� ���������
            OpenCloseBoxText.gameObject.SetActive(false); // �������� �����
        }
        // ���� ����� ����� (isBusy) � ���� ������� (box != null)
        else if (isBusy)
        {           
            if (isHit && hit.collider != null && hit.collider.GetComponent<InteractableBasket>() != null)
            {
                InteractableBasket interactableBasket = hit.collider.gameObject.GetComponent<InteractableBasket>();
                
                // ���� ������� ������� (box._isOpen), ���������� InteractableTip
                if (box._isOpen && box.shapeType == interactableBasket.currentShapeType)
                {
                    
                    InteractableTip.gameObject.SetActive(true); // ���������� ���������
                    OpenCloseBoxText.gameObject.SetActive(false); // �������� �����
                }
                else
                {
                    // ���� ������� �������, �������� InteractableTip
                    InteractableTip.gameObject.SetActive(false); // �������� ���������
                    OpenCloseBoxText.gameObject.SetActive(true); // �������� �����
                }
            }
            else
            {
                InteractableTip.gameObject.SetActive(false); // �������� ���������
                OpenCloseBoxText.gameObject.SetActive(true); // ���������� �����
            }
        }
        // �� ���� ��������� �������
        else
        {
            InteractableTip.gameObject.SetActive(false); // �������� ���������
            OpenCloseBoxText.gameObject.SetActive(false); // �������� �����
        }
    }
    private void InteractWithObject()
    {

        RaycastHit hit;
        bool isRayCastSeeInteractable = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 3f, interractLayerMask);

        if (isRayCastSeeInteractable)
        {
            // ���������, ������� �� �� �� InteractableBasket � ������ �� �� (isBusy)
            if (hit.collider.GetComponent<InteractableBasket>() && isBusy)
            {                
                if (box != null && box._isOpen)
                {
                    IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
                    interactable.Interact(transform);
                } else
                {
                    OpenBox();
                }
            }
            // ���������, ������� �� �� �� InteractableBox � �� ������ �� �� (!isBusy)
            else if (hit.collider.GetComponent<InteractableBox>() && !isBusy)
            {
                
                IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();
                interactable.Interact(transform);
                box = GetComponentInChildren<InteractableBox>();
                OpenCloseBoxText.gameObject.SetActive(true);
                OpenCloseBoxText.text = "�������";
            } else if (hit.collider.GetComponent<InteractableBasket>() && !isBusy)
            {
                Debug.Log("��������� � ������");
            }
        }
        else
        {
            if (isBusy)
            {                
                OpenBox();
            }
        }
    }

    private void OpenBox()
    {
        InteractableBox box = GetComponentInChildren<InteractableBox>();
        box.IsOpen();
    }
    private void FixedUpdate()
    {
        // �������� ���������� �� �����
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        // ������� ��� ���� ��� �������� ���������� �� �����
        Ray ray = new Ray(transform.position, Vector3.down);
        float raycastDistance = 1.1f; // ���������� �� ����� (������� �� ������� �������)

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        // ������������ ���� ��� �������
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red);
    }
}
