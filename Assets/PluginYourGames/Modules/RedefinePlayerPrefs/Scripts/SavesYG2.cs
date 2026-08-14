using System.Collections.Generic;
using Player.Characteristics;

namespace YG
{
    public partial class SavesYG
    {
        public PlayerCharacteristics PlayerCharacteristics;
        public bool IsFirstLaunch = true;
        public int Money;
        
        public List<string> stringKeys = new List<string>();
        public List<string> stringValues = new List<string>();

        public List<string> floatKeys = new List<string>();
        public List<float> floatValues = new List<float>();

        public List<string> intKeys = new List<string>();
        public List<int> intValues = new List<int>();
    }
}
