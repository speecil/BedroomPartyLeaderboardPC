﻿using QSLeaderboard.UI.Leaderboard;
using QSLeaderboard.Utils;
using Zenject;

namespace QSLeaderboard.Installers
{
    internal class MenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LeaderboardView>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesAndSelfTo<PanelView>().FromNewComponentAsViewController().AsSingle();
            Container.BindInterfacesTo<LeaderboardUtils>().AsSingle();
            Container.Bind<PlayerUtils>().AsSingle();
        }
    }
}
