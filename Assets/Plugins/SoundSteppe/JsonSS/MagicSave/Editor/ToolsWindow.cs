using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
	ToolsWindow, helpful tools for batch processing on Prefabs and stuff, open from Menu: Tools/MagicSaveTools
*/
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace MagicSave
{

    public class ToolsWindow : EditorWindow
    {
        static bool foldout1;
        static bool foldout2;
        static bool foldout3;

        string curPath; // e.g. 'Assets/Resources' or 'Assets/Resources/Enemies'



        [MenuItem("Tools/MagicSaveTools", false, 0)]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<ToolsWindow>(true, "MagicSave- Tools", true);
        }

        void OnEnable()
        {
             curPath = EditorPrefs.GetString("MagicSave.ToolsWindow.curPath");
        }

        void OnDisable()
        {
            EditorPrefs.SetString("MagicSave.ToolsWindow.curPath", curPath);
        }


        void OnGUI()
        {
            //
            UpdateSaveHandlerOnPrefabs();
            RemoveAllSaveHandlerInScene();
            ReparentAllInScene();
        }




        void UpdateSaveHandlerOnPrefabs()
        {
            foldout1 = EditorGUILayout.Foldout(foldout1, "Update SaveHandler On Prefabs");
            if (!foldout1) return;

            EditorGUILayout.HelpBox(
@"Set a Folder and press Button 'Update' to add (or update) a SaveHandler script to all Prefabs found in folder or sub folder.
This will save the prefab path in the script and will (at runtime) mark the spawned GameObj (from prefab) to be saved and loaded. See documentation for details.", MessageType.Info);


            // Choose Folder
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = false;
            EditorGUILayout.LabelField("Path", GUILayout.Width(50));
            EditorGUILayout.TextField("", curPath);
            GUI.enabled = true;

            if (GUILayout.Button("Change..."))
            {
                string newPath = EditorUtility.OpenFolderPanel("Set Path", curPath, "");
                if (!PathIsValid(newPath))
                {
                    EditorUtility.DisplayDialog("Error", "Folder must be 'Resources' or child of a 'Resources' Folder", "Got it");
                }
                else
                {
                    curPath = newPath.Substring(newPath.IndexOf("Assets"));
                }
            }
            EditorGUILayout.EndHorizontal();

            // Work
            GUI.enabled = PathIsValid(curPath);
            if (GUILayout.Button("Update", GUILayout.Height(30)))
            {
                //TestCreatePrefabs();
                UpdatePathes();
            }
            GUI.enabled = true;
        }



        void RemoveAllSaveHandlerInScene()
        {
            foldout2 = EditorGUILayout.Foldout(foldout2, "Remove All SaveHandler In Scene");
            if (!foldout2) return;

            EditorGUILayout.HelpBox(
@"Press Button 'Remove' to remove all SaveHandler scripts from any GameObj in scene.
This is necessary if you use the same savable prefabs in scene (by adding them to scene in Unity Editor) that you use for spawned GameObjs.
If you don't remove the scripts in scene, GamoObjs will duplicate with every save.", MessageType.Info);

            // Work
            if (GUILayout.Button("Remove", GUILayout.Height(30)))
            {
                SaveHandler[] allSHInScene= GameObject.FindObjectsOfType<SaveHandler>();
                for (int i=0; i< allSHInScene.Length; i++)
                {
                    GameObject.DestroyImmediate(allSHInScene[i]);
                }

                EditorUtility.DisplayDialog("Info", ""+ allSHInScene.Length+ " scripts removed.", "Got it");
            }
        }



        void TestCreatePrefabs()
        {
            GameObject cube = GameObject.Find("Cube");
            if (cube == null) return;

            for (int i=0; i< 100; i++)
            {
                PrefabUtility.CreatePrefab("Assets/MagicSave/Example/Resources/SomeEnemies/" + cube.name + i+".prefab", cube);
            }
        }

        void UpdatePathes()
        {
            string[] guids;
            string[] pathes = { curPath };
            guids = AssetDatabase.FindAssets("t:prefab", pathes);

            if(guids.Length == 0)
            {
                EditorUtility.DisplayDialog("Warning", "No prefabs found in: " +curPath+ "\n Doing nothing.", "Got it");
                return;
            }

            List<string> couldNotLoad = new List<string>();

            int prefabsChanged = 0;
            for (int i = 0; i < guids.Length; i++)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (string.IsNullOrEmpty(prefabPath)) continue;
                //Debug.Log(prefabPath);

                // res path:
                string p = prefabPath.Substring(prefabPath.IndexOf("Resources/") + "Resources/".Length);
                p = p.Substring(0, p.Length - ".prefab".Length);
                //Debug.Log(p);

                // load asset and save it's path:
                GameObject curPrefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;
                if (curPrefab == null)
                {
                    couldNotLoad.Add(prefabPath);
                    continue;
                }

                SaveHandler ppScript = curPrefab.GetComponent<SaveHandler>();
                if (ppScript == null) ppScript = curPrefab.AddComponent<SaveHandler>();
                if (ppScript.prefabPath != p)
                {
                    ppScript.prefabPath = p;
                    EditorUtility.SetDirty(ppScript);
                    prefabsChanged++;
                }

                EditorUtility.DisplayProgressBar("Working...", "Processing Prefabs", i/ (float) guids.Length);
            }

            EditorUtility.DisplayProgressBar("Working...", "Processing Prefabs", 1f);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.ClearProgressBar();

            string info = "" + prefabsChanged + " prefabs updated";
            if (couldNotLoad.Count > 0)
            {
                info += "\nCould not load:";
                for (int i = 0; i < couldNotLoad.Count; i++)
                {
                    info += "\n\t"+couldNotLoad[i];
                }
            }

            EditorUtility.DisplayDialog("Info", info, "Got it");
        }


        bool PathIsValid(string newPath)
        {
            if (!Directory.Exists(newPath)) return false;
            int assetIndex = newPath.IndexOf("Assets");
            int resIndex = newPath.IndexOf("Resources");
            if (resIndex < 0) return false;
            if (assetIndex < 0) return false;

            return true;
        }



        bool IsPrefab(GameObject go)
        {
            if (go == null) return false;
            if (PrefabUtility.GetPrefabType(go) == PrefabType.None) return false;
            if (PrefabUtility.GetPrefabType(go) == PrefabType.Prefab) return true;
            if (PrefabUtility.GetPrefabType(go) == PrefabType.ModelPrefab) return true;
            if (PrefabUtility.GetPrefabType(go) == PrefabType.MissingPrefabInstance) return true;

            return false;
        }


        static List<Type> types= new List<Type>();
        static List<string> typeNames = new List<string>();
        static int typesChoiceIndex = 0;
        static GameObject newParent;

        void ReparentAllInScene()
        {
            foldout3 = EditorGUILayout.Foldout(foldout3, "Re-Parent In Scene");
            if (!foldout3) return;

            EditorGUILayout.HelpBox(
@"Use this to bulk re-parent all specific GameObjs in scene of a given type to a new parent.
E.g re-parent all GameObjs in scene having a script 'Door' on them to scene node 'Doors'. Script must implement ISaveLoad.

1.) Press Button 'Get Types' to get all object types implementing ISaveLoad currently in scene.
2.) Choose the type of object you want to have re-parented.
3.) Drag the new parent (from scene).
4.) Press Button 'Re-Parent All'.", MessageType.Info);


            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Get Types", GUILayout.Width(100)))
            {
                var tArray= GameObject.FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoad>().ToArray();

                for (int i=0; i< tArray.Length; i++)
                {
                    Type t = tArray[i].GetType();
                    if (!types.Contains(t))
                    {
                        types.Add(t);
                        typeNames.Add(t.ToString());
                    }
                }
            }

            if (types.Count > 0)
            {
                //EditorStyles.popup.fontSize = 11;     // changes global value
                //EditorStyles.popup.fixedHeight = 19;  // changes global value
                typesChoiceIndex = EditorGUILayout.Popup(typesChoiceIndex, typeNames.ToArray());
            }
            EditorGUILayout.EndHorizontal();


            if (types.Count == 0) return;

            EditorGUILayout.BeginHorizontal();
            newParent = EditorGUILayout.ObjectField("New Parent", newParent, typeof(GameObject), true) as GameObject;
            if (newParent && !IsPrefab(newParent))
            {
                if (GUILayout.Button("Re-Parent All", GUILayout.Height(30)))
                {
                    MonoBehaviour[] thisTypeArray = GameObject.FindObjectsOfType(types[typesChoiceIndex]) as MonoBehaviour[];
                    int count = 0;
                    for (int i = 0; i < thisTypeArray.Length; i++)
                    {
                        Transform t = thisTypeArray[i].gameObject.transform;
                        if (t.parent != newParent)
                        {
                            t.SetParent(newParent.transform);
                            count++;
                        }
                    }

                    EditorUtility.DisplayDialog("Info", "" + count + " GameObjs re-parented.", "Got it");
                }
            }
            EditorGUILayout.EndHorizontal();

        }




    }






}// ns
