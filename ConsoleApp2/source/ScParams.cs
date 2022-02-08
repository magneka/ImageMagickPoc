using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp2
{
    public class ScParams
    {
        public List<string> FileNames { get; set; }
        public string Ext { get; set; }
        public List<Int32> Sizes { get; set; }
        public int Quality { get; set; }
        public string InFolder { get; set; }
        public string OutFolder { get; set; }
        public bool AutoLevel { get; set; }
        public string ColorSpace { get; set; }
        public Tuple<Double?, Double?> Modulate { get; set; }
        public double? GammaCorrect { get; set; }
        public bool Strip { get; set; }
        public string Interlace { get; set; }

        public ScParams()
        {
            Modulate = null;
            GammaCorrect = null;
            Strip = false;
        }


    }
}
