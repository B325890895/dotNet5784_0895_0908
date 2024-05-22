namespace DO;

public record Dependency//This record described the dependency between two tasks. The engineer will be able to begin the pending task only after the previous task is completed.
(
    int Id,
    int DependentTask,
    int DependsOnTask
)
{ 
    public Dependency() : this(default,default,default) { } //The empty ctor define (the parametrise ctor  auotomaticlly definede)
}
