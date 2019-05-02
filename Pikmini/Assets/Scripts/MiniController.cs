
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
    
    // Note: this project does not make use of RandomizeThrottle() and instead
    // only uses the SerializeField to set the Throttle.
    // According to the TA on piazza cid=44, either implementation is fine.
    [SerializeField]
    private float Throttle;
    private int GroupID = 1;
    
    private ColorWatcher Watcher;
    private float TimeToWatch;
    private float StartTime;

    private float Lifespan;
    private float DeathTimer;
    private float StartDeathTimer;
    
    void Awake()
    {
        this.PublisherManager = GameObject.FindGameObjectWithTag("Script Home").GetComponent<PublisherManager>();
        this.RandomizeBody();
        this.GroupID = Random.Range(1, 4);
        this.PublisherManager.Register(GroupID, OnMoveMessage);

        // Used for Throttling calls to Watch().
        this.TimeToWatch = 0.0f;
        this.StartTime = Time.time;

        // Assign pikmini lifespan to be between 10 and 40 seconds.
        this.Lifespan = Random.Range(10.0f, 40.0f);
        this.DeathTimer = 0.0f;
        this.StartDeathTimer = Time.time;
        
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
        // Throttle value of 0.0f makes the below if-else code do stage 1.1.
        // Otherwise the below if-else code does stage 1.2.
        if (this.TimeToWatch >= Throttle)
        {
            this.TimeToWatch = 0.0f;
            this.StartTime = Time.time;
            this.Watcher.Watch();
        }
        else
        {
            // keep incrementing the TimeToWatch up.
            this.TimeToWatch = (Time.time - StartTime);
        }
        
        // Determine pikmini's lifespan.
        if (this.DeathTimer >= this.Lifespan)
        {            
            // First, unregister the pikmini.
            this.PublisherManager.Unregister(this.GroupID, OnMoveMessage);
            
            // Second, invoke its death.
            Destroy(this.gameObject);
            
            // Finally, reset the death timer.
            this.DeathTimer = 0.0f;
            this.StartDeathTimer = Time.time;
        }
        else
        {
            // Keep incrementing the DeathTimer up.
            this.DeathTimer = (Time.time - StartDeathTimer);
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
        if(agent != null)
        {
            agent.SetDestination(destination);
        }
    }
}