using System;
using System.Collections.Generic;
using DiscordRPC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ContentWarningRichPresence;

public class GameRichPresence : MonoBehaviour
{

    public DiscordRpcClient client;
    public RoomStatsHolder stats;

    private Dictionary<string, string> imageKeys = new()
    {
        {"NewMainMenu","menu"},
        {"SurfaceScene","surface"},
        {"FactoryScene","depths_factory"},
        {"HarbourScene","depths_harbour"}
    };

    private Dictionary<string, string> locationNames = new()
    {
        {"NewMainMenu","Main Menu"},
        {"SurfaceScene","Home"},
        {"FactoryScene","Factory"},
        {"HarbourScene","Harbour"}
    };

    private void Start()
    {
        client = new DiscordRpcClient("1225123743353933844");

        Debug.Log("Discord application should be loaded!");
        client.Initialize();
        client.SetPresence(new DiscordRPC.RichPresence()
        {
            Details = "Content Warning",
            State = "Main Menu",
            Assets = new Assets()
            {
                LargeImageKey = "image_large",
                LargeImageText = "boom",
                SmallImageKey = "image_small"
            }
        });
    }

    private void Update()
    {
        stats = SurfaceNetworkHandler.RoomStats;

        UpdateRP();

    }

    private void UpdateRP()
    {

        string currentScene = SceneManager.GetActiveScene().name;

        if (stats != null)
        {

            int views1 = BigNumbers.GetScoreToViews((float)SurfaceNetworkHandler.RoomStats.CurrentQuota, GameAPI.CurrentDay);
            int views2 = BigNumbers.GetScoreToViews((float)SurfaceNetworkHandler.RoomStats.QuotaToReach, GameAPI.CurrentDay);
            client.SetPresence(new DiscordRPC.RichPresence()
                {
                    Details = "Views: " + BigNumbers.ViewsToString(views1) + "/" + BigNumbers.ViewsToString(views2) + " | Money: " + stats.Money,
                    State = locationNames[currentScene],
                    Assets = new Assets()
                    {
                        LargeImageKey = imageKeys[currentScene],
                        LargeImageText = "Day: " + stats.CurrentDay,
                        SmallImageKey = "image_small"
                    }
                });
            
        } else
        {
            client.SetPresence(new DiscordRPC.RichPresence()
            {
                Details = "",
                State = "Main Menu",
                Assets = new Assets()
                {
                    LargeImageKey = imageKeys[currentScene],
                    LargeImageText = "Idling",
                    SmallImageKey = "image_small"
                }
            });
        }

    }

}