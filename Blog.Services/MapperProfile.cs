using AutoMapper;
using Blog.DAL.Models;
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
        }
    }
}