using AutoMapper;
using CimscoPortal.Data.Models;
using CimscoPortal.Extensions;
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
                                opt => opt.Ignore()) //.MapFrom(i => i.InvoiceDate.ToString()))
                //.ForMember(m => m.InvoiceDate,
                //                opt => opt.MapFrom(i => i.InvoiceDate))
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
                // .ForMember(m => m.GroupCompanyName, opt => opt.MapFrom(i => i.GroupName))
                .ForMember(m => m.TopLevelName, opt => opt.MapFrom(i => i.GroupName))
                .ForMember(m => m.SiteData, opt => opt.MapFrom(i => i.Sites));
            Mapper.CreateMap<Customer, SiteHierarchyViewModel>()
                // .ForMember(m => m.GroupCompanyName, opt => opt.MapFrom(i => i.CustomerName))
                .ForMember(m => m.TopLevelName, opt => opt.MapFrom(i => i.CustomerName))
                .ForMember(m => m.SiteData, opt => opt.MapFrom(i => i.Sites));
            Mapper.CreateMap<Site, SiteHierarchyViewModel>()
                .ForMember(m => m.TopLevelName, opt => opt.MapFrom(i => i.SiteName))
                .ForMember(m => m.SiteData, opt => opt.Ignore());



            Mapper.CreateMap<InvoiceSummary, CompanyInvoiceViewModel>()
                .ForMember(m => m.YearA, opt => opt.MapFrom(i => i.TotalCharges));

            Mapper.CreateMap<InvoiceSummary, InvoiceDetail>()
                .ForMember(m => m.ApproversName, opt => opt.MapFrom(i => i.UserId.FirstName + " " + i.UserId.LastName))
                .ForMember(m => m.ApprovedDate, opt => opt.NullSubstitute(DateTime.Parse("01-01-0001")))
                // .ForMember(m => m.BDLossCharge, opt => opt.MapFrom(i => i.EnergyCharge.BDLossCharge))
                // .ForMember(m => m.LossRate, opt => opt.MapFrom(i => (i.EnergyCharge.LossRate != null ? i.EnergyCharge.LossRate : 0.00M)))
                //.ForMember(m => m.BDMeteredKwh, opt => opt.MapFrom(i => i.EnergyCharge.BDMeteredKwh))
                //.ForMember(m => m.PdfSourceLocation, opt => opt.MapFrom(i => i.SiteId.ToString().ToString().PadRight(6,'0') + "/" + i.InvoiceId.ToString().ToString().PadRight(8,'0') + ".pdf"))
                ;

            Mapper.CreateMap<AspNetUser, CommonInfoViewModel>()
                .ForMember(m => m.FullName, opt => opt.MapFrom(i => i.FirstName + " " + i.LastName))
                .ForMember(m => m.CompanyLogo, opt => opt.MapFrom(i => "/Content/images/" + i.CompanyLogo));
            //    .ForMember(m => m.UserInfo.FullName, opt => opt.MapFrom(i => i.;


            //Mapper.CreateMap<InvoiceSummary, InvoiceDetailViewModel_zz>();
            Mapper.CreateMap<InvoiceSummary, InvoiceOverviewViewModel>()
                .ForMember(m => m.ApproversName, opt => opt.MapFrom(i => (i.UserId.FirstName + " " + i.UserId.LastName) == " " ?
                                                        (i.UserId.UserName) : (i.UserId.FirstName + " " + i.UserId.LastName)))
                .ForMember(m => m.ApprovedDate, opt => opt.NullSubstitute(DateTime.Parse("01-01-0001")));

            Mapper.CreateMap<SiteData, InvoiceTally>();
            Mapper.CreateMap<SiteHierarchyViewModel, GroupCompanyDetail>();
            Mapper.CreateMap<SiteHierarchyViewModel, InvoiceTallyViewModel>()
                .ForMember(m => m.InvoiceTallies, opt => opt.MapFrom(i => i.SiteData))
                .ForMember(m => m.GroupCompanyDetail, opt => opt.MapFrom(s => s))
                //.ForMember(m => m.GroupCompanyDetail.GroupCompanyName, opt => opt.MapFrom(s => s.GroupCompanyName))
                //.ForMember(m => m.GroupCompanyDetail.Address1, opt => opt.MapFrom(s => s.Address1))
                //.ForMember(m => m.GroupCompanyDetail.Address2, opt => opt.MapFrom(s => s.Address2))
                //.ForMember(m => m.GroupCompanyDetail.Address3, opt => opt.MapFrom(s => s.Address3))
                ;

            Mapper.CreateMap<InvoiceSummary, MonthlyConsumptionViewModal>()
                .ForMember(m => m.ConsumptionBusinessDay, opt => opt.MapFrom(r => ((r.EnergyCharge.BD0004R == 0 ? 0 : r.EnergyCharge.BD0004 / r.EnergyCharge.BD0004R))
                                                                                         + ((r.EnergyCharge.BD0408R == 0 ? 0 : r.EnergyCharge.BD0408 / r.EnergyCharge.BD0408R))
                                                                                         + ((r.EnergyCharge.BD0812R == 0 ? 0 : r.EnergyCharge.BD0812 / r.EnergyCharge.BD0812R))
                                                                                         + ((r.EnergyCharge.BD1216R == 0 ? 0 : r.EnergyCharge.BD1216 / r.EnergyCharge.BD1216R))
                                                                                         + ((r.EnergyCharge.BD1620R == 0 ? 0 : r.EnergyCharge.BD1620 / r.EnergyCharge.BD1620R))
                                                                                         + ((r.EnergyCharge.BD2024R == 0 ? 0 : r.EnergyCharge.BD2024 / r.EnergyCharge.BD2024R))))
                .ForMember(m => m.ConsumptionNonBusinessDay, opt => opt.MapFrom(r => ((r.EnergyCharge.NBD0004R == 0 ? 0 : r.EnergyCharge.NBD0004 / r.EnergyCharge.NBD0004R))
                                                                                         + ((r.EnergyCharge.NBD0408R == 0 ? 0 : r.EnergyCharge.NBD0408 / r.EnergyCharge.NBD0408R))
                                                                                         + ((r.EnergyCharge.NBD0812R == 0 ? 0 : r.EnergyCharge.NBD0812 / r.EnergyCharge.NBD0812R))
                                                                                         + ((r.EnergyCharge.NBD1216R == 0 ? 0 : r.EnergyCharge.NBD1216 / r.EnergyCharge.NBD1216R))
                                                                                         + ((r.EnergyCharge.NBD1620R == 0 ? 0 : r.EnergyCharge.NBD1620 / r.EnergyCharge.NBD1620R))
                                                                                         + ((r.EnergyCharge.NBD2024R == 0 ? 0 : r.EnergyCharge.NBD2024 / r.EnergyCharge.NBD2024R))))
                .ForMember(m => m.CostBusinessDay, opt => opt.MapFrom(r => (r.EnergyCharge.BD0004 + r.EnergyCharge.BD0408 +
                                                                                         r.EnergyCharge.BD0812 + r.EnergyCharge.BD1216 +
                                                                                         r.EnergyCharge.BD1620 + r.EnergyCharge.BD2024)))
                .ForMember(m => m.CostNonBusinessDay, opt => opt.MapFrom(r => (r.EnergyCharge.NBD0004 + r.EnergyCharge.NBD0408 +
                                                                                         r.EnergyCharge.NBD0812 + r.EnergyCharge.NBD1216 +
                                                                                         r.EnergyCharge.NBD1620 + r.EnergyCharge.NBD2024)));


            Mapper.CreateMap<EnergyData, AddMonthData>(MemberList.Source)
                .ForMember(m => m.monthCount, opt => opt.Ignore());

            Mapper.CreateMap<Division, FilterItem>()
                //  .ForMember(m => m.Index, opt => opt.Ignore())
                .ForMember(m => m.Label, opt => opt.MapFrom(s => s.DivisionName))
                .ForMember(m => m.Id, opt => opt.MapFrom(s => s.DivisionId))
                ;

            Mapper.CreateMap<IndustryClassification, FilterItem>()
                //  .ForMember(m => m.Index, opt => opt.Ignore())
                .ForMember(m => m.Label, opt => opt.MapFrom(s => s.IndustryDescription))
                .ForMember(m => m.Id, opt => opt.MapFrom(s => s.IndustryId))
                ;
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