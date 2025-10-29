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
            "ae", "ii", "iu", "i", "io", "e",
            "a", "iy", "ou", "y", "o", "u"
        };

        protected override string[] GetBaseG2pConsonants() => new string[] {
            "b", "d", "l", "n", "r", "s", "t", "p", "m", "sh", "j", "x", "h", "z", "ng",
            "g", "k",
            "gh", "q"
        };
    }
}
