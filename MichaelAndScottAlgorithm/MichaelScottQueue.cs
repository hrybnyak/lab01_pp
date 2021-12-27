using System;
using MichaelAndScottAlgorithm.Atomic;

namespace MichaelAndScottAlgorithm
{
    public class MichaelScottQueue<T>
    {
        private static Node<T> dummy = new(default, new AtomicReference<Node<T>>(null));
        private AtomicReference<Node<T>> head = new(dummy);
        private AtomicReference<Node<T>> tail = new(dummy);

        public T Pop()
        {
            while (true)
            {
                Node<T> currentHead = head.Value;
                Node<T> currentTail = tail.Value;
                Node<T> nextHead = currentHead.next.Value;

                if (currentHead == currentTail)
                {
                    if (nextHead == null)
                    {
                        throw new ArgumentNullException();
                    }
                    else
                    {
                        tail.CompareAndSet(currentTail, currentTail.next.Value);
                    }
                }
                else
                {
                    if (head.CompareAndSet(currentHead, nextHead))
                    {
                        return nextHead.data;
                    }
                }
            }
        }

        public void Push(T data)
        {
            Node<T> newTail = new Node<T>(data, new AtomicReference<Node<T>>(null));

            while(true)
            {
                Node<T> currentTail = tail.Value;

                if (currentTail.next.CompareAndSet(null, newTail))
                {
                    tail.CompareAndSet(currentTail, newTail);
                    return;
                }
                else
                {
                    tail.CompareAndSet(currentTail, currentTail.next.Value);
                }
            }
        }

        public void Print() {
            Node<T> current = head.Value;

            while (current != null)
            {
                Console.WriteLine(current.data);;
                current = current.next.Value;
            }
        }
    }
}