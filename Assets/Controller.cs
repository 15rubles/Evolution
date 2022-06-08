using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject FoodPref;
    public GameObject CellPref;
    int startNumberOfCells = 100;
    public Vector2 Field = new Vector2(-9.4f, 6.2f);
    const int inputSize = 8;
    int[] sizes = new int[3] { inputSize + 3, inputSize*2,inputSize / 2 };
    //  const int inputSize = 12;
    // int[] sizes = new int[4] { 12, 8, 6, 6 };
    float FoodTimer, TimeToSpawnFood = 0.05f;
    void Start()
    {
        for (int i = 0; i < 300; i++)
        {
            GameObject food = Instantiate(FoodPref, new Vector3(Random.Range(-Field.x, Field.x), Random.Range(-Field.y, Field.y)), Quaternion.identity);
        }
        for (int i = 0; i < startNumberOfCells; i++)
        {
            GameObject cell = Instantiate(CellPref, new Vector3(Random.Range(-Field.x, Field.x), Random.Range(-Field.y, Field.y)), Quaternion.identity);
            //GameObject cell = Instantiate(CellPref, new Vector3(0,0,0), Quaternion.identity);
            AI ai = cell.GetComponent<AI>();
            ai.nn = new NN(sizes);
            ai.foodSkill = Random.Range(0f, 5f);
            ai.attackSkill = Random.Range(0f, 5f);
            ai.defenceSkill = Random.Range(0f, 5f);
            ai.size = Random.Range(0.75f, 1.5f);
            ai.SetColor();
            ai.inputsCount = inputSize;
        }
    }

    void FixedUpdate()
    {
        FoodTimer += Time.deltaTime;
        if (FoodTimer > TimeToSpawnFood)
        {
            GameObject food = Instantiate(FoodPref, new Vector3(Random.Range(-Field.x, Field.x), Random.Range(-Field.y, Field.y)), Quaternion.identity);
            FoodTimer = 0;
        }

    }
}
