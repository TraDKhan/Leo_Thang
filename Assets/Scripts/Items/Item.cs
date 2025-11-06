using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class Item : ScriptableObject
{
    public ItemType itemType;
    public int value = 1;
    public Sprite icon;
}
