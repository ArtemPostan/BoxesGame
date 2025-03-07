using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Box : MonoBehaviour
{
    public Color BoxColor { get; private set; } // Цвет ящика
   
    public ShapeType InnerShapeType { get; private set; } // Тип фигуры внутри

    private Renderer boxRenderer;

    public Animator animator;

    public List<GameObject> shapes;
    public Renderer litRenderer;
    [SerializeField] public Transform shapePlace;

    public bool IsOpen;

    public int MaxShapes = 4;

    void Awake()
    {
        boxRenderer = GetComponent<Renderer>();
        IsOpen = false;
        animator = GetComponent<Animator>();
    }

    // Инициализация ящика
    public void Initialize(Color color)
    {
        BoxColor = color;
        

        // Определяем тип фигуры внутри на основе цвета
        if (BoxConfig.ColorToShapeMap.TryGetValue(color, out ShapeType shapeType))
        {
            InnerShapeType = shapeType;
        }
       

        // Устанавливаем цвет ящика
        boxRenderer.material.color = color;
        litRenderer.material.color = color;

        SpawnInnerShape();        
        
    }

    public void SpawnInnerShape()
    {
        // Получаем префаб фигуры на основе InnerShapeType
        if (BoxConfig.ShapeTypeToPrefabMap.TryGetValue(InnerShapeType, out GameObject shapePrefab))
        {
            for (int i = 0; i < MaxShapes; i++)
            {
                // Создаем фигуру и добавляем её в список
                GameObject newShape = Instantiate(shapePrefab, GetShapePosition(i), shapePlace.rotation, shapePlace);
                //newShape.SetActive(false); // Скрываем фигуру, пока ящик не открыт
                shapes.Add(newShape); // Добавляем фигуру в список
            }
        }
        else
        {
            Debug.LogWarning($"Префаб для фигуры типа {InnerShapeType} не найден!");
        }
    }    

    private Vector3 GetShapePosition(int index)
    {
        // Пример: размещаем фигуры в виде квадрата
        float offset = 0.2f; // Расстояние между фигурами
        Vector3 basePosition = shapePlace.position;

        switch (index)
        {
            case 0:
                return basePosition + new Vector3(-offset, 0, offset); // Верхний левый угол
            case 1:
                return basePosition + new Vector3(offset, 0, offset); // Верхний правый угол
            case 2:
                return basePosition + new Vector3(-offset, 0, -offset); // Нижний левый угол
            case 3:
                return basePosition + new Vector3(offset, 0, -offset); // Нижний правый угол
            default:
                return basePosition; // По умолчанию
        }
    }

}