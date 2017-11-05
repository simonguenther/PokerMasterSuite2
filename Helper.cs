using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerMasterSuite2
{
    /// <summary>
    /// Helper Class for methods which don't find in any other class or will be needed at different times during runtime
    /// </summary>
    static class Helper
    {
        /// <summary>
        /// Calcuate Levenshtein Distance between two names => https://en.wikipedia.org/wiki/Levenshtein_distance
        /// "how many characters do I have to change to get matching strings?"
        /// f.e. "Walter" and "Watler" => Distance 2
        /// f.e. "rofl" and "r0fl" => Distance 1
        /// </summary>
        /// <param name="a">Nickname #1</param>
        /// <param name="b">Nickname #2</param>
        /// <returns>Levenshtein Distance from a to b</returns>
        public static int CalcLevenshteinDistance(string a, string b)
        {
            if (String.IsNullOrEmpty(a) || String.IsNullOrEmpty(b)) return 0;

            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min
                        (
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                        );
                }
            return distances[lengthA, lengthB];
        }

        /// <summary>
        /// Check for range of color since color of buttons are slightly different depending on Emulator used
        /// </summary>
        /// <param name="checkPix">Looking fot this color / Average color of butons</param>
        /// <param name="picPix">Color which gets compared to checkPix</param>
        /// <param name="allowedRange">+- range color is allowed to be in</param>
        /// <returns></returns>
        public static bool colorIsInRange(Color checkPix, Color picPix, int allowedRange)
        {
            int off = allowedRange;
            int _toR = checkPix.R + off;
            if (_toR > 255) _toR = 255;

            int _toB = checkPix.B + off;
            if (_toB > 255) _toB = 255;

            int _toG = checkPix.G + off;
            if (_toG > 255) _toG = 255;

            int _fromR = checkPix.R - off;
            if (_fromR < 0) _fromR = 0;

            int _fromB = checkPix.B - off;
            if (_fromB < 0) _fromB = 0;

            int _fromG = checkPix.G - off;
            if (_fromG < 0) _fromG = 0;

            Color _from = Color.FromArgb(checkPix.A, _fromR, _fromG, _fromB);
            Color _to = Color.FromArgb(checkPix.A, _toR, _toG, _toB);

            return
                (_from.R <= picPix.R && picPix.R <= _to.R) &&
                (_from.G <= picPix.G && picPix.G <= _to.G) &&
                (_from.B <= picPix.B && picPix.B <= _to.B);
        }
    }


}
