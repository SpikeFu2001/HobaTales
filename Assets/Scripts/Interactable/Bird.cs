using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bird : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [Space] 
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Vector3 _direction = new Vector3(0, 2, -1);
    [SerializeField] private float _destroyDelay = 3f;

    [SerializeField] private float _sfxVolume = 0.25f;

    private Vector3 direction = new Vector3(0, 2, 0);
    private bool _isFlying = false;

    private void Awake()
    {
        direction.x = Random.Range(-1f, 1f);
        direction.z = Random.Range(-1f, 1f);
    }

    private void Update()
    {
        if (_isFlying)
        {
            transform.localPosition += direction * (_speed * Time.deltaTime);
        }
    }

    public void Fly()
    {
        _animator.CrossFade("Fly", 0.15f);
        _isFlying = true;
        
        transform.LookAt(direction);

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayGlobalAudio("[04] Birds", _sfxVolume);
        }
        
        DestroyAfterSeconds(_destroyDelay);
    }

    public IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
    
}
