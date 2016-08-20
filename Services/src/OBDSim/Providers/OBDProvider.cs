namespace OBDSim.Providers
{
    using Models;
    using System;
    using System.Collections.Generic;

    public class OBDSimProvider : IOBDSimProvider
    {
        public List<OBDSimModel> GetOBDSims(int customerId)
        {
            //simulating the model
            //ToDo: work on generating the actual model
            List<OBDSimModel> l = new List<OBDSimModel>();
            for (int i = 0; i < 10; i++)
            {
                l.Add(new OBDSimModel
                {
                    FromDate = DateTime.Today.AddDays(-i),
                    ToDate = DateTime.Today.AddDays(-i + 2),
                    Reason = "OBDSim"
                });
            }
            return l;
        }
    }
}
