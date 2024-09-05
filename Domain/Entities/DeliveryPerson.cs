using System;

namespace BikeRentalApp.Domain.Entities;

public class DeliveryPerson
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Cnpj { get; set; }
    public DateTime BirthDate { get; set; }
    public string CnhNumber { get; set; }
    public CNHType CnhType { get; set; }
    public string CnhImageFileName { get; set; }
}

public enum CNHType
{
    A,
    B,
    AB
}