using System;
using UnityEngine;
using Pikmini;
// change name to Publisher Manager
public class PublisherManager : MonoBehaviour
{
    public int GroupCount { get; } = 3;
    
    private IPublisher Group1Publisher;
    private IPublisher Group2Publisher;
    private IPublisher Group3Publisher;

    void Awake()
    {
        Group1Publisher = new Publisher();
        Group2Publisher = new Publisher();
        Group3Publisher = new Publisher();
    }
    
    public void SendMessageWithPublisher(int group, Vector3 destination)
    {
        switch (group)
        {
            case 1:
                this.Group1Publisher.Notify(destination);
                break;
            case 2:
                this.Group2Publisher.Notify(destination);
                break;
            case 3:
                this.Group3Publisher.Notify(destination);
                break;
        }
    }
    public void Register(int group, Action<Vector3> callback)
    {
        switch (group)
        {
            case 1:
                this.Group1Publisher.Register(callback);
                break;
            case 2:
                this.Group2Publisher.Register(callback);
                break;
            case 3:
                this.Group3Publisher.Register(callback);
                break;
        }
    }
    public void Unregister(int group, Action<Vector3> callback)
    {
        switch (group)
        {
            case 1:
                this.Group1Publisher.Unregister(callback);
                break;
            case 2:
                this.Group2Publisher.Unregister(callback);
                break;
            case 3:
                this.Group3Publisher.Unregister(callback);
                break;
        }
    }    
}
