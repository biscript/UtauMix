using OpenUtau.Api;
using OpenUtau.Core.G2p;

namespace OpenUtau.Core.DiffSinger
{
    [Phonemizer("DiffSinger Kazakh Phonemizer", "DIFFS KK", language: "KK")]
    public class DiffSingerKazakhPhonemizer : DiffSingerG2pPhonemizer
    {
        protected override string GetDictionaryName()=>"dsdict-kk.yaml";
        public override string GetLangCode()=>"kk";
        protected override IG2p LoadBaseG2p() => new KazakhG2p();
        protected override string[] GetBaseG2pVowels() => new string[] {
            "a", "ae", "e", "i", "iu", "io", "ou", "o", "y", "ii"
        };

        protected override string[] GetBaseG2pConsonants() => new string[] {
            "b", "g", "d", "gh", "k", "q", "l", "n", "r", "s", 
            "t", "p", "m", "sh", "j", "x", "h", "z", "ng", "iy", "u"
        };
    }
}
