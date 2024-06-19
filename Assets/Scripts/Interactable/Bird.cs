using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [Space] 
    [SerializeField] private float _speed = 10f;
    [SerializeField] private Vector3 _direction = new Vector3(0, 2, -1);
    [SerializeField] private float _destroyDelay = 3f;

    [Space] 
    [SerializeField] private float _sfxVolume = 0.25f;
    
    public Vector3 direction => _direction.normalized;

    private bool _isFlying = false;

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
        
        AudioManager.instance.PlayGlobalAudio("[04] Birds", _sfxVolume);
        
        DestroyAfterSeconds(_destroyDelay);
    }

    public IEnumerator DestroyAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
    
}
