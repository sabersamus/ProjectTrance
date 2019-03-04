using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.SqliteClient;

public class PlayerDAO 
{
    private IDbConnection connection;

    public PlayerDAO(IDbConnection connection)
    {
        this.connection = connection;
    }

    public Player getPlayer(int id)
    {
        Player result = new Player();

        IDbCommand command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM player WHERE id = " + id;

        IDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            int dbID = reader.GetInt32(0);
            string name = reader.GetString(1);

            result.playerName = name;

            break;
        }

        return result;
    }

    public Player getPlayer(string name)
    {
        return null;
    }

    Player[] getPlayers(int[] ids)
    {
        return null;
    }

    Player[] getPlayers(string[] names)
    {
        return null;
    }

    Player createPlayer(Player player)
    {
        return null;
    }

    Player createPlayer(string name)
    {
        return null;
    }


}
