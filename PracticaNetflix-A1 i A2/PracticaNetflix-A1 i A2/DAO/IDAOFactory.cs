using System;
using System.Collections.Generic;
using System.Text;

namespace PracticaNetflix_A1_i_A2.DAO
{
    public class IDAOFactory
    {
        public IDAO CreateDAO()
        {
            IDAO dao = new DAOCSVImpl();
            return dao;
        }
    }
}
