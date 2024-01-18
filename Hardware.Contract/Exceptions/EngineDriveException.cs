namespace Hardware.Contract.Exceptions;

public class EngineDriveException : Exception
{
    public EngineDriveException()
    {
    }

    public EngineDriveException(string message)
        : base(message)
    {
    }

    public EngineDriveException(string message, Exception inner)
        : base(message, inner)
    {
    }
}