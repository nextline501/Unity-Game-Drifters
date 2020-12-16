using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupplyControlUI : MonoBehaviour
{
    Text text;
    private int supplyScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = supplyScore + "/20";
        Debug.Log(supplyScore);
    }

    public void updateScore()
    {
        supplyScore++;
    }

    public int getSupplyScore()
    {
        return supplyScore;
    }
}
