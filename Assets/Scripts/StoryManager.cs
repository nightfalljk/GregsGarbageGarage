using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using TMPro;
using Trash.Gameplay;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class StoryManager : MonoBehaviour
{
    [SerializeField]  private TextMeshProUGUI gregText;
    [SerializeField] private GameObject gregTextParentObject;
    [SerializeField] private GameObject greg;
    [SerializeField] private GameObject buildMenu;
    [SerializeField] private GameObject tooltipMenu;
    [FormerlySerializedAs("StopTimeSpeed")] [SerializeField] private float stopTimeSpeed = 0.01f;
    private string day1TextPart1 = "Oh, Hi! Welcome to my Garbage Garage.\n " +
                                   "Good to have a new Garage Manager. I had to let the old one go. " +
                                   "He just scavenged the trash bags for food and made a mess. \n" +
                                   "That’s what happens when you hire a raccoon, I guess.";
    private string day1TextPart2 = "The job is real easy. You’ll see. " +
                                   "\nI suggest you build a few new conveyor belts to the trash compactor, so you don’t have to put everything in there by hand." +
                                   "\nI’m Greg by the way. This is my garage, so we’ll be seeing each other a lot. " +
                                   "\nEnjoy your first day.";

    private string[] morningTexts;
    


    private Report m_report;
    
    private int currentDay = 0;
    private bool nextText = false;


    public void Awake()
    {
        m_report = FindObjectOfType<Report>();
        morningTexts = new string[6];
        
        morningTexts[0] = "The city council met yesterday. They fine us for burning trash now. " +
                          "\nI want you to sort the Paper and Plastic out of the trash so we can make some extra money with recycling." +
                          "\nDon’t make a mess. Or you’ll have to pay the clean up crew.";

        morningTexts[1] = "I met with city council yesterday. They need metal to make statues. " +
                          "\nWe have metal waste. They have money. I want money." +
                          "\nPut all that metal into a bin so it can be melted down and made into glorious statues.";

        morningTexts[2] =
            "Apparently, they don’t want our trash overseas anymore. So you’ll have to deal with a lot more garbage now. " +
            "\nSince the garage is so small, I decided we should stack it so you can process more garbage and make me more money. " +
            "\nGood luck with that. ";
        
        morningTexts[3] = "You smell that? That is organic waste. Disgusting, right? But also profitable. " +
                          "\nFarmers are willing to pay me good money for compost. " +
                          "\nYou’ll need to sort the organic waste into a bin, so we can turn that into money.";

        morningTexts[4] ="I got another new waste for you: Glass. " +
            "\nIt can just be crushed and melted into brand-new glass. Isn’t that great?So start sorting. " +
            "\nBut don’t cut yourself, you’re my most profitable garage manager yet.";

        morningTexts[5] =
            "Danny, the intern, had a brilliant idea. He thinks that there could be a lot of money in recycling electronics. " +
            "\nI think he is onto something there. You should start putting electronics into a bin. ";


    }

    public void Update()
    {
        if (Keyboard.current.kKey.wasReleasedThisFrame)
        {
            PauseGame();
        }
        if (Keyboard.current.lKey.wasReleasedThisFrame)
        {
            ContinueGame();
        }
        if (Keyboard.current.enterKey.wasReleasedThisFrame)
        {
            OnGregTextClick();
        }

        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            if (Time.timeScale < 0.5f)
            {
                ContinueGame();
            }
            else
            {
                PauseGame(false);
            }
        }
    }

    public void OnBeginning()
    {
        nextText = true;
        SetGregText(day1TextPart1);
    }
    
    public void OnDayStart(int day)
    {
        currentDay = day;
        Debug.Log("StartDayTriggered");
        if (currentDay == 1)
        {
            OnBeginning();
        } else
        if (currentDay-1 < morningTexts.Length)
        {
            SetGregText(morningTexts[currentDay-2]);
        }
        //currentDay += 1;
    }

//    public void OnDayEnd()
//    {
//        
//    }
//
//    public void NightStart()
//    {
//        
//    }

    public void OnNightEnd()
    {
        GenerateReport();
    }
    
    public void GenerateReport()
    {
        PauseGame();
        m_report.CreateReport();
    }

    public void PauseGame(bool forceStop=true)
    {
        Time.timeScale = stopTimeSpeed;
        if (forceStop)
        {
            ForceStopBuilding(true);
        }
        LockGrabbing(true);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        //ForceStopBuilding(false);
        LockGrabbing(false);
    }
    
    public void SetGregText(string s)
    {
        PauseGame();
        gregText.text = s;
        gregTextParentObject.SetActive(true);
        greg.SetActive(true);
        
        buildMenu.SetActive(false);
        tooltipMenu.GetComponent<Tooltip>().Deactivate();
    }

    public void OnGregTextClick()
    {
        if (nextText)
        {
            nextText = false;
            SetGregText(day1TextPart2);
        }
        else
        {
            greg.SetActive(false);
            ContinueGame();
            buildMenu.SetActive(true);
            gregTextParentObject.SetActive(false);
        }
    }

    public void ForceStopBuilding(bool b)
    {
        if (b)
        {
            GridBasedBuilding gbb = FindObjectOfType<GridBasedBuilding>();
            gbb.SetBuildmode(GridBasedBuilding.BuildMode.off);
        }
        else
        {
            
        }
    }

    public void LockGrabbing(bool b)
    {
        //TODO
        if (b)
        {
            FindObjectOfType<MouseInteraction>().allowGrabbing = false;
        }
        else
        {
            FindObjectOfType<MouseInteraction>().allowGrabbing = true;
        }
    }

    public void OnCloseReport()
    {
        ContinueGame();
    }
}
