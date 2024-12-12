using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierHealth : MonoBehaviour
{

    public int maxhealth = 100;
    public int currhealth = 0;
    public SoldierHealthBar bar;

    void Start()
    {
        currhealth = maxhealth;
        bar.Setmaxhealth(maxhealth);
    }

    public void takedmg(int dmg)
    {
        currhealth -= dmg;
        bar.Sethealth(currhealth);
    }

}