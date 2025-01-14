using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
    [SerializeField]
    Collider thisCollider;

    [SerializeField]
    Collider[] colliderToIgnore; //�]���O�h�Ө�LChild�n�Q�����A�ҥH�o�����L�אּ�}�C 

    // Start is called before the first frame update
    void Start()
    {
        foreach (Collider otherCollider in colliderToIgnore)
        {
            Physics.IgnoreCollision(thisCollider, otherCollider, true);
        }
    }
}