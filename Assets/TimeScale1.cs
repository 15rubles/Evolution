using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScale1 : MonoBehaviour
{
    Text txt;
    void Start()
    {
        txt = gameObject.GetComponent<InputField>().textComponent.GetComponent<Text>();
    }

    public void ChangeTimeScale()
    {
        float input = float.Parse(txt.text);
        if(input > 0)
            Time.timeScale = input;
    }
}
