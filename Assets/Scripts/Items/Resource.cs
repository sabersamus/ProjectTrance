using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MaterialType;

public class Resource
{
    public MaterialType matType;
    public Sprite sprite;
    public int maxStackSize;

    public Resource(MaterialType _matType)
    {
        ItemSprites sprites = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemSprites>();
        matType = _matType;
        switch (_matType)
        {
            case WOOD:
                maxStackSize = 1000;
                break;
            case STONE:
                maxStackSize = 1000;
                break;
            case MEAT:
                maxStackSize = 100;
                sprite = sprites._MeatSprite;
                break;
                

        }
    }

}
