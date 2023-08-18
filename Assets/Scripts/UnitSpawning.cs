using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
	static Dictionary<string, UnitScriptableObject> unitDictionary = new Dictionary<string, UnitScriptableObject>();
	private void Start()
	{
		UnitScriptableObject[] UnitArray = Resources.LoadAll<UnitScriptableObject>("Scriptable Obect");
		foreach (UnitScriptableObject obj in UnitArray)
		{
			unitDictionary.Add(obj.name, obj);
		}
	}

	static void Spawn(string name,Vector3 pos)
	{
		if (!unitDictionary.TryGetValue(name, out UnitScriptableObject query))
		{ print($"NAME \"{name}\" NOT FOUND"); return; }

		GameObject obj = Instantiate(query.prefab, pos, Quaternion.identity);
			
	}


	int health;
	float movespeed;
	float attackSpeed;
	float attackRange;
	string description;
	GameObject unitObject;

	Unit(UnitScriptableObject template, GameObject linkedObj)
	{
		health = template.health;
		movespeed = template.movespeed;
		attackSpeed = template.attackSpeed;
		attackRange = template.attackRange;
		description = template.description;
		unitObject = linkedObj;
	}
}
