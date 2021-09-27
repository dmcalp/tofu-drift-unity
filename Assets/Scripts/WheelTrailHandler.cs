using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailHandler : MonoBehaviour
{
    Player player;
    TrailRenderer trailRenderer;

    //Awake is called when the script instance is being loaded
    void Awake()
    {
        player = GetComponentInParent<Player>();
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false; // don't emit at start
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player.IsTyreSlipping(out float lateralVelocity))
            trailRenderer.emitting = true;
        else
            trailRenderer.emitting = false;
    }
}
