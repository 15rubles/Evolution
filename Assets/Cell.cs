using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public const float mutationChance = 0.05f;
    public int foodSkill = 1;
    public int attackSkill = 0;
    public int defenceSkill = 0;
    float speed = 2f;
    float rotateSpeed = 8f;
    public float energy = 200;
    private Rigidbody2D rb;
    public Vector2 Food;
    const string FoodConst = "food";
    const string CellConst = "cell";
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        energy -= Time.deltaTime;
        //Get inputs
        float vision = 10f;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, vision);
        Vector3 NearestOfFood = new Vector3(1000, 1000, 1000);
        for (int i = 0; i < colliders.Length; i++)
        {
            if(colliders[i].gameObject.tag == FoodConst)
            {
                if (Vector3.Distance(transform.position, colliders[i].gameObject.transform.position) < Vector3.Distance(transform.position, NearestOfFood))
                    NearestOfFood = colliders[i].gameObject.transform.position;
            }
        }
        Vector2 dir = new Vector2(NearestOfFood.x, NearestOfFood.y);
        dir -= new Vector2(transform.position.x, transform.position.y);
        Food = NearestOfFood;
        dir.Normalize();
        rb.velocity = dir * speed;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90)), rotateSpeed * Time.deltaTime);
        if (energy <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == FoodConst)
        {
            Eat(foodSkill);
            Destroy(collision.gameObject);
        }
    }

    void Eat(float food)
    {
        energy += food;
        if (energy > 16)
        {
            energy *= 0.5f;
            GameObject cell = (GameObject)Instantiate(Resources.Load("m2", typeof(GameObject)), transform.position, Quaternion.identity);
            Cell cl = cell.GetComponent<Cell>();
            cl.energy = energy;
            cl.Mutate();
        }
    }

    public void Mutate()
    {
        if (Random.value < mutationChance)
            foodSkill = Random.Range(0, 5);
        if (Random.value < mutationChance)
            attackSkill = Random.Range(0, 5);
        if (Random.value < mutationChance)
            defenceSkill = Random.Range(0, 5);
    }
}
