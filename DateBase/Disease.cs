namespace AN_UP.DateBase;

public class Disease
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int DurationLiness { get; set; }

    public Disease(int id, string name, int durationLiness)
    {
        Id = id;
        Name = name;
        DurationLiness = durationLiness;
    }
}