using AutoMapper;
using CimscoPortal.Data.Models;
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
                            opt => opt.MapFrom(i => i.MessageFormat.MessageType.PageElement))
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
                            opt => opt.MapFrom(i => i.MessageFormat.MessageType.PageElement))
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
                    //.ForMember(m => m.Subject, opt => opt.Ignore())
                    .ForMember(m => m.Name, opt => opt.Ignore())
                //  .ForMember(m => m.TimeStamp,  opt => opt.Ignore())
                //.ForMember(m => m.TimeStamp, opt => opt.ResolveUsing<TimeStringResolver>());
                  ;


            ////Mapper.CreateMap<CimscoPortal.Data.Models.Contact, CimscoPortal.Services.CompanyDataViewModel>()
            ////    .ForMember(m => m .GroupName, opt => opt.MapFrom(i => i.Groups.G))

           // Mapper.AssertConfigurationIsValid();

            Mapper.CreateMap<InvoiceSummary, EnergyData>()
                .ForMember(m => m.Energy,
                                opt => opt.MapFrom(i => i.EnergyChargesTotal))
                .ForMember(m => m.Line,
                                //opt => opt.MapFrom(i => i.TotalNetworkCharges))
                                opt => opt.MapFrom(i => i.NetworkChargesTotal))
                .ForMember(m => m.Other,
                                opt => opt.MapFrom(i => i.MiscChargesTotal))
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

            //Mapper.CreateMap<Customer, CustomerData>();
            //Mapper.CreateMap<Group, CustomerHierarchyViewModel>()
            //    //.ForMember(m => m.GroupName, opt => opt.MapFrom(i => i.GroupName))
            //    .ForMember(m => m.CustomerData, opt => opt.MapFrom(i => i.Customers));

            Mapper.CreateMap<Site, SiteData>();
            Mapper.CreateMap<Group, SiteHierarchyViewModel>()
                .ForMember(m => m.HeaderName, opt => opt.MapFrom(i => i.GroupName))
                .ForMember(m => m.SiteData, opt => opt.MapFrom(i => i.Sites));
            Mapper.CreateMap<Customer, SiteHierarchyViewModel>()
                .ForMember(m => m.HeaderName, opt => opt.MapFrom(i => i.CustomerName))
                .ForMember(m => m.SiteData, opt => opt.MapFrom(i => i.Sites));


            Mapper.CreateMap<InvoiceSummary, CompanyInvoiceViewModel2>()
                .ForMember(m => m.YearA, opt => opt.MapFrom(i => i.TotalCharges));

            Mapper.CreateMap<InvoiceSummary, InvoiceDetail>()
                .ForMember(m => m.ApproversName, opt => opt.MapFrom(i => i.UserId.FirstName + " " + i.UserId.LastName))
                .ForMember(m => m.ApprovedDate, opt => opt.NullSubstitute(DateTime.Parse("01-01-0001")))
               // .ForMember(m => m.BDLossCharge, opt => opt.MapFrom(i => i.EnergyCharge.BDLossCharge))
               .ForMember(m => m.LossRate, opt => opt.MapFrom(i => (i.EnergyCharge.LossRate != null ? i.EnergyCharge.LossRate : 0.00M)))
               //.ForMember(m => m.BDMeteredKwh, opt => opt.MapFrom(i => i.EnergyCharge.BDMeteredKwh))
                ;

            Mapper.CreateMap<AspNetUser, CommonInfoViewModel>()
                .ForMember(m => m.FullName, opt => opt.MapFrom(i => i.FirstName + " " + i.LastName))
                .ForMember(m => m.CompanyLogo, opt => opt.MapFrom(i => "/Content/images/" + i.CompanyLogo));
            //    .ForMember(m => m.UserInfo.FullName, opt => opt.MapFrom(i => i.;


            Mapper.CreateMap<InvoiceSummary, InvoiceDetailViewModel>();

        }


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