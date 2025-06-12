using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRStartPosition : MonoBehaviour
{
    // ���� ��ġ�� ��� ������ �κ����� �����Ǵ� �κ� �����ϱ� ���� ���� ��ġ ����
    [SerializeField] Transform startPoint;
    private void Start()
    {
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
    }
}
