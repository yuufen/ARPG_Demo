using System;
using System.Collections;
using System.Collections.Generic;
using DEMO.Tool;
using DEMO.Tool.Singleton;
using UnityEngine;

// TODO 用官方的 ObjectPool 更好
public class ObjectPoolManager : Singleton<ObjectPoolManager> {
    // 对象池的对象种类
    [Serializable] private class PoolItem {
        public string ItemName;
        public GameObject Item; // 生命周期每个对象自己实现
        public int InitMaxCound;
    }
    // 池中对象种类列表
    [SerializeField] private List<PoolItem> _poolItemList = new List<PoolItem>();

    // 维护对象池中的所有对象，每个 ItemName 对应一个 Queue
    private Dictionary<string, Queue<GameObject>> _poolCenter = new Dictionary<string, Queue<GameObject>>();
    
    // 用来收集实例
    private GameObject _itemParent;

    private void Start() {
        _itemParent = new GameObject("PoolObjectParent");
        _itemParent.transform.SetParent(this.transform);

        InitPool();
    }

    // 初始化对象池
    private void InitPool() {
        if (_poolItemList.Count == 0) return;

        // 遍历对象类别
        foreach (var poolItem in _poolItemList) {
            for (var j = 0; j < poolItem.InitMaxCound; j++) {
                // 创建对象
                var item = Instantiate(poolItem.Item, _itemParent.transform, true);
                item.SetActive(false);

                // 如果字典中没有改 key，就创建 Queue
                if (!_poolCenter.ContainsKey(poolItem.ItemName)) {
                    _poolCenter.Add(poolItem.ItemName, new Queue<GameObject>());
                }

                // 添加对象到 Queue
                _poolCenter[poolItem.ItemName].Enqueue(item);
            }
        }
    }

    public void TryGetPoolItem(string name, Vector3 position, Quaternion rotation) {
        if (!_poolCenter.ContainsKey(name)) {
            DevelopmentToos.WTF("申请的对象池不存在" + name);
        }

        var item = _poolCenter[name].Dequeue();
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.SetActive(true); // 生命周期每个对象自己实现

        // 出列后入列，放到队尾，这样对象池没有空闲对象后，也能拿到最老的对象
        // todo 拿到非空闲对象是不是要重置一下
        _poolCenter[name].Enqueue(item);
    }

    public GameObject TryGetPoolItem(string name) {
        if (!_poolCenter.ContainsKey(name)) {
            DevelopmentToos.WTF("申请的对象池不存在" + name);
            return null;
        }

        var item = _poolCenter[name].Dequeue();
        item.SetActive(true);

        // 出列后入列，放到队尾，这样对象池没有空闲对象后，也能拿到最老的对象
        // todo 拿到非空闲对象是不是要重置一下
        _poolCenter[name].Enqueue(item);

        return item;
    }
}