using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Common.Extensions;

public static class StringExtensions
{
    class StringTemplate
    {
        public int Index { get; set; }
        public required string TemplateName { get; set; }

        public string ApplyValue(string text, string value)
        {
            return text.Replace(TemplateName, value);
        }
    }

    public static string FillTemplate(this string template, params object[] args)
    {
        if (args == null || args.Length == 0)
            return template;
        var result = template;
        for (int i = 0; i < args.Length; i++)
        {
            result = result.Replace($"{{{i}}}", args[i]?.ToString() ?? string.Empty);
        }
        return result;
    }

    public static string StandartizePath(this string path)
    {
        path = path.Replace('\\', '/');
        return path.Last() == '/' ? path[..^1] : path;
    }

    public static string StandartizeUrl(this string path)
    {
        if (string.IsNullOrEmpty(path))
            return string.Empty;

        path = path.First() == '/' ? path[1..] : path;
        return path.Last() == '/' ? path[..^1] : path;
    }

    public static string FillRoute(this string route, params object[] args)
    {
        int appliedArg = 0;
        foreach (var template in GetStringTemplates(route))
        {
            if (appliedArg >= args.Length)
                throw new ArgumentException($"Not enough arguments provided for route template: {template.TemplateName}");

            route = template.ApplyValue(route, args[appliedArg].ToString()!);

            appliedArg++;
        }

        if (appliedArg != args.Length)
        {
            throw new ArgumentException($"Bad arguments count. Passed arguments: {args.Length}, applied arguments: {appliedArg}");
        }

        return route;
    }

    private static IEnumerable<StringTemplate> GetStringTemplates(this string templateText)
    {
        int i = 0;
        while (i < templateText.Length)
        {
            int startIndex = templateText.IndexOf('{', i);
            if (startIndex == -1)
                yield break;

            int count = templateText.IndexOf('}', i);
            if (count == -1)
                yield break;

            yield return new StringTemplate()
            {
                Index = startIndex,
                TemplateName = templateText[startIndex..(count + 1)],
            };
            i = count + 1;
        }
    }

}
