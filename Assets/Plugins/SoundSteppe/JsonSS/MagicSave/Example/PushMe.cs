using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
	PushMe, for creating and pushing cubes around. For testing saved position.
*/
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace MagicSave
{
    public class PushMe : MonoBehaviour
    {
        public float amount = 10f;
        public float torqueAmount = 100f;

        int curIndex;

        void Inc()
        {
            curIndex++;
        }

        void Force()
        {
            // because PushMe is not destroyed at LoadScene() this cannot be cashed
            SomeEnemy[] allCubes = GameObject.FindObjectsOfType<SomeEnemy>();
            if (allCubes.Length == 0) return;

            GameObject cube = allCubes[curIndex % allCubes.Length].gameObject;
            Rigidbody rb = cube.GetComponent<Rigidbody>();
            if (rb == null) return;

            Vector3 f = (Vector3.up * 1f + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f))) * amount;
            rb.AddForce(f, ForceMode.VelocityChange);

            Vector3 t = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * torqueAmount;
            rb.AddTorque(t);
        }

        void AddCube()
        {
            // because PushMe is not destroyed at LoadScene() this cannot be cashed
            SomeSpawner spawner = GameObject.FindObjectOfType<SomeSpawner>();
            if (spawner == null) return;

            spawner.Spawn();
        }


        void Start()
        {
            transform.Find("SaveLoadTest/Next").GetComponent<Button>().onClick.AddListener(Inc);
            transform.Find("SaveLoadTest/Force").GetComponent<Button>().onClick.AddListener(Force);
            transform.Find("SaveLoadTest/Add").GetComponent<Button>().onClick.AddListener(AddCube);
        }


    }
}