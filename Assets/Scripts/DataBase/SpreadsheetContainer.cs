using NorskaLib.Spreadsheets;
using UnityEngine;

namespace DataBase
{
    [CreateAssetMenu(fileName = "SpreadsheetContainer", menuName = "SpreadsheetContainer")]
    public class SpreadsheetContainer : SpreadsheetsContainerBase
    {
        [SpreadsheetContent]
        [SerializeField] private SpreadsheetContent _content;

        public SpreadsheetContent Content => _content;
    }
}