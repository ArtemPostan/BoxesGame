using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;


public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float jumpDelay = 0.0f;

    [Header("Look Settings")]
    public float mouseSensitivity = 1.0f;
    public Transform cameraTransform;
    public Transform boxPlace;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isInteracting;
    private bool isGrounded;
    public bool isBusy = false;
    private bool isDesktop = true;

    private PlayerInput playerInput;
    private Animator animator;
    public InteractableBox box;
    [SerializeField] Joystick joystick;
    [SerializeField] Joystick cameraJoystick;


    //[SerializeField] Image InteractableTip;
    //[SerializeField] public GameObject OpenCloseBoxBttn;
    //public TextMeshProUGUI boxText;

    public LayerMask interractLayerMask;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
        //boxText = OpenCloseBoxBttn.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        if (!isDesktop)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical);
            Vector2 cameraInput = new Vector2(cameraJoystick.Horizontal, cameraJoystick.Vertical);
            RotateCamera(cameraInput);
        }
        else
        {

            RotateCamera(lookInput);
        }
        HandleMovement(moveInput);
        DetectInteractableObjects();
    }

    private void HandleMovement(Vector2 input)
    {
        // Нормализуем вектор, чтобы избежать ускорения при движении по диагонали
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        move.Normalize();

        // Применяем движение
        rb.velocity = new Vector3(move.x * movementSpeed, rb.velocity.y, move.z * movementSpeed);

        // Обновляем анимацию
        bool isMoving = input.magnitude > 0;
        animator.SetBool("IsWalking", isMoving);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (isDesktop)
        {
            // Получаем ввод с клавиатуры/геймпада
            moveInput = context.ReadValue<Vector2>();
            HandleMovement(moveInput);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();

    }

    private void RotateCamera(Vector2 input)
    {
        // Вращение камеры по горизонтали (вокруг оси Y)
        float rotationY = input.x * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * rotationY * 10);

        // Вращение камеры по вертикали (вокруг оси X)
        float rotationX = -input.y * mouseSensitivity * Time.deltaTime;
        float newRotationX = cameraTransform.localEulerAngles.x + rotationX * 10;
        if (newRotationX > 180) newRotationX -= 360;
        newRotationX = Mathf.Clamp(newRotationX, -90f, 90f);
        cameraTransform.localEulerAngles = new Vector3(newRotationX, 0f, 0f);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            PerformJump();
        }
    }

    public void OnJumpButtonClicked()
    {
        if (isGrounded)
        {
            PerformJump();
        }
    }

    private void PerformJump()
    {
        animator.SetTrigger("IsJumping");
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractWithObject();
        }
    }

    public void OnClickInteract()
    {
        InteractWithObject();
    }

    private void DetectInteractableObjects()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 3f, interractLayerMask);

        if (isHit)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();           
            interactable.IfDetected(this);            
        } else
        {
            UIManager.Instance.HideInteractTip();
        }
    }  
    private void InteractWithObject()
    {

        RaycastHit hit;
        bool isHit = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, 3f, interractLayerMask);

        
            if (isHit)
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                if (interactable != null)
                {
                    interactable.Interact(this);
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
        // Проверка нахождения на земле
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        // Пускаем луч вниз для проверки нахождения на земле
        Ray ray = new Ray(transform.position, Vector3.down);
        float raycastDistance = 1.1f; // Расстояние до земли (зависит от размера объекта)

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        // Визуализация луча для отладки
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red);
    }
}
