using UnityEngine;

public class Coin : MonoBehaviour, IPickable
{
    private Rigidbody2D _rigidbody;
    private int _value;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    public void InitCoin(Vector2 launchForce, int value)
    {
        _value = value;
        _rigidbody.AddForce(launchForce, ForceMode2D.Impulse);
    }
    
    public void PickUp()
    {
        PlayerManager.Instance.Gold += _value;
        Destroy(gameObject);
    }
}
