using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerWithOutNN : MonoBehaviour
{
    public GameObject FoodPref;
    public GameObject CellPref;
    public int startNumberOfCells = 20;
    public Vector2 Field = new Vector2(-9.4f, 6.2f);
    public int[] sizes = new int[3] { 10, 10, 2 };
    void Start()
    {
        for (int i = 0; i < 300; i++)
        {
            GameObject food = Instantiate(FoodPref, new Vector3(Random.Range(-Field.x, Field.x), Random.Range(-Field.y, Field.y)), Quaternion.identity);
            food.name = "food";
        }
        for (int i = 0; i < startNumberOfCells; i++)
        {
            GameObject cell = Instantiate(CellPref, new Vector3(Random.Range(-Field.x, Field.x), Random.Range(-Field.y, Field.y)), Quaternion.identity);
            cell.name = "cell";
            Cell cl = cell.GetComponent<Cell>();
            cl.foodSkill = Random.Range(0, 5);
            cl.attackSkill = Random.Range(0, 5);
            cl.defenceSkill = Random.Range(0, 5);
        }
    }

    void FixedUpdate()
    {
        GameObject food = Instantiate(FoodPref, new Vector3(Random.Range(-Field.x, Field.x), Random.Range(-Field.y, Field.y)), Quaternion.identity);
        food.name = "food";
    }
}
