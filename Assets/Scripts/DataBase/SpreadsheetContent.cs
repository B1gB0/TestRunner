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
        [SpreadsheetPage("Enemies")] public List<EnemyData> Enemies;

        [SpreadsheetPage("GraveyardSceneLevels")] public List<SceneLevelData> GraveyardSceneLevels;
        [SpreadsheetPage("MissionLocalization")] public List<MissionLocalizationData> MissionsLocalization;
        
        [SpreadsheetPage("UILocalization")] public List<UILocalizationData> UILocalizationData;
    }
}