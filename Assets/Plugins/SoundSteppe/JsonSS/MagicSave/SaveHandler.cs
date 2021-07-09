using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
	SaveHandler, handles Save/Load.
    1.) Use 'Tools/MagicSaveTools' to add this script to any prefab in need of save/load.
        Prefabs must be under 'Resources' or in any sub folder.
    2.) Implement interface ISaveLoad for every type of Obj in need of save and add it to the prefabs root (where SaveHandler script is located).
        If your Obj doesn't need to save state, but wants to be created at load, just don't implement the interface.
    3.) Call static functions SaveHandler.SaveScene() and SaveHandler.LoadScene() when appropriate.

    Can handle all kinds of objects. All prefabs will be created on scene root.

    Calling order when loading will be:
    1.) SceneManager.LoadSceneAsync();
    2.) Awake() and Start() of all scene Objects
    3.) SaveHandler.Load()
    4.) Awake() of Objs created during Load();
    5.) OnEnable() of Objs created during Load();
    6.) Start() of Objs created during Load();

    Class is partial in case you want to extent it to save e.g. Player data in a separate file.
*/
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace MagicSave
{
    public interface ISaveLoad
    {
        object GetSaveData();
        void Load(object o);

    }


    public partial class SaveHandler : MonoBehaviour
    {
        static List<SaveHandler> allSaveHandler = new List<SaveHandler>();

        [ReadOnly]
        public string prefabPath;  // must be under 'Resources'


        [System.Serializable]
        public class SaveData
        {
            public object[] staticObjSaveDatas; // save data for all objs exported with Unity scene, don't delete or add during runtime
            public string[] prefabPathes;       // prefab paths for objs to spawn on Load()
            public object[] objSaveDatas;       // save data for all objs spawned on Load()
        }


        public static void SaveScene(string filePathAndName, ISaveLoad[] staticObjs= null)
        {
            // call on level save or level change
            if (string.IsNullOrEmpty(filePathAndName)) return;

            SaveData sd = CreateSaveData(staticObjs == null? new ISaveLoad[0]: staticObjs);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(filePathAndName); // Application.persistentDataPath + "/savedGames.gd");
            bf.Serialize(file, sd);
            file.Close();
        }

        public static void LoadScene(string filePathAndName, ISaveLoad[] staticObjs = null)
        {
            // call after level load
            if (string.IsNullOrEmpty(filePathAndName)) return;
            allSaveHandler.Clear(); // Remove all stuff from our List

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenRead(filePathAndName);
            if (file == null) return;

            SaveData sd = bf.Deserialize(file) as SaveData;
            file.Close();
            if (sd == null) return;

            // spawn stuff and call their load function:
            for (int i=0; i< sd.prefabPathes.Length; i++)
            {
                GameObject go = CreateFromPrefab(sd.prefabPathes[i]);
                if (go == null) continue;

                ISaveLoad isl = go.GetComponent<ISaveLoad>();
                if (isl != null)
                {
                    isl.Load(sd.objSaveDatas[i]);
                }
            }

            // call load of static Objs:
            if (staticObjs != null)
            {
                int length = Math.Min(staticObjs.Length, sd.staticObjSaveDatas.Length);
                for (int i=0; i< length; i++)
                {
                	if (staticObjs[i] != null)
                    {
                        staticObjs[i].Load(sd.staticObjSaveDatas[i]);
                    }
                }
            }
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*
            Helper
        */
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        static SaveData CreateSaveData(ISaveLoad[] staticObjs)
        {
            // create save data from list
            SaveData sd = new SaveData();

            sd.staticObjSaveDatas = new object[staticObjs.Length];
            sd.prefabPathes = new string[allSaveHandler.Count];
            sd.objSaveDatas = new object[allSaveHandler.Count];

            // create dynamic save datas:
            for (int i = 0; i < allSaveHandler.Count; i++)
            {
                sd.prefabPathes[i] = "";
                sd.objSaveDatas[i] = null;

                SaveHandler sh = allSaveHandler[i];
                if (sh != null)
                {
                    sd.prefabPathes[i] = sh.prefabPath;

                    ISaveLoad isl = sh.GetComponent<ISaveLoad>();
                    if (isl != null)
                    {
                        sd.objSaveDatas[i] = isl.GetSaveData();
                    }
                }
            }

            // create static save datas:
            for (int i=0; i< staticObjs.Length; i++)
            {
                if (staticObjs[i] != null)
                {
                    sd.staticObjSaveDatas[i] = staticObjs[i].GetSaveData();
                }
            }

            return sd;
        }

        static GameObject CreateFromPrefab(string path)
        {
            //
            GameObject prefab= Resources.Load<GameObject>(path);
            if (prefab == null)
            {
                Debug.Log("Cannot find: '" + path + "' in Resources");
                return null;
            }

            GameObject go=  GameObject.Instantiate(prefab); // parent to scene root
            go.name = prefab.name;
            return go;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /*
            Unity
        */
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        void OnEnable()
        {
            allSaveHandler.Add(this);
        }

        void OnDisable()
        {
            allSaveHandler.Remove(this);
        }


    }



}//ns
