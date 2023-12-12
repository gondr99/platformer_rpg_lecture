using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform _playerTrm;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float _flipYRotationTime = 0.5f;

    private Coroutine _turnCoroutine;

    private Player _player;
    private bool _facingRight = true;

    private void Awake()
    {
        _player = _playerTrm.GetComponent<Player>();
        _player.OnFlip += HandleFlipEvent;
    }

    private void Update()
    {
        transform.position = _playerTrm.position;
    }

    public void HandleFlipEvent(int facingDirection)
    {
        _facingRight = facingDirection == 1;
        
        if(_turnCoroutine != null)
            StopCoroutine( _turnCoroutine );

        _turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0;

        float elapsedTime = 0f;

        while(elapsedTime < _flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / _flipYRotationTime));
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        if (_facingRight) return 0f;
        else return 180f;
    }

    private void OnDestroy()
    {
        _player.OnFlip -= HandleFlipEvent;
    }

}
