using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionActions : MonoBehaviour
{
	public Dictionary<string, GameObject> selected;
	Vector3 mousepos;
	Vector3 origin;
	bool LoR = false;
	const float MOVE = .5F;
	List<float> distances = new();
	Dictionary<float, Movable> movables = new();
	// Update is called once per frame
	void Update()
	{
		selected = Selection.getSelected();

		if (selected.Count == 0)
		{ return; }

		if (Input.GetKey("p") || Input.GetKeyUp("p"))
		{
			Move(selected);
		}

	}
	//Code Museum

	//mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	//foreach (KeyValuePair<string, GameObject> pair in selections)
	//{
	//    if (pair.Value.TryGetComponent<Movable>(out Movable component))
	//    {
	//        print($"{pair.Key} is moving to {Camera.main.ScreenToWorldPoint(Input.mousePosition)}!");
	//        if (Input.GetKey("w"))
	//        {
	//            component.AddToMoveQueue(mousepos);
	//        }
	//        else
	//        {
	//            component.MoveTo(mousepos);
	//        }
	//    }
	//    int rand = Random.Range(0, 360);
	//    mousepos = new Vector2((float)(mousepos.x + .5 * Mathf.Cos(rand)), (float)(mousepos.y + .5 * Mathf.Sin(rand)));
	//}

	void Move(Dictionary<string, GameObject> selections)
	{


		if (Input.GetKeyDown("p"))
		{
			origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			foreach (KeyValuePair<string, GameObject> pair in selections)
			{
				if (pair.Value.TryGetComponent<Movable>(out Movable comp))
				{
					movables.Add(Vector2.Distance((Vector2)origin, pair.Value.GetComponent<Rigidbody2D>().position), comp);
					distances.Add(Vector2.Distance((Vector2)origin, pair.Value.GetComponent<Rigidbody2D>().position));
				}
			}
			distances.Sort();
		}
		mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		//Sort by movable & distance from mousepos
		//Find position of mouse relative to original position in order to find rotation of formation
		Vector3 delta;
		float rotation;
		//print(Vector3.Distance(mousepos, origin));
		if (Vector3.Distance(mousepos, origin) != 0)
		{
			delta = mousepos - origin;
		}
		else
		{
			movables.TryGetValue(distances[0], out Movable m);
			delta = mousepos - m.GetComponent<Transform>().position;
		}
		rotation = Mathf.Asin(delta.y / delta.magnitude) + Mathf.PI / 2;
		//Create a formation of positions with rotation in mind
		Vector2[] formation = new Vector2[movables.Count];
		int rows = (int)(Vector3.Distance(origin, mousepos) / MOVE);
		print(rows);
		Vector2 movefactor = new Vector2(MOVE * Mathf.Cos(rotation), MOVE * Mathf.Sin(rotation));
		Vector2 moveVectorCon = new Vector2(MOVE * Mathf.Cos(rotation), MOVE * Mathf.Sin(rotation));
		delta.Normalize();
		if (formation.Length % 2 == 0)
		{
			Vector2 smallconstant = movefactor / 2;
			formation[0] = (Vector2)origin + movefactor / 2;
			formation[1] = (Vector2)origin + movefactor / -2;
			for (int r = 0; r < rows; r++)
			{
				for (int i = 2; i < formation.Length; i++)
				{
					formation[i] = (Vector2)origin + movefactor + smallconstant;
					if (LoR)
					{
						movefactor += moveVectorCon;
					}
					LoR = !LoR;
					movefactor *= -1;
					moveVectorCon *= -1;
					smallconstant *= -1;
				}
				movefactor = moveVectorCon + (Vector2)(delta * MOVE * r);
			}
		}
		else
		{
			for (int r = 0; r < rows; r++)
			{
				formation[0] = (Vector2)origin;
				for (int i = 1; i < formation.Length; i++)
				{
					formation[i] = (Vector2)origin + movefactor;
					if (LoR)
					{
						movefactor += moveVectorCon;
					}
					LoR = !LoR;
					moveVectorCon *= -1;
					movefactor *= -1;
				}
				movefactor = moveVectorCon + (Vector2)(delta * MOVE * r);
			}
		}
		//Send out formation details on mouseup
		if (Input.GetKeyUp("p"))
		{
			for (int i = 0; i < distances.Count; i++)
			{
				movables.TryGetValue(distances[i], out Movable m);
				if (Input.GetKey("w"))
				{
					m.AddToMoveQueue(formation[i]);
				}
				else
				{
					m.MoveTo(formation[i]);
				}

				//print($"Ball {i} to {formation[i]}");
			}

			movables.Clear();
			distances.Clear();
		}
	}
}



