using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : MonoBehaviour
{
    private List<Button> _menuBtnList;
    private List<GameObject> _panelList;

    private Transform _contentTrm;
    private void Awake()
    {
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
    }

    private void CloseAllMenu()
    {
        foreach(GameObject panel in  _panelList)
        {
            panel.SetActive(false);
        }
    }
}
