using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Box : MonoBehaviour
{
    public Color BoxColor { get; private set; } // ���� �����
   
    public ShapeType InnerShapeType { get; private set; } // ��� ������ ������

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

    // ������������� �����
    public void Initialize(Color color)
    {
        BoxColor = color;
        

        // ���������� ��� ������ ������ �� ������ �����
        if (BoxConfig.ColorToShapeMap.TryGetValue(color, out ShapeType shapeType))
        {
            InnerShapeType = shapeType;
        }
       

        // ������������� ���� �����
        boxRenderer.material.color = color;
        litRenderer.material.color = color;

        SpawnInnerShape();        
        
    }

    public void SpawnInnerShape()
    {
        // �������� ������ ������ �� ������ InnerShapeType
        if (BoxConfig.ShapeTypeToPrefabMap.TryGetValue(InnerShapeType, out GameObject shapePrefab))
        {
            for (int i = 0; i < MaxShapes; i++)
            {
                // ������� ������ � ��������� � � ������
                GameObject newShape = Instantiate(shapePrefab, GetShapePosition(i), shapePlace.rotation, shapePlace);
                //newShape.SetActive(false); // �������� ������, ���� ���� �� ������
                shapes.Add(newShape); // ��������� ������ � ������
            }
        }
        else
        {
            Debug.LogWarning($"������ ��� ������ ���� {InnerShapeType} �� ������!");
        }
    }    

    private Vector3 GetShapePosition(int index)
    {
        // ������: ��������� ������ � ���� ��������
        float offset = 0.2f; // ���������� ����� ��������
        Vector3 basePosition = shapePlace.position;

        switch (index)
        {
            case 0:
                return basePosition + new Vector3(-offset, 0, offset); // ������� ����� ����
            case 1:
                return basePosition + new Vector3(offset, 0, offset); // ������� ������ ����
            case 2:
                return basePosition + new Vector3(-offset, 0, -offset); // ������ ����� ����
            case 3:
                return basePosition + new Vector3(offset, 0, -offset); // ������ ������ ����
            default:
                return basePosition; // �� ���������
        }
    }

}