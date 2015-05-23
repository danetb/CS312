using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace VisualIntelligentScissors
{
    class BinHeap <T> where T : IComparable, ITypeGetterSetter<double>
  //  class BinHeap <T> where T : IComparable
    {
      //  private int removed;
       private List<T> heap;
        private Dictionary<T,int> heap_indx;
        public bool isEmpty()
        {
            if (heap.Count == 0) return true;
            else return false;
        }
        public BinHeap() {
       //     removed = 0;
            heap = new List<T>();
            heap_indx = new Dictionary<T, int>();
        }
        public void insert(T value) //Should be O(log n)
        {
            heap.Add(value);
            heap_indx[value] = heap.Count - 1;
            bubbleUp(heap.Count-1);   
        }
        public T deletemin() //Should be O(log n)
        {
            T result = heap[0];
            heap[0] = heap[heap.Count-1];
            heap.RemoveAt(heap.Count-1);
            heap_indx.Remove(result);
            if (!isEmpty())
            {
                heap_indx[heap[0]] = 0;
                bubbleDown();
            }
            return result;
        }
        public void decreaseKey(T val, double newValue) //Should be O(log n)
        {                                      
            int indx = heap_indx[val];
            if (heap[indx].get() < newValue)
            {
                Console.WriteLine("Exception: " + heap[indx].get().ToString() + " " + newValue.ToString());
                throw new Exception();
            }       
            heap[indx].set(newValue);
            bubbleUp(indx);
        }
        private void bubbleUp(int indx)
        {
            while (indx > 0)
            {
                int z = (indx - 1) / 2;
                if (heap[z].CompareTo(heap[indx]) > 0) 
                    swap(indx, z);
                else break;
                indx = z;
            }
        }
        private void bubbleDown()
        {
            int indx = 0; //start at root.
            int g;
            while ((g = ((2 * indx) + 2)) < heap.Count)
            {
                if (heap[g - 1].CompareTo(heap[g]) < 0)
                {
                    if (heap[g - 1].CompareTo(heap[indx]) < 0)
                        swap(indx, indx = g - 1);
                    else break;
                }
                else if (heap[g].CompareTo(heap[indx]) < 0)
                    swap(indx, indx = g);
                else break;
            }
            if (g == heap.Count) //Case with only left child as leaf
            {
                if (heap[g - 1].CompareTo(heap[indx]) < 0)
                    swap(indx, g - 1);
            }
        }
        private void swap(int a, int b)
        {
            T tmp = heap[a];
            heap[a] = heap[b];
            heap[b] = tmp;
            //Switch indexes in dictionary...
          heap_indx[heap[a]] = a;
            heap_indx[heap[b]] = b;
        }
    }
}
