using System;
using JetBrains.Annotations;

#if !NET6_0_OR_GREATER
// fallback for .net framework which doesn't support DateOnly
using DateOnly = System.DateTime;
#endif

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// A 3E date attribute value
    /// </summary>
    [PublicAPI]
    public sealed class DateAttribute : IAttribute
        {
        private DateOnly? _internalValue;

        /// <summary>
        /// Constructs a new 3E date attribute value
        /// </summary>
        /// <param name="value">The initial value of the attribute</param>
        public DateAttribute(DateOnly? value)
            {
            this.Value = value;
            }

        /// <summary>
        /// Constructs a new 3E date attribute value
        /// </summary>
        /// <param name="year">The year</param>
        /// <param name="month">The month within the year</param>
        /// <param name="day">The day within the month</param>
        public DateAttribute(int year, int month, int day)
            {
            this.Value = new DateOnly(year, month, day);
            }

        /// <summary>
        /// Gets/sets the value of this attribute
        /// </summary>
        public DateOnly? Value
            {
            get => this._internalValue;
            set
                {
#if !NET6_0_OR_GREATER
                if (value.HasValue && value.Value.TimeOfDay != TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException(nameof(value), "DateAttribute value cannot include time element.");
#endif
                this._internalValue = value;
                }
            }

        /// <inheritdoc />
        public override string ToString()
            {
            return this.Value.HasValue ? this.Value.Value.ToString("yyyy-MM-dd") : string.Empty;
            }

        /// <summary>
        /// Implicitly converts a date value to a DateAttribute
        /// </summary>
        /// <param name="value">The date value to convert</param>
        public static implicit operator DateAttribute(DateOnly value)
            {
            return new DateAttribute(value);
            }

        /// <inheritdoc />
        object? IAttribute.Value => this.Value;

        /// <inheritdoc />
        public bool HasValue => this.Value.HasValue;
        }
    }