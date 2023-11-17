using System;

namespace AN_UP.DateBase;

public class Doctor
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Patronymic { get; set; }
    public DateTime EmploymentDate { get; set; }
    public int PositionID { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

    public Doctor(int id, string lastName, string firstName, string patronymic, DateTime employmentDate,
        int positionId, string login, string password)
    {
        Id = id;
        LastName = lastName;
        FirstName = firstName;
        Patronymic = patronymic;
        EmploymentDate = employmentDate;
        PositionID = positionId;
        Login = login;
        Password = password;
    }
}