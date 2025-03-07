using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFactory : MonoBehaviour
{
    // ����� ��� �������� �����
    public static GameObject CreateBox(Color boxColor, Vector3 position, Quaternion rotation)
    {
        // ��������� ������ �����
        GameObject boxPrefab = Resources.Load<GameObject>("Box");
        if (boxPrefab == null)
        {
            Debug.LogError("Box prefab not found in Resources folder!");
            return null;
        }

        // ������� ���� �� �����
        GameObject box = Object.Instantiate(boxPrefab, position, rotation);

        // �������������� ����
        Box boxComponent = box.GetComponent<Box>();
        if (boxComponent != null)
        {
            boxComponent.Initialize(boxColor);
        }
        else
        {
            Debug.LogError("Box component not found on the box prefab!");
        }

        return box;
    }
}