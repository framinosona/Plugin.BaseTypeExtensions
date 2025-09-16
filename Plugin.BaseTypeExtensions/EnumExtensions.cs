using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Plugin.BaseTypeExtensions;

/// <summary>
///     Provides extension methods for working with enum types, including flag manipulation and description retrieval.
/// </summary>
public static class EnumExtensions
{

    /// <summary>
    ///     Sets the specified flag on the enum value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="enum">The enum value.</param>
    /// <param name="flag">The flag to set.</param>
    /// <returns>The enum value with the flag set.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T SetFlag<T>(this T @enum, T flag) where T : struct, Enum
    {
        return @enum.TweakFlag(flag, on: true);
    }


    /// <summary>
    ///     Unsets the specified flag on the enum value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="enum">The enum value.</param>
    /// <param name="flag">The flag to unset.</param>
    /// <returns>The enum value with the flag unset.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T UnsetFlag<T>(this T @enum, T flag) where T : struct, Enum
    {
        return @enum.TweakFlag(flag, on: false);
    }

    /// <summary>
    ///     Sets or unsets the specified flag on the enum value.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="enum">The enum value.</param>
    /// <param name="flag">The flag to set or unset.</param>
    /// <param name="on">True to set the flag; false to unset it.</param>
    /// <returns>The enum value with the flag set or unset.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T TweakFlag<T>(this T @enum, T flag, bool on) where T : struct, Enum // https://stackoverflow.com/a/77134085
    {
        switch (Unsafe.SizeOf<T>())
        {
            case 1: //byte
                {
                    var x = on
                        ? Unsafe.As<T, byte>(ref @enum) |= Unsafe.As<T, byte>(ref flag)
                        : Unsafe.As<T, byte>(ref @enum) &= (byte)~Unsafe.As<T, byte>(ref flag);

                    return Unsafe.As<byte, T>(ref x);
                }
            case 2: //short
                {
                    var x = on
                        ? Unsafe.As<T, short>(ref @enum) |= Unsafe.As<T, short>(ref flag)
                        : Unsafe.As<T, short>(ref @enum) &= (short)~Unsafe.As<T, short>(ref flag);

                    return Unsafe.As<short, T>(ref x);
                }
            case 4: //uint
                {
                    var x = on
                        ? Unsafe.As<T, uint>(ref @enum) |= Unsafe.As<T, uint>(ref flag)
                        : Unsafe.As<T, uint>(ref @enum) &= ~Unsafe.As<T, uint>(ref flag);

                    return Unsafe.As<uint, T>(ref x);
                }
            case 8: //ulong
                {
                    var x = on
                        ? Unsafe.As<T, ulong>(ref @enum) |= Unsafe.As<T, ulong>(ref flag)
                        : Unsafe.As<T, ulong>(ref @enum) &= ~Unsafe.As<T, ulong>(ref flag);

                    return Unsafe.As<ulong, T>(ref x);
                }
            default: //close to impossible    this is dead slow but it works as a failsafe
                {
                    var underlyingType = Enum.GetUnderlyingType(@enum.GetType());
                    dynamic flagAsInt = Convert.ChangeType(flag, underlyingType, System.Globalization.CultureInfo.InvariantCulture);
                    dynamic valueAsProperType = Convert.ChangeType(@enum, underlyingType, System.Globalization.CultureInfo.InvariantCulture);
                    if (on)
                    {
                        valueAsProperType |= flagAsInt;
                    }
                    else
                    {
                        valueAsProperType &= ~flagAsInt;
                    }

                    return (T)valueAsProperType;
                }
        }
    }

    /// <summary>
    ///     Returns all values of the specified enum type as an array.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>An array of all enum values.</returns>
    public static IEnumerable<T> AllAsArray<T>()
    {
        return Enum
            .GetValues(typeof(T))
            .Cast<Enum>()
            .Select(MapToEnumNames)
            .Where(IsSaneEnumName)
            .Select(MapToEnumValue)
            .ToArray();

        string? MapToEnumNames(Enum value) => Enum.GetName(typeof(T), value);

        bool IsSaneEnumName(string? name) => !string.IsNullOrWhiteSpace(name);

        T MapToEnumValue(string? name) => (T)Enum.Parse(typeof(T), name!);
    }

    /// <summary>Returns the description attribute of an enum value based on its <see cref="DescriptionAttribute"/> (if any) - otherwise the enum value name itself.</summary>
    /// <code>
    /// public enum Duration
    /// {
    ///     [Description("Eight hours")]
    ///     Day,
    ///
    ///     [Description("Five days")]
    ///     Week,
    ///
    ///     [Description("Twenty-one days")]
    ///     Month,
    ///
    ///     Year, // will just return "Year"
    /// }
    /// </code>
    /// <param name="value">The enum value</param>
    /// <returns>The description attribute of the enum value</returns>
    /// <summary>
    ///     Returns the description attribute of an enum value based on its <see cref="DescriptionAttribute"/> (if any), otherwise the enum value name itself.
    /// </summary>
    /// <returns>The description attribute of the enum value, or the value name if no description is present.</returns>
    public static string GetDescription(this Enum value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var valueName = value.ToString();

        var attribs = value
            .GetType()
            .GetMember(valueName)
            .FirstOrDefault()
            ?.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)
            .Cast<DescriptionAttribute>() ?? [];

        return attribs.FirstOrDefault()?.Description ?? valueName;
    }

    /// <summary>
    ///     Returns the number of values in the specified enum type.
    /// </summary>
    /// <param name="enumType">The enum type.</param>
    /// <returns>The number of values in the enum type.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="enumType"/> is not an enum type.</exception>
    public static int Count(this Type enumType)
    {
        ArgumentNullException.ThrowIfNull(enumType);

        if (!enumType.IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        return Enum.GetNames(enumType).Length;
    }

    /// <summary>
    ///     Returns a value representing all flags set for the specified enum type.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <returns>A value with all flags set.</returns>
    public static T AllAsFlag<T>()
    {
        var stringValues = (
            from Enum value in Enum.GetValues(typeof(T))
            select Enum.GetName(typeof(T), value)
        ).ToArray();

        return (T)Enum.Parse(typeof(T), string.Join(" ,", stringValues));
    }
}
