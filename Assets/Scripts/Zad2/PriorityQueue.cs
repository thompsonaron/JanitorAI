using System;
using System.Collections;
using System.Collections.Generic;

public class PriorityQueue<T> where T : IComparable<T>
{
    List<T> data;
    public int Count { get { return data.Count; } }

    public PriorityQueue()
    {
        data = new List<T>();
    }

    // min heap sort
    public void Enqueue (T item)
    {
        // data is added to the end of the list
        data.Add(item);

        int childIndex = data.Count - 1;

        // binary heap sort
        while (childIndex > 0)
        {
            // calculates left child index (pos)
            int parentIndex = (childIndex - 1) / 2;

            // if child data is larger than parent data - stop
            if (data[childIndex].CompareTo(data[parentIndex]) >= 0)
            {
                break;
            }
            // otherwise swap
            T temp = data[childIndex];
            data[childIndex] = data[parentIndex];
            data[parentIndex] = temp;

            // child is now in parent pos - in next step it will check 
            //the SAME item in new position against that node's parent
            childIndex = parentIndex;
        }
    }

    public T Dequeue()
    {
        int lastIndex = data.Count - 1;
        T toppItem = data[0];

        data[0] = data[lastIndex];
        data.RemoveAt(lastIndex);

        int parentIndex = 0;
        ReorderQue(parentIndex);

        return toppItem;
    }

    public void ReorderQue(int parentIndex)
    {
        int lastIndex = data.Count - 1;
        int childIndex = parentIndex * 2 + 1;
        int rightChild = childIndex + 1;

        // check if child even exists
        if (childIndex > lastIndex)
        {
            return;
        }

        // if right child value is lesser than left one - go to that index
        if (rightChild <= lastIndex && data[rightChild].CompareTo(data[childIndex]) < 0)
        {
            childIndex = rightChild;
        }

        // if the child is equal or smaller then the parent - stop
        if (data[parentIndex].CompareTo(data[childIndex]) <= 0)
        {
            return;
        }

        // otherwise - swap
        T temp = data[parentIndex];
        data[parentIndex] = data[childIndex];
        data[childIndex] = temp;

        // swap index to value's new pos
        parentIndex = childIndex;

        ReorderQue(parentIndex);
    }

    public bool Contains(T item)
    {
        return data.Contains(item);
    }

    // return the data as a generic List
    public List<T> ToList()
    {
        return data;
    }
}