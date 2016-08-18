namespace OBDSim.Providers
{
    using Models;
    using System;
    using System.Collections.Generic;

    public class OBDSimProvider : IOBDSimProvider
    {
        public List<OBDSimModel> GetOBDSims(int employeeId)
        {
            List<OBDSimModel> l = new List<OBDSimModel>();
            for (int i = 0; i < 10; i++)
            {
                l.Add(new OBDSimModel
                {
                    FromDate = DateTime.Today.AddDays(-i),
                    ToDate = DateTime.Today.AddDays(-i + 2),
                    Reason = "Personal OBDSim"
                });
            }
            return l;
        }
    }
}