using System;



using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class PlayerStats : MonoBehaviour , ICollection<KeyValuePair<string,PlayerStat>>
{
    /// <summary>
    /// the player statistics
    /// </summary>
    Dictionary<string, PlayerStat> playerStats;

    /// <summary>
    /// get the player stat buy its key
    /// </summary>
    /// <param name="key">the key  the player stat was stored under</param>
    /// <returns>information about a player</returns>
    public PlayerStat this[string key] { get { return playerStats[key]; } }





    #region MonoBehaviour

    // Start is called before the first frame update
    
    void Start()
    {

        playerStats = new Dictionary<string, PlayerStat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion


    #region ICollection<KeyValuePair<string,PlayerStats>>

    public int Count => playerStats.Count;

    public bool IsReadOnly => false;

    /// <summary>
    /// adds a stat to the playaer stats
    /// </summary>
    /// <param name="item">the playerstat and its key</param>
    public void Add(KeyValuePair<string, PlayerStat> item)
    {
        if (!playerStats.ContainsKey(item.Key))
        {
            playerStats.Add(item.Key,item.Value);
        }
    }

    /// <summary>
    /// clears all stats
    /// </summary>
    public void Clear()
    {
        playerStats.Clear();
    }

    /// <summary>
    /// checks the list for a matching KeyValuePair
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(KeyValuePair<string, PlayerStat> item)
    {
        return playerStats.ContainsKey(item.Key) && playerStats.ContainsValue(item.Value);
    }

    public void CopyTo(KeyValuePair<string, PlayerStat>[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<KeyValuePair<string, PlayerStat>> GetEnumerator()
    {
        return playerStats.GetEnumerator();
    }

    public bool Remove(KeyValuePair<string, PlayerStat> item)
    {
        return playerStats.Remove(item.Key);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return playerStats.GetEnumerator();
    }
    #endregion
}



/// <summary>
/// holds a Players Statistic.
/// </summary>
[Serializable]
public class PlayerStat
{
    /// <summary>
    /// the name of the statistic
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// the description the statistic
    /// </summary>
    public string Description { get; }



    /// <summary>
    /// the maximum value of the Statistic
    /// </summary>
    int maxMagnitude = 0;
    /// <summary>
    /// gets or sets the maximum value or the strength of this statistic
    /// </summary>    
    public int MaxMagnitude { get { return maxMagnitude; }}


    
    /// <summary>
    /// gets the value or the strength of this statistic
    /// </summary>    
    public int Magnitude { get;private set;}

    /// <summary>
    /// fired when the magnitude changes
    /// </summary>
    public event EventHandler<MagnitudeChangedEventArgs> MagnitudeChanged;
    void OnMagnitudeChanged(MagnitudeChangedEventArgs e) { if (MagnitudeChanged != null) MagnitudeChanged(this, e); }


    /// <summary>
    /// creates a Playerstat
    /// </summary>
    /// <param name="name">the name of the stat</param>
    /// <param name="description">the description of the stat</param>
    /// <param name="magnitude">the magnitude of the stat</param>
    public PlayerStat(string name, string description, int magnitude , int maxmag)
    {
        Name = name;
        Description = description;
        Magnitude = magnitude;
        maxMagnitude = maxmag;
    }

    /// <summary>
    /// adds an amount to the magnitude upto the MaxMagnitude and not below the 0
    /// </summary>
    /// <remarks>int32 overflow will revert to 0 negative numbers not possible</remarks>
    /// <param name="amount">the amount to add to the magnitude</param>
    public void AddMagnitude(int amount)
    {
        int tempMag = Magnitude + amount;

        if (tempMag > maxMagnitude)
        {
            tempMag = maxMagnitude;
        }

        if (tempMag < 0)
        {
            tempMag = 0;
        }

        OnMagnitudeChanged(new MagnitudeChangedEventArgs(Magnitude,amount));

    }
    //overload + operator

}

/// <summary>
/// information about what changed
/// </summary>
public class MagnitudeChangedEventArgs : EventArgs
{
    /// <summary>
    /// the new magnitude
    /// </summary>
    public readonly int Magnitude;
    /// <summary>
    /// the amount the maginitude was changed
    /// </summary>
    public readonly int Change;

    /// <summary>
    /// creats a magnitude changed event args
    /// </summary>
    /// <param name="magnitude">the new magnitude</param>
    /// <param name="change">how much the magintude changed</param>
    public MagnitudeChangedEventArgs(int magnitude,int change)
    {
        Magnitude = magnitude;
        Change = change;
    }

}