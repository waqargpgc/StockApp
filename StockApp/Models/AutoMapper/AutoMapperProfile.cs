using App.Models.Identity;
using AutoMapper;
using Data.Identity;
using StockApp.Data.Enitities;
using StockApp.Models; 
using System.Linq;

namespace App.Models.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, CustomerModel>().ReverseMap();
            CreateMap<InventoryModel, Inventory>().ReverseMap();
            CreateMap<ProductModel, Product>().ReverseMap();
            CreateMap<InventoryLocationModel, InventoryLocation>().ReverseMap();

            CreateMap<Order, SalesOrderListModel>().ReverseMap();
            CreateMap<Order, SalesOrderModel>().ReverseMap();
            CreateMap<OrderDetail, SaleOrderDetailModel>().ReverseMap();

            CreateMap<RoleListModel, ApplicationRole>().ReverseMap();

            CreateMap<UserModel, ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, UserInfoModel>().ReverseMap();
            CreateMap<UserListModel, ApplicationUser>().ReverseMap();
            CreateMap<UserCreateModel, ApplicationUser>().ReverseMap();

            CreateMap<ProductDetailModel, Product>();
            CreateMap<Product, ProductDetailModel>()
                 .ForMember(dest =>
                        dest.CurrentStockLevel,
                        opt => opt.MapFrom(src => src.Inventories.Sum(x => x.StockLevel)))
                        .ForMember(dest =>
                        dest.StockLocationCount,
                        opt => opt.MapFrom(src => src.Inventories.Count))
                        .ForMember(dest =>
                        dest.CategoryName,
                        opt => opt.MapFrom(src => src.ProductCategory.Name));

            //// Configure automapper for detail View. 
            //CreateMap<Enquiry, EnquiryDetailModel>()
            //    .ForMember(dest =>
            //        dest.EnquiryFollowups,
            //             opt => opt.MapFrom(src =>
            //        src.EnquiryFollowups.ToList()))
            //        .ForMember(dest => dest.EnquiryStudents,
            //            opt => opt.MapFrom(src => src.EnquiryStudents.ToList()))
            //        .ForMember(dest => dest.EnquiryType,
            //        opt => opt.MapFrom(src => src.EnquiryTypes.Type))
            //        .ForMember(dest => dest.EnquirySource,
            //        opt => opt.MapFrom(src => src.EnquirySource.Source))
            //        .ForMember(dest => dest.EnquiryStatus,
            //        opt => opt.MapFrom(src => src.EnquiryStatus.Status));

            //CreateMap<EnquiryFollowUp, EnquiryFollowUpModel>().ReverseMap();

            CreateMap<Product, ProductListModel>()
                .ForMember(dest =>
                        dest.CurrentStockLevel,
                        opt => opt.MapFrom(src => src.Inventories.Sum(x => x.StockLevel)))
                        .ForMember(dest =>
                        dest.StockLocationCount,
                        opt => opt.MapFrom(src => src.Inventories.Count)) 
                        .ForMember(dest =>
                        dest.CategoryName,
                        opt => opt.MapFrom(src => src.ProductCategory.Name));
        }
    }
}
  