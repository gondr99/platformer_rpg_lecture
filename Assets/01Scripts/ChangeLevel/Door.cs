using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Level _from;
    [SerializeField] private Level _targetLevel;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Transform _targetPosition;
    
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            StartCoroutine(ChangeCoroutine(player));

        }
    }

    private IEnumerator ChangeCoroutine(Player player)
    {
        _inputReader.SetPlayerInputEnable(false);
        ScreenManager.Instance.FadeOut(0.7f);
        yield return new WaitForSeconds(0.7f);
        _targetLevel.LoadLevel();
        player.transform.position = _targetPosition.position;
        
        yield return new WaitForSeconds(1f);
        ScreenManager.Instance.FadeIn(0.7f);
        _inputReader.SetPlayerInputEnable(true);
        
        //가장 마지막에 해야한다.
        _from.gameObject.SetActive(false);
    }
    
}
