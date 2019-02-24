using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractContainerEventArgs : System.EventArgs
{

    public readonly Player player;
    public readonly Container container;
    public readonly EventType eventType;

    public PlayerInteractContainerEventArgs(Player _player, Container _container, EventType _eventType)
    {
        player = _player;
        container = _container;
        eventType = _eventType;
    }

    public enum EventType
    {
        OPEN,
        CLOSE
    }

}
