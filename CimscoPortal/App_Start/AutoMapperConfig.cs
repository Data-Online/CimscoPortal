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
            Mapper.CreateMap<PortalMessage, AlertData>()
                .ForMember(m => m.TypeName,
                            opt => opt.MapFrom(i => i.MessageFormat.MessageType.Description))
                .ForMember(m => m.Element1,
                            opt => opt.MapFrom(i => i.MessageFormat.Element1))
                .ForMember(m => m.Element2,
                            opt => opt.MapFrom(i => i.MessageFormat.Element2))

                //                .ForMember(x => x.TimeStamp, opt => opt.MapFrom(efo => efo.TimeStamp.ToString()));
                .ForMember(m => m.TimeStamp, opt => opt.MapFrom(i => (i.TimeStamp != null) ? i.TimeStamp.ToString() : DateTime.Now.ToString()))
                //.ForMember(m => m._timeStamp, opt => opt.MapFrom(i => (i.TimeStamp != null) ? i.TimeStamp : DateTime.Now))
                .ForMember(m => m._timeStamp, opt => opt.NullSubstitute(DateTime.Now))
                .ForMember(m => m._timeStamp, opt => opt.MapFrom(i => i.TimeStamp))
                // .ForMember(m => m._timeStamp, opt => opt.Ignore())
                    .ForMember(m => m.CategoryName, opt => opt.Ignore())
                    .ForMember(m => m.Subject, opt => opt.Ignore())
                    .ForMember(m => m.Name, opt => opt.Ignore())
                //  .ForMember(m => m.TimeStamp,  opt => opt.Ignore())
                //.ForMember(m => m.TimeStamp, opt => opt.ResolveUsing<TimeStringResolver>());
                  ;


            Mapper.CreateMap<CimscoPortal.Data.Models.PortalMessage, MessageViewModel>()
                .ForMember(m => m.TypeName,
                            opt => opt.MapFrom(i => i.MessageFormat.MessageType.Description))
                .ForMember(m => m.Element1,
                            opt => opt.MapFrom(i => i.MessageFormat.Element1))
                .ForMember(m => m.Element2,
                            opt => opt.MapFrom(i => i.MessageFormat.Element2))

                //                .ForMember(x => x.TimeStamp, opt => opt.MapFrom(efo => efo.TimeStamp.ToString()));
                .ForMember(m => m.TimeStamp, opt => opt.MapFrom(i => (i.TimeStamp != null) ? i.TimeStamp.ToString() : DateTime.Now.ToString()))
                            //.ForMember(m => m._timeStamp, opt => opt.MapFrom(i => (i.TimeStamp != null) ? i.TimeStamp : DateTime.Now))
                .ForMember(m => m._timeStamp, opt => opt.NullSubstitute(DateTime.Now))
                .ForMember(m => m._timeStamp, opt => opt.MapFrom(i => i.TimeStamp))
                            // .ForMember(m => m._timeStamp, opt => opt.Ignore())
                    .ForMember(m => m.CategoryName, opt => opt.Ignore())
                    .ForMember(m => m.Subject, opt => opt.Ignore())
                    .ForMember(m => m.Name, opt => opt.Ignore())
                            //  .ForMember(m => m.TimeStamp,  opt => opt.Ignore())
                            //.ForMember(m => m.TimeStamp, opt => opt.ResolveUsing<TimeStringResolver>());
                  ;


            ////Mapper.CreateMap<CimscoPortal.Data.Models.Contact, CimscoPortal.Services.CompanyDataViewModel>()
            ////    .ForMember(m => m .GroupName, opt => opt.MapFrom(i => i.Groups.G))

            Mapper.AssertConfigurationIsValid();

            Mapper.CreateMap<InvoiceSummary, EnergyData>()
                .ForMember(m => m.Energy,
                                opt => opt.MapFrom(i => i.TotalEnergyCharges))
                .ForMember(m => m.Line,
                opt => opt.MapFrom(i => i.TotalNetworkCharges))
                .ForMember(m => m.Other,
                                opt => opt.MapFrom(i => i.TotalMiscCharges))
                .ForMember(m => m.Month,
                                opt => opt.MapFrom(i => i.InvoiceDate.ToString()))
                .ForMember(m => m._month,
                                opt => opt.MapFrom(i => i.InvoiceDate))
                                ;

            //Mapper.CreateMap<InvoiceSummary, DonutChartViewModel>()
            //    .ForMember(m => m.DonutChartData, opt => opt.MapFrom(i => i.TotalCharges))
            //    ;
            //  Mapper.CreateMap<DateTime?, string>().ConvertUsing<DateTimeToStringConverter>();
            //Mapper.CreateMap<DateTime?, DateTime>().ConvertUsing<DateTimeConverter>();
            //Mapper.CreateMap<DateTime?, DateTime?>().ConvertUsing<NullableDateTimeConverter>();
            //Mapper.CreateMap<DateTime?, string>().ConvertUsing(new DateTimeToStringConverter());
            //Mapper.AddFormatter<DateStringFormatter>();

            // Mapper.AssertConfigurationIsValid();
        }

    }

    //public interface ITypeConverter<TSource, TDestination>
    //{
    //    TDestination Convert(TSource source);
    //}

    //public class DateTimeToStringConverter : ITypeConverter<DateTime?, string>
    //{
    //    public string Convert(DateTime source)
    //    {
    //        if (source.HasValue)
    //            return source.Value.ToString();
    //        else
    //            return "00:00";
    //    }
    //}
    //public class TimeStringResolver : ValueResolver<AlertData, string>
    //{
    //    protected override string ResolveCore(AlertData value)
    //    {
    //        //return (value == null ? "00:00" : value.TimeStamp.ToString());
    //        return "00:00";
    //    }
    //}

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



    //public class NullableDateTimeConverter : TypeConverter<DateTime?, DateTime?>
    //{
    //    protected override DateTime? ConvertCore(DateTime? source)
    //    {
    //        return source;
    //    }
    //}

}
