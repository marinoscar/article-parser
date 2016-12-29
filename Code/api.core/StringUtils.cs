using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace api.core
{
    public static class StringUtils
    {

        public static string PretifyWords(string content)
        {
            var result = new List<string>();
            foreach(var word in content.Split(" ".ToCharArray()).Where(i => !string.IsNullOrWhiteSpace(i)))
            {
                result.Add(string.Format("{0}{1}",char.ToUpper(word[0]),word.Substring(1)));
            }
            return string.Join(" ", result);
        }

        public static string RemoveStyles(string content)
        {
            const string headExp = @"<head>[\s\S]*</head>";
            const string classExp = @"class=(""|')[\s\S]*?(""|')";
            var result = string.Empty;
            result = Regex.Replace(content, headExp, string.Empty);
            result = Regex.Replace(result, classExp, string.Empty);
            return result;
        }

        public static string RemoveImages(string content)
        {
            var result = string.Empty;
            const string picExp = @"<picture>[\s\S]*</picture>";
            const string imgExp = @"<img[\s\S]*?>";
            result = Regex.Replace(content, picExp, string.Empty);
            result = Regex.Replace(result, imgExp, string.Empty);
            return result;
        }
    }
}
