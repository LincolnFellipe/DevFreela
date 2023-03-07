using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application.Commands.CreateComment
{
    //Quando o método for um void, usamos o Unit no Irequest do mediator
    public class CreateCommentCommand : IRequest<Unit>
    {
        public string Content { get; set; }
        public int IdUser { get; set; }
        public int IdProject { get; set; }
    }
}
