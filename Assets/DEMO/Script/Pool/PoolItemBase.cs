using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolItem {
    void OnSpawn();
    void Recycle();
}

// 用来实现对象池对象的生命周期
public abstract class PoolItemBase : MonoBehaviour, IPoolItem {
    private void OnEnable() {
        OnSpawn();
    }

    private void OnDisable() {
        Recycle();
    }

    public virtual void OnSpawn() {
    }

    public virtual void Recycle() {
    }
}