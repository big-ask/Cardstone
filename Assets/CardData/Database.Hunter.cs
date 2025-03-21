﻿public partial class Database
{
    static CardInfo Hunter_Secret()
    {
        CardInfo c = new();

        c.name = "Secret";
        c.text = "Hidden effect until triggered.";

        c.classType = Card.Class.Hunter;

        c.manaCost = 2;

        c.SPELL = true;
        c.SECRET = true;
        c.TOKEN = true;

        return c;
    }
    static CardInfo Hunters_Mark()
    {
        CardInfo c = new();

        c.name = "Hunter's Mark";
        c.text = "Set target minion's health to 1";

        c.classType = Card.Class.Hunter;

        c.manaCost = 0;

        c.SPELL = true;
        c.TARGETED = true;
        return c;
    }

    private static CardInfo Steamwheedle_Sniper()
    {
        CardInfo c = new();

        c.name = "Steamwheedle Sniper";
        c.text = "Your hero power can target minions.";

        c.manaCost = 2;
        c.damage = 2;
        c.health = 3;
        c.classType = Card.Class.Hunter;

        c.MINION = true;

        return c;
    }
}
