using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRotate : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
    }
}
