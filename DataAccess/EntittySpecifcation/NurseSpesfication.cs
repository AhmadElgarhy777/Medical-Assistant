using DataAccess.Specfications;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntittySpecifcation
{
    public class NurseSpesfication:Specfication<Nures>
    {
        public NurseSpesfication():base()
        {
            Includes.Add(e => e.Ratings);
        }
    }
}
