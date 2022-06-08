using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    float timer = 0;
    Text txt;
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        if (minutes / 10 == 0)
            txt.text ="0" + minutes.ToString();
        else txt.text = minutes.ToString();
        if (seconds / 10 == 0)
            txt.text += ":0" + seconds.ToString();
        else txt.text += ":" + seconds.ToString();
    }

}
