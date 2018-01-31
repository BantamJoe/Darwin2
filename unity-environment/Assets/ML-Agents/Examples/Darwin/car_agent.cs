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
            text.text = string.Format("C:({0},{1}) / T:({2},{3})    [{4}]", currentX, currentZ, targetX, targetZ, solved);

        switch ((int)action[0])
        {

            case 0:
                speed += 0.005f;
                power -= 0.005f;
                break;
            case 1:
                speed -= 0.005f;
                power -= 0.005f;
                break;
            case 2:
                angle += 0.01f;
                power -= 0.01f;
                break;
            case 3:
                angle -= 0.01f;
                power -= 0.01f;
                break;
            defult:
                return;

        }

        currentX = cube.GetComponent<Transform> ().position.x;
        currentZ = cube.GetComponent<Transform>().position.z;

        cube.transform.Translate(Vector3.forward * speed );
        cube.transform.Rotate(Vector3.up, angle);

        sideALength = targetX - currentX;
        sideBLength = targetZ - currentZ;

        pre_distance = distance;
        distance = Mathf.Sqrt(sideALength * sideALength + sideBLength * sideBLength);

        if(distance <= 0.5)
        {
            solved++;
            reward = 1;
            done = true;
            return;
        }

        if(distance < pre_distance)
        {

            reward = 0.3f;

        }
        else
        {
            reward = -0.3f;
        }

        reward = (-1.7f * distance) + (-1.2f * (100 - power));
        done = false;
        return;
        
    }

    public override void AgentReset()
    {
        power = 100;
        angle = 0;
        speed = 0;
        targetX = UnityEngine.Random.RandomRange(-1f, 1f);
        targetZ = UnityEngine.Random.RandomRange(-1f, 1f);
        sphere.position = new Vector3(targetX * 20, 3, targetZ * 20);
        currentX = 0f;
        currentZ = 0f;
        cube.position = new Vector3(currentX * 5, 3, currentZ * 5);
    }
}