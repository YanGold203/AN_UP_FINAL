using System;

namespace AN_UP.DateBase;

public class Procedure
{
    public int Id { get; set; }
    public int DoctorID { get; set; }
    public int DiseaseRecordID { get; set; }
    public string Description { get; set; }
    public DateTime DateStart { get; set; }
    public int Duration { get; set; }
    public decimal Cost { get; set; }
    public int StatusID { get; set; }
    public string OutputStatusName { get; set; }
    public string OutputDoctorFIO { get; set; }

    public Procedure(int id, int doctorId, int diseaseRecordId,
        string description, DateTime dateStart, int duration, decimal cost, int statusId)
    {
        Id = id;
        DoctorID = doctorId;
        DiseaseRecordID = diseaseRecordId;
        Description = description;
        DateStart = dateStart;
        Duration = duration;
        Cost = cost;
        StatusID = statusId;
    }

    public Procedure(int id, int doctorId, int diseaseRecordId,
        string description, DateTime dateStart, int duration, decimal cost, int statusId,
        string outputStatusName, string outputDoctorFio)
    {
        Id = id;
        DoctorID = doctorId;
        DiseaseRecordID = diseaseRecordId;
        Description = description;
        DateStart = dateStart;
        Duration = duration;
        Cost = cost;
        StatusID = statusId;
        OutputDoctorFIO = outputDoctorFio;
        OutputStatusName = outputStatusName;
    }
}