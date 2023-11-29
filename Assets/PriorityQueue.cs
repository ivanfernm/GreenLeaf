using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

    public int Count
    {
        get { return elements.Count; }
    }

    public bool IsEmpty
    {
        get { return elements.Count == 0; }
    }

    public void Enqueue(T item, float priority)
    {
        elements.Add(new KeyValuePair<T, float>(item, priority));
        elements.Sort((x, y) => x.Value.CompareTo(y.Value));
    }

    public T Dequeue()
    {
        var item = elements[0].Key;
        elements.RemoveAt(0);
        return item;
    }

    public bool Contains(T item)
    {
        return elements.Find(element => element.Key.Equals(item)).Key != null;
    }

    public void Update(T item, float priority)
    {
        var found = elements.Find(element => element.Key.Equals(item));
        if (found.Key != null)
        {
            elements.Remove(found);
            Enqueue(item, priority);
        }
    }
}
