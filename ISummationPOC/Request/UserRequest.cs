﻿using ISummationPOC.Entity;
using MediatR;

namespace ISummationPOC.Request
{
    public class CreateUserRequest : IRequest<User>
    {
        public User User { get; set; }

    }

    public class UpdateUserRequest : IRequest<User> 
    {
        public User User { get; set; }       
    }

    public class GetUserByIdRequest : IRequest<User>
    {
        public int id { get; set; }
    }

    public class DeleteUserRequest : IRequest<int>
    {
        public int id { get; set; }      
    }
}
