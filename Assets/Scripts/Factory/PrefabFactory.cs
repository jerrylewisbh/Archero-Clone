using UnityEngine;

public class PrefabFactory<T> : IFactory<T> where T : MonoBehaviour {

    private GameObject prefab;
    private string name;
    private int index = 0;

    public PrefabFactory(GameObject prefab) : this(prefab, prefab.name) { }

    public PrefabFactory(GameObject prefab, string name) {
        this.prefab = prefab;
        this.name = name;
    }

    public T Create() {
        GameObject tempGameObject = Object.Instantiate(prefab);
        tempGameObject.name = name + index;
        var objectOfType = tempGameObject.GetComponent<T>();
        index++;
        return objectOfType;
    }
}