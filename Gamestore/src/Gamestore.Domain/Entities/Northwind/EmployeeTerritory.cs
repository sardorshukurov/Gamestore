using Gamestore.Domain.Common;

namespace Gamestore.Domain.Entities.Northwind;

public class EmployeeTerritory : BaseMongoEntity
{
    public int EmployeeTerritoryId { get; set; }

    public int EmployeeId { get; set; }
}