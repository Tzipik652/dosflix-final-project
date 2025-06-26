using AutoMapper;
using BL.Models;
using DAL.Api;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL.Profiles
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {


            #region AgeGroup

            CreateMap<AgeGroup, BLAgeGroup>()
    .ForMember(dest => dest.AgeCode, opt => opt.MapFrom(src => src.AgeCode))
    .ForMember(dest => dest.AgeDescrepition, opt => opt.MapFrom(src => src.AgeDescription))
    .ForMember(dest => dest.Movies, opt => opt.Ignore()) // כדי למנוע לולאות רקורסיביות
    .ForMember(dest => dest.Users, opt => opt.Ignore()); // כנ"ל

            CreateMap<BLAgeGroup, AgeGroup>()
    .ForMember(dest => dest.Movies, opt => opt.Ignore())
    .ForMember(dest => dest.Customers, opt => opt.Ignore());




            #endregion

            #region Category

            CreateMap<Category, BLCategory>()
    .ForMember(dest => dest.CategoryCode, opt => opt.MapFrom(src => src.CategoryCode))
    .ForMember(dest => dest.CategoryDescreption, opt => opt.MapFrom(src => src.CategoryDescription))
    .ForMember(dest => dest.Movies, opt => opt.Ignore()); // להימנע מרקורסיה

            CreateMap<BLCategory, Category>()
               .ForMember(dest => dest.Movies, opt => opt.Ignore());

            #endregion

            #region User+Customer to BLUser

            CreateMap<User, BLUser>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.FullName : null))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.Phone : null))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Customer != null && src.Customer.Gender == "Female" ? false : true))
                .ForMember(dest => dest.AgeGroup, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.AgeGroup : null))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.Customer != null ? src.Customer.ProfilePicture : null))
                .ForMember(dest => dest.EmailLinks, opt => opt.Ignore()) // אפשר להשלים לפי הצורך
                .ForMember(dest => dest.Orders, opt => opt.Ignore());

            CreateMap<BLUser, User>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
               .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
               .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (int)src.Role))
               .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src)) // ✨ שימו לב לזה – נשתמש במיפוי הקיים של BLUser → Customer
               ;

            CreateMap<BLUser, Customer>()
             .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
             .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
             .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender ? "Male" : "Female"))
             .ForMember(dest => dest.AgeGroup, opt => opt.MapFrom(src => (int?)src.AgeGroup))
             .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture))
             .ForMember(dest => dest.User, opt => opt.Ignore())
             .ForMember(dest => dest.AgeGroupNavigation, opt => opt.Ignore())
             .ForMember(dest => dest.EmailLinks, opt => opt.Ignore())
             .ForMember(dest => dest.Orders, opt => opt.Ignore());


            #endregion

            #region Order

            CreateMap<Order, BLOrder>()
    .ForMember(dest => dest.Status,
               opt => opt.MapFrom(src => src.Status)) // בלי enum
    .ForMember(dest => dest.OrderItems, opt => opt.Ignore()) // אם צריך
    .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.IdCustomer))
    .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.DateOrder))
    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdOrder))
    .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
    .ForMember(dest => dest.TotalAmount,
               opt => opt.MapFrom(src => src.TotalAmount));

         
            CreateMap<BLOrder, Order>()
          .ForMember(dest => dest.Status,
                     opt => opt.MapFrom(src => src.Status)) // bool פשוט
          .ForMember(dest => dest.IdCustomer, opt => opt.MapFrom(src => src.CustomerId))
          .ForMember(dest => dest.DateOrder, opt => opt.MapFrom(src => src.OrderDate))
          .ForMember(dest => dest.IdOrder, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
          .ForMember(dest => dest.TotalAmount, opt => opt.Ignore()); // מחושב בדאטהבייס


            #endregion

            #region OrderItem

            CreateMap<OrderItem, BLOrderItem>()
       .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
       .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
       .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
       .ForMember(dest => dest.ViewerCount, opt => opt.MapFrom(src => src.ViewerCount))
       .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.ViewCount))
       .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal))
       .ForMember(dest => dest.Movie, opt => opt.MapFrom(src => src.Movie))
       .ForMember(dest => dest.LinkForMovie, opt => opt.MapFrom(src => src.LinkForMovie));

            CreateMap<BLOrderItem, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.ViewerCount, opt => opt.MapFrom(src => src.ViewerCount))
                .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.ViewCount))
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal))
                .ForMember(dest => dest.Movie, opt => opt.Ignore())
                .ForMember(dest => dest.LinkForMovie, opt => opt.MapFrom(src => src.LinkForMovie))
                .ForMember(dest => dest.Order, opt => opt.Ignore());


            #endregion

            #region EmailLink
            CreateMap<EmailLink, BLEmailLink>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LinkId))
                 .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.UserId))
                 .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.UniqueToken))
                 .ForMember(dest => dest.SentAt, opt => opt.MapFrom(src => src.DateCreated));

            CreateMap<BLEmailLink, EmailLink>()
                .ForMember(dest => dest.LinkId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.UniqueToken, opt => opt.MapFrom(src => src.Link))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.SentAt));
            #endregion


        }
    }
}
