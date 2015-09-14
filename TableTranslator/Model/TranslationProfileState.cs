namespace TableTranslator.Model
{
    public enum TranslationProfileState
    {
        /// <summary>
        /// Profile has been added to the translator, but has not yet been initialized
        /// </summary>
        NotInitialization = 0,
        /// <summary>
        /// Profile has been added to the translator and is initialized
        /// </summary>
        Initialized = 1,
        /// <summary>
        /// Profile has been added to the translator, but is pending removal
        /// </summary>
        RemovalPending = 2
    }
}