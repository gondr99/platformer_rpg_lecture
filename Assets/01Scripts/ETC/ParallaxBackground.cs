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
    

    void Awake()
    {
        _mainCamTrm = Camera.main.transform;
        _initPosition = transform.position; //시작 위치

        _spriteSize = GetComponent<SpriteRenderer>().bounds.size;
    }
    
    void LateUpdate()
    {
        float distanceToMovedX = _mainCamTrm.position.x * (1 - parallaxRatio.x);
        float distanceToMovedY = _mainCamTrm.position.y * (1 - parallaxRatio.y);
        
        Vector2 moveOffset = new Vector2(_mainCamTrm.position.x * parallaxRatio.x, _mainCamTrm.position.y * parallaxRatio.y);
        transform.position = _initPosition + moveOffset;
        
        //X축에 대한 무한 스크롤 보정
        if (distanceToMovedX > _initPosition.x + _spriteSize.x) //오른쪽이동.
        {
            _initPosition.x += _spriteSize.x;
        }
        else if( distanceToMovedX < _initPosition.x - _spriteSize.x)  //왼쪽이동
        {
            _initPosition.x -= _spriteSize.x;
        }

        if (_yAxisRepeat)
        {
            if (distanceToMovedY > _initPosition.y + _spriteSize.y) //위쪽
            {
                _initPosition.y += _spriteSize.y;
            }
            else if( distanceToMovedY < _initPosition.y - _spriteSize.y)  //아래쪽
            {
                _initPosition.y -= _spriteSize.y;
            }
        }


    }
}
