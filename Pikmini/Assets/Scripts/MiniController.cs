﻿
using UnityEngine;
using UnityEngine.AI;

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
    
    void Awake()
    {
        this.PublisherManager = GameObject.FindGameObjectWithTag("Script Home").GetComponent<PublisherManager>();
        this.RandomizeBody();
        this.RandomizeThrottle();
        this.GroupID = Random.Range(1, 4);
        this.PublisherManager.Register(GroupID, OnMoveMessage);

        Watcher = new ColorWatcher(ColorBindings.GetGroup1Color, ChangeColor);
    }

    void Update()
    {
        this.Watcher.Watch();
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