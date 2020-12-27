using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory<T> : IFactory<T> where T : new() {
    public T Create() {
        return new T();
    }
}