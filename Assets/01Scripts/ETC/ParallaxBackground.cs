using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxRatio;
    [SerializeField] private bool _yAxisRepeat = false;
    private Transform _mainCamTrm;
    private Vector2 _initPosition;
    private Vector2 _spriteSize;

    private Vector2 _camInitPosition;

    void Awake()
    {
        _mainCamTrm = Camera.main.transform;
        _camInitPosition = _mainCamTrm.position;
        _initPosition = transform.position; //시작 위치
        _spriteSize = GetComponent<SpriteRenderer>().bounds.size;
    }

    private void Start()
    {
        
    }

    void LateUpdate()
    {
        while (MoveToCameraPosition())
        {
            
        }
    }

    private bool MoveToCameraPosition()
    {
        Vector2 cameraDelta = (Vector2)_mainCamTrm.position - _camInitPosition;

        
        Vector2 moveOffset = new Vector2(cameraDelta.x * parallaxRatio.x, cameraDelta.y * parallaxRatio.y);
        transform.position = _initPosition + moveOffset;

        Vector2 deltaFromCam =  (Vector2)_mainCamTrm.position - _initPosition;

        bool moved = false;
        //X축에 대한 무한 스크롤 보정
        if (deltaFromCam.x > _spriteSize.x) //오른쪽이동.
        {
            _initPosition.x += _spriteSize.x;
            moved = true;
        }
        else if (deltaFromCam.x < -  _spriteSize.x)  //왼쪽이동
        {
            _initPosition.x -= _spriteSize.x;
            moved = true;
        }

        if (_yAxisRepeat)
        {
            if (deltaFromCam.y >  _spriteSize.y) //위쪽
            {
                _initPosition.y += _spriteSize.y;
                moved = true;
            }
            else if (deltaFromCam.y <  - _spriteSize.y)  //아래쪽
            {
                _initPosition.y -= _spriteSize.y;
                moved = true;
            }
        }

        return moved;
    }
}
