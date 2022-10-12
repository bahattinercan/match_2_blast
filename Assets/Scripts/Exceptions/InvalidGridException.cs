using System;

public class InvalidGridException : Exception
{
    public InvalidGridException(string operation)
        : base(operation)
    {
    }
}