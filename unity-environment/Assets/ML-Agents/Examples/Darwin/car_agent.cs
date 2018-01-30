using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class car_agent : Agent
{
    [SerializeField]
    private float currentX;
    [SerializeField]
    private float currentZ;
    [SerializeField]
    private float targetX;
    [SerializeField]
    private float targetZ;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Transform cube;
    [SerializeField]
    private Transform sphere;
    [SerializeField]
    private float distance = 0;
    [SerializeField]
    private float pre_distance;

    [SerializeField]
    private float power = 100;
    [SerializeField]
    private float speed = 0;
    [SerializeField]
    private float angle = 0;


    private float sideALength;
    private float sideBLength;
    int solved;

    public override List<float> CollectState()
    {
        List<float> state = new List<float>();
        state.Add(currentX);
        state.Add(currentZ);
        state.Add(targetX);
        state.Add(targetZ);
        return state;
    }

    public override void AgentStep(float[] action)
    {
        if (text != null)
            text.text = string.Format("C:({0},{1}) / T:({2},{3})", currentX, currentZ, targetX, targetZ);
        switch ((int)action[0])
        {
            case 0:
                speed += 0.1f;
                break;
            case 1:
                speed -= 0.1f;
                break;
            case 2:
                angle += 0.05f;
                break;
            case 3:
                angle -= 0.05f;
                break;
            defult:
                return;

        }

        cube.transform.Translate(Vector3.forward * speed );
        cube.transform.Rotate(Vector3.up, angle);

        sideALength = targetX - currentX;
        sideBLength = targetZ - currentZ;

        pre_distance = distance;
        distance = Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);

        
    }

    public override void AgentReset()
    {
 
    }
}