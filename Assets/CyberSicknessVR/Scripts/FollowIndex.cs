using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowIndex : MonoBehaviour
{
    [Tooltip("Select the Target that this Collider+RigidBody will follow (suggested: The Index FingerTip).")]
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = target.transform.position;
    }
}
