using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.OnnxRuntime;
using OpenUtau.Api;

namespace OpenUtau.Core.G2p {
    public class KazakhG2p : G2pPack {
        private static readonly string[] graphemes = new string[] {
            "", "", "", "", "-", "'", 
            "а", "ә", "б", "в", "г", "ғ", "д", "е", "ж", "з", 
            "и", "й", "к", "қ", "л", "м", "н", "ң", "о", "ө", 
            "п", "р", "с", "т", "у", "ұ", "ү", "ф", "х", "һ", 
            "ц", "ч", "ш", "щ", "ы", "і", "э", "ю", "я"
        };

        private static readonly string[] phonemes = new string[] {
            "", "", "", "", "SP", "AP", "cl",
            "a", "ae", "e", "i", "iu", "io", "ou", "o", "y", "ii",
            "b", "g", "d", "gh", "k", "q", "l", "n", "r", "s", 
            "t", "p", "m", "sh", "j", "x", "h", "z", "ng", "iy", "u"
        };

        private static object lockObj = new object();
        private static Dictionary<string, int> graphemeIndexes;
        private static IG2p dict;
        private static InferenceSession session;
        private static Dictionary<string, string[]> predCache = new Dictionary<string, string[]>();

        public KazakhG2p() {
            lock (lockObj) {
                if (graphemeIndexes == null) {
                    graphemeIndexes = graphemes
                    .Skip(4)
                    .Select((g, i) => Tuple.Create(g, i))
                    .ToDictionary(t => t.Item1, t => t.Item2 + 4);
                    var tuple = LoadPack(Data.Resources.g2p_kk);
                    dict = tuple.Item1;
                    session = tuple.Item2;
                }
            }
            GraphemeIndexes = graphemeIndexes;
            Phonemes = phonemes;
            Dict = dict;
            Session = session;
            PredCache = predCache;
        }
    }
}
