using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindGustControler : MonoBehaviour
{
    Text text;
    private int windGustScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = windGustScore + "/150";
        Debug.Log(windGustScore);
    }

    public void updateScore()
    {
        windGustScore++;
    }

    public int getSupplyScore()
    {
        return windGustScore;
    }
}
