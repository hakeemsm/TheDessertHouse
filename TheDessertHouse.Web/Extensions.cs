using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AutoMapper;

namespace TheDessertHouse.Web
{
    public static class GeneralExtensions
    {
        public static string ToUrlFormat(this string str)
        {
            var urlRegExp = new Regex(@"[^0-9a-z \-]*", RegexOptions.CultureInvariant | RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return urlRegExp.Replace(str.Trim(), string.Empty).Replace(' ', '-').ToLowerInvariant();
        }

        public static string ToLongString(this TimeSpan time)
        {
            string str = string.Empty;
            if (time.Days > 0)
            {
                str = str + time.Days + " days ";
            }
            if (((time.Days == 0) || (time.Days == 1)) && (time.Hours > 0))
            {
                str = str + time.Hours + " hr ";
            }
            if ((time.Days == 0) && (time.Minutes > 0))
            {
                str = str + time.Minutes + " min ";
            }
            if (str.Length == 0)
            {
                str = str + time.Seconds + " sec";
            }
            return str.Trim();
        }

        public static string SinceTime(this DateTime date)
        {
            return (DateTime.Now - date).ToLongString();
        }

        public static string[] GetEntityAndParmNames(this string actionName)
        {
            //This gets the remainder of the string after "Create". So "Department" for "CreateDepartment"
            var entityName = actionName.Substring(6);
            var names = new string[2];
            names[0] = entityName;
            var startChar = entityName.Substring(0, 1).ToLower();
            names[1] =entityName.Remove(0,1).Insert(0, startChar) + "View";
            return names;
        }

        public static void CreateImageFolder(this string itemName)
        {
            var path = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            var mainPath = path.Substring(0, path.IndexOf("bin", 0));
            var imgPath = string.Format(@"{0}\Content\images\item\{1}", mainPath, itemName);
            if (!Directory.Exists(imgPath))
                Directory.CreateDirectory(imgPath);
        }
    }


}