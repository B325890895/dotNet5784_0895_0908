namespace DO;

public record Engineer//this record save the engineer's information
(
    int Id,
    string Email,
    double Cost,
    string Name,
    EngineerExperience Level
/*
 Id int [pk]
Email string [not null, unique]
Cost double [not null, note: 'daily cost of the engineer, including salary, workplace, tools']
Name string [not null]
Level DO.EngineerExperience [not null]
 */
)
{
    public Engineer() : this(0, "", default, "", EngineerExperience.Beginner) { }//empty ctor (the parametrize ctoer was already define)
}

