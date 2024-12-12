using System.Collections.Generic;
using CarrosAPI.Models;
using System.ServiceModel;

namespace CarrosAPI.SoapServices

{
    [ServiceContract]
    public interface ICarrosSoapService
    {
        [OperationContract]
        List<ModeloCarro> GetModelosPorMarca(string marca);
    }
}
