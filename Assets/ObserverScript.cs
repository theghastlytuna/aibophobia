using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverScript : MonoBehaviour
{
    public static ObserverScript ObserverInstance { get; private set; } //singleton

    public List<GameObject> subscribers = new List<GameObject>();

    private float Timer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        //singleton check
        if (ObserverInstance != null && ObserverInstance != this)
        {
            Destroy(this);
        }
        else
        {
            ObserverInstance = this;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= 5)
        {
            NotifySubscribers();
            Timer = 0;
        }
    }

    void NotifySubscribers()
    {
        for (int num = 0; num < subscribers.Count; num++)
        {
            if (subscribers[num])
            subscribers[num].GetComponent<EnemyBehaviour>().Jump();
        }
    }
}
