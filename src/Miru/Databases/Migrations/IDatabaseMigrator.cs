namespace Miru.Databases.Migrations;

public interface IDatabaseMigrator
{
    void UpdateSchema();
        
    void DowngradeSchema(int steps = 1);
        
    void RecreateSchema();
}