using AutoMapper;
using CimscoPortal.Data;
using CimscoPortal.Models;
using System;

namespace CimscoPortal.App_Start
{
    public class AutoMapperConfig 
    {

        public static void Configure()
        {
            Mapper.CreateMap<PortalMessage, AlertViewModel>()
                .ForMember(m => m.CategoryName,
                            opt => opt.MapFrom(i => i.MessageCategory.CategoryName))
                .ForMember(m => m.TypeName,
                            opt => opt.MapFrom(i => i.MessageType.TypeName))
                .ForMember(m => m.Element1,
                            opt => opt.MapFrom(i => i.MessageCategory.Element1))
                .ForMember(m => m.Element2,
                            opt => opt.MapFrom(i => i.MessageCategory.Element2))

                //                .ForMember(x => x.TimeStamp, opt => opt.MapFrom(efo => efo.TimeStamp.ToString()));
                  .ForMember(m => m.TimeStamp, opt => opt.MapFrom(efo => (efo.TimeStamp != null) ? efo.TimeStamp.ToString() : "00:00"))
                  //.ForMember(m => m.TimeStamp, opt => opt.ResolveUsing<test>());
                  ;

            //Mapper.CreateMap<DateTime?, string>().ConvertUsing<DateTimeToStringConverter>();
            //Mapper.CreateMap<DateTime?, DateTime>().ConvertUsing<DateTimeConverter>();
            //Mapper.CreateMap<DateTime?, DateTime?>().ConvertUsing<NullableDateTimeConverter>();
            //Mapper.CreateMap<DateTime?, string>().ConvertUsing(new DateTimeToStringConverter());
            //Mapper.AddFormatter<DateStringFormatter>();
            Mapper.AssertConfigurationIsValid();
        }
    }

    //public class test : ValueResolver<DateTime?, string>
    //{
    //    protected override string ResolveCore(DateTime? source)
    //    {
    //        return "00:00";
    //    }
    //}
    //public class DateStringFormatter : BaseFormatter<DateTime?>
    //{
    //    protected override string FormatValueCore(DateTime value)
    //    {
    //        return value.ToString("dddd, MMM dd, yyyy");
    //    }
    //}
    //public abstract class BaseFormatter<T> 
    //{
    //    public string FormatValue(ResolutionContext context)
    //    {
    //        if (context.SourceValue == null)
    //            return null;

    //        if (!(context.SourceValue is T))
    //            return context.SourceValue ==
    //                                 null ? String.Empty : context.SourceValue.ToString();

    //        return FormatValueCore((T)context.SourceValue);
    //    }

    //    protected abstract string FormatValueCore(T value);
    //}

    //

    //public class DateTimeConverter : TypeConverter<DateTime?, DateTime>
    //{
    //    protected override DateTime ConvertCore(DateTime? source)
    //    {
    //        if (source.HasValue)
    //            return source.Value;
    //        else
    //            return default(DateTime);
    //    }
    //}

    //public class DateTimeToStringConverter : TypeConverter<DateTime?, string>
    //{
    //    protected override string ConvertCore(DateTime? source)
    //    {
    //        if (source.HasValue)
    //            return source.Value.ToString();
    //        else
    //            return "00:00";
    //    }
    //}

    //public class NullableDateTimeConverter : TypeConverter<DateTime?, DateTime?>
    //{
    //    protected override DateTime? ConvertCore(DateTime? source)
    //    {
    //        return source;
    //    }
    //}

}
