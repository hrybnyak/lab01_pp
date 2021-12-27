using LockFreeSkipList.Atomic;

namespace HarrisAlgorithm
{
    public class Node<T>
    {
        public T data;
        public AtomicReference<Node<T>> next;

        public Node(T data, AtomicReference<Node<T>> next)
        {
            this.data = data;
            this.next = next;
        }
    }
}