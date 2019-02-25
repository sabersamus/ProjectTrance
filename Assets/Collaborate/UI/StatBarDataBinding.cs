using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

using System;


/// <summary>
/// binds a statbar to a collection of playerstats
/// </summary>
public class StatBarDataBinding : MonoBehaviour
{
    /// <summary>
    /// the key of the satt you wish to bind
    /// </summary>
    public string BindingKey;

    /// <summary>
    /// the stats to pull data from
    /// </summary>
    public PlayerStats stats;

    /// <summary>
    /// the overall color of the UI
    /// </summary>
    public Color Tint;


    Text label,value;
    Slider bar;

    PlayerStat stat;



    // Start is called before the first frame update
    void Start()
    {
        Image background = GetComponent<Image>();
        Image emptybar = GetComponentsInChildren<Image>().Where(a => a.name == "Background").First();
        Image fillbar = GetComponentsInChildren<Image>().Where(a => a.name == "Fill").First();

        background.color = new Color(Tint.r,Tint.g,Tint.b,background.color.a);
        emptybar.color = new Color(Tint.r = 100, Tint.g - 100, Tint.b - 100, background.color.a);
        fillbar.color = new Color(Tint.r, Tint.g, Tint.b, background.color.a);


        label = GetComponentsInChildren<Text>().Where(a => a.name == "Label").First();        
        bar = GetComponentInChildren<Slider>();
        value = bar.GetComponentsInChildren<Text>().Where(a => a.name == "Value").First();

        stat = stats[BindingKey];

        label.text = stat.Name;
        value.text = $"{stat.Magnitude}/{stat.MaxMagnitude}";


        stat.MagnitudeChanged += Stat_MagnitudeChanged;
        

    }

    private void Stat_MagnitudeChanged(object sender, MagnitudeChangedEventArgs e)
    {
        

        if (stat.Magnitude > 0)
        {
            bar.value = stat.MaxMagnitude / stat.Magnitude;
            value.text = $"{stat.Magnitude}/{stat.MaxMagnitude}";
        }
        else
        {
            bar.value = 0;
            value.text = "0";
        }
    }

    // Update is called once per frame
    void Update()
    {
         
    }
}


