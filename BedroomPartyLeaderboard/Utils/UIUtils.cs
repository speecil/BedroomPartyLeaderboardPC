﻿using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BedroomPartyLeaderboard.UI.Leaderboard;
using HMUI;
using IPA.Utilities;
using IPA.Utilities.Async;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Zenject;

namespace BedroomPartyLeaderboard.Utils
{
    internal class UIUtils
    {
        [Inject] private PanelView _panelView;
        [Inject] private LeaderboardView _leaderboardView;

        public class RainbowAnimation : MonoBehaviour
        {
            public float speed = 1f; // Speed of the color change

            private ClickableText clickableText;
            private float hue;

            private void Start()
            {
                clickableText = GetComponent<ClickableText>();
            }

            private void Update()
            {
                if (clickableText == null)
                {
                    return;
                }

                hue += speed * Time.deltaTime;
                if (hue > 1f)
                {
                    hue -= 1f;
                }

                Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);
                clickableText.color = rainbowColor;
            }
        }

        public void SetProfiles(List<LeaderboardData.LeaderboardEntry> leaderboard)
        {
            for (int i = 0; i < leaderboard.Count; i++)
            {
                _leaderboardView._ImageHolders[i].setProfileImage($"{leaderboard[i].userID}");
            }

            for (int i = leaderboard.Count; i <= 10; i++)
            {
                _leaderboardView._ImageHolders[i].profileloading.gameObject.SetActive(false);
            }
        }

        public void GetCoolMaterialAndApply()
        {
            Material mat = FindCoolMaterial();
            foreach(var x in _leaderboardView._ImageHolders)
            {
                x.profileImage.material = mat;
            }
            _panelView.playerAvatar.material = mat;
        }

        private Material FindCoolMaterial()
        {
            Material cool = null;
            foreach (Material material in Resources.FindObjectsOfTypeAll<Material>())
            {
                if (material == null) continue;
                if (material.name.Contains("UINoGlowRoundEdge"))
                {
                    cool = material;
                    break;
                }
            }

            if (cool == null)
            {
                Plugin.Log.Error("Material 'UINoGlowRoundEdge' not found.");
            }

            return cool;
        }


        public void RichMyText(LeaderboardTableView tableView)
        {
            foreach (LeaderboardTableCell cell in tableView.GetComponentsInChildren<LeaderboardTableCell>())
            {
                cell.showSeparator = true;
                var nameText = cell.GetField<TextMeshProUGUI, LeaderboardTableCell>("_playerNameText");
                var rankText = cell.GetField<TextMeshProUGUI, LeaderboardTableCell>("_rankText");
                var scoreText = cell.GetField<TextMeshProUGUI, LeaderboardTableCell>("_scoreText");
                nameText.richText = true;
                rankText.richText = true;
                scoreText.richText = true;
                rankText.text = $"<size=120%><u>{rankText.text}</u></size>";
                var seperator = cell.GetField<Image, LeaderboardTableCell>("_separatorImage") as ImageView;
                seperator.color = Constants.BP_COLOR2;
                seperator.color0 = Color.white;
                seperator.color1 = new Color(1, 1, 1, 0);
            }
        }
        public class ImageHolder
        {
            private int index;

            public bool isLoading;

            public ImageHolder(int index)
            {
                this.index = index;
            }

            [UIComponent("profileImage")]
            public ImageView profileImage;

            [UIObject("profileloading")]
            public GameObject profileloading;

            public void setProfileImage(string url)
            {
                isLoading = true;
                profileloading.SetActive(true);
                profileImage.SetImage(url);
                profileloading.SetActive(false);
                isLoading = false;
            }
        }

        internal class ButtonHolder
        {
            private int index;
            private Action<LeaderboardData.LeaderboardEntry> onClick;

            public ButtonHolder(int index, Action<LeaderboardData.LeaderboardEntry> endmylife)
            {
                this.index = index;
                onClick = endmylife;
            }

            [UIComponent("infoButton")]
            public Button infoButton;

            [UIAction("infoClick")]
            private void infoClick() => onClick?.Invoke(LeaderboardView.buttonEntryArray[index]);
        }
    }
}
