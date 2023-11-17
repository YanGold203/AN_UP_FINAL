using System;

namespace AN_UP.DateBase;

public class Receipt
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public int DiseaseRecordID { get; set; }
    public DateTime DatePayment { get; set; }

    public Receipt(int id, decimal amount, int diseaseRecordId, DateTime datePayment)
    {
        Id = id;
        Amount = amount;
        DiseaseRecordID = diseaseRecordId;
        DatePayment = datePayment;
    }
}