using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI SphereText; // Текстовый элемент для отображения

    [SerializeField] GameObject InteractableTip;
    [SerializeField] GameObject OpenCloseBoxBttn;
    [SerializeField] TextMeshProUGUI boxText;

    [SerializeField] Animator UIAnimator;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        // Подписываемся на событие при включении объекта
        Progress.OnProgressUpdated += UpdateShapeText;
    }

    private void OnDisable()
    {
        // Отписываемся от события при выключении объекта
        Progress.OnProgressUpdated -= UpdateShapeText;
    }

    // Метод для обновления текста
    void UpdateShapeText()
    {
        // Получаем данные из Progress
        int spheres = Progress.Instance.spheresCollected;
        int cubes = Progress.Instance.cubesCollected;
        int cylinders = Progress.Instance.cylindersCollected;

        // Обновляем текст
        SphereText.text = $"К-{cubes}    ; С-{spheres}    ; Ц-{cylinders}";
    }

    public void ShowInteractTip()
    {
        InteractableTip.gameObject.SetActive(true);
    }

    public void HideInteractTip()
    {
        InteractableTip.gameObject.SetActive(false);
    }

    public void ShowOpenTip()
    {
        OpenCloseBoxBttn.gameObject.SetActive(true);
    }

    public void TextOpen()
    {
        boxText.text = "Открыть";
    }

    public void TextClose()
    {
        boxText.text = "Закрыть";
    }    

    public void HideAllTips()
    {
        InteractableTip.gameObject.SetActive(false);
        OpenCloseBoxBttn.gameObject.SetActive(false);
    }    
}
