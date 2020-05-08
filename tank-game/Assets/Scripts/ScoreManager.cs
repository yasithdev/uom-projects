using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Dictionary<int, Tank> tanks;


    private GameObject entry;
    private static ScoreManager instance;
    

    public static ScoreManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        instance = this;
    }
    
    //// Update is called once per frame
    void Update()
    {
        try
        {
            if (tanks != null)
            {
                GameObject C1 = instance.transform.GetChild(0).gameObject;
                C1.transform.Find("Points").GetComponent<Text>().text = tanks[0].points.ToString();
                C1.transform.Find("Coins").GetComponent<Text>().text = tanks[0].coins.ToString();
                C1.transform.Find("Health").GetComponent<Text>().text = tanks[0].health.ToString();

                GameObject C2 = instance.transform.GetChild(1).gameObject;
                C2.transform.Find("Points").GetComponent<Text>().text = tanks[1].points.ToString();
                C2.transform.Find("Coins").GetComponent<Text>().text = tanks[1].coins.ToString();
                C2.transform.Find("Health").GetComponent<Text>().text = tanks[1].health.ToString();

                GameObject C3 = instance.transform.GetChild(2).gameObject;
                C3.transform.Find("Points").GetComponent<Text>().text = tanks[2].points.ToString();
                C3.transform.Find("Coins").GetComponent<Text>().text = tanks[2].coins.ToString();
                C3.transform.Find("Health").GetComponent<Text>().text = tanks[2].health.ToString();

                GameObject C4 = instance.transform.GetChild(3).gameObject;
                C4.transform.Find("Points").GetComponent<Text>().text = tanks[3].points.ToString();
                C4.transform.Find("Coins").GetComponent<Text>().text = tanks[3].coins.ToString();
                C4.transform.Find("Health").GetComponent<Text>().text = tanks[3].health.ToString();

                GameObject C5 = instance.transform.GetChild(4).gameObject;
                C5.transform.Find("Points").GetComponent<Text>().text = tanks[4].points.ToString();
                C5.transform.Find("Coins").GetComponent<Text>().text = tanks[4].coins.ToString();
                C5.transform.Find("Health").GetComponent<Text>().text = tanks[4].health.ToString();

                //foreach (var t in tanks)
                //{
                //entry = transform.Find("p" + t.Key).gameObject;
                //entry.transform.Find("Points").GetComponent<Text>().text = t.Value.points.ToString();
                //entry.transform.Find("Coins").GetComponent<Text>().text = t.Value.coins.ToString();
                //entry.transform.Find("Health").GetComponent<Text>().text = t.Value.health.ToString();
                //foreach (Transform trns in transform)
                //{
                //    if (trns.name == "p" + t.Key)
                //    {
                //        foreach (Transform trnsform in transform)
                //        {
                //            if (trnsform.name == "Points")
                //            {
                //                trnsform.GetComponent<Text>().text = t.Value.points.ToString();
                //            }
                //            else if (trnsform.name == "Coins")
                //            {
                //                trnsform.GetComponent<Text>().text = t.Value.coins.ToString();
                //            }
                //            else if (trnsform.name == "Health")
                //            {
                //                trnsform.GetComponent<Text>().text = t.Value.health.ToString();
                //            }

                //        }
                //    }

                //}
                //}
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.LogError(ex);
        }
        
    }

    public void updateScore(Dictionary<int, Tank> tanksDictionary)
    {
        tanks = tanksDictionary;
    }
    
}
