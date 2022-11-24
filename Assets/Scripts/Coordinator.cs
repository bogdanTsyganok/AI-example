using UnityEngine;
using System.Collections.Generic;

public class Coordinator : MonoBehaviour
{
    public bool isInFormation = false;

    public List<GameObject> vehicles;
    public Transform[] coordinates;
    public bool boidsFollowing = false;
    Vector3[] positionOffset = null;

    float aliWeight = 0.4f;
    float sepWeight = 0.4f;
    float cohWeight = 0.4f;

    public enum eFormations { Circle, V, Square, Line, Rows }
    public eFormations currentFormation = eFormations.Line;
    // Start is called before the first frame update
    void Start()
    {
        positionOffset = new Vector3[12];
        setFlocking(false);
        setFollowing(false);
        copyOffset();
    }

    public void AddVehicle(GameObject vehicle)
    {
        vehicles.Add(vehicle);
    }

    private void copyOffset()
    {
        switch (currentFormation)
        {
            case eFormations.Circle:
                {
                    float radianDegrees = 0.0f;
                    float increment = Mathf.PI / 6.0f;
                    float radius = 10.0f;
                    for(int i = 0; i < 12; i++)
                    {
                        coordinates[i].localPosition = new Vector3(Mathf.Cos(radianDegrees), 0, Mathf.Sin(radianDegrees));
                        coordinates[i].localPosition *= radius;
                        radianDegrees += increment;
                    }
                    break;
                }
            case eFormations.Line:
                {
                    float position = -11.0f;

                    for (int i = 0; i < 12; i++)
                    {
                        coordinates[i].localPosition = new Vector3(position, 0, 0);
                        position += 2;
                    }
                    break;
                }
            case eFormations.Rows:
                {
                    float position = -11.0f;

                    for (int i = 0; i < 6; i++)
                    {
                        coordinates[i].localPosition = new Vector3(position, 0, 0);
                        position += 4;
                    }
                    position = -11.0f;
                    for (int i = 0; i < 6; i++)
                    {
                        coordinates[i + 6].localPosition = new Vector3(position, 0, -4);
                        position += 4;
                    }
                    break;
                }
            case eFormations.Square:
                {
                    float position = -3.0f;

                    for (int i = 0; i < 4; i++)
                    {
                        coordinates[i].localPosition = new Vector3(position, 0, -3);
                        position += 2;
                    }

                    position = -3.0f;
                    for (int i = 0; i < 4; i++)
                    {
                        coordinates[i + 4].localPosition = new Vector3(position, 0, 3);
                        position += 2;
                    }

                    position = -3.0f;
                    for (int i = 0; i < 2; i++)
                    {
                        coordinates[i + 8].localPosition = new Vector3(position, 0, 1);
                        position += 6;
                    }
                    position = -3.0f;
                    for (int i = 0; i < 2; i++)
                    {
                        coordinates[i + 10].localPosition = new Vector3(position, 0, -1);
                        position += 6;
                    }

                    break;
                }
            case eFormations.V:
                {
                    float position = 4;

                    for (int i = 0; i < 6; i++)
                    {
                        coordinates[i].localPosition = new Vector3(-position / 2, 0, position);
                        position += 4;
                    }

                    position = 0;
                    for (int i = 0; i < 6; i++)
                    {
                        coordinates[i + 6].localPosition = new Vector3(-position / 2, 0, -position);
                        position += 4;
                    }
                    break;
                }
            default:
                //no
                break;
        }
        for(int i = 0; i < coordinates.Length; i++)
        {
            positionOffset[i] = coordinates[i].localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            setFlocking(false);
            currentFormation = eFormations.Circle;
            copyOffset();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            setFlocking(false);
            currentFormation = eFormations.V;
            copyOffset();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            setFlocking(false);
            currentFormation = eFormations.Square;
            copyOffset();

        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            setFlocking(false);
            currentFormation = eFormations.Line;
            copyOffset();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            setFlocking(false);
            currentFormation = eFormations.Rows;
            copyOffset();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha6))
        {
            //start flocking
            setFlocking(true);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            //return to the formation
            setFlocking(false);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (isInFormation)
            {
                //start pathFollowing
                //this.GetComponent<PathFollowing>().active = true; setFollowing(true);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            //this.GetComponent<PathFollowing>().Reverse();
        }
        else if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            //this.GetComponent<PathFollowing>().active = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            setFlocking(false);
            setFollowing(false);
        }
        else if(Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            if (!isInFormation)
            {
                //this.GetComponent<PathFollowing>().active = true; setFollowing(true);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Z))
        {
            //decrease cohesion
            if (cohWeight > 0.1f && aliWeight < 0.9f && sepWeight < 0.9f)
            {
                cohWeight -= 0.1f;
                aliWeight += 0.05f;
                sepWeight += 0.05f;
                Debug.Log("Cohesion: " + cohWeight + ", alignment: " + aliWeight + ", seperation: " + sepWeight);
                updateWeight();
            }
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            //increase cohesion
            if (cohWeight < 0.9f && aliWeight > 0.1f && sepWeight > 0.1f)
            {
                cohWeight += 0.1f;
                aliWeight -= 0.05f;
                sepWeight -= 0.05f;
                Debug.Log("Cohesion: " + cohWeight + ", alignment: " + aliWeight + ", seperation: " + sepWeight);
                updateWeight();
            }
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            //decrease alignment
            if (aliWeight > 0.1f && cohWeight < 0.9f && sepWeight < 0.9f)
            {
                aliWeight -= 0.1f;
                cohWeight += 0.05f;
                sepWeight += 0.05f;
                Debug.Log("Cohesion: " + cohWeight + ", alignment: " + aliWeight + ", seperation: " + sepWeight);
                updateWeight();
            }
        }
        else if(Input.GetKeyDown(KeyCode.V))
        {
            //increase alignment
            if (aliWeight < 0.9f && cohWeight > 0.1f && sepWeight > 0.1f)
            {
                aliWeight += 0.1f;
                cohWeight -= 0.05f;
                sepWeight -= 0.05f;
                Debug.Log("Cohesion: " + cohWeight + ", alignment: " + aliWeight + ", seperation: " + sepWeight);
                updateWeight();
            }
        }
        else if(Input.GetKeyDown(KeyCode.B))
        {
            //decrease seperation
            if (sepWeight > 0.1f && aliWeight < 0.9f && cohWeight < 0.9f)
            {
                sepWeight -= 0.1f;
                cohWeight += 0.05f;
                aliWeight += 0.05f;
                Debug.Log("Cohesion: " + cohWeight + ", alignment: " + aliWeight + ", seperation: " + sepWeight);
                updateWeight();
            }
        }
        else if(Input.GetKeyDown(KeyCode.N))
        {
            //increase seperation
            if (sepWeight < 0.9f && cohWeight > 0.1f && sepWeight > 0.1f)
            {
                sepWeight += 0.1f;
                cohWeight -= 0.05f;
                aliWeight -= 0.05f;
                Debug.Log("Cohesion: " + cohWeight + ", alignment: " + aliWeight + ", seperation: " + sepWeight);
                updateWeight();
            }
        }

        if (isInFormation)
        {
            for (int i = 0; i < vehicles.Count; i++)
            {
                LeaderFollowing formationUnit = vehicles[i].GetComponent<LeaderFollowing>();
                formationUnit.setTargetPosition(coordinates[i].position);
            }
        }
    }

    private void setFlocking(bool shouldFlock)
    {
        for(int i = 0; i < vehicles.Count; i++)
        {
            vehicles[i].GetComponent<LeaderFollowing>().setFlocking(shouldFlock);
            isInFormation = !shouldFlock;
        }
    }

    private void setFollowing(bool shouldFollow)
    {
        for(int i = 0; i < vehicles.Count; i++)
        {
            vehicles[i].GetComponent<LeaderFollowing>().setFollowing(shouldFollow);
            boidsFollowing = shouldFollow;
        }
    }

    private void updateWeight()
    {
        for (int i = 0; i < vehicles.Count; i++)
        {
            vehicles[i].GetComponent<LeaderFollowing>().cohWeight = cohWeight;
            vehicles[i].GetComponent<LeaderFollowing>().aliWeight = aliWeight;
            vehicles[i].GetComponent<LeaderFollowing>().sepWeight = sepWeight;
        }
    }
}
