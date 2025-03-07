using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Progress : MonoBehaviour
{
    // ���������� ������� ����� ��� ������� ����
    public int spheresCollected = 0;
    public int cubesCollected = 0;
    public int cylindersCollected = 0;

    public static event Action OnProgressUpdated;
    // �������� ��� �������� ������� � Progress �� ������ ��������
    public static Progress Instance { get; private set; }

    private void Awake()
    {
        // ������������� ���������
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �����������: ��������� ������ ����� �������
            OnProgressUpdated?.Invoke();
        }
        else
        {
            Destroy(gameObject); // ���������, ��� ���������� ������ ���� ���������
        }
    }

    // ����� ��� ���������� ���������� ������� �����
    public void CollectShape(ShapeType shapeType)
    {
        switch (shapeType)
        {
            case ShapeType.Sphere:
                spheresCollected++;
                break;
            case ShapeType.Cube:
                cubesCollected++;
                break;
            case ShapeType.Cylinder:
                cylindersCollected++;
                break;
        }

        OnProgressUpdated?.Invoke();
        Debug.Log($"�-{spheresCollected} �-{cubesCollected} �-{cylindersCollected}");
    }
}
