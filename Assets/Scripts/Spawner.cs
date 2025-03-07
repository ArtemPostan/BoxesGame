using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class Spawner : MonoBehaviour
{
    public Vector3 spawnPosition = Vector3.zero; // ������� ������
    public void SpawnRandomBox()
    {
        // �������� ��������� ���� �� �������

        Color randomColor = GetRandomColor();

        // ������� ���� ����� �������
        GameObject box = BoxFactory.CreateBox(randomColor, spawnPosition, Quaternion.identity);

        if (box != null)
        {
            Debug.Log($"Spawned box with color {randomColor} and inner shape {box.GetComponent<Box>().InnerShapeType} at {spawnPosition}");
        }
    }

    public static Color GetRandomColor()
    {
        float randomValue = Random.value; // ��������� ����� �� 0 �� 1

        if (randomValue < 0.6f) // 60%
        {
            return Color.blue;
        }
        else if (randomValue < 0.9f) // 30% (0.6 + 0.3)
        {
            return Color.green;
        }
        else // 10% (�������)
        {
            return Color.yellow;
        }
    }

}