using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem {
	public int InitialAmountToPool;
	public GameObject ObjectToPool;
	public bool ShouldExpand;
}

public class ObjectPooler : MonoBehaviour {

	public static ObjectPooler SharedInstance;
	
	[SerializeField]
	private List<ObjectPoolItem> itemsToPool;

	private List<GameObject> pooledObjects;


	void Awake() {
		SharedInstance = this;
	}

	void Start() {
		pooledObjects = new List<GameObject> ();
		foreach (ObjectPoolItem item in itemsToPool) {
			for (int index = 0; index < item.InitialAmountToPool; index++) {
				CreateAndAddToPooledObjects(objectToInstantiate: item.ObjectToPool);
			}
		}
		
	}

	public GameObject GetPooledObject(string tag) {
		for (int index = 0; index <= pooledObjects.Count; index++) {
			if (!pooledObjects[index].activeInHierarchy && pooledObjects[index].CompareTag(tag)) {
				return pooledObjects[index];
			}
		}

		foreach (ObjectPoolItem item in itemsToPool) {
			if (item.ObjectToPool.CompareTag(tag)) {
				if (item.ShouldExpand) {
					GameObject obj = CreateAndAddToPooledObjects(item.ObjectToPool);
					return obj;
				} 
			}
		}

		return null;
	}

	private GameObject CreateAndAddToPooledObjects(GameObject objectToInstantiate) {
		GameObject obj = Instantiate(objectToInstantiate);
		obj.SetActive(false);
		pooledObjects.Add(obj);
		return obj;
	}
}
