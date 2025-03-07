using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxConfig
{
    // �������, ������� ��������� ���� ����� � ����� ������ ������
    public static readonly Dictionary<Color, ShapeType> ColorToShapeMap = new()
    {
        { Color.yellow, ShapeType.Cylinder },
        { Color.blue, ShapeType.Cube },
        { Color.green, ShapeType.Sphere }
    };

    // ������� ��� ����� ���� ������ � ��������
    public static readonly Dictionary<ShapeType, GameObject> ShapeTypeToPrefabMap = new()
    {
        { ShapeType.Cube, Resources.Load<GameObject>("Prefabs/Cube") },
        { ShapeType.Sphere, Resources.Load<GameObject>("Prefabs/Sphere") },
        { ShapeType.Cylinder, Resources.Load<GameObject>("Prefabs/Cylinder") }
    };
}

public enum ShapeType
{
    Cube,
    Sphere,
    Cylinder
}