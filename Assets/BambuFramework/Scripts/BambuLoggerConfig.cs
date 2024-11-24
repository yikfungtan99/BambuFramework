using System.Collections.Generic;
using UnityEngine;

namespace BambuFramework.Debugging
{
    [CreateAssetMenu(fileName = "BambuLogger Config", menuName = "BAMBU/DEBUG/BambuLoger Config")]
    public class BambuLoggerConfig : ScriptableObject
    {
        public List<BambuLogConfig> LogConfigurations;
    }
}
