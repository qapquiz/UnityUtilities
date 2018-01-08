// From https://forum.unity3d.com/threads/how-to-load-an-array-with-jsonutility.375735/
// Author: ffleurey

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class JsonHelper : MonoBehaviour {
	[System.Serializable]
	public struct Wrapper<T> {
		public T[] array;
	}
	
	public static T[] FromJsonArray<T>(string json) {
		string newJson = new StringBuilder().Append("{\"array\": ").Append(json).Append("}").ToString();
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (newJson);
		return wrapper.array;
	}
}
