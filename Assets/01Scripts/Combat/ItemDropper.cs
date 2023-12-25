using System;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [SerializeField] private DropTableSO _dropTable;
    [SerializeField] private int _dropDiceCount = 2; //드롭 주사위 굴릴 횟수
    [SerializeField] private ItemObject _dropPrefab;
    [SerializeField] private Coin _coinPrefab;

    [SerializeField] private Entity _owner;

    private void Start()
    {
        _owner.HealthCompo.OnDead += DropItem;
    }

    private void OnDestroy()
    {
        _owner.HealthCompo.OnDead -= DropItem;
    }

    public void DropItem(Vector2 dropVelocity)
    {
        int dropGoldAmount = _dropTable.GetDropGold(); //드랍할 골드를 구하고 이것도 드랍.
        Coin dropCoin = Instantiate(_coinPrefab, transform.position, Quaternion.identity);
        dropCoin.InitCoin(dropVelocity * 0.8f, dropGoldAmount); //코인의 값 측정해주고

        //경험치 추가.
        PlayerManager.Instance.AddExp(_dropTable.dropEXP);
        
        for (int i = 0; i < _dropDiceCount; ++i)
        {
            //드롭찬스 안에서 걸리면
            if (_dropTable.GetDropItem(out ItemDataSO item))
            {
                ItemObject newDrop = Instantiate(_dropPrefab, transform.position, Quaternion.identity);
                newDrop.SetupItem(item, dropVelocity);
            }
        }
    }
}
