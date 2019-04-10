using System;
using DotNetHelper_Contracts.Enum;

// ReSharper disable InconsistentNaming
namespace DotNetHelper_Contracts.Attribute
{




    /// <inheritdoc />
    /// <summary>
    /// Class DataValidationAttribute.
    /// </summary>
    /// <seealso cref="T:System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class DataValidationAttribute : System.Attribute
    {
        /// <summary>
        /// Max Size Property Can Be
        /// </summary>
        /// <value>The maximum size of the length.</value>

        public int? MaxLengthSize { get; set; } = null;
        /// <summary>
        /// Gets or sets the size of the set maximum length.
        /// </summary>
        /// <value>The size of the set maximum length.</value>
        /// <exception cref="Exception">Nooo...  Your Using DataValidationAttribute wrong  You Must Set This Attribute Value Before Trying To Get It There IS NO SUCH THING AS NULL</exception>
        public int SetMaxLengthSize
        {
            get
            {
                if (MaxLengthSize != null) { return MaxLengthSize.Value; } else { throw new Exception("Nooo...  Your Using DataValidationAttribute wrong  You Must Set This Attribute Value Before Trying To Get It There IS NO SUCH THING AS NULL"); }
            }
            //throw new Exception("Nooo...  Your Using DataValidationAttribute wrong do not try to get from the Set Property use the orignial ");
            set => MaxLengthSize = value;
        }

        /// <summary>
        /// Specifies whether the property is required. 
        /// </summary>
        /// <value><c>null</c> if [require value] contains no value, <c>true</c> if [require value]; otherwise, <c>false</c>.</value>

        public bool? RequireValue { get; set; } = null;
        /// <summary>
        /// Gets or sets a value indicating whether [set require value].
        /// </summary>
        /// <value><c>true</c> if [set require value]; otherwise, <c>false</c>.</value>
        /// <exception cref="Exception">Nooo...  Your Using DataValidationAttribute wrong  You Must Set This Attribute Value Before Trying To Get It There IS NO SUCH THING AS NULL</exception>
        public bool SetRequireValue
        {
            get
            {
                if (RequireValue != null) { return RequireValue.Value; } else { throw new Exception("Nooo...  Your Using DataValidationAttribute wrong  You Must Set This Attribute Value Before Trying To Get It There IS NO SUCH THING AS NULL"); }
            }
            set => RequireValue = value;
        }


        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public DataType? DataType { get; set; } = Enum.DataType.Unknown;
        /// <summary>
        /// Gets or sets the type of the set data.
        /// </summary>
        /// <value>The type of the set data.</value>
        /// <exception cref="Exception">Nooo...  Your Using DataValidationAttribute wrong  You Must Set This Attribute Value Before Trying To Get It There IS NO SUCH THING AS NULL</exception>
        public DataType SetDataType
        {
            get
            {
                if (DataType != null) { return DataType.Value; } else { throw new Exception("Nooo...  Your Using DataValidationAttribute wrong  You Must Set This Attribute Value Before Trying To Get It There IS NO SUCH THING AS NULL"); }
            }
            set => DataType = value;
        }


        /// <summary>
        /// Gets or sets a value indicating whether this instance can contain numbers.
        /// </summary>
        /// <value><c>null</c> if [can contain numbers] contains no value, <c>true</c> if [can contain numbers]; otherwise, <c>false</c>.</value>
        public bool? CanContainNumbers { get; set; } = null;
        /// <summary>
        /// Gets or sets a value indicating whether [set can contain numbers].
        /// </summary>
        /// <value><c>true</c> if [set can contain numbers]; otherwise, <c>false</c>.</value>
        /// <exception cref="Exception">Nooo...  Your Using DataValidationAttribute wrong  You Must Set This Attribute Value Before Trying To Get It There IS NO SUCH THING AS NULL</exception>
        public bool SetCanContainNumbers
        {
            get
            {
                if (CanContainNumbers != null) { return CanContainNumbers.Value; } else { throw new Exception("Nooo...  Your Using DataValidationAttribute wrong  You Must Set This Attribute Value Before Trying To Get It There IS NO SUCH THING AS NULL"); }
            }
            set => CanContainNumbers = value;
        }


        /// <summary>
        /// Gets or sets a value indicating whether this instance can contain letter.
        /// </summary>
        /// <value><c>null</c> if [can contain letter] contains no value, <c>true</c> if [can contain letter]; otherwise, <c>false</c>.</value>
        public bool? CanContainLetter { get; set; } = null;
        /// <summary>
        /// Gets or sets a value indicating whether [set can contain letter].
        /// </summary>
        /// <value><c>true</c> if [set can contain letter]; otherwise, <c>false</c>.</value>
        /// <exception cref="Exception">Nooo...  Your Using DataValidationAttribute wrong  You Must Set This Attribute Value Before Trying To Get It There IS NO SUCH THING AS NULL</exception>
        public bool SetCanContainLetter
        {
            get
            {
                if (CanContainLetter != null) { return CanContainLetter.Value; } else { throw new Exception("Nooo...  Your Using DataValidationAttribute wrong  You Must Set This Attribute Value Before Trying To Get It There IS NO SUCH THING AS NULL"); }
            }
            set => CanContainLetter = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="DataValidationAttribute"/> is ignore.
        /// </summary>
        /// <value><c>null</c> if [ignore] contains no value, <c>true</c> if [ignore]; otherwise, <c>false</c>.</value>
        public bool? Ignore { get; set; } = null;
        /// <summary>
        /// Gets or sets a value indicating whether [set ignore].
        /// </summary>
        /// <value><c>true</c> if [set ignore]; otherwise, <c>false</c>.</value>
        /// <exception cref="Exception">Nooo...  Your Using DataValidationAttribute wrong  You Must Set This Attribute Value Before Trying To Get It There IS NO SUCH THING AS NULL</exception>
        public bool SetIgnore
        {
            get
            {
                if (Ignore != null) { return Ignore.Value; } else { throw new Exception("Nooo...  Your Using DataValidationAttribute wrong  You Must Set This Attribute Value Before Trying To Get It There IS NO SUCH THING AS NULL"); }
            }
            set => Ignore = value;
        }

    }



    /// <summary>
    /// Enum DataValidationAttributeMembers
    /// </summary>
    public enum DataValidationAttributeMembers
    {
        /// <summary>
        /// The set maximum length size
        /// </summary>
        SetMaxLengthSize
        /// <summary>
        /// The set require value
        /// </summary>
        , SetRequireValue
        /// <summary>
        /// The set data type
        /// </summary>
        , SetDataType
        /// <summary>
        /// The set can contain numbers
        /// </summary>
        , SetCanContainNumbers
        /// <summary>
        /// The set can contain letter
        /// </summary>
        , SetCanContainLetter
        /// <summary>
        /// The set ignore
        /// </summary>
        , SetIgnore
    }



}
