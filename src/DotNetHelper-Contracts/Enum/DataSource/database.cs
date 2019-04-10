namespace DotNetHelper_Contracts.Enum.DataSource
{
    public enum DataBaseType
    {
        SqlServer,
        MySql,
        Sqlite,
        Oracle,
        Oledb,
        Access95,
        Odbc,
    }

    public enum ScriptType
    {
        Parameterized,
        HumanReadable
    }

    public enum ActionType
    {
        Insert,
        Update,
        Upsert,
        Delete,
    }


    
}
