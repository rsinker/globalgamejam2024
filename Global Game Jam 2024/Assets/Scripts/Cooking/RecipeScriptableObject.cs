using UnityEngine;

[CreateAssetMenu(menuName = ("Recipe"))]
public class Recipe : ScriptableObject
{
    public Ingredient[] m_ingredientList;
    public Enemy[] m_Stage1Enemys;
    public Enemy[] m_Stage2Enemys;
    public Enemy[] m_Stage3Enemys;
    public Sprite _image;
    public GameObject recipePrefab;
}
