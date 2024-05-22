namespace DO;
[Serializable]
public class DalDoesNotExistException : Exception//An exception that catches all exceptions that are thrown when a requested object does not exist
{
    public DalDoesNotExistException(string? message) : base(message) { }
}
[Serializable]
public class DalExistException : Exception//An exception that catches the exceptions received when you want to add an existing object
{
    public DalExistException(string? message) : base(message) { }
}

[Serializable]
public class DalDeletionIsProhibitedException : Exception//An exception that is thrown when you want to perform an unauthorized deletion
{
    public DalDeletionIsProhibitedException(string? message) : base(message) { }
}
[Serializable]
public class DalOutOfRangeChoiceException : Exception//An exception that is thrown when an option is selected that is not among the available options
{
    public DalOutOfRangeChoiceException(string? message) : base(message) { }
}
[Serializable]
public class DalDataWasNotReceived : Exception//An exception thrown when no content has been typed where it is required
{
    public DalDataWasNotReceived(string? message) : base(message) { }
}
[Serializable]
public class DalXMLFileLoadCreateException : Exception//An exception thrown when no content has been typed where it is required
{
    public DalXMLFileLoadCreateException(string? message) : base(message) { }
}
[Serializable]
public class DalCircularDependenciesAreNotAllowedException :Exception
{
    public DalCircularDependenciesAreNotAllowedException(string? message) : base(message) { }
}
[Serializable]
public class DalInvalidInputException : Exception
{
    public DalInvalidInputException(string? message) : base(message) { }
}


