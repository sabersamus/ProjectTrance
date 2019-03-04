using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class DatabaseManager : MonoBehaviour
{
    private static DBContext dBContext;

    // Start is called before the first frame update
    void Start()
    {
        //we will create our connection here
        IDbConnection dbConnection;
        string connectionString = "URI=file:" + Application.dataPath + "/local.db";
        dbConnection = (IDbConnection)new SqliteConnection(connectionString);
        dbConnection.Open();

        dBContext = new DBContext(dbConnection);
    }


    public static DBContext getDBContext()
    {
        return dBContext;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            List<ToolItem> toolItems = getDBContext().getItemDAO().getToolItems();
            foreach(ToolItem tool in toolItems)
            {
                Debug.Log(tool.itemName + ": " + tool.toolType);
            }
        }
    }
}
