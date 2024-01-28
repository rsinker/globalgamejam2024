using UnityEngine;

[CreateAssetMenu(menuName = ("Recipe"))]
public class Recipe : ScriptableObject
{
    public Ingredient[] m_ingredientList;
    public GameObject[] m_Stage1Enemys;
    public GameObject[] m_Stage2Enemys;
    public GameObject[] m_Stage3Enemys;
    public Sprite _image;
}
