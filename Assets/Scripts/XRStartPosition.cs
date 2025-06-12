using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRStartPosition : MonoBehaviour
{
    // 시작 위치가 계속 엉뚱한 부분으로 생성되는 부분 방지하기 위한 시작 위치 고정
    [SerializeField] Transform startPoint;
    private void Start()
    {
        transform.position = startPoint.position;
        transform.rotation = startPoint.rotation;
    }
}
