namespace Gamestore.Domain.Entities.Users;

[Flags]
public enum Permissions
{
    None = 0,
    ManageUsers = 2,
    ManageRoles = 4,
    ManageBusinessEntities = 6,
    EditOrders = 12,
    ViewOrders = 32,
    ChangeOrderStatusToShipped = 64,
    ManageGameComments = 128,
    BanUsers = 256,
    Comment = 512,
}
