using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WYNK.Data.Model;
using WYNK.Data.Model.ViewModel;
using WYNK.Helpers;

namespace WYNK.Data.Repository.Implementation
{
    public class ProfRepository : RepositoryBase<ProfessionalView>, IProfRepository
    {
        private readonly WYNKContext _Wynkcontext;
        private readonly CMPSContext _Cmpscontext;
        

        public ProfRepository(WYNKContext context, CMPSContext Cmpscontext) : base(context, Cmpscontext)
        {
            _Wynkcontext = context;
            _Cmpscontext = Cmpscontext;

        }

        public dynamic deleteProfes(int? ID)
        {
            var pcmntl = new ProTax();
            
            return new
            {
                Success = false,
                Message = "Some data are Missing"
            };
        }

        public ProfessionalView GetDetails()

        {
            var configuredetail = new ProfessionalView();
            var LocationMaster = CMPSContext.LocationMaster.ToList();

            
            return configuredetail;


        }

        public ProfessionalView getData()

        {
            var arr = new ProfessionalView();            

            return arr;

        }

        public dynamic Gettrans(int ID)
        {

            var detailss = new ProfessionalView();
            var nd = new ProTax();



            return detailss;
        }

        public dynamic InsertPros(ProfessionalView pos)
        {
            

                return new
                {
                    Success = false,
                    Message = "Some data are Missing"

                };
            }

            public dynamic UpdatePart(ProfessionalView De1, int ID, int? pfID)
            {
            

            return new
            {
                Succes = false,
                Message = "Some data are Missing"
            };
        }


    }


    }

 



      