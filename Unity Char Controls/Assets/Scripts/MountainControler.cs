using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MountainControler : MonoBehaviour
{
    Text text;
    private int mountainScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = mountainScore + "/1";
        Debug.Log(mountainScore);
    }

    public void updateScore()
    {
        mountainScore++;
    }

    public int getSupplyScore()
    {
        return mountainScore;
    }
}
