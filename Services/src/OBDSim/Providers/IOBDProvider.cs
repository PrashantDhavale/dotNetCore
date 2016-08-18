namespace OBDSim.Providers
{
    using Models;
    using System.Collections.Generic;

    public interface IOBDSimProvider
    {
        List<OBDSimModel> GetOBDSims(int employeeId);
    }
}