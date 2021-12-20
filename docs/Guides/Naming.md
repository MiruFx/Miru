# Naming

Naming stuff is **hard**. Everyone has an opinion and sometimes is hard a team to agree.

Here you can find a **Naming Guide** for your Miru's project.

[[toc]]

## Database

| Subject     | Strategy              | Examples                                                             |
|-------------|-----------------------|----------------------------------------------------------------------|
| Table       | PascalCase + Plural   | `Products`, `Orders`, `Customers`, `OrderItems`, `CustomerAddresses` | 
| Column      | PascalCase + Singular | `Id`, `Name`, `Price`, `Status`                                      |
| Primary Key | `Id`                  | `Id`                                                                 |
| Foreign Key | `OtherTable`+`Id`     | `ProductId`, `OtherProductId`, `OrderId`, `CustomerId`               |

### Migration

All migrations has the timestamp prefix `YYYYMMDDHHmmSS`. Example: `202112161830_CreateProducts`

| Subject      | Strategy                        | Examples                                              |
|--------------|---------------------------------|-------------------------------------------------------|
| Create Table | Create+`Table`                  | `CreateProducts`, `CreateOrders`, `CreateCustomers`   | 
| Alter Table  | Alter+`Table`+`Action`+`Column` | `AlterProductsAddQuantity`, `AlterCustomerDropStatus` |
| Drop Table   | Drop+`Table`                    | `DropProducts`, `DropCustomers`                       |
| Add Column   | Alter+`Table`+Add+`Column`      | `AlterProductsAddQuantity`                            |
| Alter Column | Alter+`Table`+Alter+`Column`    | `AlterProductsAlterQuantity`                          |
| Drop Column  | Alter+`Table`+Drop+`Column`     | `AlterProductsDropQuantity`                           |

## Domain

| Subject            | Strategy              | Examples                           |
|--------------------|-----------------------|------------------------------------|
| Entity             | PascalCase + Singular | `Product`, `Order`, `Customer`     | 
| Property           | PascalCase + Singular | `Id`, `Name`, `Price`, `Status`    |
| CollectionProperty | PascalCase + Plural   | `Products`, `Orders`, `Customers`  |
