using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attack1 : Card {

    public override async void Play()
    {
        var options = Fight.Board.GetNeighbors(Player.Token.Tile.Coord);
        var tile = await Fight.Board.SelectTile(options);

        var damagee = Fight.Players.SingleOrDefault(x => x.Token.Tile == tile);
        if (damagee != null) damagee.Damage(1);

        Fight.TurnTimer.AddSecond(SecondType.Attack);
        Player.Initiative += 2;
        Discard();
    }

    public override async void Discard()
    {
        Fight.ActivePlayer.AttackDeck.DiscardPile.Insert(this);
    }

}
