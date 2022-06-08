using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    const string FoodConst = "food";
    const string CellConst = "cell";
    const float vision = 4f;
    const float mutationChance = 0.10f;
    const float rotateSpeed = 8f;

    public int inputsCount;
    public float foodSkill = 1;
    public float attackSkill = 0;
    public float defenceSkill = 0;
    public float energy = 20;

    public float size = 1;
    public NN nn;

    public Vector2 DIR;
    public Vector2[] VEC;
    private Rigidbody2D rb;
    float speed = 0.05f;
    
    public float age = 0; 

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        VEC = new Vector2[4];
    }

    void FixedUpdate()
    {
        age += Time.deltaTime;
        energy -= Time.deltaTime;
        if (energy <= 0)
        {
            Destroy(gameObject);
        }

        //float[] walls = new float[4];
        //float[] inputs = new float[inputsCount];
        //Vector3[] vectorInputs = new Vector3[4];
        //float[] summOfMass = new float[4];
        //for (int i = 0; i < 4; i++)
        //{
        //    summOfMass[i] = 0;
        //    vectorInputs[i] = new Vector3(0, 0, 0);
        //}

        //Get inputs
        float[] inputs = new float[inputsCount+3];
        
        Vector3[] vectorInputs = new Vector3[inputsCount / 2];
        float[] summOfMass = new float[inputsCount / 2];
        for (int i = 0; i < inputsCount / 2; i++)
        {
            summOfMass[i] = 0;
            vectorInputs[i] = new Vector3(0, 0, 0);
        }
        for (int i = 0; i < inputsCount; i++)
            inputs[i] = 0;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, vision);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == FoodConst)
            {
                vectorInputs[0] += colliders[i].transform.position;
                summOfMass[0] += 1;
            }

            if (colliders[i].gameObject.tag == CellConst)
            {
                AI ai = colliders[i].GetComponent<AI>();
                if (ai.foodSkill - ai.attackSkill >= 0)
                {
                    vectorInputs[1] += colliders[i].transform.position * (ai.foodSkill - ai.attackSkill);
                    summOfMass[1] += ai.foodSkill - ai.attackSkill;
                }
                if (defenceSkill - ai.attackSkill >= 0)
                {
                    vectorInputs[2] += colliders[i].transform.position * (defenceSkill - ai.attackSkill);
                    summOfMass[2] += defenceSkill - ai.attackSkill;
                }
                if (attackSkill - ai.defenceSkill >= 0)
                {
                    vectorInputs[3] += colliders[i].transform.position * (attackSkill - ai.defenceSkill);
                    summOfMass[3] += attackSkill - ai.defenceSkill;
                }
            }
        }

        //RaycastHit2D hit;
        //Vector3 directionOfRay = transform.position;
        //hit = Physics2D.Raycast(transform.position, new Vector3(1, 0, 0), Mathf.Infinity, 3); // +x       
        //walls[0] = Mathf.Abs(hit.point.x - transform.position.x);
        //hit = Physics2D.Raycast(transform.position, new Vector3(-1, 0, 0), Mathf.Infinity, 3); // -x       
        //walls[1] = Mathf.Abs(hit.point.x - transform.position.x);
        //hit = Physics2D.Raycast(transform.position, new Vector3(0, 1, 0), Mathf.Infinity, 3); // +y       
        //walls[2] = Mathf.Abs(hit.point.y - transform.position.y);
        //hit = Physics2D.Raycast(transform.position, new Vector3(0, -1, 0), Mathf.Infinity, 3); // -y      
        //walls[3] = Mathf.Abs(hit.point.y - transform.position.y);


        // Centers of mass
        for (int i = 0; i < vectorInputs.Length; i++)
        {
            if (summOfMass[i] != 0)
                vectorInputs[i] /= summOfMass[i];
        }
        // vectors to inputs
        for (int i = 0; i < inputsCount; i += 2)
        {
            inputs[i] = vectorInputs[i / 2].x;
            inputs[i + 1] = vectorInputs[i / 2].y;
        }
        // Centers of mass
        for (int i = 0; i < 4; i++)
        {
            if (summOfMass[i] != 0)
                vectorInputs[i] /= summOfMass[i];
            vectorInputs[i] = Activate(vectorInputs[i]);
            VEC[i] = vectorInputs[i];
        }
        // vectors to inputs
        for (int i = 0; i < 8; i += 2)
        {
            inputs[i] = vectorInputs[i / 2].x;
            inputs[i + 1] = vectorInputs[i / 2].y;
        }
        inputs[inputsCount] = foodSkill/5f;
        inputs[inputsCount + 1] = attackSkill/5f;
        inputs[inputsCount + 2] = defenceSkill/5f;
        //for(int i =0;i< inputs.Length;i++)
        //{
        //    inputs[i]= Activate(inputs[i]);
        //}
        //for(int i = 0; i < 4; i++)
        //{
        //    inputs[i+8] = walls[i];
        //} 

        // Get outputs
        float[] outputs = nn.FeedForward(inputs);
        Vector2 dir = new Vector2(0,0);
        for (int i = 0; i < outputs.Length; i++)
        {
            if (outputs[i] != 0)
                dir += outputs[i] * new Vector2(vectorInputs[i].x, vectorInputs[i].y);
        }
        //for (int i = 0; i < 4; i++)
        //{
        //    if (outputs[i] != 0)
        //        dir += outputs[i] * new Vector2(vectorInputs[i].x, vectorInputs[i].y);
        //}
        //dir.x += outputs[4];
        //dir.y += outputs[5];

        //dir -= new Vector2(transform.position.x, transform.position.y);
       
        dir.Normalize();
        DIR = dir;
        // Use outputs
        rb.velocity += dir * speed / size;
        //rb.velocity += dir * speed;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90)), rotateSpeed * Time.deltaTime);
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == FoodConst)
        {
            Eat(foodSkill);
            Destroy(collision.gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (attackSkill == 0) return;
        if (age < 1) return;
        if (collision.gameObject.tag == CellConst)
        {
            AI ai = collision.gameObject.GetComponent<AI>();
            float damage = attackSkill - ai.defenceSkill;
            if (damage > 0)
            {
                damage *= 0.5f;
                ai.energy -= damage * 1.25f;
                Eat(damage);
            }
        }
    }

    void Eat(float food)
    {
        energy += food;
        if (energy > 16)
        {
            energy *= 0.5f;
            GameObject cell = (GameObject)Instantiate(Resources.Load("m1", typeof(GameObject)), transform.position, Quaternion.identity);
            AI ai = cell.GetComponent<AI>();
            ai.nn = new NN(nn.layers, nn.sizes);
            ai.defenceSkill = defenceSkill;
            ai.attackSkill = attackSkill;
            ai.foodSkill = foodSkill;
            ai.energy = energy;
            ai.inputsCount = inputsCount;
            ai.Mutate();
        }
    }

    public void SetColor()
    {
        Color col = new Color(attackSkill / 5f, foodSkill / 5f, defenceSkill / 5f, 1f);
        gameObject.GetComponent<SpriteRenderer>().color = col;
        transform.localScale = new Vector3(size, size, size);
    }
    void Mutate()
    {
        if (Random.value < mutationChance)
            foodSkill = Random.Range(0f, 5f);
        if (Random.value < mutationChance)
            attackSkill = Random.Range(0f, 5f);
        if (Random.value < mutationChance)
            defenceSkill = Random.Range(0f, 5f);
        if (Random.value < mutationChance)
            size = Random.Range(0.75f, 1.5f);
        SetColor();
    }
    Vector2 Activate(Vector2 vec)
    {
        return (vec - ((Vector2)transform.position + (vec - (Vector2)transform.position).normalized * vision / 2)).normalized;
    }
}
