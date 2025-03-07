using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxConfig
{
    // Словарь, который связывает цвет ящика с типом фигуры внутри
    public static readonly Dictionary<Color, ShapeType> ColorToShapeMap = new()
    {
        { Color.yellow, ShapeType.Cylinder },
        { Color.blue, ShapeType.Cube },
        { Color.green, ShapeType.Sphere }
    };

    // Словарь для связи типа фигуры с префабом
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