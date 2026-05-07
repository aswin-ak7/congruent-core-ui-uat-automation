using Helpers.Hook;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Helpers.support
{
    public class ABC
    {

    }

    public static class Extensions
    {
        public static string ReturnFun(this ABC a)
        {
            return "abc";
        }

        public static string ReverseString(this string s)
        {
            var sb = new StringBuilder();
            for (int i = s.Length - 1; i >= 0; i--)
                sb.Append(s[i]);
            return sb.ToString();
        }
    }

    public static class Locators
    {
        public static ILocator GetInputLocator(this IPage _page, string id, string attribute)
        {
            var x = new ABC();
            x.ReturnFun();
            _ = id.ReverseString();
            return _page.Locator($"input[name=\"{attribute}\"]");
        }

        public static ILocator GetDatepickerLocator(this IPage _page, string id, string attribute)
        {
            return _page.Locator($"input[name=\"{attribute}\"]");
        }
    }

    public class Utility
    {
        public Utility()
        {
        }

        private IPage _page = GetHelper.Page;

        public Utility(GetHelper hooks)
        {
            // optionally initialize with provided hooks
        }
    }
}
