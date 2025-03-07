using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI SphereText; // Текстовый элемент для отображения

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
}
