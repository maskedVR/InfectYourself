using System;
using BepInEx;
using Photon.Pun;
using GorillaLocomotion;
using UnityEngine;
using UnityEngine.InputSystem;
using Newtilla;

namespace GtagMod
{
	/// <summary>
	/// This is your mod's main class.
	/// </summary>

	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class Plugin : BaseUnityPlugin
	{
		bool inRoom;

		void OnEnable()
		{
			/* Set up your mod here */
			/* Code here runs at the start and whenever your mod is enabled*/

			HarmonyPatches.ApplyHarmonyPatches();
		}

		void OnDisable()
		{
			/* Undo mod setup here */
			/* This provides support for toggling mods with ComputerInterface, please implement it :) */
			/* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

			HarmonyPatches.RemoveHarmonyPatches();
		}

		void OnGameInitialized(object sender, EventArgs e)
		{
			/* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
		}

		void Update()
		{
			if(inRoom)
			{
				if(Keyboard.current.yKey.isPressed || ControllerInputPoller.instance.leftControllerSecondaryButton)
				{
					AddInfected(PhotonNetwork.LocalPlayer);
				}
			}
		}

		/* This attribute tells Utilla to call this method when a modded room is joined */
		public void OnModdedJoin(string gamemode)
		{
			inRoom = true;
		}

		/* This attribute tells Utilla to call this method when a modded room is left */
		public void OnModdedLeave(string gamemode)
		{
			inRoom = false;
		}
		public static void AddInfected(NetPlayer plr)
        {
            string gamemode = GorillaGameManager.instance.GameModeName().ToLower();
            if (gamemode.Contains("infection") || gamemode.Contains("tag"))
            {
                GorillaTagManager tagman = GameObject.Find("GT Systems/GameModeSystem/Gorilla Tag Manager").GetComponent<GorillaTagManager>(); // did not steal from a mod menu trust
                if (tagman.isCurrentlyTag)
                {
                    tagman.ChangeCurrentIt(plr);
                }
                else
                {
                    if (!tagman.currentInfected.Contains(plr))
                    {
                        tagman.AddInfectedPlayer(plr);
                    }
                }
	        }
		}
	}
}
