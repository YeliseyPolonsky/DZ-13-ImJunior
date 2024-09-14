using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Cube : MonoBehaviour
{
    private int _lifetime;
    private bool _isCollision;
    private Action<Cube> _killAction;

    public void Initialize(int lifetime, Action<Cube> killAction)
    {
        _lifetime = lifetime;
        _killAction = killAction;
        _isCollision = false;
    }

    private IEnumerator TimerToDeath()
    {
        yield return new WaitForSeconds(_lifetime);

        _killAction?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollision == false)
        {
            if (collision.collider.GetComponent<TriggerPlatform>())
            {
                _isCollision = true;
                SetRandomColor();
                StartCoroutine(TimerToDeath());
            }
        }
    }

    private void SetRandomColor() =>
        GetComponent<Renderer>().material.color = Random.ColorHSV();
}