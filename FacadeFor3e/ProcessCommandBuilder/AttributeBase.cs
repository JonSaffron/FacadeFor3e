using System;
using System.Globalization;
using JetBrains.Annotations;
#if !NET6_0_OR_GREATER
using DateOnly = System.DateTime;
#endif

namespace FacadeFor3e.ProcessCommandBuilder
    {
    /// <summary>
    /// An untyped 3E attribute value
    /// </summary>
    [PublicAPI]
    public interface IAttribute
        {
        /// <summary>
        /// Gets the untyped attribute value
        /// </summary>
        object? Value { get; }
        }

    /// <summary>
    /// A 3E decimal attribute value
    /// </summary>
    [PublicAPI]
    public sealed class DecimalAttribute : IAttribute
        {
        /// <summary>
        /// Gets/sets the value of this attribute
        /// </summary>
        public decimal? Value;

        /// <summary>
        /// Constructs a new 3E decimal attribute value
        /// </summary>
        /// <param name="value">The initial value of the attribute</param>
        public DecimalAttribute(decimal? value)
            {
            this.Value = value;
            }

        /// <inheritdoc />
        public override string ToString()
            {
            return this.Value.HasValue ? this.Value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }

        /// <summary>
        /// Implicitly converts a decimal value to a DecimalAttribute
        /// </summary>
        /// <param name="value">The decimal value to convert</param>
        public static implicit operator DecimalAttribute(decimal value)
            {
            return new DecimalAttribute(value);
            }

        /// <inheritdoc />
        object? IAttribute.Value => this.Value;
        }

    /// <summary>
    /// A 3E integer attribute value
    /// </summary>
    [PublicAPI]
    public sealed class IntAttribute : IAttribute
        {
        /// <summary>
        /// Gets/sets the value of this attribute
        /// </summary>
        public int? Value;

        /// <summary>
        /// Constructs a new 3E integer attribute value
        /// </summary>
        /// <param name="value">The initial value of the attribute</param>
        public IntAttribute(int? value)
            {
            this.Value = value;
            }

        /// <inheritdoc />
        public override string ToString()
            {
            return this.Value.HasValue ? this.Value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }

        /// <summary>
        /// Implicitly converts an integer value to a IntAttribute
        /// </summary>
        /// <param name="value">The integer value to convert</param>
        public static implicit operator IntAttribute(int value)
            {
            return new IntAttribute(value);
            }

        /// <inheritdoc />
        object? IAttribute.Value => this.Value;
        }

    /// <summary>
    /// A 3E string attribute value
    /// </summary>
    [PublicAPI]
    public sealed class StringAttribute : IAttribute
        {
        /// <summary>
        /// Gets/sets the value of this attribute
        /// </summary>
        public string? Value;

        /// <summary>
        /// Constructs a new 3E string attribute value
        /// </summary>
        /// <param name="value">The initial value of the attribute</param>
        public StringAttribute(string? value)
            {
            this.Value = value;
            }

        /// <inheritdoc />
        public override string ToString()
            {
            return this.Value ?? string.Empty;
            }

        /// <summary>
        /// Implicitly converts a string value to a StringAttribute
        /// </summary>
        /// <param name="value">The string value to convert</param>
        public static implicit operator StringAttribute(string value)
            {
            return new StringAttribute(value);
            }

        /// <inheritdoc />
        object? IAttribute.Value => this.Value;
        }

    /// <summary>
    /// A 3E guid attribute value
    /// </summary>
    [PublicAPI]
    public sealed class GuidAttribute : IAttribute
        {
        /// <summary>
        /// Gets/sets the value of this attribute
        /// </summary>
        public Guid? Value;

        /// <summary>
        /// Constructs a new 3E guid attribute value
        /// </summary>
        /// <param name="value">The initial value of the attribute</param>
        public GuidAttribute(Guid? value)
            {
            this.Value = value;
            }

        /// <inheritdoc />
        public override string ToString()
            {
            return this.Value.HasValue ? this.Value.Value.ToString("D") : string.Empty;
            }

        /// <summary>
        /// Implicitly converts a guid value to a GuidAttribute
        /// </summary>
        /// <param name="value">The guid value to convert</param>
        public static implicit operator GuidAttribute(Guid value)
            {
            return new GuidAttribute(value);
            }

        /// <inheritdoc />
        object? IAttribute.Value => this.Value;
        }

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
        }

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
        }

    /// <summary>
    /// A 3E Boolean attribute value
    /// </summary>
    [PublicAPI]
    public sealed class BoolAttribute : IAttribute
        {
        /// <summary>
        /// Gets/sets the value of this attribute
        /// </summary>
        public bool Value;

        /// <summary>
        /// Constructs a new 3E Boolean attribute value
        /// </summary>
        /// <param name="value"></param>
        public BoolAttribute(bool value)
            {
            this.Value = value;
            }

        /// <inheritdoc />
        public override string ToString()
            {
            return this.Value.ToString().ToLowerInvariant();
            }

        /// <summary>
        /// Implicitly converts a Boolean value to a BooleanAttribute
        /// </summary>
        /// <param name="value">The Boolean value to convert</param>
        public static implicit operator BoolAttribute(bool value)
            {
            return new BoolAttribute(value);
            }

        /// <inheritdoc />
        object IAttribute.Value => this.Value;
        }
    }
