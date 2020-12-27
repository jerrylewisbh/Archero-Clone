using System;
using System.Collections;
using System.Collections.Generic;

public class Pool<T> : IEnumerable where T : IResettable
{
    public List<T> items = new List<T>();
    public HashSet<T> unavailable = new HashSet<T>();
    private IFactory<T> factory;

    public Pool(IFactory<T> factory) : this(factory, 10)
    {
    }

    public Pool(IFactory<T> factory, int poolSize)
    {
        this.factory = factory;

        for (var i = 0; i < poolSize; i++)
        {
            Create();
        }
    }

    public T Allocate()
    {
        foreach (T t in items)
        {
            if (unavailable.Contains(t))
            {
                continue;
            }

            unavailable.Add(t);
            return t;
        }

        T newMember = Create();
        unavailable.Add(newMember);
        return newMember;
    }

    public void Release(T member)
    {
        member.Reset();
        unavailable.Remove(member);
    }

    private T Create()
    {
        T member = factory.Create();
        items.Add(member);
        return member;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return items.GetEnumerator();
    }
}