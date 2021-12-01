using System.Collections.Generic;
using UnityEngine;

public class PrefabPool<T> where T : Component
{
    private readonly T _prefab;
    private readonly bool _sameParent;
    private readonly Stack<T> _pool;

    public int Count => _pool.Count;

    public PrefabPool(T prefab, bool sameParent = false)
    {
        _prefab = prefab;
        _sameParent = sameParent;
        _pool = new Stack<T>();
    }

    ~PrefabPool()
    {
        Clear();
    }

    public T Pop(bool activate = true, Transform parent = null)
    {
        return Pop(out var _, activate, parent);
    }

    public T Pop(out bool createdNew, bool activate = true, Transform parent = null)
    {
        createdNew = false;

        T obj = null;

        if (_pool.Count > 0)
        {
            do
            {
                obj = _pool.Pop();
                Debug.Assert(obj != null, $"Instance of {_prefab.name} was destoryed while in pool!");
            } while (obj == null && _pool.Count > 0);
        }
        
        if (obj == null)
        {
            obj = Object.Instantiate(_prefab, _sameParent ? _prefab.transform.parent : parent);
            createdNew = true;
        }
        else
        {
            obj.transform.SetParent(_sameParent ? _prefab.transform.parent : parent);
        }

        if (activate) obj.gameObject.SetActive(true);
        obj.gameObject.hideFlags = HideFlags.None;
        return obj;
    }

    public void Push(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.gameObject.hideFlags = HideFlags.HideInHierarchy;
        _pool.Push(obj);
    }

    public void PushAllAndClear(List<T> objList)
    {
        while (objList.Count > 0)
        {
            var o = objList[objList.Count - 1];
            Push(o);
            objList.RemoveAt(objList.Count - 1);
        }
    }

    public void Clear()
    {
        while (_pool.Count > 0)
            Object.Destroy(_pool.Pop().gameObject);
    }
}