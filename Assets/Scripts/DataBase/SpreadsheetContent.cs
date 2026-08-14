using System;
using System.Collections.Generic;
using DataBase.Data;
using NorskaLib.Spreadsheets;

namespace DataBase
{
    [Serializable]
    public class SpreadsheetContent
    {
        [SpreadsheetPage("Players")] public List<PlayerData> Players;
        [SpreadsheetPage("Obstacles")] public List<ObstacleData> Obstacles;
        
        [SpreadsheetPage("UILocalization")] public List<UILocalizationData> UILocalizationData;
        [SpreadsheetPage("MissionLocalization")] public List<MissionLocalizationData> MissionsLocalization;
    }
}