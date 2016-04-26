using System;

// ReSharper disable InconsistentNaming
namespace FacadeFor3e
    {
    static class Extensions
        {
        internal static string To3eString(this bool value)
            {
            var result = value.ToString().ToLowerInvariant();
            return result;
            }

        internal static string To3eString(this int value)
            {
            var result = value.ToString("F0");
            return result;
            }

        internal static string To3eString(this int? value)
            {
            var result = value.HasValue ? value.Value.To3eString() : string.Empty;
            return result;
            }

        internal static string To3eString(this DateTime value)
            {
            string result = value.TimeOfDay == TimeSpan.Zero
                ? value.ToString("d-MMM-yyyy")
                : value.ToString("d-MMM-yyyy HH:mm:ss");
            return result;
            }

        internal static string To3eString(this DateTime? value)
            {
            string result = value.HasValue ? value.Value.To3eString() : string.Empty;
            return result;
            }

        internal static string To3eString(this decimal value)
            {
            var result = value.ToString("G");
            return result;
            }

        internal static string To3eString(this decimal? value)
            {
            var result = value.HasValue ? value.Value.To3eString() : string.Empty;
            return result;
            }

        internal static string To3eString(this Guid value)
            {
            var result = value.ToString("B");
            return result;
            }

        internal static string To3eString(this Guid? value)
            {
            var result = value.HasValue ? value.Value.To3eString() : string.Empty;
            return result;
            }

        internal static string To3eString(this string value)
            {
            var result = value ?? string.Empty;
            return result;
            }
        }
    }
