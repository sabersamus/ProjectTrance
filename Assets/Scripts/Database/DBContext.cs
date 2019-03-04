using System.Collections;
using System.Collections.Generic;

using System.Data;

public class DBContext
{
    private IDbConnection connection;

    public DBContext(IDbConnection dbConnection)
    {
        connection = dbConnection;
    }

    public void close()
    {
        if(connection != null && connection.State == ConnectionState.Open)
        {
            connection.Close();
        }
    }


    public PlayerDAO getPlayerDAO()
    {
        return new PlayerDAO(connection);
    }

    public ItemDAO getItemDAO()
    {
        return new ItemDAO(connection);
    }
}
