using System;

namespace AN_UP.DateBase;

public class Patient
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Patronymic { get; set; }
    public DateTime DateBirth { get; set; }
    public string PhoneNumber { get; set; }

    public Patient(int id, string lastName, string firstName, string patronymic, DateTime dateBirth,
        string phoneNumber)
    {
        Id = id;
        LastName = lastName;
        FirstName = firstName;
        Patronymic = patronymic;
        DateBirth = dateBirth;
        PhoneNumber = phoneNumber;
    }
}