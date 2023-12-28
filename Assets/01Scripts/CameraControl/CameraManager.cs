using Cinemachine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera[] _virtualCameras;
    [SerializeField] private CameraFollowObject _followObject;

    [Header("lerp for jump and fall")]
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    public float fallSpeedYDampingChangeThreshold = -15f;

    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Tween _lerpYPanTween;
    private CinemachineFramingTransposer _framingTransposer;
    private CinemachineVirtualCamera _currentCam;
    private CinemachineConfiner2D _confiner2D;

    private float _normalYPanAmount;


    private Tween _panCameraTween;
    private Vector2 _startingTrackedObjectOffset;

    private Dictionary<PanDirection, Vector2> _panDictionary;
    
    public Camera MainCam { get; private set; }
    
    private void Awake()
    {
        MainCam = Camera.main;
        
        ChangeCamera(_virtualCameras[0]);

        _panDictionary = new Dictionary<PanDirection, Vector2>()
        {
            { PanDirection.Up, Vector2.up },
            { PanDirection.Down, Vector2.down },
            { PanDirection.Left, Vector2.left },
            { PanDirection.Right, Vector2.right }
        };
    }

    public void ChangeCamera(CinemachineVirtualCamera activeCam)
    {
        for (int i = 0; i < _virtualCameras.Length; ++i)
        {
            _virtualCameras[i].Priority = 5;
        }

        activeCam.Priority = 10;
        _currentCam = activeCam;
        _framingTransposer = _currentCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        _confiner2D = _currentCam.GetComponent<CinemachineConfiner2D>();
        _normalYPanAmount = _framingTransposer.m_YDamping;

        _startingTrackedObjectOffset = _framingTransposer.m_TrackedObjectOffset;
        _currentCam.Follow = _followObject.transform;
    }

    public void LerpYDamping(bool isPlayerFalling)
    {
        if (_lerpYPanTween != null && _lerpYPanTween.IsActive())
            _lerpYPanTween.Kill();

        float endDampingAmount = _normalYPanAmount;
        //float endDampingAmount = _startingTrackedObjectOffset.y;
        if (isPlayerFalling)
        {
            endDampingAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }

        //y damping control
        IsLerpingYDamping = true;
        _lerpYPanTween = DOTween.To(
                () => _framingTransposer.m_YDamping,
                value => _framingTransposer.m_YDamping = value,
                endDampingAmount,
                _fallYPanTime)
            .OnComplete(() => IsLerpingYDamping = false);

        //IsLerpingYDamping = true;
        //_lerpYPanTween = DOTween.To(
        //    () => _framingTransposer.m_TrackedObjectOffset.y,
        //    value => _framingTransposer.m_TrackedObjectOffset.y = value,
        //    endDampingAmount,
        //    _fallYPanTime).OnComplete(() => IsLerpingYDamping = false);
    }

    
    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector3 endPos = Vector3.zero;

        //���������� �ǵ����Ⱑ �ƴϸ�
        if(!panToStartingPos)
        {
            endPos = _panDictionary[panDirection] * panDistance + _startingTrackedObjectOffset;
        }
        else
        {
            endPos = _startingTrackedObjectOffset;
        }

        if(_panCameraTween != null && _panCameraTween.IsActive())
        {
            _panCameraTween.Kill();
        }

        _panCameraTween = DOTween.To(
            ()=> _framingTransposer.m_TrackedObjectOffset,
            value => _framingTransposer.m_TrackedObjectOffset = value,
            endPos, panTime);
    }
    

    public void ChangeCameraBound(CompositeCollider2D confinerCollider)
    {
        _confiner2D.m_BoundingShape2D = confinerCollider;
    }


    public void SwapCamera(CinemachineVirtualCamera camFromLeft, 
        CinemachineVirtualCamera camFromRight, Vector2 triggerExitDirection)
    {
        if(_currentCam == camFromLeft && triggerExitDirection.x > 0)
        {
            ChangeCamera(camFromRight); //������ ī�޶� Ȱ��ȭ.

        }else if(_currentCam == camFromRight && triggerExitDirection.x < 0)
        {
            ChangeCamera(camFromLeft);
        }
    }
}
