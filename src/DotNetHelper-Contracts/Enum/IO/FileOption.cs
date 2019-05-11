namespace DotNetHelper_Contracts.Enum.IO
{
    /// <summary>
    /// Enum FileOption
    /// </summary>
    public enum FileOption
    {
        /// <summary>
        /// The append
        /// </summary>
        Append = 1,
        /// <summary>
        /// The overwrite
        /// </summary>
        Overwrite = 2,
        /// <summary>
        /// The do nothing if exist
        /// </summary>
        DoNothingIfExist = 3,
        /// <summary>
        /// Increment the file name +1
        /// </summary>
        IncrementFileNameIfExist = 4,
        /// <summary>
        /// Increments the file extension +1
        /// </summary>
        IncrementFileExtensionIfExist = 5,
    }

/// <summary>
/// Enum AddOrRemoveEnum
/// </summary>
public enum AddOrRemoveEnum
    {
        /// <summary>
        /// The add
        /// </summary>
        Add = 1,
        /// <summary>
        /// The remove
        /// </summary>
        Remove = 2
    }

    public enum IOType
    {
        File = 1,
        Folder = 2,
        Both = 3,
    }


    public enum FileType
    {
        Xml,
        Json,
        Csv,
        AllTypes
    }

}
