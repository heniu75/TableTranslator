namespace TableTranslator.Abstract
{
    internal interface ICloneable<out T>
    {
        /// <summary>
        /// Performs a shallow clone of the object
        /// </summary>
        /// <returns></returns>
        T ShallowClone();

        /// <summary>
        /// Performs a deep clone of the object including reference properties
        /// </summary>
        /// <returns></returns>
        T DeepClone();
    }
}