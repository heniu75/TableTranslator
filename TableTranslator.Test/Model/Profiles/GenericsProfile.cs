using System;
using System.Collections.Generic;
using TableTranslator.Model;
using TableTranslator.Model.Settings;

namespace TableTranslator.Test.Model.Profiles
{
    public class GenericsProfile : TranslationProfile
    {
        protected override void Configure()
        {
            AddTranslation<Generics.OneGeneric
                <Generics.ThreeGenerics<int, DateTime, Generics.OneGeneric
                    <int>>>>(new TranslationSettings("ThreeDeep"))
                .ForMember(x => x.TData.JData.TData);

            AddTranslation<Generics.OneGeneric<int>>(new TranslationSettings("IntGeneric"))
                .ForMember(x => x.TData);

            AddTranslation<Generics.ThreeGenerics<int, DateTime, string>>(new TranslationSettings("IntDateTimeStringGeneric"))
                .ForMember(x => x.TData, new ColumnSettings<int> { ColumnName = "T Data" })
                .ForMember(x => x.KData, new ColumnSettings<DateTime> { ColumnName = "K Data" })
                .ForMember(x => x.JData, new ColumnSettings<string> { ColumnName = "J Data" });

            AddTranslation<Generics.OneGeneric<Generics.OneGeneric<bool>>>(new TranslationSettings("NestedGeneric"))
                .ForMember(x => x.TData.TData);

            AddTranslation<Generics.OneGeneric<bool?>>(new TranslationSettings("NullableGeneric"))
                .ForMember(x => x.TData.Value);

            AddTranslation<List<bool>>()
                .ForDelegate(x => x.Count);
        }
    }
}