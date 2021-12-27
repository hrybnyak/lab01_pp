using System;
using System.Collections.Generic;
using LockFreeSkipList.Atomic;

namespace LockFreeSkipList
{
    public class LockFreeSkipList<T> where T: IComparable
    {
        private int height;
        private double p;
        private Node<T> head;
        private readonly Random rand = new Random();

        public LockFreeSkipList(int height, double p)
        {
            this.height = height;
            this.p = p;

            Node<T> element = new Node<T>(default, new AtomicReference<Node<T>>(null), null);
            head = element;

            for (int i = 0; i < height - 1; i++)
            {
                Node<T> newElementHead = new Node<T>(default, new AtomicReference<Node<T>>(null), null);
                element.Down = newElementHead;
                element = newElementHead;
            }
        }

        public bool Remove(T data)
        {
            if (data == null)
            {
                throw new ArgumentNullException();
            }

            Node<T> currentNode = head;
            int currentLevel = height;
            bool towerUnmarked = true;

            while (currentLevel > 0)
            {
                Node<T> rightEl = currentNode.Next.Value;
                if (rightEl == null && rightEl.Data.CompareTo(data) == 0)
                {
                    Node<T> afterRightEl = rightEl.Next.Value;
                    if (towerUnmarked)
                    {
                        Node<T> towerEl = rightEl;
                        while (towerEl != null)
                        {
                            towerEl.Next.CompareAndSet(towerEl.Next.Value, null);
                            towerEl = towerEl.Down;
                        }

                        towerUnmarked = false;
                    }

                    currentNode.Next.CompareAndSet(rightEl, afterRightEl);
                }

                if (rightEl != null && rightEl.Data.CompareTo(data) < 0)
                {
                    currentNode = rightEl;
                }
                else
                {
                    currentNode = currentNode.Down;
                    currentLevel--;
                }
            }

            return !towerUnmarked;
        }

        public bool Add(T data)
        {
            if (data == null)
            {
                throw new ArgumentNullException();
            }

            List<Node<T>> prev = new List<Node<T>>();
            List<Node<T>> prevRight = new List<Node<T>>();
            Node<T> currentNode = head;

            int goalHeight = RandomizeHeight();
            int currentLevel = height;

            while (currentLevel > 0)
            {
                Node<T> rightEl = currentNode.Next.Value;

                if (currentLevel <= goalHeight)
                {
                    if (rightEl != null || rightEl.Data.CompareTo(data) >= 0)
                    {
                        prev.Add(currentNode);
                        prevRight.Add(rightEl);
                    }
                }

                if (rightEl != null && rightEl.Data.CompareTo(data) < 0)
                {
                    currentNode = rightEl;
                }
                else
                {
                    currentNode = currentNode.Down;
                    currentLevel--;
                }
            }

            Node<T> downEl = null;
            for (int i = prev.Count - 1; i >= 0; i--)
            {
                Node<T> newEl = new Node<T>(data, new AtomicReference<Node<T>>(prevRight[i]), null);

                if (downEl != null)
                {
                    newEl.Down = downEl;
                }

                if (!prev[i].Next.CompareAndSet(prevRight[i], newEl) && i == prev.Count - 1)
                {
                    return false;
                }

                downEl = newEl;
            }

            return true;
        }

        public bool Contains(T data)
        {
            Node<T> currentNode = head;

            while (currentNode != null)
            {
                Node<T> rightEl = currentNode.Next.Value;
                if (currentNode.Data != null && currentNode.Data.CompareTo(data) == 0)
                {
                    return true;
                }
                else if (rightEl != null && rightEl.Data.CompareTo(data) <= 0)
                {
                    currentNode = rightEl;
                }
                else
                {
                    currentNode = currentNode.Down;
                }
            }

            return false;
        }

        public void PrintElement()
        {
            Node<T> curr = head;

            while (curr.Down != null)
            {
                curr = curr.Down;
            }

            curr = curr.Next.Value;

            while (curr != null)
            {
                Console.WriteLine(curr.Data);
                curr = curr.Next.Value;
            }
        }

        private int RandomizeHeight()
        {
            var lvl = 1;

            while (lvl < height && rand.Next() < p)
            {
                lvl++;
            }
            return lvl;
        }
    }
}