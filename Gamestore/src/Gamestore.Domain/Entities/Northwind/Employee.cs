using Gamestore.Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gamestore.Domain.Entities.Northwind;

public class Employee : BaseMongoEntity
{
    public int EmployeeId { get; set; }

    public string Address { get; set; }

    public DateTime BirthDate { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    public int Extension { get; set; }

    public string FirstName { get; set; }

    public DateTime HireDate { get; set; }

    public string HomePhone { get; set; }

    public string LastName { get; set; }

    public string Notes { get; set; }

    [BsonRepresentation(BsonType.Binary)]
    public byte[] Photo { get; set; }

    public string PhotoPath { get; set; }

    public int PostalCode { get; set; }

    public string Region { get; set; }

    public int ReportsTo { get; set; }

    public string Title { get; set; }

    public string TitleOfCourtesy { get; set; }
}