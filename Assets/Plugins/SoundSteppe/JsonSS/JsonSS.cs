﻿using System.Security.Cryptography;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using SimpleJSON;

namespace SoundSteppe.JsonSS
{
	public static class JsonSS
	{
		private static readonly string crypt_password = "laKfr4l5o45z9kj5";
		
		private static readonly byte[] crypt_iv = 
		{ 0x11, 0x3, 0x2, 0x3, 0x16, 
			0x15, 0x6, 0x15, 0x14, 0x12, 
			0x14, 0x15, 0x7, 0x15, 0x4, 0x8 
		};
		
		#region Public methods
		public static void SaveGameObject(string ID, MonoBehaviour obj)
		{
			string json = "{}";
			
			json = GetObjectJson(obj);
			string path = Application.persistentDataPath + "/" + ID + ".data";
			Encrypt(path, json);
		} 
		
		public static void LoadGameObject(string ID, MonoBehaviour mono)
		{
			string json = LoadObjectJson(ID);
			
			if(string.IsNullOrEmpty(json) == false)
			{
				var saveable = mono.gameObject.GetComponents<MonoBehaviour>();
				foreach(var s in saveable)
				{
					string componentJson = ExtractJsonTo(s, json);
					PassValuesToFields(s, componentJson);
				}
				foreach(var s in saveable)
				{
					if(s.TryGetComponent(out ISaveable isave))
						isave.OnLoad();
				}
			}
		}
		
		public static void LoadGameObjects(string ID, MonoBehaviour[] array)
		{
			var json = LoadArray(ID);
			for(int i = 0; i < array.Length; i++)
			{
				LoadGameObject(array[i], json[i]);
			}
		}
		
		public static void LoadGameObject(MonoBehaviour mono, string json)
		{
			if(string.IsNullOrEmpty(json) == false)
			{
				var saveable = mono.gameObject.GetComponents<MonoBehaviour>();
				foreach(var s in saveable)
				{
					string componentJson = ExtractJsonTo(s, json);
					PassValuesToFields(s, componentJson);
				}
				foreach(var s in saveable)
				{
					if(s.TryGetComponent(out ISaveable isave))
						isave.OnLoad();
				}
			}
		}
		
		public static void SaveGameObjects(string ID, MonoBehaviour[] array)
		{
			string json = "";
			
			foreach(var e in array)
			{
				json += "*{" + GetObjectJson(e) + "}*";
			}
			
			string path = Application.persistentDataPath + "/" + ID + ".data";
			Encrypt(path, json);
		}
		
		public static List<string> LoadArray(string ID)
		{
			string path = Application.persistentDataPath + "/" + ID + ".data";
			string json = "";
			
			json = Decrypt(path);
				
			List<string> elements = new List<string>();
			
			string element = "";
			while(json.Length > 0)
			{
				int start = json.IndexOf("*{");
				int end = json.IndexOf("}*", start);
				if(start < 0 || end < 0)
					break;
				element = json.Substring(start, end - start + 2);
				json = json.Remove(start, end - start + 2);
				element = element.Replace("*{", "{");
				element = element.Replace("}*", "}");
				elements.Add(element);
			}
			return elements;
		} 
		#endregion
		
		#region Private methods
		private static string LoadObjectJson(string ID)
		{
			string path = Application.persistentDataPath + "/" + ID + ".data";
			string json = "";
			
			json = Decrypt(path);
				
			return json;
		}
		
		private static string GetObjectJson(MonoBehaviour mono)
		{
			string json = "";
			var saveable = mono.GetComponents<ISaveable>();
			foreach(var s in saveable)
			{
				s.OnSave();
				
			}
			var ss = mono.GetComponents<MonoBehaviour>();
			foreach(var s in ss)
			{
				json += s.GetType() + ":|" + GetFieldsJson(s) + "|";
			}
			return json;
		}
		
		private static string GetFieldsJson(MonoBehaviour s)
		{
			string json = "{}";
			JSONNode node = JSON.Parse(json);
			
			FieldInfo[] fields = s.GetType().GetFields();
			foreach(FieldInfo field in fields)
			{
				if(field.GetCustomAttribute<Saveable>() != null)
				{
					//if(field.FieldType.IsArray)
					if(field.GetValue(s) is IList)
					{
						var jsonArray = node[field.Name].AsArray;
						IEnumerable array = field.GetValue(s) as IEnumerable;
						
						if(array != null)
						{
							foreach(var obj in array)
							{
								System.Type t = obj.GetType();
								if(t.IsValueType == true && t.IsPrimitive == false)
								{
									jsonArray.Add(JsonUtility.ToJson(obj));
								}
								else
								{
									JSONNode jn = JSONNode.Parse(obj.ToString());
									if(jn != null)
										jsonArray.Add(jn);
								}
							}
						}
					}
					else if(field.FieldType == typeof(Vector3))
					{
						Vector3 v = (Vector3)field.GetValue(s);
						node[field.Name] = v;
					}
					else if(field.FieldType == typeof(System.DateTime))
					{
						System.DateTime date = (System.DateTime)field.GetValue(s);
						node[field.Name] = date.ToBinary().ToString();
						//Debug.Log(node[field.Name] + "   " + date.ToBinary().ToString());
					}
					else if(field.FieldType.IsValueType == true && field.FieldType.IsPrimitive == false)
					{
						string j = JsonUtility.ToJson(field.GetValue(s));
						node[field.Name] = j;
					}
					else
					{
						node[field.Name] = field.GetValue(s).ToString();
					}
				}
			}
			return node.ToString();
		}
		
		private static void PassValuesToFields(MonoBehaviour s, string json)
		{
			JSONNode node = JSON.Parse(json);
			
			FieldInfo[] fields = s.GetType().GetFields();
			foreach(FieldInfo field in fields)
			{
				if(field.GetCustomAttribute<Saveable>() != null)
				{
					//if(field.FieldType.IsArray)
					if(field.GetValue(s) is IList)
					{
						//Debug.Log("ARR " + field.FieldType);
						SetArrayFieldValue(s, node, field);
					}
					else
					{
						//Debug.Log("OBJ " + field.FieldType);
						SetFieldValue(s, node, field);
					}
				}
			}
		}
		
		private static string ExtractJsonTo(MonoBehaviour s, string json)
		{
			string compJson = "";
			
			string compName = s.GetType() + ":|{";
			int start = json.IndexOf(compName);
			if(start < 0)
				return "";
			int end = json.IndexOf("}|", start);
			if(end < 0)
				return "";
			compJson = json.Substring(start + compName.Length - 1, end - start - compName.Length + 2);
			return compJson;
		}
		
		private static void SetArrayFieldValue(MonoBehaviour s, JSONNode node, FieldInfo field)
		{
			System.Type elementType;
			var valueObj = node[field.Name].AsStringList;
			
			if(field.FieldType.IsGenericType)
				elementType = field.FieldType.GetGenericArguments()[0];
			else
				elementType = field.FieldType.GetElementType();
						
			List<object> listObj = new List<object>();
			if(elementType.IsValueType == true && elementType.IsPrimitive == false)
			{
				for(int i = 0; i < valueObj.Count; i++)
				{
					listObj.Add(valueObj[i]);
				}
			}
			else
			{
				for(int i = 0; i < valueObj.Count; i++)
				{
					object obj = System.Convert.ChangeType(valueObj[i], elementType);
					listObj.Add(obj);
				}
			}
			
			if(elementType == typeof(System.String))
			{
				if(field.FieldType.IsGenericType)
					field.SetValue(s, listObj.Cast<System.String>().ToList());
				else
					field.SetValue(s, listObj.Cast<System.String>().ToArray());
			}
			else if(elementType == typeof(System.Int32))
			{
				if(field.FieldType.IsGenericType)
					field.SetValue(s, listObj.Cast<System.Int32>().ToList());
				else
					field.SetValue(s, listObj.Cast<System.Int32>().ToArray());
			}
			else if(elementType == typeof(System.Boolean))
			{
				if(field.FieldType.IsGenericType)
					field.SetValue(s, listObj.Cast<System.Boolean>().ToList());
				else
					field.SetValue(s, listObj.Cast<System.Boolean>().ToArray());
			}
			else if(elementType == typeof(System.Single))
			{
				if(field.FieldType.IsGenericType)
					field.SetValue(s, listObj.Cast<System.Single>().ToList());
				else
					field.SetValue(s, listObj.Cast<System.Single>().ToArray());
			}
			else if(elementType == typeof(System.Double))
			{
				if(field.FieldType.IsGenericType)
					field.SetValue(s, listObj.Cast<System.Double>().ToList());
				else
					field.SetValue(s, listObj.Cast<System.Double>().ToArray());
			}
			else if(elementType == typeof(System.Byte))
			{
				if(field.FieldType.IsGenericType)
					field.SetValue(s, listObj.Cast<System.Byte>().ToList());
				else
					field.SetValue(s, listObj.Cast<System.Byte>().ToArray());
			}
			else if(elementType == typeof(System.Int64))
			{
				if(field.FieldType.IsGenericType)
					field.SetValue(s, listObj.Cast<System.Int64>().ToList());
				else
					field.SetValue(s, listObj.Cast<System.Int64>().ToArray());
			}
			else if(elementType.IsValueType == true && elementType.IsPrimitive == false)
			{
				System.Array arr = System.Array.CreateInstance(elementType, listObj.Count);
				for(int i = 0; i < listObj.Count; i++)
				{
					arr.SetValue(JsonUtility.FromJson((string)listObj[i], elementType), i);
				}
				
				if(field.FieldType.IsGenericType)
				{
					System.Type generic = typeof(List<>).MakeGenericType(elementType);
					IList lst = (IList)System.Activator.CreateInstance(generic);
					foreach(var e in arr)
					{
						lst.Add(e);
					}
					field.SetValue(s, lst);
				}
				else
					field.SetValue(s, arr);
			}
			else
			{
				Debug.LogWarning("Unsupported array type: " + elementType);
			}
		}
		
		private static void SetFieldValue(MonoBehaviour s, JSONNode node, FieldInfo field)
		{
			string valueJson = node[field.Name].Value;
			object valueObj = null;
			try
			{
				if(field.FieldType == typeof(Vector3))
				{
					valueObj = node[field.Name].ReadVector3();
				}
				else if(field.FieldType == typeof(string))
				{
					valueObj = node[field.Name].Value;
				}
				else if(field.FieldType == typeof(System.DateTime))
				{
					string j = node[field.Name].Value;
					long b = long.Parse(j);
					System.DateTime date = System.DateTime.FromBinary(b);
					valueObj = date;
				}
				else if(field.FieldType.IsValueType == true && field.FieldType.IsPrimitive == false)
				{
					string json = node[field.Name].Value;
					valueObj = JsonUtility.FromJson(json, field.FieldType);
				}
				else
				{
					valueObj = System.Convert.ChangeType(valueJson, field.FieldType);
				}
			}
				catch(System.Exception e)
				{
					valueObj = null;
					Debug.LogWarning(e.Message);
				}
			if(valueObj != null)
				field.SetValue(s, valueObj);
		}
		
		private static void Encrypt(string path, string json)
		{
			using(Aes iAes = Aes.Create())
			{
				iAes.IV = crypt_iv;
				
				iAes.Key = ConvertToKeyBytes(iAes, crypt_password);
				var textBytes = Encoding.UTF8.GetBytes(json);
				var aesEncryptor = iAes.CreateEncryptor();
				var encryptedBytes = aesEncryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
				
				File.WriteAllText(path, System.Convert.ToBase64String(encryptedBytes));
			}
		}
		
		private static string Decrypt(string path)
		{
			if (File.Exists(path))
			{
				using (Aes aes = Aes.Create())
				{
					string json = File.ReadAllText(path);
					var ivBytes = crypt_iv;
					
					byte[] encryptedTextBytes = null;
					try
					{
						encryptedTextBytes = System.Convert.FromBase64String(json);
					}
					catch
					{
						Debug.LogError("Datafile is corrupted!");
						return "";
					}
					var decryptor = aes.CreateDecryptor(ConvertToKeyBytes(aes, crypt_password), ivBytes);
					
					byte[] decryptedBytes = null;
					try
					{
						decryptedBytes = decryptor.TransformFinalBlock(encryptedTextBytes, 0, encryptedTextBytes.Length);
					}
					catch
					{
						Debug.LogError("Datafile is corrupted!");
						return "";
					}
					
					if(decryptedBytes != null)
						json = Encoding.UTF8.GetString(decryptedBytes);
					return json;
				}
			}
			return "";
		}
		
		private static byte[] ConvertToKeyBytes(SymmetricAlgorithm algorithm, string password)
		{
			algorithm.GenerateKey();

			var keyBytes = Encoding.UTF8.GetBytes(password);
			var validKeySize = algorithm.Key.Length;

			if (keyBytes.Length != validKeySize)
			{
				var newKeyBytes = new byte[validKeySize];
				System.Array.Copy(keyBytes, newKeyBytes, Mathf.Min(keyBytes.Length, newKeyBytes.Length));
				keyBytes = newKeyBytes;
			}

			return keyBytes;
		}
		#endregion
	}
}