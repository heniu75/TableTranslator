namespace TableTranslator.Test.TestModels
{
    public class Generics
    {
        public class OneGeneric<T>
        {
            public T TData { get; set; }
        }

        public class ThreeGenerics<T, K, J>
        {
            public T TData { get; set; }
            public K KData { get; set; }
            public J JData { get; set; }
        }
    }
}