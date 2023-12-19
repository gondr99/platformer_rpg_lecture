using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[Serializable]
public struct KeySprite
{
    public Key key;
    public Sprite sprite;
}

[CreateAssetMenu(menuName = "SO/Key/KeySpriteSO")]
public class HoyKeyIconSO : ScriptableObject
{
    public List<KeySprite> keySprites = new List<KeySprite>();

    private Dictionary<Key, Sprite> _keySpriteDictionary;
    
    private void OnEnable()
    {
        _keySpriteDictionary = new Dictionary<Key, Sprite>();
        foreach (KeySprite ks in keySprites)
        {
            _keySpriteDictionary.Add(ks.key, ks.sprite);
        }
    }

    public Sprite GetSpriteByKey(Key key)
    {
        return _keySpriteDictionary[key];
    }

    public List<Key> GetAllKeyFromList()
    {
        return keySprites.Select(x => x.key).ToList();
    }
    
    public Key GetRandomKeyFromList()
    {
        if (keySprites.Count == 0)
        {
            return Key.None;
        }
        return keySprites[Random.Range(0, keySprites.Count)].key;
    }
}