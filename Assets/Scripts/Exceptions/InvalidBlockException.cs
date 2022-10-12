using System;

public class InvalidBlockException : Exception
{
	public InvalidBlockException(string operation)
		: base(operation)
	{

	}
}