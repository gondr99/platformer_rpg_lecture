using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Level : MonoBehaviour
{
    public List<CinemachineVirtualCamera> levelCameras;

    
    public void LoadLevel()
    {
        //만약 포지션을 변경해야한다면 여기서 써줘야 한다.
        gameObject.SetActive(true);
        CameraManager.Instance.SetCameras(levelCameras); //레벨로 카메라를 옮겨온다.
    }
}
