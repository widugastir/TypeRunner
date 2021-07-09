using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
	SomeSpawner, spawns a prefab, for testing save/load
*/
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace MagicSave
{
    public class SomeSpawner : MonoBehaviour, ISaveLoad
    {
        public GameObject prefab;
        bool alreadySpawned;

        [System.Serializable]
        public class MySaveData
        {
            public bool alreadySpawned;
        }


        object ISaveLoad.GetSaveData()
        {
            MySaveData mySaveData = new MySaveData();
            mySaveData.alreadySpawned = alreadySpawned;

            return mySaveData;
        }

        void ISaveLoad.Load(object o)
        {
            if (o == null) return;
            MySaveData mySaveData = o as MySaveData;

            if (mySaveData != null)
            {
                alreadySpawned = mySaveData.alreadySpawned;
            }
        }

        IEnumerator CoSpawn()
        {
            yield return null; // wait one frame in case game is loaded (Start() for scene objs is called before Load())
            if (alreadySpawned) yield break;
            Spawn();
            alreadySpawned = true;
        }


        public void Spawn()
        {
            // called from UI, spawn manually
            GameObject go = Instantiate(prefab); //
            go.name = "Spawned" + prefab.name;
            go.transform.position = transform.position;


            Vector3 f = (Vector3.up * 1f + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f))) * 2f;
            Rigidbody rb = go.GetComponent<Rigidbody>();
            rb.AddForce(f, ForceMode.VelocityChange);

        }


        void Start()
        {
            StartCoroutine(CoSpawn());
        }
    }
}