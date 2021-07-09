using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
	SomeEnemy, sample to show ISaveLoad implementation.
    Shows how to handle different versions of a save by checking for object type in Load()
*/
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace MagicSave
{
    public class SomeEnemy : MonoBehaviour, ISaveLoad
    {

        public float health = 100;

        [System.Serializable]
        public class MySaveData // old save format
        {
            public SaveHelper.TransformSerializer transform;    // save Transform
        }


        [System.Serializable]
        public class MySaveData02 // new and shiny save format
        {
            public SaveHelper.TransformSerializer transform;    // save Transform
            public float health;
        }

        /*
        object ISaveLoad.GetSaveData() // old save
        {
        // construct save data
        MySaveData mySaveData = new MySaveData();
        mySaveData.transform.From(transform);

        return mySaveData;
        }
        */

        object ISaveLoad.GetSaveData() // current save format
        {
            // construct save data, will be called when saving
            MySaveData02 mySaveData = new MySaveData02();
            mySaveData.transform.From(transform);
            mySaveData.health = health;

            return mySaveData;
        }

        void ISaveLoad.Load(object o)
        {
            // handle old and new version
            if (o == null) return;

            MySaveData mySaveData = o as MySaveData;
            MySaveData02 mySaveData02 = o as MySaveData02;

            if (mySaveData != null) // its an old save
            {
                mySaveData.transform.To(transform);
                health = 99; // some default
            }
            else if (mySaveData02 != null)  // current save format
            {
                mySaveData02.transform.To(transform);
                health = mySaveData02.health;
            }
            else // unknown save format
            {
                Debug.Log("Heeeeelp!!");
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*
            Unity
        */
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        void Awake()
        {
        }


        void Start()
        {
        }





    }
}