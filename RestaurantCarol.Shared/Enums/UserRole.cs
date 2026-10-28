namespace RestaurantCarol.Shared.Enums;

public enum UserRole
{
    Customer,
    Chef,
    DeliveryDriver,
    Manager
}

public static class UserRoleExtensions
{
    public static string ToDbString(this UserRole role) => role switch
    {
        UserRole.Customer => "Client",
        UserRole.Chef => "Bucatar",
        UserRole.DeliveryDriver => "Livrator",
        UserRole.Manager => "Manager",
        _ => "Client"
    };

    public static UserRole FromDbString(string dbString) => dbString?.Trim() switch
    {
        "Client" => UserRole.Customer,
        "Customer" => UserRole.Customer,
        "Bucatar" => UserRole.Chef,
        "Chef" => UserRole.Chef,
        "Livrator" => UserRole.DeliveryDriver,
        "DeliveryDriver" => UserRole.DeliveryDriver,
        "Manager" => UserRole.Manager,
        _ => UserRole.Customer
    };
}
