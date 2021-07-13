using UnityEngine;

         public class ConsoleToGUI : MonoBehaviour
         {
     //#if !UNITY_EDITOR
             static string myLog = "";
             private string output;
	         private string stack;
	         [SerializeField] private bool _showInEditor = false;
	         [SerializeField] private bool _showStackTrace = true;
	         GUIStyle style = new GUIStyle();
     
             void OnEnable()
             {
                 Application.logMessageReceived += Log;
             }
     
             void OnDisable()
             {
                 Application.logMessageReceived -= Log;
             }
     
             public void Log(string logString, string stackTrace, LogType type)
             {
                 output = logString;
                 stack = stackTrace;
	             if (stack.Length > 1000)
	             {
		             stack = stack.Substring(0, 999);
	             }
	             
	             if(_showStackTrace)
	            	myLog = output + ":\n\t" + stack + "\n" + myLog;
	             else
		             myLog = output + "\n" + myLog;
	            	
                 if (myLog.Length > 5000)
                 {
                     myLog = myLog.Substring(0, 4000);
                 }
             }
     
             void OnGUI()
             {
	             if (_showInEditor == false && Application.isEditor)
		             return;
	             style.normal.textColor = Color.red;
	             style.fontSize = Screen.height * 2 / 100;
	             
	             myLog = GUI.TextArea(new Rect(10, Screen.height * (9f/10f), Screen.width - 10, Screen.height), myLog, style);
             }
     //#endif
     }