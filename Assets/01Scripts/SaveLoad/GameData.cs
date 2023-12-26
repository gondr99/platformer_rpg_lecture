using System.Collections.Generic;

public class GameData
{
    public int gold;
    public int level;
    public int exp;
    public int requiredExp;
    public int skillPoint;

    public SerializableDictionary<string, int> skillTree;
    public SerializableDictionary<string, InventoryItem> inventory;
    public List<string> equipmentsIDList;

    public GameData()
    {
        gold = 0;
        exp = 0;
        level = 1;
        requiredExp = 100;
        skillPoint = 0;

        skillTree = new SerializableDictionary<string, int>();
        inventory = new SerializableDictionary<string, InventoryItem>();
        equipmentsIDList = new List<string>();
    }
}
