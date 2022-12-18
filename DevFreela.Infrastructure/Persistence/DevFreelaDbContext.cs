using DevFreela.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Infrastructure.Persistence
{
    public class DevFreelaDbContext
    {
        public DevFreelaDbContext()
        {
            Projects = new List<Project>
            {
             new Project ("Meu projeto AspNet.Core 1", "Descrição do projeto Aspnet.Core 1",1,1,1500),
             new Project ("Meu projeto AspNet.Core 2", "Descrição do projeto Aspnet.Core 2",1,1,2500),
             new Project ("Meu projeto AspNet.Core 3", "Descrição do projeto Aspnet.Core 3",1,1,3500)
            };
            Users = new List<User> 
            { 
            new User("Lincoln","lincolnfellipe@hotmail.com",new DateTime(2000,07,23)),
            new User("Fellipe","fellipe@hotmail.com",new DateTime(2000,07,24)),
            new User("Souza","souza@hotmail.com",new DateTime(2000,07,25)),
            };
            Skills = new List<Skill> 
            { 
            new Skill(".NET CORE"),
            new Skill("REACT"),
            new Skill("SQL SERVER")
            };
        }
        public List<Project> Projects { get; set; }
        public List<User> Users { get; set; }   
        public List<Skill> Skills { get; set; }
    }
}
