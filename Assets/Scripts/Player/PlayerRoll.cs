using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRoll : MonoBehaviour
{
    public int currentRoll;
    public int numOfRolls;
    
    public Image[] rolls;
    public Sprite fullRoll;
    public Sprite emptyRoll;
    
    void Awake()
    {
        currentRoll = 3;
    }

    void Update()
    {
        if (currentRoll > numOfRolls)
        {
            currentRoll = numOfRolls;
        }
        
        for (int i = 0; i < rolls.Length; i++)
        {
            if (i < currentRoll)
            {
                rolls[i].sprite = fullRoll;
            }
            else
            {
                rolls[i].sprite = emptyRoll;
            }
            
            if (i < numOfRolls)
            {
                rolls[i].enabled = true;
            }
            else
            {
                rolls[i].enabled = false;
            }
        }
    }

    public void AddRoll()
    {
        if (currentRoll < 3)
        {
            currentRoll++;
        }
    }

    public void RemoveRoll()
    {
        if (currentRoll > 0)
        {
            currentRoll--;
        }
    }
}
