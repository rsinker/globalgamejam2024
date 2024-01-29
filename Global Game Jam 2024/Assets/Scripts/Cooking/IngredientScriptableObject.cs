using UnityEngine;

[CreateAssetMenu(menuName = ("Ingredient"))]
public class Ingredient : ScriptableObject
{
    public Sprite _iconUncooked;
    public Sprite _iconCooked;
    public GameObject _foodPrefab;
    public CookingType _cookingType;
}
