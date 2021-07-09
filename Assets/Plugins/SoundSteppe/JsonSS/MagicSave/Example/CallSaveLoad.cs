using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
	CallSaveLoad, an example on how to use SaveHandler.LoadScene()/SaveScene()
*/
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace MagicSave
{
    public class CallSaveLoad : MonoBehaviour
    {
        static CallSaveLoad me; // Singleton. Here to ensure Canvas is not recreated after Load()
        string saveFilename = "MySavegame.bin";


        public void SaveScene()
        {
            //Debug.Log(Application.persistentDataPath);
            ISaveLoad[] allIsl = GameObject.Find("Level/SaveableSceneObjs").GetComponentsInChildren<ISaveLoad>(true);
            SaveHandler.SaveScene(Application.persistentDataPath + "/" + saveFilename, allIsl);
        }

        public void LoadScene()
        {
            StartCoroutine(CoLoadScene());
        }


        IEnumerator CoLoadScene()
        {
            // The Application loads the Scene in the background as the current Scene runs.
            // This is particularly good for creating loading screens.
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SampleSaveLoad");

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            ISaveLoad[] allIsl = GameObject.Find("Level/SaveableSceneObjs").GetComponentsInChildren<ISaveLoad>(true);
            SaveHandler.LoadScene(Application.persistentDataPath + "/" + saveFilename, allIsl);
        }


        void Awake()
        {
            if (me == null)
            {
                me = this;
                DontDestroyOnLoad(gameObject); // make sure this gameObj is not destroyed by LoadSceneAsync() and thus SaveHandler.Load() is called.
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            transform.Find("SaveLoadTest/SaveButton").GetComponent<Button>().onClick.AddListener(SaveScene);
            transform.Find("SaveLoadTest/LoadButton").GetComponent<Button>().onClick.AddListener(LoadScene);
        }
    }
}