using System;
using JetBrains.Annotations;

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// A 3E datetime attribute value
    /// </summary>
    [PublicAPI]
    public sealed class DateTimeAttribute : IAttribute
        {
        /// <summary>
        /// Gets/sets the value of this attribute
        /// </summary>
        public DateTime? Value;

        /// <summary>
        /// Constructs a new 3E datetime attribute value
        /// </summary>
        /// <param name="value">The initial value of the attribute</param>
        public DateTimeAttribute(DateTime? value)
            {
            this.Value = value;
            }

        /// <summary>
        /// Constructs a new 3E date attribute value
        /// </summary>
        /// <param name="year">The year</param>
        /// <param name="month">The month within the year</param>
        /// <param name="day">The day within the month</param>
        /// <param name="hour">The hour within the day</param>
        /// <param name="minute">The minute within the hour</param>
        /// <param name="second">The second within the minute</param>
        public DateTimeAttribute(int year, int month, int day, int hour, int minute, int second)
            {
            this.Value = new DateTime(year, month, day, hour, minute, second);
            }

        /// <inheritdoc />
        public override string ToString()
            {
            return this.Value.HasValue ? this.Value.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
            }

        /// <summary>
        /// Implicitly converts a datetime value to a DateTimeAttribute
        /// </summary>
        /// <param name="value">The datetime value to convert</param>
        public static implicit operator DateTimeAttribute(DateTime value)
            {
            return new DateTimeAttribute(value);
            }

        /// <inheritdoc />
        object? IAttribute.Value => this.Value;

        /// <inheritdoc />
        public bool HasValue => this.Value.HasValue;
        }
    }