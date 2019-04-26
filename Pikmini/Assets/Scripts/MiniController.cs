
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class MiniController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private ColorBind ColorBindings;
    private PublisherManager PublisherManager;
    [SerializeField]
    private float Throttle;
    private int GroupID = 1;
    
    private ColorWatcher Watcher;
    private float TimeToWatch;
    private float StartTime;
    
    void Awake()
    {
        this.PublisherManager = GameObject.FindGameObjectWithTag("Script Home").GetComponent<PublisherManager>();
        this.RandomizeBody();
        // this.RandomizeThrottle();
        this.GroupID = Random.Range(1, 4);
        this.PublisherManager.Register(GroupID, OnMoveMessage);

        this.TimeToWatch = 0.0f;
        this.StartTime = Time.time;
        // Placeholder initialization to make compiler happy.
        Func<Color> chosenFunction = () => new Color(0,0,0);
        
        // Pick a random group color assignment at Watcher initialization.
        switch(this.GroupID)
        {
            case 1:
                chosenFunction = this.ColorBindings.GetGroup1Color;
                break;
            
            case 2:
                chosenFunction = this.ColorBindings.GetGroup2Color;
                break;

            case 3:
                chosenFunction = this.ColorBindings.GetGroup3Color;
                break;
            
            default:
                Debug.Log("Entered default case (this should not happen).");
                break;
        }
        
        this.Watcher = new ColorWatcher(chosenFunction, ChangeColor);
    }

    void Update()
    {
        if (this.TimeToWatch >= Throttle)
        {
            this.TimeToWatch = 0.0f;
            this.StartTime = Time.time;
            this.Watcher.Watch();
        }
        else
        {
            this.TimeToWatch = (Time.time - StartTime);
        }
    }
    
    void OnMouseDown()
    {
        this.PublisherManager.Unregister(GroupID, OnMoveMessage);
        this.GroupID = (this.GroupID % this.PublisherManager.GroupCount) + 1;
        this.PublisherManager.Register(GroupID, OnMoveMessage);
    }
    
    private void ChangeColor(Color color)
    {
        foreach (Transform child in transform)
        {
            Renderer rend = child.GetComponent<Renderer>();
            rend.material.SetColor("_Color", color);
        }
    }

    private void RandomizeBody()
    {
        var ranScale = Random.Range(0.5f, 1.0f);
        foreach (Transform child in transform)
        {
            child.localScale *= ranScale;
        }
    }

    private void RandomizeThrottle()
    {
        Throttle = Random.Range(0.0f, 10.0f);   
    }

    public void OnMoveMessage(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
}