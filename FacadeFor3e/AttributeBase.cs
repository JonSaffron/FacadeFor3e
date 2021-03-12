using System;
using System.Globalization;

// key can be int string guid date datetime
// attributes can be: decimal  bool 
// email money text url pred multlangstring autonum image narrative



namespace FacadeFor3e
    {
    public interface IAttribute
        {
        object Value { get; }
        }

    public abstract class AttributeBase<T> : IAttribute
        {
        protected T _value;

        public AttributeBase(T value)
            {
            this._value = value;
            }

        object IAttribute.Value => this._value;

        public T Value { get; set; }
        }

    public sealed class DecimalAttribute : AttributeBase<decimal?>
        {
        public DecimalAttribute(decimal? value) : base(value)
            {
            }

        public override string ToString()
            {
            return this._value.HasValue ? this._value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }

        public static implicit operator DecimalAttribute(decimal value)
            {
            return new DecimalAttribute(value);
            }
        }

    public sealed class IntAttribute : AttributeBase<int?>
        {
        public IntAttribute(int? value) : base(value)
            {
            }

        public override string ToString()
            {
            return this._value.HasValue ? this._value.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
            }

        public static implicit operator IntAttribute(int value)
            {
            return new IntAttribute(value);
            }
        }

    public sealed class StringAttribute : AttributeBase<string>
        {
        public StringAttribute(string value) : base(value)
            {
            }

        public override string ToString()
            {
            return _value ?? string.Empty;
            }

        public static implicit operator StringAttribute(string value)
            {
            return new StringAttribute(value);
            }
        }

    public sealed class GuidAttribute : AttributeBase<Guid?>
        {
        public GuidAttribute(Guid? value) : base(value)
            {
            }

        public override string ToString()
            {
            return this._value.HasValue ? this._value.Value.ToString("B") : string.Empty;
            }

        public static implicit operator GuidAttribute(Guid value)
            {
            return new GuidAttribute(value);
            }
        }

    public sealed class DateAttribute : AttributeBase<DateTime?>
        {
        public DateAttribute(DateTime? value) : base(value)
            { }

        public override string ToString()
            {
            return this._value.HasValue ? this._value.Value.ToString("yyyy-MM-dd") : string.Empty;
            }

        public static implicit operator DateAttribute(DateTime value)
            {
            return new DateAttribute(value.Date);
            }
        }

    public sealed class DateTimeAttribute : AttributeBase<DateTime?>
        {
        public DateTimeAttribute(DateTime? value) : base(value)
            { }

        public override string ToString()
            {
            return this._value.HasValue ? this._value.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
            }

        public static implicit operator DateTimeAttribute(DateTime value)
            {
            return new DateTimeAttribute(value);
            }
        }

    public sealed class BoolAttribute : AttributeBase<bool>
        {
        public BoolAttribute(bool value) : base(value)
            { }

        public override string ToString()
            {
            return this._value.ToString().ToLowerInvariant();
            }

        public static implicit operator BoolAttribute(bool value)
            {
            return new BoolAttribute(value);
            }
        }


    }
