using LockFreeSkipList.Atomic;

namespace LockFreeSkipList
{
    public class Node<T>
    {
        public T Data;
        public AtomicReference<Node<T>> Next;
        public Node<T> Down;

        public Node(T data, AtomicReference<Node<T>> next, Node<T> down)
        {
            this.Data = data;
            this.Next = next;
            this.Down = down;
        }
    }
}