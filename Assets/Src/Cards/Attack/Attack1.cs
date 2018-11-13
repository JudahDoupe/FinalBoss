using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attack1 : Card {

    public override async void Play()
    {
        var options = Board.GetNeighbors(Player.Token.Coord);
        var tile = await Board.SelectTile(options);

        var damagee = Fight.Players.SingleOrDefault(x => x.Token.Coord == tile.Coord);
        if (damagee != null) damagee.Damage(1);

        Player.TurnTimer.AddSecond(SecondType.Attack);
        Player.Initiative += 2;
        Discard();
    }

    public override async void Discard()
    {
        Player.AttackDeck.DiscardPile.Insert(this);
    }

}
