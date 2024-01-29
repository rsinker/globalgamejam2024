using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralConstants
{
    public const int MAX_MAP_HORIZ = 6;
    public const int MAX_MAP_VERT = 6;

    public const int MIN_ROOMS_BEFORE_SHOP = 5;
    public const float BEGIINING_CHANCE_OF_SHOP = 0.15f;
    public const float CHANCE_OF_SHOP_INCREMENT = 0.1f;
    public const int FAKE_SHOP_SIZE = 0;

    public const int MIN_ROOMS_BEFORE_BOSS = 12;
    public const float BEGINING_CHANCE_OF_BOSS = 0.15f;
    public const float CHANCE_OF_BOSS_INCREMENT = 0.05f;
    public const int FAKE_BOSS_SIZE = 1;

    public const int FAKE_HUB_SIZE = 2;
}

public class GameConstants
{
    public const float PLAYER_SPAWN_OFFSET = 2.5f;

    public const float SPAWN_DELAY = 4.0f;
}