using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pool<T> where T : Component
{
    private List<T> _internalObjectStack;
    private T _template;
    private Func<T, bool> _requestCondition;
    public Transform ParentCreationTransform { get; set; }
    public Pool(T objectTemplate, Transform parentTransform = null)
    {
        ParentCreationTransform = parentTransform;
        _template = objectTemplate;
    }
    public void Initialize(int size = 0)
    {
        if (size > 0)
        {
            _internalObjectStack = new List<T>(size);
            for (int i = 0; i < size; i++)
            {
                Create();
            }
        }
    }

    public T Request()
    {
        T available = null;
        for (int i = 0; i < _internalObjectStack.Count; i++)
        {
            if (_requestCondition.Invoke(_internalObjectStack[i]))
            {
                available = _internalObjectStack[i];
                break;
            }
        }

        if (!available)
        {
            available = Create();
        }

        return available;
    }

    public T Create()
    {
        T obj;
        if (ParentCreationTransform)
            obj = GameObject.Instantiate<T>(_template, ParentCreationTransform);
        else
            obj = GameObject.Instantiate<T>(_template, Vector3.zero, Quaternion.identity);
        
        _internalObjectStack.Add(obj);
        return obj;
    }

    /// <summary>
    /// Sets the condition for the objects in the pool to be in the "available" state
    /// </summary>
    /// <param name="condition">Func<T, bool> returns a bool using T</param>
    public void SetRequestCondition(Func<T, bool> condition)
    {
        _requestCondition = condition;
    }
}
