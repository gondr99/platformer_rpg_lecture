using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : MonoSingleton<MenuWindow>
{
    public DragItem dragItem;
    
    [SerializeField] private InputReader _inputReader;
    private List<Button> _menuBtnList;
    private List<GameObject> _panelList;
    
    private Transform _contentTrm;
    private bool _isMenuOpen = false;
    private bool _isAnimating = false;
    private CanvasGroup _canvasGroup;

    public OptionUI OptionUI { get; private set; }
    
    private void Awake()
    {
        OptionUI = transform.Find("Content/OptionPanel").GetComponent<OptionUI>();
        _menuBtnList = transform.Find("Header").GetComponentsInChildren<Button>().ToList();
        _contentTrm = transform.Find("Content");
        _panelList = new List<GameObject>();

        foreach(Transform child in _contentTrm)
        {
            _panelList.Add(child.gameObject);
        }

        for(int i = 0;  i < _menuBtnList.Count; ++i)
        {
            int childIdx = i;
            _menuBtnList[i].onClick.AddListener(() =>
            {
                CloseAllMenu();
                _panelList[childIdx].gameObject.SetActive(true);
            });
        }
        
        _canvasGroup = GetComponent<CanvasGroup>();
        _inputReader.OpenMenuEvent += HandleOpenMenuEvent;
    }

    private void Start()
    {
        CloseMenuWindow(); //시작하면 닫아.
        dragItem.EndDrag();//드래그아이템은 투명처리
    }

    private void OnDestroy()
    {
        _inputReader.OpenMenuEvent -= HandleOpenMenuEvent;
    }

    private void HandleOpenMenuEvent()
    {
        if (_isAnimating) return; //애니메이션중일때는 리턴.

        if (_isMenuOpen)
        {
            CloseMenuWindow();
        }
        else
        {
            OpenMenuWindow();
        }
    }

    private void OpenMenuWindow()
    {
        _isMenuOpen = true;
        _isAnimating = true;
        Time.timeScale = 0;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.one * 1.2f, 0.3f));
        seq.Join(_canvasGroup.DOFade(1f, 0.3f));
        seq.Append(transform.DOScale(Vector3.one, 0.1f));
        seq.OnComplete(() => _isAnimating = false).SetUpdate(true);
    }

    private void CloseMenuWindow()
    {
        _isMenuOpen = false;
        _isAnimating = true;
        Time.timeScale = 1;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.one * 1.2f, 0.1f));
        seq.Append(transform.DOScale(Vector3.zero, 0.3f));
        seq.Join(_canvasGroup.DOFade(0f, 0.3f));
        seq.OnComplete(() => _isAnimating = false);
    }


    private void CloseAllMenu()
    {
        foreach(GameObject panel in _panelList)
        {
            panel.SetActive(false);
        }
    }
}
