using UnityEngine;

public enum CollectibleType
{
    Health,
    money,
}

public class CollectibleItem : MonoBehaviour
{
    public CollectibleType type;
    public int amount;
}
