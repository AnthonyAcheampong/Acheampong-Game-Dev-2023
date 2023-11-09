//Anthony Acheampong 11/09/2023 - Spawn

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject asteroids_prefab;
    public Camera camera;
    public Plane[] camera_frustum;
    private Vector3 ray_dir = Vector3.forward;
    private int asteroids_to_spawn = 3;

    [HideInInspector] public int asteroids_count = 0;
   


    // Start is called before the first frame update
    void Start()
    {
        //get planes from camera frustum
        camera_frustum = GeometryUtility.CalculateFrustumPlanes(camera);
       
        CheckAsteroidCount();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(new Vector3(camera.transform.position.x, 0, camera.transform.position.z), ray_dir * 10000, Color.green);  
        
    }

    private void SpawnAsteroids()
    {
        Vector2 random_dir = Random.insideUnitCircle.normalized;
        ray_dir = new Vector3(random_dir.x, 0, random_dir.y);
        Ray ray = new Ray(new Vector3(camera.transform.position.x, 0, camera.transform.position.z), ray_dir);

        float enter = 0.0f;
        Vector3 hit_point = Vector3.zero;
        float shortest_lenght = 1000;

        for (int i = 0; i <= 3; i++)
        {
            if (camera_frustum[i].Raycast(ray, out enter))
            {
                if (ray.GetPoint(enter).magnitude < shortest_lenght)
                {
                    hit_point = ray.GetPoint(enter);
                    shortest_lenght = ray.GetPoint(enter).magnitude;
                }
            }

        }
        GameObject asteroid = Instantiate(asteroids_prefab, hit_point, Quaternion.identity);


        asteroid.GetComponent<Asteroids>().spawner = this.gameObject;
        asteroids_count++;
    }


    public IEnumerator PopulateScene()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < asteroids_to_spawn; i++)
        {
            SpawnAsteroids();
        }
    }


    public void CheckAsteroidCount()
    {
        if (asteroids_count == 0)
        {
            StartCoroutine(PopulateScene());
        }

    }


}
