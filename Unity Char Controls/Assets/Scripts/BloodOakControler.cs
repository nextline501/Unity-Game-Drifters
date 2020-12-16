using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodOakControler : MonoBehaviour
{
    Text text;
    private int bloodScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = bloodScore + "/1";
        Debug.Log(bloodScore);
    }

    public void updateScore()
    {
        bloodScore++;
    }

    public int getSupplyScore()
    {
        return bloodScore;
    }
}
