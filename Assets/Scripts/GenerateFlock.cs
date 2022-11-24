using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFlock : MonoBehaviour
{
    public GameObject boidPrefab;
    public int numberOfBoids;
    public float maxPosX = 1.0f;
    public float maxPosZ = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        GenerateBoids();
    }

    private void GenerateBoids()
    {
        GameObject coordinator = GameObject.Find("Coordinator");
        for(int i = 0; i < numberOfBoids; i++)
        {
            GameObject boid = Instantiate(boidPrefab, Vector3.zero, Quaternion.identity);
            boid.transform.position = new Vector3(Random.Range(-maxPosX, maxPosX), 1.0f, Random.Range(-maxPosZ, maxPosZ));
            boid.transform.parent = this.transform;
            coordinator.GetComponent<Coordinator>().AddVehicle(boid);
        }
    }
}
