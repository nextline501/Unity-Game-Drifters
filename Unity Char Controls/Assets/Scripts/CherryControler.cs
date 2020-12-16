using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CherryControler : MonoBehaviour
{
    Text text;
    private int cherryScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = cherryScore + "/3";
        Debug.Log(cherryScore);
    }

    public void updateScore()
    {
        cherryScore++;
    }

    public int getSupplyScore()
    {
        return cherryScore;
    }
}
