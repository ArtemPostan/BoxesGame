using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI SphereText; // ��������� ������� ��� �����������

    private void OnEnable()
    {
        // ������������� �� ������� ��� ��������� �������
        Progress.OnProgressUpdated += UpdateShapeText;
    }

    private void OnDisable()
    {
        // ������������ �� ������� ��� ���������� �������
        Progress.OnProgressUpdated -= UpdateShapeText;
    }

    // ����� ��� ���������� ������
    void UpdateShapeText()
    {
        // �������� ������ �� Progress
        int spheres = Progress.Instance.spheresCollected;
        int cubes = Progress.Instance.cubesCollected;
        int cylinders = Progress.Instance.cylindersCollected;

        // ��������� �����
        SphereText.text = $"�-{cubes}    ; �-{spheres}    ; �-{cylinders}";
    }
}
