using AutoMapper;
using Blog.DAL.Models;
using Blog.Services.ApiModels.Comments;
using Blog.Services.ApiModels.Posts;
using Blog.Services.ApiModels.Roles;
using Blog.Services.ApiModels.Tags;
using Blog.Services.ApiModels.Users;
using Blog.Services.ViewModels.Comments;
using Blog.Services.ViewModels.Posts;
using Blog.Services.ViewModels.Tags;
using Blog.Services.ViewModels.Users;

namespace Blog.Services
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserRegisterViewModel, User>().ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email));
            CreateMap<UserCreateViewModel, User>().ForMember(x => x.UserName, opt => opt.MapFrom(x => x.Email)).ForMember(x=>x.Roles,opt=>opt.Ignore());

            CreateMap<User, UserViewModel>();
            CreateMap<User, UserEditViewModel>();

            CreateMap<PostAddViewModel, Post>().ForMember(x=>x.Tags,opt =>opt.Ignore());

            CreateMap<Post, PostViewModel>();
            CreateMap<Post, PostEditViewModel>().ForMember(x=>x.Tags,opt=>opt.Ignore());
            CreateMap<TagAddViewModel, Tag>();
            CreateMap<CommentAddViewModel,Comment>();
            CreateMap<CommentEditViewModel, Comment>();
            CreateMap<User, UserApiModel>().ForMember(x => x.Roles, opt => opt.MapFrom(x => x.Roles.Select(r => r.Name)));
            CreateMap<UserCreateApiModel, UserCreateViewModel>().ForMember(x=>x.Password2,opt=>opt.MapFrom(x=>x.Password));
            CreateMap<UserEditApiModel, UserEditViewModel>().ForMember(x => x.Password2, opt => opt.MapFrom(x => x.Password)).ForMember(x=>x.Roles,opt=>opt.Ignore());

            CreateMap<Post, PostApiModel>().ForMember(x => x.Tags, opt => opt.MapFrom(x => x.Tags.Select(x => x.Name)));
            CreateMap<PostViewModel, PostApiModel>().ForMember(x => x.Tags, opt => opt.MapFrom(x => x.Tags.Select(x => x.Name)));

            CreateMap<TagApiModel, TagViewModel>();
            CreateMap<Tag, TagApiModel>().ForMember(x => x.IsChecked, opt => opt.Ignore());

            CreateMap<Comment, CommentApiModel>();

            CreateMap<Role, RoleApiModel>();

        }
    }
}