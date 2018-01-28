using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberDemoAgent : Agent
{
    [SerializeField]
    private float currentNumberX;
    [SerializeField]
    private float currentNumberZ;
    [SerializeField]
    private float targetNumberX;
    [SerializeField]
    private float targetNumberZ;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Transform cube;
    [SerializeField]
    private Transform sphere;
    [SerializeField]
    private float difference = 0;
    [SerializeField]
    private float pre_difference;
    [SerializeField]
    private int health = 50;
    [SerializeField]
    private bool attack = false;
    [SerializeField]
    private bool can_attack = false;
    [SerializeField]
    private int enemy_health = 50;


    public Behaviour halo;
    MonitorType displayType_1;
    MonitorType displayType_2;
    MonitorType displayType_3;
    MonitorType displayType_4;


    private float sideALength;
    private float sideBLength;
    int solved = 0;
    int died = 0;


    public override List<float> CollectState()
    {
        List<float> state = new List<float>();
        state.Add(currentNumberX);
        state.Add(currentNumberZ);
        state.Add(targetNumberX);
        state.Add(targetNumberZ);
        return state;
    }

    public override void AgentStep(float[] action)
    {



        /*
                public enum MonitorType
            {
                slider,
                hist,
                text,
                bar
            }

             * Use the Monitor.Log static function to attach information to a transform.
             * If displayType is <text>, value can be any object. 
             * If sidplayType is <slider>, value must be a float.
             * If sidplayType is <hist>, value must be a List or Array of floats.
             * If sidplayType is <bar>, value must be a list or Array of positive floats.
             * Note that <slider> and <hist> caps values between -1 and 1.
             * @param key The name of the information you wish to Log.
             * @param value The value you want to display.
             * @param displayType The type of display.
             * @param target The transform you want to attach the information to.

        */






        if (text != null)
            text.text = string.Format("C:({0},{1}) / T:({2},{3})    good:[{4}]   bad:[{5}]", currentNumberX, currentNumberZ, targetNumberX, targetNumberZ, solved, died);

        switch ((int)action[0])
        {
            case 0:
                currentNumberX -= 0.1f;
                break;
            case 1:
                currentNumberZ -= 0.1f;
                break;
            case 2:
                currentNumberX += 0.1f;
                break;
            case 3:
                currentNumberZ += 0.1f;
                break;
            case 4:
                attack = true;
                break;
            defult:
                return;

        }

        cube.position = new Vector3(currentNumberX * 5f, 3f, currentNumberZ * 5f);

        sideALength = targetNumberX - currentNumberX;
        sideBLength = targetNumberZ - currentNumberZ;

        pre_difference = difference;
        difference = Mathf.Sqrt( sideALength * sideALength + sideBLength * sideBLength );


        if (health < 50)
        {

            halo.enabled = false;
            health += 1;
            can_attack = true;
            done = false;
            return;

        }



        if (health >= 25)
        {

            can_attack = true;

            if (difference < pre_difference)
            {

                reward = 0.3f ;
                
                done = false;
                return;

            }

        }
        else
        {

            if (difference > pre_difference)
            {

                reward = 0.5f ;
      
                done = false;
                return;

            }

        }


        if(health <= 0)
        {

            died++;
            reward = -1;
            done = true;

        }

        if (difference <= 1.2)
        {

            halo.enabled = true;
            health -= 20;
            reward = -0.5f;
            return;

        }


        if (enemy_health <= 0)
        {

            
            sphere.position = new Vector3(targetNumberX * 5, 10,targetNumberZ * 5);
            solved ++;
            reward = 1.2f;
            done = true;
            return;

        }


        if(attack == true)
        {

            if(difference <= 1.5 && can_attack == true)
            {

               
                enemy_health -= 1;
                reward = 1;
                attack = false;
                can_attack = false;
                done = false;
                
                return;

            }
            else
            {

                reward = -1.2f;
                done = false;
                return;

            }

        }
       
        
        

        reward = -1.2f * difference;
        return;

    }

    public override void AgentReset()
    {
        health = 50;
        enemy_health = 50;
        targetNumberX = UnityEngine.Random.RandomRange(-1f, 1f);
        targetNumberZ = UnityEngine.Random.RandomRange(-1f, 1f);
        sphere.position = new Vector3(targetNumberX * 5, 3, targetNumberZ * 5);
        currentNumberX = 0f;
        currentNumberZ = 0f;
    }
}
