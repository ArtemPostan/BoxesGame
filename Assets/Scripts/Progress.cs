using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Progress : MonoBehaviour
{
    // Количество сданных фигур для каждого типа
    public int spheresCollected = 0;
    public int cubesCollected = 0;
    public int cylindersCollected = 0;

    public static event Action OnProgressUpdated;
    // Синглтон для удобного доступа к Progress из других скриптов
    public static Progress Instance { get; private set; }

    private void Awake()
    {
        // Инициализация синглтона
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Опционально: сохранять объект между сценами
            OnProgressUpdated?.Invoke();
        }
        else
        {
            Destroy(gameObject); // Убедиться, что существует только один экземпляр
        }
    }

    // Метод для увеличения количества сданных фигур
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
        Debug.Log($"С-{spheresCollected} К-{cubesCollected} Ц-{cylindersCollected}");
    }
}
