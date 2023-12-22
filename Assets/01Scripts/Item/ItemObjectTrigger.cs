using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private IPickable _itemObject;

    private void Awake()
    {
        _itemObject = GetComponentInParent<IPickable>(); //부모에 있는거 가져오고.
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            _itemObject.PickUp();
        }
    }
}
