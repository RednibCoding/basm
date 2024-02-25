namespace Basm;

public class Stack
{
    private class Node
    {
        public Node? Prev { get; set; }
        public int Value { get; set; }

        public Node(int value)
        {
            Value = value;
        }
    }

    private Node? top;

    // Push to stack
    public void Push(int n)
    {
        var newNode = new Node(n)
        {
            Prev = top
        };
        top = newNode;
    }

    // Pop from stack
    public int Pop()
    {
        if (top == null)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        int value = top.Value;
        top = top.Prev;
        return value;
    }

    // Peek at the top element
    public int Peek()
    {
        if (top == null)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        return top.Value;
    }

    // Check if the stack is empty
    public bool IsEmpty()
    {
        return top == null;
    }
}
