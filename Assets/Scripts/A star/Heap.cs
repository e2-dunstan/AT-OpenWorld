using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[] heapItems;
    int currentItemCount;

    public Heap(int size)
    {
        heapItems = new T[size];
    }

    public void Add(T heapItem)
    {
        heapItem.HeapIndex = currentItemCount;
        heapItems[currentItemCount] = heapItem;
        SortUp(heapItem);
        currentItemCount++;
    }

    public T RemoveFirstItem()
    {
        T firstItem = heapItems[0];
        currentItemCount--;
        heapItems[0] = heapItems[currentItemCount];
        heapItems[0].HeapIndex = 0;
        SortDown(heapItems[0]);
        return firstItem;
    }

    private void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = heapItems[parentIndex];
            //Higher priority therefore lower f cost
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < currentItemCount)
                {
                    //Check priority
                    if (heapItems[childIndexLeft].CompareTo(heapItems[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if (item.CompareTo(heapItems[swapIndex]) < 0)
                {
                    Swap(item, heapItems[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(heapItems[item.HeapIndex], item);
    }

    private void Swap(T _a, T _b)
    {
        heapItems[_a.HeapIndex] = _b;
        heapItems[_b.HeapIndex] = _a;
        int itemAIndex = _a.HeapIndex;
        _a.HeapIndex = _b.HeapIndex;
        _b.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}