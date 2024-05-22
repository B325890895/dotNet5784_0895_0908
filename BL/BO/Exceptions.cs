namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception//An exception that catches all exceptions that are thrown when a requested object does not exist
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlExistException : Exception//An exception that catches the exceptions received when you want to add an existing object
{
    public BlExistException(string? message) : base(message) { }
    public BlExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

[Serializable]
public class BlDeletionIsProhibitedException : Exception//An exception that is thrown when you want to perform an unauthorized deletion
{
    public BlDeletionIsProhibitedException(string? message) : base(message) { }
    public BlDeletionIsProhibitedException(string message, Exception innerException)
                : base(message, innerException) { }
}


[Serializable]
public class BlDataWasNotReceived : Exception//An exception thrown when no content has been typed where it is required
{
    public BlDataWasNotReceived(string? message) : base(message) { }
    public BlDataWasNotReceived(string message, Exception innerException)
                : base(message, innerException) { }
}
[Serializable]
public class BlXMLFileLoadCreateException : Exception//An exception thrown when no content has been typed where it is required
{
    public BlXMLFileLoadCreateException(string? message) : base(message) { }
    public BlXMLFileLoadCreateException(string message, Exception innerException)
                : base(message, innerException) { }
}
[Serializable]
public class BlOutOfRangeChoiceException : Exception//An exception that is thrown when an option is selected that is not among the available options
{
    public BlOutOfRangeChoiceException(string? message) : base(message) { }
}
[Serializable]
public class BlDataIsInvalidException: Exception
{
    public BlDataIsInvalidException(string? message) : base(message) { }
}
[Serializable]
public class BlCircularDependenciesAreNotAllowedException : Exception
{
    public BlCircularDependenciesAreNotAllowedException(string? message) : base(message) { }
    public BlCircularDependenciesAreNotAllowedException(string message, Exception innerException)
                : base(message, innerException) { }
}
[Serializable]
public class BlTheProjectIsUnscheduledException : Exception
{
    public BlTheProjectIsUnscheduledException(string? message) : base(message) { }
    
}
[Serializable]
public class BlTheTasksAreUnscheduledException : Exception
{
    public BlTheTasksAreUnscheduledException(string? message) : base(message) { }

}
[Serializable]
public class BlCanNotUseTheEntityException : Exception
{
    public BlCanNotUseTheEntityException(string? message) : base(message) { }

}

[Serializable]
public class BlScheduledException : Exception
{
    public BlScheduledException(string? message) : base(message) { }

}
