// файл: KazakhRuleG2p.cs
using System;
using System.Collections.Generic;
using System.Linq;
using OpenUtau.Api;
using OpenUtau.Core.G2p;

namespace OpenUtau.Core.G2p {
    // Простейшая реализация IG2p: словарь -> правило (буква->фонема)
    public class KazakhG2p : IG2p {
        private Dictionary<string, string[]> dict;

        public KazakhG2p() {
            // Загружаем словарь (если есть). Для примера — пустой.
            dict = new Dictionary<string, string[]>();
            // можно загрузить dsdict-kk.yaml здесь при желании
        }

        // Псевдо-сигнатура: адаптируйте под реальный интерфейс IG2p
        public string[] Predict(string word) {
            var key = word.ToLowerInvariant();
            if (dict.ContainsKey(key)) return dict[key];

            // Простое буквенное правило — заменим буквы на фонемы через словарь char->phoneme
            // НИЖЕ — очень простой пример. Замените на корректную транскрипцию.
            var map = new Dictionary<char, string> {
                {'ә',"ae"}, {'и',"ii"}, {'ү',"iu"}, {'і',"i"}, {'ө',"io"}, {'е',"e"},
                {'а',"a"}, {'й',"iy"}, {'ұ',"ou"}, {'ы',"y"}, {'о',"o"}, {'у',"u"},
                {'б',"b"}, {'д',"d"}, {'л',"l"}, {'н',"n"}, {'р',"r"}, {'с',"s"}, {'т',"t"}, {'п',"p"}, {'м',"m"}, {'ш',"sh"}, {'ж',"j"}, {'х',"x"}, {'һ',"h"}, {'з',"z"}, {'ң',"ng"},
                {'г',"g"}, {'к',"k"},
                {'ғ',"gh"}, {'қ',"q"}
            };

            var phonemes = new List<string>();
            foreach (var ch in key) {
                if (map.TryGetValue(ch, out var ph)) phonemes.Add(ph);
                else if (char.IsWhiteSpace(ch)) phonemes.Add(" ");
                else phonemes.Add(ch.ToString()); // fallback
            }
            return phonemes.ToArray();
        }

        // Если интерфейс IG2p требует другую сигнатуру (например Phonemize(word, out ...)), адаптируйте.
    }
}
