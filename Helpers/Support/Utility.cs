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
        public string? names;
        public string? names2;
        public string? names3;
        public string cpName;
        public string? RandomNumber;
        public string? RandomNumber2;
        public ILocator? getDate;
        public string? expectedlbl, actuallbl;
        Dictionary<string, int> months = new Dictionary<string, int>
        {
            { "January", 1 },
            { "February", 2 },
            { "March", 3 },
            { "April", 4 },
            { "May", 5 },
            { "June", 6 },
            { "July", 7 },
            { "August", 8 },
            { "September", 9 },
            { "October", 10 },
            { "November", 11 },
            { "December", 12 }
        };

        public async Task GenerateName1(int len)
        {
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }
            names2 = Name;
        }

        public async Task GenerateName(int len)

        {

            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }
            names = Name;
        }

        public async Task GenerateTaxEIN(int len)
        {
            Random r = new Random();
            string[] numbers = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string number = "";
            number = number + r.Next(numbers.Length);
            int a = 1;
            while (a < len)
            {
                number = number + r.Next(numbers.Length);
                a++;
            }
            RandomNumber = number;
        }

        public async Task Calendarpicker(string month, string date, int year, IPage _page, string action = null!)
        {

            if (action == "Clear")
            {
                await _page.Locator("//input[@id='month']").ClearAsync();
                await _page.Locator("//input[@id='day']").ClearAsync();
                await _page.Locator("//input[@id='year']").ClearAsync();

            }

            //string[] dateParts = date.Split('/');
            await _page.Locator("//input[@id='month']").ClearAsync();
            await _page.Locator("//input[@id='month']").FillAsync(months[month].ToString());
            await _page.Locator("//input[@id='day']").ClearAsync();
            await _page.Locator("//input[@id='day']").FillAsync(date);
            await _page.Locator("//input[@id='year']").ClearAsync();
            await _page.Locator("//input[@id='year']").FillAsync(year.ToString());
            await _page.Locator("//button[text()='Apply']").ClickAsync();
        }
        }


    }
