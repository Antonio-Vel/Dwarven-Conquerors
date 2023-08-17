using UnityEngine;

[CreateAssetMenu(fileName = "UnitScriptableObject", menuName = "ScriptableObject/UnitScriptableObject")]
public class UnitScriptableObject : ScriptableObject
{
        public string name;
        public int health;
        public float movespeed;
        public float attackSpeed;
        public float attackRange;
        public string description;
        public GameObject prefab;
}