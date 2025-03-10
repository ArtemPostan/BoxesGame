using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextMeshProUGUI SphereText; // ��������� ������� ��� �����������

    [SerializeField] GameObject InteractableTip;
    [SerializeField] GameObject OpenCloseBoxBttn;
    [SerializeField] TextMeshProUGUI boxText;

    [SerializeField] Animator UIAnimator;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
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

    public void ShowInteractTip()
    {
        InteractableTip.gameObject.SetActive(true);
    }

    public void HideInteractTip()
    {
        InteractableTip.gameObject.SetActive(false);
    }

    public void ShowOpenTip()
    {
        OpenCloseBoxBttn.gameObject.SetActive(true);
    }

    public void TextOpen()
    {
        boxText.text = "�������";
    }

    public void TextClose()
    {
        boxText.text = "�������";
    }    

    public void HideAllTips()
    {
        InteractableTip.gameObject.SetActive(false);
        OpenCloseBoxBttn.gameObject.SetActive(false);
    }    
}
