using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class CardExecutor : MonoBehaviour{

    public static CardExecution GetCard(string name)
    {
        switch (name)
        {
            case "Dodge":
                return new Dodge();
            case "Walk":
                return new Walk();
            case "Run":
                return new Run();
            case "Punch":
                return new Punch();
            case "Bomb":
                return new Bomb();
            case "Build Wall":
                return new BuildWall();
            default:
                return null;
        }
    }

    public static void PlayCard(NetworkConnection connectionToClient, string cardName)
    {
        var card = GetCard(cardName);
        var player = Fight.Players.First(x => x.connectionToClient == connectionToClient);
        PlayCard(player, card);
    }
    public static async void PlayCard(Player player, CardExecution card)
    {
        if (!card.isPlayable(player))
        {
            player.TargetFinishPlayingCard(player.connectionToClient, false);

        }
        else
        {
            var wasPlayed = await (card?.Play(player) ?? new Task<bool>(() => false));
            if (wasPlayed)
            {
                player.RpcAddInitiative(card.Initiative);
                foreach (var cardAction in card.Actions)
                {
                    Fight.UseActions(cardAction);
                }
            }

            player.TargetFinishPlayingCard(player.connectionToClient, wasPlayed);
        }
    }
}