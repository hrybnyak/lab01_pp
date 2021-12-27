using System;
using LockFreeSkipList.Atomic;

namespace HarrisAlgorithm
{
    public class HarrisOrderedList<T> where T : IComparable
    {
        private Node<T> head = new(default, new AtomicReference<Node<T>>(null));

        public bool Remove(T data)
        {
            if (data == null)
            {
                throw new ArgumentNullException();
            }

            Node<T> prevEl = head;
            while (prevEl.next.Value != null)
            {
                Node<T> currEl = prevEl.next.Value;
                Node<T> nextEl = currEl.next.Value;

                if (currEl.data.CompareTo(data) == 0)
                {
                    if (currEl.next.CompareAndSet(nextEl, null) && prevEl.next.CompareAndSet(currEl, nextEl))
                    {
                        return true;
                    }
                }
                else
                {
                    prevEl = currEl;
                }
            }

            return false;
        }

        public void Add(T data)
        {
            if (data == null)
            {
                throw new ArgumentNullException();
            }

            Node<T> newEl = new Node<T>(data, new AtomicReference<Node<T>>(null));
            Node<T> currentEl = head;

            while (true)
            {
                Node<T> nextEl = currentEl.next.Value;

                if (nextEl != null)
                {
                    if (nextEl.data.CompareTo(data) >= 0)
                    {
                        newEl.next = new AtomicReference<Node<T>>(nextEl);
                        if (currentEl.next.CompareAndSet(nextEl, newEl))
                        {
                            return;
                        }
                    }
                    else
                    {
                        currentEl = nextEl;
                    }
                }
                else if (currentEl.next.CompareAndSet(null, newEl))
                {
                    return;
                }
            }
        }

        public bool Contains(T data)
        {
            Node<T> currentEl = head.next.Value;

            while (currentEl != null)
            {
                if (currentEl.data.CompareTo(data) == 0)
                {
                    return true;
                }

                currentEl = currentEl.next.Value;
            }

            return false;
        }

        public void NonSafePrint()
        {
            Node<T> current = head.next.Value;
            while (current != null)
            {
                Console.WriteLine(current.data);
                current = current.next.Value;
            }
        }
    }
}